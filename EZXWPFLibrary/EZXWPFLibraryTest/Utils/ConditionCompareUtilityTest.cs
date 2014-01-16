using EZXWPFLibrary.Utils;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using AutoFilterDataGrid.Model;

namespace EZXWPFLibraryTest
{


    [TestClass()]
    public class ConditionCompareUtilityTest
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

        public void CompareParameterValuesTestHelper<T>()
            where T : IComparable
        {
            T firstValue = default(T); 
            T secondValue = default(T); 
            OperatorType operatorType = new OperatorType(); 
            bool expected = false; 
            bool actual;
            actual = ConditionCompareUtility.CompareParameterValues<T>(firstValue, secondValue, operatorType);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod()]
        public void CompareParameterValuesTestForINTType()
        {
            int firstValINT = 0;
            int secondValINT = 0;
            bool expected = true;
            bool actual = true;

            firstValINT = 10;
            secondValINT = 10;
            actual = ConditionCompareUtility.CompareParameterValues(firstValINT, secondValINT, OperatorType.EQUAL);
            Assert.AreEqual(expected, actual);

            expected = false;
            actual = false;
            firstValINT = 10;
            secondValINT = 20;
            actual = ConditionCompareUtility.CompareParameterValues(firstValINT, secondValINT, OperatorType.EQUAL);
            Assert.AreEqual(expected, actual);

            expected = true;
            actual = true;
            firstValINT = 10;
            secondValINT = 20;
            actual = ConditionCompareUtility.CompareParameterValues(firstValINT, secondValINT, OperatorType.NOT_EQUAL);
            Assert.AreEqual(expected, actual);

            expected = false;
            actual = false;
            firstValINT = 10;
            secondValINT = 10;
            actual = ConditionCompareUtility.CompareParameterValues(firstValINT, secondValINT, OperatorType.NOT_EQUAL);
            Assert.AreEqual(expected, actual);

            expected = true;
            actual = true;
            firstValINT = 20;
            secondValINT = 10;
            actual = ConditionCompareUtility.CompareParameterValues(firstValINT, secondValINT, OperatorType.GREATER_THAN);
            Assert.AreEqual(expected, actual);

            expected = true;
            actual = true;
            firstValINT = 10;
            secondValINT = 10;
            actual = ConditionCompareUtility.CompareParameterValues(firstValINT, secondValINT, OperatorType.GREATER_THAN_OR_EQUAL);
            Assert.AreEqual(expected, actual);

            expected = true;
            actual = true;
            firstValINT = 5;
            secondValINT = 10;
            actual = ConditionCompareUtility.CompareParameterValues(firstValINT, secondValINT, OperatorType.LESS_THAN);
            Assert.AreEqual(expected, actual);

            expected = true;
            actual = true;
            firstValINT = 5;
            secondValINT = 5;
            actual = ConditionCompareUtility.CompareParameterValues(firstValINT, secondValINT, OperatorType.LESS_THAN_OR_EQUAL);
            Assert.AreEqual(expected, actual);

            expected = false;
            actual = false;
            firstValINT = 5;
            secondValINT = 5;
            actual = ConditionCompareUtility.CompareParameterValues(firstValINT, secondValINT, OperatorType.NA);
            Assert.AreEqual(expected, actual);

            expected = false;
            actual = false;
            firstValINT = 5;
            secondValINT = 10;
            actual = ConditionCompareUtility.CompareParameterValues(firstValINT, secondValINT, OperatorType.GREATER_THAN);
            Assert.AreEqual(expected, actual);

            expected = false;
            actual = false;
            firstValINT = 5;
            secondValINT = 10;
            actual = ConditionCompareUtility.CompareParameterValues(firstValINT, secondValINT, OperatorType.GREATER_THAN_OR_EQUAL);
            Assert.AreEqual(expected, actual);

            expected = false;
            actual = false;
            firstValINT = 15;
            secondValINT = 10;
            actual = ConditionCompareUtility.CompareParameterValues(firstValINT, secondValINT, OperatorType.LESS_THAN);
            Assert.AreEqual(expected, actual);

            expected = false;
            actual = false;
            firstValINT = 15;
            secondValINT = 5;
            actual = ConditionCompareUtility.CompareParameterValues(firstValINT, secondValINT, OperatorType.LESS_THAN_OR_EQUAL);
            Assert.AreEqual(expected, actual);

        
        }

