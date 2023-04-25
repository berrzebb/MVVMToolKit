using System.Globalization;
using System.Windows.Data;
using MVVMToolKit.Hosting.Extensions;

namespace MVVMToolKit.Converters
{
    public enum OpType : int
    {
        Eq = 0,
        Less = 1,
        LessEq = 2,
        Greater = 3,
        GreaterEq = 4
    }

    /// <summary>
    /// 입력된 조건을 Visibility로 변환해주는 컨버터
    /// </summary>
    public class NumericConditionToVisibleConverter : IVisibilityConverter
    {
        /// <summary>
        /// 입력된 값이 조건과 동일한 경우 사용되는 Visibility입니다.
        /// </summary>
        public Visibility TrueValue { get; set; } = Visibility.Visible;
        /// <summary>
        /// 입력된 값이 조건에 일치하지 않은 경우 사용되는 Visibility입니다.
        /// </summary>
        public Visibility FalseValue { get; set; } = Visibility.Collapsed;

        public object? Target { get; set; } = 0;

        public OpType Op { get; set; } = OpType.Eq;

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
            if (!value.IsNumericType() || !this.Target.IsNumericType())
            {
                return this.FalseValue;
            }

            Func<object?, bool> comparer;
            switch (this.Op)
            {
                case OpType.Less: comparer = value.Less; break;
                case OpType.LessEq: comparer = value.LessEquals; break;
                case OpType.Greater: comparer = value.Greater; break;
                case OpType.GreaterEq: comparer = value.GreaterEquals; break;
                default: comparer = value.Eq; break;
            }

            return comparer(this.Target) ? this.TrueValue : this.FalseValue;
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