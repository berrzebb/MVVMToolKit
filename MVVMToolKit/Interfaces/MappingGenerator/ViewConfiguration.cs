namespace MVVMToolKit.Interfaces
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
    public class ViewConfiguration<TViewModel> : IViewConfiguration<TViewModel>
    where TViewModel : INotifyPropertyChanged
    {
        public string? ViewType { get; set; } = default;

        public Func<TViewModel, string?>? ViewSelector { get; set; } = null;

        public ViewMode ViewMode { get; set; } = ViewMode.Single;

        /// <inheritdoc />
        Func<INotifyPropertyChanged, string?>? IViewConfiguration.ViewSelector => ViewSelector == null ? null : v => ViewSelector.Invoke((TViewModel)v);

        public ViewCacheMode CacheMode { get; set; } = ViewCacheMode.DependencyInjection;

    }

}
