using System.Windows;
using Infrastructure;
using Microsoft.Extensions.Logging;
using MVVMToolKit.Ioc;

namespace ProtoTypeModule
{
    public class ProtoTypeInterface : IPrototypeInterface
    {
        private ILogger<ProtoTypeInterface> Logger = ContainerProvider.Resolve<ILogger<ProtoTypeInterface>>();
        /// <inheritdoc />
        public void Print()
        {
            MessageBox.Show("Proto");
            //Logger?.LogInformation("Proto");
        }
    }
}
