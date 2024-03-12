using System.Runtime.InteropServices;
using System.Runtime.Versioning;

namespace MVVMToolKit.Helper.Native
{
    /// <summary>
    /// 외부 DLL 파일의 함수를 호출하기 위한 메서드와 열거형을 정의하는 클래스입니다.<br/>
    /// </summary>
    public static class NativeMethods
    {
        /// <summary>
        /// The monitor enum proc
        /// </summary>
        public delegate bool MonitorEnumProc(IntPtr monitor, IntPtr hdc, IntPtr lprcMonitor, IntPtr lParam);

        /// <summary>
        /// The dpi type enum
        /// </summary>
        public enum DpiType
        {
            /// <summary>
            /// The effective dpi type
            /// </summary>
            EFFECTIVE = 0,
            /// <summary>
            /// The angular dpi type
            /// </summary>
            ANGULAR = 1,
            /// <summary>
            /// The raw dpi type
            /// </summary>
            RAW = 2
        }

        /// <summary>
        /// The system metric enum
        /// </summary>
        public enum SystemMetric
        {
            /// <summary>
            /// The sm cxscreen system metric
            /// </summary>
            SM_CXSCREEN = 0,
            /// <summary>
            /// The sm cyscreen system metric
            /// </summary>
            SM_CYSCREEN = 1,
            /// <summary>
            /// The sm xvirtualscreen system metric
            /// </summary>
            SM_XVIRTUALSCREEN = 76,
            /// <summary>
            /// The sm yvirtualscreen system metric
            /// </summary>
            SM_YVIRTUALSCREEN = 77,
            /// <summary>
            /// The sm cxvirtualscreen system metric
            /// </summary>
            SM_CXVIRTUALSCREEN = 78,
            /// <summary>
            /// The sm cyvirtualscreen system metric
            /// </summary>
            SM_CYVIRTUALSCREEN = 79,
            /// <summary>
            /// The sm cmonitors system metric
            /// </summary>
            SM_CMONITORS = 80
        }

        /// <summary>
        /// The spi enum
        /// </summary>
        public enum SPI : uint
        {
            /// <summary>
            /// Retrieves the size of the work area on the primary display monitor. The work area is the portion of the screen not obscured
            /// by the system taskbar or by application desktop toolbars. The pvParam parameter must point to a RECT structure that receives
            /// the coordinates of the work area, expressed in virtual screen coordinates.
            /// To get the work area of a monitor other than the primary display monitor, call the GetMonitorInfo function.
            /// </summary>
            SPI_GETWORKAREA = 0x0030
        }

        /// <summary>
        /// The spif enum
        /// </summary>
        [Flags]
        public enum SPIF
        {
            /// <summary>
            /// The none spif
            /// </summary>
            None = 0x00,

            /// <summary>Writes the new system-wide parameter setting to the user profile.</summary>
            SPIF_UPDATEINIFILE = 0x01,

            /// <summary>Broadcasts the WM_SETTINGCHANGE message after updating the user profile.</summary>
            SPIF_SENDCHANGE = 0x02,

            /// <summary>Same as SPIF_SENDCHANGE.</summary>
            SPIF_SENDWININICHANGE = 0x02
        }

        /// <summary>
        /// The monitor default enum
        /// </summary>
        public enum MonitorDefault
        {
            /// <summary>If the point is not contained within any display monitor, return a handle to the display monitor that is nearest to the point.</summary>
            MONITOR_DEFAULTTONEAREST = 0x00000002,

            /// <summary>If the point is not contained within any display monitor, return NULL.</summary>
            MONITOR_DEFAULTTONULL = 0x00000000,

            /// <summary>If the point is not contained within any display monitor, return a handle to the primary display monitor.</summary>
            MONITOR_DEFAULTTOPRIMARY = 0x00000001
        }

