using System.Globalization;
using System.Windows.Data;
using MVVMToolKit.Hosting.Extensions;

namespace MVVMToolKit.Converters
{
    /// <summary>
    /// Enum 값을 Visibility 값으로 변환하는 컨버터 클래스입니다.
    /// </summary>
    public class EnumToVisibilityConverter : MarkupConverterExtension<EnumToVisibilityConverter>, IVisibilityConverter
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
            string? strEnum = value?.ToString();

            if (parameter != null && parameter.ToString()!.Equals(strEnum))
            {
                return this.TrueValue;
            }

            return this.FalseValue;
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