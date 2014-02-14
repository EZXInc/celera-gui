using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EZXWPFLibrary.Helpers;
using QStrategyWPF.Model;
using System.Collections.Concurrent;
using System.Windows.Data;
using QStrategyGUILib;
using System.Collections.ObjectModel;
using System.Windows.Threading;
using EZXWPFLibrary.Utils;

namespace QStrategyWPF
{
    public partial class DataManager : ObservableBase
    {
        public delegate void DataUpdateHandler(object sender, EventArgs e);
        public event DataUpdateHandler DataUpdateCompleted;

        private List<StrategyOrderInfo> selectedSymbolOrderInfoList;

        //Use to hold the list of seleted Orders
        public List<StrategyOrderInfo> SelectedSymbolOrderInfoList
        {
            get { return selectedSymbolOrderInfoList; }
            set { selectedSymbolOrderInfoList = value; }
        }

        
        private ConcurrentDictionary<string, SummaryOrder> summaryOrderDictionary;
        private ConcurrentDictionary<string, ConcurrentDictionary<string, StrategyOrderInfo>> strategyOrderDictionary;
        private ObservableCollection<Strategy> strategyList;

        private ListCollectionView summaryOrderCollectionView;
        private ListCollectionView strategyOrderCollectionView;
        private ListCollectionView strategyStatusOrderCollectionView;
        
        
        public ConcurrentDictionary<string, SummaryOrder> SummaryOrderDictionary
        {
            get { return summaryOrderDictionary; }
            set 
            {
                summaryOrderDictionary = value;
                this.RaisePropertyChanged(p => p.SummaryOrderDictionary);
            }
        }
        public ConcurrentDictionary<string, ConcurrentDictionary<string, StrategyOrderInfo>> StrategyOrderDictionary
        {
            get { return strategyOrderDictionary; }
            set
            {
                strategyOrderDictionary = value;
                this.RaisePropertyChanged(p => p.StrategyOrderDictionary);
            }
        }
        public ObservableCollection<Strategy> StrategyList
        {
            get { return strategyList; }
            set
            {
                strategyList = value;
                this.RaisePropertyChanged(p => p.StrategyList);
            }
        }
        
        public ListCollectionView SummaryOrderCollectionView
        {
            get { return summaryOrderCollectionView; }
            set 
            { 
                summaryOrderCollectionView = value;
                this.RaisePropertyChanged(p => p.SummaryOrderCollectionView);
            }
        }        
        public ListCollectionView StrategyOrderCollectionView
        {
            get { return strategyOrderCollectionView; }
            set 
            { 
                strategyOrderCollectionView = value;
                this.RaisePropertyChanged(p => p.StrategyOrderCollectionView);
            }
        }
        public ListCollectionView StrategyStatusOrderCollectionView
        {
            get { return strategyStatusOrderCollectionView; }
            set
            {
                strategyStatusOrderCollectionView = value;
                this.RaisePropertyChanged(p => p.StrategyStatusOrderCollectionView);
            }
        }



        private MTObservableCollection<StrategyOrderInfo> aggregateStrategyOrderInfo;
        public MTObservableCollection<StrategyOrderInfo> AggregateStrategyOrderInfo
        {
            get { return aggregateStrategyOrderInfo; }
            set
            {
                aggregateStrategyOrderInfo = value;
                this.RaisePropertyChanged(p => p.AggregateStrategyOrderInfo);
            }
        }

        private MTObservableCollection<SummaryOrder> aggregateSummaryInfo;
        public MTObservableCollection<SummaryOrder> AggregateSummaryInfo
        {
            get { return aggregateSummaryInfo; }
            set 
            { 
                aggregateSummaryInfo = value;
                this.RaisePropertyChanged(p => p.AggregateSummaryInfo);
            }
        }

        public DataManager()
            : base()
        {
            string logMessage = string.Format("Class: {0}, Method: {1}", "DataManager", "DataManager() Constructor");
            LogUtil.WriteLog(LogLevel.INFO, logMessage);

            InitializeData();

        }

