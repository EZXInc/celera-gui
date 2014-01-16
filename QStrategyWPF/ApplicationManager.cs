using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EZXWPFLibrary.Helpers;
using QStrategyWPF.Model;
using EZXWPFLibrary.Utils;
using QStrategyGUILib;
using System.Windows.Threading;
using System.Configuration;
using System.IO;

namespace QStrategyWPF
{
    public partial class ApplicationManager : ObservableBase
    {
        private string baseConfigurationDirectory = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + Path.DirectorySeparatorChar + "RhinebeckQStrategy" + Path.DirectorySeparatorChar + "Configuration";
        public string BaseConfigurationDirectory
        {
            get { return baseConfigurationDirectory; }
            set
            {
                baseConfigurationDirectory = value;
                this.RaisePropertyChanged(p => p.BaseConfigurationDirectory);
            }
        }

        private DispatcherTimer autoRefreshDataTimer;
        private BackgroundWorkerTask GetStrategySymbolUpdate_Task;
        private BackgroundWorkerTask SendStrategySymbolProcess_Task;
        private BackgroundWorkerTask ConnectionStatus_Task;

        //Private Properties
        private ApplicationConnectionMode connectionMode;
        private string connectionStatus;
        private StrategyEngine stgEngine;
        private DataManager dataMgr;
        private bool autoUpdateData;
        private string strategyStartStopTime = string.Empty;


        //Public Properties
        public ApplicationConnectionMode ConnectionMode
        {
            get
            {
                return connectionMode;
            }
            set
            {
                connectionMode = value;
                switch (connectionMode)
                {
                    case ApplicationConnectionMode.CONNECTED:
                        this.ConnectionStatus = "Connected";
                        this.RaisePropertyChanged("ConnectionMode");
                        break;
                    case ApplicationConnectionMode.DISCONNECTED:
                        this.ConnectionStatus = "Disconnected";
                        this.RaisePropertyChanged("ConnectionMode");
                        break;
                    case ApplicationConnectionMode.LOGOUT:
                        this.ConnectionStatus = "Logout";
                        this.RaisePropertyChanged("ConnectionMode");
                        break;
                    case ApplicationConnectionMode.SUSPENDLOGIN:
                        this.ConnectionStatus = "Cancel Login";
                        this.RaisePropertyChanged("ConnectionMode");
                        break;
                    default:
                        this.ConnectionStatus = "Invalid";
                        this.RaisePropertyChanged("ConnectionMode");
                        break;
                }
                this.RaisePropertyChanged("ConnectionMode");
            }
        }
        public string ConnectionStatus
        {
            get { return connectionStatus; }
            set
            {
                connectionStatus = value;
                this.RaisePropertyChanged("ConnectionStatus");
            }
        }
        public StrategyEngine StgEngine
        {
            get { return stgEngine; }
            set
            {
                stgEngine = value;
                this.RaisePropertyChanged(p => p.StgEngine);
            }
        }
        public DataManager DataMgr
        {
            get { return dataMgr; }
            set
            {
                dataMgr = value;
                this.RaisePropertyChanged(p => p.DataMgr);
            }
        }
        public bool AutoUpdateData
        {
            get { return autoUpdateData; }
            set
            {
                autoUpdateData = value;
                this.RaisePropertyChanged(p => p.AutoUpdateData);
            }
        }
        public string StrategyStartStopTime
        {
            get { return strategyStartStopTime; }
            set
            {
                strategyStartStopTime = value;
                this.RaisePropertyChanged(p => p.StrategyStartStopTime);
            }
        }



        public event GetSymbolDataUpdateHandler GetSymbolUpdatedDataCompleted;
        public delegate void GetSymbolDataUpdateHandler(object sender, EventArgs e);

        public ApplicationManager()
            : base()
        {
            string logMessage = string.Format("Class: {0}, Method: {1}, ApplicationManager Started", "ApplicationManager", "ApplicationManager(...)");
            LogUtil.WriteLog(LogLevel.INFO, logMessage);

            ICommunicationManager comMgr = new CommunicationManager();
            Initialize(comMgr);
        }

