using System.Reactive.Linq;
using NumericModule.Model;
namespace NumericModule.Service
{
    public class NumericDataService : INumericDataService
    {
        private readonly Dictionary<string, IObservable<Exchange>> _data = new();
        public NumericDataService(ICurrencyData currencyData)
        {
            foreach (var cur in currencyData.Currencys)
            {
                _data[cur.Symbol] = GenerateExchangeStream(cur);
            }
        }

        public IObservable<Exchange> Watch(string symbol)
        {
            if (symbol is null) throw new ArgumentNullException(nameof(symbol));
            if (!_data.ContainsKey(symbol)) throw new Exception(nameof(symbol) + " 은 없는 데이터입니다");
            return _data[symbol];
        }

        private IObservable<Exchange> GenerateExchangeStream(Currency currency)
        {
            return Observable.Create<Exchange>(observer =>
            {
                var exc = new Exchange(currency.Symbol, currency.BaseValue);
                observer.OnNext(exc);

                var random = new Random(currency.GetHashCode());

                return Observable.Interval(TimeSpan.FromSeconds(1))
                .Select(_ => random.Next(-1, +2))
                .Subscribe(i =>
                {
                    exc += i;
                    observer.OnNext(exc);
                });
            });
        }
    }
}
