using System.Globalization;
using System.Windows.Data;
using MVVMToolKit.Helper;

namespace MVVMToolKit.Converters
{
    /// <summary>
    /// The base 64 to bitmap image converter class
    /// </summary>
    /// <seealso cref="IValueConverter"/>
    public class Base64ToBitmapImageConverter : IValueConverter
    {
        /// <summary>
        /// Converts the value
        /// </summary>
        /// <param name="value">The value</param>
        /// <param name="targetType">The target type</param>
        /// <param name="parameter">The parameter</param>
        /// <param name="culture">The culture</param>
        /// <returns>The object</returns>
        public object? Convert(object? value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
            {
                return null;
            }

            return ImageHelper.Base64ToBitmapImage(value.ToString()) ?? null;
        }

        /// <summary>
        /// Converts the back using the specified value
        /// </summary>
        /// <param name="value">The value</param>
        /// <param name="targetType">The target type</param>
        /// <param name="parameter">The parameter</param>
        /// <param name="culture">The culture</param>
        /// <returns>The object</returns>
        public object? ConvertBack(object? value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }
}
