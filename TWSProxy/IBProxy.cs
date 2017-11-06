using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using IBApi;
using TWSProxy.messages;

namespace TWSProxy
{
    public partial class IBProxy
    {
        private Dictionary<int, Asset> dicAssets = new Dictionary<int, Asset>();

        protected IBClient ibClient;

        private EReaderMonitorSignal signal = new EReaderMonitorSignal();

        private int CONTRACT_ID = 1000000;

        private int ORDER_ID = 1000000;

        private bool isConnected = false;

        private bool isDebug = false;

        public Dictionary<int, Asset> DicAssets
        {
            get { return dicAssets; }
        }

        public bool IsConnected
        {
            get { return isConnected; }
            set { isConnected = value; }
        }

        public delegate void OnPriceHandler(OnPriceMessage msg);

        public event OnPriceHandler OnPriceEvent;

        public delegate void OnGreeksHandler(OnGreeksMessage msg);

        public event OnGreeksHandler OnGreeksEvent;

        public delegate void OnOrderStatusHandler(OnOrderStatusMessage msg);

        public event OnOrderStatusHandler OnOrderStatusEvent;

        public delegate void OnOpenOrderHandler(OnOpenOrderMessage msg);

        public event OnOpenOrderHandler OnOpenOrderEvent;

        public IBProxy(bool debug = false)
        {
            isDebug = debug;

            ibClient = new IBClient(signal);

            ibClient.Error += ibClient_Error;
            ibClient.ConnectionClosed += ibClient_ConnectionClosed;
            ibClient.CurrentTime += time => Console.WriteLine("Current Time: " + time + "\n");
            ibClient.TickPrice += ibClient_Tick;
            ibClient.TickSize += ibClient_Tick;
            ibClient.TickOptionCommunication += ibClient_Tick;
            //ibClient.TickString += (tickerId, tickType, value) => Console.WriteLine("Tick string. Ticker Id:" + tickerId + ", Type: " + TickType.getField(tickType) + ", Value: " + value + "\n");
            //ibClient.TickGeneric += (tickerId, field, value) => Console.WriteLine("Tick Generic. Ticker Id:" + tickerId + ", Field: " + TickType.getField(field) + ", Value: " + value + "\n");
            //ibClient.TickEFP += (tickerId, tickType, basisPoints, formattedBasisPoints, impliedFuture, holdDays, futureLastTradeDate, dividendImpact, dividendsToLastTradeDate) => Console.WriteLine("TickEFP. " + tickerId + ", Type: " + tickType + ", BasisPoints: " + basisPoints + ", FormattedBasisPoints: " + formattedBasisPoints + ", ImpliedFuture: " + impliedFuture + ", HoldDays: " + holdDays + ", FutureLastTradeDate: " + futureLastTradeDate + ", DividendImpact: " + dividendImpact + ", DividendsToLastTradeDate: " + dividendsToLastTradeDate + "\n");
            ibClient.TickSnapshotEnd += tickerId => Console.WriteLine("TickSnapshotEnd: " + tickerId + "\n");
            ibClient.NextValidId += ibClient_NextValidId;

            ibClient.ContractDetails += ibClient_HandleContractDataMessage;
            //ibClient.ContractDetailsEnd += reqId => UpdateUI(new ContractDetailsEndMessage());



            //ibClient.DeltaNeutralValidation += (reqId, underComp) =>
            //    Console.WriteLine("DeltaNeutralValidation. " + reqId + ", ConId: " + underComp.ConId + ", Delta: " + underComp.Delta + ", Price: " + underComp.Price + "\n");

            //ibClient.ManagedAccounts += UpdateUI;

            //ibClient.AccountSummary += accountManager.HandleAccountSummary;
            //ibClient.AccountSummaryEnd += UpdateUI;
            //ibClient.UpdateAccountValue += accountManager.HandleAccountValue;
            //ibClient.UpdatePortfolio += UpdateUI;

            //ibClient.UpdateAccountTime += message => accUpdatesLastUpdateValue.Text = message.Timestamp;
            ////ibClient.AccountDownloadEnd += (do nothing)
            ibClient.OrderStatus += ibClient_HandleOrderStatus;

            ibClient.OpenOrder += ibClient_HandleOpenOrder;
            ////ibClient.OpenOrderEnd += (do nothing)
            //ibClient.ExecDetails += orderManager.HandleExecutionMessage;
            //ibClient.ExecDetailsEnd += reqId => addTextToBox("ExecDetailsEnd. " + reqId + "\n");
            //ibClient.CommissionReport += commissionReport => orderManager.HandleCommissionMessage(new CommissionMessage(commissionReport));
            //ibClient.FundamentalData += UpdateUI;

            //ibClient.HistoricalData += historicalDataManager.UpdateUI;
            //ibClient.HistoricalDataUpdate += historicalDataManager.UpdateUI;
            //ibClient.HistoricalDataEnd += historicalDataManager.UpdateUI;

            //ibClient.MarketDataType += UpdateUI;
            //ibClient.UpdateMktDepth += deepBookManager.UpdateUI;
            //ibClient.UpdateMktDepthL2 += deepBookManager.UpdateUI;
            //ibClient.UpdateNewsBulletin += (msgId, msgType, message, origExchange) =>
            //    addTextToBox("News Bulletins. " + msgId + " - Type: " + msgType + ", Message: " + message + ", Exchange of Origin: " + origExchange + "\n");

            //ibClient.Position += accountManager.HandlePosition;
            //ibClient.PositionEnd += () => addTextToBox("PositionEnd \n");
            //ibClient.RealtimeBar += realTimeBarManager.UpdateUI;
            //ibClient.ScannerParameters += xml => scannerManager.UpdateUI(new ScannerParametersMessage(xml));
            //ibClient.ScannerData += scannerManager.UpdateUI;

            //ibClient.ScannerDataEnd += reqId => addTextToBox("ScannerDataEnd. " + reqId + "\r\n");
            //ibClient.ReceiveFA += advisorManager.UpdateUI;
            //ibClient.BondContractDetails += contractManager.HandleBondContractMessage;
            //ibClient.VerifyMessageAPI += apiData => addTextToBox("verifyMessageAPI: " + apiData);
            //ibClient.VerifyCompleted += (isSuccessful, errorText) => addTextToBox("verifyCompleted. IsSuccessfule: " + isSuccessful + " - Error: " + errorText);
            //ibClient.VerifyAndAuthMessageAPI += (apiData, xyzChallenge) => addTextToBox("verifyAndAuthMessageAPI: " + apiData + " " + xyzChallenge);
            //ibClient.VerifyAndAuthCompleted += (isSuccessful, errorText) => addTextToBox("verifyAndAuthCompleted. IsSuccessfule: " + isSuccessful + " - Error: " + errorText);
            //ibClient.DisplayGroupList += (reqId, groups) => addTextToBox("DisplayGroupList. Request: " + reqId + ", Groups" + groups);
            //ibClient.DisplayGroupUpdated += (reqId, contractInfo) => addTextToBox("displayGroupUpdated. Request: " + reqId + ", ContractInfo: " + contractInfo);

            //ibClient.PositionMulti += acctPosMultiManager.HandlePositionMulti;
            //ibClient.PositionMultiEnd += reqId => acctPosMultiManager.HandlePositionMultiEnd(new PositionMultiEndMessage(reqId));
            //ibClient.AccountUpdateMulti += acctPosMultiManager.HandleAccountUpdateMulti;
            //ibClient.AccountUpdateMultiEnd += reqId => acctPosMultiManager.HandleAccountUpdateMultiEnd(new AccountUpdateMultiEndMessage(reqId));
            //ibClient.SecurityDefinitionOptionParameter += optionsManager.UpdateUI;
            ////ibClient.SecurityDefinitionOptionParameterEnd += (do nothing)
            //ibClient.SoftDollarTiers += orderManager.HandleSoftDollarTiers;
            //ibClient.FamilyCodes += (familyCodes) => accountManager.HandleFamilyCodes(new FamilyCodesMessage(familyCodes));
            //ibClient.SymbolSamples += UpdateUI;
            //ibClient.MktDepthExchanges += (depthMktDataDescriptions) => deepBookManager.HandleMktDepthExchangesMessage(new MktDepthExchangesMessage(depthMktDataDescriptions));
            //ibClient.TickNews += newsManager.UpdateUI;
            //ibClient.TickReqParams += UpdateUI;
            //ibClient.SmartComponents += (reqId, theMap) => theMap.ToList().ForEach(i => dataGridViewSmartComponents.Rows.Add(new object[] { i.Key, i.Value.Key, i.Value.Value }));
            //ibClient.NewsProviders += (newsProviders) => newsManager.HandleNewsProviders(new NewsProvidersMessage(newsProviders));
            //ibClient.NewsArticle += newsManager.UpdateUI;
            //ibClient.HistoricalNews += newsManager.UpdateUI;
            //ibClient.HistoricalNewsEnd += newsManager.UpdateUI;
            //ibClient.HeadTimestamp += UpdateUI;
            //ibClient.HistogramData += UpdateUI;
            //ibClient.RerouteMktDataReq += (reqId, conId, exchange) => addTextToBox("Re-route market data request. ReqId: " + reqId + ", ConId: " + conId + ", Exchange: " + exchange + "\n");
            //ibClient.RerouteMktDepthReq += (reqId, conId, exchange) => addTextToBox("Re-route market depth request. ReqId: " + reqId + ", ConId: " + conId + ", Exchange: " + exchange + "\n");
            //ibClient.MarketRule += contractManager.HandleMarketRuleMessage;
            //ibClient.pnl += msg => pnldataTable.Rows.Add(msg.DailyPnL, msg.UnrealizedPnL, msg.RealizedPnL);
            //ibClient.pnlSingle += msg => pnlSingledataTable.Rows.Add(msg.Pos, msg.DailyPnL, msg.UnrealizedPnL, msg.RealizedPnL, msg.Value);
            //ibClient.historicalTick += UpdateUI;
            //ibClient.historicalTickBidAsk += UpdateUI;
            //ibClient.historicalTickLast += UpdateUI;

            ibClient.ClientSocket.reqIds(-1);
        }
        private void ibClient_Error(int id, int errorCode, string str, Exception ex)
        {
            if (ex != null)
            {
                Console.WriteLine("Error: " + ex);

                return;
            }

            if (id == 0 || errorCode == 0)
            {
                Console.WriteLine("Error: " + str + "\n");

                return;
            }
        }

