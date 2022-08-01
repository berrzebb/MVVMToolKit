using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.VisualStudio.Threading;
using MVVMToolKit.Hosting.Core;
using MVVMToolKit.Hosting.GenericHost;
using MVVMToolKit.Hosting.Internal;

namespace MVVMToolKit.Hosting.Extensions
{
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Generic Host에 WPF를 추가하기 위한 구현체입니다. <see cref="Application" />.
        /// </summary>
        /// <param name="services">The <see cref="IServiceCollection" /> to add services to.</param>
        /// <typeparam name="TApplication">WPF <see cref="Application" />.</typeparam>
        /// <returns>The same instance of the <see cref="IServiceCollection"/> for chaining.</returns>
        public static IServiceCollection AddWPF<TApplication>(this IServiceCollection services)
            where TApplication : Application
        {
            return AddWPF(services, (provider) => ActivatorUtilities.CreateInstance<TApplication>(provider));
        }

        /// <summary>
        /// Generic Host에 WPF를 추가하기 위한 구현체입니다. <see cref="Application" />.
        /// </summary>
        /// <param name="services">The <see cref="IServiceCollection" /> to add services to.</param>
        /// <param name="createApplication">The function used to create <see cref="Application" /></param>
        /// <typeparam name="TApplication">WPF <see cref="Application" />.</typeparam>
        /// <returns>The same instance of the <see cref="IServiceCollection"/> for chaining.</returns>
        public static IServiceCollection AddWPF<TApplication>(this IServiceCollection services, Func<IServiceProvider, TApplication> createApplication)
            where TApplication : Application
        {

            // internal usage only
            services.TryAddSingleton(services);
            //Only single TApplication should exist.
            services.TryAddSingleton<Func<TApplication>>(provider =>
            {
                //Rare case when someone needs to resolve TApplication implementation manually, or maybe not from the IServiceProvider but another container.
                return () => createApplication(provider);
            });

            return services.AddWPFCommonRegistrations<TApplication>();
        }
        public static IServiceCollection AddView<TView>(this IServiceCollection services)
            where TView : ContentControl
        {
            return services.AddView(provider => ActivatorUtilities.CreateInstance<TView>(provider));
        }
        public static IServiceCollection AddViewModel<TViewModel>(this IServiceCollection services)
            where TViewModel : class, IWPFViewModel
        {
            return services.AddViewModel(provider => ActivatorUtilities.CreateInstance<TViewModel>(provider));
        }
        public static IServiceCollection AddView<TView>(this IServiceCollection services, Func<IServiceProvider, TView> createView)
            where TView : ContentControl
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
        private static IServiceCollection AddWPFCommonRegistrations<TApplication>(this IServiceCollection services)
            where TApplication : Application
        {
            services.TryAddSingleton<IDisposableObjectService, DisposableObjectService>();

            //Register WpfContext
            var wpfContext = new WPFContext<TApplication>();
            services.TryAddSingleton(wpfContext); //for internal usage only
            services.TryAddSingleton<IWPFContext<TApplication>>(wpfContext);
            services.TryAddSingleton<IWPFContext>(wpfContext);

            //Register WpfThread
            services.TryAddSingleton(provider => ActivatorUtilities.CreateInstance<WPFThread<TApplication>>(provider));  //for internal usage only
            services.TryAddSingleton<IWPFThread<TApplication>>(s => s.GetRequiredService<WPFThread<TApplication>>());
            services.TryAddSingleton<IWPFThread>(s => s.GetRequiredService<WPFThread<TApplication>>());

            //Register Wpf IHostedService
            services.AddHostedService<WPFHostedService<TApplication>>();
            return services;
        }
        /// <summary>
        /// Adds tray icon functionality for WPF application.
        /// </summary>
        /// <param name="services">The <see cref="IServiceCollection" /> to add services to.</param>
        /// <typeparam name="TTrayIcon">Implementation of <see cref="ITrayIcon" />.</typeparam>
        /// <returns>The same instance of the <see cref="IServiceCollection"/> for chaining.</returns>
        public static IServiceCollection AddWpfTrayIcon<TTrayIcon>(this IServiceCollection services)
            where TTrayIcon : class, ITrayIcon
        {
            //Currently we can't use directly services.AddSingleton<IWpfComponent, TTrayIcon>() / services.AddTransient<IWpfComponent, TTrayIcon>()
            //Since no matter what lifetime you use, if you let the container to create the type by itself without Func then if IDisposable is implemented it will be auto disposed once the container is disposed https://github.com/dotnet/runtime/issues/36491
            //We do not want this, since:
            //1)It happens on non UI thread
            //2)We want to MANUALLY dispose our IWPFComponent's(if it implements IDisposable)
            //Once this is fixed https://github.com/dotnet/runtime/issues/36461 we could use transient.
            services.AddSingleton<Func<IWPFComponent>>(provider =>
            {
                //Gladly this exist to not make a lot of overloads / expose IServiceProvider :)
                return () =>
                {
                    var component = ActivatorUtilities.CreateInstance<TTrayIcon>(provider);
                    component.InitializeComponent();
                    return component;
                };
            });

            return services;
        }

