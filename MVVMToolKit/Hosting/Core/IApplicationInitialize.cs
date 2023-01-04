namespace MVVMToolKit.Hosting.Core
{
    /// <summary>
    /// The application initialize interface
    /// </summary>
    public interface IApplicationInitialize
    {
        /// <summary>
        /// <see cref="cref="Application.Run()"/> 을 실행하기 전에 호출 됩니다. 이 작업은 UI 스레드에서 발생합니다.
        /// </summary>
        void Initialize();
    }
}
