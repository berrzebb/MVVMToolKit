

namespace MVVMToolKit
{
    using Microsoft.Extensions.Logging;
    using MVVMToolKit.Ioc;

    /// <summary>
    /// The popupContext model base class.
    /// </summary>
    /// <seealso cref="ViewModelBase"/>
    public abstract class ViewModelBase<TViewModel> : ViewModelBase
        where TViewModel : ViewModelBase
    {
        /// <summary>
        /// The logger.
        /// </summary>
        protected ILogger<TViewModel> Logger;

        /// <summary>
        /// ViewModelBase 생성자
        /// </summary>
        protected ViewModelBase()
        {
            Logger = ContainerProvider.Resolve<ILogger<TViewModel>>()!;
            Logger.LogDebug($"{typeof(TViewModel).Name} InitializeModule.");
            Initialize(ContainerProvider.Resolve<IServiceProvider>()!);
        }

        private void Initialize(IServiceProvider provider)
        {
            InitializeDependency(provider);
        }
    }
}
