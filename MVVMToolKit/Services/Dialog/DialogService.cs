using MVVMToolKit.Interfaces;
using MVVMToolKit.Models;
using MVVMToolKit.Templates;

namespace MVVMToolKit.Services.Dialog
{

    /// <summary>
    /// 팝업 다이얼로그 서비스.
    /// </summary>
    public sealed class DialogService : IDialogService
    {
        /// <inheritdoc />
        public void Register<TWindow>(string hostType) where TWindow : PopupWindow
        {
            Type type = typeof(TWindow);
            if (!type.IsSubclassOf(typeof(Window)))
                throw new ArgumentException(nameof(type));

            Register(hostType, type);
        }

        /// <inheritdoc />
        public void Register(string hostType, Type targetType)
        {
            DialogTypeRegistry.RegisterDialog(hostType, targetType);
        }

        /// <inheritdoc />
        public IPopupContext CreateDialog(INotifyPropertyChanged viewModel, PopupOption options)
        {
            PopupWindow? popup = DialogTypeRegistry.CreateDialog(options.HostType, options.IsDependencyInjection) ?? throw new ArgumentException("팝업 다이얼로그를 생성할 수 없습니다. PopupWindow 타입이 맞는지 확인하여 주십시오.");
            popup.Width = options.Width;
            popup.Height = options.Height;
            popup.Title = options.Title;

            popup.TitleBarTemplate = options.TitleBarTemplate;
            popup.Content = viewModel;

            return new PopupContext(popup);
        }
    }
}
