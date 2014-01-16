using AutoFilterDataGrid.Model;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace EZXWPFLibraryTest
{


    [TestClass()]
    public class FilterColumnTest
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
        public void FilterColumnConstructorTest()
        {
            FilterColumn target = new FilterColumn();
            target.ColumnName = "TestColumn1";
            target.ColumnSelectedDataList = new System.Collections.Generic.List<string>();
            target.ColumnSelectedDataList.Add("1");
            target.ColumnSelectedDataList.Add("2");
            target.ColumnSelectedDataList.Add("3");
            target.ColumnSelectedDataList.Add("4");
            target.ConditionList = new System.Collections.Generic.List<Condition>();
            target.CurrentRowDataValue = 15;
            target.Filter = null;
            target.FilterType = FilterSelectionType.NUMERIC_EQ;
            target.Id = 25;

            Assert.AreEqual("TestColumn1", target.ColumnName);
            Assert.AreEqual(4, target.ColumnSelectedDataList.Count);
            Assert.AreEqual(0, target.ConditionList.Count);
            Assert.AreEqual(15, target.CurrentRowDataValue);
            Assert.AreEqual(null, target.Filter);
            Assert.AreEqual(FilterSelectionType.NUMERIC_EQ, target.FilterType);
            Assert.AreEqual(25, target.Id);
            Assert.AreEqual(false, target.IsConditionListExist);

            target.ConditionList.Add(new Condition("TestColumn1", OperatorType.EQUAL));
            Assert.AreEqual(true, target.IsConditionListExist);

            target.ConditionList = null;
            Assert.AreEqual(false, target.IsConditionListExist);
        }
    }
}
