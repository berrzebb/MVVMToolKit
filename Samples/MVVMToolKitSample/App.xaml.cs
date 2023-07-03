using System.Windows;
using Microsoft.Extensions.DependencyInjection;
using MVVMToolKit.Hosting;
using MVVMToolKit.Hosting.Extensions;
using MVVMToolKitSample.ViewModels;
using MVVMToolKitSample.Views;

namespace MVVMToolKitSample
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : GenericHostApplication
    {

        protected override void InitializeServices(IServiceCollection services)
        {
            base.InitializeServices(services);
        }
        protected override void InitializeViews(IServiceCollection services)
        {
            base.InitializeViews(services);
            services.AddView<MainWindow>();
        }
        protected override void InitializeViewModels(IServiceCollection services)
        {
            base.InitializeViewModels(services);
            services.AddViewModel<MainWindowViewModel>();
        }
        public override void Initialize()
        {
            //InitializeBootstrapper<MainWindow>();
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            Application.Current.ShutdownMode = ShutdownMode.OnMainWindowClose;
            var mainWindow = Host?.Services.GetRequiredService<MainWindow>();
            Application.Current.MainWindow = mainWindow;
            mainWindow?.Show();

        }
    }
}
