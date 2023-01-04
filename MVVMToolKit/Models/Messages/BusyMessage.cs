using CommunityToolkit.Mvvm.Messaging.Messages;

namespace MVVMToolKit.Models.Messages
{
    /// <summary>
    /// The busy message class
    /// </summary>
    /// <seealso cref="ValueChangedMessage{bool}"/>
    public class BusyMessage : ValueChangedMessage<bool>
    {
        /// <summary>
        /// Gets or sets the value of the busy id
        /// </summary>
        public string? BusyId { get; set; }
        /// <summary>
        /// Gets or sets the value of the busy text
        /// </summary>
        public string? BusyText { get; set; }
        
        /// <summary>
        /// Initializes a new instance of the <see cref="BusyMessage"/> class
        /// </summary>
        /// <param name="value">The value</param>
        public BusyMessage(bool value) : base(value)
        {

        }
    }
}
