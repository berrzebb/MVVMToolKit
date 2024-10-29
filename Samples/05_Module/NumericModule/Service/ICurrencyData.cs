using NumericModule.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NumericModule.Service
{
    public interface ICurrencyData
    {
        Currency[] Currencys { get; }
    }
}
