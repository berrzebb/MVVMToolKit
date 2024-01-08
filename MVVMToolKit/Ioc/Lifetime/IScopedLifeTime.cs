using Microsoft.Extensions.DependencyInjection;

namespace MVVMToolKit.Ioc.Lifetime
{
    public abstract class ScopedLifeTime : ILifeTime
    {
        public abstract Guid Id { get; }
        public ServiceLifetime Lifetime => ServiceLifetime.Scoped;
    }
}
