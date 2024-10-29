

using System.Globalization;
using System.Windows.Data;
using MVVMToolKit.Hosting.Extensions;

namespace MVVMToolKit.Converters
{
    /// <summary>
    /// 입력된 값과 같은지 체크하고, Visibility 값으로 변환하는 컨버터 클래스입니다.
    /// </summary>
    public class ObjectValueToVisibilityConverter : MarkupConverterExtension<ObjectValueToVisibilityConverter>, IVisibilityConverter
    {
        /// <summary>
        /// True일 경우 반환할 Visibility 값입니다.
        /// </summary>
        public Visibility TrueValue { get; set; } = Visibility.Visible;
        /// <summary>
        /// False일 경우 반환할 Visibility 값입니다.
        /// </summary>
        public Visibility FalseValue { get; set; } = Visibility.Collapsed;

        /// <summary>
        /// Converts the value
        /// </summary>
        /// <param name="value">The value</param>
        /// <param name="targetType">The target type</param>
        /// <param name="parameter">The parameter</param>
        /// <param name="culture">The culture</param>
        /// <returns>The object</returns>
        public override object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            return value == parameter ? TrueValue : FalseValue;
        }

        /// <summary>
        /// Converts the back using the specified value
        /// </summary>
        /// <param name="value">The value</param>
        /// <param name="targetType">The target type</param>
        /// <param name="parameter">The parameter</param>
        /// <param name="culture">The culture</param>
        /// <returns>The object</returns>
        public override object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            return Binding.DoNothing;
        }
    }
}
