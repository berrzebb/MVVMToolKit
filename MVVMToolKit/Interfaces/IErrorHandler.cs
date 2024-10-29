namespace MVVMToolKit.Interfaces
{
    /// <summary>
    /// 예외 핸들링 인터페이스
    /// </summary>
    public interface IErrorHandler
    {/// <summary>
     /// Error를 핸들링 합니다.
     /// </summary>
     /// <param name="ex">핸들링할 예외</param>
        void HandleError(Exception ex);
    }
}
