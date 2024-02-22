using System.Windows;
using Infrastructure;
using MVVMToolKit.Ioc;

namespace ProtoTypeModule
{
    using Microsoft.Extensions.Logging;

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
