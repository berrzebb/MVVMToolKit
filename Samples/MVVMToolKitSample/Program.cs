using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using MVVMToolKit.Hosting.Extensions;
using MVVMToolKitSample.Locator;
using MVVMToolKitSample.ViewModels;
using System;
using System.Threading.Tasks;

namespace MVVMToolKitSample
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            using IHost host = CreateHostBuilder(args)
                .Build()
                .UseWPFViewModelLocator<App, IViewModelLocator>(provider => new ViewModelLocator(provider));
            await host.RunAsync();
            Environment.Exit(0);
            host.WaitForShutdown();
        }

        private static IHostBuilder CreateHostBuilder(string[] args)
        {
            return Host.CreateDefaultBuilder(args)
                .ConfigureAppConfiguration(ConfigureAppConfiguration)
                .ConfigureLogging(ConfigureLogging)
                .UseWPFLifetime()
                .ConfigureServices(ConfigureServices);
        }

        private static void ConfigureLogging(HostBuilderContext context, ILoggingBuilder logging)
        {
            logging.AddJsonConsole();
        }

        private static void ConfigureAppConfiguration(HostBuilderContext context, IConfigurationBuilder configurationBuilder)
        {
            configurationBuilder
                .SetBasePath(context.HostingEnvironment.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddEnvironmentVariables();
        }
        private static void ConfigureServices(HostBuilderContext hostContext, IServiceCollection services)
        {
            services.AddThreadSwitching();
            services.AddLogging(configuration =>
            {
                configuration
                .AddDebug()
                .AddConsole()
                .AddJsonConsole();
            });
            services.AddWPF<App>();
            services.AddTransient<MainWindowViewModel>();
        }
    }
}
