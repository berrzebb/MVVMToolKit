using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using MVVMToolKit.Hosting;
using MVVMToolKit.Hosting.Core;
using MVVMToolKit.Hosting.Extensions;
using MVVMToolKit.Hosting.Locator;
using MVVMToolKitSample.Locator;
using MVVMToolKitSample.Views;
using Prism.Ioc;

namespace MVVMToolKitSample
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : GenericHostPrismApplication, IViewModelLocatorInitialization<IViewModelLocator>, IApplicationInitializeComponent
    {

        private App() : this(null!, null!)
        {
            this.CheckForInvalidConstructorConfiguration();
        }
        public App(ILogger<App> logger, IServiceCollection serviceCollection) : base(logger, serviceCollection)
        {
            this.CheckForInvalidConstructorConfiguration();
        }
        public void Initialize()
        {
            //
        }
        public void InitializeLocator(IViewModelLocator viewModelLocator)
        {
            var viewModelLocatorHost = ViewModelLocatorHost.GetInstance(this);
            viewModelLocatorHost?.SetViewModelLocator(viewModelLocator);
            InitializeBootstrapper<MainWindow>();
        }

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {

        }
    }
}