        /// <summary>
        /// Adds tray icon functionality for WPF application.
        /// </summary>
        /// <param name="services">The <see cref="IServiceCollection" /> to add services to.</param>
        /// <param name="createTray">The function used to create <see cref="ITrayIcon" /></param>
        /// <typeparam name="TTrayIcon">Implementation of <see cref="ITrayIcon" />.</typeparam>
        /// <returns>The same instance of the <see cref="IServiceCollection"/> for chaining.</returns>
        public static IServiceCollection AddWpfTrayIcon<TTrayIcon>(this IServiceCollection services, Func<IServiceProvider, TTrayIcon> createTray)
            where TTrayIcon : class, ITrayIcon
        {
            services.AddSingleton<Func<IWPFComponent>>(provider =>
            {
                //Rare case when someone needs to resolve ITrayIcon implementation manually, or maybe not from the IServiceProvider but another container.
                return () => createTray(provider);
            });

            return services;
        }

        /// <summary>
        /// Adds Thread Switching functionality for WPF application.
        /// </summary>
        /// <param name="services">The <see cref="IServiceCollection" /> to add services to.</param>
        /// <typeparam name="TApplication">WPF <see cref="Application" /></typeparam>
        /// <returns>The same instance of the <see cref="IServiceCollection"/> for chaining.</returns>
        /// <see cref="AddThreadSwitching(IServiceCollection)"/>
        /// <note>Should we mark this method as obsolete or keep it? There is no real scenario where we need <see cref="TApplication"/>.</note>
        public static IServiceCollection AddThreadSwitching<TApplication>(this IServiceCollection services)
            where TApplication : Application, IComponentConnector, new()
        {
            services.AddSingleton(provider =>
            {
                var wpfThread = provider.GetRequiredService<IWPFThread<TApplication>>();

                return new JoinableTaskContext(wpfThread.MainThread, wpfThread.SynchronizationContext);
            });
            services.AddSingleton<JoinableTaskFactory>();

            return services;
        }

        /// <summary>
        /// Adds Thread Switching functionality for WPF application.
        /// </summary>
        /// <param name="services">The <see cref="IServiceCollection" /> to add services to.</param>
        /// <returns>The same instance of the <see cref="IServiceCollection"/> for chaining.</returns>
        public static IServiceCollection AddThreadSwitching(this IServiceCollection services)
        {
            services.AddSingleton(provider =>
            {
                var wpfThread = provider.GetRequiredService<IWPFThread>();

                return new JoinableTaskContext(wpfThread.MainThread, wpfThread.SynchronizationContext);
            });
            services.AddSingleton<JoinableTaskFactory>();
            return services;
        }
        /// <summary>
        /// Adds <see cref="IBootstrapper{TContainer}"/> to <see cref="IServiceCollection"/> for <see cref="WpfHostingExtensions.UseWpfContainerBootstrapper{TContainer}"/>.
        /// </summary>
        /// <typeparam name="TContainer">The Dependency Injection container.</typeparam>
        /// <typeparam name="TBootstrapper"><see cref="IBootstrapper{TContainer}"/> bootstrap type.</typeparam>
        /// <param name="services">The <see cref="IServiceCollection" /> to add services to.</param>
        /// <returns>The same instance of the <see cref="IServiceCollection"/> for chaining.</returns>
        public static IServiceCollection AddBootstrapper<TContainer, TBootstrap>(this IServiceCollection services)
            where TContainer : class
            where TBootstrap : class, IBootstrapper<TContainer>
        {
            services.AddSingleton<IBootstrapper<TContainer>, TBootstrap>();

            return services;
        }
    }
}
