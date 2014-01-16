using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using QStrategyGUILib.QStrategySVR;
using EZXWPFLibrary.Helpers;
using EZXWPFLibrary.Utils;

namespace QStrategyGUILib
{
    public partial class CommunicationManager : ICommunicationManager
    {       
        private StrategyWebServiceClient service;
        public StrategyWebServiceClient Service
        {
            get { return service; }
            set { service = value; }
        }

        private StrategyEngine strategyEngine;
        public StrategyEngine StrategyEngine
        {
            get { return strategyEngine; }
            set
            {
                strategyEngine = value;
            }
        }


        public CommunicationManager()
        {
            string logMessage = string.Format("Class: {0}, Method: {1}", "CommunicationManager", "CommunicationManager()");
            LogUtil.WriteLog(LogLevel.INFO, logMessage);
        }

        public CommunicationManager(StrategyEngine strategyEngine)
        {
            string logMessage = string.Format("Class: {0}, Method: {1}", "CommunicationManager", "CommunicationManager(StrategyEngine strategyEngine)");
            LogUtil.WriteLog(LogLevel.INFO, logMessage);

            this.strategyEngine = strategyEngine;
        }

        public void Init()
        {
            string logMessage = string.Format("Class: {0}, Method: {1}", "CommunicationManager", "Init()");
            LogUtil.WriteLog(LogLevel.INFO, logMessage);

            Service = CreateWebServiceClient();
        }

        private StrategyWebServiceClient CreateWebServiceClient()
        {
            string logMessage = string.Format("Class: {0}, Method: {1}", "CommunicationManager", "CreateWebServiceClient()");
            LogUtil.WriteLog(LogLevel.INFO, logMessage);

            StrategyWebServiceClient client = new StrategyWebServiceClient();

            //QStrategyGUILib.MockupService.MockupMainServiceClient client = new MockupService.MockupMainServiceClient();
            //return client;
            int timeoutSec = 30;

            if ((ConfigurationManager.AppSettings.AllKeys.Contains("QSTRATEGY_SERVICE_URL")))
            {
                //client.Endpoint.Address = new System.ServiceModel.EndpointAddress(ConfigurationManager.AppSettings["QSTRATEGY_SERVICE_URL"]);

                string baseUri = ConfigurationManager.AppSettings["QSTRATEGY_SERVICE_URL"];
                string serviceName = "qs";

                Uri newUri = new Uri(new Uri(baseUri), serviceName);
                client.Endpoint.ListenUri = newUri;

                client = new StrategyWebServiceClient("QStrategyWebServiceImplPort", newUri.AbsoluteUri);

                this.StrategyEngine.WebServiceURL = baseUri.Replace("qs","");
            }

            if (ConfigurationManager.AppSettings.AllKeys.Contains("QSTRATEGY_SERVICE_TIMEOUT"))
            {
                int.TryParse(ConfigurationManager.AppSettings["QSTRATEGY_SERVICE_TIMEOUT"], out timeoutSec);
            }

            logMessage = string.Format("Class: {0}, Method: {1}, Setting Timeout: {2} sec", "CommunicationManager", "CreateWebServiceClient()", timeoutSec);
            LogUtil.WriteLog(LogLevel.INFO, logMessage);

            client.Endpoint.Binding.OpenTimeout = new TimeSpan(0, 0, timeoutSec); // 30 x 1000(ms) = 30 sec
            //client.Endpoint.Binding.ReceiveTimeout = new TimeSpan(0, 0, timeoutSec); // 30 x 1000(ms) = 30 sec
            //client.Endpoint.Binding.CloseTimeout = new TimeSpan(0, 0, timeoutSec); // 30 x 1000(ms) = 30 sec
            //client.Endpoint.Binding.SendTimeout = new TimeSpan(0, 0, timeoutSec); // 30 x 1000(ms) = 30 sec
            return client;
        }

