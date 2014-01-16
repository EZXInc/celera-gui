using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EZXWPFLibrary.Helpers;
using System.Collections.ObjectModel;
using EZXWPFLibrary.Utils;
namespace QStrategyGUILib
{
    public partial class StrategyEngine : ObservableBase
    {
        private bool isConnected;
        private ICommunicationManager comMgr;
        private string apiState;
        private string strategyEngineStatus;
        private bool isStrategyEngineNotRunning;
        private bool isInvalidSymbolUpdateResponse;
        private string lastInvalidSymbolUpdateMessage;
        private string lastInvalidSymbolUpdateMessageDetail;
        private int seedQtyThreshold;

        public bool IsConnected
        {
            get { return isConnected; }
            set
            {
                isConnected = value;
                this.RaisePropertyChanged(p => p.IsConnected);
            }
        }
        public ICommunicationManager ComMgr
        {
            get { return comMgr; }
            set { comMgr = value; }
        }
        public string APIState
        {
            get { return apiState; }
            set
            {
                apiState = value;
                this.RaisePropertyChanged(p => p.APIState);
            }
        }
        public string StrategyEngineStatus
        {
            get { return strategyEngineStatus; }
            set
            {
                strategyEngineStatus = value;
                this.RaisePropertyChanged(p => p.StrategyEngineStatus);
                if (strategyEngineStatus.Equals("Running"))
                {
                    IsStrategyEngineNotRunning = false;
                }
                else
                {
                    IsStrategyEngineNotRunning = true;
                }
            }
        }
        public bool IsStrategyEngineNotRunning
        {
            get { return isStrategyEngineNotRunning; }
            set 
            { 
                isStrategyEngineNotRunning = value;
                this.RaisePropertyChanged(p => p.IsStrategyEngineNotRunning);
            }
        }

        public bool IsInvalidSymbolUpdateResponse
        {
            get { return isInvalidSymbolUpdateResponse; }
            set
            {
                isInvalidSymbolUpdateResponse = value;
                this.RaisePropertyChanged(p => p.IsInvalidSymbolUpdateResponse);
            }
        }
        public string LastInvalidSymbolUpdateMessage
        {
            get { return lastInvalidSymbolUpdateMessage; }
            set
            {
                lastInvalidSymbolUpdateMessage = value;
                this.RaisePropertyChanged(p => p.LastInvalidSymbolUpdateMessage);
            }
        }
        public string LastInvalidSymbolUpdateMessageDetail
        {
            get { return lastInvalidSymbolUpdateMessageDetail; }
            set
            {
                lastInvalidSymbolUpdateMessageDetail = value;
                this.RaisePropertyChanged(p => p.LastInvalidSymbolUpdateMessageDetail);
            }
        }
        public int SeedQtyThreshold
        {
            get { return seedQtyThreshold; }
            set 
            { 
                seedQtyThreshold = value;
                this.RaisePropertyChanged(p => p.seedQtyThreshold);
            }
        }


        private string webServiceURL;
        public string WebServiceURL
        {
            get { return webServiceURL; }
            set
            {
                webServiceURL = value;
                this.RaisePropertyChanged(p => p.WebServiceURL);
            }
        }

        public StrategyEngine(ICommunicationManager comMgr)
            : base()
        {
            string logMessage = string.Format("Class: {0}, Method: {1}, StrategyEngine Started", "StrategyEngine", "StrategyEngine(..)");
            LogUtil.WriteLog(LogLevel.INFO, logMessage);

            this.ComMgr = comMgr;
            comMgr.StrategyEngine = this;
        }


        public List<StrategyOrderInfo> GetStrategyUpdate()
        {
            string logMessage = string.Format("Class: {0}, Method: {1}", "StrategyEngine", "List<StrategyOrderInfo> GetStrategyUpdate()");
            LogUtil.WriteLog(LogLevel.INFO, logMessage);

            try
            {
                List<StrategyOrderInfo> StrategyOrderInfo = this.ComMgr.GetStrategyUpdate();
                ClearInvalidResponse();
                return StrategyOrderInfo;
            }
            catch (Exception ex)
            {
                string location = string.Format("Class: {0}, Method:{1}", "StrategyEngine", "public List<StrategyOrder> GetStrategyUpdate()");
                QStrategyException rex = new QStrategyException(ex.Message, ex, ExceptionType.ReceiveSymbolUpdateException, location);
                RecordInvalidResponse(rex, location);
                return new List<StrategyOrderInfo>();
            }
        }

