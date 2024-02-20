using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Messaging;
using MVVMToolKit.Hosting.Core;
using MVVMToolKit.Hosting.Internal;
using MVVMToolKit.Interfaces;

namespace MVVMToolKit.ViewModels
{

    /// <summary>
    /// The view model base class.
    /// </summary>
    /// <seealso cref="ObservableObject"/>
    /// <seealso cref="IWPFViewModel"/>
    public abstract class ViewModelBase : ObservableRecipient, IWPFViewModel
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
        private bool disposedValue;

        /// <summary>
        /// The disposable.
        /// </summary>
        private readonly DisposableList<IDisposable> disposables = new();

        protected new WeakReferenceMessenger? Messenger => base.Messenger as WeakReferenceMessenger;
        /// <summary>
        /// Adds the disposable.
        /// </summary>
        /// <param name="disposable">The disposable.</param>
        /// <returns>The iwpf view model.</returns>
        protected IWPFViewModel Add(IDisposable disposable)
        {
            this.disposables.Add(disposable);
            return this;
        }

        /// <summary>
        /// Disposes the disposing.
        /// </summary>
        /// <param name="disposing">The disposing.</param>
        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposedValue)
            {
                if (disposing)
                {
                    // TODO: 관리형 상태(관리형 개체)를 삭제합니다.
                }

                // TODO: 비관리형 리소스(비관리형 개체)를 해제하고 종료자를 재정의합니다.
                // TODO: 큰 필드를 null로 설정합니다.
                this.disposables.Dispose();
                this.disposableObjectService?.Remove(this);
                this.Cleanup();
                this.disposedValue = true;
            }
        }

        /// <summary>
        /// Disposes this instance.
        /// </summary>
        public void Dispose()
        {
            // 이 코드를 변경하지 마세요. 'Dispose(bool disposing)' 메서드에 정리 코드를 입력합니다.
            this.Dispose(disposing: true);
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
        void IWPFViewModel.InitializeDependency(IServiceProvider containerProvider) => this.InitializeDependency(containerProvider);
    }
}
