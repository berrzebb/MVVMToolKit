using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;

namespace MVVMToolKit.ViewModels
{
    /// <summary>
    /// 팝업 다이얼로그를 위한 ViewModel
    /// </summary>
    public partial class PopupDialogViewModelBase : ObservableObject
    {
        [ObservableProperty] private ObservableObject? _viewModel;

        [RelayCommand]
        private void Close()
        {
            this.ViewModel = null;
        }

        public virtual void Cleanup()
        {
            WeakReferenceMessenger.Default.Cleanup();
        }
    }
}
