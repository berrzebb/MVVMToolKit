using System;
using Microsoft.Extensions.DependencyInjection;
using MVVMToolKit.Hosting.Core;
using MVVMToolKit.Hosting.Internal;
using MVVMToolKit.Ioc;

namespace MVVMToolKit.Hosting.Extensions
{
    /// <summary>
    /// The service collection extensions class
    /// </summary>
    public static class ServiceCollectionExtensions
    {
        static readonly Type frameworkElementType = typeof(FrameworkElement);
        static readonly Type viewModelType = typeof(INotifyPropertyChanged);

        internal static readonly List<Type> singletonTypes = new();
        private static bool CheckTransientType(Type type)
        {
            return type.IsSubclassOf(frameworkElementType) || type.IsSubclassOf(viewModelType);
        }

        /// <summary>
        /// 특정 타입을 DI Container에 등록합니다.
        /// </summary>
        /// <param name="services">DI Container</param>
        /// <param name="createService">Service Factory</param>
        /// <param name="lifeTime">DI 생존 주기</param>
        /// <typeparam name="TService">등록할 서비스</typeparam>
        /// <returns>DI Container</returns>
        public static IServiceCollection RegisterService<TService>(this IServiceCollection services, Func<IServiceProvider, TService>? createService = null, ServiceLifetime? lifeTime = null, bool isInitialized = false)
        {
            Type registeredType = typeof(TService);
            ServiceLifetime serviceLifeTime = lifeTime
                ?? (CheckTransientType(registeredType) ?
                ServiceLifetime.Transient : ServiceLifetime.Singleton);
            Func<IServiceProvider, TService> serviceFactory =
                createService ?? (provider => ActivatorUtilities.CreateInstance<TService>(provider));
            object Factory(IServiceProvider provider)
            {

                var service = serviceFactory(provider);
                if (service is IDisposableObject disposable)
                {
                    IDisposableObjectService disposableObjectService = provider.GetRequiredService<IDisposableObjectService>();
                    disposableObjectService.Add(disposable);
                }

                return service!;
            }

            services.Add(new ServiceDescriptor(registeredType, Factory,
                serviceLifeTime));

            TypeProvider.RegisterType(registeredType);
            if (serviceLifeTime == ServiceLifetime.Singleton && isInitialized) singletonTypes.Add(registeredType);
            return services;
        }

        /// <summary>
        /// 지연된 생성을 허용합니다.
        /// </summary>
        /// <param name="services">DI Container</param>
        /// <returns>DI Container</returns>
        public static IServiceCollection AllowLazy(this IServiceCollection services)
        {
            var lastRegistration = services[^1];

            var lazyServiceType = typeof(Lazy<>).MakeGenericType(lastRegistration.ServiceType);

            var lazyServiceImplementationType = typeof(LazyService<>).MakeGenericType(lastRegistration.ServiceType);

            services.Add(new ServiceDescriptor(lazyServiceType, lazyServiceImplementationType, lastRegistration.Lifetime));
            return services;
        }
    }
}
