using Microsoft.Extensions.Logging;
using MVVMToolKit.Hosting.Core;
using MVVMToolKit.Hosting.Extensions;
using MVVMToolKit.Hosting.Locator;
using MVVMToolKitSample.Locator;
using System.Windows;

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
        }
        public void Initialize() { }
        public void InitializeLocator(IViewModelLocator viewModelLocator)
        {
            var viewModelLocatorHost = ViewModelLocatorHost.GetInstance(this);
            viewModelLocatorHost?.SetViewModelLocator(viewModelLocator);
        }
    }
}
