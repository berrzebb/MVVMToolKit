using Microsoft.Extensions.DependencyInjection;
using MVVMToolKit.Hosting.Core;

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
        /// <returns>The service collection</returns>
        public static IServiceCollection AddView<TView>(this IServiceCollection services)
            where TView : FrameworkElement
        {
            return services.AddView(provider => ActivatorUtilities.CreateInstance<TView>(provider));
        }
        /// <summary>
        /// Adds the view model using the specified services
        /// </summary>
        /// <typeparam name="TViewModel">The view model</typeparam>
        /// <param name="services">The services</param>
        /// <returns>The service collection</returns>
        public static IServiceCollection AddViewModel<TViewModel>(this IServiceCollection services)
            where TViewModel : class, IWPFViewModel
        {
            return services.AddViewModel(provider => ActivatorUtilities.CreateInstance<TViewModel>(provider));
        }
        /// <summary>
        /// Adds the view using the specified services
        /// </summary>
        /// <typeparam name="TView">The view</typeparam>
        /// <param name="services">The services</param>
        /// <param name="createView">The create view</param>
        /// <returns>The services</returns>
        public static IServiceCollection AddView<TView>(this IServiceCollection services, Func<IServiceProvider, TView> createView)
            where TView : FrameworkElement
        {
            services.AddTransient(provider =>
            {
                var view = createView(provider);
                if (view is IDisposableObject disposable)
                {
                    var disposableObjectService = provider.GetRequiredService<IDisposableObjectService>();
                    disposableObjectService.Add(disposable);
                }
                return view;
            });
            return services;
        }
        /// <summary>
        /// Adds the view model using the specified services
        /// </summary>
        /// <typeparam name="TViewModel">The view model</typeparam>
        /// <param name="services">The services</param>
        /// <param name="createViewModel">The create view model</param>
        /// <returns>The services</returns>
        public static IServiceCollection AddViewModel<TViewModel>(this IServiceCollection services, Func<IServiceProvider, TViewModel> createViewModel)
            where TViewModel : class, IWPFViewModel, IDisposableObject
        {
            services.AddTransient(provider =>
            {
                var viewModel = createViewModel(provider);
                var disposableObjectService = provider.GetRequiredService<IDisposableObjectService>();
                disposableObjectService.Add(viewModel);
                return viewModel;
            });
            return services;
        }
    }
}
