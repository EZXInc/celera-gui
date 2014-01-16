using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using QStrategyGUILib;

namespace QStrategyTest.Mockup
{
    public class MockupManager
    {
        private static Dictionary<string, Strategy> strategyDictionary;
        public static Dictionary<string, Strategy> StrategyDictionary
        {
            get { return strategyDictionary; }
            set { strategyDictionary = value; }
        }

        private static List<StrategyOrderInfo> strategyOrderList;

        public static List<StrategyOrderInfo> StrategyOrderList
        {
            get { return strategyOrderList; }
            set { strategyOrderList = value; }
        }

        public MockupManager()
        {
            if (MockupManager.StrategyDictionary == null)
            {
                MockupManager.StrategyDictionary = new Dictionary<string, Strategy>();
                StrategyOrderList = new List<StrategyOrderInfo>();

                StrategyDictionary["ST1"] = new Strategy() { StrategyId = "ST1", StrategyName = "Q Strategy 1" };
                StrategyDictionary["ST2"] = new Strategy() { StrategyId = "ST2", StrategyName = "Q Strategy 2" };
                StrategyDictionary["ST3"] = new Strategy() { StrategyId = "ST3", StrategyName = "Q Strategy 3" };
                StrategyDictionary["ST4"] = new Strategy() { StrategyId = "ST4", StrategyName = "Q Strategy 4" };

                StrategyOrderInfo order1 = new StrategyOrderInfo(StrategyDictionary["ST1"], "BAC", "Trading", 112.12, 2.5, -300, 104.93, 2, 153879, 0.00073, 100, 250.00, 1000, 10000);
                StrategyOrderInfo order2 = new StrategyOrderInfo(StrategyDictionary["ST1"], "AMD", "Trading", -1.23, 2.5, 100, 9.02, 3, 70200, -0.00002, 25.00, 2.00, 5000, 5000);
                StrategyOrderInfo order3 = new StrategyOrderInfo(StrategyDictionary["ST1"], "MSFT", "Stopped", 0.45, 0.55, 250, 89.21, 1, 10100, 0.00000, -78.00, 45.56, 1000, 500);
                StrategyOrderInfo order4 = new StrategyOrderInfo(StrategyDictionary["ST1"], "LVLT", "Max_Loss", 9.99, 2.5, 200, 70.10, 1, 98000, 0.00010, 2.00, 120.40, 100, 8500);
                StrategyOrderInfo order5 = new StrategyOrderInfo(StrategyDictionary["ST1"], "INTC", "Trading", 3.45, 2.5, 0, 114.32, 0, 112352, 0.00003, 13.00, -2.45, 100, 24300);
                StrategyOrderInfo order6 = new StrategyOrderInfo(StrategyDictionary["ST1"], "QQQQ", "Trading", -1.23, 2.5, -200, 41.20, 2, 65850, 0.00002, 5.00, 1.99, 100, 1000);

                StrategyOrderList.Add(order1);
                StrategyOrderList.Add(order2);
                StrategyOrderList.Add(order3);
                StrategyOrderList.Add(order4);
                StrategyOrderList.Add(order5);
                StrategyOrderList.Add(order6);
            }
        }

        public static void AddNew(int count)
        {
            int oldCount = StrategyOrderList.Count;
            int newCount = oldCount + count;
            for (int x = oldCount; x < newCount; x++)
            {
                string symbol = "SYMBOL_"+ (x+1).ToString();
                StrategyOrderInfo order1 = new StrategyOrderInfo(StrategyDictionary["ST1"], symbol, "Trading", 112.12, 2.5, -300, 104.93, 2, 153879, 0.00073, 100, 250.00, 1000, 10000);
                StrategyOrderList.Add(order1);
            }
        }


        internal List<StrategyOrderInfo> GetStrategyUpdate()
        {
            RadomizeValue(StrategyOrderList);
            return StrategyOrderList;
        }

        private void RadomizeValue(List<StrategyOrderInfo> StrategyOrderList)
        {
            for (int i = 0; i < StrategyOrderList.Count; i++)
            {
                Random rnd = new Random();
                StrategyOrderList[i].Position_Shares = rnd.Next(100, 2000);
                StrategyOrderList[i].Position_Amount = (rnd.NextDouble() + 1) * 50000;
                StrategyOrderList[i].Max_Loss = rnd.Next(3000, 25000);
            }
        }

        internal List<StrategyOrderInfo> ProcessSymbol(string StrategyId, string[] symbolList, ProcessType processType)
        {
            List<StrategyOrderInfo> returnStrategyOrderList = new List<StrategyOrderInfo>();
            for (int i = 0; i < StrategyOrderList.Count; i++)
            {
                if (StrategyOrderList[i].StrategyId == StrategyId)
                {
                    StrategyOrderInfo order = StrategyOrderList[i];
                    if (symbolList.Contains(order.Symbol))
                    {
                        if (processType == ProcessType.START)
                        {
                            order.Status = "Trading";
                        }
                        else if (processType == ProcessType.STOP)
                        {
                            order.Status = "Stopped";
                        }
                        else if (processType == ProcessType.CANCELALL)
                        {
                            order.Status = "Cancel";
                        }
                        else if (processType == ProcessType.LOCK)
                        {
                            order.Status = "Locked";
                        }
                        else if (processType == ProcessType.UNLOCK)
                        {
                            order.Status = "Stoped";
                        }

                        returnStrategyOrderList.Add(order);
                    }
                }
            }
            return returnStrategyOrderList;
        }

        internal List<StrategyOrderInfo> ProcessAllSymbol(string StrategyId, ProcessType processType)
        {
            List<StrategyOrderInfo> returnStrategyOrderList = new List<StrategyOrderInfo>();
            for (int i = 0; i < StrategyOrderList.Count; i++)
            {
                if (StrategyOrderList[i].StrategyId == StrategyId)
                {
                    StrategyOrderInfo order = StrategyOrderList[i];
                    if (processType == ProcessType.START)
                    {
                        order.Status = "Trading";
                    }
                    else if (processType == ProcessType.STOP)
                    {
                        order.Status = "Stopped";
                    }
                    else if (processType == ProcessType.CANCELALL)
                    {
                        order.Status = "Cancel";
                    }
                    else if (processType == ProcessType.LOCK)
                    {
                        order.Status = "Locked";
                    }
                    else if (processType == ProcessType.UNLOCK)
                    {
                        order.Status = "Stoped";
                    }

                    returnStrategyOrderList.Add(order);
                }
            }
            return returnStrategyOrderList;
        }
    }
}
