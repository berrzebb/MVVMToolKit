using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using MVVMToolKit.Helper;
using MVVMToolKit.Helper.Native;
using MVVMToolKit.Hosting.Core;
using MVVMToolKit.Hosting.Internal;
using MVVMToolKit.Interfaces;
using MVVMToolKit.Ioc;
using MVVMToolKit.Services;

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
        protected ILogger<GenericHostApplication>? Logger = null;
        
        /// <summary>
        /// The disposable service.
        /// </summary>
        private IDisposableObjectService? disposableService = null;

        /// <summary>
        /// Initializes a new instance of the <see cref="GenericHostApplication"/> class.
        /// </summary>
        protected GenericHostApplication()
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

        /// <summary>
        /// Describes whether this instance check duplicate process.
        /// </summary>
        /// <returns>The result.</returns>
        protected bool CheckDuplicateProcess(string appName = "", string appTitle = "")
        {
            bool result = false;
            if (ProcessHelper.Do(appName))
            {
                result = true;

                string? processName = System.Reflection.Assembly.GetExecutingAssembly().GetName().Name;
                int currentProcess = System.Diagnostics.Process.GetCurrentProcess().Id;
                System.Diagnostics.Process[] processes = System.Diagnostics.Process.GetProcessesByName(processName);
                foreach (System.Diagnostics.Process process in processes)
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

                this.Logger?.LogCritical("!!DuplicateProcess!!");
                Current.Shutdown();
            }
            
            return result;
        }
        
        /// <summary>
        /// Ons the startup using the specified e.
        /// </summary>
        /// <param name="e">The e.</param>
        protected override async void OnStartup(StartupEventArgs e)
        {
            await Host!.StartAsync();
            // this.Initialize();

            this.Logger = Host.Services.GetRequiredService<ILogger<GenericHostApplication>>();
            this.disposableService = Host.Services.GetRequiredService<IDisposableObjectService>();

            if (this.CheckDuplicateProcess())
            {
                Current.Shutdown(0);
            }
            
            base.OnStartup(e);
        }

        /// <summary>
        /// Ons the exit using the specified e.
        /// </summary>
        /// <param name="e">The e. </param>
        protected override async void OnExit(ExitEventArgs e)
        {
            this.disposableService?.Dispose();
            await Host!.StopAsync();
            Host.Dispose();
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
            services.AddSingleton<IDisposableObjectService, DisposableObjectService>();

            services.AddSingleton<IDialogService, DialogService>();
            this.InitializeServices(services);
            this.InitializeViewModels(services);
            this.InitializeViews(services);
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

                this.Logger?.LogError(e.Exception, $"[App Error Catch] {e.Exception}");
                this.Logger?.LogError(e.Exception, $"[App_DispatcherUnhandledExceptionFilter] {e.Exception.Message}");
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

                this.Logger?.LogError(e.Exception, $"[App Error Catch] {e.Exception}");
                this.Logger?.LogError(e.Exception, $"[App_DispatcherUnhandledException] {e.Exception.Message}");
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