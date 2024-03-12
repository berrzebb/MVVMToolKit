﻿using System.Windows.Input;

namespace MVVMToolKit.AttachedProperties
{
    public static class LayoutExtension
    {
        public static readonly DependencyProperty IsDragWindowProperty = DependencyProperty.RegisterAttached("IsDragWindow", typeof(bool), typeof(LayoutExtension), new PropertyMetadata(IsDragWindowPropertyChanged));

        private static object GetIsDragWindow(FrameworkElement target)
            => target.GetValue(IsDragWindowProperty);

        public static void SetIsDragWindow(FrameworkElement target, object value) =>
            target.SetValue(IsDragWindowProperty, value);

        private static FrameworkElement? AssociatedObject;

        private static void IsDragWindowPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is not Window window) return;
            if (e.NewValue is not bool) return;
            if (AssociatedObject == null)
            {
                AssociatedObject = window;
                AssociatedObject.Loaded += AssociatedObjectOnLoaded;
                AssociatedObject.Unloaded += AssociatedObjectOnUnloaded;

            }

        }

        private static void AssociatedObjectOnUnloaded(object sender, RoutedEventArgs e)
        {
            if (AssociatedObject == null) return;
            AssociatedObject.MouseMove -= AssociatedObject_MouseMove;
        }

        private static void AssociatedObjectOnLoaded(object sender, RoutedEventArgs e)
        {
            if (AssociatedObject == null) return;
            AssociatedObject.MouseMove += AssociatedObject_MouseMove;
        }
        private static void DoDragWindow(bool isDragging = false)
        {
            if (AssociatedObject == null) return;
            Window? targetWindow;
            if (AssociatedObject is Window window)
            {
                targetWindow = window;
            }
            else
            {
                targetWindow = Window.GetWindow(AssociatedObject);
            }
            if (targetWindow == null) return;

            if (isDragging) targetWindow.DragMove();

        }
        private static void AssociatedObject_MouseMove(object sender, MouseEventArgs e)
        {
            if (AssociatedObject == null) return;
            bool isDragWindow = (bool)GetIsDragWindow(AssociatedObject);
            if (isDragWindow) DoDragWindow(e.LeftButton == MouseButtonState.Pressed);

        }
    }
}
