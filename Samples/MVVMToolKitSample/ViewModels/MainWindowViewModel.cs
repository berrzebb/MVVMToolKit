using Microsoft.Toolkit.Mvvm.ComponentModel;

namespace MVVMToolKitSample.ViewModels
{
    public partial class MainWindowViewModel : ObservableObject
    {
        [ObservableProperty]
        private string? _sample;

        public MainWindowViewModel()
        {
            Sample = "Hello World";

        }
        [Microsoft.Toolkit.Mvvm.Input.ICommand]
        private void DoomsDay()
        {
            Sample = "Dooms Day";
        }
    }
}
