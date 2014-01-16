using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using System.Windows;
using AutoFilterDataGrid.View;
using System.Windows.Controls.Primitives;
using System.Windows.Media;
using System.Windows.Data;
using System.Collections.ObjectModel;
using AutoFilterDataGrid.Model;
using System.ComponentModel;

namespace AutoFilterDataGrid
{
    //AutofilterDataGrid MAIN
    public partial class AutofilterDataGrid : DataGrid
    {
        public event EventHandler FiterUpdated;

        public List<FilterSelectionView> ColumnFilterSelectionViewList = new List<FilterSelectionView>();

        public AutofilterDataGrid()
            : base()
        {
            _filters = new List<FilterColumn>();
        }


        protected override void OnMouseRightButtonUp(System.Windows.Input.MouseButtonEventArgs e)
        {
            if (!this.IsFilterableGrid)
            {
                return;
            }

            DependencyObject dependencyObject = (DependencyObject)e.OriginalSource;
            while ((dependencyObject != null) && !(dependencyObject is DataGridColumnHeader))
            {
                if (!(dependencyObject is Visual))
                    return;
                dependencyObject = VisualTreeHelper.GetParent(dependencyObject);
            }

            if (dependencyObject == null)
            {
                return;
            }
            if (dependencyObject is DataGridColumnHeader)
            {
                DataGridColumnHeader columnHeader = dependencyObject as DataGridColumnHeader;
                string columnName = columnHeader.Column.SortMemberPath;
                int columnIndex = columnHeader.Column.DisplayIndex;

                if (string.IsNullOrEmpty(columnName) || columnName.StartsWith("EmptyCol"))
                {
                    return;
                }

                columnIndex = ShowColumnFilterPopup(columnHeader, columnName, columnIndex);
            }
            base.OnMouseRightButtonUp(e);
        }

        private int ShowColumnFilterPopup(DataGridColumnHeader columnHeader, string columnName, int columnIndex)
        {
            FilterSelectionView existingFilterPopupView = ColumnFilterSelectionViewList.Where(f => f.SelectedColumn == columnName).FirstOrDefault();

            if (existingFilterPopupView == null)
            {
                columnIndex = GetColumnIndex(columnName);
                ObservableCollection<string> columnDataList = GetSelectedColumnAllData(columnName);
                bool isNumericColumn = IsNumericColumn(columnName);

                Point relativePoint = columnHeader.TransformToAncestor(this).Transform(new Point(0, 0));
                FilterSelectionView filterPopupView = new FilterSelectionView(this, columnName, columnDataList, columnHeader, isNumericColumn, relativePoint);
                PresentationSource ScreenPos = PresentationSource.FromVisual(this);
                filterPopupView.Left = relativePoint.X + (1 * ScreenPos.CompositionTarget.TransformToDevice.M11);
                filterPopupView.Top = relativePoint.Y + (96.0 * ScreenPos.CompositionTarget.TransformToDevice.M22);

                filterPopupView.ShowInTaskbar = false;
                filterPopupView.Topmost = true;



                filterPopupView.ShowInTaskbar = false;
                filterPopupView.Topmost = true;
                filterPopupView.Show();
                ColumnFilterSelectionViewList.Add(filterPopupView);
            }
            else
            {
                existingFilterPopupView.Topmost = true;
                existingFilterPopupView.Focus();
            }
            return columnIndex;
        }

        private bool IsNumericColumn(string columnName)
        {
            bool isNumeric = false;

            System.Collections.IEnumerator rowEnumerator = (this.ItemsSource as CollectionView).SourceCollection.GetEnumerator();
            while (rowEnumerator.MoveNext())
            {
                object rowObj = rowEnumerator.Current;
                object val = rowObj.GetType().GetProperty(columnName).GetValue(rowObj, null);

                if (val != null && (val is int || val is double || val is decimal || val is float || val is long))
                {
                    return true;
                }
                else if (val != null)
                {
                    return false;
                }
            }

            return isNumeric;
        }

        private int GetColumnIndex(string columnName)
        {
            int colIndex = 0;
            for (int i = 0; i < this.Columns.Count; i++)
            {
                if (this.Columns[i].SortMemberPath.ToUpper().Equals(columnName.ToUpper()))
                {
                    break;
                }
                colIndex++;
            }
            return colIndex;
        }

        private ObservableCollection<string> GetSelectedColumnAllData(string columnName)
        {
            ObservableCollection<string> columnDataList = new ObservableCollection<string>();

            System.Collections.IEnumerator rowEnumerator = (this.ItemsSource as CollectionView).SourceCollection.GetEnumerator();
            while (rowEnumerator.MoveNext())
            {
                object rowObj = rowEnumerator.Current;
                object val = rowObj.GetType().GetProperty(columnName).GetValue(rowObj, null);

                if (val != null && !columnDataList.Any(s => s.ToUpper() == val.ToString().ToUpper()))
                {
                    columnDataList.Add(val.ToString());
                }
            }
            
            return columnDataList;
        }

