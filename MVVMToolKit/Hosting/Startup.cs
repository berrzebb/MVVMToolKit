using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using MVVMToolKit.Hosting.Extensions;
using System.Threading.Tasks;
using System.Windows;

namespace MVVMToolKit.Hosting
{
    public class Startup
    {
        public async Task RunAsync<TApp>(string[] args)
            where TApp : Application
        {
            using IHost host = Host.CreateDefaultBuilder(args)
                .ConfigureAppConfiguration(ConfigureAppConfiguration)
                .ConfigureLogging(ConfigureLogging)
                .UseWPFLifetime()
                .ConfigureServices(ConfigureServices<TApp>)
                .Build();
            await host.RunAsync();
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