        /// <summary>
        /// The d2d1 factory type enum
        /// </summary>
        public enum D2D1_FACTORY_TYPE
        {
            /// <summary>
            /// The d2d1 factory type single threaded d2d1 factory type
            /// </summary>
            D2D1_FACTORY_TYPE_SINGLE_THREADED = 0,
            /// <summary>
            /// The d2d1 factory type multi threaded d2d1 factory type
            /// </summary>
            D2D1_FACTORY_TYPE_MULTI_THREADED = 1,
        }
        /// <summary>Enumeration of the different ways of showing a window using 
        /// ShowWindow</summary>
        public enum WindowShowStyle : uint
        {
            /// <summary>Hides the window and activates another window.</summary>
            /// <remarks>See SW_HIDE</remarks>
            Hide = 0,

            /// <summary>Activates and displays a window. If the window is minimized 
            /// or maximized, the system restores it to its original size and 
            /// position. An application should specify this flag when displaying 
            /// the window for the first time.</summary>
            /// <remarks>See SW_SHOWNORMAL</remarks>
            ShowNormal = 1,

            /// <summary>Activates the window and displays it as a minimized window.</summary>
            /// <remarks>See SW_SHOWMINIMIZED</remarks>
            ShowMinimized = 2,

            /// <summary>Activates the window and displays it as a maximized window.</summary>
            /// <remarks>See SW_SHOWMAXIMIZED</remarks>
            ShowMaximized = 3,

            /// <summary>Maximizes the specified window.</summary>
            /// <remarks>See SW_MAXIMIZE</remarks>
            Maximize = 3,

            /// <summary>Displays a window in its most recent size and position. 
            /// This value is similar to "ShowNormal", except the window is not 
            /// actived.</summary>
            /// <remarks>See SW_SHOWNOACTIVATE</remarks>
            ShowNormalNoActivate = 4,

            /// <summary>Activates the window and displays it in its current size 
            /// and position.</summary>
            /// <remarks>See SW_SHOW</remarks>
            Show = 5,

            /// <summary>Minimizes the specified window and activates the next 
            /// top-level window in the Z order.</summary>
            /// <remarks>See SW_MINIMIZE</remarks>
            Minimize = 6,

            /// <summary>Displays the window as a minimized window. This value is 
            /// similar to "ShowMinimized", except the window is not activated.</summary>
            /// <remarks>See SW_SHOWMINNOACTIVE</remarks>
            ShowMinNoActivate = 7,

            /// <summary>Displays the window in its current size and position. This 
            /// value is similar to "Show", except the window is not activated.</summary>
            /// <remarks>See SW_SHOWNA</remarks>
            ShowNoActivate = 8,

            /// <summary>Activates and displays the window. If the window is 
            /// minimized or maximized, the system restores it to its original size 
            /// and position. An application should specify this flag when restoring 
            /// a minimized window.</summary>
            /// <remarks>See SW_RESTORE</remarks>
            Restore = 9,

            /// <summary>Sets the show state based on the SW_ value specified in the 
            /// STARTUPINFO structure passed to the CreateProcess function by the 
            /// program that started the application.</summary>
            /// <remarks>See SW_SHOWDEFAULT</remarks>
            ShowDefault = 10,

            /// <summary>Windows 2000/XP: Minimizes a window, even if the thread 
            /// that owns the window is hung. This flag should only be used when 
            /// minimizing windows from a different thread.</summary>
            /// <remarks>See SW_FORCEMINIMIZE</remarks>
            ForceMinimized = 11
        }
        /// <summary>
        /// The zero
        /// </summary>
        public static readonly HandleRef NullHandleRef = new(null, IntPtr.Zero);

        /// <summary>
        /// Gets the dpi for monitor using the specified hmonitor
        /// </summary>
        /// <param name="hmonitor">The hmonitor</param>
        /// <param name="dpiType">The dpi type</param>
        /// <param name="dpiX">The dpi</param>
        /// <param name="dpiY">The dpi</param>
        /// <returns>The int ptr</returns>
        [DllImport(ExternDll.Shcore, CharSet = CharSet.Auto)]
        [ResourceExposure(ResourceScope.None)]
        public static extern IntPtr GetDpiForMonitor([In] IntPtr hmonitor, [In] DpiType dpiType, [Out] out uint dpiX, [Out] out uint dpiY);

