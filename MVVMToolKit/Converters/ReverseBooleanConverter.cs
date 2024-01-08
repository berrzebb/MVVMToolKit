using System.Globalization;
using System.Windows.Data;
using MVVMToolKit.Hosting.Extensions;

namespace MVVMToolKit.Converters
{
    /// <summary>
    /// Boolean 값을 반전시키는 컨버터 클래스입니다.<br/>
    /// IValueConverter 인터페이스를 구현합니다.
    /// </summary>
    public class ReverseBooleanConverter : MarkupConverterExtension<ReverseBooleanConverter>, IValueConverter
    {
        /// <summary>
        /// 주어진 boolean 값을 반전시킵니다.<br/>
        /// null이나 boolean이 아닌 값이 주어지면 항상 false를 반환합니다.
        /// </summary>
        /// <param name="value">변환할 boolean 값입니다.</param>
        /// <param name="targetType">대상 타입입니다. 이 컨버터에서는 무시됩니다.</param>
        /// <param name="parameter">변환에 사용할 추가 매개변수입니다. 이 컨버터에서는 무시됩니다.</param>
        /// <param name="culture">문화권 정보입니다. 이 컨버터에서는 무시됩니다.</param>
        /// <returns>변환된 boolean 값입니다.</returns>
        public override object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            return value switch
            {
                null => false,
                bool condition => !condition,
                _ => false
            };
        }
        /// <summary>
        /// 주어진 boolean 값을 반전시킵니다.<br/>
        /// null이나 boolean이 아닌 값이 주어지면 항상 false를 반환합니다.
        /// </summary>
        /// <param name="value">변환을 되돌릴 boolean 값입니다.</param>
        /// <param name="targetType">대상 타입입니다. 이 컨버터에서는 무시됩니다.</param>
        /// <param name="parameter">변환에 사용할 추가 매개변수입니다. 이 컨버터에서는 무시됩니다.</param>
        /// <param name="culture">문화권 정보입니다. 이 컨버터에서는 무시됩니다.</param>
        /// <returns>변환을 되돌린 boolean 값입니다.</returns>
        public override object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            return value switch
            {
                null => false,
                bool condition => !condition,
                _ => false
            };
        }
    }
}
