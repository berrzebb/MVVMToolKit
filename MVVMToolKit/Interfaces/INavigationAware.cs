using System.Windows.Navigation;

namespace MVVMToolKit.Interfaces
{
    public interface INavigationAware
    {
        void OnNavigating(object? sender, NavigatingCancelEventArgs? navigationEventArgs);
        void OnNavigated(object? sender, NavigationEventArgs? navigationEventArgs);
    }
}
