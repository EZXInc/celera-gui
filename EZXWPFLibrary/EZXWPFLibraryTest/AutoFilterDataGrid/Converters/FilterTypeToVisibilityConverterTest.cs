using AutoFilterDataGrid.Converters;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Globalization;
using AutoFilterDataGrid.Model;
using System.Windows;

namespace EZXWPFLibraryTest
{


    [TestClass()]
    public class FilterTypeToVisibilityConverterTest
    {

        FilterTypeToVisibilityConverter target = new FilterTypeToVisibilityConverter(); 

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
        public void FilterTypeToVisibilityConverterConstructorTest()
        {
            FilterTypeToVisibilityConverter conv = new FilterTypeToVisibilityConverter();
            Assert.IsNotNull(conv);
        }

        [TestMethod()]
        public void ConvertTestOfFilterSelectionType_WithOtherValueType()
        {
            int value = 10;
            Type targetType = null;
            string parameter = null;
            CultureInfo culture = null;
            object expected = Visibility.Collapsed;
            object actual;
            actual = target.Convert(value, targetType, parameter, culture);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod()]
        public void ConvertTestOfFilterSelectionType_WithOtherParamType()
        {
            object value = FilterSelectionType.NUMERIC_EQ;
            Type targetType = null;
            int parameter = 25;
            CultureInfo culture = null;
            object expected = Visibility.Collapsed;
            object actual;
            actual = target.Convert(value, targetType, parameter, culture);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod()]
        public void ConvertTestOfFilterSelectionType_WithNULLParameter()
        {
            object value = FilterSelectionType.NUMERIC_EQ; 
            Type targetType = null; 
            string parameter = null; 
            CultureInfo culture = null;
            object expected = Visibility.Collapsed; 
            object actual;
            actual = target.Convert(value, targetType, parameter, culture);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod()]
        public void ConvertTestOfFilterSelectionType_WithParameterIsNotNumeric()
        {
            object value = FilterSelectionType.NUMERIC_EQ;
            Type targetType = null;
            string parameter = "NON_NUMERIC_PARAM";
            CultureInfo culture = null;
            object expected = Visibility.Collapsed;
            object actual;
            actual = target.Convert(value, targetType, parameter, culture);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod()]
        public void ConvertTestOfFilterSelectionType_NA()
        {
            object value = FilterSelectionType.NA;
            Type targetType = null;
            string parameter = "NUMERIC";
            CultureInfo culture = null;
            object expected = Visibility.Collapsed;
            object actual;
            actual = target.Convert(value, targetType, parameter, culture);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod()]
        public void ConvertTestOfFilterSelectionType_CUST()
        {
            object value = FilterSelectionType.NUMERIC_CUST;
            Type targetType = null;
            string parameter = "NUMERIC";
            CultureInfo culture = null;
            object expected = Visibility.Visible;
            object actual;
            actual = target.Convert(value, targetType, parameter, culture);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod()]
        public void ConvertTestOfFilterSelectionType_EQ()
        {
            object value = FilterSelectionType.NUMERIC_EQ;
            Type targetType = null;
            string parameter = "NUMERIC";
            CultureInfo culture = null;
            object expected = Visibility.Visible;
            object actual;
            actual = target.Convert(value, targetType, parameter, culture);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod()]
        public void ConvertTestOfFilterSelectionType_GE()
        {
            object value = FilterSelectionType.NUMERIC_GE;
            Type targetType = null;
            string parameter = "NUMERIC";
            CultureInfo culture = null;
            object expected = Visibility.Visible;
            object actual;
            actual = target.Convert(value, targetType, parameter, culture);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod()]
        public void ConvertTestOfFilterSelectionType_GT()
        {
            object value = FilterSelectionType.NUMERIC_GT;
            Type targetType = null;
            string parameter = "NUMERIC";
            CultureInfo culture = null;
            object expected = Visibility.Visible;
            object actual;
            actual = target.Convert(value, targetType, parameter, culture);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod()]
        public void ConvertTestOfFilterSelectionType_LE()
        {
            object value = FilterSelectionType.NUMERIC_LE;
            Type targetType = null;
            string parameter = "NUMERIC";
            CultureInfo culture = null;
            object expected = Visibility.Visible;
            object actual;
            actual = target.Convert(value, targetType, parameter, culture);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod()]
        public void ConvertTestOfFilterSelectionType_LT()
        {
            object value = FilterSelectionType.NUMERIC_LT;
            Type targetType = null;
            string parameter = "NUMERIC";
            CultureInfo culture = null;
            object expected = Visibility.Visible;
            object actual;
            actual = target.Convert(value, targetType, parameter, culture);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod()]
        public void ConvertTestOfFilterSelectionType_NE()
        {
            object value = FilterSelectionType.NUMERIC_NE;
            Type targetType = null;
            string parameter = "NUMERIC";
            CultureInfo culture = null;
            object expected = Visibility.Visible;
            object actual;
            actual = target.Convert(value, targetType, parameter, culture);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod()]
        public void ConvertTestOfFilterSelectionType_RNG()
        {
            object value = FilterSelectionType.NUMERIC_RNG;
            Type targetType = null;
            string parameter = "NUMERIC";
            CultureInfo culture = null;
            object expected = Visibility.Visible;
            object actual;
            actual = target.Convert(value, targetType, parameter, culture);
            Assert.AreEqual(expected, actual);
        }


        [TestMethod()]
        public void ConvertBackTest()
        {
            FilterTypeToVisibilityConverter target = new FilterTypeToVisibilityConverter(); 
            int value = 10; 
            Type targetType = null; 
            object parameter = null; 
            CultureInfo culture = null; 
            int expected = 10; 
            int actual = 0;
            actual = (int)target.ConvertBack(value, targetType, parameter, culture);
            Assert.AreEqual(expected, actual);
        }
    }
}
