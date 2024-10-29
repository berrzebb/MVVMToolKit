using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Interop;
using MVVMToolKit.Interfaces;

namespace MVVMToolKit.Models
{
    public partial class PopupContext : IPopupContext
    {
        private readonly WeakReference _viewReference;
        /// <summary>
        /// 팝업에 대한 컨텍스트.
        /// </summary>
        /// <param name="view">사용할 팝업 화면</param>
        /// <exception cref="ArgumentNullException"></exception>
        public PopupContext(FrameworkElement? view)
        {
            if (view == null)
                throw new ArgumentNullException(nameof(view));

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

            if (Source is FrameworkElement frameworkElement)
            {
                frameworkElement.DataContext = null;

            }

            if (DataContext is IDialogContext dialogContext)
            {
                dialogContext.Cleanup();
            }

            _closeListener?.Invoke(this);
        }
        /// <inheritdoc/>
        public bool IsAlive => _viewReference.IsAlive;

        private Action<IPopupContext>? _closeListener;
        /// <inheritdoc/>
        public object? Source
        {
            get
            {
                if (!IsAlive || _viewReference.Target == null)
                    return null;

                return _viewReference.Target;
            }
        }
        /// <inheritdoc/>
        public object? DataContext
        {
            get
            {
                return Source is not ContentControl contentControl ? null : contentControl.Content;
            }
        }
        /// <inheritdoc/>
        public IntPtr Handle
        {
            get
            {

                if (Source is not Window window)
                {
                    return IntPtr.Zero;
                }

                var helper = new WindowInteropHelper(window);
                return helper.Handle;
            }
        }
        /// <inheritdoc/>
        public object? GetOwner()
        {
            if (Source is Window window)
                return window;
            return Source is not FrameworkElement frameworkElement ? null : Window.GetWindow(frameworkElement);
        }
        /// <inheritdoc/>
        public override bool Equals(object? obj)
        {
            if (obj is not PopupContext other)
                return false;
            if (Source is null)
                return false;
            return Source.Equals(other.Source);
        }
        /// <inheritdoc/>
        public override int GetHashCode() => Source!.GetHashCode() + DataContext!.GetHashCode();

        /// <inheritdoc/>
        public bool Activate()
        {
            return Owner?.Activate() ?? false;
        }
        /// <inheritdoc/>
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
        /// <inheritdoc/>
        public DialogResult Show(bool isModal = false)
        {
            DialogResult result = DialogResult.None;
            if (isModal)
            {
                bool done = Owner?.ShowDialog() ?? false;

                result = done ? DialogResult.Yes : DialogResult.No;
            }
            else
            {
                Owner?.Show();
            }

            return result;
        }
        /// <inheritdoc/>
        public IPopupContext Topmost(bool isEnabled)
        {
            if (Owner != null)
                Owner.Topmost = isEnabled;
            return this;
        }
        /// <inheritdoc/>
        public IPopupContext CloseListener(Action<IPopupContext> onClose)
        {
            _closeListener = onClose;
            return this;
        }
        /// <inheritdoc/>
        public IPopupContext Title(string title)
        {
            if (Owner != null)
            {
                Owner.Title = title;
            }

            return this;
        }
        /// <inheritdoc/>
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
        /// <inheritdoc/>
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
        /// <inheritdoc/>
        public IPopupContext Taskbar(bool isShow)
        {
            if (Owner != null)
                Owner.ShowInTaskbar = isShow;
            return this;
        }
        /// <inheritdoc/>
        public IPopupContext ShowActivated(bool isShow)
        {
            if (Owner != null)
                Owner.ShowActivated = isShow;
            return this;
        }
        /// <inheritdoc/>
        public IPopupContext ManualSize() => UpdateSizeToContent(SizeToContent.Manual);
        /// <inheritdoc/>
        public IPopupContext AutoSize() => UpdateSizeToContent(SizeToContent.WidthAndHeight);
        /// <inheritdoc/>
        public IPopupContext AutoSizeWidth() => UpdateSizeToContent(SizeToContent.Width);
        /// <inheritdoc/>
        public IPopupContext AutoSizeHeight() => UpdateSizeToContent(SizeToContent.Height);

        /// <inheritdoc/>
        public IPopupContext ManualLocation(double left = 0, double top = 0) => UpdateLocation(WindowStartupLocation.Manual, new WindowLocation(left, top));
        /// <inheritdoc/>
        public IPopupContext ManualLocation(Func<(double Width, double Height), (double Left, double Top)> getLocation)
        {
            (double Width, double Height) sourceLocation = (0, 0);
            if (Owner != null)
            {
                sourceLocation.Width = Owner.Width;
                sourceLocation.Height = Owner.Height;
            }

            (double left, double top) = getLocation(sourceLocation);
            return ManualLocation(left, top);
        }
        /// <inheritdoc/>
        public IPopupContext ScreenLocation() => UpdateLocation(WindowStartupLocation.CenterScreen);
        /// <inheritdoc/>
        public IPopupContext OwnerLocation() => UpdateLocation(WindowStartupLocation.CenterOwner);

        /// <inheritdoc/>
        public IPopupContext Minimize() => UpdateState(WindowState.Minimized);
        /// <inheritdoc/>
        public IPopupContext Maximize() => UpdateState(WindowState.Maximized);
        /// <inheritdoc/>
        public IPopupContext Normal() => UpdateState(WindowState.Normal);

    }
}
