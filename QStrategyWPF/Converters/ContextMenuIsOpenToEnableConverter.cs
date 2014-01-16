using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Data;
using QStrategyGUILib;

namespace QStrategyWPF.Converters
{
    public class ContextMenuIsOpenToEnableConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (App.AppManager != null && App.AppManager.DataMgr != null && App.AppManager.DataMgr.SelectedSymbolOrderInfoList != null && App.AppManager.DataMgr.SelectedSymbolOrderInfoList.Count > 0 && parameter != null && parameter is string)
            {
                List<StrategyOrderInfo> StrategyOrderInfo = App.AppManager.DataMgr.SelectedSymbolOrderInfoList;
                int allSeletedCount = StrategyOrderInfo.Count;
                string paramValue = parameter.ToString();
                int countLocked = StrategyOrderInfo.Where(s => s.Status == "Locked").Count();
                int countTrading = StrategyOrderInfo.Where(s => s.Status == "Trading").Count();
                int countStopped = StrategyOrderInfo.Where(s => s.Status == "Stopped").Count();
                int countHung = StrategyOrderInfo.Where(s => s.Status == "Hung").Count();
                int countMAXLOSS = StrategyOrderInfo.Where(s => s.Status == "MaxLoss").Count();

                switch (paramValue)
                {
                    case "START":
                        if (allSeletedCount == countTrading || allSeletedCount == countLocked)
                        {
                            return false;
                        }
                        else
                        {
                            if (App.AppManager.StgEngine.StrategyEngineStatus.Equals("Running"))
                            {
                                if (countStopped > 0 || countHung > 0 || countMAXLOSS >0)
                                {
                                    return true;
                                }
                                else
                                {
                                    return false;
                                }
                            }
                            else
                            {
                                return false;
                            }
                        }
                        
                    case "STOP":
                        if (allSeletedCount == countStopped || allSeletedCount == countLocked)
                        {
                            return false;
                        }
                        else if (allSeletedCount == (countStopped + countLocked))
                        {
                            return false;
                        }
                        else 
                        {
                            return true;
                        }
                        
                    case "LOCK":
                        if (allSeletedCount == countLocked)
                        {
                            return false;
                        }
                        else
                        {
                            return true;
                        }
                        
                    case "UNLOCK":
                        int countUnlocked = StrategyOrderInfo.Where(s => s.Status == "Locked").Count();
                        if (countUnlocked <=0)
                        {
                            return false;
                        }
                        else
                        {
                            return true;
                        }
                }
            }
            return false;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
