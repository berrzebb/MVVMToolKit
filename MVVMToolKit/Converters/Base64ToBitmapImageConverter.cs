using System.Globalization;
using System.Windows.Data;
using MVVMToolKit.Helper;
using MVVMToolKit.Hosting.Extensions;

namespace MVVMToolKit.Converters
{
    /// <summary>
    /// Base64 문자열을 Bitmap 이미지로 변환하는 컨버터 클래스입니다.
    /// </summary>
    /// <seealso cref="IValueConverter"/>
    public class Base64ToBitmapImageConverter : MarkupConverterExtension<Base64ToBitmapImageConverter>
    {
        /// <summary>
        /// 주어진 값을 변환합니다.
        /// </summary>
        /// <param name="value">변환할 Base64 문자열입니다.</param>
        /// <param name="targetType">대상 타입입니다. 이 컨버터에서는 무시됩니다.</param>
        /// <param name="parameter">변환에 사용할 추가 매개변수입니다. 이 컨버터에서는 무시됩니다.</param>
        /// <param name="culture">문화권 정보입니다. 이 컨버터에서는 무시됩니다.</param>
        /// <returns>변환된 Bitmap 이미지 객체입니다.</returns>
        public override object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            if (value == null)
            {
                return null;
            }

            return ImageHelper.Base64ToBitmapImage(value.ToString()) ?? null;
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
            return null;
        }
    }
}
