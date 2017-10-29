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
        static void Main(string[] args)
        {
            IBProxy proxy = new IBProxy(true);

            proxy.connect("127.0.0.1", 7496, 1);

            var spy = proxy.add("SPY");
            var vxx = proxy.add("STK.VXX");
            var vxx_call = proxy.add("OPT.VXX.C.20171117.35.5");
            var es = proxy.add("FUT.ES.20171215");
            var es_call = proxy.add("FOP.ES.C.20171215.2550");
            

            Console.ReadKey();
        }
    }
}
