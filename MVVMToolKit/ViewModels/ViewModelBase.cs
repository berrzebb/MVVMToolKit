using System;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Messaging;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using MVVMToolKit.Hosting.Core;
using MVVMToolKit.Hosting.Internal;

namespace MVVMToolKit.ViewModels
{
    public abstract class ViewModelBase<TViewModel> : ViewModelBase where TViewModel : ViewModelBase
    {
        protected IMessenger Messenger => WeakReferenceMessenger.Default;
        protected readonly ILogger<TViewModel> Logger;

        protected ViewModelBase(IServiceProvider provider)
        {
            _provider = provider;
            disposableObjectService = provider.GetRequiredService<IDisposableObjectService>();
            Logger = provider.GetRequiredService<ILogger<TViewModel>>();
            InitializeDependency(provider);
        }

        protected override void InitializeDependency(IServiceProvider containerProvider)
        {
        }
    }
    public abstract class ViewModelBase : ObservableObject, IWPFViewModel
    {
        public Guid Guid { get; set; }
        protected IDisposableObjectService? disposableObjectService;

        protected IServiceProvider? _provider = null;
        private bool disposedValue;
        private readonly DisposableList<IDisposable> _disposables = new DisposableList<IDisposable>();

        protected IWPFViewModel Add(IDisposable disposable)
        {
            _disposables.Add(disposable);
            return this;
        }
        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: 관리형 상태(관리형 개체)를 삭제합니다.
                }

                // TODO: 비관리형 리소스(비관리형 개체)를 해제하고 종료자를 재정의합니다.
                // TODO: 큰 필드를 null로 설정합니다.
                _disposables.Dispose();
                disposableObjectService?.Remove(this);
                disposedValue = true;
            }
        }

        // // TODO: 비관리형 리소스를 해제하는 코드가 'Dispose(bool disposing)'에 포함된 경우에만 종료자를 재정의합니다.
        // ~ViewModelBase()
        // {
        //     // 이 코드를 변경하지 마세요. 'Dispose(bool disposing)' 메서드에 정리 코드를 입력합니다.
        //     Dispose(disposing: false);
        // }

        public void Dispose()
        {
            // 이 코드를 변경하지 마세요. 'Dispose(bool disposing)' 메서드에 정리 코드를 입력합니다.
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
        protected abstract void InitializeDependency(IServiceProvider containerProvider);

        void IWPFViewModel.InitializeDependency(IServiceProvider containerProvider)
        {
            InitializeDependency(containerProvider);
        }
    }
}
