using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AutoFilterDataGrid.Model;

namespace AutoFilterDataGrid.ViewModel
{
    public partial class NumericFilterVM : ViewModelBase
    {
        private string fieldName;
        private string fieldNameTitle;
        private Condition firstCondition;
        private Condition secondCondition;
        private NumericFilterSelectionType numericFilterType = NumericFilterSelectionType.CUSTOM;
        private LogicOperatorType selectedLogicOperator;

        public List<ConditionOperatorType> OperatorList
        {
            get
            {
                List<ConditionOperatorType> opList = new List<ConditionOperatorType>();
                ConditionOperatorType cOpt1 = new ConditionOperatorType() { Operation = OperatorType.EQUAL, OperatorTypeText = "is equal to" };
                ConditionOperatorType cOpt2 = new ConditionOperatorType() { Operation = OperatorType.NOT_EQUAL, OperatorTypeText = "is not equal to" };
                ConditionOperatorType cOpt3 = new ConditionOperatorType() { Operation = OperatorType.GREATER_THAN, OperatorTypeText = "is greater than" };
                ConditionOperatorType cOpt4 = new ConditionOperatorType() { Operation = OperatorType.GREATER_THAN_OR_EQUAL, OperatorTypeText = "is greater than or equal to" };
                ConditionOperatorType cOpt5 = new ConditionOperatorType() { Operation = OperatorType.LESS_THAN, OperatorTypeText = "is less than" };
                ConditionOperatorType cOpt6 = new ConditionOperatorType() { Operation = OperatorType.LESS_THAN_OR_EQUAL, OperatorTypeText = "is less than or equal to" };
                opList.Add(cOpt1);
                opList.Add(cOpt2);
                opList.Add(cOpt3);
                opList.Add(cOpt4);
                opList.Add(cOpt5);
                opList.Add(cOpt6);

                return opList;                
            }
        }


        public string FieldName
        {
            get { return fieldName; }
            set
            {
                fieldName = value;
                this.RaisePropertyChanged("FieldName");
            }
        }
        public string FieldNameTitle
        {
            get { return fieldNameTitle; }
            set
            {
                fieldNameTitle = value;
                this.RaisePropertyChanged("FieldNameTitle");
            }
        }

        public Condition FirstCondition
        {
            get { return firstCondition; }
            set 
            { 
                firstCondition = value;
                this.RaisePropertyChanged("FirstCondition");
            }
        }
        public Condition SecondCondition
        {
            get { return secondCondition; }
            set
            {
                secondCondition = value;
                this.RaisePropertyChanged("SecondCondition");
            }
        }
        public NumericFilterSelectionType NumericFilterType
        {
            get { return numericFilterType; }
            set 
            { 
                numericFilterType = value;
                this.RaisePropertyChanged("NumericFilterType");
            }
        }
        public LogicOperatorType SelectedLogicOperator
        {
            get { return selectedLogicOperator; }
            set 
            { 
                selectedLogicOperator = value;
                this.RaisePropertyChanged("SelectedLogicOperator");
            }
        }

        public NumericFilterVM()
            : this(string.Empty, NumericFilterSelectionType.CUSTOM)
       {
       }


        public NumericFilterVM(string columnName, NumericFilterSelectionType _filterType)
            : base()
        {

        }

