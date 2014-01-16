using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EZXWPFLibrary.Helpers;

namespace QStrategyGUILib
{
    public interface ICommunicationManager
    {
        StrategyEngine StrategyEngine{set; get;}

        void Init();

        bool CheckConnection();

        List<StrategyOrderInfo> GetStrategyUpdate();

        List<StrategyOrderInfo> ProcessSymbol(string StrategyId, string[] symbol, ProcessType _processType);

        List<StrategyOrderInfo> ProcessAllSymbol(string StrategyId, ProcessType _processType);


    }
}
