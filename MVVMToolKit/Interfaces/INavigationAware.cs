using System.Windows.Navigation;

namespace MVVMToolKit.Interfaces
{
    /// <summary>
    /// The navigation aware interface
    /// </summary>
    public interface INavigationAware
    {
        /// <summary>
        /// Ons the navigating using the specified sender
        /// </summary>
        /// <param name="sender">The sender</param>
        /// <param name="navigationEventArgs">The navigation event args</param>
        void OnNavigating(object? sender, NavigatingCancelEventArgs? navigationEventArgs);
        /// <summary>
        /// Ons the navigated using the specified sender
        /// </summary>
        /// <param name="sender">The sender</param>
        /// <param name="navigationEventArgs">The navigation event args</param>
        void OnNavigated(object? sender, NavigationEventArgs? navigationEventArgs);
    }
}
