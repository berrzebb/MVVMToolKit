using System.Collections;
using System.Runtime.InteropServices;
using System.Windows.Interop;
using MVVMToolKit.Helper.Native;

namespace MVVMToolKit.Helper.ScreenHelper
{
    /// <summary>
    /// Represents a display device or multiple display devices on a single system.
    /// </summary>
    public class Screen
    {
        // References:
        // http://referencesource.microsoft.com/#System.Windows.Forms/ndp/fx/src/winforms/Managed/System/WinForms/Screen.cs
        // http://msdn.microsoft.com/en-us/library/windows/desktop/dd145072.aspx
        // http://msdn.microsoft.com/en-us/library/windows/desktop/dd183314.aspx

        /// <summary>
        /// Indicates if we have more than one monitor.
        /// </summary>
        private static readonly bool s_multiMonitorSupport;

        // This identifier is just for us, so that we don't try to call the multimon
        // functions if we just need the primary monitor... this is safer for
        // non-multimon OSes.
        /// <summary>
        /// The primary monitor
        /// </summary>
        private const int PrimaryMonitor = unchecked((int)0xBAADF00D);

        /// <summary>
        /// The monitor info primary.
        /// </summary>
        private const int MonitorInfofPrimary = 0x00000001;

        /// <summary>
        /// The monitor handle.
        /// </summary>
        private readonly IntPtr _monitorHandle;

        /// <summary>
        /// Initializes static members of the <see cref="Screen"/> class.
        /// </summary>
        static Screen() => s_multiMonitorSupport = NativeMethods.GetSystemMetrics(NativeMethods.SystemMetric.SM_CMONITORS) != 0;

        /// <summary>
        /// Initializes a new instance of the <see cref="Screen"/> class.
        /// </summary>
        /// <param name="monitor">The monitor.</param>
        private Screen(IntPtr monitor)
            : this(monitor, IntPtr.Zero)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Screen"/> class.
        /// </summary>
        /// <param name="monitor">The monitor.</param>
        /// <param name="hdc">The hdc.</param>
        private Screen(IntPtr monitor, IntPtr hdc)
        {
            if (NativeMethods.IsProcessDPIAware())
            {
                uint dpiX;

                try
                {
                    if (monitor == (IntPtr)PrimaryMonitor)
                    {
                        var ptr = NativeMethods.MonitorFromPoint(new NativeMethods.POINTSTRUCT(0, 0), NativeMethods.MonitorDefault.MONITOR_DEFAULTTOPRIMARY);
                        NativeMethods.GetDpiForMonitor(ptr, NativeMethods.DpiType.EFFECTIVE, out dpiX, out _);
                    }
                    else
                    {
                        NativeMethods.GetDpiForMonitor(monitor, NativeMethods.DpiType.EFFECTIVE, out dpiX, out _);
                    }
                }
                catch
                {
                    // Windows 7 fallback
                    var hr = NativeMethods.D2D1CreateFactory(NativeMethods.D2D1_FACTORY_TYPE.D2D1_FACTORY_TYPE_SINGLE_THREADED, typeof(NativeMethods.ID2D1Factory).GUID, IntPtr.Zero, out var factory);
                    if (hr < 0)
                    {
                        dpiX = 96;
                    }
                    else
                    {
                        factory.GetDesktopDpi(out var x, out _);
                        Marshal.ReleaseComObject(factory);
                        dpiX = (uint)x;
                    }
                }

                ScaleFactor = dpiX / 96.0;
            }

            if (!s_multiMonitorSupport || monitor == (IntPtr)PrimaryMonitor)
            {
                var size = new Size(
                    NativeMethods.GetSystemMetrics(NativeMethods.SystemMetric.SM_CXSCREEN),
                    NativeMethods.GetSystemMetrics(NativeMethods.SystemMetric.SM_CYSCREEN));

                Bounds = new Rect(0, 0, size.Width, size.Height);
                Primary = true;
                DeviceName = "DISPLAY";
            }
            else
            {
                var info = new NativeMethods.MONITORINFOEX();

                NativeMethods.GetMonitorInfo(new HandleRef(null, monitor), info);

                Bounds = new Rect(
                    info.rcMonitor.left,
                    info.rcMonitor.top,
                    info.rcMonitor.right - info.rcMonitor.left,
                    info.rcMonitor.bottom - info.rcMonitor.top);
                Primary = (info.dwFlags & MonitorInfofPrimary) != 0;
                DeviceName = new string(info.szDevice).TrimEnd((char)0);
            }

            _monitorHandle = monitor;
        }

