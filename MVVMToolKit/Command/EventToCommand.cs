using System.Windows.Input;
using Microsoft.Xaml.Behaviors;

namespace MVVMToolKit.Command
{
    /// <summary>
    /// 이 <see cref="T:System.Windows.Interactivity.TriggerAction`1" />는 어떤 FrameworkElement의 어떤 이벤트든지 <see cref="ICommand" />에 바인딩하는 데 사용할 수 있습니다.
    /// 일반적으로, 이 요소는 XAML에서 첨부된 요소를 ViewModel에 위치한 명령어에 연결하는 데 사용됩니다. 이 트리거는 FrameworkElement 또는 FrameworkElement에서 파생된 클래스에만 첨부할 수 있습니다.
    /// <para>발생한 이벤트의 EventArgs에 접근하려면, RelayCommand&lt;EventArgs&gt;를 사용하고 CommandParameter와 CommandParameterValue를 비워두세요!</para>
    /// </summary>
    public class EventToCommand : TriggerAction<DependencyObject>
    {
        /// <summary>
        /// <see cref="Command" /> 종속성 속성을 식별합니다.
        /// </summary>
        public static readonly DependencyProperty CommandProperty = DependencyProperty.Register(
            nameof(Command),
            typeof(ICommand),
            typeof(EventToCommand),
            new PropertyMetadata(
                null,
                (s, e) => OnCommandChanged(s as EventToCommand, e)));
        /// <summary>
        /// <see cref="CommandParameter" /> 종속성 속성을 식별합니다.
        /// </summary>
        public static readonly DependencyProperty CommandParameterProperty = DependencyProperty.Register(
            nameof(CommandParameter),
            typeof(object),
            typeof(EventToCommand),

            new PropertyMetadata(null, (s, e) =>
            {
                var sender = s as EventToCommand;
                if (sender == null)
                {
                    return;
                }

                if (sender.AssociatedObject == null)
                {
                    return;
                }

                sender.EnableDisableElement();
            })
            );

        /// <summary>
        /// <see cref="MustToggleIsEnabled" /> 종속성 속성을 식별합니다.
        /// </summary>
        public static readonly DependencyProperty MustToggleIsEnabledProperty = DependencyProperty.Register(
            nameof(MustToggleIsEnabled),
            typeof(bool),
            typeof(EventToCommand),
            new PropertyMetadata(
                false,
                (s, e) =>
                {
                    var sender = s as EventToCommand;
                    if (sender == null)
                    {
                        return;
                    }

                    if (sender.AssociatedObject == null)
                    {
                        return;
                    }

                    sender.EnableDisableElement();
                }));

        /// <summary>
        /// <see cref="EventArgsConverterParameter" /> 종속성 속성을 식별합니다.
        /// </summary>
        public static readonly DependencyProperty EventArgsConverterParameterProperty = DependencyProperty.Register(
            nameof(EventArgsConverterParameter),
            typeof(object),
            typeof(EventToCommand),
            new PropertyMetadata(null));

        /// <summary>
        /// <see cref="AlwaysInvokeCommand" /> 종속성 속성을 식별합니다.
        /// </summary>
        public static readonly DependencyProperty AlwaysInvokeCommandProperty = DependencyProperty.Register(
            nameof(AlwaysInvokeCommand),
            typeof(bool),
            typeof(EventToCommand),
            new PropertyMetadata(false));

        /// <summary>
        /// 이 트리거가 바인딩된 ICommand를 가져오거나 설정합니다. 이것은 종속성 속성입니다.
        /// </summary>
        public ICommand Command
        {
            get => (ICommand)this.GetValue(CommandProperty);
            set => this.SetValue(CommandProperty, value);
        }

        /// <summary>
        /// 이 트리거에 첨부된 <see cref="Command" />에 전달될 객체를 가져오거나 설정합니다. 이것은 종속성 속성입니다.
        /// </summary>
        public object? CommandParameter
        {
            get => this.GetValue(CommandParameterProperty);
            set => this.SetValue(CommandParameterProperty, value);
        }