        /// <summary>
        /// Describes whether get monitor info
        /// </summary>
        /// <param name="hmonitor">The hmonitor</param>
        /// <param name="info">The info</param>
        /// <returns>The bool</returns>
        [DllImport(ExternDll.User32, CharSet = CharSet.Auto)]
        [ResourceExposure(ResourceScope.None)]
        public static extern bool GetMonitorInfo(HandleRef hmonitor, [In][Out] MONITORINFOEX info);

        /// <summary>
        /// Describes whether enum display monitors
        /// </summary>
        /// <param name="hdc">The hdc</param>
        /// <param name="rcClip">The rc clip</param>
        /// <param name="lpfnEnum">The lpfn enum</param>
        /// <param name="dwData">The dw data</param>
        /// <returns>The bool</returns>
        [DllImport(ExternDll.User32, ExactSpelling = true)]
        [ResourceExposure(ResourceScope.None)]
        public static extern bool EnumDisplayMonitors(HandleRef hdc, COMRECT? rcClip, MonitorEnumProc lpfnEnum, IntPtr dwData);

        /// <summary>
        /// Monitors the from window using the specified handle
        /// </summary>
        /// <param name="handle">The handle</param>
        /// <param name="flags">The flags</param>
        /// <returns>The int ptr</returns>
        [DllImport(ExternDll.User32, ExactSpelling = true)]
        [ResourceExposure(ResourceScope.None)]
        public static extern IntPtr MonitorFromWindow(HandleRef handle, int flags);

        /// <summary>
        /// Gets the system metrics using the specified n index
        /// </summary>
        /// <param name="nIndex">The index</param>
        /// <returns>The int</returns>
        [DllImport(ExternDll.User32, ExactSpelling = true, CharSet = CharSet.Auto)]
        [ResourceExposure(ResourceScope.None)]
        public static extern int GetSystemMetrics(SystemMetric nIndex);

        /// <summary>
        /// Describes whether system parameters info
        /// </summary>
        /// <param name="nAction">The action</param>
        /// <param name="nParam">The param</param>
        /// <param name="rc">The rc</param>
        /// <param name="nUpdate">The update</param>
        /// <returns>The bool</returns>
        [DllImport(ExternDll.User32, CharSet = CharSet.Auto)]
        [ResourceExposure(ResourceScope.None)]
        public static extern bool SystemParametersInfo(SPI nAction, int nParam, ref RECT rc, SPIF nUpdate);

        /// <summary>
        /// Monitors the from point using the specified pt
        /// </summary>
        /// <param name="pt">The pt</param>
        /// <param name="flags">The flags</param>
        /// <returns>The int ptr</returns>
        [DllImport(ExternDll.User32, ExactSpelling = true)]
        [ResourceExposure(ResourceScope.None)]
        public static extern IntPtr MonitorFromPoint(POINTSTRUCT pt, MonitorDefault flags);

        /// <summary>
        /// Describes whether get cursor pos
        /// </summary>
        /// <param name="pt">The pt</param>
        /// <returns>The bool</returns>
        [DllImport(ExternDll.User32, ExactSpelling = true, CharSet = CharSet.Auto)]
        [ResourceExposure(ResourceScope.None)]
        public static extern bool GetCursorPos([In][Out] POINT pt);

        /// <summary>
        /// Describes whether is process dpi aware
        /// </summary>
        /// <returns>The bool</returns>
        [DllImport(ExternDll.User32, SetLastError = true)]
        public static extern bool IsProcessDPIAware();

        /// <summary>
        /// Describes whether move window
        /// </summary>
        /// <param name="hWnd">The wnd</param>
        /// <param name="X">The </param>
        /// <param name="Y">The </param>
        /// <param name="nWidth">The width</param>
        /// <param name="nHeight">The height</param>
        /// <param name="bRepaint">The repaint</param>
        /// <returns>The bool</returns>
        [DllImport(ExternDll.User32, SetLastError = true)]
        public static extern bool MoveWindow(IntPtr hWnd, int X, int Y, int nWidth, int nHeight, bool bRepaint);

