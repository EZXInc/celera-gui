using AutoFilterDataGrid.Converters;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Globalization;
using AutoFilterDataGrid.Model;

namespace EZXWPFLibraryTest
{


    [TestClass()]
    public class FilterTypeToBooleanConverterTest
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

        FilterTypeToBooleanConverter target = new FilterTypeToBooleanConverter();

        [TestMethod()]
        public void FilterTypeToBooleanConverterConstructorTest()
        {
            FilterTypeToBooleanConverter conv = new FilterTypeToBooleanConverter();
            Assert.IsNotNull(conv);
        }

        [TestMethod()]
        public void ConvertTest_WithNullParam()
        {
            object value = FilterSelectionType.NUMERIC_EQ; 
            Type targetType = null; 
            object parameter = null; 
            CultureInfo culture = null; 
            object expected = false; 
            object actual;
            actual = target.Convert(value, targetType, parameter, culture);
            Assert.AreEqual(expected, actual);
        }


        [TestMethod()]
        public void ConvertTest_WithParam_EQ()
        {
            object value = FilterSelectionType.NUMERIC_EQ;
            Type targetType = null;
            object parameter = "EQ";
            CultureInfo culture = null;
            object expected = true;
            object actual;
            actual = target.Convert(value, targetType, parameter, culture);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod()]
        public void ConvertTest_WithOtherParam_EQ()
        {
            object value = FilterSelectionType.NUMERIC_EQ;
            Type targetType = null;
            object parameter = "NE";
            CultureInfo culture = null;
            object expected = false;
            object actual;
            actual = target.Convert(value, targetType, parameter, culture);
            Assert.AreEqual(expected, actual);
        }


        [TestMethod()]
        public void ConvertTest_WithParam_NE()
        {
            object value = FilterSelectionType.NUMERIC_NE;
            Type targetType = null;
            object parameter = "NE";
            CultureInfo culture = null;
            object expected = true;
            object actual;
            actual = target.Convert(value, targetType, parameter, culture);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod()]
        public void ConvertTest_WithOtherParam_NE()
        {
            object value = FilterSelectionType.NUMERIC_NE;
            Type targetType = null;
            object parameter = "EQ";
            CultureInfo culture = null;
            object expected = false;
            object actual;
            actual = target.Convert(value, targetType, parameter, culture);
            Assert.AreEqual(expected, actual);
        }



        [TestMethod()]
        public void ConvertTest_WithParam_GE()
        {
            object value = FilterSelectionType.NUMERIC_GE;
            Type targetType = null;
            object parameter = "GE";
            CultureInfo culture = null;
            object expected = true;
            object actual;
            actual = target.Convert(value, targetType, parameter, culture);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod()]
        public void ConvertTest_WithOtherParam_GE()
        {
            object value = FilterSelectionType.NUMERIC_GE;
            Type targetType = null;
            object parameter = "GT";
            CultureInfo culture = null;
            object expected = false;
            object actual;
            actual = target.Convert(value, targetType, parameter, culture);
            Assert.AreEqual(expected, actual);
        }


        [TestMethod()]
        public void ConvertTest_WithParam_GT()
        {
            object value = FilterSelectionType.NUMERIC_GT;
            Type targetType = null;
            object parameter = "GT";
            CultureInfo culture = null;
            object expected = true;
            object actual;
            actual = target.Convert(value, targetType, parameter, culture);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod()]
        public void ConvertTest_WithOtherParam_GT()
        {
            object value = FilterSelectionType.NUMERIC_GT;
            Type targetType = null;
            object parameter = "GE";
            CultureInfo culture = null;
            object expected = false;
            object actual;
            actual = target.Convert(value, targetType, parameter, culture);
            Assert.AreEqual(expected, actual);
        }


        [TestMethod()]
        public void ConvertTest_WithParam_LE()
        {
            object value = FilterSelectionType.NUMERIC_LE;
            Type targetType = null;
            object parameter = "LE";
            CultureInfo culture = null;
            object expected = true;
            object actual;
            actual = target.Convert(value, targetType, parameter, culture);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod()]
        public void ConvertTest_WithOtherParam_LE()
        {
            object value = FilterSelectionType.NUMERIC_LE;
            Type targetType = null;
            object parameter = "LT";
            CultureInfo culture = null;
            object expected = false;
            object actual;
            actual = target.Convert(value, targetType, parameter, culture);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod()]
        public void ConvertTest_WithParam_LT()
        {
            object value = FilterSelectionType.NUMERIC_LT;
            Type targetType = null;
            object parameter = "LT";
            CultureInfo culture = null;
            object expected = true;
            object actual;
            actual = target.Convert(value, targetType, parameter, culture);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod()]
        public void ConvertTest_WithOtherParam_LT()
        {
            object value = FilterSelectionType.NUMERIC_LT;
            Type targetType = null;
            object parameter = "LE";
            CultureInfo culture = null;
            object expected = false;
            object actual;
            actual = target.Convert(value, targetType, parameter, culture);
            Assert.AreEqual(expected, actual);

            parameter = "LT";
            expected = false;
            actual = target.Convert(value, targetType, parameter, culture);
            Assert.AreNotEqual(expected, actual);

        }