        /// <summary>
        /// Gets an array of all displays on the system.
        /// </summary>
        /// <returns>An enumerable of type Screen, containing all displays on the system.</returns>
        public static IEnumerable<Screen?> AllScreens
        {
            get
            {
                if (s_multiMonitorSupport)
                {
                    var closure = new MonitorEnumCallback();
                    var proc = new NativeMethods.MonitorEnumProc(closure.Callback);
                    NativeMethods.EnumDisplayMonitors(NativeMethods.NullHandleRef, null, proc, IntPtr.Zero);
                    if (closure.Screens.Count > 0)
                    {
                        return closure.Screens.Cast<Screen>();
                    }
                }

                return new Screen?[] { new((IntPtr)PrimaryMonitor) };
            }
        }

        /// <summary>
        /// Gets the primary display.
        /// </summary>
        /// <returns>The primary display.</returns>
        public static Screen? PrimaryScreen
        {
            get
            {
                return s_multiMonitorSupport ? AllScreens.FirstOrDefault(t => t != null && t.Primary) : new Screen((IntPtr)PrimaryMonitor);
            }
        }

        /// <summary>
        /// Gets the bounds of the display in units.
        /// </summary>
        /// <returns>A <see cref="T:System.Windows.Rect" />, representing the bounds of the display in units.</returns>
        public Rect WpfBounds =>
            ScaleFactor.Equals(1.0)
                ? Bounds
                : new Rect(
                    Bounds.X / ScaleFactor,
                    Bounds.Y / ScaleFactor,
                    Bounds.Width / ScaleFactor,
                    Bounds.Height / ScaleFactor);

        /// <summary>
        /// Gets the device name associated with a display.
        /// </summary>
        /// <returns>The device name associated with a display.</returns>
        public string DeviceName { get; }

        /// <summary>
        /// Gets the bounds of the display in pixels.
        /// </summary>
        /// <returns>A <see cref="T:System.Windows.Rect" />, representing the bounds of the display in pixels.</returns>
        public Rect Bounds { get; }

        /// <summary>
        /// Gets a value indicating whether a particular display is the primary device.
        /// </summary>
        /// <returns>true if this display is primary; otherwise, false.</returns>
        public bool Primary { get; }

        /// <summary>
        /// Gets the scale factor of the display.
        /// </summary>
        /// <returns>The scale factor of the display.</returns>
        public double ScaleFactor { get; } = 1.0;

        /// <summary>
        /// Gets the working area of the display. The working area is the desktop area of the display, excluding task bars,
        /// docked windows, and docked tool bars in pixels.
        /// </summary>
        /// <returns>A <see cref="T:System.Windows.Rect" />, representing the working area of the display in pixels.</returns>
        public Rect WorkingArea
        {
            get
            {
                Rect workingArea;

                if (!s_multiMonitorSupport || _monitorHandle == (IntPtr)PrimaryMonitor)
                {
                    var rc = new NativeMethods.RECT();

                    NativeMethods.SystemParametersInfo(NativeMethods.SPI.SPI_GETWORKAREA, 0, ref rc, NativeMethods.SPIF.SPIF_SENDCHANGE);

                    workingArea = new Rect(rc.left, rc.top, rc.right - rc.left, rc.bottom - rc.top);
                }
                else
                {
                    var info = new NativeMethods.MONITORINFOEX();
                    NativeMethods.GetMonitorInfo(new HandleRef(null, _monitorHandle), info);

                    workingArea = new Rect(info.rcWork.left, info.rcWork.top, info.rcWork.right - info.rcWork.left, info.rcWork.bottom - info.rcWork.top);
                }

                return workingArea;
            }
        }

