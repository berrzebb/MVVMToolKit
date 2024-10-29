using System.Windows;

namespace MVVMToolKit.Navigation.Mapping.Internals
{
    /// <summary>
    /// 경로 저장소 인터페이스
    /// </summary>
    public interface IRouteRegistry
    {
        /// <summary>
        /// 경로 키를 입력받아 경로에 맞는 화면을 반환합니다.
        /// </summary>
        /// <param name="routeKey">경로 키</param>
        /// <returns>화면 Template</returns>
        DataTemplate? this[DataTemplateKey routeKey] { get; }
    }
}