        public ApplicationManager(ICommunicationManager comMgr)
            : base()
        {
            string logMessage = string.Format("Class: {0}, Method: {1}", "ApplicationManager", "ApplicationManager(ICommunicationManager comMgr)");
            LogUtil.WriteLog(LogLevel.INFO, logMessage);

            Initialize(comMgr);
        }

        private void Initialize(ICommunicationManager comMgr)
        {
            string logMessage = string.Format("Class: {0}, Method: {1}", "ApplicationManager", "Initialize(CommunicationManager comMgr)");
            LogUtil.WriteLog(LogLevel.INFO, logMessage);

            this.StgEngine = new StrategyEngine(comMgr);
            this.DataMgr = new DataManager();
        }

        internal void Init()
        {
            string logMessage = string.Format("Class: {0}, Method: {1}", "ApplicationManager", "Init()");
            LogUtil.WriteLog(LogLevel.INFO, logMessage);

            GetStrategySymbolUpdate();


            int autoRefreshSec = 3;
            if ((ConfigurationManager.AppSettings.AllKeys.Contains("AUTOREFRESH_INTERVAL"))
                && (int.TryParse(ConfigurationManager.AppSettings["AUTOREFRESH_INTERVAL"], out autoRefreshSec)))
            {

            }

            logMessage = string.Format("Class: {0}, Method: {1}, Set auto-refresh interval: {2}", "ApplicationManager", "Init()", autoRefreshSec);
            LogUtil.WriteLog(LogLevel.INFO, logMessage);

            this.ConnectionStatus_Task = new BackgroundWorkerTask(CheckConnectionStatus);
            this.GetStrategySymbolUpdate_Task = new BackgroundWorkerTask(GetStrategySymbolUpdate);
            this.SendStrategySymbolProcess_Task = new BackgroundWorkerTask(SendStrategySymbolProcess);
            this.AutoUpdateData = true;

            autoRefreshDataTimer = new DispatcherTimer();
            autoRefreshDataTimer.Tick += new EventHandler(AutoRefreshDataTimer_Tick);
            autoRefreshDataTimer.Interval = new TimeSpan(0, 0, autoRefreshSec);
            autoRefreshDataTimer.Start();
        }


        private void SendStrategySymbolProcess()
        {
            bool isPreviouslyStarted = this.StgEngine.StrategyEngineStatus.Equals("Running") ? true : false;

            List<StrategyOrderInfo> StrategyOrderInfoList = new List<StrategyOrderInfo>();
            if (selectedStrategyAndSymbolDictionary != null)
            {
                if (this.isStrategyLevelProcess)
                {
                    for (int i = 0; i < this.selectedStrategyAndSymbolDictionary.Count(); i++)
                    {
                        string strategyId = this.selectedStrategyAndSymbolDictionary.Keys.ToList()[i];
                        StrategyOrderInfoList.AddRange(this.StgEngine.ProcessAllSymbol(strategyId, this.currentProcess));

                    }
                }
                else
                {
                    for (int i = 0; i < this.selectedStrategyAndSymbolDictionary.Count(); i++)
                    {
                        string strategyId = this.selectedStrategyAndSymbolDictionary.Keys.ToList()[i];
                        string[] symbolList = this.selectedStrategyAndSymbolDictionary.Values.ToList()[i].ToArray();
                        StrategyOrderInfoList.AddRange(this.StgEngine.ProcessSymbol(strategyId, symbolList, this.currentProcess));
                    }
                }

                this.DataMgr.UpdateStrategyData(StrategyOrderInfoList, true);
                this.currentProcess = ProcessType.NA;
                this.selectedStrategyAndSymbolDictionary = new Dictionary<string, List<string>>();
                this.isStrategyLevelProcess = false;
            }

            bool isCurrenlyStarted = this.StgEngine.StrategyEngineStatus.Equals("Running") ? true : false;

            if (isCurrenlyStarted != isPreviouslyStarted)
            {
                if (isCurrenlyStarted)
                {
                    this.StrategyStartStopTime = string.Format("(Running since: {0})", DateTime.Now.ToLongTimeString());
                }
                else
                {
                    this.StrategyStartStopTime = string.Format("(Stopped since: {0})", DateTime.Now.ToLongTimeString());
                }
            }
        }

        #region All Process

        private ProcessType currentProcess;
        private bool isStrategyLevelProcess;
        private Dictionary<string, List<string>> selectedStrategyAndSymbolDictionary;