        /// <summary>
        /// Ds the 2 d 1 create factory using the specified factory type
        /// </summary>
        /// <param name="factoryType">The factory type</param>
        /// <param name="riid">The riid</param>
        /// <param name="pFactoryOptions">The factory options</param>
        /// <param name="ppIFactory">The pp factory</param>
        /// <returns>The int</returns>
        [DllImport(ExternDll.D2D1)]
        public static extern int D2D1CreateFactory(D2D1_FACTORY_TYPE factoryType, [MarshalAs(UnmanagedType.LPStruct)] Guid riid, IntPtr pFactoryOptions, out ID2D1Factory ppIFactory);


        /// <summary>
        /// Finds the window using the specified class name
        /// </summary>
        /// <param name="className">The class name</param>
        /// <param name="windowName">The window name</param>
        /// <returns>The int ptr</returns>
        [DllImport(ExternDll.User32)]
        public static extern IntPtr FindWindow(string className, string windowName);

        //Import the SetForeground API to activate it
        /// <summary>
        /// Sets the foreground window using the specified h wnd
        /// </summary>
        /// <param name="hWnd">The wnd</param>
        /// <returns>The int ptr</returns>
        [DllImport(ExternDll.User32)]
        public static extern IntPtr SetForegroundWindow(IntPtr hWnd);

        /// <summary>
        /// Shows the window using the specified hwnd
        /// </summary>
        /// <param name="hwnd">The hwnd</param>
        /// <param name="iCmdShow">The cmd show</param>
        /// <returns>The int</returns>
        [DllImport(ExternDll.User32)]
        public static extern int ShowWindow(IntPtr hwnd, int iCmdShow);

        /// <summary>
        /// Describes whether is iconic
        /// </summary>
        /// <param name="hwnd">The hwnd</param>
        /// <returns>The bool</returns>
        [DllImport(ExternDll.User32)]
        public static extern bool IsIconic(IntPtr hwnd);


        /// <summary>
        /// The rect
        /// </summary>
        [StructLayout(LayoutKind.Sequential)]
        public struct RECT
        {
            /// <summary>
            /// The left
            /// </summary>
            public int left;
            /// <summary>
            /// The top
            /// </summary>
            public int top;
            /// <summary>
            /// The right
            /// </summary>
            public int right;
            /// <summary>
            /// The bottom
            /// </summary>
            public int bottom;

            /// <summary>
            /// Initializes a new instance of the <see cref="RECT"/> class
            /// </summary>
            /// <param name="left">The left</param>
            /// <param name="top">The top</param>
            /// <param name="right">The right</param>
            /// <param name="bottom">The bottom</param>
            public RECT(int left, int top, int right, int bottom)
            {
                this.left = left;
                this.top = top;
                this.right = right;
                this.bottom = bottom;
            }

            /// <summary>
            /// Initializes a new instance of the <see cref="RECT"/> class
            /// </summary>
            /// <param name="r">The </param>
            public RECT(Rect r)
            {
                left = (int)r.Left;
                top = (int)r.Top;
                right = (int)r.Right;
                bottom = (int)r.Bottom;
            }

            /// <summary>
            /// Creates the xywh using the specified x
            /// </summary>
            /// <param name="x">The </param>
            /// <param name="y">The </param>
            /// <param name="width">The width</param>
            /// <param name="height">The height</param>
            /// <returns>The rect</returns>
            public static RECT FromXYWH(int x, int y, int width, int height)
            {
                return new RECT(x, y, x + width, y + height);
            }

            /// <summary>
            /// Gets the value of the size
            /// </summary>
            public Size Size => new(right - left, bottom - top);
        }

        // use this in cases where the Native API takes a POINT not a POINT*
        // classes marshal by ref.
        /// <summary>
        /// The pointstruct
        /// </summary>
        [StructLayout(LayoutKind.Sequential)]
        public struct POINTSTRUCT
        {
            /// <summary>
            /// The 
            /// </summary>
            public int x;
            /// <summary>
            /// The 
            /// </summary>
            public int y;

            /// <summary>
            /// Initializes a new instance of the <see cref="POINTSTRUCT"/> class
            /// </summary>
            /// <param name="x">The </param>
            /// <param name="y">The </param>
            public POINTSTRUCT(int x, int y)
            {
                this.x = x;
                this.y = y;
            }
        }

