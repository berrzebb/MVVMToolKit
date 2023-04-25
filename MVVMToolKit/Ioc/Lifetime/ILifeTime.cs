using Microsoft.Extensions.DependencyInjection;

namespace MVVMToolKit.Ioc.Lifetime
{
    public interface ILifeTime
    {
        Guid Id { get; }

        ServiceLifetime Lifetime { get; }

    }
}
