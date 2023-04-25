using Microsoft.Extensions.DependencyInjection;

namespace MVVMToolKit.Ioc.Lifetime
{
    public interface ITransientLifetime : ILifeTime
    {
        new ServiceLifetime Lifetime => ServiceLifetime.Transient;
    }
}
