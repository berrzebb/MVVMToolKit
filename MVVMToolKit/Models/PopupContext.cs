using System.Windows.Controls;
using System.Windows.Interop;
using MVVMToolKit.Interfaces;

namespace MVVMToolKit.Models
{
    public partial class PopupContext : IPopupContext
    {
        private readonly WeakReference _viewReference;

        public PopupContext(FrameworkElement? view)
        {
            if (view == null) throw new ArgumentNullException(nameof(view));

            _viewReference = new WeakReference(view);
            if (view is Window window)
            {
                window.Closed += Window_Closed;
            }
        }

        private void Window_Closed(object? sender, EventArgs e)
        {
            if (sender == _viewReference.Target)
            {
                _viewReference.Target = null;
                if (sender is Window window)
                {
                    window.Closed -= Window_Closed;
                }
            }
        }

        public bool IsAlive => _viewReference.IsAlive;


        private Action<IPopupContext>? _closeListener;

        public object Source
        {
            get
            {
                if (!IsAlive) throw new InvalidOperationException("View has been garbage collected");
                if (_viewReference.Target == null) throw new InvalidOperationException("View has been set to null");

                return _viewReference.Target;
            }
        }
        public object DataContext
        {
            get
            {
                if (!IsAlive) throw new InvalidOperationException("View has been garbage collected");
                if (_viewReference.Target == null) throw new InvalidOperationException("View has been set to null");

                if (_viewReference.Target is not ContentControl contentControl)
                {
                    throw new InvalidOperationException("Invalid ContentControl");
                }

                return contentControl.Content;
            }
        }
        public IntPtr Handle
        {
            get
            {
                if (!IsAlive) throw new InvalidOperationException("View has been garbage collected");
                if (_viewReference.Target == null) throw new InvalidOperationException("View has been set to null");

                if (_viewReference.Target is not Window window)
                {
                    throw new InvalidOperationException("Invalid Window");
                }
                var helper = new WindowInteropHelper(window);
                return helper.Handle;
            }
        }
        public object? GetOwner()
        {
            if (Source is Window window) return window;
            return Source is not FrameworkElement frameworkElement ? null : Window.GetWindow(frameworkElement);
        }

        public override bool Equals(object? obj)
        {
            if (obj is not PopupContext other) return false;

            return Source.Equals(other.Source);
        }
        public override int GetHashCode() => Source.GetHashCode() + DataContext.GetHashCode();


        public bool Activate()
        {
            return Owner?.Activate() ?? false;
        }

        public void Close()
        {

            if (Source is FrameworkElement frameworkElement)
            {
                frameworkElement.DataContext = null;

            }
            if (DataContext is IDialogContext dialogContext)
            {
                dialogContext.Cleanup();
            }
            _closeListener?.Invoke(this);

            Owner?.Close();
        }

        public IPopupContext Show(bool isModal)
        {
            if (isModal)
            {
                Owner?.ShowDialog();

            }
            else
            {
                Owner?.Show();
            }

            return this;
        }
        public IPopupContext Topmost(bool isEnabled)
        {
            if (Owner != null) Owner.Topmost = isEnabled;
            return this;
        }
        public IPopupContext CloseListener(Action<IPopupContext> onClose)
        {
            _closeListener = onClose;
            return this;
        }
        public IPopupContext Title(string title)
        {
            if (Owner != null)
            {
                Owner.Title = title;
            }
            return this;
        }
        public IPopupContext TitleBar(bool isEnabled, bool isToolWindow = false, double captionHeight = 28)
        {
            if (Owner != null)
            {
                if (isEnabled)
                {
                    Owner.WindowStyle = isToolWindow ? WindowStyle.ToolWindow : WindowStyle.SingleBorderWindow;
                    Owner.CaptionHeight = captionHeight;
                }
                else
                {
                    Owner.WindowStyle = WindowStyle.None;

                }
            }
            return this;
        }
        public IPopupContext EnableResize(bool isResize, bool isMinimize = false, bool isResizeGrip = false)
        {
            if (Owner != null)
            {
                if (isResize)
                {
                    Owner.ResizeMode = isResizeGrip ? ResizeMode.CanResizeWithGrip : ResizeMode.CanResize;

                }
                else
                {
                    Owner.ResizeMode = isMinimize ? ResizeMode.CanMinimize : ResizeMode.NoResize;
                }
            }
            return this;
        }
        public IPopupContext Taskbar(bool isShow)
        {
            if (Owner != null) Owner.ShowInTaskbar = isShow;
            return this;
        }
        public IPopupContext ShowActivated(bool isShow)
        {
            if (Owner != null) Owner.ShowActivated = isShow;
            return this;
        }

        public IPopupContext ManualSize() => UpdateSizeToContent(SizeToContent.Manual);
        public IPopupContext AutoSize() => UpdateSizeToContent(SizeToContent.WidthAndHeight);
        public IPopupContext AutoSizeWidth() => UpdateSizeToContent(SizeToContent.Width);
        public IPopupContext AutoSizeHeight() => UpdateSizeToContent(SizeToContent.Height);


        public IPopupContext ManualLocation(double left = 0, double top = 0) => UpdateLocation(WindowStartupLocation.Manual, new WindowLocation(left, top));
        public IPopupContext ManualLocation(Func<(double Width, double Height), (double Left, double Top)> getLocation)
        {
            (double Width, double Height) sourceLocation = (0, 0);
            if (Owner != null)
            {
                sourceLocation.Width = Owner.Width;
                sourceLocation.Height = Owner.Height;
            }
            (double Left, double Top) windowLocation = getLocation(sourceLocation);
            return ManualLocation(windowLocation.Left, windowLocation.Top);
        }
        public IPopupContext ScreenLocation() => UpdateLocation(WindowStartupLocation.CenterScreen);
        public IPopupContext OwnerLocation() => UpdateLocation(WindowStartupLocation.CenterOwner);


        public IPopupContext Minimize() => UpdateState(WindowState.Minimized);
        public IPopupContext Maximize() => UpdateState(WindowState.Maximized);
        public IPopupContext Normal() => UpdateState(WindowState.Normal);

    }
}
