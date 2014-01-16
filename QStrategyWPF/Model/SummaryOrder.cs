using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EZXWPFLibrary.Helpers;
using QStrategyGUILib;

namespace QStrategyWPF.Model
{
    public partial class SummaryOrder : ObservableBase
    {
        private string strategyId;
        private string strategyName;
        private string status;
        private string symbolTrading;
        private double pnL;
        private double ur_PnL;        
        private int positionShares;
        private double positionAmount;
        private int openOrders;
        private int volume;
        private double pnLPerShares;
        private double tradingRevenue;
        private double rebateRevenue;
        private double maxLoss;
        private int seedRemaining;
        private int tradingCount;
        private int totalCount;
        private bool isAggregatedRow;

        public string StrategyId
        {
            get { return strategyId; }
            set
            {
                strategyId = value;
                this.RaisePropertyChanged(p => p.StrategyId);
            }
        }
        public string StrategyName
        {
            get { return strategyName; }
            set
            {
                strategyName = value;
                this.RaisePropertyChanged(p => p.StrategyName);
            }
        }
        public string Status
        {
            get { return status; }
            set
            {
                status = value;
                this.RaisePropertyChanged(p => p.Status);
            }
        }
        public string SymbolTrading
        {
            get { return symbolTrading; }
            set
            {
                symbolTrading = value;
                this.RaisePropertyChanged(p => p.SymbolTrading);
            }
        }
        public double PnL
        {
            get { return pnL; }
            set
            {
                pnL = value;
                this.RaisePropertyChanged(p => p.PnL);
            }
        }
        public double UR_PnL
        {
            get { return ur_PnL; }
            set
            {
                ur_PnL = value;
                this.RaisePropertyChanged(p => p.UR_PnL);
            }
        }        
        public int PositionShares
        {
            get { return positionShares; }
            set
            {
                positionShares = value;
                this.RaisePropertyChanged(p => p.PositionShares);
            }
        }
        public double PositionAmount
        {
            get { return positionAmount; }
            set
            {
                positionAmount = value;
                this.RaisePropertyChanged(p => p.PositionAmount);
            }
        }
        public int OpenOrders
        {
            get { return openOrders; }
            set
            {
                openOrders = value;
                this.RaisePropertyChanged(p => p.OpenOrders);
            }
        }
        public int Volume
        {
            get { return volume; }
            set
            {
                volume = value;
                this.RaisePropertyChanged(p => p.Volume);
                if (this.Volume != 0)
                {
                    this.PnLPerShares = this.PnL / this.Volume;
                }
                else
                {
                    this.PnLPerShares = 0;
                }

            }
        }
        public double PnLPerShares
        {
            get { return pnLPerShares; }
            set
            {
                pnLPerShares = value;
                this.RaisePropertyChanged(p => p.PnLPerShares);
            }
        }
        public double TradingRevenue
        {
            get { return tradingRevenue; }
            set
            {
                tradingRevenue = value;
                this.RaisePropertyChanged(p => p.TradingRevenue);
            }
        }
        public double RebateRevenue
        {
            get { return rebateRevenue; }
            set
            {
                rebateRevenue = value;
                this.RaisePropertyChanged(p => p.RebateRevenue);
            }
        }
        public double MaxLoss
        {
            get { return maxLoss; }
            set
            {
                maxLoss = value;
                this.RaisePropertyChanged(p => p.MaxLoss);
            }
        }
        public int SeedRemaining
        {
            get { return seedRemaining; }
            set
            {
                seedRemaining = value;
                this.RaisePropertyChanged(p => p.SeedRemaining);
            }
        }

        public int TradingCount
        {
            get { return tradingCount; }
            set
            {
                tradingCount = value;
                this.RaisePropertyChanged(p => p.TradingCount);
            }
        }
        public int TotalCount
        {
            get { return totalCount; }
            set
            {
                totalCount = value;
                this.RaisePropertyChanged(p => p.TotalCount);
            }
        }
        public bool IsAggregatedRow
        {
            get { return isAggregatedRow; }
            set
            {
                isAggregatedRow = value;
                this.RaisePropertyChanged(p => p.IsAggregatedRow);
            }
        }

