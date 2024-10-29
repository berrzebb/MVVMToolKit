using Microsoft.Extensions.DependencyInjection;
using MVVMToolKit.Interfaces;
using NumericModule.Service;

namespace NumericModule
{
    public class NumericModule : IModule
    {
        public Task InitializeModule(IServiceProvider? provider)
        {
            return Task.CompletedTask;
        }

        public void RegisterTypes(IServiceCollection services)
        {
            services.AddSingleton<ICurrencyData, CurrencyData>();
            services.AddSingleton<INumericDataService, NumericDataService>();
            services.AddSingleton<IDataController, DataController>();
        }
    }
}
