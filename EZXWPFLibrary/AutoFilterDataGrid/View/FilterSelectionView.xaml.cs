using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using AutoFilterDataGrid.ViewModel;
using System.Collections.ObjectModel;
using System.Collections;
using System.Windows.Controls.Primitives;
using AutoFilterDataGrid.Model;

namespace AutoFilterDataGrid.View
{
    /// <summary>
    /// Interaction logic for FilterSelectionView.xaml
    /// </summary>
    public partial class FilterSelectionView : Window
    {

        private string selectedColumn = string.Empty;

        public string SelectedColumn
        {
            get { return selectedColumn; }
            set { selectedColumn = value; }
        }
        private AutofilterDataGrid autofilterDataGrid;
        private DataGridColumnHeader selectedColumnHeader;
        string columnTitle = string.Empty;

        private bool isNumericColumn;

        public FilterSelectionVM VM
        {
            get 
            {
                return this.DataContext as FilterSelectionVM; 
            }
        }

        private Point relativePoints;
        public FilterSelectionView(AutofilterDataGrid _autofilterDataGrid, string columnName, ObservableCollection<string> columnDataList, DataGridColumnHeader _columnHeader, bool isNumericColumn, Point _relativePoint)
        {
            InitializeComponent();
            this.SelectedColumn = columnName;
            this.autofilterDataGrid = _autofilterDataGrid;
            this.selectedColumnHeader = _columnHeader;
            this.relativePoints = _relativePoint;


            columnTitle = columnName;
            if (_columnHeader != null && !string.IsNullOrEmpty(_columnHeader.Content as string))
            {
                columnTitle = _columnHeader.Content as string;
            }
            this.Title = string.Format("{0} - Selection", columnTitle);

            this.VM.SelectedFilterColumn = this.autofilterDataGrid.GetFilterColumnByName(columnName);

            ObservableCollection<string> sortedColumnDataList = SortData(columnDataList, isNumericColumn);

            //VM.InitFilteredData(columnName, columnDataList);
            VM.RefreshFilteredData(columnName, sortedColumnDataList, this.VM.SelectedFilterColumn);

            if (this.VM.SelectedFilterColumn == null)
            {
                this.VM.SelectedFilterColumn = new FilterColumn();
            }

            this.isNumericColumn = isNumericColumn;

            if (isNumericColumn)
            {
                this.mnuNumeric.IsEnabled = true;
            }
            else
            {
                this.mnuNumeric.IsEnabled = false;
            }

            this.Loaded += new RoutedEventHandler(FilterSelectionView_Loaded);
            this.Closed += new EventHandler(FilterSelectionView_Closed);
        }

        private ObservableCollection<string> SortData(ObservableCollection<string> columnDataList, bool IsNumericData)
        {
            ObservableCollection<string> returnedData = new ObservableCollection<string>();
            if (IsNumericData)
            {
                List<double> numberList = new List<double>();
                foreach (string numText in columnDataList)
                {
                    double num = 0;
                    if (double.TryParse(numText, out num))
                    {
                        numberList.Add(num);
                    }
                }

                foreach (double number in numberList.OrderBy(x => x).ToList())
                {
                    returnedData.Add(number.ToString());
                }
            }
            else
            {
                foreach (string text in columnDataList.OrderBy(x=>x).ToList())
                {
                    returnedData.Add(text);
                }
            }
            return returnedData;
        }

        void FilterSelectionView_Loaded(object sender, RoutedEventArgs e)
        {
            Application curApp = Application.Current;
            Window mainWindow = curApp.MainWindow;
            this.Left = mainWindow.Left + (mainWindow.Width - this.ActualWidth) / 2;
            this.Top = mainWindow.Top + (mainWindow.Height - this.ActualHeight) / 2;
        }

        void FilterSelectionView_Closed(object sender, EventArgs e)
        {
            FilterSelectionView view = this.autofilterDataGrid.ColumnFilterSelectionViewList.Where(c => c.SelectedColumn == this.SelectedColumn).FirstOrDefault();
            if (view != null)
            {
                this.autofilterDataGrid.ColumnFilterSelectionViewList.Remove(view);
            }
        }



        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (this.VM.SelectedFilterColumn == null || this.VM.SelectedFilterColumn.FilterType == FilterSelectionType.NA)
            {
                this.autofilterDataGrid.FilterDataGrid(this.selectedColumn, this.VM.FilterItemList);
            }
            this.Close();
        }