        /// <summary>
        /// The point class
        /// </summary>
        [StructLayout(LayoutKind.Sequential)]
        public class POINT
        {
            /// <summary>
            /// The 
            /// </summary>
            public int x;
            /// <summary>
            /// The 
            /// </summary>
            public int y;

            /// <summary>
            /// Initializes a new instance of the <see cref="POINT"/> class
            /// </summary>
            public POINT()
            {
            }

            /// <summary>
            /// Initializes a new instance of the <see cref="POINT"/> class
            /// </summary>
            /// <param name="x">The </param>
            /// <param name="y">The </param>
            public POINT(int x, int y)
            {
                this.x = x;
                this.y = y;
            }

#if DEBUG

            public override string ToString()
            {
                return "{x=" + x + ", y=" + y + "}";
            }

#endif
        }

        /// <summary>
        /// The monitorinfoex class
        /// </summary>
        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto, Pack = 4)]
        public class MONITORINFOEX
        {
            /// <summary>
            /// The monitorinfoex
            /// </summary>
            internal int cbSize = Marshal.SizeOf(typeof(MONITORINFOEX));

            /// <summary>
            /// The rect
            /// </summary>
            internal RECT rcMonitor = new();
            /// <summary>
            /// The rect
            /// </summary>
            internal RECT rcWork = new();
            /// <summary>
            /// The dw flags
            /// </summary>
            internal int dwFlags = 0;

            /// <summary>
            /// The sz device
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 32)]
            internal char[] szDevice = new char[32];
        }

        /// <summary>
        /// The comrect class
        /// </summary>
        [StructLayout(LayoutKind.Sequential)]
        public class COMRECT
        {
            /// <summary>
            /// The bottom
            /// </summary>
            public int bottom;
            /// <summary>
            /// The left
            /// </summary>
            public int left;
            /// <summary>
            /// The right
            /// </summary>
            public int right;
            /// <summary>
            /// The top
            /// </summary>
            public int top;

            /// <summary>
            /// Initializes a new instance of the <see cref="COMRECT"/> class
            /// </summary>
            public COMRECT()
            {
            }

            /// <summary>
            /// Initializes a new instance of the <see cref="COMRECT"/> class
            /// </summary>
            /// <param name="r">The </param>
            public COMRECT(Rect r)
            {
                left = (int)r.X;
                top = (int)r.Y;
                right = (int)r.Right;
                bottom = (int)r.Bottom;
            }

            /// <summary>
            /// Initializes a new instance of the <see cref="COMRECT"/> class
            /// </summary>
            /// <param name="left">The left</param>
            /// <param name="top">The top</param>
            /// <param name="right">The right</param>
            /// <param name="bottom">The bottom</param>
            public COMRECT(int left, int top, int right, int bottom)
            {
                this.left = left;
                this.top = top;
                this.right = right;
                this.bottom = bottom;
            }

            /// <summary>
            /// Creates the xywh using the specified x
            /// </summary>
            /// <param name="x">The </param>
            /// <param name="y">The </param>
            /// <param name="width">The width</param>
            /// <param name="height">The height</param>
            /// <returns>The comrect</returns>
            public static COMRECT FromXYWH(int x, int y, int width, int height)
            {
                return new COMRECT(x, y, x + width, y + height);
            }

            /// <summary>
            /// Returns the string
            /// </summary>
            /// <returns>The string</returns>
            public override string ToString()
            {
                return "Left = " + left + " Top " + top + " Right = " + right + " Bottom = " + bottom;
            }
        }

        /// <summary>
        /// The id factory interface
        /// </summary>
        [InterfaceType(ComInterfaceType.InterfaceIsIUnknown), Guid("06152247-6f50-465a-9245-118bfd3b6007")]
        public interface ID2D1Factory
        {
            /// <summary>
            /// Reloads the system metrics
            /// </summary>
            /// <returns>The int</returns>
            int ReloadSystemMetrics();

            /// <summary>
            /// Gets the desktop dpi using the specified dpi x
            /// </summary>
            /// <param name="dpiX">The dpi</param>
            /// <param name="dpiY">The dpi</param>
            [PreserveSig]
            void GetDesktopDpi(out float dpiX, out float dpiY);

            // the rest is not implemented as we don't need it
        }
    }
}
