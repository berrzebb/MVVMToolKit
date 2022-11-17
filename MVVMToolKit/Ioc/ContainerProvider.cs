using System;
using Microsoft.Extensions.DependencyInjection;

namespace MVVMToolKit.Ioc
{
    internal class ContainerProvider
    {
        internal static IServiceProvider? provider = null;
        
        public static object? Resolve(Type? type)
        {
            if (type == null || provider == null)
            {
                return null;
            }

            return provider.GetRequiredService(type);
        }
    }
}
