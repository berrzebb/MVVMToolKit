using System;
using System.Globalization;
using System.Windows.Data;

namespace MVVMToolKit.Converters
{
    /// <summary>
    /// Bool을 반대로 변환해주는 컨버터
    /// </summary>
    public class ReverseBooleanConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is bool boolValue)
            {
                return !boolValue;
            }

            return Binding.DoNothing;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return Binding.DoNothing;
        }
    }
}
