using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;

namespace MVVMToolKit.Interfaces
{
    public interface IModule
    {
        void ConfigureServices(IServiceCollection services);
        void ConfigureMappings(IMappingBuilder builder);
        Task Initialize(IServiceProvider? provider);
    }
}
