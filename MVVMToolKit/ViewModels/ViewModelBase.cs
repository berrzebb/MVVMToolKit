using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Messaging;
using MVVMToolKit.Hosting.Core;
using MVVMToolKit.Hosting.Internal;
using MVVMToolKit.Interfaces;

namespace MVVMToolKit.ViewModels
{

    /// <summary>
    /// The popupContext model base class.
    /// </summary>
    /// <seealso cref="ObservableObject"/>
    /// <seealso cref="IWpfViewModel"/>
    public abstract class ViewModelBase : ObservableRecipient, IWpfViewModel
    {
        /// <summary>
        /// Gets or sets the value of the guid.
        /// </summary>
        public Guid Guid { get; set; }

        /// <summary>
        /// The disposable object service.
        /// </summary>
        protected IDisposableObjectService? disposableObjectService;

        protected IDispatcherService? dispatcherService;

        /// <summary>
        /// The provider.
        /// </summary>
        protected IServiceProvider? currentProvider;

        /// <summary>
        /// The disposed value.
        /// </summary>
        private bool _disposedValue;

        /// <summary>
        /// The disposable.
        /// </summary>
        private readonly DisposableList<IDisposable> _disposables = new();

        protected new WeakReferenceMessenger? Messenger => base.Messenger as WeakReferenceMessenger;
        /// <summary>
        /// Adds the disposable.
        /// </summary>
        /// <param name="disposable">The disposable.</param>
        /// <returns>The iwpf popupContext model.</returns>
        protected IWpfViewModel Add(IDisposable disposable)
        {
            _disposables.Add(disposable);
            return this;
        }

        /// <summary>
        /// Disposes the disposing.
        /// </summary>
        /// <param name="disposing">The disposing.</param>
        protected virtual void Dispose(bool disposing)
        {
            if (!_disposedValue)
            {
                _disposables.Dispose();
                disposableObjectService?.Remove(this);
                Cleanup();
                _disposedValue = true;
            }
        }

        /// <summary>
        /// Disposes this instance.
        /// </summary>
        public void Dispose()
        {
            // 이 코드를 변경하지 마세요. 'Dispose(bool disposing)' 메서드에 정리 코드를 입력합니다.
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Cleanups this instance.
        /// </summary>
        public virtual void Cleanup()
        {
            WeakReferenceMessenger.Default.Cleanup();
        }

        /// <summary>
        /// Initializes the dependency using the specified container provider.
        /// </summary>
        /// <param name="containerProvider">The container provider.</param>
        protected abstract void InitializeDependency(IServiceProvider containerProvider);

        /// <summary>
        /// Initializes the dependency using the specified container provider.
        /// </summary>
        /// <param name="containerProvider">The container provider.</param>
        void IWpfViewModel.InitializeDependency(IServiceProvider containerProvider) => InitializeDependency(containerProvider);
    }
}
