using System.Diagnostics;
using System.Reflection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using MVVMToolKit.Helper;
using MVVMToolKit.Helper.Native;
using MVVMToolKit.Hosting.Core;
using MVVMToolKit.Hosting.Extensions;
using MVVMToolKit.Hosting.Internal;
using MVVMToolKit.Interfaces;
using MVVMToolKit.Interfaces.Modules;
using MVVMToolKit.Interfaces.ViewMapper;
using MVVMToolKit.Ioc;
using MVVMToolKit.Services;
using MVVMToolKit.Utilities;

namespace MVVMToolKit.Hosting
{
    /// <summary>
    /// The generic host application class.
    /// </summary>
    /// <seealso cref="Application"/>
    public abstract class GenericHostApplication : Application
    {
        /// <summary>
        /// Gets the value of the host.
        /// </summary>
        public static IHost? Host { get; private set; }

        /// <summary>
        /// The logger.
        /// </summary>
        protected ILogger<GenericHostApplication>? Logger;

        /// <summary>
        /// The disposable service.
        /// </summary>
        private IDisposableObjectService? disposableService;

        private readonly IModuleCatalog? _moduleCatalog;
        private readonly IMappingBuilder _builder = new MappingBuilder();

        /// <summary>
        /// Initializes a new instance of the <see cref="GenericHostApplication"/> class.
        /// </summary>
        protected GenericHostApplication()
        {
            var builder = Microsoft.Extensions.Hosting.Host.CreateDefaultBuilder();
            SafeFireAndForgetExtensions.SetDefaultExceptionHandling(OnSafeFireAndForgetExceptionHandler);
            _moduleCatalog = CreateModuleCatalog_Internal();
            if (_moduleCatalog != null)
            {
                InitializeModuleCatalog_Internal(_moduleCatalog);
            }

            Host = builder
                .UseDefaultServiceProvider(ConfigureServiceProvider)
                .ConfigureAppConfiguration(ConfigureAppConfiguration)
                .ConfigureLogging(ConfigureLogging)
                .ConfigureServices(ConfigureServices)
                .Build();

            ContainerProvider.Initialize(Host.Services);
            Dispatcher.UnhandledException += Dispatcher_UnhandledException;
            Dispatcher.UnhandledExceptionFilter += Dispatcher_UnhandledExceptionFilter;
        }

        private void OnSafeFireAndForgetExceptionHandler(Exception? ex)
        {
            Logger?.LogError(ex, $"[SafeFireAndForget]");
        }
        private void OnInitializeMapping()
        {
            var mappingResources = _builder.Build();
            Resources.MergedDictionaries.Add(mappingResources);

        }
        private void OnInitializeModule()
        {
            if (_moduleCatalog == null) return;
            foreach (var module in _moduleCatalog.Modules)
            {
                module.Initialize(Host?.Services).SafeFireAndForget();
            }
        }

        private IModuleCatalog CreateModuleCatalog_Internal()
        {
            return CreateModuleCatalog();
        }
        private void InitializeModuleCatalog_Internal(IModuleCatalog moduleCatalog)
        {
            InitializeModuleCatalog(moduleCatalog);
        }

        protected virtual IModuleCatalog CreateModuleCatalog() => new DirectoryModuleCatalog("Modules");

        /// <summary>
        /// Basic Implement Module Catalog With Assembly
        /// </summary>

        protected virtual void InitializeModuleCatalog(IModuleCatalog moduleCatalog)
        {
        }


        /// <summary>
        /// Describes whether this instance check duplicate process.
        /// </summary>
        /// <returns>The result.</returns>
        private bool CheckDuplicateProcess(string appName = "", string appTitle = "")
        {
            bool result = false;
            if (ProcessHelper.IsRunningProcess(appName))
            {
                result = true;

                string? processName = Assembly.GetExecutingAssembly().GetName().Name;
                int currentProcess = Process.GetCurrentProcess().Id;
                Process[] processes = Process.GetProcessesByName(processName);
                foreach (Process process in processes)
                {
                    if (currentProcess == process.Id)
                    {
                        continue;
                    }

                    // find MainWindow Title
                    IntPtr hwnd = ProcessHelper.FindWindow("", appTitle);
                    if (hwnd.ToInt32() > 0)
                    {
                        // Activate
                        ProcessHelper.SetForegroundWindow(hwnd);

                        NativeMethods.WindowShowStyle command = ProcessHelper.IsIconic(hwnd)
                            ? NativeMethods.WindowShowStyle.Restore
                            : NativeMethods.WindowShowStyle.Show;
                        ProcessHelper.ShowWindow(hwnd, command);
                    }
                }

                Logger?.LogCritical("!!DuplicateProcess!!");
                Current.Shutdown();
            }

            return result;
        }


