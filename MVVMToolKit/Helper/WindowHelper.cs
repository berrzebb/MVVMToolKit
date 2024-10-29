using System.Windows.Interop;
using MVVMToolKit.Helper.Native;
using MVVMToolKit.Helper.ScreenHelper;

namespace MVVMToolKit.Helper
{
    /// <summary>
    /// The window positions enum
    /// </summary>
    public enum WindowPositions
    {
        /// <summary>
        /// The center window positions
        /// </summary>
        Center,
        /// <summary>
        /// The left window positions
        /// </summary>
        Left,
        /// <summary>
        /// The top window positions
        /// </summary>
        Top,
        /// <summary>
        /// The right window positions
        /// </summary>
        Right,
        /// <summary>
        /// The bottom window positions
        /// </summary>
        Bottom,
        /// <summary>
        /// The top left window positions
        /// </summary>
        TopLeft,
        /// <summary>
        /// The top right window positions
        /// </summary>
        TopRight,
        /// <summary>
        /// The bottom right window positions
        /// </summary>
        BottomRight,
        /// <summary>
        /// The bottom left window positions
        /// </summary>
        BottomLeft,
        /// <summary>
        /// The maximize window positions
        /// </summary>
        Maximize
    }
    /// <summary>
    /// Provides helper functions for window class.
    /// </summary>
    public static class WindowHelper
    {
        /// <summary>
        /// Moves window to desired position on screen.
        /// </summary>
        /// <param name="window">Window to move.</param>
        /// <param name="x">X coordinate for moving.</param>
        /// <param name="y">Y coordinate for moving.</param>
        /// <param name="width">New width of the window.</param>
        /// <param name="height">New height of the window.</param>
        public static void SetWindowPosition(this Window window, int x, int y, int width, int height)
        {
            // The first move puts it on the correct monitor, which triggers WM_DPICHANGED
            // The +1/-1 coerces WPF to update Window.Top/Left/Width/Height in the second move
            NativeMethods.MoveWindow(new WindowInteropHelper(window).Handle, x - 1, y, width + 1, height, false);
            NativeMethods.MoveWindow(new WindowInteropHelper(window).Handle, x, y, width, height, true);
        }

        /// <summary>
        /// Moves window to desired position on screen.
        /// </summary>
        /// <param name="window">Window to move.</param>
        /// <param name="pos">Desired position.</param>
        /// <param name="screen">The screen to which we move.</param>
        public static void SetWindowPosition(this Window window, WindowPositions pos, Screen screen)
        {
            var coordinates = CalculateWindowCoordinates(window, pos, screen);

            window.SetWindowPosition((int)coordinates.X, (int)coordinates.Y, (int)coordinates.Width, (int)coordinates.Height);
        }

        /// <summary>
        /// Sets the window position using the specified window
        /// </summary>
        /// <param name="window">The window</param>
        /// <param name="screen">The screen</param>
        public static void SetWindowPosition(this Window window, Screen screen)
        {
            window.Left = screen.WorkingArea.X;
            window.Top = screen.WorkingArea.Y;
            window.Width = screen.WorkingArea.Width;
            window.Height = screen.WorkingArea.Height;
        }
        /// <summary>
        /// Gets window position on screen with respect of screen scale factor.
        /// </summary>
        public static Rect GetWindowAbsolutePlacement(this Window window)
        {
            var placement = window.GetWindowPlacement();

            return new Rect(
                Math.Abs(placement.Left),
                Math.Abs(placement.Top),
                placement.Width,
                placement.Height);
        }

        /// <summary>
        /// Gets the window placement using the specified window
        /// </summary>
        /// <param name="window">The window</param>
        /// <returns>The rect</returns>
        public static Rect GetWindowPlacement(this Window window)
        {
            var screen = Screen.FromWindow(window);

            var left = (screen.WpfBounds.Left - window.Left) * screen.ScaleFactor;
            var top = (screen.WpfBounds.Top - window.Top) * screen.ScaleFactor;
            var width = window.Width * screen.ScaleFactor;
            var height = window.Height * screen.ScaleFactor;

            return new Rect(left, top, width, height);
        }

