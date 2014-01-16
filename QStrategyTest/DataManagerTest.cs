using QStrategyWPF;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using QStrategyGUILib;
using System.Collections.Generic;
using EZXWPFLibrary.Helpers;
using QStrategyWPF.Model;
using System.Collections.ObjectModel;
using System.Windows.Data;
using System.Collections.Concurrent;

namespace QStrategyTest
{


    [TestClass()]
    public class DataManagerTest
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

        #region Additional test attributes
        // 
        //You can use the following additional attributes as you write your tests:
        //
        //Use ClassInitialize to run code before running the first test in the class
        //[ClassInitialize()]
        //public static void MyClassInitialize(TestContext testContext)
        //{
        //}
        //
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
        public void DataManagerConstructorTest()
        {
            DataManager target = new DataManager();
        }

        [TestMethod()]
        [DeploymentItem("QStrategyWPF.exe")]
        public void ModifyListCollectionViewTest()
        {
            DataManager_Accessor target = new DataManager_Accessor(); // TODO: Initialize to an appropriate value
            string strategyIdKey = string.Empty; // TODO: Initialize to an appropriate value
            string symbolKey = string.Empty; // TODO: Initialize to an appropriate value
            string operation = string.Empty; // TODO: Initialize to an appropriate value
            target.ModifyListCollectionView(strategyIdKey, symbolKey, operation);
        }

        [TestMethod()]
        [DeploymentItem("QStrategyWPF.exe")]
        public void ModifySummaryListCollectionViewTest()
        {
            DataManager_Accessor target = new DataManager_Accessor(); // TODO: Initialize to an appropriate value
            string key = string.Empty; // TODO: Initialize to an appropriate value
            string operation = string.Empty; // TODO: Initialize to an appropriate value
            target.ModifySummaryListCollectionView(key, operation);
        }

        [TestMethod()]
        [DeploymentItem("QStrategyWPF.exe")]
        public void UpdateStrategyDataTest()
        {
            DataManager_Accessor target = new DataManager_Accessor(); // TODO: Initialize to an appropriate value
            List<StrategyOrderInfo> updateOrderList = null; // TODO: Initialize to an appropriate value
            bool IsSendProcessReponse = false; // TODO: Initialize to an appropriate value
            target.UpdateStrategyData(updateOrderList, IsSendProcessReponse);
        }

        [TestMethod()]
        public void AggregateStrategyOrderInfoTest()
        {
            DataManager target = new DataManager(); // TODO: Initialize to an appropriate value
            MTObservableCollection<StrategyOrderInfo> expected = null; // TODO: Initialize to an appropriate value
            MTObservableCollection<StrategyOrderInfo> actual;
            target.AggregateStrategyOrderInfo = expected;
            actual = target.AggregateStrategyOrderInfo;
            Assert.AreEqual(expected, actual);
        }

        [TestMethod()]
        public void AggregateSummaryInfoTest()
        {
            DataManager target = new DataManager(); // TODO: Initialize to an appropriate value
            MTObservableCollection<SummaryOrder> expected = null; // TODO: Initialize to an appropriate value
            MTObservableCollection<SummaryOrder> actual;
            target.AggregateSummaryInfo = expected;
            actual = target.AggregateSummaryInfo;
            Assert.AreEqual(expected, actual);
        }

        [TestMethod()]
        public void StrategyListTest()
        {
            DataManager target = new DataManager(); // TODO: Initialize to an appropriate value
            ObservableCollection<Strategy> expected = null; // TODO: Initialize to an appropriate value
            ObservableCollection<Strategy> actual;
            target.StrategyList = expected;
            actual = target.StrategyList;
            Assert.AreEqual(expected, actual);
        }

        [TestMethod()]
        public void StrategyOrderCollectionViewTest()
        {
            DataManager target = new DataManager(); // TODO: Initialize to an appropriate value
            ListCollectionView expected = null; // TODO: Initialize to an appropriate value
            ListCollectionView actual;
            target.StrategyOrderCollectionView = expected;
            actual = target.StrategyOrderCollectionView;
            Assert.AreEqual(expected, actual);
        }

        [TestMethod()]
        public void StrategyOrderDictionaryTest()
        {
            DataManager target = new DataManager(); // TODO: Initialize to an appropriate value
            ConcurrentDictionary<string, ConcurrentDictionary<string, StrategyOrderInfo>> expected = null; // TODO: Initialize to an appropriate value
            ConcurrentDictionary<string, ConcurrentDictionary<string, StrategyOrderInfo>> actual;
            target.StrategyOrderDictionary = expected;
            actual = target.StrategyOrderDictionary;
            Assert.AreEqual(expected, actual);
        }

        [TestMethod()]
        public void SummaryOrderCollectionViewTest()
        {
            DataManager target = new DataManager(); // TODO: Initialize to an appropriate value
            ListCollectionView expected = null; // TODO: Initialize to an appropriate value
            ListCollectionView actual;
            target.SummaryOrderCollectionView = expected;
            actual = target.SummaryOrderCollectionView;
            Assert.AreEqual(expected, actual);
        }

        [TestMethod()]
        public void SummaryOrderDictionaryTest()
        {
            DataManager target = new DataManager(); // TODO: Initialize to an appropriate value
            ConcurrentDictionary<string, SummaryOrder> expected = null; // TODO: Initialize to an appropriate value
            ConcurrentDictionary<string, SummaryOrder> actual;
            target.SummaryOrderDictionary = expected;
            actual = target.SummaryOrderDictionary;
            Assert.AreEqual(expected, actual);
        }
    }
}
