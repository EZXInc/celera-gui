using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using QStrategyGUILib;
using System.Windows.Data;

namespace QStrategyWPF.ViewModel
{
    public partial class StrategyStatusVM : ViewModelBase
    {
        private string strategyStatus;

        public string StrategyStatus
        {
            get { return strategyStatus; }
            set 
            { 
                strategyStatus = value;
                this.RaisePropertyChanged("StrategyStatus");
            }
        }

        public StrategyStatusVM()
        {
        }

        public void Init(string _strategyStatus)
        {
            this.StrategyStatus = _strategyStatus;
            FilterOrder();
        }

        public void FilterOrder()
        {
            if (AppManager.DataMgr.StrategyStatusOrderCollectionView != null && !string.IsNullOrEmpty(this.StrategyStatus))
            {
                AppManager.DataMgr.StrategyStatusOrderCollectionView.Filter =
                    delegate(object item)
                    {
                        StrategyOrderInfo obj = item as StrategyOrderInfo;
                        if (obj != null)
                        {
                            if (string.IsNullOrEmpty(obj.Status) || !this.StrategyStatus.ToUpper().Equals(obj.Status.ToUpper()))
                            {
                                return false;
                            }
                        }
                        return true;
                    };
            }
        }

    }
}
