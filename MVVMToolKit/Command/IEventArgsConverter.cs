namespace MVVMToolKit.Command
{
    /// <summary>
    /// <see cref="EventToCommand"/> 클래스에서 EventArgs를 변환하는 데 사용되는 변환기의 정의입니다.<br/>
    /// <see cref="EventToCommand.PassEventArgsToCommand"/> 속성이 true인 경우에 사용됩니다.<br/>
    /// 이 클래스의 인스턴스를 EventToCommand 인스턴스의 <see cref="EventToCommand.EventArgsConverter"/> 속성에 설정합니다.
    /// </summary>
    ////[ClassInfo(typeof(EventToCommand))]
    public interface IEventArgsConverter
    {
        /// <summary>
        /// EventArgs 인스턴스를 변환하는 데 사용되는 메서드입니다.
        /// </summary>
        /// <param name="value">EventToCommand 인스턴스가 처리하는 이벤트에 의해 전달된 EventArgs의 인스턴스입니다.</param>
        /// <param name="parameter">변환에 사용되는 선택적 매개변수입니다. 이 값을 설정하려면 <br/>
        /// <see cref="EventToCommand.EventArgsConverterParameter"/> 속성을 사용합니다. 이 값은 null일 수 있습니다.</param>
        /// <returns>변환된 값입니다.</returns>
        object? Convert(object? value, object parameter);
    }
}
