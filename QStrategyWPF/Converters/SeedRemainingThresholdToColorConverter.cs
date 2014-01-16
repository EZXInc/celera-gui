using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Data;
using System.Windows.Media;

namespace QStrategyWPF.Converters
{
    public class SeedRemainingThresholdToColorConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            Brush returnedColor = null;
            returnedColor = App.Current.Resources["SolidColorBrush_Transparent"] as SolidColorBrush;

            if (App.AppManager.DataMgr.StrategyOrderCollectionView.Count == 0)
            {
                return returnedColor;
            }


            if (values != null && values.Count() >= 2 && values[0] is int && values[1] is int)
            {
                int seedRemaining = (int)values[0];
                int seedRemainingThreshold = (int)values[1];

                if (seedRemaining < seedRemainingThreshold)
                {
                    returnedColor = App.Current.Resources["SolidColorBrush_Red"] as SolidColorBrush;
                }
                else
                {
                    //double TenPercentOfseedRemainingThreshold = seedRemainingThreshold * 0.10;
                    //double TenPercentOfseedRemainingThreshold = seedRemaining * 0.10;
                    double TenPercentOfseedRemainingThreshold = 1000;
                    double TenPercentAboveThreshold = TenPercentOfseedRemainingThreshold + seedRemainingThreshold;
                    if (seedRemaining <= TenPercentAboveThreshold)
                    {
                        returnedColor = App.Current.Resources["SolidColorBrush_Yellow"] as SolidColorBrush;
                    }
                }
            }


            return returnedColor;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
