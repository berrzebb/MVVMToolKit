using System.Collections.Specialized;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Media;

namespace MVVMToolKit.AttachedProperties
{
    public class ItemsControlExtension
    {
        public static readonly DependencyProperty IsAutoScrollProperty = DependencyProperty.RegisterAttached("IsAutoScroll", typeof(bool), typeof(ItemsControlExtension), new(IsAutoScrollPropertyChanged));

        public static object GetIsAutoScroll(ItemsControl target)
=> target.GetValue(IsAutoScrollProperty);

        public static void SetIsAutoScroll(ItemsControl target, object value) =>
            target.SetValue(IsAutoScrollProperty, value);

        private static ItemsControl? _associatedObject;
        private static ScrollViewer? _scrollViewer;

        private static bool _autoScroll = true;
        private static bool _justWheeled;
        private static bool _userInteracting;

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
            if (_associatedObject == null) return;
            _associatedObject.Loaded += AssociatedObjectOnLoaded;
            _associatedObject.Unloaded += AssociatedObjectOnUnloaded;
        }

        private static void AssociatedObjectOnUnloaded(object sender, RoutedEventArgs e)
        {
            if (_associatedObject == null) return;

            if (_scrollViewer != null)
            {
                _scrollViewer.ScrollChanged -= ScrollViewerOnScrollChanged;
            }

            if (_associatedObject is ListBox listBox)
            {
                listBox.SelectionChanged -= ListBoxOnSelectionChanged;
            }
            _associatedObject.ItemContainerGenerator.ItemsChanged -= ItemContainerGeneratorOnItemsChanged;
            _associatedObject.GotMouseCapture -= ItemsControlOnGotMouseCapture;
            _associatedObject.LostMouseCapture -= ItemsControlOnLostMouseCapture;
            _associatedObject.PreviewMouseWheel -= ItemsControlOnPreviewMouseWheel;

            _scrollViewer = null;
        }

        private static void AssociatedObjectOnLoaded(object sender, RoutedEventArgs e)
        {
            if (_associatedObject == null) return;

            _scrollViewer = GetScrollViewer(_associatedObject);

            if (_scrollViewer != null)
            {
                _scrollViewer.ScrollChanged += ScrollViewerOnScrollChanged;
            }

            if (_associatedObject is ListBox listBox)
            {
                listBox.SelectionChanged += ListBoxOnSelectionChanged;
            }
            _associatedObject.ItemContainerGenerator.ItemsChanged += ItemContainerGeneratorOnItemsChanged;
            _associatedObject.GotMouseCapture += ItemsControlOnGotMouseCapture;
            _associatedObject.LostMouseCapture += ItemsControlOnLostMouseCapture;
            _associatedObject.PreviewMouseWheel += ItemsControlOnPreviewMouseWheel;
        }

        private static void IsAutoScrollPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is not ItemsControl itemsControl) return;
            if (e.NewValue is not bool isAutoScroll) return;
            if (_associatedObject == null)
            {
                _associatedObject = itemsControl;
                OnAttached();

            }
        }

        private static void ListBoxOnSelectionChanged(object sender, SelectionChangedEventArgs e) => _autoScroll = false;

        private static void ItemsControlOnPreviewMouseWheel(object sender, MouseWheelEventArgs e) => _justWheeled = true;

        private static void ItemsControlOnLostMouseCapture(object sender, MouseEventArgs e) => _userInteracting = false;

        private static void ItemsControlOnGotMouseCapture(object sender, MouseEventArgs e)
        {
            _userInteracting = true;
            _autoScroll = false;
        }

        private static void ItemContainerGeneratorOnItemsChanged(object sender, ItemsChangedEventArgs e)
        {
            if (e.Action != NotifyCollectionChangedAction.Add &&
                e.Action != NotifyCollectionChangedAction.Reset) return;
            if (_associatedObject == null) return;
            if (GetIsAutoScroll(_associatedObject) is bool isAutoScroll)
            {
                if (!_autoScroll || _userInteracting || !isAutoScroll) return;
                _scrollViewer?.ScrollToBottom();

            }
        }

        private static void ScrollViewerOnScrollChanged(object sender, ScrollChangedEventArgs e)
        {

            if (_scrollViewer == null) return;
            double diff = (_scrollViewer.VerticalOffset - (_scrollViewer.ExtentHeight - _scrollViewer.ViewportHeight));


            if (_justWheeled && diff != 0.0)
            {
                _justWheeled = false;
                _autoScroll = false;
                return;
            }

            if (diff == 0.0)
            {

                _autoScroll = true;
            }
        }
    }
}
