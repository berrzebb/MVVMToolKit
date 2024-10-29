using System.Collections.ObjectModel;
using MVVMToolKit;
using MVVMToolKit.Attributes;
using NumericModule.Service;
using PresenterModule.Views;

namespace PresenterModule.ViewModels
{
    [Navigatable(RouteName = "Data", ViewName = nameof(DataView))]
    public class DataViewModel : ViewModelBase<DataViewModel>
    {
        public ReadOnlyObservableCollection<string> SubscribedList { get; }

        public DataViewModel(IDataController dataController)
        {
            this.SubscribedList = new(dataController.UpdatedCurrencyDatas);
        }
    }
}
