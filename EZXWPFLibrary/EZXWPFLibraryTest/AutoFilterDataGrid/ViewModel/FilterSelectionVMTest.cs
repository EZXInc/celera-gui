using AutoFilterDataGrid.ViewModel;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.ObjectModel;
using AutoFilterDataGrid.Model;

namespace EZXWPFLibraryTest
{


    [TestClass()]
    public class FilterSelectionVMTest
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
        public void FilterSelectionVMConstructorTest()
        {
            FilterSelectionVM target = new FilterSelectionVM();
            Assert.IsTrue(target.FilterItemList == null);
            Assert.IsTrue(target.SelectedFilterColumn == null);
        }

        [TestMethod()]
        public void CheckAllFilterItemsTest()
        {
            FilterSelectionVM target = new FilterSelectionVM();
            target.FilterItemList = GetFilterItem(false);
            
            for(int x=0; x< target.FilterItemList.Count; x++)
            {
                Assert.IsFalse(target.FilterItemList[x].IsSelected);
            }

            target.CheckAllFilterItems();

            for (int x = 0; x < target.FilterItemList.Count; x++)
            {
                if (!target.FilterItemList[x].Data.Equals("Unselect all"))
                {
                    Assert.IsTrue(target.FilterItemList[x].IsSelected);
                }
            }

        }

        [TestMethod()]
        public void UncheckAllFilterItemsTest()
        {
            FilterSelectionVM target = new FilterSelectionVM();
            target.FilterItemList = GetFilterItem(true);

            for (int x = 0; x < target.FilterItemList.Count; x++)
            {
                Assert.IsTrue(target.FilterItemList[x].IsSelected);
            }

            target.UncheckAllFilterItems();

            for (int x = 0; x < target.FilterItemList.Count; x++)
            {
                if (!target.FilterItemList[x].Data.Equals("Unselect all"))
                {
                    Assert.IsFalse(target.FilterItemList[x].IsSelected);
                }
            }
        }

        [TestMethod()]
        public void MarkBlankSelectFilterItemTest()
        {
            FilterSelectionVM target = new FilterSelectionVM();
            target.FilterItemList = new ObservableCollection<FilterItem>();
            target.FilterItemList.Add(new FilterItem(){ Data="Test", IsSelected = true});
            target.MarkBlankSelectFilterItem();
            for (int x = 0; x < target.FilterItemList.Count; x++)
            {
                Assert.IsTrue(target.FilterItemList[x].IsSelected);
            }

            target.FilterItemList = GetFilterItem(true);
            target.MarkBlankSelectFilterItem();
            for (int x = 0; x < target.FilterItemList.Count; x++)
            {
                if (target.FilterItemList[x].Data.Equals("Select all"))
                {
                    Assert.IsFalse(target.FilterItemList[x].IsSelected);
                }
            }
        }


        [TestMethod()]
        public void MarkBlankUnSelectFilterItemTest()
        {
            FilterSelectionVM target = new FilterSelectionVM(); 
            target.FilterItemList = GetFilterItem(true);
            target.MarkBlankUnSelectFilterItem();
            for (int x = 0; x < target.FilterItemList.Count; x++)
            {
                if (target.FilterItemList[x].Data.Equals("Unselect all"))
                {
                    Assert.IsFalse(target.FilterItemList[x].IsSelected);
                }
            }


        }

        [TestMethod()]
        public void InitFilteredDataTest()
        {
            FilterSelectionVM target = new FilterSelectionVM();
            string columnName = "TestColumn1";

            target.SelectedFilterColumn = new FilterColumn();

            ObservableCollection<string> filterStringList = new ObservableCollection<string>();
            filterStringList.Add("Data 1");
            filterStringList.Add("Data 2");
            filterStringList.Add("Data 3");
            filterStringList.Add("Data 4");
            target.InitFilteredData(columnName, filterStringList);
            
            Assert.AreEqual(6, target.FilterItemList.Count);
            Assert.AreEqual("Select all", target.FilterItemList[0].Data);
            Assert.AreEqual(true, target.FilterItemList[0].IsSelected);

            Assert.AreEqual("Unselect all", target.FilterItemList[1].Data);
            Assert.AreEqual(false, target.FilterItemList[1].IsSelected);

            Assert.AreEqual("Data 1", target.FilterItemList[2].Data);
            Assert.AreEqual(true, target.FilterItemList[2].IsSelected);

            Assert.AreEqual("Data 2", target.FilterItemList[3].Data);
            Assert.AreEqual(true, target.FilterItemList[3].IsSelected);

            Assert.AreEqual("Data 3", target.FilterItemList[4].Data);
            Assert.AreEqual(true, target.FilterItemList[4].IsSelected);

            Assert.AreEqual("Data 4", target.FilterItemList[5].Data);
            Assert.AreEqual(true, target.FilterItemList[5].IsSelected);

        }