        private void ibClient_ConnectionClosed()
        {
            IsConnected = false;
            Console.WriteLine("Disconnected...\n");
        }

        private void ibClient_Tick(TickPriceMessage msg)
        {
            if (isDebug)
            {
                Console.WriteLine("Tick Price. Ticker Id:{1}, Symbol: {2}, Type: {3}, Price: {4}, Pre-Open: {4}",
                    msg.RequestId, dicAssets[msg.RequestId].Con.Symbol, TickType.getField(msg.Field), msg.Price, msg.Attribs.PreOpen);
            }

            switch (msg.Field)
            {
                case TickType.ASK:
                case TickType.DELAYED_ASK:
                    dicAssets[msg.RequestId].Ask = msg.Price;
                    break;
                case TickType.BID:
                case TickType.DELAYED_BID:
                    dicAssets[msg.RequestId].Bid = msg.Price;
                    break;
                case TickType.LAST:
                case TickType.DELAYED_LAST:
                    dicAssets[msg.RequestId].Last = msg.Price;
                    break;
            }

            if (OnPriceEvent != null)
            {
                OnPriceMessage e = new OnPriceMessage();
                e.RequestId = msg.RequestId;
                e.Symbol = dicAssets[msg.RequestId].Con.Symbol;
                e.AssetID = dicAssets[msg.RequestId].ID;
                e.Type = TickType.getField(msg.Field);
                e.Price = msg.Price;
                e.PreOpen = msg.Attribs.PreOpen;

                OnPriceEvent(e);
            }
        }

