using System;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;
using Microsoft.VisualStudio.Threading;
using MVVMToolKit.Hosting.Core;

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

        public bool IsRunning { get; internal set; }
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
            this.WPFContext = wpfContext ?? throw new ArgumentNullException(nameof(wpfContext));
            this.ServiceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));
            this.Options = options.Value ?? throw new ArgumentNullException(nameof(options));
            this.Environment = environment ?? throw new ArgumentNullException(nameof(environment));
            this.ApplicationLifetime = applicationLifetime ?? throw new ArgumentNullException(nameof(applicationLifetime));
            this.HostOptions = hostOptions.Value ?? throw new ArgumentNullException(nameof(hostOptions));
            this.Logger = loggerFactory.CreateLogger("Microsoft.Hosting.Lifetime");
        }

        public Task WaitForStartAsync(CancellationToken cancellationToken)
        {
            //Indicate that we are using our custom lifetime
            this.WPFContext.IsLifetime = true;


            this._applicationStartedRegistration = this.ApplicationLifetime
                .ApplicationStarted
                .Register(state => ((WPFLifeTime)state!).OnApplicationStarted(), this);

            this._applicationStoppingRegistration = this.ApplicationLifetime
                .ApplicationStopping
                .Register(state => ((WPFLifeTime)state!).OnApplicationStopping(), this);

            this._applicationStoppedRegistration = this.ApplicationLifetime
                .ApplicationStopped
                .Register(state => ((WPFLifeTime)state!).OnWpfApplicationStopped(), this);

            AppDomain.CurrentDomain.ProcessExit += this.OnProcessExit;
            //Applications start immediately.
            return Task.CompletedTask;
        }
        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
        private void OnApplicationStarted()
        {
            if (!this.Options.SuppressStatusMessages)
            {
                this.Logger.LogInformation("Wpf application started");
                this.Logger.LogInformation("Lifetime: {Lifetime}", nameof(WPFLifeTime));
                this.Logger.LogInformation("Hosting environment: {EnvName}", this.Environment.EnvironmentName);
                this.Logger.LogInformation("Content root path: {ContentRoot}", this.Environment.ContentRootPath);
            }
            this.WPFContext.ExitHandler = this.OnWpfExiting;
            this.IsRunning = true;
        }
        private void OnApplicationStopping()
        {
            if (!this.Options.SuppressStatusMessages)
            {
                this.Logger.LogInformation("Wpf application is shutting down...");
            }

            if (this.WPFContext.WPFApplication is not null)
            {
                //Make sure to do it only on Main UI thread because it have VerifyAccess()
                var joinableTaskFactory = this.ServiceProvider.GetService<JoinableTaskFactory>();
                if (joinableTaskFactory != null)
                {
                    //await joinableTaskFactory.SwitchToMainThreadAsync();
                    this.WPFContext.WPFApplication.Exit -= this.OnWpfExiting;
                }
            }
        }
        private void OnWpfApplicationStopped()
        {
            if (!this.Options.SuppressStatusMessages)
            {
                this.Logger.LogInformation("Wpf application was stopped.");
            }
        }
        private async void OnExiting(Action act)
        {
            var joinableTaskFactory = this.ServiceProvider.GetService<JoinableTaskFactory>();
            if (joinableTaskFactory != null)
            {
                await joinableTaskFactory.SwitchToMainThreadAsync();
                var disposableObjectService = this.ServiceProvider.GetRequiredService<IDisposableObjectService>();
                disposableObjectService.Dispose();
            } else
            {
                Application.Current.Dispatcher.Invoke(() => {
                    var disposableObjectService = this.ServiceProvider.GetRequiredService<IDisposableObjectService>();
                    disposableObjectService.Dispose();
                });
            }
            this.ApplicationLifetime.StopApplication();
            act();
            this.IsRunning = false;
        }
        private void OnProcessExit(object? sender, EventArgs e)
        {
            this.OnExiting(() =>
            {
                if (!this._shutdownBlock.WaitOne(this.HostOptions.ShutdownTimeout))
                {
                    this.Logger.LogInformation("Waiting for the host to be disposed, please ensure all 'IHost' instances are wrapped in 'using' blocks");
                }

                //_shutdownBlock.WaitOne();

                // On Linux if the shutdown is triggered by SIGTERM then that's signaled with the 143 exit code.
                // Suppress that since we shut down gracefully. https://github.com/aspnet/AspNetCore/issues/6526
                System.Environment.ExitCode = 0;
            });
        }
        private void OnWpfExiting(object? sender, ExitEventArgs e)
        {
            this.OnExiting(() =>
            {
                this.Logger.LogInformation("Wpf application is exit");
            });
        }
        public void Dispose()
        {
            this._shutdownBlock.Set();

            AppDomain.CurrentDomain.ProcessExit -= this.OnProcessExit;

            this._applicationStartedRegistration.Dispose();
            this._applicationStoppingRegistration.Dispose();
            this._applicationStoppedRegistration.Dispose();
        }
    }
}