        [TestMethod()]
        public void RefreshFilteredDataTest()
        {
            FilterSelectionVM target = new FilterSelectionVM();
            string columnName = "TestColumn1";
            ObservableCollection<string> filterStringList = new ObservableCollection<string>();
            filterStringList.Add("Data 1");
            filterStringList.Add("Data 2");
            filterStringList.Add("Data 3");
            filterStringList.Add("Data 4"); 
            
            FilterColumn filterColumn = null; 
            target.RefreshFilteredData(columnName, filterStringList, filterColumn);

            Assert.AreEqual(6, target.FilterItemList.Count);
            Assert.AreEqual("Select all", target.FilterItemList[0].Data);
            Assert.AreEqual(true, target.FilterItemList[0].IsSelected);

            Assert.AreEqual("Unselect all", target.FilterItemList[1].Data);
            Assert.AreEqual(false, target.FilterItemList[1].IsSelected);

            Assert.AreEqual("Data 1", target.FilterItemList[2].Data);
            Assert.AreEqual(true, target.FilterItemList[2].IsSelected);

            Assert.AreEqual("Data 2", target.FilterItemList[3].Data);
            Assert.AreEqual(true, target.FilterItemList[3].IsSelected);

            Assert.AreEqual("Data 3", target.FilterItemList[4].Data);
            Assert.AreEqual(true, target.FilterItemList[4].IsSelected);

            Assert.AreEqual("Data 4", target.FilterItemList[5].Data);
            Assert.AreEqual(true, target.FilterItemList[5].IsSelected);


        }

        [TestMethod()]
        public void RefreshFilteredDataTest_IfFilterAlreadyDefined()
        {
            FilterSelectionVM target = new FilterSelectionVM();
            string columnName = "TestColumn1";
            ObservableCollection<string> filterStringList = new ObservableCollection<string>();
            filterStringList.Add("Data 1");
            filterStringList.Add("Data 2");
            filterStringList.Add("Data 3");
            filterStringList.Add("Data 4");

            FilterColumn filterColumn = new FilterColumn();
            filterColumn.ColumnSelectedDataList = new System.Collections.Generic.List<string>();
            filterColumn.ColumnSelectedDataList.Add("Data 1");
            filterColumn.ColumnSelectedDataList.Add("Data 3");
            target.RefreshFilteredData(columnName, filterStringList, filterColumn);

            Assert.AreEqual(6, target.FilterItemList.Count);
            Assert.AreEqual("Select all", target.FilterItemList[0].Data);
            Assert.AreEqual(false, target.FilterItemList[0].IsSelected);

            Assert.AreEqual("Unselect all", target.FilterItemList[1].Data);
            Assert.AreEqual(false, target.FilterItemList[1].IsSelected);

            Assert.AreEqual("Data 1", target.FilterItemList[2].Data);
            Assert.AreEqual(true, target.FilterItemList[2].IsSelected);

            Assert.AreEqual("Data 2", target.FilterItemList[3].Data);
            Assert.AreEqual(false, target.FilterItemList[3].IsSelected);

            Assert.AreEqual("Data 3", target.FilterItemList[4].Data);
            Assert.AreEqual(true, target.FilterItemList[4].IsSelected);

            Assert.AreEqual("Data 4", target.FilterItemList[5].Data);
            Assert.AreEqual(false, target.FilterItemList[5].IsSelected);
        }

