using System.Windows;
using MVVMToolKit.Interfaces;
using MVVMToolKit.Ioc;

namespace MVVMToolKit.Hosting.Extensions
{
    /// <summary>
    /// Application객체의 확장입니다.
    /// </summary>
    public static class ApplicationExtensions
    {
        /// <summary>
        /// 특정 Zone을 입력받은 Route Key에 해당하는 화면으로 전환합니다.
        /// </summary>
        /// <param name="app">현재 사용중인 Application.</param>
        /// <param name="zoneName">전환할 Zone의 이름.</param>
        /// <param name="routeKey">전환할 화면의 키.</param>
        /// <param name="contextType">전환할 화면에서 사용할 DataContext Type</param>
        /// <returns>화면 전환 결과.</returns>
        public static NavigationResult Navigate(this Application app, string zoneName, object? routeKey = null, Type? contextType = null)
        {
            var zoneNavigator = ContainerProvider.Resolve<IZoneNavigator>();
            if (zoneNavigator == null) return new NavigationResult(false, "Zone Navigator가 등록되지 않았습니다.");
            return zoneNavigator.Navigate(zoneName, routeKey, contextType);
        }
    }
}
