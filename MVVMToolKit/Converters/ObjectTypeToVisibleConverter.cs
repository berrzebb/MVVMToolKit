

using System.Globalization;
using System.Windows.Data;
using MVVMToolKit.Hosting.Extensions;

namespace MVVMToolKit.Converters
{
    /// <summary>
    /// 객체의 타입을 Visibility 값으로 변환하는 컨버터 클래스입니다.<br/>
    /// IVisibilityConverter 인터페이스를 구현합니다.
    /// </summary>
    public class ObjectTypeToVisibleConverter : MarkupConverterExtension<ObjectTypeToVisibleConverter>, IVisibilityConverter
    {
        /// <summary>
        /// 입력된 객체의 타입이 파라미터와 동일한 경우 사용되는 Visibility입니다.
        /// </summary>
        public Visibility TrueValue { get; set; } = Visibility.Visible;
        /// <summary>
        /// 입력된 객체의 타입이 파라미터와 동일하지 않은 경우 사용되는 Visibility입니다.
        /// </summary>
        public Visibility FalseValue { get; set; } = Visibility.Collapsed;


        /// <summary>
        /// 주어진 객체의 타입을 Visibility 값으로 변환합니다.<br/>
        /// 객체의 타입이 파라미터와 동일하면 TrueValue를, 그렇지 않으면 FalseValue를 반환합니다.
        /// </summary>
        /// <param name="value">변환할 객체입니다.</param>
        /// <param name="targetType">대상 타입입니다. 이 컨버터에서는 무시됩니다.</param>
        /// <param name="parameter">변환에 사용할 추가 매개변수입니다. 이 컨버터에서는 무시됩니다.</param>
        /// <param name="culture">문화권 정보입니다. 이 컨버터에서는 무시됩니다.</param>
        /// <returns>변환된 Visibility 값입니다.</returns>
        public override object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            if (value == null)
            {
                return FalseValue;
            }

            var name = value.GetType().Name;
            if (name == (string?)parameter)
            {
                return TrueValue;
            }

            return FalseValue;
        }

        /// <summary>
        /// 변환을 되돌립니다. 이 컨버터에서는 지원되지 않으므로 항상 null을 반환합니다.
        /// </summary>
        /// <param name="value">변환을 되돌릴 값입니다.</param>
        /// <param name="targetType">대상 타입입니다. 이 컨버터에서는 무시됩니다.</param>
        /// <param name="parameter">변환에 사용할 추가 매개변수입니다. 이 컨버터에서는 무시됩니다.</param>
        /// <param name="culture">문화권 정보입니다. 이 컨버터에서는 무시됩니다.</param>
        /// <returns>항상 null입니다.</returns>/param>
        /// <returns>The object</returns>
        public override object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            return Binding.DoNothing;
        }
    }
}