        [TestMethod()]
        public void ConvertTest_WithParam_CUST()
        {
            object value = FilterSelectionType.NUMERIC_CUST;
            Type targetType = null;
            object parameter = "CUST";
            CultureInfo culture = null;
            object expected = true;
            object actual;
            actual = target.Convert(value, targetType, parameter, culture);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod()]
        public void ConvertTest_WithOtherParam_CUST()
        {
            object value = FilterSelectionType.NUMERIC_CUST;
            Type targetType = null;
            object parameter = "RNG";
            CultureInfo culture = null;
            object expected = false;
            object actual;
            actual = target.Convert(value, targetType, parameter, culture);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod()]
        public void ConvertTest_WithParam_RNG()
        {
            object value = FilterSelectionType.NUMERIC_RNG;
            Type targetType = null;
            object parameter = "RNG";
            CultureInfo culture = null;
            object expected = true;
            object actual;
            actual = target.Convert(value, targetType, parameter, culture);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod()]
        public void ConvertTest_WithOtherParam_RNG()
        {
            object value = FilterSelectionType.NUMERIC_RNG;
            Type targetType = null;
            object parameter = "CUST";
            CultureInfo culture = null;
            object expected = false;
            object actual;
            actual = target.Convert(value, targetType, parameter, culture);
            Assert.AreEqual(expected, actual);
        }


        [TestMethod()]
        public void ConvertTest_WithOtherParam()
        {
            object value = FilterSelectionType.NUMERIC_RNG;
            Type targetType = null;
            object parameter = "TEXT";
            CultureInfo culture = null;
            object expected = false;
            object actual;
            actual = target.Convert(value, targetType, parameter, culture);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod()]
        public void ConvertTest_WithOtherParamType()
        {
            object value = FilterSelectionType.NUMERIC_RNG;
            Type targetType = null;
            int parameter = 10;
            CultureInfo culture = null;
            object expected = false;
            object actual;
            actual = target.Convert(value, targetType, parameter, culture);
            Assert.AreEqual(expected, actual);
        }

        
        [TestMethod()]
        public void ConvertTest_WithParam_NUMERIC()
        {
            object value = FilterSelectionType.NUMERIC_EQ;
            Type targetType = null;
            object parameter = "NUMERIC";
            CultureInfo culture = null;
            object expected = true;
            object actual;
            actual = target.Convert(value, targetType, parameter, culture);
            Assert.AreEqual(expected, actual);

            value = FilterSelectionType.NUMERIC_NE;
            actual = target.Convert(value, targetType, parameter, culture);
            Assert.AreEqual(expected, actual);

            value = FilterSelectionType.NUMERIC_GE;
            actual = target.Convert(value, targetType, parameter, culture);
            Assert.AreEqual(expected, actual);

            value = FilterSelectionType.NUMERIC_LE;
            actual = target.Convert(value, targetType, parameter, culture);
            Assert.AreEqual(expected, actual);

            value = FilterSelectionType.NUMERIC_GT;
            actual = target.Convert(value, targetType, parameter, culture);
            Assert.AreEqual(expected, actual);

            value = FilterSelectionType.NUMERIC_LT;
            actual = target.Convert(value, targetType, parameter, culture);
            Assert.AreEqual(expected, actual);

            value = FilterSelectionType.NUMERIC_RNG;
            actual = target.Convert(value, targetType, parameter, culture);
            Assert.AreEqual(expected, actual);

            value = FilterSelectionType.NUMERIC_CUST;
            actual = target.Convert(value, targetType, parameter, culture);
            Assert.AreEqual(expected, actual);

            expected = false;
            value = FilterSelectionType.NA;
            actual = target.Convert(value, targetType, parameter, culture);
            Assert.AreEqual(expected, actual);

        }


        [TestMethod()]
        public void ConvertBackTest()
        {
            object value = true; 
            Type targetType = null; 
            object parameter = null; 
            CultureInfo culture = null; 
            object expected = true; 
            object actual = false;
            actual = target.ConvertBack(value, targetType, parameter, culture);
            Assert.AreEqual(expected, actual);
        }
    }
}
