namespace MVVMToolKit.Behaviors
{
    using System.Collections.Specialized;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Media;
    using Microsoft.Xaml.Behaviors;
    using MVVMToolKit.Comparers;
    using ListBox = System.Windows.Controls.ListBox;
    /// <summary>
    /// ItemsControl의 내용물을 자동 스크롤 하기 위한 Behavior
    /// </summary>
    public class AutoScrollBehavior : Behavior<ItemsControl>
    {
        private ScrollViewer? _scrollViewer;
        private bool _autoScroll = true;
        private bool _justWheeled = false;
        private bool _userInteracting = false;
        /// <summary>
        /// Attached.
        /// </summary>
        protected override void OnAttached()
        {
            AssociatedObject.Loaded += AssociatedObjectOnLoaded;
            AssociatedObject.Unloaded += AssociatedObjectOnUnloaded;
        }
        private void AssociatedObjectOnUnloaded(object sender, RoutedEventArgs routedEventArgs)
        {
            if (_scrollViewer != null)
            {
                _scrollViewer.ScrollChanged -= ScrollViewerOnScrollChanged;
            }

            if (AssociatedObject is ListBox lb)
            {

                lb.SelectionChanged -= AssociatedObjectOnSelectionChanged;
            }

            AssociatedObject.ItemContainerGenerator.ItemsChanged -= ItemContainerGeneratorItemsChanged;
            AssociatedObject.GotMouseCapture -= AssociatedObject_GotMouseCapture;
            AssociatedObject.LostMouseCapture -= AssociatedObject_LostMouseCapture;
            AssociatedObject.PreviewMouseWheel -= AssociatedObject_PreviewMouseWheel;

            _scrollViewer = null;
        }

        private void AssociatedObjectOnLoaded(object sender, RoutedEventArgs routedEventArgs)
        {
            _scrollViewer = GetScrollViewer(AssociatedObject);
            if (_scrollViewer != null)
            {
                _scrollViewer.ScrollChanged += ScrollViewerOnScrollChanged;

                if (AssociatedObject is ListBox lb)
                {

                    lb.SelectionChanged += AssociatedObjectOnSelectionChanged;
                }

                AssociatedObject.ItemContainerGenerator.ItemsChanged += ItemContainerGeneratorItemsChanged;
                AssociatedObject.GotMouseCapture += AssociatedObject_GotMouseCapture;
                AssociatedObject.LostMouseCapture += AssociatedObject_LostMouseCapture;
                AssociatedObject.PreviewMouseWheel += AssociatedObject_PreviewMouseWheel;
            }
        }

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

        void AssociatedObject_GotMouseCapture(object sender, System.Windows.Input.MouseEventArgs e)
        {
            _userInteracting = true;
            _autoScroll = false;
        }

        void AssociatedObject_LostMouseCapture(object sender, System.Windows.Input.MouseEventArgs e)
        {

            _userInteracting = false;
        }

        private void ScrollViewerOnScrollChanged(object sender, ScrollChangedEventArgs e)
        {

            if (_scrollViewer == null)
                return;
            double diff = _scrollViewer.VerticalOffset - (_scrollViewer.ExtentHeight - _scrollViewer.ViewportHeight);

            if (_justWheeled && !DoubleUtil.Equals(diff, 0.0))
            {
                _justWheeled = false;
                _autoScroll = false;
                return;
            }

            if (DoubleUtil.Equals(diff, 0.0))
            {

                _autoScroll = true;
            }
        }

        private void ItemContainerGeneratorItemsChanged(object sender, System.Windows.Controls.Primitives.ItemsChangedEventArgs e)
        {
            if (e.Action is
                not NotifyCollectionChangedAction.Add
                and
                not NotifyCollectionChangedAction.Reset)
                return;

            if (!_autoScroll || _userInteracting)
                return;
            _scrollViewer?.ScrollToBottom();
        }

        private void AssociatedObjectOnSelectionChanged(object sender, SelectionChangedEventArgs selectionChangedEventArgs)
        {
            _autoScroll = false;
        }

        void AssociatedObject_PreviewMouseWheel(object sender, System.Windows.Input.MouseWheelEventArgs e)
        {

            _justWheeled = true;
        }
    }
}
