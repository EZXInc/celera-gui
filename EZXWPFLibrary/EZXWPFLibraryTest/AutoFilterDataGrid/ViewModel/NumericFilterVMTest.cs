using AutoFilterDataGrid.ViewModel;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using AutoFilterDataGrid.Model;

namespace EZXWPFLibraryTest
{


    [TestClass()]
    public class NumericFilterVMTest
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
        public void NumericFilterVMConstructorTest()
        {
            string columnName = "TestColumn1"; 
            NumericFilterSelectionType _filterType = new NumericFilterSelectionType();
            _filterType = NumericFilterSelectionType.EQUAL;
            NumericFilterVM target = new NumericFilterVM(columnName, _filterType);
            Assert.AreNotEqual(columnName, target.FieldName);
            Assert.AreNotEqual(_filterType, target.NumericFilterType);

            Assert.IsTrue(string.IsNullOrEmpty(target.FieldName));
            Assert.AreEqual(NumericFilterSelectionType.CUSTOM, target.NumericFilterType);
        }

        [TestMethod()]
        public void NumericFilterVMConstructorTest1()
        {
            NumericFilterVM target = new NumericFilterVM();
            Assert.IsTrue(string.IsNullOrEmpty(target.FieldName));
            Assert.AreEqual(NumericFilterSelectionType.CUSTOM, target.NumericFilterType);

        }

        [TestMethod()]
        public void InitTest_EQ()
        {
            NumericFilterVM target = new NumericFilterVM(); 
            string columnName = "TestColumn1"; 
            string columnTitle = "Test Column 1"; 
            NumericFilterSelectionType _filterType = new NumericFilterSelectionType();
            _filterType = NumericFilterSelectionType.EQUAL;
            FilterColumn _column = new FilterColumn();
            _column.ColumnName = columnName;
            _column.ColumnSelectedDataList = new System.Collections.Generic.List<string>();
            _column.ConditionList = new System.Collections.Generic.List<Condition>();
            _column.CurrentRowDataValue = null;

            target.Init(columnName, columnTitle, _filterType, _column);

            Assert.AreEqual(columnName, target.FieldName);
            Assert.AreEqual(columnTitle, target.FieldNameTitle);
            Assert.AreEqual(OperatorType.EQUAL, target.FirstCondition.ConditionOperator);
        }

        [TestMethod()]
        public void InitTest_NE()
        {
            NumericFilterVM target = new NumericFilterVM();
            string columnName = "TestColumn1";
            string columnTitle = "Test Column 1";
            NumericFilterSelectionType _filterType = new NumericFilterSelectionType();
            _filterType = NumericFilterSelectionType.NOT_EQUAL;
            FilterColumn _column = new FilterColumn();
            _column.ColumnName = columnName;
            _column.ColumnSelectedDataList = new System.Collections.Generic.List<string>();
            _column.ConditionList = new System.Collections.Generic.List<Condition>();
            _column.CurrentRowDataValue = null;

            target.Init(columnName, columnTitle, _filterType, _column);

            Assert.AreEqual(columnName, target.FieldName);
            Assert.AreEqual(columnTitle, target.FieldNameTitle);
            Assert.AreEqual(OperatorType.NOT_EQUAL, target.FirstCondition.ConditionOperator);
        }

        [TestMethod()]
        public void InitTest_GT()
        {
            NumericFilterVM target = new NumericFilterVM();
            string columnName = "TestColumn1";
            string columnTitle = "Test Column 1";
            NumericFilterSelectionType _filterType = new NumericFilterSelectionType();
            _filterType = NumericFilterSelectionType.GREATER_THAN;
            FilterColumn _column = new FilterColumn();
            _column.ColumnName = columnName;
            _column.ColumnSelectedDataList = new System.Collections.Generic.List<string>();
            _column.ConditionList = new System.Collections.Generic.List<Condition>();
            _column.CurrentRowDataValue = null;

            target.Init(columnName, columnTitle, _filterType, _column);

            Assert.AreEqual(columnName, target.FieldName);
            Assert.AreEqual(columnTitle, target.FieldNameTitle);
            Assert.AreEqual(OperatorType.GREATER_THAN, target.FirstCondition.ConditionOperator);
        }

