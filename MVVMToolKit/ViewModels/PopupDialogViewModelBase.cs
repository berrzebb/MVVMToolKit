using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;

namespace MVVMToolKit.ViewModels
{
    /// <summary>
    /// 팝업 다이얼로그를 위한 ViewModel.
    /// </summary>
    public partial class PopupDialogViewModelBase : ObservableObject
    {
        /// <summary>
        /// The view model.
        /// </summary>
        [ObservableProperty] private ObservableObject? _viewModel;

        /// <summary>
        /// Closes this instance.
        /// </summary>
        [RelayCommand]
        private void Close()
        {
            this.ViewModel = null;
        }

        /// <summary>
        /// Cleanups this instance.
        /// </summary>
        public virtual void Cleanup()
        {
            WeakReferenceMessenger.Default.Cleanup();
        }
    }
}
