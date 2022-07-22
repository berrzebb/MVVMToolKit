using Microsoft.Extensions.DependencyInjection;
using Prism.Ioc;
using Prism.Microsoft.DependencyInjection;
using System;
using System.Windows;

namespace MVVMToolKit.Hosting
{
    public abstract class Bootstrapper : Prism.PrismBootstrapperBase
    {
        internal IContainerExtension _containerExtension;
        internal Action<IContainerRegistry> _RegisterTypes = (containerRegistry) => { };
        public Bootstrapper(IServiceCollection serviceCollection)
        {
            _containerExtension = PrismContainerExtension.Init(serviceCollection);
        }
        protected override IContainerExtension CreateContainerExtension()
        {
            return _containerExtension;
        }
    }
    public class Bootstrapper<TMainWindow> : Bootstrapper where TMainWindow : Window
    {
        public Bootstrapper(IServiceCollection serviceCollection) : base(serviceCollection)
        {
        }

        protected override DependencyObject CreateShell() => Container.Resolve<TMainWindow>();

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            _RegisterTypes?.Invoke(containerRegistry);
        }
    }
}