        //Start
        public void Start(Dictionary<string, List<string>> _strategySymbolDictionary, bool allSymbol)
        {
            string logMessage = string.Format("Class: {0}, Method: {1}", "ApplicationManager", "Start(Dictionary<string, List<string>> _strategySymbolDictionary, bool allSymbol)");
            LogUtil.WriteLog(LogLevel.INFO, logMessage);

            this.currentProcess = ProcessType.START;
            this.selectedStrategyAndSymbolDictionary = _strategySymbolDictionary;
            this.isStrategyLevelProcess = allSymbol;
            this.SendStrategySymbolProcess_Task.RunProcess();
        }

        //Stop  
        public void Stop(Dictionary<string, List<string>> _strategySymbolDictionary, bool allSymbol)
        {
            string logMessage = string.Format("Class: {0}, Method: {1}", "ApplicationManager", "Stop(Dictionary<string, List<string>> _strategySymbolDictionary, bool allSymbol)");
            LogUtil.WriteLog(LogLevel.INFO, logMessage);

            this.currentProcess = ProcessType.STOP;
            this.selectedStrategyAndSymbolDictionary = _strategySymbolDictionary;
            this.isStrategyLevelProcess = allSymbol;
            this.SendStrategySymbolProcess_Task.RunProcess();
        }

        //Cancel
        public void Cancel(Dictionary<string, List<string>> _strategySymbolDictionary, bool allSymbol)
        {
            string logMessage = string.Format("Class: {0}, Method: {1}", "ApplicationManager", "Cancel(Dictionary<string, List<string>> _strategySymbolDictionary, bool allSymbol)");
            LogUtil.WriteLog(LogLevel.INFO, logMessage);

            this.currentProcess = ProcessType.CANCELALL;
            this.selectedStrategyAndSymbolDictionary = _strategySymbolDictionary;
            this.isStrategyLevelProcess = allSymbol;
            this.SendStrategySymbolProcess_Task.RunProcess();
        }

        //Buy
        public void Buy(Dictionary<string, List<string>> _strategySymbolDictionary, bool allSymbol)
        {
            string logMessage = string.Format("Class: {0}, Method: {1}", "ApplicationManager", "Buy(Dictionary<string, List<string>> _strategySymbolDictionary, bool allSymbol)");
            LogUtil.WriteLog(LogLevel.INFO, logMessage);

            this.currentProcess = ProcessType.BUY;
            SetSymbolAndSend(_strategySymbolDictionary, allSymbol);
        }

        //Sell
        public void Sell(Dictionary<string, List<string>> _strategySymbolDictionary, bool allSymbol)
        {
            string logMessage = string.Format("Class: {0}, Method: {1}", "ApplicationManager", "Sell(Dictionary<string, List<string>> _strategySymbolDictionary, bool allSymbol)");
            LogUtil.WriteLog(LogLevel.INFO, logMessage);

            this.currentProcess = ProcessType.SELL;
            SetSymbolAndSend(_strategySymbolDictionary, allSymbol);
        }

        //Both
        public void Both(Dictionary<string, List<string>> _strategySymbolDictionary, bool allSymbol)
        {
            string logMessage = string.Format("Class: {0}, Method: {1}", "ApplicationManager", "Both(Dictionary<string, List<string>> _strategySymbolDictionary, bool allSymbol)");
            LogUtil.WriteLog(LogLevel.INFO, logMessage);

            this.currentProcess = ProcessType.BOTH;
            SetSymbolAndSend(_strategySymbolDictionary, allSymbol);
        }

        private void SetSymbolAndSend(Dictionary<string, List<string>> _strategySymbolDictionary, bool allSymbol)
        {
            this.selectedStrategyAndSymbolDictionary = _strategySymbolDictionary;
            this.isStrategyLevelProcess = allSymbol;
            this.SendStrategySymbolProcess_Task.RunProcess();
        }


