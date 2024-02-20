using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using MVVMToolKit.Navigation.Mapping;

namespace MVVMToolKit.Interfaces
{
    public interface IModule
    {
        void ConfigureServices(IServiceCollection services);
        void ConfigureMappings(IMappingRegistry registry);
        Task Initialize(IServiceProvider? provider);
    }
}
