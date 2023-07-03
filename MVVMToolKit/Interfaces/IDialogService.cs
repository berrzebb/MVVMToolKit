using CommunityToolkit.Mvvm.ComponentModel;
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
        void Update(ObservableObject viewModel, string? title, double width, double height, string hostType,
            bool isModal = true);
        /// <summary>
        /// 다이얼로그 컨텐츠 설정
        /// </summary>
        /// <param name="viewModel">컨텐츠에서 사용할 ViewModel</param>
        /// <param name="options">팝업창 설정</param>
        void Update(ObservableObject viewModel, PopupOption options);
        /// <summary>
        /// 다이얼로그 정리
        /// </summary>
        void Clear();
    }
}