        //Unwind
        public void Unwind(Dictionary<string, List<string>> _strategySymbolDictionary, bool allSymbol)
        {
            string logMessage = string.Format("Class: {0}, Method: {1}", "ApplicationManager", "Unwind(Dictionary<string, List<string>> _strategySymbolDictionary, bool allSymbol)");
            LogUtil.WriteLog(LogLevel.INFO, logMessage);

            this.currentProcess = ProcessType.UNWIND;
            this.selectedStrategyAndSymbolDictionary = _strategySymbolDictionary;
            this.isStrategyLevelProcess = allSymbol;
            this.SendStrategySymbolProcess_Task.RunProcess();
        }

        //Lock
        public void Lock(Dictionary<string, List<string>> _strategySymbolDictionary, bool allSymbol)
        {
            string logMessage = string.Format("Class: {0}, Method: {1}", "ApplicationManager", "Lock(Dictionary<string, List<string>> _strategySymbolDictionary, bool allSymbol)");
            LogUtil.WriteLog(LogLevel.INFO, logMessage);

            this.currentProcess = ProcessType.LOCK;
            this.selectedStrategyAndSymbolDictionary = _strategySymbolDictionary;
            this.isStrategyLevelProcess = allSymbol;
            this.SendStrategySymbolProcess_Task.RunProcess();
        }

        //Unlock
        public void Unlock(Dictionary<string, List<string>> _strategySymbolDictionary, bool allSymbol)
        {
            string logMessage = string.Format("Class: {0}, Method: {1}", "ApplicationManager", "Unlock(Dictionary<string, List<string>> _strategySymbolDictionary, bool allSymbol)");
            LogUtil.WriteLog(LogLevel.INFO, logMessage);

            this.currentProcess = ProcessType.UNLOCK;
            this.selectedStrategyAndSymbolDictionary = _strategySymbolDictionary;
            this.isStrategyLevelProcess = allSymbol;
            this.SendStrategySymbolProcess_Task.RunProcess();
        }
        #endregion

        private void GetStrategySymbolUpdate()
        {
            try
            {
                this.AutoUpdateData = false;
                List<StrategyOrderInfo> orderList = this.StgEngine.GetStrategyUpdate();
                if (!this.StgEngine.IsInvalidSymbolUpdateResponse)
                {
                    this.DataMgr.UpdateStrategyData(orderList, false);
                    this.AutoUpdateData = true;
                }
                else
                {
                    this.AutoUpdateData = false;
                }
            }
            catch (Exception ex)
            {
                LogUtil.WriteLog(LogLevel.ERROR, ex.Message);
                this.AutoUpdateData = false;
            }

            if (this.GetSymbolUpdatedDataCompleted != null)
            {
                this.GetSymbolUpdatedDataCompleted(this, new EventArgs());
            }
        }

        public void CheckConnectionStatus()
        {
            string logMessage = string.Format("Class: {0}, Method: {1}", "ApplicationManager", "CheckConnectionStatus()");
            LogUtil.WriteLog(LogLevel.INFO, logMessage);

            bool previousStatus = this.StgEngine.IsConnected;
            bool connectionStatus = this.StgEngine.IsServerConnected();


            if ((previousStatus == true) && (connectionStatus == false))
            {
                //Disconnected after connect
            }
            else if ((previousStatus == false) && (connectionStatus == true))
            {
               this.DataMgr.ClearAll();
               GetStrategySymbolUpdate();
               this.StrategyStartStopTime = string.Empty;
            }
            else if (connectionStatus == false)
            {
                this.AutoUpdateData = false;
            }
            else if (connectionStatus == true)
            {
                this.AutoUpdateData = true;
            }
        }

        void AutoRefreshDataTimer_Tick(object sender, EventArgs e)
        {
            string logMessage = string.Format("Class: {0}, Method: {1}", "ApplicationManager", "AutoRefreshDataTimer_Tick(...)");
            LogUtil.WriteLog(LogLevel.INFO, logMessage);

            if (this.AutoUpdateData)
            {
                this.GetStrategySymbolUpdate_Task.RunProcess();
            }
            else
            {
                this.ConnectionStatus_Task.RunProcess();
            }
        }

        public void RunOnDispatcherThread(Action action)
        {
            Dispatcher currentDispatcher = null;
            currentDispatcher = Dispatcher.CurrentDispatcher;
            if (currentDispatcher == null)
            {
                throw new Exception("currentDispatcher == null");
            }
            //invoke
            currentDispatcher.Invoke(action, null);
        }
    }
}
