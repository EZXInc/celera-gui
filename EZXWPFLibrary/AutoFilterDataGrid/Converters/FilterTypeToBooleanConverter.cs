using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Data;
using AutoFilterDataGrid.Model;

namespace AutoFilterDataGrid.Converters
{
    public class FilterTypeToBooleanConverter : IValueConverter
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
                        case "EQ":
                            if (filterType == FilterSelectionType.NUMERIC_EQ)
                                return true;
                            else
                                return false;
                        case "NE":
                            if (filterType == FilterSelectionType.NUMERIC_NE)
                                return true;
                            else
                                return false;
                        case "GT":
                            if (filterType == FilterSelectionType.NUMERIC_GT)
                                return true;
                            else
                                return false;
                        case "GE":
                            if (filterType == FilterSelectionType.NUMERIC_GE)
                                return true;
                            else
                                return false;
                        case "LT":
                            if (filterType == FilterSelectionType.NUMERIC_LT)
                                return true;
                            else
                                return false;
                        case "LE":
                            if (filterType == FilterSelectionType.NUMERIC_LE)
                                return true;
                            else
                                return false;
                        case "RNG":
                            if (filterType == FilterSelectionType.NUMERIC_RNG)
                                return true;
                            else
                                return false;
                        case "CUST":
                            if (filterType == FilterSelectionType.NUMERIC_CUST)
                                return true;
                            else
                                return false;
                        case "NUMERIC":
                            if (filterType == FilterSelectionType.NUMERIC_CUST || filterType == FilterSelectionType.NUMERIC_EQ || filterType == FilterSelectionType.NUMERIC_NE
                                || filterType == FilterSelectionType.NUMERIC_GE || filterType == FilterSelectionType.NUMERIC_GT || filterType == FilterSelectionType.NUMERIC_RNG
                                || filterType == FilterSelectionType.NUMERIC_LE || filterType == FilterSelectionType.NUMERIC_LT)
                                return true;
                            else
                                return false;
                        default:
                            return false;
                    }
                }
            }
            return false;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            object o = value;
            return o;
        }
    }

}