        private void RecordInvalidResponse(QStrategyException rex, string location)
        {
            string logMessage = string.Format("Class: {0}, Method: {1}", "StrategyEngine", "RecordInvalidResponse(...)");
            LogUtil.WriteLog(LogLevel.INFO, logMessage);

            this.IsInvalidSymbolUpdateResponse = true;
            Exception excep = rex;
            while (excep.InnerException != null)
            {
                excep = excep.InnerException;
            }

            if (excep is System.Net.Sockets.SocketException || excep is System.Net.WebException)
            {
                this.IsConnected = false;
                this.APIState = "";
            }
            this.LastInvalidSymbolUpdateMessage = excep.Message;
            this.LastInvalidSymbolUpdateMessageDetail = excep.StackTrace;
            throw excep;
        }


        public List<StrategyOrderInfo> ProcessSymbol(string _strategyId, string[] _symbolList, ProcessType _processType)
        {
            string logMessage = string.Format("Class: {0}, Method: {1}", "StrategyEngine", "ProcessSymbol(...)");
            LogUtil.WriteLog(LogLevel.INFO, logMessage);

            try
            {
                List<StrategyOrderInfo> StrategyOrderInfoList = this.ComMgr.ProcessSymbol(_strategyId, _symbolList, _processType);
                ClearInvalidResponse(); 
                return StrategyOrderInfoList;
            }
            catch (Exception ex)
            {
                string location = string.Format("Class: {0}, Method:{1}", "StrategyEngine", "public List<StrategyOrder> ProcessSymbol(string " + _strategyId + ",_symbolList, " + _processType + ")");
                QStrategyException qex = new QStrategyException(ex.Message, ex, ExceptionType.ProcessSymbolException, location);
                RecordInvalidResponse(qex, location);
                return new List<StrategyOrderInfo>();
            }
        }

        public List<StrategyOrderInfo> ProcessAllSymbol(string _strategyId, ProcessType _processType)
        {
            string logMessage = string.Format("Class: {0}, Method: {1}", "StrategyEngine", "ProcessAllSymbol(...)");
            LogUtil.WriteLog(LogLevel.INFO, logMessage);

            try
            {
                List<StrategyOrderInfo> StrategyOrderInfoList = this.ComMgr.ProcessAllSymbol(_strategyId, _processType);
                ClearInvalidResponse();
                return StrategyOrderInfoList; 
            }
            catch (Exception ex)
            {
                string location = string.Format("Class: {0}, Method:{1}", "StrategyEngine", "public List<StrategyOrder> ProcessAllSymbol(string " + _strategyId + ", " + _processType + ")");
                QStrategyException qex = new QStrategyException(ex.Message, ex, ExceptionType.ProcessAllSymbolException, location);
                RecordInvalidResponse(qex, location);
                return new List<StrategyOrderInfo>();
            }
        }

        public bool IsServerConnected()
        {
            string logMessage = string.Format("Class: {0}, Method: {1}", "StrategyEngine", "IsServerConnected()");
            LogUtil.WriteLog(LogLevel.INFO, logMessage);

            try
            {
                bool serverStatus = false;
                serverStatus = this.ComMgr.CheckConnection();
                this.IsConnected = serverStatus;
                return serverStatus;
            }
            catch (Exception ex)
            {
                this.IsConnected = false;
                string location = string.Format("Class: {0}, Method:{1}", "StrategyEngine", "public bool IsServerConnected()");
                QStrategyException qex = new QStrategyException(ex.Message, ex, ExceptionType.ServerConnectionException, location);
                this.StrategyEngineStatus = "";

                while (ex.InnerException != null)
                {
                    ex = ex.InnerException;
                }

                if (ex is System.Net.Sockets.SocketException || ex is System.Net.WebException)
                {
                    this.IsConnected = false;
                    this.APIState = "";
                }
                else
                {
                    RecordInvalidResponse(qex, location);
                }
                return false;
            }
        }

        public void ClearInvalidResponse()
        {
            string logMessage = string.Format("Class: {0}, Method: {1}", "StrategyEngine", "ClearInvalidResponse()");
            LogUtil.WriteLog(LogLevel.INFO, logMessage);

            this.IsInvalidSymbolUpdateResponse = false;
            this.LastInvalidSymbolUpdateMessage = string.Empty;
            this.LastInvalidSymbolUpdateMessageDetail = string.Empty;
        }
    }
}
