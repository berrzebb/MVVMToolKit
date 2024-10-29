namespace MVVMToolKit.Interfaces
{
    /// <summary>
    /// 메시지 팝업의 유형을 나타내는 열거형입니다.<br/>
    /// </summary>
    public enum MessagePopupType
    {
        /// <summary>
        /// 확인 메시지 팝업 유형을 나타냅니다.<br/>
        /// </summary>
        Confirm,
        /// <summary>
        /// 예/아니오 메시지 팝업 유형을 나타냅니다.<br/>
        /// </summary>
        YesNo
    }
    /// <summary>
    /// 메시지 팝업의 아이콘 유형을 나타내는 열거형입니다.<br/>
    /// </summary>
    public enum MessagePopupIconType
    {
        /// <summary>
        /// 아이콘이 없는 메시지 팝업 유형을 나타냅니다.<br/>
        /// </summary>
        None,
        /// <summary>
        /// 경고 아이콘 메시지 팝업 유형을 나타냅니다.<br/>
        /// </summary>
        Warning,
        /// <summary>
        /// 오류 아이콘 메시지 팝업 유형을 나타냅니다.<br/>
        /// </summary>
        Error,
        /// <summary>
        /// 정보 아이콘 메시지 팝업 유형을 나타냅니다.<br/>
        /// </summary>
        Info
    }
}
