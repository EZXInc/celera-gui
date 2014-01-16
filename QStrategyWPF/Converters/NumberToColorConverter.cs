using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Data;
using System.Windows;
using System.Windows.Media;

namespace QStrategyWPF.Converters
{
    public class NumberToColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            Brush returnedColor = null;
            returnedColor = App.Current.Resources["SolidColorBrush_Black"] as SolidColorBrush;

            object o = value;
            if ((o != null))
            {
                if (o is double)
                {
                    double curencyValue = (double)o;
                    if (curencyValue < 0)
                    {
                        returnedColor = App.Current.Resources["SolidColorBrush_Red"] as SolidColorBrush;
                    }
                }
                else if (o is decimal)
                {
                    decimal curencyValue = (decimal)o;
                    if (curencyValue < 0)
                    {
                        returnedColor = App.Current.Resources["SolidColorBrush_Red"] as SolidColorBrush;
                    }
                }
                else if (o is int)
                {
                    int curencyValue = (int)o;
                    if (curencyValue < 0)
                    {
                        returnedColor = App.Current.Resources["SolidColorBrush_Red"] as SolidColorBrush;
                    }
                }
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
