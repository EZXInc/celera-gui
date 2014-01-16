using EZXWPFLibrary.AutoFilterDataGrid.Controller;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using EZXWPFLibrary.AutoFilterDataGrid.Model;
using System.Collections.Generic;
using AutoFilterDataGrid;
using System.Windows.Controls;
using System.Windows;

namespace EZXWPFLibraryTest
{


    [TestClass()]
    public class ColumnsConfigurationControllerTest
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
        public void ColumnsConfigurationControllerConstructorTest()
        {
            ColumnsConfigurationController target = new ColumnsConfigurationController();
            Assert.AreEqual(System.IO.Directory.GetCurrentDirectory(),target.BaseDirectory);
            Assert.IsTrue(!string.IsNullOrEmpty(target.FileName));
        }

        [TestMethod()]
        public void GetColumnsConfigurationListTest()
        {
            List<ColumnsConfigInfo> ColumnsConfigList = new List<ColumnsConfigInfo>();
            ColumnsConfigList.AddRange(GetColumnConfigList("GridTest1"));
            ColumnsConfigList.AddRange(GetColumnConfigList("GridTest2"));
            
            ColumnsConfigurationController target = new ColumnsConfigurationController();
            target.ColumnsConfig = ColumnsConfigList;
            string gridName = string.Empty; 
            int expectedCount = 3; 
            List<ColumnsConfigInfo> actual;
            actual = target.GetColumnsConfigurationList("GridTest1");
            Assert.AreEqual(expectedCount, actual.Count);

            actual = target.GetColumnsConfigurationList("GridTest2");
            Assert.AreEqual(expectedCount, actual.Count);

        }

        [TestMethod()]
        public void SaveAndLoadColumnsConfigurationControllerTest()
        {
            ColumnsConfigurationController target = new ColumnsConfigurationController();
            target.FileName = "abc.xml";
            target.LoadColumnsConfigurationController();
            Assert.IsNotNull(target.ColumnsConfig);
            Assert.AreEqual(0, target.ColumnsConfig.Count);

            target.FileName = string.Empty;
            string gridName = "DGTest1";
            AutofilterDataGrid dgGrid = new AutofilterDataGrid();

            DataGridTextColumn dgCol1 = new DataGridTextColumn();
            dgCol1.Width = 100.00;
            dgCol1.SortMemberPath = "Col1";
            dgCol1.DisplayIndex = 0;
            dgCol1.Visibility = Visibility.Visible;

            DataGridTextColumn dgCol2 = new DataGridTextColumn();
            dgCol2.Width = 100.00;
            dgCol2.SortMemberPath = "Col1";
            dgCol2.DisplayIndex = 1;
            dgCol2.Visibility = Visibility.Visible;

            dgGrid.Columns.Add(dgCol1);
            dgGrid.Columns.Add(dgCol2);


            target.ColumnsConfig = new List<ColumnsConfigInfo>();
            target.SetColumnsConfigurationList(gridName, dgGrid);

            target.BaseDirectory = System.IO.Directory.GetCurrentDirectory();
            target.Save();

            ColumnsConfigurationController target2 = new ColumnsConfigurationController();
            target2.LoadColumnsConfigurationController();

            Assert.AreEqual(target.ColumnsConfig.Count, target2.ColumnsConfig.Count);

        }

        [TestMethod()]
        public void SetColumnsConfigurationListTest()
        {
            string gridName = "DGTest1";
            AutofilterDataGrid dgGrid = new AutofilterDataGrid();

            DataGridTextColumn dgCol1 = new DataGridTextColumn();
            dgCol1.Width = 100.00;
            dgCol1.SortMemberPath = "Col1";
            dgCol1.DisplayIndex = 0;
            dgCol1.Visibility = Visibility.Visible;

            DataGridTextColumn dgCol2 = new DataGridTextColumn();
            dgCol2.Width = 100.00;
            dgCol2.SortMemberPath = "Col1";
            dgCol2.DisplayIndex = 1;
            dgCol2.Visibility = Visibility.Visible;

            dgGrid.Columns.Add(dgCol1);
            dgGrid.Columns.Add(dgCol2);


            ColumnsConfigurationController target = new ColumnsConfigurationController();
            target.ColumnsConfig = new List<ColumnsConfigInfo>();
            target.SetColumnsConfigurationList(gridName, dgGrid);

            Assert.AreEqual(dgGrid.Columns.Count, target.ColumnsConfig.Count);

            Assert.AreEqual(dgCol1.SortMemberPath, target.ColumnsConfig[0].ColumnSortMemberPath);
            Assert.AreEqual(dgCol2.SortMemberPath, target.ColumnsConfig[1].ColumnSortMemberPath);

            Assert.AreEqual(dgCol1.DisplayIndex, target.ColumnsConfig[0].ColumnDisplayIndex);
            Assert.AreEqual(dgCol2.DisplayIndex, target.ColumnsConfig[1].ColumnDisplayIndex);

            //Re set columns, so the old columns will remove and then set again.
            target.SetColumnsConfigurationList(gridName, dgGrid);

            Assert.AreEqual(dgGrid.Columns.Count, target.ColumnsConfig.Count);

            Assert.AreEqual(dgCol1.SortMemberPath, target.ColumnsConfig[0].ColumnSortMemberPath);
            Assert.AreEqual(dgCol2.SortMemberPath, target.ColumnsConfig[1].ColumnSortMemberPath);

            Assert.AreEqual(dgCol1.DisplayIndex, target.ColumnsConfig[0].ColumnDisplayIndex);
            Assert.AreEqual(dgCol2.DisplayIndex, target.ColumnsConfig[1].ColumnDisplayIndex);
        }

        [TestMethod()]
        public void SetFileNameTest()
        {
            ColumnsConfigurationController target = new ColumnsConfigurationController();
            target.FileName = string.Empty;
            Assert.AreEqual("AutoFilterGridColumnsConfig.xml", target.FileName);

            target.FileName = "TestFile.xml";
            Assert.AreEqual("TestFile.xml", target.FileName);
        }

        
        private static List<ColumnsConfigInfo> GetColumnConfigList(string gridName)
        {
            List<ColumnsConfigInfo> ColumnsConfigList = new List<ColumnsConfigInfo>();
            ColumnsConfigInfo col1;
            ColumnsConfigInfo col2;
            ColumnsConfigInfo col3;


            col1 = new ColumnsConfigInfo();
            col1.ColumnDisplayIndex = 0;
            col1.ColumnHeader = "COL 1";
            col1.ColumnSortMemberPath = "COL1";
            col1.ColumnVisibility = System.Windows.Visibility.Visible;
            col1.ColumnWidth = new ColumnWidth(100.0);
            col1.GridName = gridName;


            col2 = new ColumnsConfigInfo();
            col2.ColumnDisplayIndex = 1;
            col2.ColumnHeader = "COL 2";
            col2.ColumnSortMemberPath = "COL2";
            col2.ColumnVisibility = System.Windows.Visibility.Visible;
            col2.ColumnWidth = new ColumnWidth(100.0);
            col2.GridName = gridName;


            col3 = new ColumnsConfigInfo();
            col3.ColumnDisplayIndex = 2;
            col3.ColumnHeader = "COL 3";
            col3.ColumnSortMemberPath = "COL3";
            col3.ColumnVisibility = System.Windows.Visibility.Visible;
            col3.ColumnWidth = new ColumnWidth(100.0);
            col3.GridName = gridName;

            ColumnsConfigList.Add(col1);
            ColumnsConfigList.Add(col2);
            ColumnsConfigList.Add(col3);

            return ColumnsConfigList;
        }


    }
}
