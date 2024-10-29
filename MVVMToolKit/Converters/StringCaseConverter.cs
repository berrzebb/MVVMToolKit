using System.Globalization;
using MVVMToolKit.Hosting.Extensions;

namespace MVVMToolKit.Converters
{
    /// <summary>
    /// 대소문자 여부
    /// </summary>
    public enum StringCase
    {
        /// <summary>
        /// 대문자
        /// </summary>
        Upper,
        /// <summary>
        /// 소문자
        /// </summary>
        Lower
    }

    /// <summary>
    /// 대소문자 변환
    /// </summary>
    public class StringCaseConverter : MarkupConverterExtension<StringCaseConverter>
    {
        /// <summary>
        /// 대소문자 여부
        /// </summary>
        public StringCase TargetCase { get; set; }

        /// <inheritdoc />
        public override object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            string? str = value?.ToString();
            return TargetCase == StringCase.Upper ? str?.ToUpper() : str?.ToLower();
        }

        /// <inheritdoc />
        public override object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            return value;
        }
    }
}
