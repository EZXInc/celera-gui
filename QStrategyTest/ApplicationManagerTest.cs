using QStrategyWPF;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using QStrategyWPF.Model;
using QStrategyGUILib;
using QStrategyTest.Mockup;
using System.Windows.Threading;

namespace QStrategyTest
{


    [TestClass()]
    public class ApplicationManagerTest
    {


        private TestContext testContextInstance;

        public TestContext TestContext
        {
            get
            {
                return testContextInstance;
            }
            set
            {
                testContextInstance = value;
            }
        }

        static ApplicationManager_Accessor target;


        #region Additional test attributes

        
        [ClassInitialize()]
        public static void MyClassInitialize(TestContext testContext)
        {
            ICommunicationManager mockupComMgr = new MockupCommunicationManager();
            target = new ApplicationManager_Accessor(mockupComMgr);
        }
        
        //Use ClassCleanup to run code after all tests in a class have run
        //[ClassCleanup()]
        //public static void MyClassCleanup()
        //{
        //}
        //
        //Use TestInitialize to run code before running each test
        //[TestInitialize()]
        //public void MyTestInitialize()
        //{
        //}
        //
        //Use TestCleanup to run code after each test has run
        //[TestCleanup()]
        //public void MyTestCleanup()
        //{
        //}
        //
        #endregion


        [TestMethod()]
        public void ApplicationManagerConstructorTest()
        {
            Assert.IsNotNull(target.StgEngine);
            Assert.IsNotNull(target.DataMgr);
            Assert.IsNotNull(target.StgEngine.ComMgr);
            Assert.IsNotNull(target.StgEngine.ComMgr.StrategyEngine);
        }

        [TestMethod()]
        [DeploymentItem("QStrategyWPF.exe")]
        public void InitTest()
        {
            App app = new App();
            App.Current.Resources.Add("AppManager", new ApplicationManager(target.StgEngine.ComMgr));
            target.Init();
            Assert.IsTrue(target.AutoUpdateData);
            Assert.AreEqual(MockupManager.StrategyOrderList.Count, target.DataMgr.StrategyOrderCollectionView.Count);
        }

        
        [TestMethod()]
        public void AutoRefreshDataTimer_TickTest()
        {

            App app = new App();
            App.Current.Resources.Add("AppManager", new ApplicationManager(target.StgEngine.ComMgr));
            target.Init();
            object sender = null; // TODO: Initialize to an appropriate value
            EventArgs e = null; // TODO: Initialize to an appropriate value

            target.ConnectionStatus_Task.Bw.Dispose();
            target.GetStrategySymbolUpdate_Task.Bw.Dispose();
            target.SendStrategySymbolProcess_Task.Bw.Dispose();

            target.AutoUpdateData = false;
            MockupManager.AddNew(4);
            target.AutoRefreshDataTimer_Tick(sender, e);
            Assert.AreNotEqual(MockupManager.StrategyOrderList.Count, target.DataMgr.StrategyOrderCollectionView.Count);

            target.AutoUpdateData = true;
            target.AutoRefreshDataTimer_Tick(sender, e);

            target.GetStrategySymbolUpdate();

            Assert.AreEqual(MockupManager.StrategyOrderList.Count, target.DataMgr.StrategyOrderCollectionView.Count);        

        }

        [TestMethod()]
        public void CheckConnectionStatusTest()
        {
            target.CheckConnectionStatus();

            Assert.IsTrue(target.StgEngine.IsConnected);
        }


        [TestMethod()]
        public void GetStrategySymbolUpdateTest()
        {
            App app = new App();
            App.Current.Resources.Add("AppManager", new ApplicationManager(target.StgEngine.ComMgr));
            target.Init();

            target.ConnectionStatus_Task.Bw.Dispose();
            target.GetStrategySymbolUpdate_Task.Bw.Dispose();
            target.SendStrategySymbolProcess_Task.Bw.Dispose();

            int expected = MockupManager.StrategyOrderList.Count;
            int symbolToAdd = 4;
            MockupManager.AddNew(symbolToAdd);
            expected = expected + symbolToAdd;

            Assert.AreNotEqual(expected, target.DataMgr.StrategyOrderCollectionView.Count);

            target.GetStrategySymbolUpdate();

            Assert.AreEqual(expected, target.DataMgr.StrategyOrderCollectionView.Count);
        }


