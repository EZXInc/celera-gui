using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Data;

namespace QStrategyWPF.Converters
{
    public class AutoUpdateDataToContentConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {

            string returnedTxt = "Auto-update on";

            if (App.AppManager.AutoUpdateData)
            {
                returnedTxt = "Auto-update off";
            }
            return returnedTxt;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            object o = value;
            return o;
        }
    }
}
