using System.Collections.Specialized;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Media;

namespace MVVMToolKit.AttachedProperties
{
    public static class ItemsControlExtension
    {
        public static readonly DependencyProperty IsAutoScrollProperty = DependencyProperty.RegisterAttached("IsAutoScroll", typeof(bool), typeof(ItemsControlExtension), new(IsAutoScrollPropertyChanged));

        private static object GetIsAutoScroll(ItemsControl target)
=> target.GetValue(IsAutoScrollProperty);

        public static void SetIsAutoScroll(ItemsControl target, object value) =>
            target.SetValue(IsAutoScrollProperty, value);

        private static ItemsControl? s_associatedObject;
        private static ScrollViewer? s_scrollViewer;

        private static bool s_autoScroll = true;
        private static bool s_justWheeled;
        private static bool s_userInteracting;

        private static ScrollViewer? GetScrollViewer(DependencyObject root)
        {
            int childCount = VisualTreeHelper.GetChildrenCount(root);
            for (int i = 0; i < childCount; ++i)
            {
                DependencyObject child = VisualTreeHelper.GetChild(root, i);
                if (child is ScrollViewer sv)
                {
                    return sv;
                }

                ScrollViewer? foundChild = GetScrollViewer(child);
                if (foundChild != child)
                {
                    return foundChild;
                }
            }
            return null;
        }

        private static void OnAttached()
        {
            if (s_associatedObject == null) return;
            s_associatedObject.Loaded += AssociatedObjectOnLoaded;
            s_associatedObject.Unloaded += AssociatedObjectOnUnloaded;
        }

        private static void AssociatedObjectOnUnloaded(object sender, RoutedEventArgs e)
        {
            if (s_associatedObject == null) return;

            if (s_scrollViewer != null)
            {
                s_scrollViewer.ScrollChanged -= ScrollViewerOnScrollChanged;
            }

            if (s_associatedObject is ListBox listBox)
            {
                listBox.SelectionChanged -= ListBoxOnSelectionChanged;
            }
            s_associatedObject.ItemContainerGenerator.ItemsChanged -= ItemContainerGeneratorOnItemsChanged;
            s_associatedObject.GotMouseCapture -= ItemsControlOnGotMouseCapture;
            s_associatedObject.LostMouseCapture -= ItemsControlOnLostMouseCapture;
            s_associatedObject.PreviewMouseWheel -= ItemsControlOnPreviewMouseWheel;

            s_scrollViewer = null;
        }

        private static void AssociatedObjectOnLoaded(object sender, RoutedEventArgs e)
        {
            if (s_associatedObject == null) return;

            s_scrollViewer = GetScrollViewer(s_associatedObject);

            if (s_scrollViewer != null)
            {
                s_scrollViewer.ScrollChanged += ScrollViewerOnScrollChanged;
            }

            if (s_associatedObject is ListBox listBox)
            {
                listBox.SelectionChanged += ListBoxOnSelectionChanged;
            }
            s_associatedObject.ItemContainerGenerator.ItemsChanged += ItemContainerGeneratorOnItemsChanged;
            s_associatedObject.GotMouseCapture += ItemsControlOnGotMouseCapture;
            s_associatedObject.LostMouseCapture += ItemsControlOnLostMouseCapture;
            s_associatedObject.PreviewMouseWheel += ItemsControlOnPreviewMouseWheel;
        }

        private static void IsAutoScrollPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is not ItemsControl itemsControl) return;

            if (s_associatedObject == null)
            {
                s_associatedObject = itemsControl;
                OnAttached();

            }
        }

        private static void ListBoxOnSelectionChanged(object sender, SelectionChangedEventArgs e) => s_autoScroll = false;

        private static void ItemsControlOnPreviewMouseWheel(object sender, MouseWheelEventArgs e) => s_justWheeled = true;

        private static void ItemsControlOnLostMouseCapture(object sender, MouseEventArgs e) => s_userInteracting = false;

        private static void ItemsControlOnGotMouseCapture(object sender, MouseEventArgs e)
        {
            s_userInteracting = true;
            s_autoScroll = false;
        }

        private static void ItemContainerGeneratorOnItemsChanged(object sender, ItemsChangedEventArgs e)
        {
            if (e.Action != NotifyCollectionChangedAction.Add &&
                e.Action != NotifyCollectionChangedAction.Reset) return;
            if (s_associatedObject == null) return;
            if (GetIsAutoScroll(s_associatedObject) is bool isAutoScroll)
            {
                if (!s_autoScroll || s_userInteracting || !isAutoScroll) return;
                s_scrollViewer?.ScrollToBottom();

            }
        }

        private static void ScrollViewerOnScrollChanged(object sender, ScrollChangedEventArgs e)
        {

            if (s_scrollViewer == null) return;
            double diff = (s_scrollViewer.VerticalOffset - (s_scrollViewer.ExtentHeight - s_scrollViewer.ViewportHeight));


            if (s_justWheeled && diff != 0.0)
            {
                s_justWheeled = false;
                s_autoScroll = false;
                return;
            }

            if (diff == 0.0)
            {

                s_autoScroll = true;
            }
        }
    }
}
