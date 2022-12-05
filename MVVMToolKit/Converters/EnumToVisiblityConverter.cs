using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace MVVMToolKit.Converters
{
    /// <summary>
    /// Enum을 Visibility로 변환해주는 컨버터
    /// </summary>
    public class EnumToVisibilityConverter : IValueConverter
    {
        /// <summary>
        /// 입력된 Enum값이 파라미터와 동일한 경우 사용되는 Visibility입니다.
        /// </summary>
        public Visibility TrueValue { get; set; } = Visibility.Visible;
        /// <summary>
        /// 입력된 Enum값이 파라미터와 동일하지 않은 경우 사용되는 Visibility입니다.
        /// </summary>
        public Visibility FalseValue { get; set; } = Visibility.Collapsed;
        
        public object Convert(object value, Type targetType, object? parameter, CultureInfo culture)
        {
            string? strEnum = value.ToString();

            if (parameter != null && parameter.ToString()!.Equals(strEnum))
            {
                return this.TrueValue;
            }
            else
            {
                return this.FalseValue;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return Binding.DoNothing;
        }
    }
}