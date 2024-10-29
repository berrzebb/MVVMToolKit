using System;
using MVVMToolKit.Navigation.Mapping;

namespace MVVMToolKit.Attributes
{
    /// <summary>
    /// 탐색 가능 속성
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    public class NavigatableAttribute : Attribute
    {
        /// <summary>
        /// 경로 명
        /// </summary>
        public string RouteName { get; init; }
        /// <summary>
        /// 화면 명
        /// </summary>
        public string ViewName { get; init; }
        /// <summary>
        /// 화면 전시 모드
        /// </summary>
        public ViewMode ViewMode { get; init; }
        /// <summary>
        /// 화면 캐시 모드
        /// </summary>
        public ViewCacheMode CacheMode { get; init; }

        /// <summary>
        /// <see cref="NavigatableAttribute"/> 생성자
        /// </summary>
        /// <param name="viewName">보여져야할 화면의 이름</param>
        public NavigatableAttribute(string viewName) : this(viewName, string.Empty) { }

        /// <summary>
        /// <see cref="NavigatableAttribute"/> 생성자
        /// </summary>
        /// <param name="viewName">보여져야할 화면의 이름</param>
        /// <param name="routeName">변경되어야 할 경로의 이름</param>
        public NavigatableAttribute(string viewName, string routeName)
        {
            ViewName = viewName;
            RouteName = routeName;
        }
        /// <summary>
        /// <see cref="NavigatableAttribute"/> 생성자
        /// </summary>
        public NavigatableAttribute() : this(string.Empty, string.Empty) { }
    }
}
