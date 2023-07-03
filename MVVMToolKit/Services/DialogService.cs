using CommunityToolkit.Mvvm.ComponentModel;
using MVVMToolKit.Interfaces;
using MVVMToolKit.Ioc;
using MVVMToolKit.Models;
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
        /// <param name="isModal">The is model</param>
        /// <exception cref="Exception">팝업 다이얼로그를 생성할 수 없습니다. IDialog 타입이 맞는지 확인하여 주십시오.</exception>
        public void Update(ObservableObject viewModel, string? title, double width, double height, string hostType,
            bool isModal = true)
        {
            Update(viewModel, new PopupOption
            {
                Title = title,
                Width = width,
                Height = height,
                HostType = hostType,
                IsModal = isModal
            });
        }
        /// <summary>
        /// Updates the view model
        /// </summary>
        /// <param name="viewModel">The view model</param>
        /// <param name="options"> 팝업 창 설정</param>
        /// <exception cref="Exception">팝업 다이얼로그를 생성할 수 없습니다. IDialog 타입이 맞는지 확인하여 주십시오.</exception>
        public void Update(ObservableObject viewModel, PopupOption options)
        {
            if (!this._dialogHostDictionary.TryGetValue(options.HostType, out Type? hostWindowType))
            {
                throw new Exception("팝업 다이얼로그를 생성할 수 없습니다. IDialog 타입이 맞는지 확인하여 주십시오.");
            }
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
                popup.Width = options.Width;
                popup.Height = options.Height;
                popup.SizeToContent = options.SizeToContent;
                popup.ShowActivated = options.ShowActivated;
                popup.ShowInTaskbar = options.ShowInTaskbar;
                popup.Topmost = options.Topmost;
                popup.Title = options.Title;
                popup.ResizeMode = options.ResizeMode;
                vm.ViewModel = viewModel;
            }

            if (options.IsModal)
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
