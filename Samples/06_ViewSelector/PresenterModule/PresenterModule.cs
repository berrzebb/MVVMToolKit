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
            services.RegisterService<AnimalInformationView>();
            services.RegisterService<AnimalInfomationViewModel>();
            services.RegisterService<TextView>();
            services.RegisterService<TextWithImageView>();
        }
    }
}
