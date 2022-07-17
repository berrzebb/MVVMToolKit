using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;
using Microsoft.VisualStudio.Threading;
using MVVMToolKit.Hosting.Core;
using System;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace MVVMToolKit.Hosting.GenericHost
{
    public class WPFLifeTime : IHostLifetime, IDisposable
    {
        private readonly ManualResetEvent _shutdownBlock = new(false);
        private CancellationTokenRegistration _applicationStartedRegistration;
        private CancellationTokenRegistration _applicationStoppingRegistration;
        private CancellationTokenRegistration _applicationStoppedRegistration;

        private IWPFContext WPFContext { get; }

        private IServiceProvider ServiceProvider { get; }
        private WPFLifeTimeOptions Options { get; }
        private IHostEnvironment Environment { get; }

        private IHostApplicationLifetime ApplicationLifetime { get; }
        private HostOptions HostOptions { get; }

        private ILogger Logger { get; }

        public WPFLifeTime(
            IWPFContext wpfContext,
            IServiceProvider serviceProvider,
            IOptions<WPFLifeTimeOptions> options,
            IHostEnvironment environment,
            IHostApplicationLifetime applicationLifeTime,
            IOptions<HostOptions> hostOptions) : this(wpfContext, serviceProvider, options, environment, applicationLifeTime, hostOptions, NullLoggerFactory.Instance)
        {
        }
        public WPFLifeTime(
            IWPFContext wpfContext,
            IServiceProvider serviceProvider,
            IOptions<WPFLifeTimeOptions> options,
            IHostEnvironment environment,
            IHostApplicationLifetime applicationLifetime,
            IOptions<HostOptions> hostOptions,
            ILoggerFactory loggerFactory)
        {
            WPFContext = wpfContext ?? throw new ArgumentNullException(nameof(wpfContext));
            ServiceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));
            Options = options.Value ?? throw new ArgumentNullException(nameof(options));
            Environment = environment ?? throw new ArgumentNullException(nameof(environment));
            ApplicationLifetime = applicationLifetime ?? throw new ArgumentNullException(nameof(applicationLifetime));
            HostOptions = hostOptions.Value ?? throw new ArgumentNullException(nameof(hostOptions));
            Logger = loggerFactory.CreateLogger("Microsoft.Hosting.Lifetime");
        }
        public Task WaitForStartAsync(CancellationToken cancellationToken)
        {
            //Indicate that we are using our custom lifetime
            WPFContext.IsLifetime = true;


            _applicationStartedRegistration = ApplicationLifetime
                .ApplicationStarted
                .Register(state => ((WPFLifeTime)state!).OnApplicationStarted(), this);

            _applicationStoppingRegistration = ApplicationLifetime
                .ApplicationStopping
                .Register(state => ((WPFLifeTime)state!).OnApplicationStopping(), this);

            _applicationStoppedRegistration = ApplicationLifetime
                .ApplicationStopped
                .Register(state => ((WPFLifeTime)state!).OnWpfApplicationStopped(), this);

            AppDomain.CurrentDomain.ProcessExit += OnProcessExit;
            //Applications start immediately.
            return Task.CompletedTask;
        }
        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
        private void OnApplicationStarted()
        {
            if (!Options.SuppressStatusMessages)
            {
                Logger.LogInformation("Wpf application started");
                Logger.LogInformation("Lifetime: {Lifetime}", nameof(WPFLifeTime));
                Logger.LogInformation("Hosting environment: {EnvName}", Environment.EnvironmentName);
                Logger.LogInformation("Content root path: {ContentRoot}", Environment.ContentRootPath);
            }
            WPFContext.ExitHandler = OnWpfExiting;
        }
        private void OnApplicationStopping()
        {
            if (!Options.SuppressStatusMessages)
            {
                Logger.LogInformation("Wpf application is shutting down...");
            }

            if (WPFContext.WPFApplication is not null)
            {
                //Make sure to do it only on Main UI thread because it have VerifyAccess()
                var joinableTaskFactory = ServiceProvider.GetService<JoinableTaskFactory>();
                if (joinableTaskFactory != null)
                {
                    joinableTaskFactory.SwitchToMainThreadAsync();
                    WPFContext.WPFApplication.Exit -= OnWpfExiting;
                }
            }
        }
        private void OnWpfApplicationStopped()
        {
            if (!Options.SuppressStatusMessages)
            {
                Logger.LogInformation("Wpf application was stopped.");
            }
        }
        private void OnProcessExit(object? sender, EventArgs e)
        {
            ApplicationLifetime.StopApplication();
            if (!_shutdownBlock.WaitOne(HostOptions.ShutdownTimeout))
            {
                Logger.LogInformation("Waiting for the host to be disposed, please ensure all 'IHost' instances are wrapped in 'using' blocks");
            }

            _shutdownBlock.WaitOne();

            // On Linux if the shutdown is triggered by SIGTERM then that's signaled with the 143 exit code.
            // Suppress that since we shut down gracefully. https://github.com/aspnet/AspNetCore/issues/6526
            System.Environment.ExitCode = 0;
        }
        private void OnWpfExiting(object? sender, ExitEventArgs e)
        {
            ApplicationLifetime.StopApplication();
        }
        public void Dispose()
        {
            _shutdownBlock.Set();

            AppDomain.CurrentDomain.ProcessExit -= OnProcessExit;

            _applicationStartedRegistration.Dispose();
            _applicationStoppingRegistration.Dispose();
            _applicationStoppedRegistration.Dispose();
        }
    }
}
