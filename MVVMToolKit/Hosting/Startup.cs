using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using MVVMToolKit.Hosting.Core;
using MVVMToolKit.Hosting.Extensions;
using System.Threading.Tasks;
using System.Windows;

namespace MVVMToolKit.Hosting
{
    public class Startup
    {
        public async Task RunAsync<TViewModel, TApp>(string[] args)
            where TApp : Application, IApplicationInitializeComponent
            where TViewModel : class, IWPFViewModel
        {
            using IHost host = Host.CreateDefaultBuilder(args)
                .ConfigureAppConfiguration(ConfigureAppConfiguration)
                .ConfigureLogging(ConfigureLogging)
                .UseWPFLifetime()
                .ConfigureServices(ConfigureServices<TViewModel, TApp>)
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
        protected virtual void ConfigureServices<TViewModel, TApp>(HostBuilderContext hostContext, IServiceCollection services)
            where TApp : Application, IApplicationInitializeComponent
            where TViewModel : class, IWPFViewModel
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
            services.AddViewModel<TViewModel>();
        }
    }
}
