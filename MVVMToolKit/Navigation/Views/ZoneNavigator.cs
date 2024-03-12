namespace MVVMToolKit.Navigation.Views
{
    using System.Windows.Controls;
    using Interfaces;
    using Ioc;
    using Mapping.Internals;
    using Zones;

    public class ZoneNavigator : IZoneNavigator
    {
        private readonly IZoneRegistry _zoneRegistry;
        private readonly IRouteRegistry _routeRegistry;

        public ZoneNavigator(IZoneRegistry zoneRegistry, IRouteRegistry routeRegistry)
        {
            _zoneRegistry = zoneRegistry;
            _routeRegistry = routeRegistry;
        }
        public NavigationResult Navigate(string zoneName, object routeKey, Type? contextType = null)
        {

            DependencyObject? zone = _zoneRegistry[zoneName];


            if (zone == null) return new NavigationResult(false, $"[ZoneNavigator] {zoneName}에 해당하는 zone을 찾을 수 없습니다.");
            if (zone is ContentControl currentZone)
            {
                DataTemplateKey dataTemplateKey = new(routeKey);

                DataTemplate? route = _routeRegistry[dataTemplateKey];

                if (route == null) return new NavigationResult(false, $"[ZoneNavigator] {routeKey}에 해당하는 Route를 찾을 수 없습니다.");

                currentZone.ContentTemplate = route;

                contextType ??= route.DataType as Type;
                contextType ??= routeKey as Type;
                if (contextType != null)
                {
                    object? instance = ContainerProvider.Resolve(contextType);
                    if (instance is INotifyPropertyChanged viewModel)
                    {
                        currentZone.Content = viewModel;

                    }
                }
            }
            return new NavigationResult();
        }
    }
}
