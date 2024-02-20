namespace MVVMToolKit.Navigation.Mapping;
public interface IMappingConfiguration
{
    string? RouteName { get; }
    Type? ContextType { get; }
    string? ViewType { get; }
    ViewCacheMode CacheMode { get; }

    ViewMode ViewMode { get; }

    Func<INotifyPropertyChanged, string?>? ViewSelector { get; }
}
public interface IMappingConfiguration<in TViewModel> : IMappingConfiguration
where TViewModel : INotifyPropertyChanged
{
    new Func<TViewModel, string?>? ViewSelector { get; }
}