namespace WithModuleSample
{
    using System;
    using CommunityToolkit.Mvvm.Input;
    using MVVMToolKit.Interfaces;
    using MVVMToolKit.ViewModels;

    public partial class RegionableViewModel : ViewModelBase<RegionableViewModel>
    {
        readonly IZoneNavigator _navigator;
        int _viewType = 0;
        public RegionableViewModel(IServiceProvider provider, IZoneNavigator navigator) : base(provider)
        {
            _navigator = navigator;
        }

        [RelayCommand]
        private void SwitchRegion()
        {
            switch (_viewType)
            {
                case 0: _navigator.Navigate("RegionZone", "Golden"); break;
                case 1: _navigator.Navigate("RegionZone", "Silver"); break;
            }

            _viewType++;
            if (_viewType == 2)
            {
                _viewType = 0;
            }
        }
    }
}