        public void Init(string columnName, string columnTitle, NumericFilterSelectionType _filterType, FilterColumn _column)
        {
            this.NumericFilterType = _filterType;
            this.FieldName = columnName;
            this.FieldNameTitle = columnTitle;
            bool previouslyExist = false;
            if (_filterType == NumericFilterSelectionType.RANGE)
            {
                if (_column.FilterType == FilterSelectionType.NUMERIC_RNG)
                {
                    previouslyExist = true;
                }
                FirstCondition = new Condition(columnName, OperatorType.GREATER_THAN_OR_EQUAL);
                SecondCondition = new Condition(columnName, OperatorType.LESS_THAN_OR_EQUAL);
            }
            else if (_filterType == NumericFilterSelectionType.EQUAL)
            {
                if (_column.FilterType == FilterSelectionType.NUMERIC_EQ)
                {
                    previouslyExist = true;
                } 
                FirstCondition = new Condition(columnName, OperatorType.EQUAL);
            }
            else if (_filterType == NumericFilterSelectionType.NOT_EQUAL)
            {
                if (_column.FilterType == FilterSelectionType.NUMERIC_NE)
                {
                    previouslyExist = true;
                } 
                FirstCondition = new Condition(columnName, OperatorType.NOT_EQUAL);
            }
            else if (_filterType == NumericFilterSelectionType.GREATER_THAN)
            {
                if (_column.FilterType == FilterSelectionType.NUMERIC_GT)
                {
                    previouslyExist = true;
                }
                FirstCondition = new Condition(columnName, OperatorType.GREATER_THAN);
            }
            else if (_filterType == NumericFilterSelectionType.GREATER_THAN_OR_EQUALTO)
            {
                if (_column.FilterType == FilterSelectionType.NUMERIC_GE)
                {
                    previouslyExist = true;
                }
                FirstCondition = new Condition(columnName, OperatorType.GREATER_THAN_OR_EQUAL);
            }
            else if (_filterType == NumericFilterSelectionType.LESS_THAN)
            {
                if (_column.FilterType == FilterSelectionType.NUMERIC_LT)
                {
                    previouslyExist = true;
                }
                FirstCondition = new Condition(columnName, OperatorType.LESS_THAN);
            }
            else if (_filterType == NumericFilterSelectionType.LESS_THAN_OR_EQUALTO)
            {
                if (_column.FilterType == FilterSelectionType.NUMERIC_LE)
                {
                    previouslyExist = true;
                }
                FirstCondition = new Condition(columnName, OperatorType.LESS_THAN_OR_EQUAL);
            }
            else if (_filterType == NumericFilterSelectionType.CUSTOM)
            {
                if (_column.FilterType == FilterSelectionType.NUMERIC_CUST)
                {
                    previouslyExist = true;
                }
                FirstCondition = new Condition(columnName, OperatorType.GREATER_THAN_OR_EQUAL);
                SecondCondition = new Condition(columnName, OperatorType.GREATER_THAN_OR_EQUAL);
            }

            if (previouslyExist && _column.ConditionList != null && _column.ConditionList.Count > 0)
            {
                if (_filterType == NumericFilterSelectionType.RANGE || _filterType == NumericFilterSelectionType.CUSTOM)
                {
                    if (_column.ConditionList[0].IsLogicalOperatorOfIncludedConditionSpecified == true &&
                            _column.ConditionList[0].IncludedConditionList != null
                            && _column.ConditionList[0].IncludedConditionList.Count > 1)
                        FirstCondition = _column.ConditionList[0].IncludedConditionList[0];
                    SecondCondition = _column.ConditionList[0].IncludedConditionList[1];
                }
                else
                {
                    FirstCondition = _column.ConditionList[0];
                }
            }

            if (FirstCondition != null)
            {
                FirstCondition.FieldToMatchValueType = ValueDataType.NUMERIC;
            }

            if (SecondCondition != null)
            {
                SecondCondition.FieldToMatchValueType = ValueDataType.NUMERIC;
            }
        }
    }

    public class ConditionOperatorType
    {
        private string operatorTypeText;
        private OperatorType operation;

        public string OperatorTypeText
        {
            get { return operatorTypeText; }
            set { operatorTypeText = value; }
        }
        public OperatorType Operation
        {
            get { return operation; }
            set { operation = value; }
        }

        public ConditionOperatorType()
        {
        }

        public ConditionOperatorType(OperatorType operatorType, string text)
        {
            this.Operation = operatorType;
            this.OperatorTypeText = text;
        }

    }
}
