using System;
using System.Globalization;
using System.Linq;
using System.Windows.Data;
using System.Windows.Markup;

namespace MVVMToolKit.Converters
{
    /// <summary>
    /// Char 배열을 String형태로 변환합니다.
    /// </summary>
    public class CharToStringConverter : MarkupExtension, IValueConverter
    {
        /// <summary>
        /// 문자열 길이
        /// </summary>
        private int _charLength;
        /// <inheritdoc/>
        public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            if (value is char[] charArray)
            {
                _charLength = charArray.Length;
                return new string(charArray);
            }

            if (value is string)
            {
                return value;
            }

            return "";
        }
        /// <inheritdoc/>

        public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            if (value is string text)
            {
                return text.Substring(0, Math.Min(text.Length, _charLength)).ToArray();
            }

            return null;
        }
        /// <inheritdoc/>

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return this;
        }
    }
}
