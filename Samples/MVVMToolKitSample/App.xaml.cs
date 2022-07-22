using Microsoft.Extensions.DependencyInjection;
using MVVMToolKit.Hosting.Extensions;
using MVVMToolKitSample.ViewModels;
using MVVMToolKitSample.Views;
using System;

namespace MVVMToolKitSample
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App
    {

        private App() : this(null!)
        {
            this.CheckForInvalidConstructorConfiguration();
        }
        public App(IServiceProvider provider) : base(provider)
        {
            this.CheckForInvalidConstructorConfiguration();
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
            InitializeBootstrapper<MainWindow>();
        }
    }
}
