using System.Windows.Input;

namespace MVVMToolKit.AttachedProperties
{
    public class LayoutExtension
    {
        public static readonly DependencyProperty IsDragWindowProperty = DependencyProperty.RegisterAttached("IsDragWindow", typeof(bool), typeof(LayoutExtension), new PropertyMetadata(IsDragWindowPropertyChanged));

        public static object GetIsDragWindow(FrameworkElement target)
            => target.GetValue(IsDragWindowProperty);

        public static void SetIsDragWindow(FrameworkElement target, object value) =>
            target.SetValue(IsDragWindowProperty, value);

        private static FrameworkElement? _associatedObject;

        private static void IsDragWindowPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is not Window window) return;
            if (e.NewValue is not bool isDragWindow) return;
            if (_associatedObject == null)
            {
                _associatedObject = window;
                _associatedObject.Loaded += AssociatedObjectOnLoaded;
                _associatedObject.Unloaded += AssociatedObjectOnUnloaded;

            }

        }

        private static void AssociatedObjectOnUnloaded(object sender, RoutedEventArgs e)
        {
            if (_associatedObject == null) return;
            _associatedObject.MouseMove -= AssociatedObject_MouseMove;
        }

        private static void AssociatedObjectOnLoaded(object sender, RoutedEventArgs e)
        {
            if (_associatedObject == null) return;
            _associatedObject.MouseMove += AssociatedObject_MouseMove;
        }
        private static void DoDragWindow(bool isDragging = false)
        {
            if (_associatedObject == null) return;
            Window? targetWindow;
            if (_associatedObject is Window window)
            {
                targetWindow = window;
            }
            else
            {
                targetWindow = Window.GetWindow(_associatedObject);
            }
            if (targetWindow == null) return;

            if (isDragging) targetWindow.DragMove();

        }
        private static void AssociatedObject_MouseMove(object sender, MouseEventArgs e)
        {
            if (_associatedObject == null) return;
            bool isDragWindow = (bool)GetIsDragWindow(_associatedObject);
            if (isDragWindow) DoDragWindow(e.LeftButton == MouseButtonState.Pressed);

        }
    }
}
