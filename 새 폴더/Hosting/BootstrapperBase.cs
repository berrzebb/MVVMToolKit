using System;
using System.Linq;
using System.Windows;
using System.Windows.Threading;

namespace MVVMToolKit.Core
{
    public abstract class BootstrapperBase : IBootstrapper
    {

        public Application Application { get; private set; }
        public string[] Args { get; private set; }

        protected BootstrapperBase()
        {
        }
        public void Setup(Application application)
        {
            if (application == null)
            {
                throw new ArgumentException(nameof(application));
            }

            Application = application;
            Application.Startup += Application_Startup;
            Application.Exit += Application_Exit;
            Application.DispatcherUnhandledException += Application_DispatcherUnhandledException;
        }

        private void Application_DispatcherUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
        {
            OnUnhandledException(e);
        }

        public virtual void Start(string[] args)
        {
            Args = args;
            OnStart();

            this.ConfigureBootstrapper();

            Application?.Resources.Add(RequestBringIntoViewEventArgs.)
        }
        public virtual Window GetActiveWindow()
        {
            return Application?.Windows.OfType<Window>().FirstOrDefault(x => x.IsActive) ?? Application?.MainWindow;
        }

        protected virtual void Configure() { }
        protected abstract void Lanuch();
        protected virtual void OnStart() { }
        protected virtual void OnLaunch() { }
        protected virtual void OnExit(ExitEventArgs e) { }
        protected virtual void OnUnhandledException(DispatcherUnhandledExceptionEventArgs e) { }
        private async void Application_Startup(object sender, StartupEventArgs e)
        {
            await _host.StartAsync();

            var mainWindow = _host.Services.GetService<MainWindow>();
            mainWindow.Show();
        }

        private async void Application_Exit(object sender, ExitEventArgs e)
        {
            using (_host)
            {
                await _host.StopAsync(TimeSpan.FromSeconds(5));
            }
        }
    }
}