        [TestMethod()]
        public void BaseConfigurationDirectoryTest()
        {
            ApplicationManager target = new ApplicationManager(); // TODO: Initialize to an appropriate value
            string expected = "\\AppData\\Roaming\\RhinebeckQStrategy\\Configuration";
            string actual;
            actual = target.BaseConfigurationDirectory;
            Assert.IsTrue(actual.EndsWith(expected));
        }


        [TestMethod()]
        public void CancelTest()
        {
            if (App.Current == null)
            {
                App app = new App();
            }

            App.Current.Dispatcher.Thread.SetApartmentState(System.Threading.ApartmentState.STA);

            App.Current.Resources.Remove("AppManager");
            App.Current.Resources.Add("AppManager", new ApplicationManager(target.StgEngine.ComMgr));
            target.Init();

            string expected = "Hung";

            target.ConnectionStatus_Task.Bw.Dispose();
            target.GetStrategySymbolUpdate_Task.Bw.Dispose();
            target.SendStrategySymbolProcess_Task.Bw.Dispose();

            MockupManager.StrategyOrderList[0].Status = string.Empty;
            MockupManager.StrategyOrderList[1].Status = string.Empty;
            MockupManager.StrategyOrderList[2].Status = string.Empty;

            string strategyId = MockupManager.StrategyOrderList[0].StrategyId;

            Dictionary<string, List<string>> strategySymbollist = new Dictionary<string, List<string>>();
            List<string> symbolList = new List<string>();

            symbolList.Add(MockupManager.StrategyOrderList[0].Symbol);
            symbolList.Add(MockupManager.StrategyOrderList[1].Symbol);
            symbolList.Add(MockupManager.StrategyOrderList[2].Symbol);

            strategySymbollist[strategyId] = symbolList;

            target.Cancel(strategySymbollist, false);
            target.GetStrategySymbolUpdate();

            StrategyOrderInfo order1 = target.DataMgr.StrategyOrderCollectionView.GetItemAt(0) as StrategyOrderInfo;
            Assert.AreEqual(expected, order1.Status);

            StrategyOrderInfo order2 = target.DataMgr.StrategyOrderCollectionView.GetItemAt(1) as StrategyOrderInfo;
            Assert.AreEqual(expected, order2.Status);


            StrategyOrderInfo order3 = target.DataMgr.StrategyOrderCollectionView.GetItemAt(2) as StrategyOrderInfo;
            Assert.AreEqual(expected, order3.Status);
        }

        [TestMethod()]
        public void LockTest()
        {
            App app = new App();
            App.Current.Resources.Add("AppManager", new ApplicationManager(target.StgEngine.ComMgr));
            target.Init();

            string expected = "Locked";

            target.ConnectionStatus_Task.Bw.Dispose();
            target.GetStrategySymbolUpdate_Task.Bw.Dispose();
            target.SendStrategySymbolProcess_Task.Bw.Dispose();

            MockupManager.StrategyOrderList[0].Status = string.Empty;
            MockupManager.StrategyOrderList[1].Status = string.Empty;
            MockupManager.StrategyOrderList[2].Status = string.Empty;

            string strategyId = MockupManager.StrategyOrderList[0].StrategyId;

            Dictionary<string, List<string>> strategySymbollist = new Dictionary<string, List<string>>();
            List<string> symbolList = new List<string>();

            symbolList.Add(MockupManager.StrategyOrderList[0].Symbol);
            symbolList.Add(MockupManager.StrategyOrderList[1].Symbol);
            symbolList.Add(MockupManager.StrategyOrderList[2].Symbol);

            strategySymbollist[strategyId] = symbolList;

            target.Lock(strategySymbollist, false);
            target.GetStrategySymbolUpdate();
            StrategyOrderInfo order1 = target.DataMgr.StrategyOrderCollectionView.GetItemAt(0) as StrategyOrderInfo;
            Assert.AreEqual(expected, order1.Status);

            StrategyOrderInfo order2 = target.DataMgr.StrategyOrderCollectionView.GetItemAt(1) as StrategyOrderInfo;
            Assert.AreEqual(expected, order2.Status);


            StrategyOrderInfo order3 = target.DataMgr.StrategyOrderCollectionView.GetItemAt(2) as StrategyOrderInfo;
            Assert.AreEqual(expected, order3.Status);
        }

