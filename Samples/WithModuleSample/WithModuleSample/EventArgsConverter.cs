using System.Windows.Markup;
using MVVMToolKit.Command;

namespace WithModuleSample
{

    public class EventArgsConverter : MarkupExtension, IEventArgsConverter
    {
        /// <summary>
        /// `_converter`는 `T` 타입의 인스턴스를 생성하고 이를 저장하는 `Lazy{T}` 타입의 필드입니다.<br/>
        /// </summary>
        private static readonly Lazy<EventArgsConverter> converter = new(() => new EventArgsConverter());
        /// <summary>
        /// `ProvideValue` 메서드는 XAML 마크업에서 이 확장을 사용할 때 호출되며, `converter` 필드에 저장된 값 변환기 인스턴스를 반환합니다.<br/>
        /// </summary>
        public override object? ProvideValue(IServiceProvider serviceProvider) => converter.Value;
        public object? Convert(object? value, object parameter)
        {
            if (value is not EventArgs eventArgs) return null;
            return (eventArgs, parameter);
        }
    }
}
