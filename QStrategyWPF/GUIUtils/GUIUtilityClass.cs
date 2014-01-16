using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EZXWPFLibrary.Utils;

namespace QStrategyWPF.GUIUtils
{
    public class GUIUtilityClass
    {
        public static double GetColumnMinWidth(string propertyName)
        {
            double minWidth = 50.0;
            
            if (propertyName != null && propertyName.Length > 5)
            {
                int headerLength = propertyName.Length;

                if (headerLength > 20)
                {
                    return -1.0;
                }
                else if (headerLength > 14)
                {
                    return 125.0;
                }
                else if (headerLength > 13)
                {
                    return 120.0;
                }
                else if (headerLength > 12)
                {
                    return 115.0;
                }
                else if (headerLength > 11)
                {
                    return 110.0;
                }
                else if (headerLength > 10)
                {
                    return 105.0;
                }
                else if (headerLength > 9)
                {
                    return 100.0;
                }
                else if (headerLength > 8)
                {
                    return 95.0;
                }
                else if (headerLength > 7)
                {
                    return 90.0;
                }
                else if (headerLength > 6)
                {
                    return 85.0;
                }
                else if (headerLength > 5)
                {
                    return 80;
                }
            }

            return minWidth;
        }

        public static string GetTextFormRersources(string propertyName, bool isCamelcase)
        {

            if (propertyName.Equals("PnL")
                || propertyName.Equals("UR_PnL")
                || propertyName.Equals("PositionShares")
                || propertyName.Equals("PositionAmount")
                || propertyName.Equals("OpenOrders")
                || propertyName.Equals("PnLPerShares")
                || propertyName.Equals("TradingRevenue")
                || propertyName.Equals("RebateRevenue")
                || propertyName.Equals("MaxLoss"))
            {
                switch (propertyName)
                {
                    case "PnL": return "PnL $";
                    case "UR_PnL": return "UR_PnL $";
                    case "PositionShares": return "Position (Shrs)";
                    case "PositionAmount": return "Position $";
                    case "OpenOrders": return "# Open Orders";
                    case "PnLPerShares": return "PnL/Share";
                    case "TradingRevenue": return "Trading Rev. $";
                    case "RebateRevenue": return "Rebate Rev. $";
                    case "MaxLoss": return "MAX Loss $";
                }
            }
            else if (propertyName.Equals("Position_Shares")
                || propertyName.Equals("Position_Amount")
                || propertyName.Equals("Number_Of_Open_Orders")
                || propertyName.Equals("PnL_Per_Share")
                || propertyName.Equals("Trading_Revenue")
                || propertyName.Equals("Rebate_Revenue")
                || propertyName.Equals("Max_Loss"))
            {
                switch (propertyName)
                {
                    case "Position_Shares": return "Position (Shrs)";
                    case "Position_Amount": return "Position $";
                    case "Number_Of_Open_Orders": return "# Open Orders";
                    case "PnL_Per_Share": return "PnL/Share";
                    case "Trading_Revenue": return "Trading Rev. $";
                    case "Rebate_Revenue": return "Rebate Rev. $";
                    case "Max_Loss": return "MAX Loss $";
                }
            }
            if (isCamelcase)
            {
                string convertedText = StringUtils.CamelCaseToLabel(propertyName);
                return convertedText;
            }
            return string.Empty;
        }

    }
}
