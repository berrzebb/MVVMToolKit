using CommunityToolkit.Mvvm.Messaging.Messages;

namespace MVVMToolKit.Models.Messages
{
    public class BusyMessage : ValueChangedMessage<bool>
    {
        public string? BusyId { get; set; }
        public string? BusyText { get; set; }
        
        public BusyMessage(bool value) : base(value)
        {

        }
    }
}
