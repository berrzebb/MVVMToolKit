using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using MVVMToolKit.Hosting.Core;
using MVVMToolKit.Hosting.Extensions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace MVVMToolKit.Hosting.GenericHost
{
    public class WPFHostedService<TApplication>
        : IHostedService where TApplication : Application, IApplicationInitializeComponent
    {
        private readonly ILogger<WPFHostedService<TApplication>> _logger;
        private readonly IWPFThread<TApplication> _wpfThread;
        private readonly IWPFContext<TApplication> _wpfContext;

        /// <summary>
        /// The constructor which takes all the DI objects
        /// </summary>
        /// <param name="logger">ILogger</param>
        /// <param name="wpfThread">WpfThread</param>
        /// <param name="wpfContext">WpfContext</param>
        public WPFHostedService(ILogger<WPFHostedService<TApplication>> logger, IWPFThread<TApplication> wpfThread, IWPFContext<TApplication> wpfContext)
        {
            _logger = logger;
            _wpfThread = wpfThread;
            _wpfContext = wpfContext;
        }

        /// <inheritdoc />
        public Task StartAsync(CancellationToken cancellationToken)
        {
            if (cancellationToken.IsCancellationRequested)
            {
                return Task.CompletedTask;
            }

            _logger?.LogInformation($"Starting WPF application {nameof(WPFHostedService<TApplication>)}.");
            // Make the UI thread go
            _wpfThread?.Start();

            _logger?.LogInformation("WPF thread started.");

            return Task.CompletedTask;
        }

        /// <inheritdoc />
        public async Task StopAsync(CancellationToken cancellationToken)
        {
            if (_wpfContext.IsRunning)
            {
                if (_wpfContext.WPFApplication is not null)
                {
                    //If true means that WPF is already shutdown internally. Usually happens when ShutdownMode is set to OnLastWindowClose or OnMainWindowClose
                    //We need to check otherwise if we call Shutdown twice we get an exception
                    bool isShutdown = _wpfContext.WPFApplication.IsWPFShutdown();
                    if (!isShutdown)
                    {
                        _logger.LogInformation("Stopping WPF with Application.Shutdown() due to application exit.");
                        // Stop application
                        if (_wpfThread.JoinableTaskFactory != null)
                        {
                            await _wpfThread.JoinableTaskFactory.SwitchToMainThreadAsync();
                            _wpfContext.IsRunning = false;
                            //_wpfContext.WPFApplication.Shutdown(0);
                        }
                    }
                }
            }
        }
    }
}
