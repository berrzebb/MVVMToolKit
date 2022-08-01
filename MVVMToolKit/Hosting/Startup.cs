using System.Threading.Tasks;
using System.Windows;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using MVVMToolKit.Hosting.Extensions;
using MVVMToolKit.Hosting.GenericHost;

namespace MVVMToolKit.Hosting
{
    public class Startup
    {
        private IHost host = null;
        private WPFLifeTime wpfLifeTime;
        public bool IsRunning => this.wpfLifeTime.IsRunning;
        public async Task StopAsync()
        {
            await this.host.StopAsync();
        }
        public async Task StartAsync<TApp>(string[] args)
            where TApp : Application
        {
            this.host = Host.CreateDefaultBuilder(args)
                .ConfigureAppConfiguration(this.ConfigureAppConfiguration)
                .ConfigureLogging(this.ConfigureLogging)
                .UseWPFLifetime()
                .ConfigureServices(this.ConfigureServices<TApp>)
                .Build();
            await this.host.StartAsync();
            this.wpfLifeTime = this.host.Services.GetRequiredService<IHostLifetime>() as WPFLifeTime;
        }
        protected virtual void ConfigureLogging(HostBuilderContext context, ILoggingBuilder logging)
        {
            logging.AddJsonConsole();
        }
        protected virtual void ConfigureAppConfiguration(HostBuilderContext context, IConfigurationBuilder configurationBuilder)
        {
            configurationBuilder
                .SetBasePath(context.HostingEnvironment.ContentRootPath)
                .AddEnvironmentVariables();
        }
        protected virtual void ConfigureServices<TApp>(HostBuilderContext hostContext, IServiceCollection services)
            where TApp : Application
        {
            services.AddThreadSwitching();
            services.AddLogging(configuration =>
            {
                configuration
                .AddDebug()
                .AddConsole()
                .AddJsonConsole();
            });
            services.AddWPF<TApp>();
        }
    }
}
