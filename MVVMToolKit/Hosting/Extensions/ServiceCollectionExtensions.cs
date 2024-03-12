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
        /// <summary>
        /// Adds the popupContext using the specified services
        /// </summary>
        /// <typeparam name="TView">The popupContext</typeparam>
        /// <param name="services">The services</param>
        /// <returns>The service collection.</returns>
        public static IServiceCollection AddView<TView>(this IServiceCollection services)
            where TView : FrameworkElement
        {
            return services.AddView<TView>(ServiceLifetime.Transient);
        }
        public static IServiceCollection AddService<TService>(this IServiceCollection services, Func<IServiceProvider, TService> factory, ServiceLifetime lifetime)
        {
            services.Add(new ServiceDescriptor(typeof(TService), provider =>
            {
                var service = factory(provider);
                if (service is IDisposableObject disposable)
                {
                    IDisposableObjectService disposableObjectService = provider.GetRequiredService<IDisposableObjectService>();
                    disposableObjectService.Add(disposable);
                }
                return service;
            },
                lifetime));

            Type registeredType = typeof(TService);
            TypeProvider.RegisterType(registeredType);
            return services;
        }
        /// <summary>
        /// Adds the popupContext using the specified services
        /// </summary>
        /// <typeparam name="TView">The popupContext</typeparam>
        /// <param name="services">The services</param>
        /// <param name="lifeTime">Service Lifetime.</param>
        /// <returns>The service collection.</returns>
        public static IServiceCollection AddView<TView>(this IServiceCollection services, ServiceLifetime lifeTime)
            where TView : FrameworkElement
        {
            return services.AddView(provider => ActivatorUtilities.CreateInstance<TView>(provider), lifeTime);
        }


        /// <summary>
        /// Adds the popupContext using the specified services
        /// </summary>
        /// <typeparam name="TView">The popupContext</typeparam>
        /// <param name="services">The services</param>
        /// <param name="createView">The creation popupContext</param>
        /// <param name="lifeTime">Service Lifetime.</param>
        /// <returns>The services</returns>
        public static IServiceCollection AddView<TView>(this IServiceCollection services, Func<IServiceProvider, TView> createView, ServiceLifetime lifeTime)
            where TView : FrameworkElement
        {
            return services.AddService(createView, lifeTime);
        }
        /// <summary>
        /// Adds the popupContext model using the specified services
        /// </summary>
        /// <typeparam name="TViewModel">The popupContext model</typeparam>
        /// <param name="services">The services</param>
        /// <returns>The service collection.</returns>
        public static IServiceCollection AddViewModel<TViewModel>(this IServiceCollection services)
            where TViewModel : class, IWpfViewModel
        {
            return services.AddViewModel<TViewModel>(ServiceLifetime.Transient);
        }
        /// <summary>
        /// Adds the popupContext model using the specified services
        /// </summary>
        /// <typeparam name="TViewModel">The popupContext model</typeparam>
        /// <param name="services">The services</param>
        /// <param name="lifeTime">Service Lifetime.</param>
        /// <returns>The service collection.</returns>
        public static IServiceCollection AddViewModel<TViewModel>(this IServiceCollection services, ServiceLifetime lifeTime)
            where TViewModel : class, IWpfViewModel
        {
            return services.AddViewModel(provider => ActivatorUtilities.CreateInstance<TViewModel>(provider), lifeTime);
        }
        /// <summary>
        /// Adds the popupContext model using the specified services
        /// </summary>
        /// <typeparam name="TViewModel">The popupContext model</typeparam>
        /// <param name="services">The services</param>
        /// <param name="createViewModel">The creation popupContext model</param>
        /// <param name="lifeTime">Service Lifetime.</param>

        /// <returns>The services</returns>
        public static IServiceCollection AddViewModel<TViewModel>(this IServiceCollection services, Func<IServiceProvider, TViewModel> createViewModel, ServiceLifetime lifeTime)
            where TViewModel : class, IWpfViewModel, IDisposableObject
        {
            return services.AddService(createViewModel, lifeTime);
        }

        public static IServiceCollection AllowLazy(this IServiceCollection services)
        {
            var lastRegistration = services.Last();

            var lazyServiceType = typeof(Lazy<>).MakeGenericType(lastRegistration.ServiceType);

            var lazyServiceImplementationType = typeof(LazyService<>).MakeGenericType(lastRegistration.ServiceType);

            services.Add(new ServiceDescriptor(lazyServiceType, lazyServiceImplementationType, lastRegistration.Lifetime));
            return services;
        }
    }
}
