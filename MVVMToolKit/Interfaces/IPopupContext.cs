using System;

namespace MVVMToolKit.Interfaces
{
    /// <summary>
    /// 다이얼로그 Show의 결과입니다.
    /// </summary>
    public enum DialogResult
    {

        /// <summary>
        /// Default
        /// </summary>
        None,
        /// <summary>
        /// Modal Dialog Yes.
        /// </summary>
        Yes,
        /// <summary>
        /// Modal Dialog No.
        /// </summary>
        No,
    };
    /// <summary>
    /// 팝업에 대한 문맥 객체.
    /// </summary>
    public interface IPopupContext
    {
        /// <summary>
        /// 팝업창에서 사용중인Source. 
        /// </summary>
        object? Source { get; }
        /// <summary>
        /// 팝업창에서 사용중인 DataContext.
        /// </summary>
        object? DataContext { get; }
        /// <summary>
        /// 팝업창 객체가 살아 있는지 여부.
        /// </summary>
        bool IsAlive { get; }
        /// <summary>
        /// 팝업창의 Window Handle.
        /// </summary>
        IntPtr Handle { get; }
        /// <summary>
        /// 팝업창에서 사용중인 Owner Window를 반환합니다.
        /// </summary>
        /// <returns>팝업창의 Owner Window</returns>
        object? GetOwner();

        /// <summary>
        /// 팝업창을 활성화 합니다.
        /// </summary>
        /// <returns>팝업창 활성화 여부</returns>
        bool Activate();
        /// <summary>
        /// 팝업창을 닫습니다.
        /// </summary>
        void Close();

        #region Fluent Interface
        /// <summary>
        /// 팝업을 표시합니다.
        /// <seealso cref="DialogResult">DialogResult</seealso>
        /// </summary>
        /// <param name="isModal">Modal 형태의 팝업을 사용할지 설정.</param>
        /// <returns>팝업의 수행결과를 반환합니다. 반환 결과는 <seealso cref="DialogResult">DialogResult</seealso> Enum을 사용합니다. 
        ///<br/>isModal이 false인 경우 <seealso cref="DialogResult.None">DialogResult.None</seealso>을 반환합니다.
        ///<br/>isModal이 true인 경우 ShowDialog의 결과에 따라 <seealso cref="DialogResult.Yes">DialogResult.Yes</seealso>, <seealso cref="DialogResult.No">DialogResult.No</seealso>가 반환됩니다.
        /// </returns>
        DialogResult Show(bool isModal = false);

        /// <summary>
        /// 팝업 종료 시 수행할 작업을 등록합니다.
        /// </summary>
        /// <param name="onClose">팝업 종료 시 수행할 작업.</param>
        /// <returns>팝업의 문맥 객체</returns>
        IPopupContext CloseListener(Action<IPopupContext> onClose);
        /// <summary>
        /// 화면을 항상 위로 표시할지 설정합니다.
        /// </summary>
        /// <param name="isEnabled">항상 위 표시 여부</param>
        /// <returns>팝업의 문맥 객체</returns>
        IPopupContext Topmost(bool isEnabled);
        /// <summary>
        /// 팝업의 타이틀을 설정합니다. 
        /// </summary>
        /// <param name="title">팝업창의 타이틀.</param>
        /// <returns>팝업의 문맥 객체</returns>
        IPopupContext Title(string title);
        /// <summary>
        /// 타이틀 바의 표시 여부와 높이를 설정합니다.
        /// </summary>
        /// <param name="isEnabled">타이틀 바 표시 여부</param>
        /// <param name="isToolWindow">최소화/최대화 표시 여부</param>
        /// <param name="captionHeight">타이틀바 높이</param>
        /// <returns>팝업의 문맥 객체</returns>
        IPopupContext TitleBar(bool isEnabled, bool isToolWindow = false, double captionHeight = 28);
        /// <summary>
        /// 팝업창의 Resize 가능 여부를 설정합니다.
        /// </summary>
        /// <param name="isResize">Resize 가능 여부</param>
        /// <param name="isMinimize">Minimize 가능 여부</param>
        /// <param name="isResizeGrip">Resize Grip 사용 여부</param>
        /// <returns>팝업의 문맥 객체</returns>
        IPopupContext EnableResize(bool isResize, bool isMinimize = false, bool isResizeGrip = false);
        /// <summary>
        /// 작업 표시줄 표시 여부를 설정합니다. <br/>
        /// <seealso href="https://learn.microsoft.com/ko-kr/dotnet/api/system.windows.window.showintaskbar?view=windowsdesktop-8.0">참고 자료</seealso>
        /// </summary>
        /// <param name="isShow">작업 표시줄 표시 여부</param>
        /// <returns>팝업의 문맥 객체</returns>
        IPopupContext Taskbar(bool isShow);
        /// <summary>
        /// 팝업창이 처음 표시 될때 활성화 여부를 설정합니다. <br/>
        /// <seealso href="https://learn.microsoft.com/ko-kr/dotnet/api/system.windows.window.showactivated?view=windowsdesktop-8.0">참고 자료</seealso>
        /// </summary>
        /// <param name="isShow">팝업창 활성화 여부</param>
        /// <returns>팝업의 문맥 객체</returns>
        IPopupContext ShowActivated(bool isShow);
        /// <summary>
        /// 화면의 사이즈를 Manual로 설정합니다.
        /// </summary>
        /// <returns>팝업의 문맥 객체</returns>
        IPopupContext ManualSize();
        /// <summary>
        /// 화면의 사이즈를 WidthAndHeight 에 맞춰 설정합니다.
        /// </summary>
        /// <returns>팝업의 문맥 객체</returns>
        IPopupContext AutoSize();
        /// <summary>
        /// 화면의 사이즈를 Width에 맞춰 설정합니다.
        /// </summary>
        /// <returns>팝업의 문맥 객체</returns>
        IPopupContext AutoSizeWidth();
        /// <summary>
        /// 화면의 사이즈를 Height에 맞춰 설정합니다.
        /// </summary>
        /// <returns>팝업의 문맥 객체</returns>
        IPopupContext AutoSizeHeight();

