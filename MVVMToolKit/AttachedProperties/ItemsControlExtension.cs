using System.Collections.Specialized;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Media;
using MVVMToolKit.Comparers;

namespace MVVMToolKit.AttachedProperties
{
    /// <summary>
    /// ItemsControl의 확장 메서드입니다.
    /// </summary>
    public static class ItemsControlExtension
    {
        /// <summary>
        /// 자동 스크롤 여부.
        /// </summary>
        public static readonly DependencyProperty IsAutoScrollProperty = DependencyProperty.RegisterAttached("IsAutoScroll", typeof(bool), typeof(ItemsControlExtension), new(IsAutoScrollPropertyChanged));

        /// <summary>
        /// 자동으로 스크롤 될지 여부의 값을 가져옵니다.
        /// </summary>
        /// <param name="target">가져올 객체</param>
        /// <returns>자동으로 스크롤 되는지 여부</returns>
        public static object? GetIsAutoScroll(ItemsControl target)
=> target.GetValue(IsAutoScrollProperty);

        /// <summary>
        /// 자동으로 스크롤 될지 여부를 설정합니다.
        /// </summary>
        /// <param name="target">설정할 객체</param>
        /// <param name="value">자동으로 스크롤 되는지 여부</param>
        public static void SetIsAutoScroll(ItemsControl target, object value) => target.SetValue(IsAutoScrollProperty, value);

        private static ItemsControl? sAssociatedObject;
        private static ScrollViewer? sScrollViewer;

        private static bool sAutoScroll = true;
        private static bool sJustWheeled;
        private static bool sUserInteracting;

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
            if (sAssociatedObject == null)
                return;
            sAssociatedObject.Loaded += AssociatedObjectOnLoaded;
            sAssociatedObject.Unloaded += AssociatedObjectOnUnloaded;
        }

        private static void AssociatedObjectOnUnloaded(object sender, RoutedEventArgs e)
        {
            if (sAssociatedObject == null)
                return;

            if (sScrollViewer != null)
            {
                sScrollViewer.ScrollChanged -= ScrollViewerOnScrollChanged;
            }

            if (sAssociatedObject is ListBox listBox)
            {
                listBox.SelectionChanged -= ListBoxOnSelectionChanged;
            }

            sAssociatedObject.ItemContainerGenerator.ItemsChanged -= ItemContainerGeneratorOnItemsChanged;
            sAssociatedObject.GotMouseCapture -= ItemsControlOnGotMouseCapture;
            sAssociatedObject.LostMouseCapture -= ItemsControlOnLostMouseCapture;
            sAssociatedObject.PreviewMouseWheel -= ItemsControlOnPreviewMouseWheel;

            sScrollViewer = null;
        }

        private static void AssociatedObjectOnLoaded(object sender, RoutedEventArgs e)
        {
            if (sAssociatedObject == null)
                return;

            sScrollViewer = GetScrollViewer(sAssociatedObject);

            if (sScrollViewer != null)
            {
                sScrollViewer.ScrollChanged += ScrollViewerOnScrollChanged;
            }

            if (sAssociatedObject is ListBox listBox)
            {
                listBox.SelectionChanged += ListBoxOnSelectionChanged;
            }

            sAssociatedObject.ItemContainerGenerator.ItemsChanged += ItemContainerGeneratorOnItemsChanged;
            sAssociatedObject.GotMouseCapture += ItemsControlOnGotMouseCapture;
            sAssociatedObject.LostMouseCapture += ItemsControlOnLostMouseCapture;
            sAssociatedObject.PreviewMouseWheel += ItemsControlOnPreviewMouseWheel;
        }

        private static void IsAutoScrollPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is not ItemsControl itemsControl)
                return;

            if (sAssociatedObject == null)
            {
                sAssociatedObject = itemsControl;
                OnAttached();

            }
        }

        private static void ListBoxOnSelectionChanged(object sender, SelectionChangedEventArgs e) => sAutoScroll = false;

        private static void ItemsControlOnPreviewMouseWheel(object sender, MouseWheelEventArgs e) => sJustWheeled = true;

        private static void ItemsControlOnLostMouseCapture(object sender, MouseEventArgs e) => sUserInteracting = false;

        private static void ItemsControlOnGotMouseCapture(object sender, MouseEventArgs e)
        {
            sUserInteracting = true;
            sAutoScroll = false;
        }

        private static void ItemContainerGeneratorOnItemsChanged(object sender, ItemsChangedEventArgs e)
        {
            if (e.Action is
                not NotifyCollectionChangedAction.Add
                and
                not NotifyCollectionChangedAction.Reset)
                return;

            if (sAssociatedObject == null)
                return;
            if (GetIsAutoScroll(sAssociatedObject) is bool isAutoScroll)
            {
                if (!sAutoScroll || sUserInteracting || !isAutoScroll)
                    return;
                sScrollViewer?.ScrollToBottom();

            }
        }

        private static void ScrollViewerOnScrollChanged(object sender, ScrollChangedEventArgs e)
        {

            if (sScrollViewer == null)
                return;
            double diff = sScrollViewer.VerticalOffset - (sScrollViewer.ExtentHeight - sScrollViewer.ViewportHeight);

            if (sJustWheeled && !DoubleUtil.Equals(diff, 0.0))
            {
                sJustWheeled = false;
                sAutoScroll = false;
                return;
            }

            if (DoubleUtil.Equals(diff, 0.0))
            {

                sAutoScroll = true;
            }
        }
    }
}