        [TestMethod()]
        public void CompareParameterValuesTestForDoubleType()
        {
            double firstValDouble = 0.0;
            double secondValDouble = 0.0;
            bool expected = true;
            bool actual = true;

            firstValDouble = 10;
            secondValDouble = 10;
            actual = ConditionCompareUtility.CompareParameterValues(firstValDouble, secondValDouble, OperatorType.EQUAL);
            Assert.AreEqual(expected, actual);

            expected = false;
            actual = false;
            firstValDouble = 10;
            secondValDouble = 20;
            actual = ConditionCompareUtility.CompareParameterValues(firstValDouble, secondValDouble, OperatorType.EQUAL);
            Assert.AreEqual(expected, actual);

            expected = true;
            actual = true;
            firstValDouble = 10;
            secondValDouble = 20;
            actual = ConditionCompareUtility.CompareParameterValues(firstValDouble, secondValDouble, OperatorType.NOT_EQUAL);
            Assert.AreEqual(expected, actual);

            expected = false;
            actual = false;
            firstValDouble = 10;
            secondValDouble = 10;
            actual = ConditionCompareUtility.CompareParameterValues(firstValDouble, secondValDouble, OperatorType.NOT_EQUAL);
            Assert.AreEqual(expected, actual);

            expected = true;
            actual = true;
            firstValDouble = 20;
            secondValDouble = 10;
            actual = ConditionCompareUtility.CompareParameterValues(firstValDouble, secondValDouble, OperatorType.GREATER_THAN);
            Assert.AreEqual(expected, actual);

            expected = true;
            actual = true;
            firstValDouble = 10;
            secondValDouble = 10;
            actual = ConditionCompareUtility.CompareParameterValues(firstValDouble, secondValDouble, OperatorType.GREATER_THAN_OR_EQUAL);
            Assert.AreEqual(expected, actual);

            expected = true;
            actual = true;
            firstValDouble = 5;
            secondValDouble = 10;
            actual = ConditionCompareUtility.CompareParameterValues(firstValDouble, secondValDouble, OperatorType.LESS_THAN);
            Assert.AreEqual(expected, actual);

            expected = true;
            actual = true;
            firstValDouble = 5;
            secondValDouble = 5;
            actual = ConditionCompareUtility.CompareParameterValues(firstValDouble, secondValDouble, OperatorType.LESS_THAN_OR_EQUAL);
            Assert.AreEqual(expected, actual);

            expected = false;
            actual = false;
            firstValDouble = 5;
            secondValDouble = 5;
            actual = ConditionCompareUtility.CompareParameterValues(firstValDouble, secondValDouble, OperatorType.NA);
            Assert.AreEqual(expected, actual);

            expected = false;
            actual = false;
            firstValDouble = 5;
            secondValDouble = 10;
            actual = ConditionCompareUtility.CompareParameterValues(firstValDouble, secondValDouble, OperatorType.GREATER_THAN);
            Assert.AreEqual(expected, actual);

            expected = false;
            actual = false;
            firstValDouble = 5;
            secondValDouble = 10;
            actual = ConditionCompareUtility.CompareParameterValues(firstValDouble, secondValDouble, OperatorType.GREATER_THAN_OR_EQUAL);
            Assert.AreEqual(expected, actual);

            expected = false;
            actual = false;
            firstValDouble = 15;
            secondValDouble = 10;
            actual = ConditionCompareUtility.CompareParameterValues(firstValDouble, secondValDouble, OperatorType.LESS_THAN);
            Assert.AreEqual(expected, actual);

            expected = false;
            actual = false;
            firstValDouble = 15;
            secondValDouble = 5;
            actual = ConditionCompareUtility.CompareParameterValues(firstValDouble, secondValDouble, OperatorType.LESS_THAN_OR_EQUAL);
            Assert.AreEqual(expected, actual);


        }

