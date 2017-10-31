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

        private bool isConnected = false;

        private bool isDebug = false;

        public Dictionary<int, Asset>  DicAssets
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

            ibClient.ContractDetails += HandleContractDataMessage;
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
            //ibClient.OrderStatus += orderManager.HandleOrderStatus;

            //ibClient.OpenOrder += orderManager.handleOpenOrder;
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
                Console.WriteLine("Tick Price. Ticker Id:" + msg.RequestId + ", Symbol: " + dicAssets[msg.RequestId].Con.Symbol + ", Type: " + TickType.getField(msg.Field) + ", Price: " + msg.Price + ", Pre-Open: " + msg.Attribs.PreOpen);
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
                Console.WriteLine("Tick Size. Ticker Id:" + msg.RequestId + ", Symbol: " + dicAssets[msg.RequestId].Con.Symbol + ", Type: " + TickType.getField(msg.Field) + ", Size: " + msg.Size);
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
                Console.WriteLine("Tick Size. Ticker Id:" + msg.RequestId + ", Symbol: " + dicAssets[msg.RequestId].Con.Symbol + ", Type: " + TickType.getField(msg.Field));
                Console.WriteLine("Delta: " + msg.Delta + ", Gamma: " + msg.Gamma + ", Vega: " + msg.Vega + ", Theta: " + ", IV: " + msg.ImpliedVolatility + ", OptPrice:" + msg.OptPrice + ", PvDividend: " + msg.PvDividend + ", UndPrice: " + msg.UndPrice);
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
            }
            else
            {
                Console.WriteLine("Disconnected...");
            }
        }

        private void HandleContractDataMessage(ContractDetailsMessage message)
        {
            Contract con = message.ContractDetails.Summary;

            dicAssets[message.RequestId].Con = con;

            ibClient.ClientSocket.reqMktData(message.RequestId, con, null, false, false, new List<TagValue>());
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

        public int add(string symbol)
        {
            int conId = CONTRACT_ID++;

            Contract con = new Contract();

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
                    con.SecType = "STK";
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
            }

            Asset a = new Asset();
            dicAssets[conId] = a;

            ibClient.ClientSocket.reqContractDetails(conId, con);
            return conId;
        }
    }

    public struct OnPriceMessage
    {
        public int      RequestId;
        public string   Symbol;
        public string   Type;
        public double   Price;
        public bool     PreOpen;
    }

    public struct OnGreeksMessage
    {
        public int RequestId;
        public string Symbol;
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
}
