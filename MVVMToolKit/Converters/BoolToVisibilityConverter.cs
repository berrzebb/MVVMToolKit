using System.Globalization;
using System.Windows.Data;
using MVVMToolKit.Hosting.Extensions;

namespace MVVMToolKit.Converters
{
    /// <summary>
    /// Bool 값을 Visibility 값으로 변환하는 컨버터 클래스입니다.
    /// </summary>
    public class BoolToVisibilityConverter : MarkupConverterExtension<BoolToVisibilityConverter>, IVisibilityConverter
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
        /// 주어진 bool 값을 Visibility 값으로 변환합니다.
        /// </summary>
        /// <param name="value">변환할 bool 값입니다.</param>
        /// <param name="targetType">대상 타입입니다. 이 컨버터에서는 무시됩니다.</param>
        /// <param name="parameter">변환에 사용할 추가 매개변수입니다. 이 컨버터에서는 무시됩니다.</param>
        /// <param name="culture">문화권 정보입니다. 이 컨버터에서는 무시됩니다.</param>
        /// <returns>변환된 Visibility 값입니다.</returns>
        public override object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            if (value is bool boolValue)
            {
                return boolValue ? TrueValue : FalseValue;
            }

            return Binding.DoNothing;
        }

        /// <summary>
        /// 변환을 되돌립니다. 이 컨버터에서는 지원되지 않으므로 항상 null을 반환합니다.
        /// </summary>
        /// <param name="value">변환을 되돌릴 값입니다.</param>
        /// <param name="targetType">대상 타입입니다. 이 컨버터에서는 무시됩니다.</param>
        /// <param name="parameter">변환에 사용할 추가 매개변수입니다. 이 컨버터에서는 무시됩니다.</param>
        /// <param name="culture">문화권 정보입니다. 이 컨버터에서는 무시됩니다.</param>
        /// <returns>항상 null입니다.</returns>
        public override object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            return Binding.DoNothing;
        }
    }
}