        [TestMethod()]
        public void InitTest_GE()
        {
            NumericFilterVM target = new NumericFilterVM();
            string columnName = "TestColumn1";
            string columnTitle = "Test Column 1";
            NumericFilterSelectionType _filterType = new NumericFilterSelectionType();
            _filterType = NumericFilterSelectionType.GREATER_THAN_OR_EQUALTO;
            FilterColumn _column = new FilterColumn();
            _column.ColumnName = columnName;
            _column.ColumnSelectedDataList = new System.Collections.Generic.List<string>();
            _column.ConditionList = new System.Collections.Generic.List<Condition>();
            _column.CurrentRowDataValue = null;

            target.Init(columnName, columnTitle, _filterType, _column);

            Assert.AreEqual(columnName, target.FieldName);
            Assert.AreEqual(columnTitle, target.FieldNameTitle);
            Assert.AreEqual(OperatorType.GREATER_THAN_OR_EQUAL, target.FirstCondition.ConditionOperator);
        }

        [TestMethod()]
        public void InitTest_LT()
        {
            NumericFilterVM target = new NumericFilterVM();
            string columnName = "TestColumn1";
            string columnTitle = "Test Column 1";
            NumericFilterSelectionType _filterType = new NumericFilterSelectionType();
            _filterType = NumericFilterSelectionType.LESS_THAN;
            FilterColumn _column = new FilterColumn();
            _column.ColumnName = columnName;
            _column.ColumnSelectedDataList = new System.Collections.Generic.List<string>();
            _column.ConditionList = new System.Collections.Generic.List<Condition>();
            _column.CurrentRowDataValue = null;

            target.Init(columnName, columnTitle, _filterType, _column);

            Assert.AreEqual(columnName, target.FieldName);
            Assert.AreEqual(columnTitle, target.FieldNameTitle);
            Assert.AreEqual(OperatorType.LESS_THAN, target.FirstCondition.ConditionOperator);
        }

        [TestMethod()]
        public void InitTest_LE()
        {
            NumericFilterVM target = new NumericFilterVM();
            string columnName = "TestColumn1";
            string columnTitle = "Test Column 1";
            NumericFilterSelectionType _filterType = new NumericFilterSelectionType();
            _filterType = NumericFilterSelectionType.LESS_THAN_OR_EQUALTO;
            FilterColumn _column = new FilterColumn();
            _column.ColumnName = columnName;
            _column.ColumnSelectedDataList = new System.Collections.Generic.List<string>();
            _column.ConditionList = new System.Collections.Generic.List<Condition>();
            _column.CurrentRowDataValue = null;

            target.Init(columnName, columnTitle, _filterType, _column);

            Assert.AreEqual(columnName, target.FieldName);
            Assert.AreEqual(columnTitle, target.FieldNameTitle);
            Assert.AreEqual(OperatorType.LESS_THAN_OR_EQUAL, target.FirstCondition.ConditionOperator);
        }

        [TestMethod()]
        public void InitTest_RANGE()
        {
            NumericFilterVM target = new NumericFilterVM();
            string columnName = "TestColumn1";
            string columnTitle = "Test Column 1";
            NumericFilterSelectionType _filterType = new NumericFilterSelectionType();
            _filterType = NumericFilterSelectionType.RANGE;
            FilterColumn _column = new FilterColumn();
            _column.ColumnName = columnName;
            _column.ColumnSelectedDataList = new System.Collections.Generic.List<string>();
            _column.ConditionList = new System.Collections.Generic.List<Condition>();
            _column.CurrentRowDataValue = null;

            target.Init(columnName, columnTitle, _filterType, _column);

            Assert.AreEqual(columnName, target.FieldName);
            Assert.AreEqual(columnTitle, target.FieldNameTitle);
            Assert.AreEqual(OperatorType.GREATER_THAN_OR_EQUAL, target.FirstCondition.ConditionOperator);
            Assert.AreEqual(OperatorType.LESS_THAN_OR_EQUAL, target.SecondCondition.ConditionOperator);
        }

