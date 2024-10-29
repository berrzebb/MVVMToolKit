using CommunityToolkit.Mvvm.ComponentModel;
using MVVMToolKit;
using MVVMToolKit.Attributes;

namespace NavigatorWithViewModelSample.ViewModels
{
    [Navigatable(RouteName = "Specific", ViewName = "CustomView")]
    public partial class SpecificViewModel : ViewModelBase<SpecificViewModel>
    {
        [ObservableProperty]
        private string _text = "Specific";
        public SpecificViewModel()
        {
        }
    }
}
