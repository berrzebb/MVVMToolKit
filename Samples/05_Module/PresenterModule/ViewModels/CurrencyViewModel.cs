using System.Collections.ObjectModel;
using MVVMToolKit;
using MVVMToolKit.Attributes;
using NumericModule.Service;
using PresenterModule.Views;

namespace PresenterModule.ViewModels
{
    [Navigatable(RouteName = "Currency", ViewName = nameof(CurrencyView))]
    public class CurrencyViewModel : ViewModelBase<CurrencyViewModel>
    {
        public ReadOnlyObservableCollection<string> CurrencyList { get; }
        public CurrencyViewModel(IDataController dataController)
        {
            CurrencyList = new(dataController.CurrencyList);
        }
    }
}
