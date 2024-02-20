using Microsoft.Extensions.DependencyInjection;

namespace MVVMToolKit.Hosting.Internal
{
    internal sealed class LazyService<T> : Lazy<T> where T : class
    {
        public LazyService(IServiceScopeFactory scopeFactory) : base(() =>
        {
            var scope = scopeFactory.CreateScope();
            return scope.ServiceProvider.GetRequiredService<T>();
        })
        {

        }
    }
}
