using System;
using System.Windows;
using Microsoft.Extensions.DependencyInjection;
using MVVMToolKit.Hosting.Core;

namespace MVVMToolKit.Hosting.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddView<TView>(this IServiceCollection services)
            where TView : FrameworkElement
        {
            return services.AddView(provider => ActivatorUtilities.CreateInstance<TView>(provider));
        }
        public static IServiceCollection AddViewModel<TViewModel>(this IServiceCollection services)
            where TViewModel : class, IWPFViewModel
        {
            return services.AddViewModel(provider => ActivatorUtilities.CreateInstance<TViewModel>(provider));
        }
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
