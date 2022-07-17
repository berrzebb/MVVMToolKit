using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Windows;


namespace MVVMToolKit
{
    public abstract class GenericHostApplication : Application
    {
        private readonly IHost _host;

        public GenericHostApplication()
        {
            Startup += Application_Startup;
            Exit += Application_Exit;

            _host = new HostBuilder()
                .ConfigureAppConfiguration(ConfigureAppConfiguration)
                .ConfigureServices(ConfigureServices)
                .ConfigureLogging(ConfigureLogging)
                .Build();
        }
        protected virtual void ConfigureAppConfiguration(HostBuilderContext context,  IConfigurationBuilder configurationBuilder)
        {
            configurationBuilder.SetBasePath(context.HostingEnvironment.ContentRootPath);
            configurationBuilder.AddJsonFile("appsettings.json", optional: false);
        }
        protected virtual void ConfigureServices(HostBuilderContext context, IServiceCollection services)
        {
            services.Configure<Settings>
        }
        protected virtual void ConfigureLogging(ILoggingBuilder logging)
        {
            logging.AddConsole();
        }
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