        [TestMethod()]
        public void RefreshFilteredDataTest_IfFilterAlreadyDefinedWitNoNumericFilter()
        {
            FilterSelectionVM target = new FilterSelectionVM();
            string columnName = "TestColumn1";
            target.SelectedFilterColumn = new FilterColumn();
            ObservableCollection<string> filterStringList = new ObservableCollection<string>();
            filterStringList.Add("Data 1");
            filterStringList.Add("Data 2");
            filterStringList.Add("Data 3");
            filterStringList.Add("Data 4");

            FilterColumn filterColumn = new FilterColumn();
            filterColumn.ColumnSelectedDataList = new System.Collections.Generic.List<string>();
            filterColumn.ColumnSelectedDataList.Add("Data 1");
            filterColumn.ColumnSelectedDataList.Add("Data 3");
            target.RefreshFilteredData(columnName, filterStringList, filterColumn);

            Assert.AreEqual(6, target.FilterItemList.Count);
            Assert.AreEqual("Select all", target.FilterItemList[0].Data);
            Assert.AreEqual(false, target.FilterItemList[0].IsSelected);

            Assert.AreEqual("Unselect all", target.FilterItemList[1].Data);
            Assert.AreEqual(false, target.FilterItemList[1].IsSelected);

            Assert.AreEqual("Data 1", target.FilterItemList[2].Data);
            Assert.AreEqual(true, target.FilterItemList[2].IsSelected);

            Assert.AreEqual("Data 2", target.FilterItemList[3].Data);
            Assert.AreEqual(false, target.FilterItemList[3].IsSelected);

            Assert.AreEqual("Data 3", target.FilterItemList[4].Data);
            Assert.AreEqual(true, target.FilterItemList[4].IsSelected);

            Assert.AreEqual("Data 4", target.FilterItemList[5].Data);
            Assert.AreEqual(false, target.FilterItemList[5].IsSelected);
        }
        
        [TestMethod()]
        public void RefreshFilteredDataTest_IfFilterAlreadyDefinedWitNumericFilter()
        {
            FilterSelectionVM target = new FilterSelectionVM();
            string columnName = "TestColumn1";
            target.SelectedFilterColumn = new FilterColumn();
            ObservableCollection<string> filterStringList = new ObservableCollection<string>();
            filterStringList.Add("Data 1");
            filterStringList.Add("Data 2");
            filterStringList.Add("Data 3");
            filterStringList.Add("Data 4");

            FilterColumn filterColumn = new FilterColumn();
            filterColumn.FilterType = FilterSelectionType.NUMERIC_EQ;
            filterColumn.ColumnSelectedDataList = new System.Collections.Generic.List<string>();
            target.RefreshFilteredData(columnName, filterStringList, filterColumn);

            Assert.AreEqual(6, target.FilterItemList.Count);
            Assert.AreEqual("Select all", target.FilterItemList[0].Data);
            Assert.AreEqual(false, target.FilterItemList[0].IsSelected);

            Assert.AreEqual("Unselect all", target.FilterItemList[1].Data);
            Assert.AreEqual(false, target.FilterItemList[1].IsSelected);

            Assert.AreEqual("Data 1", target.FilterItemList[2].Data);
            Assert.AreEqual(false, target.FilterItemList[2].IsSelected);

            Assert.AreEqual("Data 2", target.FilterItemList[3].Data);
            Assert.AreEqual(false, target.FilterItemList[3].IsSelected);

            Assert.AreEqual("Data 3", target.FilterItemList[4].Data);
            Assert.AreEqual(false, target.FilterItemList[4].IsSelected);

            Assert.AreEqual("Data 4", target.FilterItemList[5].Data);
            Assert.AreEqual(false, target.FilterItemList[5].IsSelected);
        }


        [TestMethod()]
        public void FilterItemDataDisplayTest()
        {
            FilterItem item = new FilterItem();
            item.Data = "5";
            item.IsSelected = true;
            Assert.AreEqual(item.Data, item.DataDisplay);

            FilterItem item2 = new FilterItem();
            item2.Data = "255.50";
            item2.IsSelected = true;
            Assert.AreEqual("255.50000", item2.DataDisplay);

            FilterItem item3 = new FilterItem();
            item3.Data = "Test";
            item3.IsSelected = true;
            Assert.AreEqual("Test", item3.DataDisplay);
        }


        private ObservableCollection<FilterItem> GetFilterItem(bool isSelected)
        {
            ObservableCollection<FilterItem> filterItemList = new ObservableCollection<FilterItem>();

            FilterItem fSelect = new FilterItem();
            fSelect.Data = "Select all";
            fSelect.IsSelected = isSelected;

            FilterItem fUnselect = new FilterItem();
            fUnselect.Data = "Unselect all";
            fUnselect.IsSelected = isSelected;
            
            
            FilterItem f1 = new FilterItem();
            f1.Data = "Data1";
            f1.IsSelected = isSelected;

            FilterItem f2 = new FilterItem();
            f2.Data = "Data2";
            f2.IsSelected = isSelected;

            FilterItem f3 = new FilterItem();
            f3.Data = "Data3";
            f3.IsSelected = isSelected;

            filterItemList.Add(fSelect);
            filterItemList.Add(fUnselect);
            filterItemList.Add(f1);
            filterItemList.Add(f2);
            filterItemList.Add(f3);

            return filterItemList;
        }

    }
}
