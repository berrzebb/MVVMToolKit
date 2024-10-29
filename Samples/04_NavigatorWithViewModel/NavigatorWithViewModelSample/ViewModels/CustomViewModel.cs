using CommunityToolkit.Mvvm.ComponentModel;
using MVVMToolKit;
using MVVMToolKit.Attributes;

namespace NavigatorWithViewModelSample.ViewModels
{
    [Navigatable(RouteName = "Custom", ViewName = "CustomView")]
    public partial class CustomViewModel : ViewModelBase<CustomViewModel>
    {
        [ObservableProperty]
        private string _text = "Custom";
        public CustomViewModel()
        {
        }
    }
}
