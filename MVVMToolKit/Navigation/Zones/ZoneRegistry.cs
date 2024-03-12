namespace MVVMToolKit.Navigation.Zones
{
    using System.Collections.Concurrent;
    using System.Diagnostics;
    using System.Windows;

    public interface IZoneRegistry
    {
        DependencyObject? this[string zoneName] { get; }
    }

    public class ZoneRegistry : IZoneRegistry
    {
        private static readonly ConcurrentDictionary<string, DependencyObject> Zones = new();
        public static readonly DependencyProperty ZoneNameProperty = DependencyProperty.RegisterAttached("ZoneName", typeof(string), typeof(ZoneRegistry), new PropertyMetadata(defaultValue: null, propertyChangedCallback: OnZoneNameChanged));

        DependencyObject? IZoneRegistry.this[string zoneName] => Zones.TryGetValue(zoneName, out DependencyObject? zone) ? zone : null;

        public static void SetZoneName(DependencyObject target, object value)
        {
            Debug.Assert(target != null);

            target?.SetValue(ZoneNameProperty, value);
        }
        public static string? GetZoneName(DependencyObject target)
        {
            Debug.Assert(target != null);

            return target?.GetValue(ZoneNameProperty) as string;
        }
        private static bool IsInDesignMode(DependencyObject element)
        {
#if HAS_WINUI
            return Windows.ApplicationModel.DesignMode.DesignModeEnabled;
#else
            return DesignerProperties.GetIsInDesignMode(element);
#endif
        }
        private static void OnZoneNameChanged(DependencyObject element, DependencyPropertyChangedEventArgs args)
        {

            if (IsInDesignMode(element)) return;
            string? zoneName = GetZoneName(element);

            if (string.IsNullOrEmpty(zoneName)) return;


            Zones.AddOrUpdate(zoneName, v => element, (v, current) => element);

        }
    }
}