        [TestMethod()]
        public void CompareParameterValuesTestForDecimalType()
        {
            decimal firstValDecimal;
            decimal secondValDecimal;
            bool expected = true;
            bool actual = true;

            firstValDecimal = 10;
            secondValDecimal = 10;
            actual = ConditionCompareUtility.CompareParameterValues(firstValDecimal, secondValDecimal, OperatorType.EQUAL);
            Assert.AreEqual(expected, actual);

            expected = false;
            actual = false;
            firstValDecimal = 10;
            secondValDecimal = 20;
            actual = ConditionCompareUtility.CompareParameterValues(firstValDecimal, secondValDecimal, OperatorType.EQUAL);
            Assert.AreEqual(expected, actual);

            expected = true;
            actual = true;
            firstValDecimal = 10;
            secondValDecimal = 20;
            actual = ConditionCompareUtility.CompareParameterValues(firstValDecimal, secondValDecimal, OperatorType.NOT_EQUAL);
            Assert.AreEqual(expected, actual);

            expected = false;
            actual = false;
            firstValDecimal = 10;
            secondValDecimal = 10;
            actual = ConditionCompareUtility.CompareParameterValues(firstValDecimal, secondValDecimal, OperatorType.NOT_EQUAL);
            Assert.AreEqual(expected, actual);

            expected = true;
            actual = true;
            firstValDecimal = 20;
            secondValDecimal = 10;
            actual = ConditionCompareUtility.CompareParameterValues(firstValDecimal, secondValDecimal, OperatorType.GREATER_THAN);
            Assert.AreEqual(expected, actual);

            expected = true;
            actual = true;
            firstValDecimal = 10;
            secondValDecimal = 10;
            actual = ConditionCompareUtility.CompareParameterValues(firstValDecimal, secondValDecimal, OperatorType.GREATER_THAN_OR_EQUAL);
            Assert.AreEqual(expected, actual);

            expected = true;
            actual = true;
            firstValDecimal = 5;
            secondValDecimal = 10;
            actual = ConditionCompareUtility.CompareParameterValues(firstValDecimal, secondValDecimal, OperatorType.LESS_THAN);
            Assert.AreEqual(expected, actual);

            expected = true;
            actual = true;
            firstValDecimal = 5;
            secondValDecimal = 5;
            actual = ConditionCompareUtility.CompareParameterValues(firstValDecimal, secondValDecimal, OperatorType.LESS_THAN_OR_EQUAL);
            Assert.AreEqual(expected, actual);

            expected = false;
            actual = false;
            firstValDecimal = 5;
            secondValDecimal = 5;
            actual = ConditionCompareUtility.CompareParameterValues(firstValDecimal, secondValDecimal, OperatorType.NA);
            Assert.AreEqual(expected, actual);

            expected = false;
            actual = false;
            firstValDecimal = 5;
            secondValDecimal = 10;
            actual = ConditionCompareUtility.CompareParameterValues(firstValDecimal, secondValDecimal, OperatorType.GREATER_THAN);
            Assert.AreEqual(expected, actual);

            expected = false;
            actual = false;
            firstValDecimal = 5;
            secondValDecimal = 10;
            actual = ConditionCompareUtility.CompareParameterValues(firstValDecimal, secondValDecimal, OperatorType.GREATER_THAN_OR_EQUAL);
            Assert.AreEqual(expected, actual);

            expected = false;
            actual = false;
            firstValDecimal = 15;
            secondValDecimal = 10;
            actual = ConditionCompareUtility.CompareParameterValues(firstValDecimal, secondValDecimal, OperatorType.LESS_THAN);
            Assert.AreEqual(expected, actual);

            expected = false;
            actual = false;
            firstValDecimal = 15;
            secondValDecimal = 5;
            actual = ConditionCompareUtility.CompareParameterValues(firstValDecimal, secondValDecimal, OperatorType.LESS_THAN_OR_EQUAL);
            Assert.AreEqual(expected, actual);


        }


