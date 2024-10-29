namespace MVVMToolKit.Interfaces
{
    /// <summary>
    /// Navigation 작업의 결과
    /// </summary>
    public class NavigationResult
    {
        /// <summary>
        /// Navigation 성공 여부.
        /// </summary>
        public bool IsSuccess { get; private set; }
        /// <summary>
        /// Navigation 상세 정보.
        /// </summary>
        public string? Detail { get; private set; }
        /// <summary>
        /// Navigation 작업의 결과
        /// </summary>
        /// <param name="isSuccess">성공 여부</param>
        /// <param name="detail">Navigation 상세 정보</param>
        public NavigationResult(bool isSuccess = true, string detail = "")
        {
            IsSuccess = isSuccess;
            Detail = detail;
        }
    }
    /// <summary>
    /// Zone과 Route를 이용하여 화면의 이동을 수행합니다.
    /// </summary>
    public interface IZoneNavigator
    {
        /// <summary>
        /// 입력된 Zone에 Route를 변경합니다.
        /// </summary>
        /// <param name="zoneName">변경될 Zone Name.</param>
        /// <param name="routeKey">Route의 Key.</param>
        /// <param name="contextType">화면에서 사용할 Data Context Type(ViewModel).</param>
        /// <returns><seealso cref="NavigationResult">Navigation 수행 결과</seealso></returns>
        NavigationResult? Navigate(string zoneName, object? routeKey = null, Type? contextType = null);
    }
}
