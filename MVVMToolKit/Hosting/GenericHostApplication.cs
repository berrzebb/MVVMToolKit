

using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Windows;
using System.Windows.Threading;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using MVVMToolKit.Attributes;
using MVVMToolKit.Helper;
using MVVMToolKit.Helper.Native;
using MVVMToolKit.Hosting.Core;
using MVVMToolKit.Hosting.Extensions;
using MVVMToolKit.Hosting.Internal;
using MVVMToolKit.Interfaces;
using MVVMToolKit.Ioc;
using MVVMToolKit.Ioc.Modules;
using MVVMToolKit.Navigation.Mapping;
using MVVMToolKit.Navigation.Mapping.Internals;
using MVVMToolKit.Navigation.Views;
using MVVMToolKit.Navigation.Zones;
using MVVMToolKit.Services.Dialog;
using MVVMToolKit.Tasks;
using MVVMToolKit.Threading.UI;

namespace MVVMToolKit.Hosting
{
    /// <summary>
    /// The generic host application class.
    /// </summary>
    /// <seealso cref="Application"/>
    public abstract partial class GenericHostApplication : Application
    {
        /// <summary>
        /// Gets the value of the host.
        /// </summary>
        private static IHost? Host { get; set; }
        private bool _canGenerateDump;
        private CoreDumpHelper.MiniDumpType _dumpType = CoreDumpHelper.MiniDumpType.MiniDumpNormal;
        /// <summary>
        /// The logger.
        /// </summary>
        private ILogger<GenericHostApplication>? _logger;
        /// <summary>
        /// The disposable service.
        /// </summary>
        private IDisposableObjectService? _disposableService;

        private readonly IModuleCatalog? _moduleCatalog;
        private readonly IMappingRegistry _mappingRegistry = new MappingManager();

        /// <summary>
        /// Dump가 발생했을때 덤프를 저장할 위치를 얻어옵니다.
        /// </summary>
        /// <returns>Dump의 저장 위치</returns>
        protected virtual string GetDumpPath()
        {
            var assembly = Assembly.GetEntryAssembly();
            string? dirPath = Path.GetDirectoryName(assembly?.Location);
            string exeName = AppDomain.CurrentDomain.FriendlyName;
            string dateTime = DateTime.Now.ToString("[yyyy-MM-dd][HH-mm-ss-fff]", CultureInfo.InvariantCulture);

            return $"{dirPath}/[{exeName}]{dateTime}.dmp";
        }

        /// <summary>
        /// Crash 발생 시 Dump를 생성할 수 있는 옵션을 설정합니다.
        /// </summary>
        /// <param name="canGenerateDump">덤프를 생성할지 여부.</param>
        /// <param name="dumpType">생성할 덤프의 타입</param>
        protected void SetDumpOption(bool canGenerateDump, CoreDumpHelper.MiniDumpType dumpType = CoreDumpHelper.MiniDumpType.MiniDumpNormal)
        {
            _canGenerateDump = canGenerateDump;
            _dumpType = dumpType;
        }

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

            ShutdownMode = ShutdownMode.OnMainWindowClose;
        }
        /// <summary>
        /// SafeAndFireForget Exception.
        /// </summary>
        /// <param name="logger">대상 로거</param>
        /// <param name="exceptionType">예외 타입</param>
        /// <param name="exceptionMessage">예외 메시지</param>
        /// <param name="exceptionStack">예외 스택</param>
        [LoggerMessage(
                    Level = LogLevel.Error,
                    Message = "[SafeFireAndForget] {exceptionType}: `{exceptionMessage}`\nException Stack: {exceptionStack}"
                    )]
        public static partial void SafeFireAndForgetException(ILogger? logger, string? exceptionType, string? exceptionMessage, string? exceptionStack);
        private void OnSafeFireAndForgetExceptionHandler(Exception ex) => SafeFireAndForgetException(_logger, ex.GetType().ToString(), ex.Message, ex.StackTrace);


        private void OnInitializeMapping()
        {
            if (_mappingRegistry is not IMappingBuilder mappingBuilder)
            {
                return;
            }
            ResourceDictionary mappingResources = mappingBuilder.Build();
            InitializeMapping(mappingResources);
            Resources.MergedDictionaries.Add(mappingResources);

        }
        private void OnInitializeModule()
        {
            if (_moduleCatalog == null) return;
            foreach (IModule module in _moduleCatalog)
            {
                module.InitializeModule(Host?.Services).SafeFireAndForget();
            }
        }

        private IModuleCatalog CreateModuleCatalog_Internal() => CreateModuleCatalog();
        private void InitializeModuleCatalog_Internal(IModuleCatalog moduleCatalog) => InitializeModuleCatalog(moduleCatalog);

        /// <summary>
        /// Module Catalog를 생성합니다.
        /// </summary>
        /// <returns>생성된 Module Catalog</returns>
        protected virtual IModuleCatalog CreateModuleCatalog() => new DirectoryModuleCatalog("Modules");

        /// <summary>
        /// Basic Implement Module Catalog With Assembly
        /// </summary>

        protected virtual void InitializeModuleCatalog(IModuleCatalog moduleCatalog)
        {
        }
        protected static DataTemplate CreateDataTemplateFromConfiguration(IMappingConfiguration configuration) => MappingManager.CreateFromConfiguration(configuration);
        protected virtual void InitializeMapping(ResourceDictionary resoruceDictionary)
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
                int currentProcess = Environment.ProcessId;
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