        private void InitializeData()
        {
            string logMessage = string.Format("Class: {0}, Method: {1}", "DataManager", "DataManager.IntializeData()");
            LogUtil.WriteLog(LogLevel.INFO, logMessage);

            //Use to hold the list of seleted Orders
            this.SelectedSymbolOrderInfoList = new List<StrategyOrderInfo>();

            this.SummaryOrderDictionary = new ConcurrentDictionary<string, SummaryOrder>();
            this.SummaryOrderCollectionView = new ListCollectionView(this.SummaryOrderDictionary.Values.ToList());

            this.StrategyOrderDictionary = new ConcurrentDictionary<string, ConcurrentDictionary<string, StrategyOrderInfo>>();
            ObservableCollection<StrategyOrderInfo> tempStrategyOrderInfo = new ObservableCollection<StrategyOrderInfo>();
            ObservableCollection<StrategyOrderInfo> tempStrategyOrderInfo2 = new ObservableCollection<StrategyOrderInfo>();
            this.StrategyOrderCollectionView = new ListCollectionView(tempStrategyOrderInfo);
            this.StrategyStatusOrderCollectionView = new ListCollectionView(tempStrategyOrderInfo2);

            if (this.StrategyList == null)
            {
                this.StrategyList = new ObservableCollection<Strategy>();
                this.StrategyList.Add(new Strategy("-1", "All Strategies"));
            }

            this.AggregateStrategyOrderInfo = new MTObservableCollection<StrategyOrderInfo>();
            this.AggregateStrategyOrderInfo.Add(new StrategyOrderInfo());
            this.AggregateStrategyOrderInfo[0].IsSummaryRow = true;
            this.AggregateStrategyOrderInfo[0].Symbol = "Total:";

            this.AggregateSummaryInfo = new MTObservableCollection<SummaryOrder>();
            this.AggregateSummaryInfo.Add(new SummaryOrder());
            this.AggregateSummaryInfo[0].IsAggregatedRow = true;
            this.AggregateSummaryInfo[0].Status = string.Empty;
            this.AggregateSummaryInfo[0].StrategyName = "Total:";
        }

        internal void UpdateStrategyData(List<StrategyOrderInfo> updateOrderList, bool IsSendProcessReponse)
        {
            string logMessage = string.Format("Class: {0}, Method: {1}", "DataManager", "UpdateStrategyData(..)");
            LogUtil.WriteLog(LogLevel.INFO, logMessage);

            updateOrderList = updateOrderList.OrderBy(s => s.Strategy.StrategyName).ToList();
            for (int i = 0; i < updateOrderList.Count; i++)
            {
                string symbolKey = updateOrderList[i].Symbol;
                string strategyIdKey = updateOrderList[i].Strategy.StrategyId;

                //oldStrategyInfo: To hold the value of existing order, so it will use in running summary-calculation, 
                //to subtract the values, before adding the new values
                StrategyOrderInfo oldStrategyInfo = new StrategyOrderInfo();
                
                if (!this.StrategyOrderDictionary.ContainsKey(strategyIdKey))
                {
                    this.StrategyOrderDictionary[strategyIdKey] = new ConcurrentDictionary<string,StrategyOrderInfo>();
                }

                if (this.StrategyOrderDictionary[strategyIdKey].ContainsKey(symbolKey))
                {
                    StrategyOrderInfo existingStrategyOrder = this.StrategyOrderDictionary[strategyIdKey][symbolKey];
                    if (existingStrategyOrder.InProcess == true && !IsSendProcessReponse)
                    {
                        //Since row is in send process, therefore auto-update should not modify
                    }
                    else
                    {
                        oldStrategyInfo.Update(existingStrategyOrder); //Clone the values of existingStrategyOrder 
                        existingStrategyOrder.Update(updateOrderList[i]); // Update with new values 
                        ModifyListCollectionView(strategyIdKey, symbolKey, "EDIT");
                        ModifyStrategyStatusListCollectionView(strategyIdKey, symbolKey, "EDIT");
                    }
                }
                else
                {
                    oldStrategyInfo = null;
                    this.StrategyOrderDictionary[strategyIdKey][symbolKey] = updateOrderList[i];
                    RunOnDispatcherThread((Action)(() =>
                    {           
                        if (!this.StrategyList.Any(s => s.StrategyId == updateOrderList[i].Strategy.StrategyId))
                        {
                            this.StrategyList.Add(new Strategy(updateOrderList[i].Strategy.StrategyId, updateOrderList[i].Strategy.StrategyName));
                        }
                    }));
                    ModifyListCollectionView(strategyIdKey, symbolKey, "NEW");
                    ModifyStrategyStatusListCollectionView(strategyIdKey, symbolKey, "NEW");

                }
                
                if (this.SummaryOrderDictionary.ContainsKey(strategyIdKey))
                {
                    if (this.StrategyOrderDictionary[strategyIdKey][symbolKey].InProcess == true && !IsSendProcessReponse)
                    {
                        //Since row is in send process, therefore auto-update should not modify summary Row
                    }
                    else
                    {
                        this.SummaryOrderDictionary[strategyIdKey].Update(oldStrategyInfo, updateOrderList[i]);
                        ModifySummaryListCollectionView(strategyIdKey, "EDIT");
                    }
                }
                else
                {
                    this.SummaryOrderDictionary[strategyIdKey] = new SummaryOrder(updateOrderList[i]);
                    ModifySummaryListCollectionView(strategyIdKey, "NEW");
                }
            }

            if (this.DataUpdateCompleted != null)
            {
                this.DataUpdateCompleted(this, new EventArgs());
            }
        }

