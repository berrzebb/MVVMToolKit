namespace MVVMToolKit.Helper.Native
{
    /// <summary>
    /// 외부 DLL 파일의 이름을 상수로 정의하는 클래스입니다.<br/>
    /// </summary>
    internal class ExternDll
    {
        /// <summary>
        /// User32.dll 파일의 이름을 나타냅니다.<br/>
        /// 이 DLL은 사용자 인터페이스를 제어하는 Windows API 함수를 포함하고 있습니다.
        /// </summary>
        public const string User32 = "user32.dll";
        /// <summary>
        /// Shcore.dll 파일의 이름을 나타냅니다.<br/>
        /// 이 DLL은 고해상도 디스플레이를 지원하는 Windows API 함수를 포함하고 있습니다.
        /// </summary>
        public const string Shcore = "shcore.dll";
        /// <summary>
        /// D2D1.dll 파일의 이름을 나타냅니다.<br/>
        /// 이 DLL은 2D 그래픽 렌더링을 위한 Direct2D API 함수를 포함하고 있습니다.
        /// </summary>
        public const string D2D1 = "d2d1.dll";
    }
}
