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
        public string? RouteName { get; set; }
        public Type? ContextType { get; set; } = null;
        public string? ViewType { get; set; } = default;


        public ViewMode ViewMode { get; set; } = ViewMode.Single;


        public ViewCacheMode CacheMode { get; set; } = ViewCacheMode.DependencyInjection;

    }
    public class MappingConfiguration<TViewModel> : MappingConfiguration
    where TViewModel : INotifyPropertyChanged
    {

        public MappingConfiguration() => ContextType = typeof(TViewModel);
    }

}