        [TestMethod()]
        public void InitTest_EQ_AndPreviouslyApplied()
        {
            NumericFilterVM target = new NumericFilterVM();
            string columnName = "TestColumn1";
            string columnTitle = "Test Column 1";
            NumericFilterSelectionType _filterType = new NumericFilterSelectionType();
            _filterType = NumericFilterSelectionType.EQUAL;
            FilterColumn _column = new FilterColumn();
            _column.ColumnName = columnName;
            _column.FilterType = FilterSelectionType.NUMERIC_EQ;
            _column.ColumnSelectedDataList = new System.Collections.Generic.List<string>();
            _column.ConditionList = new System.Collections.Generic.List<Condition>();
            Condition cond = new Condition("TestColumn1", OperatorType.EQUAL);
            _column.ConditionList.Add(cond);

            _column.CurrentRowDataValue = null;

            target.Init(columnName, columnTitle, _filterType, _column);

            Assert.AreEqual(columnName, target.FieldName);
            Assert.AreEqual(columnTitle, target.FieldNameTitle);
            Assert.AreEqual(OperatorType.EQUAL, target.FirstCondition.ConditionOperator);

            Assert.AreEqual(cond, target.FirstCondition);
        }

        [TestMethod()]
        public void InitTest_NE_AndPreviouslyApplied()
        {
            NumericFilterVM target = new NumericFilterVM();
            string columnName = "TestColumn1";
            string columnTitle = "Test Column 1";
            NumericFilterSelectionType _filterType = new NumericFilterSelectionType();
            _filterType = NumericFilterSelectionType.NOT_EQUAL;
            FilterColumn _column = new FilterColumn();
            _column.ColumnName = columnName;
            _column.FilterType = FilterSelectionType.NUMERIC_NE;
            _column.ColumnSelectedDataList = new System.Collections.Generic.List<string>();
            _column.ConditionList = new System.Collections.Generic.List<Condition>();
            Condition cond = new Condition("TestColumn1", OperatorType.NOT_EQUAL);
            _column.ConditionList.Add(cond);

            _column.CurrentRowDataValue = null;

            target.Init(columnName, columnTitle, _filterType, _column);

            Assert.AreEqual(columnName, target.FieldName);
            Assert.AreEqual(columnTitle, target.FieldNameTitle);
            Assert.AreEqual(OperatorType.NOT_EQUAL, target.FirstCondition.ConditionOperator);

            Assert.AreEqual(cond, target.FirstCondition);
        }

        [TestMethod()]
        public void InitTest_GE_AndPreviouslyApplied()
        {
            NumericFilterVM target = new NumericFilterVM();
            string columnName = "TestColumn1";
            string columnTitle = "Test Column 1";
            NumericFilterSelectionType _filterType = new NumericFilterSelectionType();
            _filterType = NumericFilterSelectionType.GREATER_THAN_OR_EQUALTO;
            FilterColumn _column = new FilterColumn();
            _column.ColumnName = columnName;
            _column.FilterType = FilterSelectionType.NUMERIC_GE;
            _column.ColumnSelectedDataList = new System.Collections.Generic.List<string>();
            _column.ConditionList = new System.Collections.Generic.List<Condition>();
            Condition cond = new Condition("TestColumn1", OperatorType.GREATER_THAN_OR_EQUAL);
            _column.ConditionList.Add(cond);

            _column.CurrentRowDataValue = null;

            target.Init(columnName, columnTitle, _filterType, _column);

            Assert.AreEqual(columnName, target.FieldName);
            Assert.AreEqual(columnTitle, target.FieldNameTitle);
            Assert.AreEqual(OperatorType.GREATER_THAN_OR_EQUAL, target.FirstCondition.ConditionOperator);

            Assert.AreEqual(cond, target.FirstCondition);
        }

        [TestMethod()]
        public void InitTest_GT_AndPreviouslyApplied()
        {
            NumericFilterVM target = new NumericFilterVM();
            string columnName = "TestColumn1";
            string columnTitle = "Test Column 1";
            NumericFilterSelectionType _filterType = new NumericFilterSelectionType();
            _filterType = NumericFilterSelectionType.GREATER_THAN;
            FilterColumn _column = new FilterColumn();
            _column.ColumnName = columnName;
            _column.FilterType = FilterSelectionType.NUMERIC_GT;
            _column.ColumnSelectedDataList = new System.Collections.Generic.List<string>();
            _column.ConditionList = new System.Collections.Generic.List<Condition>();
            Condition cond = new Condition("TestColumn1", OperatorType.GREATER_THAN);
            _column.ConditionList.Add(cond);

            _column.CurrentRowDataValue = null;

            target.Init(columnName, columnTitle, _filterType, _column);

            Assert.AreEqual(columnName, target.FieldName);
            Assert.AreEqual(columnTitle, target.FieldNameTitle);
            Assert.AreEqual(OperatorType.GREATER_THAN, target.FirstCondition.ConditionOperator);

            Assert.AreEqual(cond, target.FirstCondition);
        }

