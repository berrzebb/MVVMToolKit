namespace MVVMToolKit.Navigation.Mapping
{
    public interface IMappingConfiguration
    {
        string RouteName { get; }
        Type? ContextType { get; }
        string? ViewName { get; }
        ViewCacheMode CacheMode { get; }

        ViewMode ViewMode { get; }
    }
}
