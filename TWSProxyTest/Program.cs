using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TWSProxy;

namespace TWSProxyTest
{
    class Program
    {
        static void OnPrice(OnPriceMessage msg)
        {
            Console.WriteLine("Symbol: {0}, Type: {1}, Price: {2}, PreOpen: {3}", msg.Symbol, msg.Type, msg.Price, msg.PreOpen);
        }

        static void OnGreeks(OnGreeksMessage msg)
        {
            Console.WriteLine("Symbol: {0}, Type: {1}, Delta: {2}, Gamma: {3}, Vega: {4}, Theta: {5}, IV: {6}, OptPrice: {7}, PvDividend: {8}, UndPrice: {9}", 
                msg.Symbol, msg.Type, msg.Delta, msg.Gamma, msg.Vega, msg.Theta, msg.ImpliedVolatility, msg.OptPrice, msg.PvDividend, msg.UndPrice);
        }

        static void Main(string[] args)
        {
            IBProxy proxy = new IBProxy(true);

            proxy.connect("127.0.0.1", 7496, 10);

            /*
            proxy.OnPriceEvent += OnPrice;
            proxy.OnGreeksEvent += OnGreeks;

            var spy = proxy.add("SPY");
            var vxx = proxy.add("STK.VXX");
            var vxx_call = proxy.add("OPT.VXX.C.20171117.35.5");
            var es = proxy.add("FUT.ES.20171215");
            var es_call = proxy.add("FOP.ES.C.20171215.2550");

            */
            proxy.OnOpenOrderEvent += (msg) => {
                Console.WriteLine("Open Order, OrderID: {0}", msg.OrderId);
            };

            proxy.OnOrderStatusEvent += (msg) =>
            {
                Console.WriteLine("Order Status: {0}, {1}, {2}", msg.OrderId, msg.Status, msg.WhyHeld);
            };

            Console.ReadKey();

            proxy.placeOrderLMT("STK.BABA", "BUY", 200, 1.5);
            Console.WriteLine("Order Placed.");

            
            Console.ReadKey();
        }
    }
}