        private ObservableCollection<string> GetSelectedColumnData(int columnIndex)
        {

            ObservableCollection<string> columnDataList = new ObservableCollection<string>();
            for (int rowindex = 0; rowindex < this.Items.Count; rowindex++)
            {
                DataGridRow rowContainer = GetRow(this, rowindex);
                DataGridCell cell = GetCell(this, rowContainer, columnIndex);

                //DataGridRow rowContainer = (DataGridRow)this.ItemContainerGenerator.ContainerFromIndex(rowindex);
                //DataGridCellsPresenter presenter = GetVisualChild<DataGridCellsPresenter>(rowContainer);
                //DataGridCell cell = (DataGridCell)presenter.ItemContainerGenerator.ContainerFromIndex(columnIndex);

                if (cell != null)
                {
                    TextBlock textBlk = GetVisualChild<TextBlock>(cell);
                    if (textBlk != null && !string.IsNullOrEmpty(textBlk.Text))
                    {
                        if (!columnDataList.Any(s => s.ToUpper() == textBlk.Text.ToUpper()))
                        {
                            columnDataList.Add(textBlk.Text);
                        }
                    }
                }
            }

            for (int rowindex = (this.Items.Count-1) ; rowindex > this.Items.Count; rowindex--)
            {
                DataGridRow rowContainer = GetRow(this, rowindex);
            }
            return columnDataList;
        }


        public static T GetVisualChild<T>(Visual parent) where T : Visual
        {
            T child = default(T);
            int numVisuals = VisualTreeHelper.GetChildrenCount(parent);

            for (int i = 0; i < numVisuals; i++)
            {
                Visual v = (Visual)VisualTreeHelper.GetChild(parent, i);
                child = v as T;

                if (child == null)
                    child = GetVisualChild<T>(v);
                else
                    break;
            }

            return child;
        }

        public DataGridRow GetRow(AutofilterDataGrid grid, int index)
        {
            DataGridRow row = (DataGridRow)grid.ItemContainerGenerator.ContainerFromIndex(index);
            if (row == null)
            {
                // May be virtualized, bring into view and try again.
                grid.UpdateLayout();
                grid.ScrollIntoView(grid.Items[index]);
                row = (DataGridRow)grid.ItemContainerGenerator.ContainerFromIndex(index);
            }
            return row;
        }

        public DataGridCell GetCell(AutofilterDataGrid grid, DataGridRow row, int column)
        {
            if (row != null)
            {
                DataGridCellsPresenter presenter = GetVisualChild<DataGridCellsPresenter>(row);

                if (presenter == null)
                {
                    grid.ScrollIntoView(row, grid.Columns[column]);
                    presenter = GetVisualChild<DataGridCellsPresenter>(row);
                }

                DataGridCell cell = (DataGridCell)presenter.ItemContainerGenerator.ContainerFromIndex(column);
                return cell;
            }
            return null;
        }


        private List<FilterColumn> _filters;
        public Predicate<object> Filter { get; set; }
        public void AddFilter(FilterColumn filter)
        {
            if (_filters.Any(f=>f.ColumnName == filter.ColumnName))
            {
                FilterColumn prevFilter = _filters.Where(f => f.ColumnName == filter.ColumnName).FirstOrDefault();
                _filters.Remove(prevFilter);
            }
            _filters.Add(filter);
        }

        public void RemoveFilters()
        {
            foreach (FilterColumn filter in _filters)
            {
                if (_filters.Contains(filter))
                {
                    filter.Filter = null;
                }
            }
            _filters.Clear();
            
            //AK[3/19/2013]: Optimaize code for Datagrid Performance
            this.Items.Filter = null;
            //this.Items.Filter = InternalFilter;
            //if (this.FiterUpdated != null)
            //{
            //    this.FiterUpdated(this, EventArgs.Empty);
            //}
            SetImageForFilteredColumnsName();
        }

      
        public ObservableCollection<string> RemoveFiltersFromColumn(string columnName, DataGridColumnHeader columnHeader)
        {
            FilterColumn filter = _filters.Where(f => f.ColumnName == columnName).FirstOrDefault();
            if  (filter != null)
            {
                filter.Filter = null;
                _filters.Remove(filter);
            }

            this.Items.Filter = InternalFilter;
            int columnIndex = columnHeader.Column.DisplayIndex;
            columnIndex = GetColumnIndex(columnName);

            ObservableCollection<string> columnDataList = GetSelectedColumnAllData(columnName);
            SetImageForFilteredColumnsName();
            return columnDataList;
        }


        public static object GetPropValue(object src, string propName)
        {
            return src.GetType().GetProperty(propName).GetValue(src, null);
        }

