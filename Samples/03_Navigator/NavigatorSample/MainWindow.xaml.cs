using System.Windows;
using MVVMToolKit.Interfaces;
using MVVMToolKit.Ioc;

namespace NavigatorSample
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        IZoneNavigator _zoneNavigator;
        public MainWindow()
        {
            InitializeComponent();
            this._zoneNavigator = ContainerProvider.Resolve<IZoneNavigator>();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            _zoneNavigator.Navigate("MainZone", "AView");
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            _zoneNavigator.Navigate("MainZone", "BView");
        }
    }
}