        public SummaryOrder()
            : base()
        {

        }

        public SummaryOrder(StrategyOrderInfo strategyOrderInfo)
        {
            this.StrategyId = strategyOrderInfo.Strategy.StrategyId;
            this.StrategyName = strategyOrderInfo.Strategy.StrategyName;
            this.Status = App.AppManager.StgEngine.StrategyEngineStatus;
            this.TotalCount = 1;
            this.TradingCount = 0;
            if (strategyOrderInfo.Status.Equals("Trading"))
            {
                this.TradingCount = 1;
            }

            this.Status = App.AppManager.StgEngine.StrategyEngineStatus;
            this.SymbolTrading = string.Format("{0} of {1}", this.TradingCount, this.TotalCount);
            this.PnL = strategyOrderInfo.PnL;
            this.UR_PnL = strategyOrderInfo.UR_PnL;
            this.PositionShares = strategyOrderInfo.Position_Shares;
            this.PositionAmount = strategyOrderInfo.Position_Amount;
            this.OpenOrders = strategyOrderInfo.Number_Of_Open_Orders;
            this.Volume = strategyOrderInfo.Volume;
            this.TradingRevenue = strategyOrderInfo.Trading_Revenue;
            this.RebateRevenue = strategyOrderInfo.Rebate_Revenue;
            this.MaxLoss = strategyOrderInfo.Max_Loss;
            this.SeedRemaining = strategyOrderInfo.SeedRemaining;
        }

        internal void Update(StrategyOrderInfo oldStrategyOrderInfo, StrategyOrderInfo strategyOrderInfo)
        {
            if (oldStrategyOrderInfo != null)
            {
                if (oldStrategyOrderInfo.Status.Equals("Trading"))
                {
                    this.TradingCount = this.TradingCount - 1;
                }
                this.TotalCount -= 1;
                this.PnL -= oldStrategyOrderInfo.PnL;
                this.UR_PnL -= oldStrategyOrderInfo.UR_PnL;
                this.PositionShares -= oldStrategyOrderInfo.Position_Shares;
                this.PositionAmount -= oldStrategyOrderInfo.Position_Amount;
                this.Volume -= oldStrategyOrderInfo.Volume;
                this.OpenOrders -= oldStrategyOrderInfo.Number_Of_Open_Orders;
                this.TradingRevenue -= oldStrategyOrderInfo.Trading_Revenue;
                this.RebateRevenue -= oldStrategyOrderInfo.Rebate_Revenue;
                this.MaxLoss -= oldStrategyOrderInfo.Max_Loss;
                this.SeedRemaining -= oldStrategyOrderInfo.SeedRemaining;
            }

            this.StrategyName = strategyOrderInfo.Strategy.StrategyName;
            this.TotalCount += 1;
            if (strategyOrderInfo.Status.Equals("Trading"))
            {
                this.TradingCount = this.TradingCount + 1;
            }

            if (App.AppManager != null)
            {
                this.Status = App.AppManager.StgEngine.StrategyEngineStatus;
                this.SymbolTrading = string.Format("{0} of {1}", this.TradingCount, this.TotalCount);
                this.PnL += strategyOrderInfo.PnL;
                this.UR_PnL += strategyOrderInfo.UR_PnL;
                this.PositionShares += strategyOrderInfo.Position_Shares;
                this.PositionAmount += strategyOrderInfo.Position_Amount;
                this.OpenOrders += strategyOrderInfo.Number_Of_Open_Orders;
                this.Volume += strategyOrderInfo.Volume;
                this.TradingRevenue += strategyOrderInfo.Trading_Revenue;
                this.RebateRevenue += strategyOrderInfo.Rebate_Revenue;
                this.MaxLoss += strategyOrderInfo.Max_Loss;
                this.SeedRemaining += strategyOrderInfo.SeedRemaining;
            }
        }
    }
}
