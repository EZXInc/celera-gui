using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EZXWPFLibrary.Helpers;
using QStrategyGUILib.QStrategySVR;
using EZXWPFLibrary.Utils;


namespace QStrategyGUILib
{
    public class StrategyOrderInfo : ObservableBase
    {
        private bool inProcess;
        public bool InProcess
        {
            get { return inProcess; }
            set
            {
                inProcess = value;
                this.RaisePropertyChanged(p => p.InProcess);
            }
        }
        private bool isSummaryRow;
        public bool IsSummaryRow
        {
            get { return isSummaryRow; }
            set
            {
                isSummaryRow = value;
                this.RaisePropertyChanged(p => p.IsSummaryRow);
            }
        }

        private bool isHitByMaxLoss;
        public bool IsHitByMaxLoss
        {
            get { return isHitByMaxLoss; }
            set
            {
                isHitByMaxLoss = value;
                this.RaisePropertyChanged(p => p.IsHitByMaxLoss);
            }
        }

        private symbolUpdate strategyOrderSRV;
        private bool isAlreadyHadSameProcess;

        private Strategy strategy;
        public Strategy Strategy
        {
            get { return strategy; }
            set
            {
                strategy = value;
                this.RaisePropertyChanged(p => p.Strategy);
            }
        }

        public string StrategyId
        {
            get
            {
                return Strategy.StrategyId;
            }
        }
        public string Symbol
        {
            get { return StrategyOrderSRV.symbol; }
            set
            {
                StrategyOrderSRV.symbol = value;
                this.RaisePropertyChanged(p => p.Symbol);
            }
        }
        public string Status
        {
            get
            {
                if (StrategyOrderSRV == null || this.IsSummaryRow)
                {
                    return string.Empty;
                }
                
                if (StrategyOrderSRV.state == strategyState.MaxLoss)
                {
                    this.IsHitByMaxLoss = true;
                }
                else if (StrategyOrderSRV.state == strategyState.Trading)
                {
                    this.IsHitByMaxLoss = false;
                }

                return StrategyOrderSRV.state.ToString();
            }
            set
            {
                strategyState tempStatus;
                if (Enum.TryParse(value, out tempStatus))
                {
                    StrategyOrderSRV.state = tempStatus;
                }
                this.RaisePropertyChanged(p => p.Status);
            }
        }
        public double PnL
        {
            get { return StrategyOrderSRV.pnl; }
            set
            {
                StrategyOrderSRV.pnl = value;
                this.RaisePropertyChanged(p => p.PnL);
                CalculatePnlPerShares();
            }
        }
        public double UR_PnL
        {
            get { return StrategyOrderSRV.unrealizedPnl; }
            set
            {
                StrategyOrderSRV.unrealizedPnl = value;
                this.RaisePropertyChanged(p => p.UR_PnL);
            }
        }
        public int Position_Shares
        {
            get { return StrategyOrderSRV.position; }
            set
            {
                StrategyOrderSRV.position = value;
                this.RaisePropertyChanged(p => p.Position_Shares);
            }
        }
        public double Position_Amount
        {
            get { return StrategyOrderSRV.positionValue; }
            set
            {
                StrategyOrderSRV.positionValue = value;
                this.RaisePropertyChanged(p => p.Position_Amount);
            }
        }
        public int Volume
        {
            get { return StrategyOrderSRV.volume; }
            set
            {
                StrategyOrderSRV.volume = value;
                this.RaisePropertyChanged(p => p.Volume);
                CalculatePnlPerShares();
            }
        }
        //public int TradingMode
        //{
        //    get { return StrategyOrderSRV; }
        //    set
        //    {
        //        StrategyOrderSRV.volume = value;
        //        this.RaisePropertyChanged(p => p.Volume);
        //        CalculatePnlPerShares();
        //    }
        //}

        private void CalculatePnlPerShares()
        {
            if (this.Volume != 0)
            {
                this.PnL_Per_Share = this.PnL / this.Volume;
            }
            else
            {
                this.PnL_Per_Share = 0;
            }
            this.RaisePropertyChanged(p => p.PnL_Per_Share);
        }
        public int Number_Of_Open_Orders
        {
            get { return StrategyOrderSRV.openOrderCount; }
            set
            {
                StrategyOrderSRV.openOrderCount = value;
                this.RaisePropertyChanged(p => p.Number_Of_Open_Orders);
            }
        }
        private double pnL_Per_Share;
        public double PnL_Per_Share
        {
            get
            {
                if (this.Volume != 0)
                {
                    return this.PnL / this.Volume;
                }
                return 0;
            }
            set
            {
                pnL_Per_Share = value;
                this.RaisePropertyChanged(p => p.PnL_Per_Share);
            }
        }
        public double Trading_Revenue
        {
            get { return StrategyOrderSRV.tradingPnl; }
            set
            {
                StrategyOrderSRV.tradingPnl = value;
                this.RaisePropertyChanged(p => p.Trading_Revenue);
            }
        }
        public double Rebate_Revenue
        {
            get { return StrategyOrderSRV.rebates; }
            set
            {
                StrategyOrderSRV.rebates = value;
                this.RaisePropertyChanged(p => p.Rebate_Revenue);
            }
        }
        public double Max_Loss
        {
            get { return StrategyOrderSRV.maxLoss; }
            set
            {
                StrategyOrderSRV.maxLoss = value;
                this.RaisePropertyChanged(p => p.Max_Loss);
            }
        }
        public int SeedRemaining
        {
            get { return StrategyOrderSRV.unroutedQty; }
            set
            {
                StrategyOrderSRV.unroutedQty = value;
                this.RaisePropertyChanged(p => p.SeedRemaining);
            }

        }
        public string[] User_Message
        {
            get { return StrategyOrderSRV.userMessages; }
            set
            {
                StrategyOrderSRV.userMessages = value;
                this.RaisePropertyChanged(p => p.User_Message);
            }
        }
        public string TradingMode
        {
            get
            {
                if (StrategyOrderSRV == null || this.IsSummaryRow
                    || StrategyOrderSRV.mode == null || !StrategyOrderSRV.modeSpecified)
                {
                    return string.Empty;
                }

                return StrategyOrderSRV.mode.ToString();
            }
            set
            {
                tradingMode temptradingMode;
                if (Enum.TryParse(value, out temptradingMode))
                {
                    StrategyOrderSRV.mode = temptradingMode;
                }
                this.RaisePropertyChanged(p => p.TradingMode);
            }
        }
        public bool IsAlreadyHadSameProcess
        {
            get { return isAlreadyHadSameProcess; }
            set
            {
                isAlreadyHadSameProcess = value;
                this.RaisePropertyChanged(p => p.IsAlreadyHadSameProcess);
            }
        }