        //Check connection from server
        public bool CheckConnection()
        {
            try
            {
                Service.echo(string.Empty);
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<StrategyOrderInfo> GetStrategyUpdate()
        {
            string logMessage = string.Format("Class: {0}, Method: {1}", "CommunicationManager", "GetStrategyUpdate()");
            LogUtil.WriteLog(LogLevel.DEBUG, logMessage);

            strategyResultsUpdate update = Service.getUpdates();
            symbolUpdate[] StrategyOrderSRVList = update.updates;

            //TO test UI with dummy data, please uncomment the region below
            //and comment the above 2 lines
            #region MockDataForTest
            //strategyResultsUpdate update = new strategyResultsUpdate();
            //update.isTrading = true;
            //update.strategyName = "MockStrategy";
            //symbolUpdate[] StrategyOrderSRVList = new symbolUpdate[6];
            //StrategyOrderSRVList[0] = mock("IBM", strategyState.Trading, 500.45, 200, 14555.45, 45555, 12, 15000);           
            //StrategyOrderSRVList[1] = mock("GOOG", strategyState.Stopped, 5100.45, 200, 45755.45, 44555, 112, 55000);
            //StrategyOrderSRVList[2] = mock("SPY", strategyState.Locked, 5030.45, 200, 45515.45, 45155, 121, 5100);
            //StrategyOrderSRVList[3] = mock("MSFT", strategyState.Trading, 2500.45, 200, 45525.45, 45155, 124, 50200);
            //StrategyOrderSRVList[4] = mock("AA", strategyState.Trading, 5500.45, 200, 45545.45, 45855, 132, 50700);
            //StrategyOrderSRVList[5] = mock("CSCO", strategyState.Trading, 5060.45, 200, 45755.45, 454255, 1, 50800);
            #endregion            
            
            List<StrategyOrderInfo> StrategyOrderList = new List<StrategyOrderInfo>();

            CovertServiceObjectToClinetObject(StrategyOrderList, StrategyOrderSRVList, update.strategyName);
            SetServerStatus(update);
            return StrategyOrderList;
        }

        private void SetServerStatus(strategyResultsUpdate update)
        {
            string logMessage = string.Format("Class: {0}, Method: {1}", "CommunicationManager", "SetServerStatus()");
            LogUtil.WriteLog(LogLevel.INFO, logMessage);

            if (update.isTrading)
            {
                this.StrategyEngine.StrategyEngineStatus = "Running";
            }
            else
            {
                this.StrategyEngine.StrategyEngineStatus = "Stopped";
            }

            this.StrategyEngine.SeedQtyThreshold = update.seedQtyThreshold;

            this.StrategyEngine.APIState = update.apiState.ToString();
            this.StrategyEngine.IsConnected = true;
        }

        public List<StrategyOrderInfo> ProcessSymbol(string StrategyId, string[] symbol, ProcessType _processType)
        {
            string logMessage = string.Format("Class: {0}, Method: {1}", "CommunicationManager", "ProcessSymbol(...)");
            LogUtil.WriteLog(LogLevel.DEBUG, logMessage);

            string symbolSpec = null;
            if (symbol != null)
            {
                symbolSpec = EZXWPFLibrary.Utils.StringUtils.StringListToText(symbol.ToList(), ",");
            }

            List<StrategyOrderInfo> StrategyOrderList = new List<StrategyOrderInfo>();
            strategyResultsUpdate update = new strategyResultsUpdate();
            switch (_processType)
            {
                case ProcessType.START:
                    update = Service.start(symbolSpec);
                    break;
                case ProcessType.STOP:
                    update = Service.stop(symbolSpec);
                    break;
                case ProcessType.CANCELALL:                    
                    Service.cancelAll(symbolSpec);
                    update = Service.getUpdates();
                    break;
                case ProcessType.LOCK:
                    update = Service.@lock(symbolSpec);
                    break;
                case ProcessType.UNLOCK:
                    update = Service.unlock(symbolSpec);
                    break;
                case ProcessType.UNWIND:
                    update = Service.getUpdates();
                    break;
                case ProcessType.BUY:
                    update = Service.buy(symbolSpec);
                    break;
                case ProcessType.SELL:
                    update = Service.sell(symbolSpec);
                    break;
                case ProcessType.BOTH:
                    update = Service.both(symbolSpec);
                    break;
            }

            symbolUpdate[] StrategyOrderSRVList = update.updates;
            CovertServiceObjectToClinetObject(StrategyOrderList, StrategyOrderSRVList, update.strategyName);
            SetServerStatus(update);
            return StrategyOrderList;
        }

        public List<StrategyOrderInfo> ProcessAllSymbol(string StrategyId, ProcessType _processType)
        {
            string logMessage = string.Format("Class: {0}, Method: {1}", "CommunicationManager", "ProcessAllSymbol(...)");
            LogUtil.WriteLog(LogLevel.DEBUG, logMessage);

            return ProcessSymbol(StrategyId, null, _processType);
        }

        private static void CovertServiceObjectToClinetObject(List<StrategyOrderInfo> StrategyOrderList, symbolUpdate[] StrategyOrderSRVList, string StartegyName)
        {
            string logMessage = string.Format("Class: {0}, Method: {1}", "CommunicationManager", "CovertServiceObjectToClinetObject(...)");
            LogUtil.WriteLog(LogLevel.DEBUG, logMessage);
            if (string.IsNullOrEmpty(StartegyName))
            {
                throw new QStrategyException("Startegyname is missing! Strategy name is configurable for engine in config/config.xml) ", ExceptionType.StrategyName, "CommuniationManager.CovertServiceObjectToClinetObject(...)");
            }

            if (StrategyOrderSRVList != null && !string.IsNullOrEmpty(StartegyName) && StrategyOrderSRVList.Count() > 0)
            {
                logMessage = string.Format("Class: {0}, Method: {1}, StrategyOrderSRVList.Count: {2}", "CommunicationManager", "CovertServiceObjectToClinetObject(...)", StrategyOrderSRVList.Count());
                LogUtil.WriteLog(LogLevel.DEBUG, logMessage);

                for (int i = 0; i < StrategyOrderSRVList.Count(); i++)
                {
                    logMessage = string.Format("Class: {0}, Method: {1}, Converting Server Object to Client for Symbol: {2}", "CommunicationManager", "CovertServiceObjectToClinetObject(...)", StrategyOrderSRVList[i].symbol);
                    LogUtil.WriteLog(LogLevel.DEBUG, logMessage); 
                    StrategyOrderList.Add(new StrategyOrderInfo(StartegyName, StrategyOrderSRVList[i]));
                }
            }
            else
            {
                logMessage = string.Format("Class: {0}, Method: {1}, StrategyOrderSRVList is null or empty!:", "CommunicationManager", "CovertServiceObjectToClinetObject(...)");
                LogUtil.WriteLog(LogLevel.DEBUG, logMessage);
            }
        }

    }

}
