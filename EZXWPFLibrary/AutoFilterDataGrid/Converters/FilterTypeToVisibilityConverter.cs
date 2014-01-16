using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Data;
using AutoFilterDataGrid.Model;
using System.Windows;

namespace AutoFilterDataGrid.Converters
{
    public class FilterTypeToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value != null && parameter != null && value is FilterSelectionType)
            {
                //FilterSelectionType filterType = (FilterSelectionType)value;
                bool isFilterTypeConverted = false;
                FilterSelectionType filterType = FilterSelectionType.NA;
                try
                {
                    filterType = (FilterSelectionType)value;
                    isFilterTypeConverted = true;
                }
                catch (Exception ex)
                {
                    EZXWPFLibrary.Utils.LogUtil.WriteLog(EZXWPFLibrary.Utils.LogLevel.WARN, ex.Message + "\n" + ex.StackTrace);
                    isFilterTypeConverted = false;
                }

                string param = parameter as string;
                if (param != null && isFilterTypeConverted)
                {
                    switch(param)
                    {
                        case "NUMERIC":
                            if (filterType == FilterSelectionType.NUMERIC_CUST || filterType == FilterSelectionType.NUMERIC_EQ || filterType == FilterSelectionType.NUMERIC_NE
                                || filterType == FilterSelectionType.NUMERIC_GE || filterType == FilterSelectionType.NUMERIC_GT || filterType == FilterSelectionType.NUMERIC_RNG
                                || filterType == FilterSelectionType.NUMERIC_LE || filterType == FilterSelectionType.NUMERIC_LT)
                                return Visibility.Visible;
                            else
                                return Visibility.Collapsed;
                        default:
                            return Visibility.Collapsed;
                    }
                }
            }
            return Visibility.Collapsed;;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            object o = value;
            return o;
        }
    }

}
