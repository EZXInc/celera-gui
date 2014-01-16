using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EZXWPFLibrary.Helpers;
using System.Collections.ObjectModel;
using AutoFilterDataGrid.Model;
using System.Windows.Data;
using System.Collections;

namespace AutoFilterDataGrid.ViewModel
{
    public partial class FilterSelectionVM : ViewModelBase
    {
        private ObservableCollection<FilterItem> filterItemList;
        public ObservableCollection<FilterItem> FilterItemList
        {
            get { return filterItemList; }
            set
            {
                filterItemList = value;
                this.RaisePropertyChanged("FilterItemList");
            }
        }

        private FilterColumn selectedFilterColumn;
        public FilterColumn SelectedFilterColumn
        {
            get { return selectedFilterColumn; }
            set 
            { 
                selectedFilterColumn = value;
                this.RaisePropertyChanged(p => p.SelectedFilterColumn);
            }
        }


        public void InitFilteredData(string columnName, ObservableCollection<string> filterStringList)
        {
            FilterItemList = new ObservableCollection<FilterItem>();
            FilterItemList.Add(new FilterItem() { Data = EZXWPFLibrary.Properties.Resources.FILTER_SELECTALL, IsSelected = true});
            FilterItemList.Add(new FilterItem() { Data = EZXWPFLibrary.Properties.Resources.FILTER_UNSELECTALL, IsSelected = false});

            foreach (string item in filterStringList.ToList())
            {
                FilterItemList.Add(new FilterItem() { Data = item, IsSelected = true });
            }

            if (this.SelectedFilterColumn != null)
            {
                this.SelectedFilterColumn.ConditionList = new List<Condition>();
                this.SelectedFilterColumn.FilterType = FilterSelectionType.NA;
            }
            this.RaisePropertyChanged(p => p.SelectedFilterColumn);

        }

        public void RefreshFilteredData(string columnName, ObservableCollection<string> filterStringList, FilterColumn filterColumn)
        {
            FilterItemList = new ObservableCollection<FilterItem>();

            FilterItemList.Add(new FilterItem() { Data = EZXWPFLibrary.Properties.Resources.FILTER_SELECTALL, IsSelected = true });
            FilterItemList.Add(new FilterItem() { Data = EZXWPFLibrary.Properties.Resources.FILTER_UNSELECTALL, IsSelected = false });

            if (filterColumn == null || filterColumn.FilterType == FilterSelectionType.NA)
            {
                if (filterColumn == null)
                {
                    foreach (string item in filterStringList.ToList())
                    {
                        FilterItemList.Add(new FilterItem() { Data = item, IsSelected = true });
                    }
                }
                else
                {
                    foreach (string item in filterStringList.ToList())
                    {
                        if (filterColumn.ColumnSelectedDataList.Any(x => x == item))
                        {
                            FilterItemList.Add(new FilterItem() { Data = item, IsSelected = true });
                        }
                        else
                        {
                            FilterItemList.Add(new FilterItem() { Data = item, IsSelected = false });
                            FilterItemList[0].IsSelected = false;
                        }
                    }
                }

                if (this.SelectedFilterColumn != null)
                {
                    this.SelectedFilterColumn.ConditionList = new List<Condition>();
                    this.SelectedFilterColumn.FilterType = FilterSelectionType.NA;
                }
            }
            else
            {
                FilterItemList[0].IsSelected = false;
                FilterItemList[1].IsSelected = false;
                foreach (string item in filterStringList.ToList())
                {
                    FilterItemList.Add(new FilterItem() { Data = item, IsSelected = false });
                }
            }
            this.RaisePropertyChanged(p => p.SelectedFilterColumn);
        }


        public void CheckAllFilterItems()
        {
            foreach (FilterItem filterItem in this.FilterItemList)
            {
                if (!filterItem.Data.ToUpper().Equals(EZXWPFLibrary.Properties.Resources.FILTER_UNSELECTALL.ToUpper()))
                {
                    filterItem.IsSelected = true;
                }
                else
                {
                    filterItem.IsSelected = false;
                }
            }
        }
        public void UncheckAllFilterItems()
        {
            foreach (FilterItem filterItem in this.FilterItemList)
            {
                if (!filterItem.Data.ToUpper().Equals(EZXWPFLibrary.Properties.Resources.FILTER_UNSELECTALL.ToUpper()))
                {
                    filterItem.IsSelected = false;
                }
                else
                {
                    filterItem.IsSelected = true;
                }
            }
        }

        public void MarkBlankSelectFilterItem()
        {
            foreach (FilterItem filterItem in this.FilterItemList)
            {
                if (filterItem.Data.ToUpper().Equals(EZXWPFLibrary.Properties.Resources.FILTER_SELECTALL.ToUpper()))
                {
                    filterItem.IsSelected = false;
                    break;
                }
            }
        }
        public void MarkBlankUnSelectFilterItem()
        {
            foreach (FilterItem filterItem in this.FilterItemList)
            {
                if (filterItem.Data.ToUpper().Equals(EZXWPFLibrary.Properties.Resources.FILTER_UNSELECTALL.ToUpper()))
                {
                    filterItem.IsSelected = false;
                    break;
                }
            }
        }

    }
}
