namespace MVVMToolKit.Navigation.Mapping
{
    public interface IViewSelector : INotifyPropertyChanged
    {
        string GetView();
    }
}