        private void ibClient_Tick(TickSizeMessage msg)
        {
            if (isDebug)
            {
                Console.WriteLine("Tick Size. Ticker Id: {0}, Symbol: {1}, Type: {2}, Size: {3}",
                    msg.RequestId, dicAssets[msg.RequestId].Con.Symbol, TickType.getField(msg.Field), msg.Size);
            }

            switch (msg.Field)
            {
                case TickType.ASK_SIZE:
                    dicAssets[msg.RequestId].AskSize = msg.Size;
                    break;
                case TickType.BID_SIZE:
                    dicAssets[msg.RequestId].BidSize = msg.Size;
                    break;
            }
        }

        private void ibClient_Tick(TickOptionMessage msg)
        {
            if (isDebug)
            {
                Console.WriteLine("Tick Option. Ticker Id: {0}, Symbol: {1}, Type: {2}",
                    msg.RequestId, dicAssets[msg.RequestId].Con.Symbol, TickType.getField(msg.Field));
                Console.WriteLine("Delta: {0}, Gamma: {1}, Vega: {2}, Theta: {3}, IV: {4}, OptPrice: {5}, PvDividend: {6}, UndPrice: {7}",
                    msg.Delta, msg.Gamma, msg.Vega, msg.Theta, msg.ImpliedVolatility, msg.OptPrice, msg.PvDividend, msg.UndPrice);
            }

            dicAssets[msg.RequestId].ImpliedVolatility = msg.ImpliedVolatility;
            dicAssets[msg.RequestId].Delta = msg.Delta;
            dicAssets[msg.RequestId].Gamma = msg.Gamma;
            dicAssets[msg.RequestId].Vega = msg.Vega;
            dicAssets[msg.RequestId].Theta = msg.Theta;
            dicAssets[msg.RequestId].OptPrice = msg.OptPrice;
            dicAssets[msg.RequestId].PvDividend = msg.PvDividend;
            dicAssets[msg.RequestId].UndPrice = msg.UndPrice;

            if (OnGreeksEvent != null)
            {
                OnGreeksMessage e = new OnGreeksMessage();
                e.RequestId = msg.RequestId;
                e.Symbol = dicAssets[msg.RequestId].Con.Symbol;
                e.AssetID = dicAssets[msg.RequestId].ID;
                e.Type = TickType.getField(msg.Field);
                e.Delta = msg.Delta;
                e.Gamma = msg.Gamma;
                e.Vega = msg.Vega;
                e.Theta = msg.Theta;
                e.ImpliedVolatility = msg.ImpliedVolatility;
                e.OptPrice = msg.OptPrice;
                e.PvDividend = msg.PvDividend;
                e.UndPrice = msg.UndPrice;

                OnGreeksEvent(e);
            }
        }