        private void CheckBox_Checked(object sender, RoutedEventArgs e)
        {
            if ((sender as CheckBox).IsChecked == true)
            {
                if ((sender as CheckBox).Content.ToString().ToUpper().Equals(EZXWPFLibrary.Properties.Resources.FILTER_SELECTALL.ToUpper()))
                {
                    this.VM.CheckAllFilterItems();
                }
                else if ((sender as CheckBox).Content.ToString().ToUpper().Equals(EZXWPFLibrary.Properties.Resources.FILTER_UNSELECTALL.ToUpper()))
                {
                    this.VM.UncheckAllFilterItems();
                }
                else
                {
                    this.VM.MarkBlankUnSelectFilterItem();
                }
            }
        }

        private void CheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            if ((sender as CheckBox).IsChecked == false)
            {
                if (!(sender as CheckBox).Content.ToString().ToUpper().Equals(EZXWPFLibrary.Properties.Resources.FILTER_UNSELECTALL.ToUpper()))
                {
                    this.VM.MarkBlankSelectFilterItem();
                }
            }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            ObservableCollection<string> columnDataList = this.autofilterDataGrid.RemoveFiltersFromColumn(selectedColumn,selectedColumnHeader);
            ObservableCollection<string> sortedColumnDataList = SortData(columnDataList, isNumericColumn);
            VM.InitFilteredData(selectedColumn, sortedColumnDataList);

        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            this.autofilterDataGrid.Sort(this.selectedColumn, SortOrder.ASC);
        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            this.autofilterDataGrid.Sort(this.selectedColumn, SortOrder.DESC);
        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {

        }

        private void mnuSortLowToHigh_Click(object sender, RoutedEventArgs e)
        {
            this.autofilterDataGrid.Sort(this.selectedColumn, SortOrder.ASC);
        }

        private void mnuSortHighToLow_Click(object sender, RoutedEventArgs e)
        {
            this.autofilterDataGrid.Sort(this.selectedColumn, SortOrder.DESC);
        }

        private void mnuClear_Click(object sender, RoutedEventArgs e)
        {
            ObservableCollection<string> columnDataList = this.autofilterDataGrid.RemoveFiltersFromColumn(selectedColumn, selectedColumnHeader);
            ObservableCollection<string> sortedColumnDataList = SortData(columnDataList, isNumericColumn);
            VM.InitFilteredData(selectedColumn, sortedColumnDataList);
        }

        private void ShowNumericView(NumericFilterSelectionType _type)
        {
            FilterColumn column = this.autofilterDataGrid.GetFilterColumnByName(this.selectedColumn);
            if (column == null)
            {
                column = new FilterColumn();
                column.ColumnName = this.selectedColumn;
            }

            NumericFilterView view = new NumericFilterView(this.selectedColumn, columnTitle, _type, column);
            bool? status = view.ShowDialog();
            if (status != null && status == true)
            {
                this.autofilterDataGrid.FilterDataGrid(column);
                this.Close();
            }
        }


        private void mnuNumeric_Click(object sender, RoutedEventArgs e)
        {
            if (this.VM.SelectedFilterColumn != null)
            {
                this.VM.RaisePropertyChanged("SelectedFilterColumn");
            }
        }

        private void mnuNumericEquals_Click(object sender, RoutedEventArgs e)
        {
            NumericFilterSelectionType _type = NumericFilterSelectionType.EQUAL;
            ShowNumericView(_type);
        }


        private void mnuNumericNotEquals_Click(object sender, RoutedEventArgs e)
        {
            NumericFilterSelectionType _type = NumericFilterSelectionType.NOT_EQUAL;
            ShowNumericView(_type);
        }

        private void mnuNumericGreaterThan_Click(object sender, RoutedEventArgs e)
        {
            NumericFilterSelectionType _type = NumericFilterSelectionType.GREATER_THAN;
            ShowNumericView(_type);
        }

        private void mnuNumericGreaterThanorEqual_Click(object sender, RoutedEventArgs e)
        {
            NumericFilterSelectionType _type = NumericFilterSelectionType.GREATER_THAN_OR_EQUALTO;
            ShowNumericView(_type);
        }

        private void mnuNumericLessThan_Click(object sender, RoutedEventArgs e)
        {
            NumericFilterSelectionType _type = NumericFilterSelectionType.LESS_THAN;
            ShowNumericView(_type);
        }

        private void mnuNumericLessThanOrEquals_Click(object sender, RoutedEventArgs e)
        {
            NumericFilterSelectionType _type = NumericFilterSelectionType.LESS_THAN_OR_EQUALTO;
            ShowNumericView(_type);
        }

        private void mnuNumericRange_Click(object sender, RoutedEventArgs e)
        {
            NumericFilterSelectionType _type = NumericFilterSelectionType.RANGE;
            ShowNumericView(_type);
        }

        private void mnuNumeric_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
        }

        private void Window_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key.GetHashCode() == 13)
            {
                this.Close();
            }
        }



    }
}