                _logger?.LogCritical("!!DuplicateProcess!!");
                Current.Shutdown();
            }

            return result;
        }
        /// <summary>
        /// Application에서 사용되는 Main Shell
        /// </summary>
        protected Window Shell { get; private set; } = new();

        /// <summary>
        /// Application 에서 사용되는 Main Shell을 생성합니다.
        /// </summary>
        /// <returns></returns>
        protected virtual Window CreateShell() => new();
        /// <summary>
        /// Application의 Shell 초기화 작업을 수행합니다.
        /// </summary>
        protected virtual void InitializeShell()
        {

            MainWindow = Shell;


        }
        /// <summary>
        /// Ons the startup using the specified e.
        /// </summary>
        /// <param name="e">The e.</param>
        protected override async void OnStartup(StartupEventArgs e)
        {
            await Host!.StartAsync().ConfigureAwait(false);

            _logger = Host.Services.GetRequiredService<ILogger<GenericHostApplication>>();
            _disposableService = Host.Services.GetRequiredService<IDisposableObjectService>();
            foreach (var registeredType in ServiceCollectionExtensions.singletonTypes)
            {
                ContainerProvider.Resolve(registeredType);
            }
            if (CheckDuplicateProcess())
            {
                Current.Shutdown(0);
            }

            OnInitializeMapping();

            OnInitializeModule();

            Shell = CreateShell();
            InitializeShell();

            base.OnStartup(e);

            Initialize();
        }



        /// <summary>
        /// Ons the exit using the specified e.
        /// </summary>
        /// <param name="e">The e. </param>
        protected override async void OnExit(ExitEventArgs e)
        {
            _disposableService?.Dispose();
            await Host!.StopAsync().ConfigureAwait(false);
            base.OnExit(e);
        }

        /// <summary>
        /// Configures the app configuration using the specified context.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="configurationBuilder">The configuration builder.</param>
        protected virtual void ConfigureAppConfiguration(
            HostBuilderContext context,
            IConfigurationBuilder configurationBuilder) =>
            configurationBuilder
                .SetBasePath(context.HostingEnvironment.ContentRootPath)
                .AddEnvironmentVariables();

        /// <summary>
        /// Hosting 에서 사용되는 ServiceProvider에 대하여 설정합니다.
        /// </summary>
        /// <param name="context">Host Builder의 Context</param>
        /// <param name="options">Host Builder의 ServiceProvider 설정</param>
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
            logging
                .ClearProviders()
                .AddConsole();
        }
        /// <summary>
        /// Configures the services using the specified host context.
        /// </summary>
        /// <param name="hostContext">The host context.</param>
        /// <param name="services">The services.</param>
        protected virtual void ConfigureServices(HostBuilderContext hostContext, IServiceCollection services)
        {
            services.AddSingleton<IDispatcherService, DispatcherService>().AllowLazy();
            services.AddSingleton<IDisposableObjectService, DisposableObjectService>();

            services.AddSingleton<IZoneRegistry, ZoneRegistry>();
            services.AddSingleton((IRouteRegistry)_mappingRegistry);
            services.AddSingleton<IZoneNavigator, ZoneNavigator>();
            services.AddSingleton<IDialogService, DialogService>();

            IEnumerable<IModule> currentModules = _moduleCatalog?.ToArray() ?? Array.Empty<IModule>();
            foreach (IModule module in currentModules)
            {
                module.RegisterTypes(services);
            }
            RegisterTypes(services);
            ScanMappingAttributes(_mappingRegistry);
        }
        private static void ScanMappingAttributes(IMappingRegistry mappingRegistry)
        {
            foreach (var type in TypeProvider.Types.Select(v => v.ActualType))
            {
                var mappingAttrs = type.GetCustomAttributes<NavigatableAttribute>().ToArray();
                if (mappingAttrs.Length == 0) continue;

                Type? contextType = null;
                if (typeof(INotifyPropertyChanged).IsAssignableFrom(type))
                {
                    contextType = type;
                }
                foreach (var mappingAttr in mappingAttrs)
                {
                    string? viewName = mappingAttr.ViewName;
                    if (typeof(FrameworkElement).IsAssignableFrom(type))
                    {
                        viewName = string.IsNullOrEmpty(viewName) ? type.Name : viewName;
                    }
                    mappingRegistry.Register(new MappingConfiguration(mappingAttr.RouteName, viewName)
                    {
                        ContextType = contextType,
                        CacheMode = mappingAttr.CacheMode,
                    });
                }

            }
        }


        /// <summary>
        /// 응용프로그램에서 사용할 타입들을 등록합니다.
        /// </summary>
        /// <param name="services">Service Collection</param>
        protected virtual void RegisterTypes(IServiceCollection services)
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

                _logger?.LogError(e.Exception, $"[App Error Catch] {e.Exception}");
                _logger?.LogError(e.Exception, $"[App_DispatcherUnhandledExceptionFilter] {e.Exception.Message}");
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

                _logger?.LogError(e.Exception, $"[App Error Catch] {e.Exception}");
                _logger?.LogError(e.Exception, $"[App_DispatcherUnhandledException] {e.Exception.Message}");
                if (_canGenerateDump)
                {
                    string dumpPath = GetDumpPath();
                    CoreDumpHelper.CreateMemoryDump(_dumpType, dumpPath);
                }
            }
            catch
            {
                // ignored
            }
        }

        /// <summary>
        /// Application의 OnStartUp 작업 이후에 수행되는 기능을 정의합니다.
        /// </summary>
        protected virtual void Initialize()
        {
        }
    }
}
