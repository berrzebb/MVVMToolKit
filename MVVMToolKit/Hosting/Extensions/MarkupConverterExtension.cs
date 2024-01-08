using System.Globalization;
using System.Windows.Data;
using System.Windows.Markup;

namespace MVVMToolKit.Hosting.Extensions
{
    /// <summary>
    /// `MarkupConverterExtension{T}`는 `MarkupExtension`과 `IValueConverter` 인터페이스를 구현하는 추상 클래스입니다.<br/>
    /// 이 클래스는 XAML 마크업에서 사용할 수 있는 값 변환기를 제공합니다.<br/>
    /// </summary>
    /// <typeparam name="T">변환기의 타입을 지정하는 제네릭 매개변수입니다.</typeparam>
    public abstract class MarkupConverterExtension<T> : MarkupExtension, IValueConverter
        where T : class, new()

    {
        /// <summary>
        /// `_converter`는 `T` 타입의 인스턴스를 생성하고 이를 저장하는 `Lazy{T}` 타입의 필드입니다.<br/>
        /// </summary>
        private static readonly Lazy<T> _converter = new(() => new T());
        /// <summary>
        /// `ProvideValue` 메서드는 XAML 마크업에서 이 확장을 사용할 때 호출되며, `_converter` 필드에 저장된 값 변환기 인스턴스를 반환합니다.<br/>
        /// </summary>
        public override object? ProvideValue(IServiceProvider serviceProvider) => _converter.Value;

        public abstract object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture);
        public abstract object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture);

    }
    /// <summary>
    /// `MarkupMultiConverterExtension{T}`는 `MarkupExtension`과 `IMultiValueConverter` 인터페이스를 구현하는 추상 클래스입니다.<br/>
    /// 이 클래스는 여러 입력 값을 하나의 출력 값으로 변환하는 변환기를 제공합니다.<br/>
    /// </summary>
    /// <typeparam name="T">변환기의 타입을 지정하는 제네릭 매개변수입니다.</typeparam>
    public abstract class MarkupMultiConverterExtension<T> : MarkupExtension, IMultiValueConverter
    where T : class, new()
    {
        /// <summary>
        /// `_converter`는 `T` 타입의 인스턴스를 생성하고 이를 저장하는 `Lazy{T}` 타입의 필드입니다.<br/>
        /// </summary>
        private static readonly Lazy<T> _converter = new(() => new T());
        /// <summary>
        /// `ProvideValue` 메서드는 XAML 마크업에서 이 확장을 사용할 때 호출되며, `_converter` 필드에 저장된 값 변환기 인스턴스를 반환합니다.<br/>
        /// </summary>
        public override object? ProvideValue(IServiceProvider serviceProvider) => _converter.Value;

        public abstract object Convert(object[] values, Type targetType, object? parameter, CultureInfo culture);
        public abstract object[] ConvertBack(object value, Type[] targetTypes, object? parameter, CultureInfo culture);

    }
}