        private void ibClient_NextValidId(ConnectionStatusMessage statusMessage)
        {
            IsConnected = statusMessage.IsConnected;

            if (statusMessage.IsConnected)
            {
                Console.WriteLine("Connected! Your client Id: " + ibClient.ClientId);
                ORDER_ID = ibClient.NextOrderId;
                Console.WriteLine("Next Order ID: {0}", ORDER_ID);
            }
            else
            {
                Console.WriteLine("Disconnected...");
            }
        }

        protected string generateAssetIDFromContract(Contract con)
        {
            if (con.SecType == "STK")
            {
                return string.Format("STK.{0}", con.Symbol);
            }

            if (con.SecType == "IND")
            {
                return string.Format("IND.{0}", con.Symbol);
            }

            if (con.SecType == "CMDTY")
            {
                return string.Format("CMDTY.{0}", con.Symbol);
            }

            if (con.SecType == "FUT")
            {
                return string.Format("FUT.{0}.{1}", con.Symbol, con.LastTradeDateOrContractMonth);
            }

            if (con.SecType == "OPT")
            {
                return string.Format("OPT.{0}.{1}.{2}.{3}", con.Symbol, con.Right.Substring(0, 1), con.LastTradeDateOrContractMonth.Substring(0, 8), con.Strike);
            }

            if (con.SecType == "FOP")
            {
                return string.Format("OPT.{0}.{1}.{2}.{3}", con.Symbol, con.Right.Substring(0, 1), con.LastTradeDateOrContractMonth.Substring(0, 8), con.Strike);
            }

            return "NULL";
        }

