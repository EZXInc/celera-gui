using AutoFilterDataGrid.Model;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace EZXWPFLibraryTest
{


    [TestClass()]
    public class ConditionTest
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
        public void ConditionConstructorTest()
        {
            string _filedName = "TestColumn1"; 
            Condition target = new Condition(_filedName, OperatorType.EQUAL);
            Assert.AreEqual(0, target.IncludedConditionList.Count);
            
            target.IncludedConditionList = null;
            Assert.AreEqual(null, target.IncludedConditionList);
            
            Assert.AreEqual(OperatorType.EQUAL, target.ConditionOperator);
            Assert.AreEqual(true, target.IsConditionOperatorSpecified);

            target.ConditionOperator = OperatorType.NA;
            Assert.AreEqual(false, target.IsConditionOperatorSpecified);

            Assert.AreEqual(false, target.IsLogicalOperatorOfIncludedConditionSpecified);
            Assert.AreEqual(false, target.IsParentConditionExist);
            Assert.AreEqual(LogicOperatorType.NA, target.LogicalOperatorOfIncludedCondition);
            Assert.AreEqual(null, target.ParentCondition);
            Assert.AreEqual(_filedName, target.FieldName);
            Assert.IsTrue(string.IsNullOrEmpty(target.FieldToMatchValue));

        }

        [TestMethod()]
        public void ConditionConstructorTest1()
        {
            string _filedName = string.Empty; 
            Condition target = new Condition(_filedName);
        }

        [TestMethod()]
        public void ConditionConstructorTest2()
        {
            Condition target = new Condition();
        }

        [TestMethod()]
        [DeploymentItem("EZXWPFLibrary.dll")]
        public void CompareValuesTest()
        {
            Condition_Accessor target = new Condition_Accessor(); 
            object ValueInRow = null; 
            string ValueInCondition = string.Empty; 
            OperatorType operatorType = new OperatorType(); 
            bool expected = false; 
            bool actual;
            actual = target.CompareValues(ValueInRow, ValueInCondition, operatorType);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod()]
        public void MatchComparissionTest()
        {
            Condition target = new Condition(); 
            object value = null; 
            Nullable<bool> status = new Nullable<bool>(); 
            Nullable<bool> expected = new Nullable<bool>(); 
            Nullable<bool> actual;
            actual = target.MatchComparission(value, status);
            Assert.AreEqual(expected, actual);
        }
    }
}