        protected GenericHostApplication AddMapping<TViewModel>(IViewConfiguration configuration)
        {
            _builder.AddMapping<TViewModel>(configuration);
            return this;
        }
        protected GenericHostApplication AddMapping(Uri uri)
        {
            _builder.AddMapping(uri);
            return this;
        }
        /// <summary>
        /// Ons the startup using the specified e.
        /// </summary>
        /// <param name="e">The e.</param>
        protected override async void OnStartup(StartupEventArgs e)
        {
            await Host!.StartAsync();

            Logger = Host.Services.GetRequiredService<ILogger<GenericHostApplication>>();
            disposableService = Host.Services.GetRequiredService<IDisposableObjectService>();

            if (CheckDuplicateProcess())
            {
                Current.Shutdown(0);
            }

            OnInitializeModule();
            OnInitializeMapping();

            base.OnStartup(e);
        }



        /// <summary>
        /// Ons the exit using the specified e.
        /// </summary>
        /// <param name="e">The e. </param>
        protected override async void OnExit(ExitEventArgs e)
        {
            disposableService?.Dispose();
            await Host!.StopAsync();
            base.OnExit(e);
        }

        /// <summary>
        /// Configures the app configuration using the specified context.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="configurationBuilder">The configuration builder.</param>
        protected virtual void ConfigureAppConfiguration(
            HostBuilderContext context,
            IConfigurationBuilder configurationBuilder)
        {
            configurationBuilder
                .SetBasePath(context.HostingEnvironment.ContentRootPath)
                .AddEnvironmentVariables();
        }

        protected virtual void ConfigureServiceProvider(HostBuilderContext context, ServiceProviderOptions options)
        {
            options.ValidateScopes = true;
            options.ValidateOnBuild = true;
        }

        /// <summary>
        /// Configures the logging using the specified context.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="logging">The logging.</param>
        protected virtual void ConfigureLogging(HostBuilderContext context, ILoggingBuilder logging)
        {
            logging.AddJsonConsole();
        }

        /// <summary>
        /// Configures the services using the specified host context.
        /// </summary>
        /// <param name="hostContext">The host context.</param>
        /// <param name="services">The services.</param>
        protected virtual void ConfigureServices(HostBuilderContext hostContext, IServiceCollection services)
        {
            services.AddLogging(configuration =>
            {
                configuration
                    .AddDebug()
                    .AddConsole()
                    .AddJsonConsole();
            });

            services.AddSingleton<IDispatcherService, DispatcherService>().AllowLazy();
            services.AddSingleton<IDisposableObjectService, DisposableObjectService>();

            services.AddSingleton<IDialogService, DialogService>();
            foreach (var module in _moduleCatalog.Modules)
            {
                module.ConfigureServices(services);
            }
            InitializeServices(services);
            InitializeViewModels(services);
            InitializeViews(services);

            foreach (var module in _moduleCatalog.Modules)
            {
                module.ConfigureMappings(_builder);
            }

            InitializeMappings(_builder);
        }


        /// <summary>
        /// Initializes the services using the specified services.
        /// </summary>
        /// <param name="services">The services.</param>
        protected virtual void InitializeServices(IServiceCollection services)
        {
        }

        /// <summary>
        /// Initializes the views using the specified services.
        /// </summary>
        /// <param name="services">The services.</param>
        protected virtual void InitializeViews(IServiceCollection services)
        {
        }

        /// <summary>
        /// Initializes the view models using the specified services.
        /// </summary>
        /// <param name="services">The services.</param>
        protected virtual void InitializeViewModels(IServiceCollection services)
        {
        }

        protected virtual void InitializeMappings(IMappingBuilder builder)
        {
        }
        /// <summary>
        /// Dispatchers the unhandled exception filter using the specified sender.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The e.</param>
        private void Dispatcher_UnhandledExceptionFilter(object sender, DispatcherUnhandledExceptionFilterEventArgs e)
        {
            try
            {
                e.RequestCatch = true;

                Logger?.LogError(e.Exception, $"[App Error Catch] {e.Exception}");
                Logger?.LogError(e.Exception, $"[App_DispatcherUnhandledExceptionFilter] {e.Exception.Message}");
            }
            catch
            {
                // ignored
            }
        }

        /// <summary>
        /// Dispatchers the unhandled exception using the specified sender.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The e.</param>
        private void Dispatcher_UnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
        {
            try
            {
                e.Handled = true;

                Logger?.LogError(e.Exception, $"[App Error Catch] {e.Exception}");
                Logger?.LogError(e.Exception, $"[App_DispatcherUnhandledException] {e.Exception.Message}");
                CoreDumpHelper.CreateMemoryDump();
            }
            catch
            {
                // ignored
            }
        }

        /// <summary>
        /// Initializes this instance.
        /// </summary>
        public virtual void Initialize()
        {
        }
    }
}