using System.Windows;
using MVVMToolKit.Interfaces;
using MVVMToolKit.Ioc;

namespace ViewSelectorSample
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App
    {
        protected override Window CreateShell()
        {
            return new Shell();
        }
        protected override void InitializeModuleCatalog(IModuleCatalog moduleCatalog)
        {
            moduleCatalog.AddModule<DataModule.DataModule>();
            moduleCatalog.AddModule<PresenterModule.PresenterModule>();
        }

        protected override void InitializeShell()
        {
            Shell.Show();
            var navigator = ContainerProvider.Resolve<IZoneNavigator>();

            navigator.Navigate("Data", "Animal");
        }
    }

}
