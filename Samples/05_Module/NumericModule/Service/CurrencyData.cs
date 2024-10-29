using NumericModule.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NumericModule.Service
{
    public class CurrencyData : ICurrencyData
    {
        public Currency[] Currencys { get; } = new[] 
        { 
            new Currency("USD",1200),
            new Currency("AUD",850),
            new Currency("GBP",1700),
            new Currency("CNY",180),
            new Currency("EUR",1400),
            new Currency("JPY",1000)
        }; 
    }
}
