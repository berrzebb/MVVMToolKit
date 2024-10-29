using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NumericModule.Model
{
    public class Currency
    {
        public string Symbol { get; }
        public int BaseValue { get; }

        public Currency(string symbol, int baseValue)
        {
            this.Symbol = symbol;
            this.BaseValue = baseValue;
        }

        public override string ToString()
        {
            return $"화폐 종류 : {Symbol}, 표준 가격 : {BaseValue} ";
        }
    }
}
