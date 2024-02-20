namespace MVVMToolKit.Navigation.Zones
{
    using System.Windows;

    public class ZoneRegistry
    {
        public static readonly DependencyProperty ZoneNameProperty = DependencyProperty.RegisterAttached("ZoneName", typeof(string), typeof(ZoneRegistry), new PropertyMetadata(defaultValue: null, propertyChangedCallback: OnSetZoneName));
        public static void SetZoneName(FrameworkElement target, object value) =>
            target.SetValue(ZoneNameProperty, value);

        public static string? GetZoneName(DependencyObject target)
        {
            if (target == null) throw new ArgumentNullException(nameof(target));
            return target.GetValue(ZoneNameProperty) as string;
        }
        private static void OnSetZoneName(DependencyObject element, DependencyPropertyChangedEventArgs args)
        {
            /// TODO
            /// 
        }
    }
}
