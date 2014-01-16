using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AutoFilterDataGrid.Model;

namespace EZXWPFLibrary.Utils
{
    public static class ConditionCompareUtility
    {
        public static bool CompareParameterValues<T>(T firstValue, T secondValue, OperatorType operatorType) where T : IComparable
        {
            bool status = false;
            switch (operatorType)
            {
                case OperatorType.EQUAL:
                    status = IsEquals<T>(firstValue, secondValue);
                    break;
                case OperatorType.NOT_EQUAL:
                    status = IsNotEquals<T>(firstValue, secondValue);
                    break;
                case OperatorType.GREATER_THAN:
                    status = IsGreaterThan<T>(firstValue, secondValue);
                    break;
                case OperatorType.GREATER_THAN_OR_EQUAL:
                    status = IsGreaterOrEquals<T>(firstValue, secondValue);
                    break;
                case OperatorType.LESS_THAN:
                    status = IsLessThan<T>(firstValue, secondValue);
                    break;
                case OperatorType.LESS_THAN_OR_EQUAL:
                    status = IsLessOrEquals<T>(firstValue, secondValue);
                    break;
                default:
                    status = false;
                    break;

            }
            return status;
        }

        public static bool IsGreaterThan<T>(this T value, T other) where T : IComparable
        {
            return value.CompareTo(other) > 0;
        }

        public static bool IsGreaterOrEquals<T>(this T value, T other) where T : IComparable
        {
            return value.CompareTo(other) >= 0;
        }

        public static bool IsLessThan<T>(this T value, T other) where T : IComparable
        {
            return value.CompareTo(other) < 0;
        }

        public static bool IsLessOrEquals<T>(this T value, T other) where T : IComparable
        {
            return value.CompareTo(other) <= 0;
        }

        public static bool IsNotEquals<T>(this T value, T other) where T : IComparable
        {
            return value.CompareTo(other) != 0;
        }

        public static bool IsEquals<T>(this T value, T other) where T : IComparable
        {
            return value.CompareTo(other) == 0;
        }

    }
}
