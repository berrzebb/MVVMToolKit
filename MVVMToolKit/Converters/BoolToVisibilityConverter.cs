using System.Globalization;
using System.Windows.Data;

namespace MVVMToolKit.Converters
{
    /// <summary>
    /// Bool을 Visibility로 변환해주는 컨버터
    /// </summary>
    public class BoolToVisibilityConverter : IValueConverter
    {
        /// <summary>
        /// 입력된 Boolean값이 True일때 사용되는 Visibility입니다.
        /// </summary>
        public Visibility TrueValue { get; set; } = Visibility.Visible;
        /// <summary>
        /// 입력된 Boolean값이 False일때 사용되는 Visibility입니다.
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
        public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            if (value is bool boolValue)
            {
                if (boolValue)
                {
                    return this.TrueValue;
                }

                return this.FalseValue;
            }

            return Binding.DoNothing;
        }

        /// <summary>
        /// Converts the back using the specified value
        /// </summary>
        /// <param name="value">The value</param>
        /// <param name="targetType">The target type</param>
        /// <param name="parameter">The parameter</param>
        /// <param name="culture">The culture</param>
        /// <returns>The object</returns>
        public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            return Binding.DoNothing;
        }
    }
}
