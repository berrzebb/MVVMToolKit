using System;
using System.Windows;
using System.Windows.Threading;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using MVVMToolKit.Hosting.Core;
using MVVMToolKit.Hosting.Internal;
using MVVMToolKit.Interfaces;
using MVVMToolKit.Ioc;
using MVVMToolKit.Services;

namespace MVVMToolKit.Hosting
{
    public abstract class GenericHostApplication : Application
    {

        public static IHost? Host { get; private set; }
        protected readonly IServiceProvider? _provider;
        protected ILogger<GenericHostApplication>? Logger = null;
        private IDisposableObjectService disposableService;

        public GenericHostApplication()
        {
            Host = Microsoft.Extensions.Hosting.Host.CreateDefaultBuilder()
                .ConfigureAppConfiguration(this.ConfigureAppConfiguration)
                .ConfigureLogging(this.ConfigureLogging)
                .ConfigureServices(this.ConfigureServices)
                .Build();
            ContainerProvider.provider = Host.Services;
            this.Dispatcher.UnhandledException += this.Dispatcher_UnhandledException;
            this.Dispatcher.UnhandledExceptionFilter += this.Dispatcher_UnhandledExceptionFilter;
        }

        protected override async void OnStartup(StartupEventArgs e)
        {
            await Host!.StartAsync();
            //this.Initialize();

            this.Logger = Host.Services.GetRequiredService<ILogger<GenericHostApplication>>();
            this.disposableService = Host.Services.GetRequiredService<IDisposableObjectService>();

            base.OnStartup(e);
        }

        protected override async void OnExit(ExitEventArgs e)
        {
            this.disposableService.Dispose();
            await Host!.StopAsync();
            base.OnExit(e);
        }

        protected virtual void ConfigureAppConfiguration(HostBuilderContext context,
            IConfigurationBuilder configurationBuilder)
        {
            configurationBuilder
                .SetBasePath(context.HostingEnvironment.ContentRootPath)
                .AddEnvironmentVariables();
        }

        protected virtual void ConfigureLogging(HostBuilderContext context, ILoggingBuilder logging)
        {
            logging.AddJsonConsole();
        }

        protected virtual void ConfigureServices(HostBuilderContext hostContext, IServiceCollection services)
        {
            services.AddLogging(configuration =>
            {
                configuration
                    .AddDebug()
                    .AddConsole()
                    .AddJsonConsole();
            });
            services.AddSingleton<IDisposableObjectService, DisposableObjectService>();

            services.AddSingleton<IDialogService, DialogService>();
            this.InitializeServices(services);
            this.InitializeViewModels(services);
            this.InitializeViews(services);
        }

        protected virtual void InitializeServices(IServiceCollection services)
        {

        }

        protected virtual void InitializeViews(IServiceCollection services)
        {

        }

        protected virtual void InitializeViewModels(IServiceCollection services)
        {
        }

        private void Dispatcher_UnhandledExceptionFilter(object sender, DispatcherUnhandledExceptionFilterEventArgs e)
        {
            try
            {
                e.RequestCatch = true;

                this.Logger.LogError(e.Exception, $"[App Error Catch] {e.Exception}");
                this.Logger.LogError(e.Exception, $"[App_DispatcherUnhandledExceptionFilter] {e.Exception.Message}");
            }
            catch
            {
            }
        }

        private void Dispatcher_UnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
        {
            try
            {
                e.Handled = true;

                this.Logger.LogError(e.Exception, $"[App Error Catch] {e.Exception}");
                this.Logger.LogError(e.Exception, $"[App_DispatcherUnhandledException] {e.Exception.Message}");
            }
            catch
            {
            }

        }

        public virtual void Initialize()
        {

        }
    }
}