        /// <summary>
        /// Calculates window end position.
        /// </summary>
        private static Rect CalculateWindowCoordinates(FrameworkElement window, WindowPositions pos, Screen screen)
        {
            switch (pos)
            {
                case WindowPositions.Center:
                    {
                        var x = screen.WpfBounds.X + (screen.WpfBounds.Width - window.Width) / 2.0;
                        var y = screen.WpfBounds.Y + (screen.WpfBounds.Height - window.Height) / 2.0;

                        return new Rect(x * screen.ScaleFactor, y * screen.ScaleFactor, window.Width * screen.ScaleFactor, window.Height * screen.ScaleFactor);
                    }

                case WindowPositions.Left:
                    {
                        var y = screen.WpfBounds.Y + (screen.WpfBounds.Height - window.Height) / 2.0;

                        return new Rect(screen.WpfBounds.X * screen.ScaleFactor, y * screen.ScaleFactor, window.Width * screen.ScaleFactor, window.Height * screen.ScaleFactor);
                    }

                case WindowPositions.Top:
                    {
                        var x = screen.WpfBounds.X + (screen.WpfBounds.Width - window.Width) / 2.0;

                        return new Rect(x * screen.ScaleFactor, screen.WpfBounds.Y * screen.ScaleFactor, window.Width * screen.ScaleFactor, window.Height * screen.ScaleFactor);
                    }

                case WindowPositions.Right:
                    {
                        var x = screen.WpfBounds.X + (screen.WpfBounds.Width - window.Width);
                        var y = screen.WpfBounds.Y + (screen.WpfBounds.Height - window.Height) / 2.0;

                        return new Rect(x * screen.ScaleFactor, y * screen.ScaleFactor, window.Width * screen.ScaleFactor, window.Height * screen.ScaleFactor);
                    }

                case WindowPositions.Bottom:
                    {
                        var x = screen.WpfBounds.X + (screen.WpfBounds.Width - window.Width) / 2.0;
                        var y = screen.WpfBounds.Y + (screen.WpfBounds.Height - window.Height);

                        return new Rect(x * screen.ScaleFactor, y * screen.ScaleFactor, window.Width * screen.ScaleFactor, window.Height * screen.ScaleFactor);
                    }

                case WindowPositions.TopLeft:
                    return new Rect(screen.WpfBounds.X * screen.ScaleFactor, screen.WpfBounds.Y * screen.ScaleFactor, window.Width * screen.ScaleFactor, window.Height * screen.ScaleFactor);

                case WindowPositions.TopRight:
                    {
                        var x = screen.WpfBounds.X + (screen.WpfBounds.Width - window.Width);

                        return new Rect(x * screen.ScaleFactor, screen.WpfBounds.Y * screen.ScaleFactor, window.Width * screen.ScaleFactor, window.Height * screen.ScaleFactor);
                    }

                case WindowPositions.BottomRight:
                    {
                        var x = screen.WpfBounds.X + (screen.WpfBounds.Width - window.Width);
                        var y = screen.WpfBounds.Y + (screen.WpfBounds.Height - window.Height);

                        return new Rect(x * screen.ScaleFactor, y * screen.ScaleFactor, window.Width * screen.ScaleFactor, window.Height * screen.ScaleFactor);
                    }

                case WindowPositions.BottomLeft:
                    {
                        var y = screen.WpfBounds.Y + (screen.WpfBounds.Height - window.Height);

                        return new Rect(screen.WpfBounds.X * screen.ScaleFactor, y * screen.ScaleFactor, window.Width * screen.ScaleFactor, window.Height * screen.ScaleFactor);
                    }

                case WindowPositions.Maximize:
                    return new Rect(screen.WpfBounds.X * screen.ScaleFactor, screen.WpfBounds.Y * screen.ScaleFactor, screen.WpfBounds.Width * screen.ScaleFactor, screen.WpfBounds.Height * screen.ScaleFactor);

                default:
                    return Rect.Empty;
            }
        }
    }
}