        /// <summary>
        /// Gets the working area of the display. The working area is the desktop area of the display, excluding task bars,
        /// docked windows, and docked tool bars in units.
        /// </summary>
        /// <returns>A <see cref="T:System.Windows.Rect" />, representing the working area of the display in units.</returns>
        public Rect WpfWorkingArea =>
            ScaleFactor.Equals(1.0)
                ? WorkingArea
                : new Rect(
                    WorkingArea.X / ScaleFactor,
                    WorkingArea.Y / ScaleFactor,
                    WorkingArea.Width / ScaleFactor,
                    WorkingArea.Height / ScaleFactor);

        /// <summary>
        /// Retrieves a Screen for the display that contains the largest portion of the specified control.
        /// </summary>
        /// <param name="hwnd">The window handle for which to retrieve the Screen.</param>
        /// <returns>
        /// A Screen for the display that contains the largest region of the object. In multiple display environments
        /// where no display contains any portion of the specified window, the display closest to the object is returned.
        /// </returns>
        public static Screen FromHandle(IntPtr hwnd)
        {
            return s_multiMonitorSupport
                       ? new Screen(NativeMethods.MonitorFromWindow(new HandleRef(null, hwnd), 2))
                       : new Screen((IntPtr)PrimaryMonitor);
        }

        /// <summary>
        /// Retrieves a Screen for the display that contains the specified point in pixels.
        /// </summary>
        /// <param name="point">A <see cref="T:System.Windows.Point" /> that specifies the location for which to retrieve a Screen.</param>
        /// <returns>
        /// A Screen for the display that contains the point in pixels. In multiple display environments where no display contains
        /// the point, the display closest to the specified point is returned.
        /// </returns>
        public static Screen FromPoint(Point point)
        {
            if (s_multiMonitorSupport)
            {
                var pt = new NativeMethods.POINTSTRUCT((int)point.X, (int)point.Y);
                return new Screen(NativeMethods.MonitorFromPoint(pt, NativeMethods.MonitorDefault.MONITOR_DEFAULTTONEAREST));
            }

            return new Screen((IntPtr)PrimaryMonitor);
        }

        /// <summary>
        /// Retrieves a Screen for the display that contains the largest portion of the specified control.
        /// </summary>
        /// <param name="window">The window for which to retrieve the Screen.</param>
        /// <returns>
        /// A Screen for the display that contains the largest region of the object. In multiple display environments
        /// where no display contains any portion of the specified window, the display closest to the object is returned.
        /// </returns>
        public static Screen FromWindow(Window window)
        {
            return FromHandle(new WindowInteropHelper(window).Handle);
        }

        /// <summary>
        /// Gets or sets a value indicating whether the specified object is equal to this Screen.
        /// </summary>
        /// <param name="obj">The object to compare to this Screen.</param>
        /// <returns>true if the specified object is equal to this Screen; otherwise, false.</returns>
        public override bool Equals(object? obj)
        {
            if (obj is Screen monitor)
            {
                if (_monitorHandle == monitor._monitorHandle)
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Computes and retrieves a hash code for an object.
        /// </summary>
        /// <returns>A hash code for an object.</returns>
        public override int GetHashCode()
        {
            return _monitorHandle.GetHashCode();
        }

        /// <summary>
        /// The monitor enum callback.
        /// </summary>
        private sealed class MonitorEnumCallback
        {
            /// <summary>
            /// Initializes a new instance of the <see cref="MonitorEnumCallback"/> class.
            /// </summary>
            public MonitorEnumCallback()
            {
                Screens = new ArrayList();
            }

            /// <summary>
            /// Gets the screens.
            /// </summary>
            public ArrayList Screens { get; }

            /// <summary>
            /// Describes whether this instance callback
            /// </summary>
            /// <param name="monitor">The monitor</param>
            /// <param name="hdc">The hdc</param>
            /// <param name="lprcMonitor">The lprc monitor</param>
            /// <param name="lparam">The lparam</param>
            /// <returns>The bool</returns>
            public bool Callback(IntPtr monitor, IntPtr hdc, IntPtr lprcMonitor, IntPtr lparam)
            {
                Screens.Add(new Screen(monitor, hdc));
                return true;
            }
        }
    }
}
