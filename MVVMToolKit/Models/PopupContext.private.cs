using MVVMToolKit.Interfaces;
using MVVMToolKit.Templates;

namespace MVVMToolKit.Models
{
    /// <inheritdoc />
    public partial class PopupContext
    {
        private PopupWindow? Owner => GetOwner() as PopupWindow;
        private sealed class WindowLocation
        {
            public double Left { get; }
            public double Top { get; }
            public WindowLocation(double left, double top)
            {
                Left = left;
                Top = top;
            }
        }
        private IPopupContext UpdateSizeToContent(SizeToContent sizeToContent)
        {
            if (Owner != null) Owner.SizeToContent = sizeToContent;
            return this;
        }

        private IPopupContext UpdateLocation(WindowStartupLocation location, WindowLocation? windowLocation = null)
        {
            if (Owner == null)
            {
                return this;
            }

            Owner.WindowStartupLocation = location;
            if (location == WindowStartupLocation.Manual && windowLocation != null)
            {
                Owner.Left = windowLocation.Left;
                Owner.Top = windowLocation.Top;
            }


            return this;
        }
        private IPopupContext UpdateState(WindowState state)
        {

            if (Owner != null) Owner.WindowState = state;
            return this;
        }
    }
}
