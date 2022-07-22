using Microsoft.Toolkit.Mvvm.ComponentModel;
using MVVMToolKit.Hosting;
using Prism.Ioc;

namespace MVVMToolKitSample.ViewModels
{
    public partial class MainWindowViewModel : ViewModelBase<MainWindowViewModel>
    {

        [ObservableProperty]
        private string? _sample;

        [Microsoft.Toolkit.Mvvm.Input.ICommand]
        private void DoomsDay()
        {
            Sample = "Dooms Day";
        }

        public MainWindowViewModel(IContainerProvider containerProvider) : base(containerProvider)
        {
            Sample = "Hello World";

        }
    }
}