        [TestMethod()]
        public void InitTest_LE_AndPreviouslyApplied()
        {
            NumericFilterVM target = new NumericFilterVM();
            string columnName = "TestColumn1";
            string columnTitle = "Test Column 1";
            NumericFilterSelectionType _filterType = new NumericFilterSelectionType();
            _filterType = NumericFilterSelectionType.LESS_THAN_OR_EQUALTO;
            FilterColumn _column = new FilterColumn();
            _column.ColumnName = columnName;
            _column.FilterType = FilterSelectionType.NUMERIC_LE;
            _column.ColumnSelectedDataList = new System.Collections.Generic.List<string>();
            _column.ConditionList = new System.Collections.Generic.List<Condition>();
            Condition cond = new Condition("TestColumn1", OperatorType.LESS_THAN_OR_EQUAL);
            _column.ConditionList.Add(cond);

            _column.CurrentRowDataValue = null;

            target.Init(columnName, columnTitle, _filterType, _column);

            Assert.AreEqual(columnName, target.FieldName);
            Assert.AreEqual(columnTitle, target.FieldNameTitle);
            Assert.AreEqual(OperatorType.LESS_THAN_OR_EQUAL, target.FirstCondition.ConditionOperator);

            Assert.AreEqual(cond, target.FirstCondition);
        }

        [TestMethod()]
        public void InitTest_LT_AndPreviouslyApplied()
        {
            NumericFilterVM target = new NumericFilterVM();
            string columnName = "TestColumn1";
            string columnTitle = "Test Column 1";
            NumericFilterSelectionType _filterType = new NumericFilterSelectionType();
            _filterType = NumericFilterSelectionType.LESS_THAN;
            FilterColumn _column = new FilterColumn();
            _column.ColumnName = columnName;
            _column.FilterType = FilterSelectionType.NUMERIC_LT;
            _column.ColumnSelectedDataList = new System.Collections.Generic.List<string>();
            _column.ConditionList = new System.Collections.Generic.List<Condition>();
            Condition cond = new Condition("TestColumn1", OperatorType.LESS_THAN);
            _column.ConditionList.Add(cond);

            _column.CurrentRowDataValue = null;

            target.Init(columnName, columnTitle, _filterType, _column);

            Assert.AreEqual(columnName, target.FieldName);
            Assert.AreEqual(columnTitle, target.FieldNameTitle);
            Assert.AreEqual(OperatorType.LESS_THAN, target.FirstCondition.ConditionOperator);

            Assert.AreEqual(cond, target.FirstCondition);
        }

        [TestMethod()]
        public void InitTest_CUST_AndPreviouslyApplied()
        {
            NumericFilterVM target = new NumericFilterVM();
            string columnName = "TestColumn1";
            string columnTitle = "Test Column 1";
            NumericFilterSelectionType _filterType = new NumericFilterSelectionType();
            _filterType = NumericFilterSelectionType.CUSTOM;
            FilterColumn _column = new FilterColumn();
            _column.ColumnName = columnName;
            _column.FilterType = FilterSelectionType.NUMERIC_CUST;
            _column.ColumnSelectedDataList = new System.Collections.Generic.List<string>();
            _column.ConditionList = new System.Collections.Generic.List<Condition>();
            Condition cond = new Condition();
            cond.IncludedConditionList = new System.Collections.ObjectModel.ObservableCollection<Condition>();
            cond.LogicalOperatorOfIncludedCondition = LogicOperatorType.AND;

            Condition cond1 = new Condition("TestColumn1", OperatorType.GREATER_THAN_OR_EQUAL);
            Condition cond2 = new Condition("TestColumn1", OperatorType.LESS_THAN_OR_EQUAL);
            cond.IncludedConditionList.Add(cond1);
            cond.IncludedConditionList.Add(cond2);
            
            _column.ConditionList.Add(cond);
            _column.CurrentRowDataValue = null;

            target.Init(columnName, columnTitle, _filterType, _column);

            Assert.AreEqual(columnName, target.FieldName);
            Assert.AreEqual(columnTitle, target.FieldNameTitle);
            Assert.AreEqual(OperatorType.GREATER_THAN_OR_EQUAL, target.FirstCondition.ConditionOperator);

            Assert.AreEqual(cond1, target.FirstCondition);
            Assert.AreEqual(cond2, target.SecondCondition);
        }

