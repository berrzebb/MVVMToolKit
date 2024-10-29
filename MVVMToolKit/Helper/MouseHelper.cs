using MVVMToolKit.Helper.Native;

namespace MVVMToolKit.Helper
{
    /// <summary>
    /// The mouse helper class
    /// </summary>
    public static class MouseHelper
    {
        /// <summary>
        /// Gets the value of the mouse position
        /// </summary>
        public static Point MousePosition
        {
            get
            {
                var pt = new NativeMethods.POINT();
                NativeMethods.GetCursorPos(pt);
                return new Point(pt.x, pt.y);
            }
        }
    }
}
