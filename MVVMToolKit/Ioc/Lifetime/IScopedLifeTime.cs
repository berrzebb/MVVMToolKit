using Microsoft.Extensions.DependencyInjection;

namespace MVVMToolKit.Ioc.Lifetime
{
    public interface IScopedLifeTime : ILifeTime
    {
        new ServiceLifetime Lifetime => ServiceLifetime.Scoped;
    }
}
