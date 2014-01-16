using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EZXWPFLibrary.Helpers;

namespace QStrategyGUILib
{
    public partial class Strategy : ObservableBase
    {
        private string strategyId;
        private string strategyName;
        
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

        public Strategy(string strategy)
        {
            this.StrategyId = "ID"+strategy.Trim().Replace(" ",string.Empty);
            this.StrategyName = strategy;
        }
        
        public Strategy()
        {
        }
        
        public Strategy(string Id, string name)
        {
            this.StrategyId = Id;
            this.StrategyName = name;
        }
    }
}