        internal symbolUpdate StrategyOrderSRV
        {
            get { return strategyOrderSRV; }
            set
            {
                strategyOrderSRV = value;
                this.RaisePropertyChanged(p => p.StrategyOrderSRV);
            }
        }

        public StrategyOrderInfo()
        {
            this.StrategyOrderSRV = new symbolUpdate();
            this.Strategy = new Strategy();
        }

        public StrategyOrderInfo(string strategyName, symbolUpdate _strategyOrderSRV)
        {
            string logMessage = string.Format("Class: {0}, Method: {1}", "StrategyOrderInfo", "StrategyOrderInfo(string strategyName, symbolUpdate _strategyOrderSRV)(...)");
            LogUtil.WriteLog(LogLevel.DEBUG, logMessage);


            string _strategyOrderSRVSLog = EZXWPFLibrary.Utils.StringUtils.GetObjectLog(_strategyOrderSRV);
            logMessage = string.Format("Class: {0}, Method: {1}, symbolUpdate: {2}", "StrategyOrderInfo", "StrategyOrderInfo(string strategyName, symbolUpdate _strategyOrderSRV)", _strategyOrderSRVSLog);
            LogUtil.WriteLog(LogLevel.DEBUG, logMessage); 


            this.Strategy = new Strategy(strategyName);

            this.StrategyOrderSRV = _strategyOrderSRV;

            string StrategyOrderInfoLog = EZXWPFLibrary.Utils.StringUtils.GetObjectLog(this);
            logMessage = string.Format("Class: {0}, Method: {1}, After mapping in StrategyOrderInfo: {2}", "StrategyOrderInfo", "StrategyOrderInfo(string strategyName, symbolUpdate _strategyOrderSRV)", StrategyOrderInfoLog);
            LogUtil.WriteLog(LogLevel.DEBUG, logMessage); 

        }

        public StrategyOrderInfo(Strategy _strategyInfo, string _symbol, string _status, double _pnl, double _ur_PnL, int _posShares, double _posAmnt,
            int _openOrders, int _vol, double _pnlPerShares, double _tradeingRev, double _rebateRev, double _maxLoss, int seedRemaining)
        {
            if (this.StrategyOrderSRV == null)
            {
                this.StrategyOrderSRV = new symbolUpdate();
            }
            this.Strategy = _strategyInfo;
            this.Symbol = _symbol;
            this.Status = _status;
            this.PnL = _pnl;
            this.UR_PnL = _ur_PnL;
            this.Position_Shares = _posShares;
            this.Position_Amount = _posAmnt;
            this.Number_Of_Open_Orders = _openOrders;
            this.Volume = _vol;
            this.Trading_Revenue = _tradeingRev;
            this.Rebate_Revenue = _rebateRev;
            this.Max_Loss = _maxLoss;
            this.User_Message = new string[1];
            this.User_Message[0] = string.Empty;
            this.SeedRemaining = seedRemaining;
        }


        public void Update(StrategyOrderInfo strategyOrderInfo)
        {
            this.Strategy.StrategyId = strategyOrderInfo.Strategy.StrategyId;
            this.Strategy.StrategyName = strategyOrderInfo.Strategy.StrategyName;
            this.Symbol = strategyOrderInfo.Symbol;
            this.Status = strategyOrderInfo.Status;
            this.TradingMode = strategyOrderInfo.TradingMode;
            this.PnL = strategyOrderInfo.PnL;
            this.UR_PnL = strategyOrderInfo.UR_PnL;
            this.Position_Shares = strategyOrderInfo.Position_Shares;
            this.Position_Amount = strategyOrderInfo.Position_Amount;
            this.Number_Of_Open_Orders = strategyOrderInfo.Number_Of_Open_Orders;
            this.Volume = strategyOrderInfo.Volume;
            this.Trading_Revenue = strategyOrderInfo.Trading_Revenue;
            this.Rebate_Revenue = strategyOrderInfo.Rebate_Revenue;
            this.Max_Loss = strategyOrderInfo.Max_Loss;
            this.SeedRemaining = strategyOrderInfo.SeedRemaining;
            this.User_Message = strategyOrderInfo.User_Message;
            this.InProcess = false;
        }
    }
}
