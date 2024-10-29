using Microsoft.Extensions.DependencyInjection;
using MVVMToolKit.Interfaces;
namespace DataModule
{
    public class DataModule : IModule
    {
        public Task InitializeModule(IServiceProvider? provider)
        {
            return Task.CompletedTask;
        }

        public void RegisterTypes(IServiceCollection services)
        {
            services.AddSingleton<IAnimalData, AnimalData>();
            services.AddSingleton<IAnimalService, AnimalService>();
        }
    }
}
