using System.Globalization;
using System.Windows.Data;
using MVVMToolKit.Helper;
using MVVMToolKit.Hosting.Extensions;

namespace MVVMToolKit.Converters
{
    /// <summary>
    /// Base64 ���ڿ��� Bitmap �̹����� ��ȯ�ϴ� ������ Ŭ�����Դϴ�.
    /// </summary>
    /// <seealso cref="IValueConverter"/>
    public class Base64ToBitmapImageConverter : MarkupConverterExtension<Base64ToBitmapImageConverter>
    {
        /// <summary>
        /// �־��� ���� ��ȯ�մϴ�.
        /// </summary>
        /// <param name="value">��ȯ�� Base64 ���ڿ��Դϴ�.</param>
        /// <param name="targetType">��� Ÿ���Դϴ�. �� �����Ϳ����� ���õ˴ϴ�.</param>
        /// <param name="parameter">��ȯ�� ����� �߰� �Ű������Դϴ�. �� �����Ϳ����� ���õ˴ϴ�.</param>
        /// <param name="culture">��ȭ�� �����Դϴ�. �� �����Ϳ����� ���õ˴ϴ�.</param>
        /// <returns>��ȯ�� Bitmap �̹��� ��ü�Դϴ�.</returns>
        public override object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            if (value == null)
            {
                return null;
            }

            return ImageHelper.Base64ToBitmapImage(value.ToString()) ?? null;
        }

        /// <summary>
        /// ��ȯ�� �ǵ����ϴ�. �� �����Ϳ����� �������� �����Ƿ� �׻� null�� ��ȯ�մϴ�.
        /// </summary>
        /// <param name="value">��ȯ�� �ǵ��� ���Դϴ�.</param>
        /// <param name="targetType">��� Ÿ���Դϴ�. �� �����Ϳ����� ���õ˴ϴ�.</param>
        /// <param name="parameter">��ȯ�� ����� �߰� �Ű������Դϴ�. �� �����Ϳ����� ���õ˴ϴ�.</param>
        /// <param name="culture">��ȭ�� �����Դϴ�. �� �����Ϳ����� ���õ˴ϴ�.</param>
        /// <returns>�׻� null�Դϴ�.</returns>
        public override object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            return null;
        }
    }
}