        [TestMethod()]
        public void CompareParameterValuesTestForDateTimeType()
        {
            DateTime firstValDT;
            DateTime secondValDT;
            bool expected = true;
            bool actual = true;

            firstValDT = DateTime.Now;
            secondValDT = firstValDT;
            actual = ConditionCompareUtility.CompareParameterValues(firstValDT, secondValDT, OperatorType.EQUAL);
            Assert.AreEqual(expected, actual);

            expected = false;
            actual = false;
            firstValDT = DateTime.Now;
            secondValDT = DateTime.Now.AddHours(10);
            actual = ConditionCompareUtility.CompareParameterValues(firstValDT, secondValDT, OperatorType.EQUAL);
            Assert.AreEqual(expected, actual);

            expected = true;
            actual = true;
            firstValDT = DateTime.Now;
            secondValDT = DateTime.Now.AddHours(10);
            actual = ConditionCompareUtility.CompareParameterValues(firstValDT, secondValDT, OperatorType.NOT_EQUAL);
            Assert.AreEqual(expected, actual);

            expected = false;
            actual = false;
            firstValDT = DateTime.Now;
            secondValDT = firstValDT;
            actual = ConditionCompareUtility.CompareParameterValues(firstValDT, secondValDT, OperatorType.NOT_EQUAL);
            Assert.AreEqual(expected, actual);

            expected = true;
            actual = true;
            firstValDT = DateTime.Now.AddHours(10);
            secondValDT = DateTime.Now;
            actual = ConditionCompareUtility.CompareParameterValues(firstValDT, secondValDT, OperatorType.GREATER_THAN);
            Assert.AreEqual(expected, actual);

            expected = true;
            actual = true;
            firstValDT = DateTime.Now;
            secondValDT = firstValDT;
            actual = ConditionCompareUtility.CompareParameterValues(firstValDT, secondValDT, OperatorType.GREATER_THAN_OR_EQUAL);
            Assert.AreEqual(expected, actual);

            expected = true;
            actual = true;
            firstValDT = DateTime.Now.AddHours(-10);
            secondValDT = DateTime.Now;
            actual = ConditionCompareUtility.CompareParameterValues(firstValDT, secondValDT, OperatorType.LESS_THAN);
            Assert.AreEqual(expected, actual);

            expected = true;
            actual = true;
            firstValDT = DateTime.Now;
            secondValDT = firstValDT;
            actual = ConditionCompareUtility.CompareParameterValues(firstValDT, secondValDT, OperatorType.LESS_THAN_OR_EQUAL);
            Assert.AreEqual(expected, actual);

            expected = false;
            actual = false;
            firstValDT = DateTime.Now;
            secondValDT = DateTime.Now;
            actual = ConditionCompareUtility.CompareParameterValues(firstValDT, secondValDT, OperatorType.NA);
            Assert.AreEqual(expected, actual);

            expected = false;
            actual = false;
            firstValDT = DateTime.Now.AddHours(-5);
            secondValDT = DateTime.Now;
            actual = ConditionCompareUtility.CompareParameterValues(firstValDT, secondValDT, OperatorType.GREATER_THAN);
            Assert.AreEqual(expected, actual);

            expected = false;
            actual = false;
            firstValDT = DateTime.Now.AddHours(-5);
            secondValDT = DateTime.Now;
            actual = ConditionCompareUtility.CompareParameterValues(firstValDT, secondValDT, OperatorType.GREATER_THAN_OR_EQUAL);
            Assert.AreEqual(expected, actual);

            expected = false;
            actual = false;
            firstValDT = DateTime.Now.AddHours(15);
            secondValDT = DateTime.Now;
            actual = ConditionCompareUtility.CompareParameterValues(firstValDT, secondValDT, OperatorType.LESS_THAN);
            Assert.AreEqual(expected, actual);

            expected = false;
            actual = false;
            firstValDT = DateTime.Now.AddHours(15);
            secondValDT = DateTime.Now;
            actual = ConditionCompareUtility.CompareParameterValues(firstValDT, secondValDT, OperatorType.LESS_THAN_OR_EQUAL);
            Assert.AreEqual(expected, actual);


        }


        [TestMethod()]
        public void CompareParameterValuesTestForTextType()
        {
            string firstValText;
            string secondValText;
            bool expected = true;
            bool actual = true;

            firstValText = "10";
            secondValText = "10";
            actual = ConditionCompareUtility.CompareParameterValues(firstValText, secondValText, OperatorType.EQUAL);
            Assert.AreEqual(expected, actual);

            expected = false;
            actual = false;
            firstValText = "10";
            secondValText = "20";
            actual = ConditionCompareUtility.CompareParameterValues(firstValText, secondValText, OperatorType.EQUAL);
            Assert.AreEqual(expected, actual);

            expected = true;
            actual = true;
            firstValText = "10";
            secondValText = "20";
            actual = ConditionCompareUtility.CompareParameterValues(firstValText, secondValText, OperatorType.NOT_EQUAL);
            Assert.AreEqual(expected, actual);

            expected = false;
            actual = false;
            firstValText = "10";
            secondValText = "10";
            actual = ConditionCompareUtility.CompareParameterValues(firstValText, secondValText, OperatorType.NOT_EQUAL);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod()]
        public void CompareParameterValuesTestForBoolType()
        {
            bool firstValBool;
            bool secondValBool;
            bool expected = true;
            bool actual = true;

            firstValBool = true;
            secondValBool = true;
            actual = ConditionCompareUtility.CompareParameterValues(firstValBool, secondValBool, OperatorType.EQUAL);
            Assert.AreEqual(expected, actual);

            expected = false;
            actual = false;
            firstValBool = true;
            secondValBool = false;
            actual = ConditionCompareUtility.CompareParameterValues(firstValBool, secondValBool, OperatorType.EQUAL);
            Assert.AreEqual(expected, actual);

            expected = true;
            actual = true;
            firstValBool = true;
            secondValBool = false;
            actual = ConditionCompareUtility.CompareParameterValues(firstValBool, secondValBool, OperatorType.NOT_EQUAL);
            Assert.AreEqual(expected, actual);

            expected = false;
            actual = false;
            firstValBool = true;
            secondValBool = true;
            actual = ConditionCompareUtility.CompareParameterValues(firstValBool, secondValBool, OperatorType.NOT_EQUAL);
            Assert.AreEqual(expected, actual);
        }
    }
}
