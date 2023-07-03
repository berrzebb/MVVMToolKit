using System;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MVVMToolKit.ViewModels;

namespace MVVMToolKitSample.ViewModels
{
    public partial class MainWindowViewModel : ViewModelBase<MainWindowViewModel>
    {

        [ObservableProperty]
        private string? _sample;

        [RelayCommand]
        private void DoomsDay()
        {
            Sample = "Dooms Day";
        }

        public MainWindowViewModel(IServiceProvider containerProvider) : base(containerProvider)
        {
            Sample = "Hello World";

        }
    }
}
