using CommunityToolkit.Mvvm.ComponentModel;
using MVVMToolKit.Interfaces;
using MVVMToolKit.Ioc;
using MVVMToolKit.ViewModels;

namespace MVVMToolKit.Services
{
    /// <summary>
    /// The dialog service class
    /// </summary>
    /// <seealso cref="IDialogService"/>
    public class DialogService : IDialogService
    {
        /// <summary>
        /// The dialog host dictionary
        /// </summary>
        private readonly Dictionary<string, Type> _dialogHostDictionary;

        /// <summary>
        /// Initializes a new instance of the <see cref="DialogService"/> class
        /// </summary>
        public DialogService()
        {
            // 미리 Capacity를 5개 할당합니다.
            this._dialogHostDictionary = new(5);
        }
        /// <summary>
        /// Registers the host type
        /// </summary>
        /// <param name="hostType">The host type</param>
        /// <param name="targetType">The target type</param>
        public void Register(string hostType, Type targetType)
        {
            this._dialogHostDictionary.Add(hostType, targetType);
        }
        
        /// <inheritdoc cref="IDialogService"/>
        public bool CheckActivate(string? title)
        {
            var popup = Application.Current.Windows.Cast<Window>().FirstOrDefault(p => p.Title == title);
            if (popup is not null)
            {
                popup.Activate();
                return true;
            }

            return false;
        }

        /// <summary>
        /// Updates the view model
        /// </summary>
        /// <param name="viewModel">The view model</param>
        /// <param name="title">The title</param>
        /// <param name="width">The width</param>
        /// <param name="height">The height</param>
        /// <param name="hostType">The host type</param>
        /// <param name="isModel">The is model</param>
        /// <exception cref="Exception">팝업 다이얼로그를 생성할 수 없습니다. IDialog 타입이 맞는지 확인하여 주십시오.</exception>
        public void Update(ObservableObject viewModel, string? title, double width, double height, string hostType,
            bool isModel = true)
        {
            var hostWindowType = this._dialogHostDictionary[hostType];
            // ContainerProvider를 통해 등록되어 있는 Window를 취득
            var popup = ContainerProvider.Resolve(hostWindowType) as IDialog;
            if (popup is null)
            {
                throw new Exception("팝업 다이얼로그를 생성할 수 없습니다. IDialog 타입이 맞는지 확인하여 주십시오.");
            }

            popup.OnClose = () =>
            {
                popup.OnClose = null;

                if (popup.DataContext is PopupDialogViewModelBase vm)
                {
                    vm.Cleanup();
                }

                popup.DataContext = null;
            };

            if (popup.DataContext is PopupDialogViewModelBase vm)
            {
                popup.Width = width;
                popup.Height = height;
                popup.Title = title;
                vm.ViewModel = viewModel;
            }

            if (isModel)
            {
                popup.ShowDialog();
            }
            else
            {
                popup.Show();
            }
        }

        /// <summary>
        /// Clears this instance
        /// </summary>
        public void Clear()
        {
            foreach (var window in Application.Current.Windows)
            {
                if (window is IDialog popupDialog)
                {
                    popupDialog.OnClose = null;

                    if (popupDialog.DataContext is PopupDialogViewModelBase vm)
                    {
                        vm.Cleanup();
                    }
                    popupDialog.DataContext = null;
                }
            }

            this._dialogHostDictionary.Clear();
        }
    }
}
