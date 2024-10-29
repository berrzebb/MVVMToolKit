using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NumericModule.Model
{
    public class Exchange
    {
        public string Symbol { get; }
        public int Price { get; }

        public Exchange(string symbol, int price)
        {
            Symbol = symbol;
            Price = price;
        }

        public override string ToString()
        {
            return $"화폐 종류 : {Symbol}, 현재 가격 {Price}";
        }

        public static Exchange operator +(Exchange val, int dis)
        {
            var adjustPrice = val.Price + dis;
            return new Exchange(val.Symbol, adjustPrice);
        }

        public static Exchange operator -(Exchange val, int dis)
        {
            var adjustPrice = val.Price - dis;
            return new Exchange(val.Symbol, adjustPrice);
        }
    }
}
