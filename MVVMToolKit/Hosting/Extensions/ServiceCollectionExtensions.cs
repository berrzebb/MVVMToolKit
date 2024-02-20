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
        /// Adds the view using the specified services
        /// </summary>
        /// <typeparam name="TView">The view</typeparam>
        /// <param name="services">The services</param>
        /// <param name="lifeTime">Service Lifetime.</param>
        /// <returns>The service collection</returns>
        public static IServiceCollection AddView<TView>(this IServiceCollection services, ServiceLifetime lifeTime = ServiceLifetime.Transient)
            where TView : FrameworkElement
        {
            return services.AddView(provider => ActivatorUtilities.CreateInstance<TView>(provider), lifeTime);
        }
        /// <summary>
        /// Adds the view model using the specified services
        /// </summary>
        /// <typeparam name="TViewModel">The view model</typeparam>
        /// <param name="services">The services</param>
        /// <param name="lifeTime">Service Lifetime.</param>
        /// <returns>The service collection</returns>
        public static IServiceCollection AddViewModel<TViewModel>(this IServiceCollection services, ServiceLifetime lifeTime = ServiceLifetime.Transient)
            where TViewModel : class, IWPFViewModel
        {
            return services.AddViewModel(provider => ActivatorUtilities.CreateInstance<TViewModel>(provider), lifeTime);
        }

        /// <summary>
        /// Adds the view using the specified services
        /// </summary>
        /// <typeparam name="TView">The view</typeparam>
        /// <param name="services">The services</param>
        /// <param name="createView">The create view</param>
        /// <param name="lifeTime">Service Lifetime.</param>
        /// <returns>The services</returns>
        public static IServiceCollection AddView<TView>(this IServiceCollection services, Func<IServiceProvider, TView> createView, ServiceLifetime lifeTime = ServiceLifetime.Transient)
            where TView : FrameworkElement
        {
            services.Add(new ServiceDescriptor(typeof(TView), provider =>
            {
                var view = createView(provider);
                if (view is IDisposableObject disposable)
                {
                    var disposableObjectService = provider.GetRequiredService<IDisposableObjectService>();
                    disposableObjectService.Add(disposable);
                }
                return view;
            }, lifeTime));
            var registeredType = typeof(TView);

            TypeProvider.RegisterType(registeredType);
            return services;
        }
        /// <summary>
        /// Adds the view model using the specified services
        /// </summary>
        /// <typeparam name="TViewModel">The view model</typeparam>
        /// <param name="services">The services</param>
        /// <param name="createViewModel">The create view model</param>
        /// <param name="lifeTime">Service Lifetime.</param>

        /// <returns>The services</returns>
        public static IServiceCollection AddViewModel<TViewModel>(this IServiceCollection services, Func<IServiceProvider, TViewModel> createViewModel, ServiceLifetime lifeTime = ServiceLifetime.Transient)
            where TViewModel : class, IWPFViewModel, IDisposableObject
        {
            services.Add(new ServiceDescriptor(typeof(TViewModel), provider =>
            {
                var viewModel = createViewModel(provider);
                var disposableObjectService = provider.GetRequiredService<IDisposableObjectService>();
                disposableObjectService.Add(viewModel);
                return viewModel;
            }, lifeTime));
            var registeredType = typeof(TViewModel);

            TypeProvider.RegisterType(registeredType);

            return services;
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
