namespace MVVMToolKit.Navigation.Mapping
{
    /// <summary>
    /// 화면 전시 모드
    /// </summary>
    public enum ViewMode
    {
        /// <summary>
        /// 단독 모드(Default)
        /// </summary>
        Single,
        /// <summary>
        /// 선택 모드(<seealso cref="IViewSelector"/>)
        /// </summary>
        Selector
    }
    /// <summary>
    /// 화면 캐시 모드
    /// </summary>
    public enum ViewCacheMode
    {
        /// <summary>
        /// 캐싱(Local Cache)
        /// </summary>
        Cached,
        /// <summary>
        /// 캐싱 안함
        /// </summary>
        NonCached,
        /// <summary>
        /// <seealso href="https://ko.wikipedia.org/wiki/%EC%9D%98%EC%A1%B4%EC%84%B1_%EC%A3%BC%EC%9E%85">Dependency Injection</seealso>을 통해 가져오기
        /// </summary>
        DependencyInjection,
    }

    /// <summary>
    /// Mapping Configuration
    /// </summary>
    public class MappingConfiguration : IMappingConfiguration
    {
        /// <summary>
        /// MappingConfiguration 생성자
        /// </summary>
        /// <param name="routeName">Route 명칭</param>
        /// <param name="viewName">View 명칭</param>
        public MappingConfiguration(string routeName = "", string? viewName = "")
        {
            RouteName = routeName;
            ViewName = viewName;
        }

        public string RouteName { get; set; }
        public Type? ContextType { get; init; }
        public string? ViewName { get; set; }


        public ViewMode ViewMode { get; init; } = ViewMode.Single;


        public ViewCacheMode CacheMode { get; init; } = ViewCacheMode.DependencyInjection;

    }

}
