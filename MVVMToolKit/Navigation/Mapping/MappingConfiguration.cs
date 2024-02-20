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
        public Func<INotifyPropertyChanged, string?>? ViewSelector { get; set; } = null;

    }
    public class MappingConfiguration<TViewModel> : MappingConfiguration, IMappingConfiguration<TViewModel>
    where TViewModel : INotifyPropertyChanged
    {

        public MappingConfiguration() => ContextType = typeof(TViewModel);

        /// <inheritdoc />
        Func<INotifyPropertyChanged, string?>? IMappingConfiguration.ViewSelector => ViewSelector == null ? null : v => ViewSelector.Invoke((TViewModel)v);
        public Func<TViewModel, string?>? ViewSelector { get; set; } = null;

    }

}
