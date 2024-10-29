using CommunityToolkit.Mvvm.Input;
using MVVMToolKit;
using MVVMToolKit.Interfaces;

namespace NavigatorWithViewModelSample
{
    public partial class MainWindowViewModel : ViewModelBase<MainWindowViewModel>
    {
        private readonly IZoneNavigator _zoneNavigator;
        public MainWindowViewModel(IZoneNavigator zoneNavigator)
        {
            this._zoneNavigator = zoneNavigator;
        }
        [RelayCommand]
        private void Specific()
        {
            _zoneNavigator.Navigate("MainZone", "Specific");
        }
        [RelayCommand]
        private void Custom()
        {
            _zoneNavigator.Navigate("MainZone", "Custom");
        }
    }
}