        private void ibClient_HandleContractDataMessage(ContractDetailsMessage message)
        {
            Contract con = message.ContractDetails.Summary;

            dicAssets[message.RequestId].Con = con;
            dicAssets[message.RequestId].ID = generateAssetIDFromContract(con);

            if (isDebug)
            {
                Console.WriteLine("Contract ID = " + dicAssets[message.RequestId].ID);
            }
            ibClient.ClientSocket.reqMktData(message.RequestId, con, null, false, false, new List<TagValue>());
        }

        private void ibClient_HandleOrderStatus(OrderStatusMessage message)
        {
            if (OnOrderStatusEvent != null)
            {
                OnOrderStatusMessage msg = new OnOrderStatusMessage(message);

                OnOrderStatusEvent(msg);
            }
        }

        private void ibClient_HandleOpenOrder(OpenOrderMessage message)
        {
            if (OnOpenOrderEvent != null)
            {
                OnOpenOrderMessage msg = new OnOpenOrderMessage(message);

                OnOpenOrderEvent(msg);
            }
        }

        public void connect(string host, int port, int ClientID)
        {
            ibClient.ClientId = ClientID;
            ibClient.ClientSocket.eConnect(host, port, ClientID);

            var reader = new EReader(ibClient.ClientSocket, signal);

            reader.Start();

            new Thread(() =>
            {
                while (ibClient.ClientSocket.IsConnected())
                {
                    signal.waitForSignal();
                    reader.processMsgs();
                }
            })
            { IsBackground = true }.Start();
        }

        public int add(string symbol, string secType, string exchange, string currency, string multiplier, string right, string expireDate, double strike)
        {
            int conId = CONTRACT_ID++;

            Contract con = new Contract();
            con.Symbol = symbol;
            con.SecType = secType;
            con.Exchange = exchange;
            con.Currency = currency;
            con.Multiplier = multiplier;
            con.Right = right;
            con.Strike = strike;
            con.LastTradeDateOrContractMonth = expireDate;

            Asset a = new Asset();
            dicAssets[conId] = a;

            ibClient.ClientSocket.reqContractDetails(conId, con);

            return conId;
        }

        public int add(string symbol, string secType, string exchange, string currency, string multiplier)
        {
            int conId = CONTRACT_ID++;

            Contract con = new Contract();
            con.Symbol = symbol;
            con.SecType = secType;
            con.Exchange = exchange;
            con.Currency = currency;
            con.Multiplier = multiplier;

            Asset a = new Asset();
            dicAssets[conId] = a;

            ibClient.ClientSocket.reqContractDetails(conId, con);

            return conId;
        }

        protected Contract generateContractFromAssetID(string assetID)
        {
            Contract con = new Contract();
            var symbol = assetID;

            if (!symbol.Contains("."))
            {
                if (symbol.Length < 6)
                {
                    con.SecType = "STK";
                    con.Exchange = "SMART";
                }
                else if (symbol.Length == 6)
                {
                    con.SecType = "CASH";
                    con.Exchange = "IDEALPRO";
                }

                //VIX指数作为特例
                if (symbol == "VIX")
                {
                    con.SecIdType = "IND";
                    con.Exchange = "CBOE";
                }
                con.Symbol = symbol;
            }
            else
            {
                //STK.AAPL
                if (symbol.StartsWith("STK"))
                {
                    string[] assetProperty = symbol.Split('.');
                    con.SecType = "STK";
                    con.Exchange = "SMART";
                    con.Symbol = assetProperty[1];
                }
                //OPT.SPY.C.20171006.244
                if (symbol.StartsWith("OPT"))
                {
                    string[] assetProperty = symbol.Split(new char[] { '.' }, 5);
                    con.SecType = "OPT";
                    con.Symbol = assetProperty[1];
                    con.Right = assetProperty[2];
                    con.LastTradeDateOrContractMonth = assetProperty[3];
                    con.Strike = double.Parse(assetProperty[4]);

                }
                //FUT.ES.20171215
                if (symbol.StartsWith("FUT"))
                {
                    string[] assetProperty = symbol.Split(new char[] { '.' }, 3);
                    con.SecType = "FUT";
                    con.Symbol = assetProperty[1];
                    con.LastTradeDateOrContractMonth = assetProperty[2];

                }
                //FOP.ES.C.20171117.2550
                if (symbol.StartsWith("FOP"))
                {
                    string[] assetProperty = symbol.Split(new char[] { '.' }, 5);
                    con.SecType = "FOP";
                    con.Symbol = assetProperty[1];
                    con.Right = assetProperty[2];
                    con.LastTradeDateOrContractMonth = assetProperty[3];
                    con.Strike = double.Parse(assetProperty[4]);
                }
                //IND.VIX
                if (symbol.StartsWith("IND"))
                {
                    string[] assetProperty = symbol.Split('.');
                    con.SecType = "IND";
                    con.Exchange = "SMART";
                    con.Symbol = assetProperty[1];
                }
                //CMDTY.XAUUSD
                if (symbol.StartsWith("CMDTY"))
                {
                    string[] assetProperty = symbol.Split('.');
                    con.SecType = "CMDTY";
                    con.Exchange = "SMART";
                    con.Symbol = assetProperty[1];
                }

                //VIX.20171220
                string[] asset_logogram = symbol.Split('.');
                if (asset_logogram.Length == 2)
                {
                    con.Symbol = asset_logogram[0];
                    con.SecType = "FUT";
                    con.LastTradeDateOrContractMonth = asset_logogram[1];
                    con.Exchange = "SMART";
                }
            }
            return con;
        }