        private void ModifyListCollectionView(string strategyIdKey, string symbolKey, string operation)
        {
            string logMessage = string.Format("Class: {0}, Method: ModifyListCollectionView({1},{2},{3})", "DataManager", strategyIdKey, symbolKey, operation);
            LogUtil.WriteLog(LogLevel.DEBUG, logMessage);

            RunOnDispatcherThread((Action)(() =>
            {
                if (operation.Equals("EDIT"))
                {
                    this.StrategyOrderCollectionView.EditItem(this.StrategyOrderDictionary[strategyIdKey][symbolKey]);
                    this.StrategyOrderCollectionView.CommitEdit();
                }
                else if (operation.Equals("NEW"))
                {
                    this.StrategyOrderCollectionView.AddNewItem(this.StrategyOrderDictionary[strategyIdKey][symbolKey]);
                    this.StrategyOrderCollectionView.CommitNew();
                }
                else if (operation.Equals("REMOVE"))
                {
                    this.StrategyOrderCollectionView.Remove(this.StrategyOrderDictionary[strategyIdKey][symbolKey]);
                    this.StrategyOrderCollectionView.CommitEdit();
                }
            }));
        }
        private void ModifySummaryListCollectionView(string key, string operation)
        {
            string logMessage = string.Format("Class: {0}, Method: ModifySummaryListCollectionView({1},{2})", "DataManager", key, operation);
            LogUtil.WriteLog(LogLevel.DEBUG, logMessage);

            RunOnDispatcherThread((Action)(() =>
            {
                if (operation.Equals("EDIT"))
                {
                    this.SummaryOrderCollectionView.EditItem(this.SummaryOrderDictionary[key]);
                    this.SummaryOrderCollectionView.CommitEdit();
                }
                else if (operation.Equals("NEW"))
                {
                    this.SummaryOrderCollectionView.AddNewItem(this.SummaryOrderDictionary[key]);
                    this.SummaryOrderCollectionView.CommitNew();
                }
                else if (operation.Equals("REMOVE"))
                {
                    this.SummaryOrderCollectionView.Remove(this.SummaryOrderDictionary[key]);
                    this.SummaryOrderCollectionView.CommitEdit();
                }
            }));
        }
        private void ModifyStrategyStatusListCollectionView(string strategyIdKey, string symbolKey, string operation)
        {
            string logMessage = string.Format("Class: {0}, Method: ModifyStrategyStatusListCollectionView({1},{2},{3})", "DataManager", strategyIdKey, symbolKey, operation);
            LogUtil.WriteLog(LogLevel.DEBUG, logMessage);

            RunOnDispatcherThread((Action)(() =>
            {
                if (operation.Equals("EDIT"))
                {
                    this.StrategyStatusOrderCollectionView.EditItem(this.StrategyOrderDictionary[strategyIdKey][symbolKey]);
                    this.StrategyStatusOrderCollectionView.CommitEdit();
                }
                else if (operation.Equals("NEW"))
                {
                    this.StrategyStatusOrderCollectionView.AddNewItem(this.StrategyOrderDictionary[strategyIdKey][symbolKey]);
                    this.StrategyStatusOrderCollectionView.CommitNew();
                }
                else if (operation.Equals("REMOVE"))
                {
                    this.StrategyStatusOrderCollectionView.Remove(this.StrategyOrderDictionary[strategyIdKey][symbolKey]);
                    this.StrategyStatusOrderCollectionView.CommitEdit();
                }
            }));
        }

        protected void RunOnDispatcherThread(Action action)
        {
            Dispatcher currentDispatcher = App.Current.Dispatcher;
            if (currentDispatcher == null)
            {
                throw new Exception("currentDispatcher == null");
            }
            //invoke
            currentDispatcher.Invoke(action, null);
        }

        internal void ClearAll()
        {
            string logMessage = string.Format("Class: {0}, Method: {1}", "DataManager", "DataManager.ClearAll()");
            LogUtil.WriteLog(LogLevel.INFO, logMessage);

            RunOnDispatcherThread((Action)(() =>
            {
                this.SelectedSymbolOrderInfoList.Clear();


                this.SummaryOrderDictionary.Clear();
                this.SummaryOrderCollectionView.Refresh();

                this.StrategyOrderDictionary.Clear();
                this.StrategyOrderCollectionView.Refresh();

                this.StrategyStatusOrderCollectionView.Refresh();

                int strategyListCount = this.StrategyList.Count;
                for (int i = 0; i < strategyListCount; i++)
                {
                    Strategy stg = this.StrategyList[i];
                    if (!stg.StrategyId.Equals("-1"))
                    {
                        this.StrategyList.Remove(stg);
                        strategyListCount--;
                    }
                }

                this.AggregateStrategyOrderInfo.Clear();
                this.AggregateSummaryInfo.Clear();

                InitializeData();
            }));
        }

        public void ClearProcessSelectionIndication()
        {
            for (int i = 0; i < this.StrategyOrderCollectionView.Count; i++)
            {
                StrategyOrderInfo orderInfo = this.StrategyOrderCollectionView.GetItemAt(i) as StrategyOrderInfo;
                if (orderInfo != null)
                {
                    orderInfo.IsAlreadyHadSameProcess = false;
                }
            }
        }
    }
}
