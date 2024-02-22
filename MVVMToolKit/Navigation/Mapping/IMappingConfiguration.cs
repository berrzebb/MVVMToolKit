namespace MVVMToolKit.Navigation.Mapping;
public interface IMappingConfiguration
{
    string RouteName { get; }
    Type? ContextType { get; }
    string? ViewType { get; }
    ViewCacheMode CacheMode { get; }

    ViewMode ViewMode { get; }
}