using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using QStrategyGUILib;
using System.Windows.Data;
using System.Collections.ObjectModel;
using System.Windows.Controls;

namespace QStrategyWPF.ViewModel
{
    public partial class OrderBlotterUserControlVM : ViewModelBase
    {

        private string selectedStrategyId;

        public string SelectedStrategyId
        {
            get { return selectedStrategyId; }
            set 
            { 
                selectedStrategyId = value;
                this.RaisePropertyChanged("SelectedStrategyId");
            }
        }

        public OrderBlotterUserControlVM()
            : base()
        {
        }

        public void Init()
        {
            Strategy strategy = AppManager.DataMgr.StrategyList.Where(s => s.StrategyId == "-1").FirstOrDefault();
            this.SelectedStrategyId = strategy.StrategyId;
            FilterOrder();
        }

        public void FilterOrder()
        {
            if (AppManager.DataMgr.StrategyOrderCollectionView != null && !string.IsNullOrEmpty(this.SelectedStrategyId))
            {
                AppManager.DataMgr.StrategyOrderCollectionView.Filter =
                    delegate(object item)
                    {
                        StrategyOrderInfo obj = item as StrategyOrderInfo;
                        if (obj != null)
                        {
                            if (this.SelectedStrategyId != "-1" && this.SelectedStrategyId != obj.Strategy.StrategyId)
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
