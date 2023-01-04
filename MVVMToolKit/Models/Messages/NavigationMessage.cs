using CommunityToolkit.Mvvm.Messaging.Messages;

namespace MVVMToolKit.Models.Messages
{
    /// <summary>
    /// The navigation message class
    /// </summary>
    /// <seealso cref="ValueChangedMessage{string}"/>
    public class NavigationMessage : ValueChangedMessage<string>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="NavigationMessage"/> class
        /// </summary>
        /// <param name="value">The value</param>
        public NavigationMessage(string value) : base(value)
        {

        }
    }
}