        /// <summary>
        /// 화면의 위치를 수동으로 관리합니다. <br/>
        /// <seealso href="https://learn.microsoft.com/ko-kr/dotnet/api/system.windows.window.windowstartuplocation?view=windowsdesktop-8.0">참고자료</seealso>
        /// </summary>
        /// <param name="left">화면의 left 좌표.</param>
        /// <param name="top">화면의 top 좌표.</param>
        /// <returns>팝업의 문맥 객체</returns>
        IPopupContext ManualLocation(double left = 0, double top = 0);

        /// <summary>
        /// 화면의 위치를 수동으로 관리합니다.
        /// <br/>
        /// <seealso href="https://learn.microsoft.com/ko-kr/dotnet/api/system.windows.window.windowstartuplocation?view=windowsdesktop-8.0">참고자료</seealso>
        /// </summary>
        /// <param name="getLocation">화면의 위치를 얻어 올 수 있는 메서드.</param>
        /// <returns>팝업의 문맥 객체</returns>
        IPopupContext ManualLocation(Func<(double Width, double Height), (double Left, double Top)> getLocation);

        /// <summary>
        /// 화면의 위치를 화면을 중심으로 가운데에 표시합니다.<br/>
        /// <seealso href="https://learn.microsoft.com/ko-kr/dotnet/api/system.windows.window.windowstartuplocation?view=windowsdesktop-8.0">참고자료</seealso>
        /// </summary>
        /// <returns>팝업의 문맥 객체.</returns>
        IPopupContext ScreenLocation();

        /// <summary>
        /// 화면의 위치를 팝업창의 Owner를 기준으로 가운데에 표시합니다.<br/>
        /// <seealso href="https://learn.microsoft.com/ko-kr/dotnet/api/system.windows.window.windowstartuplocation?view=windowsdesktop-8.0">참고자료</seealso>
        /// </summary>
        /// <returns>팝업의 문맥 객체.</returns>
        IPopupContext OwnerLocation();

        /// <summary>
        /// 팝업의 상태를 최소화 상태로 설정합니다.<br/>
        /// <seealso cref="Maximize">팝업 최대화 상태.</seealso><br/>
        /// <seealso cref="Normal">팝업 기본 상태.</seealso><br/>
        /// <seealso href="https://learn.microsoft.com/ko-kr/dotnet/api/system.windows.window.windowstate?view=windowsdesktop-8.0">참고자료</seealso><br/>
        /// </summary>
        /// <returns>팝업의 문맥 객체.</returns>
        IPopupContext Minimize();
        /// <summary>
        /// 팝업의 상태를 최대화 상태로 설정합니다.<br/>
        /// <seealso cref="Minimize">팝업 최소화 상태.</seealso><br/>
        /// <seealso cref="Normal">팝업 기본 상태.</seealso><br/>
        /// <seealso href="https://learn.microsoft.com/ko-kr/dotnet/api/system.windows.window.windowstate?view=windowsdesktop-8.0">참고자료</seealso><br/>
        /// </summary>
        /// <returns>팝업의 문맥 객체</returns>
        IPopupContext Maximize();
        /// <summary>
        /// 팝업의 상태를 기본 상태로 설정합니다.<br/>
        /// <seealso cref="Minimize">팝업 최소화 상태.</seealso><br/>
        /// <seealso cref="Maximize">팝업 최대화 상태.</seealso><br/>
        /// <seealso href="https://learn.microsoft.com/ko-kr/dotnet/api/system.windows.window.windowstate?view=windowsdesktop-8.0">참고자료</seealso><br/>
        /// </summary>
        /// <returns>팝업의 문맥 객체</returns>
        IPopupContext Normal();
        #endregion
    }
}
