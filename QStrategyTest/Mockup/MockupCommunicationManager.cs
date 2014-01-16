using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using QStrategyGUILib;

namespace QStrategyTest.Mockup
{
    public partial class MockupCommunicationManager : ICommunicationManager
    {
        private MockupManager manager = new MockupManager();

        private StrategyEngine strategyEngine;
        public StrategyEngine StrategyEngine
        {
            get { return strategyEngine; }
            set
            {
                strategyEngine = value;
            }
        }

        public MockupCommunicationManager()
        {
        }

        public MockupCommunicationManager(StrategyEngine engine)
        {
            this.StrategyEngine = engine;
        }

        public void Init()
        {
            throw new NotImplementedException();
        }

        public bool CheckConnection()
        {
            return true;
        }

        public List<StrategyOrderInfo> GetStrategyUpdate()
        {
            return MockupManager.StrategyOrderList;
        }

        public List<StrategyOrderInfo> ProcessSymbol(string StrategyId, string[] symbol, ProcessType _processType)
        {
            if (symbol == null || symbol.Count() == 0)
            {
                for (int i = 0; i < MockupManager.StrategyOrderList.Count; i++)
                {
                    StrategyOrderInfo order = MockupManager.StrategyOrderList[i];
                    ProcessOrder(_processType, order);
                }
            }
            else if (symbol != null && symbol.Count() > 0)
            {
                for (int i = 0; i < MockupManager.StrategyOrderList.Count; i++)
                {
                    StrategyOrderInfo order = MockupManager.StrategyOrderList[i];
                    if (symbol.Contains(order.Symbol))
                    {
                        ProcessOrder(_processType, order);
                    }
                }            
            }
            
            return MockupManager.StrategyOrderList;
        }

        private static void ProcessOrder(ProcessType _processType, StrategyOrderInfo order)
        {
            switch (_processType)
            {
                case ProcessType.START: order.Status = "Trading"; break;
                case ProcessType.STOP: order.Status = "Stopped"; break;
                case ProcessType.LOCK: order.Status = "Locked"; break;
                case ProcessType.UNLOCK: order.Status = "Stopped"; break;
                case ProcessType.CANCELALL: order.Status = "Hung"; break;
                case ProcessType.UNWIND: order.Status = "MaxLoss"; break;
                default: break;
            }
        }

        public List<StrategyOrderInfo> ProcessAllSymbol(string StrategyId, ProcessType _processType)
        {
            return MockupManager.StrategyOrderList;
        }
    }
}
