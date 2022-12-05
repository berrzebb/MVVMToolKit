using System;
using System.Globalization;
using System.Windows;
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
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is bool boolValue)
            {
                if (boolValue)
                {
                    return this.TrueValue;
                }
                else
                {
                    return this.FalseValue;
                }
            }

            return Binding.DoNothing;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return Binding.DoNothing;
        }
    }
}
