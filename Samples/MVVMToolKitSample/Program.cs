using System.Threading.Tasks;
using MVVMToolKit.Hosting;

namespace MVVMToolKitSample
{
    public class Program
    {
        public async static Task Main(string[] args)
        {
            Startup startup = new Startup();
            await startup.StartAsync<App>(args);
            while (startup.IsRunning)
            {

            }
            await startup.StopAsync();
        }
    }
}
