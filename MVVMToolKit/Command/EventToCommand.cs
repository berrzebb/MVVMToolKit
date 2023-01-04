using System.Windows.Input;
using Microsoft.Xaml.Behaviors;

namespace MVVMToolKit.Command
{
    /// <summary>
    /// This <see cref="T:System.Windows.Interactivity.TriggerAction`1" /> can be
    /// used to bind any event on any FrameworkElement to an <see cref="ICommand" />.
    /// Typically, this element is used in XAML to connect the attached element
    /// to a command located in a ViewModel. This trigger can only be attached
    /// to a FrameworkElement or a class deriving from FrameworkElement.
    /// <para>To access the EventArgs of the fired event, use a RelayCommand&lt;EventArgs&gt;
    /// and leave the CommandParameter and CommandParameterValue empty!</para>
    /// </summary>
    public class EventToCommand : TriggerAction<DependencyObject>
    {
        /// <summary>
        /// Identifies the <see cref="Command" /> dependency property
        /// </summary>
        public static readonly DependencyProperty CommandProperty = DependencyProperty.Register(
            nameof(ICommand),
            typeof(ICommand),
            typeof(EventToCommand),
            new PropertyMetadata(
                null,
                (s, e) => OnCommandChanged(s as EventToCommand, e)));
        /// <summary>
        /// Identifies the <see cref="CommandParameter" /> dependency property
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
        /// Identifies the <see cref="MustToggleIsEnabled" /> dependency property
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
        /// Identifies the <see cref="EventArgsConverterParameter" /> dependency property.
        /// </summary>
        public static readonly DependencyProperty EventArgsConverterParameterProperty = DependencyProperty.Register(
            nameof(EventArgsConverterParameter),
            typeof(object),
            typeof(EventToCommand),
            new PropertyMetadata(null));
        
        /// <summary>
        /// Identifies the <see cref="AlwaysInvokeCommand" /> dependency property.
        /// </summary>
        public static readonly DependencyProperty AlwaysInvokeCommandProperty = DependencyProperty.Register(
            nameof(AlwaysInvokeCommand),
            typeof(bool),
            typeof(EventToCommand),
            new PropertyMetadata(false));
        
        /// <summary>
        /// Gets or sets the ICommand that this trigger is bound to. This
        /// is a DependencyProperty.
        /// </summary>
        public ICommand Command
        {
            get => (ICommand)this.GetValue(CommandProperty);
            set => this.SetValue(CommandProperty, value);
        }
        
        /// <summary>
        /// Gets or sets an object that will be passed to the <see cref="Command" />
        /// attached to this trigger. This is a DependencyProperty.
        /// </summary>
        public object? CommandParameter
        {
            get => this.GetValue(CommandParameterProperty);
            set => this.SetValue(CommandParameterProperty, value);
        }
        
        /// <summary>
        /// Gets or sets a value indicating whether the attached element must be
        /// disabled when the <see cref="Command" /> property's CanExecuteChanged
        /// event fires. If this property is true, and the command's CanExecute 
        /// method returns false, the element will be disabled. If this property
        /// is false, the element will not be disabled when the command's
        /// CanExecute method changes. This is a DependencyProperty.
        /// </summary>
        public bool MustToggleIsEnabled
        {
            get => (bool)this.GetValue(MustToggleIsEnabledProperty);

            set => this.SetValue(MustToggleIsEnabledProperty, value);
        }
        
        /// <summary>
        /// Gets or sets a parameters for the converter used to convert the EventArgs when using
        /// <see cref="PassEventArgsToCommand"/>. If PassEventArgsToCommand is false,
        /// this property is never used. This is a dependency property.
        /// </summary>
        public object EventArgsConverterParameter
        {
            get => this.GetValue(EventArgsConverterParameterProperty);
            set => this.SetValue(EventArgsConverterParameterProperty, value);
        }
        
        /// <summary>
        /// Gets or sets a value indicating if the command should be invoked even
        /// if the attached control is disabled. This is a dependency property.
        /// </summary>
        public bool AlwaysInvokeCommand
        {
            get => (bool)this.GetValue(AlwaysInvokeCommandProperty);
            set => this.SetValue(AlwaysInvokeCommandProperty, value);
        }
        
        /// <summary>
        /// Specifies whether the EventArgs of the event that triggered this
        /// action should be passed to the bound RelayCommand. If this is true,
        /// the command should accept arguments of the corresponding
        /// type (for example RelayCommand&lt;MouseButtonEventArgs&gt;).
        /// </summary>
        public bool PassEventArgsToCommand { get; set; }
        
        /// <summary>
        /// Gets or sets a converter used to convert the EventArgs when using
        /// <see cref="PassEventArgsToCommand"/>. If PassEventArgsToCommand is false,
        /// this property is never used.
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
                ((ICommand) e.OldValue).CanExecuteChanged -= element.OnCommandCanExecuteChanged;
            }

            var command = (ICommand?) e.NewValue;

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
