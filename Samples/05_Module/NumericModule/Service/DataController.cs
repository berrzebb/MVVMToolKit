using System.Collections.ObjectModel;
using MVVMToolKit.Interfaces;
namespace NumericModule.Service
{
    public class DataController : IDataController
    {
        private readonly ICurrencyData _curencyData;
        private readonly INumericDataService _numericDataService;
        private readonly IDispatcherService _dispatcherService;
        public ObservableCollection<string> CurrencyList { get; }

        public ObservableCollection<string> UpdatedCurrencyDatas { get; } = new();
        public ObservableCollection<string> SubscribeList { get; } = new();
        private Dictionary<string, IDisposable> _subscribedExchangeDict = new();

        public DataController(ICurrencyData currencyData, INumericDataService numericDataService, IDispatcherService dispatcher)
        {
            this._curencyData = currencyData;
            this._numericDataService = numericDataService;
            this._dispatcherService = dispatcher;
            CurrencyList = new(_curencyData.Currencys.Select(currency => currency.Symbol).ToList());
        }

        public string AddSubscribe(string symbol)
        {
            if (SubscribeList.Contains(symbol))
            {
                return "이미 추가 된 항목입니다.";
            }
            var stream = _numericDataService.Watch(symbol).Subscribe(exc =>
            {
                _dispatcherService.Invoke(() =>
                {
                    UpdatedCurrencyDatas.Add(exc.ToString());
                });
            });
            _subscribedExchangeDict.Add(symbol, stream);
            SubscribeList.Add(symbol);
            return "추가 되었습니다.";
        }

        public string RemoveSubscribe(string symbol)
        {
            if (!SubscribeList.Contains(symbol))
            {
                return "추가 된 항목이 아닙니다.";
            }
            _subscribedExchangeDict[symbol].Dispose();
            _subscribedExchangeDict.Remove(symbol);
            SubscribeList.Remove(symbol);
            return "삭제 되었습니다";
        }
    }
}
