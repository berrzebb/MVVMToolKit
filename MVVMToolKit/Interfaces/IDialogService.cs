using MVVMToolKit.Models;
using MVVMToolKit.Templates;

namespace MVVMToolKit.Interfaces
{
    /// <summary>
    /// The popup service interface
    /// </summary>
    public interface IDialogService
    {

        /// <summary>
        /// 팝업 다이얼로그 등록
        /// </summary>
        /// <typeparam name="TWindow">호스트 View(Window) 타입</typeparam>
        /// <param name="hostType">다이얼로그 타입</param>
        void Register<TWindow>(string hostType) where TWindow : PopupWindow;
        /// <summary>
        /// 팝업 다이얼로그 등록
        /// </summary>
        /// <param name="hostType">다이얼로그 타입</param>
        /// <param name="targetType">호스트 View(Window) 타입</param>
        void Register(string hostType, Type targetType);

        /// <summary>
        /// 팝업 컨텐츠 설정
        /// </summary>
        /// <param name="viewModel">컨텐츠에서 사용할 ViewModel</param>
        /// <param name="title">팝업 타이틀.</param>
        /// <param name="width">팝업 크기.</param>
        /// <param name="height">팝업 크기.</param>
        /// <param name="hostType">팝업창 호스트 타입</param>
        /// <param name="isModal">Modal 여부</param>
        IPopupContext CreateDialog(INotifyPropertyChanged viewModel, string? title, double width, double height, string hostType,
            bool isModal = true);


        /// <summary>
        /// 팝업 컨텐츠 설정
        /// </summary>
        /// <param name="viewModel">컨텐츠에서 사용할 ViewModel</param>
        /// <param name="options">팝업창 설정</param>
        IPopupContext CreateDialog(INotifyPropertyChanged viewModel, PopupOption options);

        /// <summary>
        /// 팝업 컨텐츠 설정
        /// </summary>

        /// <summary>
        /// 팝업 컨텐츠 설정
        /// </summary>

    }
}
