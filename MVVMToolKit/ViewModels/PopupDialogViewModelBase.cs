using System.Windows.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using MVVMToolKit.Interfaces;
using MVVMToolKit.Ioc;

namespace MVVMToolKit.ViewModels
{
    /// <summary>
    /// 팝업 다이얼로그를 위한 ViewModel.
    /// </summary>
    public abstract partial class PopupDialogViewModelBase : ObservableObject, IDialogContext
    {
        /// <summary>
        /// The view model.
        /// </summary>
        [ObservableProperty] private INotifyPropertyChanged? _viewModel;

        [ObservableProperty] private string? _title;
        /// <summary>
        /// Closes this instance.
        /// </summary>
        [RelayCommand]
        public void Close()
        {
            this.ViewModel = null;

        }

        /// <summary>
        /// Cleanups this instance.
        /// </summary>
        public virtual void Cleanup()
        {
            Close();
            WeakReferenceMessenger.Default.Cleanup();
        }


        [RelayCommand]
        private void TitleBarMouseMove(MouseEventArgs args)
        {
            if (args is { Source: FrameworkElement control, LeftButton: MouseButtonState.Pressed })
            {
                var dispatcherService = ContainerProvider.Resolve<IDispatcherService>();
                dispatcherService?.Invoke(() =>
                {
                    var window = Window.GetWindow(control);
                    window?.DragMove();
                });
            }
        }
    }
}