        private bool InternalFilter(object rowObj)
        {
            if (rowObj != null)
            {
                foreach (FilterColumn filter in _filters)
                {
                    String propertyName = filter.ColumnName;
                    object value = GetPropValue(rowObj, propertyName);
                    if (value != null)
                    {
                        if (filter.IsConditionListExist)
                        {
                            if (value is int || value is double || value is decimal || value is DateTime)
                            {
                                bool status = MatchingValueInCondition(value, filter);
                                if (!status)
                                {
                                    return false;
                                }
                            }
                        }
                        else
                        {
                            if (!filter.ColumnSelectedDataList.Contains(value.ToString()))
                            {
                                if (value is double || value is decimal || value is DateTime)
                                {
                                    if (filter.ColumnSelectedDataList.Count == 0)
                                    {
                                        if (value is double || value is decimal)
                                        {
                                            filter.ColumnSelectedDataList.Add("-4545454545");
                                        }
                                        else if (value is DateTime)
                                        {
                                            DateTime dt = DateTime.MinValue.AddHours(45);
                                            filter.ColumnSelectedDataList.Add(dt.ToString());
                                        }
                                    }
                                    for (int i = 0; i < filter.ColumnSelectedDataList.Count; i++)
                                    {
                                        if (value is double)
                                        {
                                            double valueDouble;
                                            if (Double.TryParse(filter.ColumnSelectedDataList[i], out valueDouble))
                                            {
                                                if (valueDouble != (double)value)
                                                {
                                                    return false;
                                                }
                                            }
                                            else
                                            {
                                                return false;
                                            }
                                        }
                                        else if (value is decimal)
                                        {
                                            decimal valueDecimal;
                                            if (Decimal.TryParse(filter.ColumnSelectedDataList[i], out valueDecimal))
                                            {
                                                if (valueDecimal != (decimal)value)
                                                {
                                                    return false;
                                                }
                                            }
                                            else
                                            {
                                                return false;
                                            }
                                        }
                                        else if (value is DateTime)
                                        {
                                            DateTime valueDateTime;
                                            if (DateTime.TryParse(filter.ColumnSelectedDataList[i], out valueDateTime))
                                            {
                                                if (valueDateTime.TimeOfDay != ((DateTime)value).TimeOfDay)
                                                {
                                                    return false;
                                                }
                                            }
                                            else
                                            {
                                                return false;
                                            }
                                        }
                                    }
                                }
                                else
                                {
                                    return false;
                                }
                            }
                        }
                    }
                }
            }
            return true;
        }

        private bool MatchingValueInCondition(object value, FilterColumn filter)
        {
            bool? status = null;
            foreach (Model.Condition condition in filter.ConditionList)
            {
                status = condition.MatchComparission(value, status);
            }
            return status.Value;
        }


        public void FilterDataGrid(string columnName, ObservableCollection<FilterItem> filterItemList)
        {
            FilterColumn filterCol = new FilterColumn();
            filterCol.ColumnName = columnName;
            filterCol.ColumnSelectedDataList = filterItemList.Where(f => f.IsSelected == true).Select(a => a.Data).ToList();

            if (filterCol.ColumnSelectedDataList.Any(x=> x.ToUpper().Equals("SELECT ALL")))
            {
                FilterColumn filter = _filters.Where(f => f.ColumnName == columnName).FirstOrDefault();
                if (filter != null)
                {
                    filter.Filter = null;
                    _filters.Remove(filter);
                }

                this.Items.Filter = InternalFilter;
                SetImageForFilteredColumnsName();
                return;
            }

            AddFilter(filterCol);

            this.Items.Filter = InternalFilter;

            if (this.FiterUpdated != null)
            {
                this.FiterUpdated(this, EventArgs.Empty);
            }
            SetImageForFilteredColumnsName();
        }

        public void FilterDataGrid(FilterColumn filterCol)
        {
            filterCol.ColumnSelectedDataList = new List<string>();
            AddFilter(filterCol);

            this.Items.Filter = InternalFilter;

            if (this.FiterUpdated != null)
            {
                this.FiterUpdated(this, EventArgs.Empty);
            }
            SetImageForFilteredColumnsName();
        }

        public void Sort(string columnName, SortOrder sortOrder)
        {
            int selectedColumnIndex = GetColumnIndex(columnName);
            if (selectedColumnIndex >= 0)
            {
                DataGridColumn dgColumn = this.Columns[selectedColumnIndex];
                ListSortDirection sortDirection = ListSortDirection.Descending;

                if (sortOrder == SortOrder.ASC)
                {
                    sortDirection = ListSortDirection.Ascending;
                }

                if (dgColumn != null)
                {
                    foreach (DataGridColumn dgCol in this.Columns)
                    {
                        dgCol.SortDirection = null;
                    }

                    dgColumn.SortDirection = sortDirection;
                    this.Items.SortDescriptions.Clear();
                    SortDescription sd = new SortDescription(columnName, sortDirection);
                    this.Items.SortDescriptions.Add(sd);
                    this.Items.Refresh();
                }
            }



        }

        public FilterColumn GetFilterColumnByName(string colName)
        {
            return _filters.Where(f => f.ColumnName != null && f.ColumnName.Equals(colName)).FirstOrDefault();
        }
    }
}
