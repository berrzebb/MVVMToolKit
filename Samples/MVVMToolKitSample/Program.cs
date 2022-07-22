using MVVMToolKit.Hosting;
using System.Threading.Tasks;

namespace MVVMToolKitSample
{
    public class Program
    {
        public async static Task Main(string[] args)
        {
            Startup startup = new Startup();
            await startup.RunAsync<App>(args);
        }
    }
}
