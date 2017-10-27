using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IBApi;

namespace TWSProxy
{
    public enum SecType
    {
        STK,
        OPT,
        FUT,
        FOP,
        CMDTY,
        CASH
    }
    public class Asset
    {
        public int conId { get; set; }
        public string ID { get; set; }
        public string Symbol { get; set; }
        public double Ask { get; set; }
        public double Bid { get; set; }

        public int AskSize { get; set; }
        public int BidSize { get; set; }
        public string Currency { get; set; }
        public string Exchange { get; set; }
    }
}
