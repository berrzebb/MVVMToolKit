using CommunityToolkit.Mvvm.Messaging.Messages;

namespace MVVMToolKit.Models.Messages
{
    public class NavigationMessage : ValueChangedMessage<string>
    {
        public NavigationMessage(string value) : base(value)
        {

        }
    }
}
