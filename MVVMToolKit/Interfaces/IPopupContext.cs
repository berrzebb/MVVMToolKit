namespace MVVMToolKit.Interfaces
{
    public interface IPopupContext
    {
        object? Source { get; }
        object? DataContext { get; }
        bool IsAlive { get; }
        IntPtr Handle { get; }
        object? GetOwner();

        bool Activate();
        void Close();


        #region Fluent Interface
        /// <summary>
        /// 팝업을 표시합니다.
        /// </summary>
        /// <param name="isModal">Modal 형태의 팝업을 사용할지 설정.</param>
        /// <returns></returns>
        IPopupContext Show(bool isModal = false);

        /// <summary>
        /// 팝업 종료 시 수행할 작업을 등록합니다.
        /// </summary>
        /// <param name="onClose">팝업 종료 시 수행할 작업.</param>
        /// <returns></returns>
        IPopupContext CloseListener(Action<IPopupContext> onClose);
        /// <summary>
        /// 화면을 항상 위로 표시할지 설정합니다.
        /// </summary>
        /// <param name="isEnabled">항상 위 표시 여부</param>
        /// <returns></returns>
        IPopupContext Topmost(bool isEnabled);
        /// <summary>
        /// 팝업의 타이틀을 설정합니다. 
        /// </summary>
        /// <param name="title">팝업창의 타이틀.</param>
        /// <returns></returns>
        IPopupContext Title(string title);
        /// <summary>
        /// 타이틀 바의 표시 여부와 높이를 설정합니다.
        /// </summary>
        /// <param name="isEnabled">타이틀 바 표시 여부</param>
        /// <param name="isToolWindow">최소화/최대화 표시 여부</param>
        /// <param name="captionHeight">타이틀바 높이</param>
        /// <returns></returns>
        IPopupContext TitleBar(bool isEnabled, bool isToolWindow = false, double captionHeight = 28);
        /// <summary>
        /// 팝업창의 Resize 가능 여부를 설정합니다.
        /// </summary>
        /// <param name="isResize">Resize 가능 여부</param>
        /// <param name="isMinimize">Minimize 가능 여부</param>
        /// <param name="isResizeGrip">Resize Grip 사용 여부</param>
        /// <returns></returns>
        IPopupContext EnableResize(bool isResize, bool isMinimize = false, bool isResizeGrip = false);
        /// <summary>
        /// 작업 표시줄 표시 여부를 설정합니다. <br/>
        /// <seealso href="https://learn.microsoft.com/ko-kr/dotnet/api/system.windows.window.showintaskbar?view=windowsdesktop-8.0">참고 자료</seealso>
        /// </summary>
        /// <param name="isShow">작업 표시줄 표시 여부</param>
        /// <returns></returns>
        IPopupContext Taskbar(bool isShow);
        /// <summary>
        /// 팝업창이 처음 표시 될때 활성화 여부를 설정합니다. <br/>
        /// <seealso href="https://learn.microsoft.com/ko-kr/dotnet/api/system.windows.window.showactivated?view=windowsdesktop-8.0">참고 자료</seealso>
        /// </summary>
        /// <param name="isShow">팝업창 활성화 여부</param>
        /// <returns></returns>
        IPopupContext ShowActivated(bool isShow);
        /// <summary>
        /// 화면의 사이즈를 Manual로 설정합니다.
        /// </summary>
        /// <returns></returns>
        IPopupContext ManualSize();
        /// <summary>
        /// 화면의 사이즈를 WidthAndHeight 에 맞춰 설정합니다.
        /// </summary>
        /// <returns></returns>
        IPopupContext AutoSize();
        /// <summary>
        /// 화면의 사이즈를 Width에 맞춰 설정합니다.
        /// </summary>
        /// <returns></returns>
        IPopupContext AutoSizeWidth();
        /// <summary>
        /// 화면의 사이즈를 Height에 맞춰 설정합니다.
        /// </summary>
        /// <returns></returns>
        IPopupContext AutoSizeHeight();

        /// <summary>
        /// 화면의 위치를 수동으로 관리합니다. <br/>
        /// <seealso href="https://learn.microsoft.com/ko-kr/dotnet/api/system.windows.window.windowstartuplocation?view=windowsdesktop-8.0">참고자료</seealso>
        /// </summary>
        /// <param name="left">화면의 left 좌표.</param>
        /// <param name="top">화면의 top 좌표.</param>
        /// <returns></returns>
        IPopupContext ManualLocation(double left = 0, double top = 0);
        /// <summary>
        /// 화면의 위치를 수동으로 관리합니다.
        /// <br/>
        /// <seealso href="https://learn.microsoft.com/ko-kr/dotnet/api/system.windows.window.windowstartuplocation?view=windowsdesktop-8.0">참고자료</seealso>
        /// </summary>
        /// <param name="getLocation">화면의 위치를 얻어 올 수 있는 메서드.</param>

        /// <returns></returns>

        IPopupContext ManualLocation(Func<(double Width, double Height), (double Left, double Top)> getLocation);
        /// <summary>
        /// 화면의 위치를 화면을 중심으로 가운데에 표시합니다.<br/>
        /// <seealso href="https://learn.microsoft.com/ko-kr/dotnet/api/system.windows.window.windowstartuplocation?view=windowsdesktop-8.0">참고자료</seealso>
        /// </summary>
        /// <returns></returns>
        IPopupContext ScreenLocation();
        /// <summary>
        /// 화면의 위치를 팝업창의 Owner를 기준으로 가운데에 표시합니다.<br/>
        /// <seealso href="https://learn.microsoft.com/ko-kr/dotnet/api/system.windows.window.windowstartuplocation?view=windowsdesktop-8.0">참고자료</seealso>

        /// </summary>
        /// <returns></returns>
        IPopupContext OwnerLocation();

        /// <summary>
        /// 팝업의 상태를 최소화 상태로 설정합니다.<br/>
        /// <seealso cref="Maximize">팝업 최대화 상태.</seealso><br/>
        /// <seealso cref="Normal">팝업 기본 상태.</seealso><br/>
        /// <seealso href="https://learn.microsoft.com/ko-kr/dotnet/api/system.windows.window.windowstate?view=windowsdesktop-8.0">참고자료</seealso><br/>
        /// </summary>
        /// <returns></returns>
        IPopupContext Minimize();
        /// <summary>
        /// 팝업의 상태를 최대화 상태로 설정합니다.<br/>
        /// <seealso cref="Minimize">팝업 최소화 상태.</seealso><br/>
        /// <seealso cref="Normal">팝업 기본 상태.</seealso><br/>
        /// <seealso href="https://learn.microsoft.com/ko-kr/dotnet/api/system.windows.window.windowstate?view=windowsdesktop-8.0">참고자료</seealso><br/>
        /// </summary>
        /// <returns></returns>
        IPopupContext Maximize();
        /// <summary>
        /// 팝업의 상태를 기본 상태로 설정합니다.<br/>
        /// <seealso cref="Minimize">팝업 최소화 상태.</seealso><br/>
        /// <seealso cref="Maximize">팝업 최대화 상태.</seealso><br/>
        /// <seealso href="https://learn.microsoft.com/ko-kr/dotnet/api/system.windows.window.windowstate?view=windowsdesktop-8.0">참고자료</seealso><br/>
        /// </summary>
        /// <returns></returns>
        IPopupContext Normal();
        #endregion
    }
}