        [TestMethod()]
        public void StartTest()
        {
            App app = new App();
            App.Current.Resources.Add("AppManager", new ApplicationManager(target.StgEngine.ComMgr));
            target.Init();

            string expected = "Trading";

            target.ConnectionStatus_Task.Bw.Dispose();
            target.GetStrategySymbolUpdate_Task.Bw.Dispose();
            target.SendStrategySymbolProcess_Task.Bw.Dispose();

            MockupManager.StrategyOrderList[0].Status = string.Empty;
            MockupManager.StrategyOrderList[1].Status = string.Empty;
            MockupManager.StrategyOrderList[2].Status = string.Empty;

            string strategyId = MockupManager.StrategyOrderList[0].StrategyId; 

            Dictionary<string, List<string>> strategySymbollist = new Dictionary<string, List<string>>();
            List<string> symbolList = new List<string>();

            symbolList.Add(MockupManager.StrategyOrderList[0].Symbol);
            symbolList.Add(MockupManager.StrategyOrderList[1].Symbol);
            symbolList.Add(MockupManager.StrategyOrderList[2].Symbol);

            strategySymbollist[strategyId] = symbolList;

            target.Start(strategySymbollist, false);
            target.GetStrategySymbolUpdate();

            StrategyOrderInfo order1 = target.DataMgr.StrategyOrderCollectionView.GetItemAt(0) as StrategyOrderInfo;
            Assert.AreEqual(expected, order1.Status);

            StrategyOrderInfo order2 = target.DataMgr.StrategyOrderCollectionView.GetItemAt(1) as StrategyOrderInfo;
            Assert.AreEqual(expected, order2.Status);


            StrategyOrderInfo order3 = target.DataMgr.StrategyOrderCollectionView.GetItemAt(2) as StrategyOrderInfo;
            Assert.AreEqual(expected, order3.Status);
        }

        [TestMethod()]
        public void StopTest()
        {
            if (App.Current == null)
            {
                App app = new App();
            }

            App.Current.Dispatcher.Thread.Join();

            App.Current.Resources.Remove("AppManager");
            App.Current.Resources.Add("AppManager", new ApplicationManager(target.StgEngine.ComMgr));
            target.Init();

            string expected = "Stopped";

            target.ConnectionStatus_Task.Bw.Dispose();
            target.GetStrategySymbolUpdate_Task.Bw.Dispose();
            target.SendStrategySymbolProcess_Task.Bw.Dispose();

            MockupManager.StrategyOrderList[0].Status = string.Empty;
            MockupManager.StrategyOrderList[1].Status = string.Empty;
            MockupManager.StrategyOrderList[2].Status = string.Empty;

            string strategyId = MockupManager.StrategyOrderList[0].StrategyId;

            Dictionary<string, List<string>> strategySymbollist = new Dictionary<string, List<string>>();
            List<string> symbolList = new List<string>();

            symbolList.Add(MockupManager.StrategyOrderList[0].Symbol);
            symbolList.Add(MockupManager.StrategyOrderList[1].Symbol);
            symbolList.Add(MockupManager.StrategyOrderList[2].Symbol);

            strategySymbollist[strategyId] = symbolList;

            target.Stop(strategySymbollist, false);
            target.GetStrategySymbolUpdate();

            StrategyOrderInfo order1 = target.DataMgr.StrategyOrderCollectionView.GetItemAt(0) as StrategyOrderInfo;
            Assert.AreEqual(expected, order1.Status);

            StrategyOrderInfo order2 = target.DataMgr.StrategyOrderCollectionView.GetItemAt(1) as StrategyOrderInfo;
            Assert.AreEqual(expected, order2.Status);


            StrategyOrderInfo order3 = target.DataMgr.StrategyOrderCollectionView.GetItemAt(2) as StrategyOrderInfo;
            Assert.AreEqual(expected, order3.Status);
        }

