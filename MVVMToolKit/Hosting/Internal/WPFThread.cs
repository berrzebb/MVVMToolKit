using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.VisualStudio.Threading;
using MVVMToolKit.Hosting.Core;
using MVVMToolKit.Hosting.Extensions;
using System;
using System.Threading;
using System.Windows;
using System.Windows.Threading;

namespace MVVMToolKit.Hosting.Internal
{
    internal class WPFThread<TApplication>
        : IWPFThread<TApplication> where TApplication : Application, IApplicationInitializeComponent
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly WPFContext<TApplication> _wpfContext;
        private Action<WPFContext<TApplication>>? _preContextInitialization;
        private bool _initializationLocked;
        private SynchronizationContext? _synchronizationContext;
        IWPFContext IWPFThread.WPFContext => _wpfContext;

        public IWPFContext<TApplication> WPFContext => _wpfContext;

        public Thread MainThread { get; }

        public SynchronizationContext SynchronizationContext => _synchronizationContext ?? throw new InvalidOperationException("WPF Thread was not started.");

        public JoinableTaskFactory? JoinableTaskFactory => _serviceProvider.GetService<JoinableTaskFactory>();

        /// <summary>
        /// 생성자
        /// </summary>
        /// <param name="serviceProvider">IServiceProvider</param>
        /// <param name="wpfContext">WPFContext</param>
        public WPFThread(IServiceProvider serviceProvider, WPFContext<TApplication> wpfContext)
        {
            _serviceProvider = serviceProvider;
            _wpfContext = wpfContext;
            //WPF UI Thread를 생성합니다.
            MainThread = new Thread(InternalUiThreadStart)
            {
                Name = "WPF Main UI Thread",
                IsBackground = true
            };
            // apartment state를 설정합니다.
            MainThread.SetApartmentState(ApartmentState.STA);
        }

        /// <inheritdoc />
        public void Start()
        {
            MainThread.Start();
        }

        /// <inheritdoc />
        public void HandleApplicationExit()
        {
            if (!_wpfContext.IsRunning)
            {
                return;
            }

            // 활성창이 열려있는 상태에서 StopApplication을 호출하면 예외가 발생합니다.
            // UIElement의 Visibility에 관련된 특정 케이스에서 발생합니다.
            // 예를 들면 트레이를 통해 HandleApplicationExit가 수동으로 호출된 경우, StopApplication이 발생합니다.
            // 이를 방지하기 위하여 StopApplication을 호출하기 전에 열려있는 모든 윈도우를 닫습니다.
            _wpfContext.WPFApplication.CloseAllWindowsIfAny();

            var applicationLifeTime = _serviceProvider.GetService<IHostApplicationLifetime>();
            applicationLifeTime?.StopApplication();
        }

        /// <summary>
        /// UI Thread를 시작합니다.
        /// </summary>
        private void InternalUiThreadStart()
        {
            PreUIThreadStart();
            // Run the actual code
            UIThreadStart();
        }

        /// <summary>
        /// UI Thread가 시작하기 전 사전 작업을 진행합니다.
        /// </summary>
        private void PreUIThreadStart()
        {
            //SynchronizationContext를 생성합니다.
            var synchronizationContext = new DispatcherSynchronizationContext(Dispatcher.CurrentDispatcher);
            SynchronizationContext.SetSynchronizationContext(new DispatcherSynchronizationContext(Dispatcher.CurrentDispatcher));
            _synchronizationContext = synchronizationContext;

            var application = CreateApplication();

            // 현재 WPF Context에서 생명주기를 관리하고 있지 않고 있다면, 생명 주기를 관리하기 위해 이벤트를 등록합니다.
            if (_wpfContext.ExitHandler == null)
            {
                // 호스트 응용프로그램이 종료 되었을때 발생하는 이벤트입니다.
                application.Exit += (_, _) =>
                {
                    HandleApplicationExit();
                };
            }
            else
            {
                application.Exit += _wpfContext.ExitHandler;
            }

            // Context에서 현재 응용프로그램을 접근 할 수 있도록 설정합니다.
            _wpfContext.SetWPFApplication(application);
            //기본 응용프로그램의 객체들을 초기화 합니다.
            application.InitializeComponent();
            _preContextInitialization?.Invoke(_wpfContext);

        }

        /// <summary>
        /// UI Thread를 시작합니다.
        /// </summary>
        private void UIThreadStart()
        {
            // 응용프로그램이 시작되었다고 체크합니다.
            _wpfContext.IsRunning = true;

            // WPF Component 인터페이스가 정의된 객체들을 Dispose목록에 등록합니다.
            var wpfComponentsFunc = _serviceProvider.GetServices<Func<IWPFComponent>>();
            using var disposableList = new DisposableList<IWPFComponent>();
            foreach (var wpfComponentFunc in wpfComponentsFunc)
            {
                var wpfComponent = wpfComponentFunc.Invoke();
                wpfComponent.InitializeComponent();
                disposableList.Add(wpfComponent);
            }

            //WPF 응용프로그램을 시작합니다. 해당 작업은 Blocking 작업입니다.
            _wpfContext.WPFApplication?.Run();
        }

        private TApplication CreateApplication()
        {
            var applicationFunction = _serviceProvider.GetRequiredService<Func<TApplication>>();
            return applicationFunction.Invoke();
        }

        /// <summary>
        /// Pre initialization that happens before <see cref="Application.Run()"/>. This action happens on UI thread.
        /// </summary>
        internal void SetPreContextInitialization(Action<IWPFContext<TApplication>> preContextInitialization)
        {
            if (_initializationLocked)
            {
                throw new InvalidOperationException($"Do not use .UseWpfInitialization<{typeof(TApplication).Name}> and .UseWpfViewModelLocator<{typeof(TApplication).Name}> together or call them multiple times.");
            }
            _preContextInitialization = preContextInitialization;
            _initializationLocked = true;
        }
    }
}
