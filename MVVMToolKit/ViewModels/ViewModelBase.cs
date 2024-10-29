namespace MVVMToolKit
{
    using System.ComponentModel;
    using CommunityToolkit.Mvvm.ComponentModel;
    using CommunityToolkit.Mvvm.Messaging;
    using MVVMToolKit.Hosting.Core;
    using MVVMToolKit.Hosting.Internal;
    using MVVMToolKit.Interfaces;
    using MVVMToolKit.Ioc;

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
        /// Disposable 객체 관리 서비스
        /// </summary>
        protected readonly IDisposableObjectService disposableObjectService;
        /// <summary>
        /// UI Dispatcher 접근 서비스
        /// </summary>
        protected IDispatcherService dispatcherService;

        /// <summary>
        /// The disposed value.
        /// </summary>
        private bool _disposedValue;

        /// <summary>
        /// The disposable.
        /// </summary>
        private readonly DisposableList<IDisposable> _disposables = new();
        /// <summary>
        /// Weak Reference Messenger
        /// </summary>
        protected new WeakReferenceMessenger Messenger => (base.Messenger as WeakReferenceMessenger)!;
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
        /// ViewModel Base 생성자
        /// </summary>
        protected ViewModelBase()
        {
            disposableObjectService = ContainerProvider.Resolve<IDisposableObjectService>()!;
            dispatcherService = ContainerProvider.Resolve<IDispatcherService>()!;
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
                disposableObjectService.Remove(this);
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
        protected virtual void InitializeDependency(IServiceProvider containerProvider)
        {

        }
        /// <summary>
        /// 입력된 ViewName을 통해 Navigate를 수행합니다.<br/>
        /// 해당 메서드는 <see cref="IViewSelector"/>인터페이스가 구현되어야 사용될 수 있습니다.
        /// </summary>
        /// <param name="viewName">이동할 ViewName</param>
        public void NavigateTo(string viewName)
        {
            if (this is not IViewSelector viewSelector)
            {
                throw new InvalidOperationException($"NavigateTo 메서드를 수행하기 위해 필요한 IViewSelector 인터페이스가 구현되어 있지 않습니다.");

            }
            IViewSelector.NavigateTo(viewSelector, viewName);
        }
        /// <summary>
        /// Initializes the dependency using the specified container provider.
        /// </summary>
        /// <param name="containerProvider">The container provider.</param>
        void IWpfViewModel.InitializeDependency(IServiceProvider containerProvider) => InitializeDependency(containerProvider);

        protected override void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            dispatcherService.InvokeAsync(() =>
            {
                base.OnPropertyChanged(e);
            });
        }
        protected override void OnPropertyChanging(PropertyChangingEventArgs e)
        {
            dispatcherService.InvokeAsync(() =>
            {
                base.OnPropertyChanging(e);
            });
        }
    }
}
