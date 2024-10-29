using Microsoft.Extensions.DependencyInjection;
using MVVMToolKit.Hosting.Extensions;
using MVVMToolKit.Interfaces;
using PresenterModule.ViewModels;
using PresenterModule.Views;

namespace PresenterModule
{
    public class PresenterModule : IModule
    {
        public Task InitializeModule(IServiceProvider? provider)
        {
            return Task.CompletedTask;
        }

        public void RegisterTypes(IServiceCollection services)
        {
            services.RegisterService<ControllerViewModel>();
            services.RegisterService<ControllerView>();
            services.RegisterService<CurrencyViewModel>();
            services.RegisterService<CurrencyView>();
            services.RegisterService<DataViewModel>();
            services.RegisterService<DataView>();
        }
    }
}
