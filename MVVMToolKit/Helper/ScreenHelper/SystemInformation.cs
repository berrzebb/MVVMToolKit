using MVVMToolKit.Helper.Native;

namespace MVVMToolKit.Helper.ScreenHelper
{
    /// <summary>
    /// Provides information about the current system environment.
    /// </summary>
    public static class SystemInformation
    {
        /// <summary>
        /// Gets the bounds of the virtual screen in pixels.
        /// </summary>
        /// <returns>
        /// A <see cref="T:System.Windows.Rect" /> that specifies the bounding rectangle of the entire virtual screen in pixels.
        /// </returns>
        public static Rect VirtualScreen
        {
            get
            {
                var size = new Size(
                    NativeMethods.GetSystemMetrics(NativeMethods.SystemMetric.SM_CXVIRTUALSCREEN),
                    NativeMethods.GetSystemMetrics(NativeMethods.SystemMetric.SM_CYVIRTUALSCREEN));
                var location = new Point(
                    NativeMethods.GetSystemMetrics(NativeMethods.SystemMetric.SM_XVIRTUALSCREEN),
                    NativeMethods.GetSystemMetrics(NativeMethods.SystemMetric.SM_YVIRTUALSCREEN));
                return new Rect(location.X, location.Y, size.Width, size.Height);
            }
        }

        /// <summary>
        /// Gets the bounds of the virtual screen in units.
        /// </summary>
        /// <returns>
        /// A <see cref="T:System.Windows.Rect" /> that specifies the bounding rectangle of the entire virtual screen in units.
        /// </returns>
        public static Rect WpfVirtualScreen
        {
            get
            {
                if (NativeMethods.IsProcessDPIAware())
                {
                    var values = Screen.AllScreens.Aggregate(
                        new
                        {
                            xMin = 0.0,
                            yMin = 0.0,
                            xMax = 0.0,
                            yMax = 0.0,
                        },
                        (accumulator, s) =>
                        {
                            if (s != null)
                            {
                                return new
                                {
                                    xMin = Math.Min(s.WpfBounds.X, accumulator.xMin),
                                    yMin = Math.Min(s.WpfBounds.Y, accumulator.yMin),
                                    xMax = Math.Max(s.WpfBounds.Right, accumulator.xMax),
                                    yMax = Math.Max(s.WpfBounds.Bottom, accumulator.yMax),
                                };
                            }

                            return new
                            {
                                xMin = 0.0,
                                yMin = 0.0,
                                xMax = 0.0,
                                yMax = 0.0,
                            };
                        });

                    return new Rect(values.xMin, values.yMin, values.xMax - values.xMin, values.yMax - values.yMin);
                }

                return VirtualScreen;
            }
        }

        /// <summary>
        /// Gets the size, in pixels, of the working area of the screen.
        /// </summary>
        /// <returns>A <see cref="T:System.Windows.Rect" /> that represents the size, in pixels, of the working area of the screen.</returns>
        public static Rect WorkingArea
        {
            get
            {
                if (Screen.PrimaryScreen != null)
                {
                    return Screen.PrimaryScreen.WorkingArea;
                }

                return default;
            }
        }

        /// <summary>
        /// Gets the size, in units, of the working area of the screen.
        /// </summary>
        /// <returns>A <see cref="T:System.Windows.Rect" /> that represents the size, in units, of the working area of the screen.</returns>
        public static Rect WpfWorkingArea
        {
            get
            {
                if (Screen.PrimaryScreen != null)
                {
                    return Screen.PrimaryScreen.WpfWorkingArea;
                }

                return default;
            }
        }
    }
}
