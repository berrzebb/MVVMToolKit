using System;
using System.Windows;
using System.Windows.Controls;
using MVVMToolKit.Interfaces;
using MVVMToolKit.Ioc;
using MVVMToolKit.Navigation.Mapping.Internals;
using MVVMToolKit.Navigation.Zones;

namespace MVVMToolKit.Navigation.Views
{
    /// <inheritdocs/>
    public class ZoneNavigator : IZoneNavigator
    {
        private readonly IDispatcherService _dispatcherService;
        private readonly IZoneRegistry _zoneRegistry;
        private readonly IRouteRegistry _routeRegistry;
        /// <summary>
        /// Zone Navigator 생성자.
        /// </summary>
        /// <param name="dispatcherService">UI에 접근하기 위한 Service</param>
        /// <param name="zoneRegistry">zone들의 정보가 등록되어 있는 Registry</param>
        /// <param name="routeRegistry">Route들의 정보가 등록되어 있는 Registry</param>
        public ZoneNavigator(IDispatcherService dispatcherService, IZoneRegistry zoneRegistry, IRouteRegistry routeRegistry)
        {
            _dispatcherService = dispatcherService;
            _zoneRegistry = zoneRegistry;
            _routeRegistry = routeRegistry;
        }

        static void UpdateTemplate(DependencyObject target, DataTemplate? template)
        {
            switch (target)
            {
                case ContentControl control:
                    {
                        control.ContentTemplate = template;
                        return;
                    }
                case ContentPresenter presenter:
                    {
                        presenter.ContentTemplate = template;
                        return;
                    }
                default:
                    return;
            }
        }
        static void UpdateContent(DependencyObject target, object? content)
        {
            switch (target)
            {
                case ContentControl control:
                    {
                        control.Content = content;
                        return;
                    }
                case ContentPresenter presenter:
                    {
                        presenter.Content = content;
                        return;
                    }
                default:
                    return;
            }
        }
        /// <inheritdocs/>
        public NavigationResult? Navigate(string zoneName, object? routeKey = null, Type? contextType = null)
        {

            return _dispatcherService?.Invoke(() =>
            {

                DependencyObject? zone = _zoneRegistry[zoneName];

                if (zone == null)
                    return new NavigationResult(false, detail: $"[ZoneNavigator] {zoneName}에 해당하는 zone을 찾을 수 없습니다.");

                if (routeKey == null)
                {
                    UpdateTemplate(zone, null);
                    UpdateContent(zone, null);
                    return new NavigationResult(detail: $"[ZoneNavigator] {zoneName}의 화면이 초기화 되었습니다.");
                }

                DataTemplateKey dataTemplateKey = new(routeKey);

                DataTemplate? route = _routeRegistry[dataTemplateKey];

                if (route == null)
                    return new NavigationResult(false, detail: $"[ZoneNavigator] {routeKey}에 해당하는 Route를 찾을 수 없습니다.");

                UpdateTemplate(zone, route);

                contextType ??= route.DataType as Type;
                contextType ??= routeKey as Type;
                if (contextType != null)
                {
                    object? instance = ContainerProvider.Resolve(contextType);
                    if (instance is INotifyPropertyChanged viewModel)
                    {
                        UpdateContent(zone, viewModel);
                    }
                }

                return new NavigationResult(detail: $"[ZoneNavigator] {zoneName}의 화면이 {routeKey}에 해당되는 화면으로 갱신 되었습니다.");
            });
        }
    }
}
