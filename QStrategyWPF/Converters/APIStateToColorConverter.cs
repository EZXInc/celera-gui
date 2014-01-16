using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Data;
using System.Windows.Media;

namespace QStrategyWPF.Converters
{
    public class APIStateToColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            Brush returnedColor = null;
            returnedColor = App.Current.Resources["SolidColorBrush_Red"] as SolidColorBrush;

            if (App.AppManager.StgEngine.APIState != null && App.AppManager.StgEngine.APIState.ToUpper().Equals("READY"))
            {
                returnedColor = App.Current.Resources["SolidColorBrush_Green"] as SolidColorBrush;
            }
            return returnedColor;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            object o = value;
            return o;
        }
    }

}