        /// <summary>
        /// <see cref="Command" /> 속성의 CanExecuteChanged
        /// 이벤트가 발생할 때 첨부된 요소가 비활성화되어야 하는지 여부를 나타내는 값을 가져오거나 설정합니다.
        /// 이 속성이 true이고, 커맨드의 CanExecute 메서드가 false를 반환하면, 요소는 비활성화됩니다.
        /// 이 속성이 false인 경우, 커맨드의 CanExecute 메서드가 변경되어도 요소는 비활성화되지 않습니다.
        /// 이것은 DependencyProperty입니다.
        /// CanExecute method changes. This is a DependencyProperty.
        /// </summary>
        public bool MustToggleIsEnabled
        {
            get => (bool)this.GetValue(MustToggleIsEnabledProperty);

            set => this.SetValue(MustToggleIsEnabledProperty, value);
        }

        /// <summary>
        /// <see cref="PassEventArgsToCommand"/>를 사용할 때 EventArgs를 변환하는 데 사용되는 변환기에 대한 매개변수를 가져오거나 설정합니다.
        /// PassEventArgsToCommand가 false인 경우, 이 속성은 사용되지 않습니다. 이것은 종속성 속성입니다.

        /// </summary>
        public object EventArgsConverterParameter
        {
            get => this.GetValue(EventArgsConverterParameterProperty);
            set => this.SetValue(EventArgsConverterParameterProperty, value);
        }

        /// <summary>
        /// 첨부된 컨트롤이 비활성화되어 있어도 명령이 호출되어야 하는지 여부를 나타내는 값을 가져오거나 설정합니다.<br/>
        /// 이것은 종속성 속성입니다.
        /// </summary>
        public bool AlwaysInvokeCommand
        {
            get => (bool)this.GetValue(AlwaysInvokeCommandProperty);
            set => this.SetValue(AlwaysInvokeCommandProperty, value);
        }

        /// <summary>
        /// 이 액션을 트리거한 이벤트의 EventArgs가 바인딩된 RelayCommand에 전달되어야 하는지를 지정합니다. <br/>
        /// 이 값이 true인 경우, 명령은 해당 타입의 인수를 받아들여야 합니다(예: RelayCommand&lt;MouseButtonEventArgs&gt;).
        /// </summary>
        public bool PassEventArgsToCommand { get; set; }

        /// <summary>
        /// <see cref="PassEventArgsToCommand"/>를 사용할 때 EventArgs를 변환하는 데 사용되는 변환기를 가져오거나 설정합니다.<br/>
        /// PassEventArgsToCommand가 false인 경우, 이 속성은 사용되지 않습니다.
        /// </summary>
        public IEventArgsConverter? EventArgsConverter { get; set; }

        /// <summary>
        /// Called when this trigger is attached to a FrameworkElement.
        /// </summary>
        protected override void OnAttached()
        {
            base.OnAttached();
            this.EnableDisableElement();
        }

        public void Invoke()
        {
            this.Invoke(null);
        }

        protected override void Invoke(object? parameter)
        {
            if (this.AssociatedElementIsDisabled() && !this.AlwaysInvokeCommand)
            {
                return;
            }

            var command = this.Command;
            var commandParameter = this.CommandParameter;

            if (this.PassEventArgsToCommand)
            {
                commandParameter = this.EventArgsConverter?.Convert(parameter, this.EventArgsConverterParameter) ??
                                   parameter;
            }

            if (command.CanExecute(commandParameter))
            {
                command.Execute(commandParameter);
            }
        }
        private static void OnCommandChanged(
            EventToCommand? element,
            DependencyPropertyChangedEventArgs e)
        {
            if (element == null)
            {
                return;
            }

            if (e.OldValue != null)
            {
                ((ICommand)e.OldValue).CanExecuteChanged -= element.OnCommandCanExecuteChanged;
            }

            var command = (ICommand?)e.NewValue;

            if (command != null)
            {
                command.CanExecuteChanged += element.OnCommandCanExecuteChanged;
            }

            element.EnableDisableElement();
        }
        private bool AssociatedElementIsDisabled()
        {
            var element = this.AssociatedObject as FrameworkElement;

            return element is null or { IsEnabled: false };
        }

        private void EnableDisableElement()
        {
            var element = this.AssociatedObject as FrameworkElement;

            if (element == null)
            {
                return;
            }

            var command = this.Command;

            if (this.MustToggleIsEnabled)
            {
                element.IsEnabled = command.CanExecute(this.CommandParameter);
            }
        }

        private void OnCommandCanExecuteChanged(object? sender, EventArgs? e)
        {
            this.EnableDisableElement();
        }
    }
}
