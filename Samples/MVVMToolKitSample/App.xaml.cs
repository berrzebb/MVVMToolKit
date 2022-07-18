using Microsoft.Extensions.Logging;
using MVVMToolKit.Hosting.Core;
using MVVMToolKit.Hosting.Extensions;
using MVVMToolKit.Hosting.Locator;
using MVVMToolKitSample.Locator;
using System.Windows;
using System.Windows.Threading;

namespace MVVMToolKitSample
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application, IViewModelLocatorInitialization<IViewModelLocator>, IApplicationInitializeComponent
    {
        private readonly ILogger<App> _logger;
        private App() : this(null!)
        {
            this.CheckForInvalidConstructorConfiguration();

        }
        public App(ILogger<App> logger)
        {
            this.CheckForInvalidConstructorConfiguration();
            _logger = logger;
            Dispatcher.UnhandledException += Dispatcher_UnhandledException;
            Dispatcher.UnhandledExceptionFilter += Dispatcher_UnhandledExceptionFilter;
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

        public void Initialize()
        {
        }
        public void InitializeLocator(IViewModelLocator viewModelLocator)
        {
            var viewModelLocatorHost = ViewModelLocatorHost.GetInstance(this);
            viewModelLocatorHost?.SetViewModelLocator(viewModelLocator);
        }
    }
}
