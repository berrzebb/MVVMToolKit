using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using MVVMToolKit.Hosting.Core;
using MVVMToolKit.Hosting.Extensions;
using Prism.Ioc;
using System;
using System.Windows;
using System.Windows.Threading;

namespace MVVMToolKit.Hosting
{
    public abstract class GenericHostPrismApplication : Application, IApplicationInitialize, IApplicationInitializeComponent
    {
        protected Bootstrapper? _bootstrapper = null;
        protected readonly IServiceProvider _provider;
        private IServiceCollection _serviceCollection;
        protected readonly ILogger<GenericHostPrismApplication> _logger;
        public GenericHostPrismApplication(IServiceProvider provider)
        {
            this.CheckForInvalidConstructorConfiguration();
            _provider = provider;
            _logger = _provider.GetRequiredService<ILogger<GenericHostPrismApplication>>();
            _serviceCollection = _provider.GetRequiredService<IServiceCollection>();
            Dispatcher.UnhandledException += Dispatcher_UnhandledException;
            Dispatcher.UnhandledExceptionFilter += Dispatcher_UnhandledExceptionFilter;
        }
        public void InitializeBootstrapper<TMainWindow>() where TMainWindow : Window
        {
            _logger.LogInformation($"Starting Prism Bootstrapper");
            _bootstrapper = new Bootstrapper<TMainWindow>(_serviceCollection);
            _bootstrapper._RegisterTypes = RegisterTypes;
            _bootstrapper.Run();
            _logger.LogInformation($"Prism Bootstrapper started.");
        }
        protected virtual void RegisterTypes(IContainerRegistry containerRegistry)
        {

        }
        private void Dispatcher_UnhandledExceptionFilter(object sender, DispatcherUnhandledExceptionFilterEventArgs e)
        {
            try
            {
                e.RequestCatch = true;

                _logger.LogError(e.Exception, $"[App Error Catch] {e.Exception}");
                _logger.LogError(e.Exception, $"[App_DispatcherUnhandledExceptionFilter] {e.Exception.Message}");
            }
            catch { }
        }

        private void Dispatcher_UnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
        {
            try
            {
                e.Handled = true;

                _logger.LogError(e.Exception, $"[App Error Catch] {e.Exception}");
                _logger.LogError(e.Exception, $"[App_DispatcherUnhandledException] {e.Exception.Message}");
            }
            catch { }

        }

        public abstract void Initialize();

        void IApplicationInitializeComponent.InitializeComponent() { }
    }
}
