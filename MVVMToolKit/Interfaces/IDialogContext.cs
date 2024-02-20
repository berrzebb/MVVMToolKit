namespace MVVMToolKit.Interfaces
{
    public interface IDialogContext
    {
        INotifyPropertyChanged ViewModel { get; internal set; }
        string? Title { get; internal set; }
        void Cleanup();
    }
}
