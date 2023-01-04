namespace MVVMToolKit.Hosting.Core
{
    /// <summary>
    /// The iwpf view model interface
    /// </summary>
    /// <seealso cref="IDisposableObject"/>
    public interface IWPFViewModel : IDisposableObject
    {
        /// <summary>
        /// Initializes the dependency using the specified container provider
        /// </summary>
        /// <param name="containerProvider">The container provider</param>
        void InitializeDependency(IServiceProvider containerProvider);
    }
}
