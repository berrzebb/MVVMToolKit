using System.Windows;
using MVVMToolKit.Hosting;
using MVVMToolKit.Interfaces;
using MVVMToolKit.Ioc;

namespace ModuleSample
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : GenericHostApplication
    {
        protected override void InitializeModuleCatalog(IModuleCatalog moduleCatalog)
        {
            moduleCatalog.AddModule<NumericModule.NumericModule>();
            moduleCatalog.AddModule<PresenterModule.PresenterModule>();
        }

        protected override Window CreateShell()
        {
            return new MainShell();
        }

        protected override void InitializeShell()
        {
            Shell.Show();
            var navigator = ContainerProvider.Resolve<IZoneNavigator>();
            navigator.Navigate("UserZone", "Controller");
            navigator.Navigate("InfoZone", "Currency");
            navigator.Navigate("DataZone", "Data");
        }
    }
}
