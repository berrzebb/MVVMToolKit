using MVVMToolKit.Interfaces;
using MVVMToolKit.Models;
using MVVMToolKit.Services.Dialog;
using MVVMToolKit.Templates;

namespace MVVMToolKit.Services
{
    /// <summary>
    /// The dialog service class
    /// </summary>
    /// <seealso cref="IDialogService"/>
    public sealed class DialogService : IDialogService
    {
        public void Register<TWindow>(string hostType) where TWindow : PopupWindow
        {
            Type type = typeof(TWindow);
            if (!type.IsSubclassOf(typeof(Window))) throw new ArgumentException(nameof(type));

            Register(hostType, type);
        }
        /// <summary>
        /// Registers the host type
        /// </summary>
        /// <param name="hostType">The host type</param>
        /// <param name="targetType">The target type</param>
        public void Register(string hostType, Type targetType)
        {
            DialogTypeRegistry.RegisterDialog(hostType, targetType);
        }


        /// <summary>
        /// Updates the popupContext model
        /// </summary>
        /// <param name="viewModel">The popupContext model</param>
        /// <param name="title">The title</param>
        /// <param name="width">The width</param>
        /// <param name="height">The height</param>
        /// <param name="hostType">The host type</param>
        /// <param name="isModal">The is model</param>
        /// <exception cref="Exception">팝업 다이얼로그를 생성할 수 없습니다. IDialog 타입이 맞는지 확인하여 주십시오.</exception>
        public IPopupContext CreateDialog(INotifyPropertyChanged viewModel, string? title, double width, double height, string hostType,
            bool isModal = true)
        {
            return CreateDialog(viewModel, new PopupOption
            {
                Title = title,
                Width = width,
                Height = height,
            });
        }
        /// <summary>
        /// Updates the popupContext model
        /// </summary>
        /// <param name="viewModel">The popupContext model</param>
        /// <param name="options"> 팝업 창 설정</param>
        /// <exception cref="Exception">팝업 다이얼로그를 생성할 수 없습니다. IDialog 타입이 맞는지 확인하여 주십시오.</exception>
        public IPopupContext CreateDialog(INotifyPropertyChanged viewModel, PopupOption options)
        {
            PopupWindow? popup = DialogTypeRegistry.CreateDialog(options.HostType, options.IsDependencyInjection);
            if (popup is null)
            {
                throw new ArgumentException("팝업 다이얼로그를 생성할 수 없습니다. IDialog 타입이 맞는지 확인하여 주십시오.");
            }


            popup.Width = options.Width;
            popup.Height = options.Height;
            popup.Title = options.Title;

            popup.TitleBarTemplate = options.TitleBarTemplate;
            popup.Content = viewModel;

            return new PopupContext(popup);
        }


    }
}
