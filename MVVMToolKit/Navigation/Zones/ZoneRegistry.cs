using System.Collections.Concurrent;
using System.ComponentModel;
using System.Windows;

namespace MVVMToolKit.Navigation.Zones
{
    /// <summary>
    /// Zone 의 저장소 인터페이스
    /// </summary>
    public interface IZoneRegistry
    {
        /// <summary>
        /// Zone Name에 해당하는 View 영역을 반환합니다.
        /// </summary>
        /// <param name="zoneName">Zone Name</param>
        /// <returns>뷰 영역</returns>
        DependencyObject? this[string zoneName] { get; }
    }
    /// <inheritdoc/>
    public class ZoneRegistry : IZoneRegistry
    {
        private static readonly ConcurrentDictionary<string, DependencyObject> zones = new();
        /// <summary>
        /// ZoneName을 설정할 수 있는 속성
        /// </summary>
        public static readonly DependencyProperty ZoneNameProperty = DependencyProperty.RegisterAttached("ZoneName", typeof(string), typeof(ZoneRegistry), new PropertyMetadata(defaultValue: null, propertyChangedCallback: OnZoneNameChanged));

        DependencyObject? IZoneRegistry.this[string zoneName] => zones.GetValueOrDefault(zoneName);

        /// <summary>
        /// ZoneName을 설정합니다.
        /// </summary>
        /// <param name="target">설정할 객체.</param>
        /// <param name="value">Zone Name</param>
        public static void SetZoneName(DependencyObject target, object value)
        {
            target.SetValue(ZoneNameProperty, value);
        }
        /// <summary>
        /// Zone Name을 가져옵니다.
        /// </summary>
        /// <param name="target">가져올 객체</param>
        /// <returns>Zone Name</returns>
        public static string? GetZoneName(DependencyObject target)
        {

            return target.GetValue(ZoneNameProperty) as string;
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

            if (IsInDesignMode(element))
                return;
            string? zoneName = GetZoneName(element);

            if (string.IsNullOrEmpty(zoneName))
                return;

            _ = zones.AddOrUpdate(zoneName, _ => element, (_, _) => element);

        }
    }
}
