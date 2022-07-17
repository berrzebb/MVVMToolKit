using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using MVVMToolKit.Hosting.Extensions;
using MVVMToolKitSample.Locator;
using MVVMToolKitSample.ViewModels;
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
            //logging.AddConsole();
        }

        private static void ConfigureAppConfiguration(HostBuilderContext context, IConfigurationBuilder configurationBuilder)
        {
            configurationBuilder.SetBasePath(context.HostingEnvironment.ContentRootPath);
            configurationBuilder.AddJsonFile("appsettings.json", optional: false);
        }
        private static void ConfigureServices(HostBuilderContext hostContext, IServiceCollection services)
        {
            services.AddThreadSwitching();
            //services.AddLogging();
            services.AddWPF<App>();
            services.AddTransient<MainWindowViewModel>();
        }
    }
}