        public int add(string symbol)
        {
            int conId = CONTRACT_ID++;

            Contract con = generateContractFromAssetID(symbol);

            Asset a = new Asset();
            dicAssets[conId] = a;

            ibClient.ClientSocket.reqContractDetails(conId, con);
            return conId;
        }

        public void placeOrderMKT(string assetID, string action, double quantity)
        {
            int order_id = ORDER_ID++;

            Contract con = generateContractFromAssetID(assetID);

            Order order = new Order();
            order.Action = action;
            order.OrderType = "MKT";
            order.TotalQuantity = quantity;

            ibClient.ClientSocket.placeOrder(order_id, con, order);
        }

        public void placeOrderLMT(string assetID, string action, double quantity, double limitPrice)
        {
            int order_id = ORDER_ID++;

            Contract con = generateContractFromAssetID(assetID);

            Order order = new Order();
            order.Action = action;
            order.OrderType = "LMT";
            order.TotalQuantity = quantity;
            order.LmtPrice = limitPrice;

            ibClient.ClientSocket.placeOrder(order_id, con, order);
        }
    }

    public struct OnPriceMessage
    {
        public int RequestId;
        public string Symbol;
        public string AssetID;
        public string Type;
        public double Price;
        public bool PreOpen;
    }

    public struct OnGreeksMessage
    {
        public int RequestId;
        public string Symbol;
        public string AssetID;
        public string Type;
        public double Delta;
        public double Gamma;
        public double Vega;
        public double Theta;
        public double ImpliedVolatility;
        public double OptPrice;
        public double PvDividend;
        public double UndPrice;
    }

    public class OnOrderStatusMessage : OrderStatusMessage
    {
        public OnOrderStatusMessage(int orderId, string status, double filled, double remaining, double avgFillPrice,
           int permId, int parentId, double lastFillPrice, int clientId, string whyHeld, double mktCapPrice) : 
            base(orderId, status, filled, remaining, avgFillPrice, permId, parentId, lastFillPrice, clientId, whyHeld, mktCapPrice)
        { }

        public OnOrderStatusMessage(OrderStatusMessage msg):
            base(msg.OrderId, msg.Status, msg.Filled, msg.Remaining, msg.AvgFillPrice, msg.PermId, msg.ParentId, msg.LastFillPrice, msg.ClientId, msg.WhyHeld, msg.MktCapPrice)
        { }
    }

    public class OnOpenOrderMessage: OpenOrderMessage
    {
        public OnOpenOrderMessage(int orderId, Contract contract, Order order, OrderState orderState) :
            base(orderId, contract, order, orderState)
        { }

        public OnOpenOrderMessage(OpenOrderMessage msg):
            base(msg.OrderId, msg.Contract, msg.Order, msg.OrderState)
        { }
    }
}
