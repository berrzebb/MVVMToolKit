using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NumericModule.Service
{
    public interface IDataController
    {
        ObservableCollection<string> CurrencyList { get; }
        ObservableCollection<string> UpdatedCurrencyDatas { get; }
        ObservableCollection<string> SubscribeList { get; }
        public string AddSubscribe(string symbol);
        public string RemoveSubscribe(string symbol);
    }
}
