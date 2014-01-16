using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AutoFilterDataGrid.Model
{
    public partial class FilterColumn
    {
        private int id;
        private FilterSelectionType filterType = FilterSelectionType.NA;

        private string columnName;
        private List<string> columnSelectedDataList;

        private Predicate<object> _filter;
        private object currentRowDataValue;

        public int Id
        {
            get { return id; }
            set { id = value; }
        }
        public FilterSelectionType FilterType
        {
            get { return filterType; }
            set { filterType = value; }
        } 

        public string ColumnName
        {
            get { return columnName; }
            set { columnName = value; }
        }
        public List<string> ColumnSelectedDataList
        {
            get { return columnSelectedDataList; }
            set { columnSelectedDataList = value; }
        }


        public object CurrentRowDataValue
        {
            get { return currentRowDataValue; }
            set { currentRowDataValue = value; }
        }
        public Predicate<object> Filter
        {
            get { return _filter; }
            set { _filter = value; }
        }


        private List<Condition> conditionList = new List<Condition>();
        public List<Condition> ConditionList
        {
            get { return conditionList; }
            set { conditionList = value; }
        }

        public bool IsConditionListExist
        {
            get
            {
                if (this.ConditionList != null && ConditionList.Count > 0)
                {
                    return true;
                }
                return false;
            }
        }
    
    }
}
