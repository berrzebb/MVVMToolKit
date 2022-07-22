using MVVMToolKit.Hosting;
using MVVMToolKitSample.ViewModels;
using System.Threading.Tasks;

namespace MVVMToolKitSample
{
    public class Program
    {
        public async static Task Main(string[] args)
        {
            Startup startup = new Startup();
            await startup.RunAsync<MainWindowViewModel, App>(args);
        }
    }
}
