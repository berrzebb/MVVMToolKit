using System.Globalization;
using System.Windows.Data;
using MVVMToolKit.Hosting.Extensions;

namespace MVVMToolKit.Converters
{
    /// <summary>
    /// 숫자 비교 연산자를 나타내는 열거형입니다.
    /// </summary>
    public enum OpType : int
    {
        /// <summary>
        /// 동일함을 나타냅니다.
        /// </summary>
        Eq = 0,
        /// <summary>
        /// 작음을 나타냅니다.
        /// </summary>
        Less = 1,
        /// <summary>
        /// 작거나 같음을 나타냅니다.
        /// </summary>
        LessEq = 2,
        /// <summary>
        /// 큼을 나타냅니다.
        /// </summary>
        Greater = 3,
        /// <summary>
        /// 크거나 같음을 나타냅니다.
        /// </summary>
        GreaterEq = 4
    }

    /// <summary>
    /// 숫자 조건을 Visibility 값으로 변환하는 컨버터 클래스입니다.
    /// IVisibilityConverter 인터페이스를 구현합니다.
    /// </summary>
    public class NumericConditionToVisibleConverter : MarkupConverterExtension<NumericConditionToVisibleConverter>, IVisibilityConverter
    {
        /// <summary>
        /// 입력된 값이 조건과 동일한 경우 사용되는 Visibility입니다.
        /// </summary>
        public Visibility TrueValue { get; set; } = Visibility.Visible;
        /// <summary>
        /// 입력된 값이 조건에 일치하지 않은 경우 사용되는 Visibility입니다.
        /// </summary>
        public Visibility FalseValue { get; set; } = Visibility.Collapsed;

        /// <summary>
        /// 비교 대상 값입니다.
        /// </summary>
        public object? Target { get; set; } = 0;
        /// <summary>
        /// 비교 연산자 타입입니다.<br/>
        /// OpType.Eq: 동일함을 나타냅니다.<br/>
        /// OpType.Less: 작음을 나타냅니다.<br/>
        /// OpType.LessEq: 작거나 같음을 나타냅니다.<br/>
        /// OpType.Greater: 큼을 나타냅니다.<br/>
        /// OpType.GreaterEq: 크거나 같음을 나타냅니다.
        /// </summary>
        public OpType Op { get; set; } = OpType.Eq;

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
            if (!value.IsNumericType() || !Target.IsNumericType())
            {
                return FalseValue;
            }

            Func<object?, bool> comparer;
            switch (Op)
            {
                case OpType.Less: comparer = value.Less; break;
                case OpType.LessEq: comparer = value.LessEquals; break;
                case OpType.Greater: comparer = value.Greater; break;
                case OpType.GreaterEq: comparer = value.GreaterEquals; break;
                default: comparer = value.Eq; break;
            }

            return comparer(Target) ? TrueValue : FalseValue;
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