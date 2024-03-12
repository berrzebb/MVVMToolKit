namespace MVVMToolKit.Navigation.Mapping
{
    public enum ViewMode
    {
        Single,
        Selector
    }

    public enum ViewCacheMode
    {
        Cached,
        NonCached,
        DependencyInjection,
    }
    public class MappingConfiguration : IMappingConfiguration
    {
        public MappingConfiguration(string routeName = "")
        {
            RouteName = routeName;
        }

        public string RouteName { get; set; }
        public Type? ContextType { get; protected set; }
        public string? ViewType { get; set; } = default;


        public ViewMode ViewMode { get; set; } = ViewMode.Single;


        public ViewCacheMode CacheMode { get; set; } = ViewCacheMode.DependencyInjection;

    }
    public class MappingConfiguration<TViewModel> : MappingConfiguration
    where TViewModel : INotifyPropertyChanged
    {

        public MappingConfiguration(string routeName = "") : base(routeName) => ContextType = typeof(TViewModel);
    }

}
