using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IBApi;

namespace TWSProxy
{
    public class Asset
    {
        public string ID { get; set; }
        public string Symbol { get; set; }
        public double Ask { get; set; }
        public double Bid { get; set; }
        public double Last { get; set; }
        public int AskSize { get; set; }
        public int BidSize { get; set; }

        public double ImpliedVolatility { get; set; }
        public double Delta { get; set; }
        public double Gamma { get; set; }
        public double Vega { get; set; }
        public double Theta { get; set; }
        public double OptPrice { get; set; }
        public double PvDividend { get; set; }
        public double UndPrice { get; set; }

        public Contract Con { get; set; }
        
        public Asset()
        {
            //Con = new Contract();
        }
        
    }
}
