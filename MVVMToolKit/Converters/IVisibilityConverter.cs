using System.Windows.Data;

namespace MVVMToolKit.Converters
{
    /// <summary>
    /// Visibility 값을 반환하는 인터페이스입니다.
    /// </summary>
    public interface IVisibilityConverter : IValueConverter
    {
        /// <summary>
        /// True일 경우 반환할 Visibility 값입니다.
        /// </summary>
        Visibility TrueValue { get; set; }
        /// <summary>
        /// False일 경우 반환할 Visibility 값입니다.
        /// </summary>
        Visibility FalseValue { get; set; }
    }
}
