using System.Globalization;
using System.Windows.Data;

namespace MVVMToolKit.Converters
{
    /// <summary>
    /// Object�� Visibility�� ��ȯ���ִ� ������
    /// </summary>
    /// <seealso cref="IValueConverter"/>
    public class ObjectToVisibleConverter : IValueConverter
    {
        /// <summary>
        /// �Էµ� Boolean���� True�϶� ���Ǵ� Visibility�Դϴ�.
        /// </summary>
        public Visibility TrueValue { get; set; } = Visibility.Visible;
        /// <summary>
        /// �Էµ� Boolean���� False�϶� ���Ǵ� Visibility�Դϴ�.
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
            if (value == null)
            {
                return this.FalseValue;
            }

            var name = value.GetType().Name;
            if (name == (string?)parameter)
            {
                return this.TrueValue;
            }

            return this.FalseValue;
        }

        /// <summary>
        /// Converts the back using the specified value
        /// </summary>
        /// <param name="value">The value</param>
        /// <param name="targetType">The target type</param>
        /// <param name="parameter">The parameter</param>
        /// <param name="culture">The culture</param>
        /// <returns>The object</returns>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return Binding.DoNothing;
        }
    }
}
