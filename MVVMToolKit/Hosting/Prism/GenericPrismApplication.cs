using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using MVVMToolKit.Hosting.Extensions;
using Prism.Ioc;
using System.Windows;
using System.Windows.Threading;

namespace MVVMToolKit.Hosting
{
    public abstract class GenericHostPrismApplication : Application
    {
        protected Bootstrapper? _bootstrapper = null;
        protected readonly IServiceCollection _serviceCollection;
        protected readonly ILogger<GenericHostPrismApplication> _logger;
        public GenericHostPrismApplication(ILogger<GenericHostPrismApplication> logger, IServiceCollection serviceCollection)
        {
            this.CheckForInvalidConstructorConfiguration();
            _logger = logger;
            _serviceCollection = serviceCollection;
            Dispatcher.UnhandledException += Dispatcher_UnhandledException;
            Dispatcher.UnhandledExceptionFilter += Dispatcher_UnhandledExceptionFilter;
        }
        public void InitializeBootstrapper<TMainWindow>() where TMainWindow : Window
        {
            _bootstrapper = new Bootstrapper<TMainWindow>(_serviceCollection);
            _bootstrapper._RegisterTypes = RegisterTypes;
            _bootstrapper.Run();
        }
        protected abstract void RegisterTypes(IContainerRegistry containerRegistry);
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
    }
}