        [TestMethod()]
        public void InitTest_RANGE_AndPreviouslyApplied()
        {
            NumericFilterVM target = new NumericFilterVM();
            string columnName = "TestColumn1";
            string columnTitle = "Test Column 1";
            NumericFilterSelectionType _filterType = new NumericFilterSelectionType();
            _filterType = NumericFilterSelectionType.RANGE;
            FilterColumn _column = new FilterColumn();
            _column.ColumnName = columnName;
            _column.FilterType = FilterSelectionType.NUMERIC_RNG;
            _column.ColumnSelectedDataList = new System.Collections.Generic.List<string>();
            _column.ConditionList = new System.Collections.Generic.List<Condition>();
            Condition cond = new Condition("TestColumn1");
            cond.LogicalOperatorOfIncludedCondition = LogicOperatorType.AND;
            cond.IncludedConditionList = new System.Collections.ObjectModel.ObservableCollection<Condition>();

            Condition cond1 = new Condition("TestColumn1", OperatorType.GREATER_THAN_OR_EQUAL);
            Condition cond2 = new Condition("TestColumn1", OperatorType.LESS_THAN_OR_EQUAL);

            cond.IncludedConditionList.Add(cond1);
            cond.IncludedConditionList.Add(cond2);

            _column.ConditionList.Add(cond);


            _column.CurrentRowDataValue = null;

            target.Init(columnName, columnTitle, _filterType, _column);

            Assert.AreEqual(columnName, target.FieldName);
            Assert.AreEqual(columnTitle, target.FieldNameTitle);
            Assert.AreEqual(OperatorType.GREATER_THAN_OR_EQUAL, target.FirstCondition.ConditionOperator);
            Assert.AreEqual(OperatorType.LESS_THAN_OR_EQUAL, target.SecondCondition.ConditionOperator);

            Assert.AreEqual(cond1, target.FirstCondition);
            Assert.AreEqual(cond2, target.SecondCondition);
        }

        [TestMethod()]
        public void OperatorTest()
        {
            NumericFilterVM target = new NumericFilterVM();
            Assert.AreEqual(6, target.OperatorList.Count);
            Assert.AreEqual(OperatorType.EQUAL, target.OperatorList[0].Operation);
            Assert.AreEqual(OperatorType.NOT_EQUAL, target.OperatorList[1].Operation);
            Assert.AreEqual(OperatorType.GREATER_THAN, target.OperatorList[2].Operation);
            Assert.AreEqual(OperatorType.GREATER_THAN_OR_EQUAL, target.OperatorList[3].Operation);
            Assert.AreEqual(OperatorType.LESS_THAN, target.OperatorList[4].Operation);
            Assert.AreEqual(OperatorType.LESS_THAN_OR_EQUAL, target.OperatorList[5].Operation);
        }

        [TestMethod()]
        public void SelectedLogicOperatorTest()
        {
            NumericFilterVM target = new NumericFilterVM();
            target.SelectedLogicOperator = LogicOperatorType.AND;
            Assert.AreEqual(LogicOperatorType.AND, target.SelectedLogicOperator);
        }

        [TestMethod()]
        public void ConditionOperatorTypeTest()
        {
            ConditionOperatorType conditionOptType = new ConditionOperatorType(OperatorType.EQUAL, "Equals");
            Assert.AreEqual(OperatorType.EQUAL, conditionOptType.Operation);
            Assert.AreEqual("Equals", conditionOptType.OperatorTypeText);
        }
    }
}
