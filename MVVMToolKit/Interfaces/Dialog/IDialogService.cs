using MVVMToolKit.Models;

namespace MVVMToolKit.Interfaces
{
    /// <summary>
    /// The dialog service interface
    /// </summary>
    public interface IDialogService
    {
        /// <summary>
        /// 팝업 다이얼로그 등록
        /// </summary>
        /// <param name="hostType">다이얼로그 타입</param>
        /// <param name="targetType">호스트 View(Window) 타입</param>
        void Register(string hostType, Type targetType);

        /// <summary>
        /// 입력된 다이얼로그가 떠있는지 확인
        /// </summary>
        /// <param name="title">다이얼로그 명칭</param>
        /// <returns></returns>
        bool CheckActivate(string? title);

        /// <summary>
        /// 다이얼로그 컨텐츠 설정
        /// </summary>
        /// <param name="viewModel">컨텐츠에서 사용할 ViewModel</param>
        /// <param name="title">팝업 타이틀.</param>
        /// <param name="width">팝업 크기.</param>
        /// <param name="height">팝업 크기.</param>
        /// <param name="hostType">팝업창 호스트 타입</param>
        /// <param name="isModal">Modal 여부</param>
        void Update(INotifyPropertyChanged viewModel, string? title, double width, double height, string hostType,
            bool isModal = true);
        /// <summary>
        /// 입력된 명칭을 가진 다이얼로그를 반환.
        /// </summary>
        /// <param name="title">다이얼로그 명칭</param>
        /// <returns></returns>
        IDialog? GetDialog(string? title);

        /// <summary>
        /// 다이얼로그 컨텐츠 설정
        /// </summary>
        /// <param name="viewModel">컨텐츠에서 사용할 ViewModel</param>
        /// <param name="options">팝업창 설정</param>
        void Update(INotifyPropertyChanged viewModel, PopupOption options);
        /// <summary>
        /// 다이얼로그 종료
        /// </summary>
        /// <param name="dialog">다이얼로그 객체</param>
        /// <param name="actionClosing">종료전 수행할 작업</param>
        void Close(IDialog dialog, Action? actionClosing = null);
        /// <summary>
        /// 다이얼로그 종료
        /// </summary>
        /// <param name="title">다이얼로그 명칭</param>
        /// <param name="actionClosing">종료전 수행할 작업</param>
        void Close(string? title, Action? actionClosing = null);
        /// <summary>
        /// 다이얼로그 종료
        /// </summary>
        /// <param name="vm">종료할 창의 vm</param>
        /// <param name="actionClosing">종료전 수행할 작업</param>
        void Close(INotifyPropertyChanged? vm, Action? actionClosing = null);
        /// <summary>
        /// 다이얼로그 정리
        /// </summary>
        void Clear();
    }
}