        [TestMethod()]
        public void UnlockTest()
        {
            App app = new App();
            App.Current.Resources.Add("AppManager", new ApplicationManager(target.StgEngine.ComMgr));
            target.Init();

            string expected = "Stopped";

            target.ConnectionStatus_Task.Bw.Dispose();
            target.GetStrategySymbolUpdate_Task.Bw.Dispose();
            target.SendStrategySymbolProcess_Task.Bw.Dispose();

            MockupManager.StrategyOrderList[0].Status = string.Empty;
            MockupManager.StrategyOrderList[1].Status = string.Empty;
            MockupManager.StrategyOrderList[2].Status = string.Empty;

            string strategyId = MockupManager.StrategyOrderList[0].StrategyId;

            Dictionary<string, List<string>> strategySymbollist = new Dictionary<string, List<string>>();
            List<string> symbolList = new List<string>();

            symbolList.Add(MockupManager.StrategyOrderList[0].Symbol);
            symbolList.Add(MockupManager.StrategyOrderList[1].Symbol);
            symbolList.Add(MockupManager.StrategyOrderList[2].Symbol);

            strategySymbollist[strategyId] = symbolList;

            target.Unlock(strategySymbollist, false);
            target.GetStrategySymbolUpdate();

            StrategyOrderInfo order1 = target.DataMgr.StrategyOrderCollectionView.GetItemAt(0) as StrategyOrderInfo;
            Assert.AreEqual(expected, order1.Status);

            StrategyOrderInfo order2 = target.DataMgr.StrategyOrderCollectionView.GetItemAt(1) as StrategyOrderInfo;
            Assert.AreEqual(expected, order2.Status);


            StrategyOrderInfo order3 = target.DataMgr.StrategyOrderCollectionView.GetItemAt(2) as StrategyOrderInfo;
            Assert.AreEqual(expected, order3.Status);
        }

        [TestMethod()]
        public void UnwindTest()
        {
            App app = new App();
            App.Current.Resources.Add("AppManager", new ApplicationManager(target.StgEngine.ComMgr));
            target.Init();

            string expected = "MaxLoss";

            target.ConnectionStatus_Task.Bw.Dispose();
            target.GetStrategySymbolUpdate_Task.Bw.Dispose();
            target.SendStrategySymbolProcess_Task.Bw.Dispose();

            MockupManager.StrategyOrderList[0].Status = string.Empty;
            MockupManager.StrategyOrderList[1].Status = string.Empty;
            MockupManager.StrategyOrderList[2].Status = string.Empty;

            string strategyId = MockupManager.StrategyOrderList[0].StrategyId;

            Dictionary<string, List<string>> strategySymbollist = new Dictionary<string, List<string>>();
            List<string> symbolList = new List<string>();

            symbolList.Add(MockupManager.StrategyOrderList[0].Symbol);
            symbolList.Add(MockupManager.StrategyOrderList[1].Symbol);
            symbolList.Add(MockupManager.StrategyOrderList[2].Symbol);

            strategySymbollist[strategyId] = symbolList;

            target.Unwind(strategySymbollist, false);
            target.GetStrategySymbolUpdate();
            StrategyOrderInfo order1 = target.DataMgr.StrategyOrderCollectionView.GetItemAt(0) as StrategyOrderInfo;
            Assert.AreEqual(expected, order1.Status);

            StrategyOrderInfo order2 = target.DataMgr.StrategyOrderCollectionView.GetItemAt(1) as StrategyOrderInfo;
            Assert.AreEqual(expected, order2.Status);


            StrategyOrderInfo order3 = target.DataMgr.StrategyOrderCollectionView.GetItemAt(2) as StrategyOrderInfo;
            Assert.AreEqual(expected, order3.Status);
        }


        [TestMethod()]
        public void ConnectionModeTest()
        {
            ApplicationConnectionMode expected = new ApplicationConnectionMode();
            ApplicationConnectionMode actual;
            actual = target.ConnectionMode;
            Assert.AreEqual(expected, actual);
        }

        [TestMethod()]
        public void DataMgrTest()
        {
            Assert.IsNotNull(target.DataMgr.StrategyOrderDictionary);
        }

        [TestMethod()]
        public void StgEngineTest()
        {
            Assert.IsNotNull(target.StgEngine.ComMgr);
        }
    }
}
