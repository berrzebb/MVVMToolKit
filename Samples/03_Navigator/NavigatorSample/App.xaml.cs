using System.Windows;
using Microsoft.Extensions.DependencyInjection;
using MVVMToolKit.Hosting.Extensions;
using NavigatorSample.Views;

namespace NavigatorSample
{
    public partial class App
    {
        public App()
        {
        }

        protected override void RegisterTypes(IServiceCollection services)
        {
            services.RegisterService<AView>();
            services.RegisterService<BView>();
        }

        protected override Window CreateShell()
        {
            return new MainWindow();
        }
        protected override void InitializeShell()
        {
            Shell?.Show();
        }
    }


}
