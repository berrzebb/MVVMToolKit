namespace WithModuleSample
{
    using System;
    using CommunityToolkit.Mvvm.Input;
    using MVVMToolKit.Interfaces;
    using MVVMToolKit.ViewModels;

    public partial class RegionableViewModel : ViewModelBase<RegionableViewModel>
    {
        IViewNavigator _navigator;
        public RegionableViewModel(IServiceProvider provider, IViewNavigator navigator) : base(provider)
        {
            _navigator = navigator;
        }

        [RelayCommand]
        private void SwitchRegion()
        {
            _navigator.Navigate("RegionZone", "Golden", GetType());
        }
    }
}
