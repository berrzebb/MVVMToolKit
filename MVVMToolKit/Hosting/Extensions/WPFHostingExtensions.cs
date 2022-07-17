using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MVVMToolKit.Hosting.Core;
using MVVMToolKit.Hosting.GenericHost;
using MVVMToolKit.Hosting.Internal;
using MVVMToolKit.Hosting.Locator;
using System;
using System.Windows;

namespace MVVMToolKit.Hosting.Extensions
{
    public static class WpfHostingExtensions
    {
        /// <summary>
        /// Listens for Application.Current.Exit to start the shutdown process.
        /// This will unblock extensions like RunAsync and WaitForShutdownAsync.
        /// </summary>
        /// <param name="hostBuilder">The <see cref="IHostBuilder" /> to configure.</param>
        /// <param name="configureOptions">The delegate for configuring the <see cref="WPFLifeTime"/>.</param>
        /// <returns>The same instance of the <see cref="IHostBuilder"/> for chaining.</returns>
        public static IHostBuilder UseWPFLifetime(this IHostBuilder hostBuilder, Action<WPFLifeTimeOptions> configureOptions)
        {
            if (hostBuilder is null)
            {
                throw new ArgumentNullException(nameof(hostBuilder));
            }

            return hostBuilder.ConfigureServices((_, collection) =>
            {
                collection.AddSingleton<IHostLifetime, WPFLifeTime>();
                collection.Configure(configureOptions);
            });
        }

        /// <summary>
        /// Listens for Application.Current.Exit to start the shutdown process.
        /// This will unblock extensions like RunAsync and WaitForShutdownAsync.
        /// </summary>
        /// <param name="hostBuilder">The <see cref="IHostBuilder" /> to configure.</param>
        /// <returns>The same instance of the <see cref="IHostBuilder"/> for chaining.</returns>
        public static IHostBuilder UseWPFLifetime(this IHostBuilder hostBuilder)
        {
            if (hostBuilder is null)
            {
                throw new ArgumentNullException(nameof(hostBuilder));
            }

            return hostBuilder.ConfigureServices((_, collection) => collection.AddSingleton<IHostLifetime, WPFLifeTime>());
        }

        /// <summary>
        /// Adds feature to use ViewModelLocator and calls <see cref="IViewModelLocatorInitialization{TViewModelLocator}"/>
        /// </summary>
        /// <typeparam name="TApplication">WPF <see cref="Application" />.</typeparam>
        /// <param name="host">The <see cref="IHostBuilder" /> to configure.</param>
        /// <returns>The same instance of the <see cref="IHostBuilder"/> for chaining.</returns>
        public static IHost UseWPFInitialization<TApplication>(this IHost host)
            where TApplication : Application, IApplicationInitializeComponent, IApplicationInitialize
        {
            if (host is null)
            {
                throw new ArgumentNullException(nameof(host));
            }

            WPFThread<TApplication> wpfThread = host.Services.GetRequiredService<WPFThread<TApplication>>();
            wpfThread.SetPreContextInitialization(context =>
            {
                context.WPFApplication?.Initialize();
            });

            return host;
        }

        /// <summary>
        /// Adds feature to use ViewModelLocator and calls <see cref="IViewModelLocatorInitialization{TViewModelLocator}"/>
        /// </summary>
        /// <typeparam name="TApplication">WPF <see cref="Application" />.</typeparam>
        /// <typeparam name="TViewModelLocator">The View Model Locator</typeparam>
        /// <param name="host">The <see cref="IHostBuilder" /> to configure.</param>
        /// <param name="viewModelLocator">Instance of <see cref="TViewModelLocator"/></param>
        /// <returns>The same instance of the <see cref="IHostBuilder"/> for chaining.</returns>
        public static IHost UseWPFViewModelLocator<TApplication, TViewModelLocator>(this IHost host, TViewModelLocator viewModelLocator)
            where TApplication : Application, IApplicationInitializeComponent, IViewModelLocatorInitialization<TViewModelLocator>
        {
            if (host is null)
            {
                throw new ArgumentNullException(nameof(host));
            }

            WPFThread<TApplication> wpfThread = host.Services.GetRequiredService<WPFThread<TApplication>>();
            wpfThread.SetPreContextInitialization(context =>
            {
                context.WPFApplication?.Initialize();
                context.WPFApplication?.InitializeLocator(viewModelLocator);
            });

            return host;
        }

        /// <summary>
        /// Adds feature to use ViewModelLocator and calls <see cref="IViewModelLocatorInitialization{TViewModelLocator}"/>
        /// </summary>
        /// <typeparam name="TApplication">WPF <see cref="Application" />.</typeparam>
        /// <typeparam name="TViewModelLocator">The View Model Locator</typeparam>
        /// <param name="host">The <see cref="IHostBuilder" /> to configure.</param>
        /// <param name="viewModelLocatorFunc">Function for creating <see cref="TViewModelLocator"/></param>
        /// <returns>The same instance of the <see cref="IHostBuilder"/> for chaining.</returns>
        public static IHost UseWPFViewModelLocator<TApplication, TViewModelLocator>(this IHost host, Func<IServiceProvider, TViewModelLocator> viewModelLocatorFunc)
            where TApplication : Application, IApplicationInitializeComponent, IViewModelLocatorInitialization<TViewModelLocator>
        {
            if (host is null)
            {
                throw new ArgumentNullException(nameof(host));
            }

            WPFThread<TApplication> wpfThread = host.Services.GetRequiredService<WPFThread<TApplication>>();
            wpfThread.SetPreContextInitialization(context =>
            {
                context.WPFApplication?.Initialize();
                var viewModelLocator = viewModelLocatorFunc(host.Services);
                context.WPFApplication?.InitializeLocator(viewModelLocator);
            });

            return host;
        }
        /// <summary>
        /// Bootstraps <see cref="IBootstrapper{TContainer}"/> Dependency Injection container that is not Microsoft <see cref="IServiceCollection"/>.
        /// </summary>
        /// <typeparam name="TContainer">Container type.</typeparam>
        /// <param name="host">The <see cref="IHost" /> to configure.</param>
        /// <param name="container">The Dependency Injection container</param>
        /// <returns>The same instance of the <see cref="IHost"/> for chaining.</returns>
        public static IHost UseWPFContainerBootstrapper<TContainer>(this IHost host, TContainer container)
            where TContainer : class
        {
            if (host is null)
            {
                throw new ArgumentNullException(nameof(host));
            }

            var assemblies = AppDomain.CurrentDomain.GetAssemblies();
            var bootstrappers = host.Services.GetServices<IBootstrapper<TContainer>>();
            foreach (var bootstrapper in bootstrappers)
            {
                bootstrapper.Boot(container, assemblies);
            }

            return host;
        }
    }
}
