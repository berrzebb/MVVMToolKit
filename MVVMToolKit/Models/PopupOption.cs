using System.Windows;

namespace MVVMToolKit.Models
{
    /// <summary>
    /// 팝업 다이얼로그 생성을 위한 설정 객체
    /// </summary>
    public class PopupOption
    {
        /// <summary>
        ///  팝업의 타이틀
        /// </summary>
        public string? Title { get; set; } = "";
        /// <summary>
        ///  팝업의 가로크기
        /// </summary>
        public double Width { get; set; } = 0;
        /// <summary>
        /// 팝업의 세로크기
        /// </summary>
        public double Height { get; set; } = 0;
        /// <summary>
        /// Dependency Injection 사용 여부(기본값 : true)
        /// </summary>
        public bool IsDependencyInjection { get; set; } = true;
        /// <summary>
        /// 팝업을 띄우기 위한 창의 타입(기본값 : Default)
        /// </summary>
        public string HostType { get; set; } = "Default";
        /// <summary>
        /// 팝업의 제목바 Template
        /// </summary>
        public DataTemplate? TitleBarTemplate { get; set; } = null;

    }
}
