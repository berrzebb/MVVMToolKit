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
        internal static IServiceProvider? Provider = null;

        /// <summary>
        /// Resolves the type.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns>The object.</returns>
        public static object? Resolve(Type? type)
        {
            if (type == null || Provider == null)
            {
                return null;
            }

            return Provider.GetRequiredService(type);
        }

        /// <summary>
        /// Resolves.
        /// </summary>
        /// <typeparam name="T">The type.</typeparam>
        /// <returns>The object.</returns>
        public static T? Resolve<T>()
        {
            Type type = typeof(T);

            return (T?)Resolve(type);
        }
    }
}
