namespace MVVMToolKit.Interfaces
{
    /// <summary>
    /// 팝업에서 사용할 ViewModel의 인터페이스입니다.
    /// </summary>
    public interface IDialogContext
    {
        /// <summary>
        /// 팝업의 리소스를 정리합니다.
        /// </summary>
        void Cleanup();
    }
}
