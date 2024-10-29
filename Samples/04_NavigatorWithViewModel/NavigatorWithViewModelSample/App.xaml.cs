using System.Windows;
using Microsoft.Extensions.DependencyInjection;
using MVVMToolKit.Hosting.Extensions;
using NavigatorWithViewModelSample.ViewModels;
using NavigatorWithViewModelSample.Views;

namespace NavigatorWithViewModelSample
{
    public partial class App
    {
        public App()
        {
        }
        protected override void RegisterTypes(IServiceCollection services)
        {
            services.RegisterService<CustomView>();
            services.RegisterService<CustomViewModel>();
            services.RegisterService<SpecificViewModel>();
            services.RegisterService<MainWindowViewModel>();
        }

        protected override void InitializeShell()
        {
            Shell.Show();
        }

        protected override Window CreateShell()
        {
            return new MainWindow();
        }
    }


}
