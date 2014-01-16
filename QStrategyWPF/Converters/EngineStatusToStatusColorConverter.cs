using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Data;
using System.Windows.Media;

namespace QStrategyWPF.Converters
{
    public partial class EngineStatusToStatusColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            Brush returnedColor = null;
            returnedColor = App.Current.Resources["SolidColorBrush_Transparent"] as SolidColorBrush;

            object o = value;

            if ((o != null) && (o is string) && parameter == null)
            {
                string status = o as string;
                if (!status.Equals("Running"))
                {
                    returnedColor = App.Current.Resources["SolidColorBrush_Running"] as SolidColorBrush;
                }
                else
                {
                    returnedColor = App.Current.Resources["SolidColorBrush_Stopped"] as SolidColorBrush;
                }
            }
            else if ((o != null) && (o is string) && parameter != null && parameter is string)
            {
                string param = parameter as string;
                string status = o as string;
                if (parameter.Equals("SUMMARYSTATUS"))
                {
                    if (status.Equals("Running"))
                    {
                        returnedColor = App.Current.Resources["SolidColorBrush_Running"] as SolidColorBrush;
                    }
                    else if (status.Equals("Stopped"))
                    {
                        returnedColor = App.Current.Resources["SolidColorBrush_Stopped"] as SolidColorBrush;
                    }
                    else if (status.Equals("Pause"))
                    {
                        returnedColor = App.Current.Resources["SolidColorBrush_Pause"] as SolidColorBrush;
                    }
                    else if (status.Equals("MaxLoss"))
                    {
                        returnedColor = App.Current.Resources["SolidColorBrush_Max_Loss"] as SolidColorBrush;
                    }
                    else if (status.Equals("Hung"))
                    {
                        returnedColor = App.Current.Resources["SolidColorBrush_Hung"] as SolidColorBrush;
                    }
                    else if (status.Equals("Locked"))
                    {
                        returnedColor = App.Current.Resources["SolidColorBrush_Locked"] as SolidColorBrush;
                    }
                }
            }
            return returnedColor;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
