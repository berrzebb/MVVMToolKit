using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using MVVMToolKit.Hosting.Core;
using MVVMToolKit.Hosting.GenericHost;
using MVVMToolKit.Hosting.Internal;
using System;
using System.Windows;

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
            where TApplication : Application, IApplicationInitializeComponent
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
            where TApplication : Application, IApplicationInitializeComponent
        {
            //Only single TApplication should exist.
            services.TryAddSingleton<Func<TApplication>>(provider =>
            {
                //Rare case when someone needs to resolve TApplication implementation manually, or maybe not from the IServiceProvider but another container.
                return () => createApplication(provider);
            });

            return services.AddWPFCommonRegistrations<TApplication>(); ;
        }

        private static IServiceCollection AddWPFCommonRegistrations<TApplication>(this IServiceCollection services)
            where TApplication : Application, IApplicationInitializeComponent
        {
            //Register WpfContext
            var wpfContext = new WPFContext<TApplication>();
            services.TryAddSingleton(wpfContext); //for internal usage only
            services.TryAddSingleton<IWPFContext<TApplication>>(wpfContext);
            services.TryAddSingleton<IWPFContext>(wpfContext);

            //Register WpfThread
            services.TryAddSingleton<WPFThread<TApplication>>();  //for internal usage only
            services.TryAddSingleton<IWPFThread<TApplication>>(s => s.GetRequiredService<WPFThread<TApplication>>());
            services.TryAddSingleton<IWPFThread>(s => s.GetRequiredService<WPFThread<TApplication>>());

            //Register Wpf IHostedService
            services.AddHostedService<WPFHostedService<TApplication>>();

            return services;
        }
    }
}
