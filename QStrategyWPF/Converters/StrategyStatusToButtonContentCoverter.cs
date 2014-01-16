using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Data;
using System.Windows;
using System.Windows.Media;

namespace QStrategyWPF.Converters
{
    public class StrategyStatusToButtonContentCoverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            string returnedContent = "START";
            if (parameter != null && parameter is string && parameter.ToString().Equals("LOCK"))
            {
                returnedContent = "Lock";
                object o = value;
                if ((o != null) && (o is string))
                {
                    string status = o as string;
                    if (!status.Equals("Lock"))
                    {
                        returnedContent = "Lock";
                    }
                    else
                    {
                        returnedContent = "Unlock";
                    }
                }

            }
            else
            {
                object o = value;
                if ((o != null) && (o is string))
                {
                    string status = o as string;
                    if (!status.Equals("Running"))
                    {
                        returnedContent = "START";
                    }
                    else
                    {
                        returnedContent = "STOP";
                    }
                }
            }
            return returnedContent;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            object o = value;
            return o;
        }
    }
}
