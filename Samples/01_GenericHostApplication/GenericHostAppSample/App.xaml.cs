using System.Windows;
using MVVMToolKit.Hosting;

namespace GenericHostAppSample
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : GenericHostApplication
    {
        public App()
        {
        }
        protected override Window CreateShell()
        {
            return new MainWindow();
        }
        protected override void InitializeShell()
        {
            Shell?.Show();
        }
    }
}
