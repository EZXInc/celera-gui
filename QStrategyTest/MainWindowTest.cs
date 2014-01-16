using QStrategyWPF;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Windows.Markup;
using System.ComponentModel;

namespace QStrategyTest
{


    [TestClass()]
    public class MainWindowTest
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
        public void MainWindowConstructorTest()
        {
            MainWindow target = new MainWindow();
        }

        [TestMethod()]
        public void InitializeComponentTest()
        {
            MainWindow target = new MainWindow(); // TODO: Initialize to an appropriate value
            target.InitializeComponent();
        }

        [TestMethod()]
        [DeploymentItem("QStrategyWPF.exe")]
        public void LoginTimer_TickTest()
        {
            MainWindow_Accessor target = new MainWindow_Accessor(); // TODO: Initialize to an appropriate value
            object sender = null; // TODO: Initialize to an appropriate value
            EventArgs e = null; // TODO: Initialize to an appropriate value
            target.LoginTimer_Tick(sender, e);
        }

        [TestMethod()]
        public void ShowLoginViewTest()
        {
            MainWindow target = new MainWindow(); // TODO: Initialize to an appropriate value
            bool expected = false; // TODO: Initialize to an appropriate value
            bool actual;
            actual = target.ShowLoginView();
            Assert.AreEqual(expected, actual);
        }

        [TestMethod()]
        [DeploymentItem("QStrategyWPF.exe")]
        public void ConnectTest()
        {
            IComponentConnector target = new MainWindow(); // TODO: Initialize to an appropriate value
            int connectionId = 0; // TODO: Initialize to an appropriate value
            object target1 = null; // TODO: Initialize to an appropriate value
            target.Connect(connectionId, target1);
        }

        [TestMethod()]
        [DeploymentItem("QStrategyWPF.exe")]
        public void Window_ClosingTest()
        {
            MainWindow_Accessor target = new MainWindow_Accessor(); // TODO: Initialize to an appropriate value
            object sender = null; // TODO: Initialize to an appropriate value
            CancelEventArgs e = null; // TODO: Initialize to an appropriate value
            target.Window_Closing(sender, e);
        }

        [TestMethod()]
        [DeploymentItem("QStrategyWPF.exe")]
        public void _CreateDelegateTest()
        {
            MainWindow_Accessor target = new MainWindow_Accessor(); // TODO: Initialize to an appropriate value
            Type delegateType = null; // TODO: Initialize to an appropriate value
            string handler = string.Empty; // TODO: Initialize to an appropriate value
            Delegate expected = null; // TODO: Initialize to an appropriate value
            Delegate actual;
            actual = target._CreateDelegate(delegateType, handler);
            Assert.AreEqual(expected, actual);
        }
    }
}
