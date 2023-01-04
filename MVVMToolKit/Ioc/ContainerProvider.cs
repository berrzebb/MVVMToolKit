using Microsoft.Extensions.DependencyInjection;

namespace MVVMToolKit.Ioc
{
    /// <summary>
    /// The container provider class
    /// </summary>
    public class ContainerProvider
    {
        /// <summary>
        /// The provider
        /// </summary>
        internal static IServiceProvider? provider = null;
        
        /// <summary>
        /// Resolves the type
        /// </summary>
        /// <param name="type">The type</param>
        /// <returns>The object</returns>
        public static object? Resolve(Type? type)
        {
            if (type == null || provider == null)
            {
                return null;
            }

            return provider.GetRequiredService(type);
        }

        /// <summary>
        /// Resolves
        /// </summary>
        /// <typeparam name="T">The </typeparam>
        /// <returns>The</returns>
        public static T? Resolve<T>()
        {
            Type type = typeof(T);

            return (T?)Resolve(type);
        }
    }
}
