namespace MVVMToolKit.Interfaces;

public interface IViewConfiguration
{
    string? ViewType { get; }
    ViewCacheMode CacheMode { get; }

    ViewMode ViewMode { get; }

    Func<INotifyPropertyChanged, string?>? ViewSelector { get; }
}
public interface IViewConfiguration<in TViewModel> : IViewConfiguration
where TViewModel : INotifyPropertyChanged
{
    new Func<TViewModel, string?>? ViewSelector { get; }
}