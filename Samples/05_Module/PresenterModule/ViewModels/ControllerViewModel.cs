using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MVVMToolKit;
using MVVMToolKit.Attributes;
using NumericModule.Service;
using PresenterModule.Views;

namespace PresenterModule.ViewModels
{
    [Navigatable(RouteName = "Controller", ViewName = nameof(ControllerView))]
    public partial class ControllerViewModel : ViewModelBase<ControllerViewModel>
    {
        private readonly IDataController _dataController;

        [ObservableProperty]
        private string _addInput = string.Empty;
        [ObservableProperty]
        private string _removeInput = string.Empty;
        [ObservableProperty]
        private string _taskLog = string.Empty;
        public ControllerViewModel(IDataController dataController)
        {
            _dataController = dataController;
        }

        [RelayCommand]
        private void Add()
        {
            TaskLog = _dataController.AddSubscribe(AddInput);
        }
        [RelayCommand]
        private void Remove()
        {
            TaskLog = _dataController.RemoveSubscribe(RemoveInput);
        }
    }
}
