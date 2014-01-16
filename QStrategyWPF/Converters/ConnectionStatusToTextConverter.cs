using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Data;

namespace QStrategyWPF.Converters
{
    public class ConnectionStatusToTextConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            string returnedText = "Disconnected";
            if (App.AppManager.StgEngine.IsConnected)
            {
                returnedText = "Connected";
            }
            return returnedText;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            object o = value;
            return o;
        }
    }
}
