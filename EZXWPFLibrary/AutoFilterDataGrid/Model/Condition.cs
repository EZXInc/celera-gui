using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EZXWPFLibrary.Helpers;
using System.Collections.ObjectModel;

namespace AutoFilterDataGrid.Model
{
    public partial class Condition : ObservableBase
    {
        private string fieldName;
        private string fieldNameToMatch;
        private string fieldToMatchValue;
        private ValueDataType fieldToMatchValueType = ValueDataType.TEXT;
        private OperatorType conditionOperator;
        private LogicOperatorType logicalOperatorOfIncludedCondition;
        private ObservableCollection<Condition> includedConditionList;
        private Condition parentCondition;

        public string FieldName
        {
            get { return fieldName; }
            set 
            { 
                fieldName = value;
                this.RaisePropertyChanged(p => p.FieldName);
            }
        }
        public string FiledNameToMatch
        {
            get { return fieldNameToMatch; }
            set 
            { 
                fieldNameToMatch = value;
                this.RaisePropertyChanged(p => p.FiledNameToMatch); 
            }
        }
        public string FieldToMatchValue
        {
            get { return fieldToMatchValue; }
            set
            {
                fieldToMatchValue = value;
                this.RaisePropertyChanged(p => p.FieldToMatchValue);
            }
        }
        public ValueDataType FieldToMatchValueType
        {
            get { return fieldToMatchValueType; }
            set
            {
                fieldToMatchValueType = value;
                this.RaisePropertyChanged(p => p.FieldToMatchValueType);
            }
        }
        public OperatorType ConditionOperator
        {
            get { return conditionOperator; }
            set
            {
                conditionOperator = value;
                this.RaisePropertyChanged(p => p.ConditionOperator);
            }
        }
        public LogicOperatorType LogicalOperatorOfIncludedCondition
        {
            get { return logicalOperatorOfIncludedCondition; }
            set
            {
                logicalOperatorOfIncludedCondition = value;
                this.RaisePropertyChanged(p => p.LogicalOperatorOfIncludedCondition);
            }
        }
        public ObservableCollection<Condition> IncludedConditionList
        {
            get { return includedConditionList; }
            set
            {
                includedConditionList = value;
                this.RaisePropertyChanged(p => p.IncludedConditionList);
            }
        }
        public Condition ParentCondition
        {
            get { return parentCondition; }
            set
            {
                parentCondition = value; 
                this.RaisePropertyChanged(p => p.ParentCondition);
            }
        }

        public bool IsConditionOperatorSpecified
        {
            get
            {
                if (this.ConditionOperator != OperatorType.NA)
                {
                    return true;
                }
                return false;
            }
        }
        public bool IsLogicalOperatorOfIncludedConditionSpecified
        {
            get
            {
                if (this.IncludedConditionList != null && this.IncludedConditionList.Count > 0)
                {
                    return true;
                }
                return false;
            }
        }
        public bool IsParentConditionExist
        {
            get
            {
                if (this.ParentCondition != null)
                {
                    return true;
                }
                return false;
            }
        }

        public Condition()
        {
            this.LogicalOperatorOfIncludedCondition = LogicOperatorType.NA;
            this.IncludedConditionList = new ObservableCollection<Condition>();
            this.FiledNameToMatch = string.Empty;
        }

        public Condition(string _filedName):this()
        {
            this.FieldName = _filedName;
        }

        public Condition(string _filedName, OperatorType _conditionOperator)
            : this(_filedName)
        {
            this.ConditionOperator = _conditionOperator;
        }


        public bool? MatchComparission(object value, bool? status)
        {
            if (this.IsLogicalOperatorOfIncludedConditionSpecified)
            {
                foreach (Condition condition in this.IncludedConditionList)
                {
                    bool? previousStatus = status;
                    status = condition.MatchComparission(value, status);
                    if (condition.IsParentConditionExist)
                    {
                        if (previousStatus != null)
                        {
                            if (status != null)
                            {
                                if (condition.parentCondition.LogicalOperatorOfIncludedCondition == LogicOperatorType.AND)
                                {
                                    status = previousStatus.Value && status.Value;
                                }
                                else if (condition.parentCondition.LogicalOperatorOfIncludedCondition == LogicOperatorType.OR)
                                {
                                    status = previousStatus.Value || status.Value;
                                }
                            }
                            else
                            {
                                status = previousStatus;
                            }
                        }
                    }
                }
            }
            else
            {
                if (this.IsConditionOperatorSpecified)
                {
                    if (this.ConditionOperator == OperatorType.EXISTS)
                    {
                        if (value != null)
                        {
                            status = true;
                        }
                        else
                        {
                            status = false;
                        }
                    }
                    else if (this.ConditionOperator == OperatorType.NOT_EXISTS)
                    {
                        if (value != null)
                        {
                            status = false;
                        }
                        else
                        {
                            status = true;
                        }
                    }
                    else
                    {
                        string ValueInCondition = string.Empty;
                        object ValueInRow = value;
                        if (!string.IsNullOrEmpty(this.FieldToMatchValue))
                        {
                            ValueInCondition = this.FieldToMatchValue;
                        }

                        status = CompareValues(ValueInRow, ValueInCondition, this.ConditionOperator);
                    }
                }
            }
            return status;
        }

        private bool CompareValues(object ValueInRow,string ValueInCondition, OperatorType operatorType)
        {
            bool status = false;

            if (this.FieldToMatchValueType == ValueDataType.NUMERIC)
            {
                try
                {
                    double SecondValueInDouble = double.Parse(ValueInCondition);
                    double firstValueInDouble = double.MaxValue;
                    if (ValueInRow != null && !string.IsNullOrEmpty(ValueInRow.ToString()))
                    {
                        Double.TryParse(ValueInRow.ToString(),out firstValueInDouble);
                    }
                    return EZXWPFLibrary.Utils.ConditionCompareUtility.CompareParameterValues(firstValueInDouble, SecondValueInDouble, operatorType);
                }
                catch (Exception ex)
                {
                    EZXWPFLibrary.Utils.LogUtil.WriteLog(EZXWPFLibrary.Utils.LogLevel.WARN, ex.Message + "\n" + ex.StackTrace);
                    return false;
                }
            }
            else if (this.FieldToMatchValueType == ValueDataType.DATETIME)
            {
                try
                {
                    DateTime secondValueInDateTime = DateTime.Parse(ValueInCondition);
                    DateTime firstValueInDateTime = (DateTime)ValueInRow;
                    return EZXWPFLibrary.Utils.ConditionCompareUtility.CompareParameterValues(firstValueInDateTime, secondValueInDateTime, operatorType);
                }
                catch (Exception ex)
                {
                    EZXWPFLibrary.Utils.LogUtil.WriteLog(EZXWPFLibrary.Utils.LogLevel.WARN, ex.Message + "\n" + ex.StackTrace);
                    return false;
                }
            }
            else if (this.FieldToMatchValueType == ValueDataType.TEXT)
            {
                string ValueInRowString = ValueInRow as string;
                return EZXWPFLibrary.Utils.ConditionCompareUtility.CompareParameterValues(ValueInRowString, ValueInCondition, operatorType);
            }


            return status;
        }
    }
}
