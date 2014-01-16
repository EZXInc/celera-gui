using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EZXWPFLibrary.AutoFilterDataGrid.Model;
using EZXWPFLibrary.Utils;
using System.Windows.Controls;
using EZXWPFLibrary.AutoFilterDataGrid.Controller;
using System.Windows;
using System.ComponentModel;
using System.Windows.Media.Imaging;
using System.Windows.Media;
using AutoFilterDataGrid.Model;
using System.Windows.Controls.Primitives;
using System.Windows.Input;

namespace AutoFilterDataGrid
{
    //EXTENSION: AutofilterDataGrid_Ext_ColumnOrdering.cs
    public partial class AutofilterDataGrid
    {
        public event EventHandler ColumnUpdated;

        // Using a DependencyProperty IsColumnConfigurationIncluded
        public bool IsColumnConfigurationIncluded
        {
            get { return (bool)GetValue(IsColumnConfigurationIncludedProperty); }
            set { SetValue(IsColumnConfigurationIncludedProperty, value); }
        }
        public static readonly DependencyProperty IsColumnConfigurationIncludedProperty =
            DependencyProperty.Register("IsColumnConfigurationIncluded", typeof(bool), typeof(DataGrid), new PropertyMetadata(false, OnIsColumnConfigurationIncludedChanged));
        private static void OnIsColumnConfigurationIncludedChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            AutofilterDataGrid autofilterDataGrid = sender as AutofilterDataGrid;
            if (autofilterDataGrid.IsColumnConfigurationIncluded) 
            {
                autofilterDataGrid.InitializeConfigurationController();
            }
        }


        // Using a DependencyProperty IsColumnConfigurationIncluded
        public bool IsFilterableGrid
        {
            get { return (bool)GetValue(IsFilterableGridProperty); }
            set { SetValue(IsFilterableGridProperty, value); }
        }
        public static readonly DependencyProperty IsFilterableGridProperty =
            DependencyProperty.Register("IsFilterableGrid", typeof(bool), typeof(DataGrid), new PropertyMetadata(false, OnIsFilterableGridChanged));
        private static void OnIsFilterableGridChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
        }
        
        
        
        private ColumnsConfigurationController columnsConfigurationController;
        public ColumnsConfigurationController ColumnsConfigurationController
        {
            get { return columnsConfigurationController; }
            set { columnsConfigurationController = value; }
        }
        

        private void UpdateColumnDisplay(List<ColumnsConfigInfo> colsConfigList)
        {
            LogUtil.WriteLog(LogLevel.DEBUG, "AutofilterDataGrid.UpdateColumnDisplay(List<ColumnsConfigInfo>) started");

            foreach (DataGridColumn col in Columns.Reverse())
            {
                ColumnsConfigInfo columnConfigInfo = GetColumnConfig(col, colsConfigList);
                if (columnConfigInfo != null)
                {
                    if ((columnConfigInfo.ColumnDisplayIndex >= 0) && (Columns.Count > columnConfigInfo.ColumnDisplayIndex))
                    {
                        col.DisplayIndex = columnConfigInfo.ColumnDisplayIndex;
                        col.Visibility = columnConfigInfo.ColumnVisibility;
                        if (columnConfigInfo.ColumnWidth.ColumnWidthType == LengthType.AUTO)
                        {
                            col.Width = new DataGridLength(1, DataGridLengthUnitType.Auto);
                        }
                        else if (columnConfigInfo.ColumnWidth.ColumnWidthType == LengthType.UNIT)
                        {
                            col.Width = new DataGridLength(columnConfigInfo.ColumnWidth.Width.Value);
                        }
                        else
                        {
                            col.Width = new DataGridLength(1, DataGridLengthUnitType.SizeToCells);
                        }
                    }
                }
            }

            LogUtil.WriteLog(LogLevel.DEBUG, "AutofilterDataGrid.UpdateColumnDisplay(List<ColumnsConfigInfo>) finished");

        }

        private ColumnsConfigInfo GetColumnConfig(DataGridColumn col, List<ColumnsConfigInfo> colsConfigList)
        {
            LogUtil.WriteLog(LogLevel.DEBUG, "AutofilterDataGrid.GetColumnConfig() started");

            ColumnsConfigInfo columnsConfigInfo = colsConfigList.Where(columnConfig => columnConfig.ColumnSortMemberPath == col.SortMemberPath && col.SortMemberPath != "").FirstOrDefault();
            if (columnsConfigInfo != null)
            {
                return columnsConfigInfo;
            }
            columnsConfigInfo = colsConfigList.Where(columnConfig => columnConfig.ColumnHeader == col.Header.ToString()).FirstOrDefault();
            if (columnsConfigInfo != null)
            {
                LogUtil.WriteLog(LogLevel.DEBUG, "AutofilterDataGrid.GetColumnConfig() finished with return columnsConfigInfo");
                return columnsConfigInfo;
            }

            LogUtil.WriteLog(LogLevel.DEBUG, "AutofilterDataGrid.GetColumnConfig() finished with return = NULL");
            return null;
        }

        public void UpdateColumnDisplay(string gridName)
        {
            LogUtil.WriteLog(LogLevel.DEBUG, "AutofilterDataGrid.UpdateColumnDisplay(gridName) ...");
            if (IsColumnConfigurationIncluded)
            {
                UpdateColumnDisplay(ColumnsConfigurationController.GetColumnsConfigurationList(gridName));
                if (this.ColumnUpdated != null)
                {
                    this.ColumnUpdated(this, EventArgs.Empty);
                }
            }
        }

        public void InitializeConfigurationController()
        {
            this.Loaded += new RoutedEventHandler(AutofilterDataGrid_Loaded);
            this.ColumnReordered += new EventHandler<DataGridColumnEventArgs>(AutofilterDataGrid_ColumnReordered);

            this.ColumnsConfigurationController = new ColumnsConfigurationController();
        }


        public void SetColConfigFileBaseDirectoryAndFile(string baseDirectory)
        {
            SetColConfigFileBaseDirectoryAndFile(baseDirectory, this.Name + "ColumnConfig.xml");
        }

        public void SetColConfigFileBaseDirectoryAndFile(string baseDirectory, string fileName)
        {
            if (this.IsColumnConfigurationIncluded)
            {
                ColumnsConfigurationController.BaseDirectory = baseDirectory;
                ColumnsConfigurationController.FileName = fileName;
            }
            else
            {
                throw new Exception("DataGrid Property[IsColumnConfigurationIncluded] must be define with True value, to SetColConfigFileBaseDirectoryAndFile");
            }
        }

        public string GetColConfigFileBaseDirectory()
        {
            return ColumnsConfigurationController.BaseDirectory;
        }

        void AutofilterDataGrid_Loaded(object sender, RoutedEventArgs e)
        {
            ColumnsConfigurationController.LoadColumnsConfigurationController();

            if (!string.IsNullOrEmpty(this.Name))
            {
                SetValueForEmptyColumnsName();

                SetImageForFilteredColumnsName();

                UpdateColumnDisplay(this.Name);
            }
            else
            {
                throw new Exception("AutofilterDataGrid Name property should assign to include if column-config need to persist");
            }            
        }

        private void SetImageForFilteredColumnsName()
        {
            foreach (DataGridColumn col in this.Columns.Where(c => c.SortMemberPath != null || c.SortMemberPath.Trim() != string.Empty).ToList())
            {

                if (col.SortMemberPath.StartsWith("EmptyCol"))
                {
                    continue;
                }
                System.Windows.DataTemplate template = new System.Windows.DataTemplate();
                template.DataType = typeof(HeaderedContentControl);

                //System.Windows.FrameworkElementFactory blockFactory = new System.Windows.FrameworkElementFactory(typeof(StackPanel));
                //blockFactory.SetValue(StackPanel.OrientationProperty, Orientation.Horizontal);
                //blockFactory.SetValue(StackPanel.OrientationProperty, this.Columns[col.DisplayIndex].SortMemberPath);
                //blockFactory.SetValue(Button.ToolTipProperty, this.Columns[col.DisplayIndex].SortMemberPath);
                
                
                //template.VisualTree = blockFactory;

                //this.Columns[col.DisplayIndex].HeaderTemplate = template;
                
                string headerText = col.Header.ToString();
                if (string.IsNullOrEmpty(headerText))
                {
                    headerText = col.SortMemberPath;
                }


                if (_filters != null && _filters.Count > 0 && _filters.Any(f => f.ColumnName == col.SortMemberPath))
                {
                    FilterColumn filter = _filters.Where(f => f.ColumnName == col.SortMemberPath).FirstOrDefault();
                    //if (filter.ColumnSelectedDataList.Where)

                    FrameworkElementFactory blockFactory = new FrameworkElementFactory(typeof(StackPanel));
                    blockFactory.SetValue(StackPanel.OrientationProperty, Orientation.Horizontal);

                    //blockFactory.SetValue(StackPanel.BackgroundProperty, Brushes.Beige);
                    //blockFactory.SetValue(StackPanel.MarginProperty, new Thickness(0));

                    FrameworkElementFactory _txtBlkChild = new FrameworkElementFactory(typeof(TextBlock));
                    _txtBlkChild.SetValue(TextBlock.TextProperty, headerText);
                    _txtBlkChild.SetValue(TextBlock.TextWrappingProperty, TextWrapping.Wrap);

                    BitmapImage bmp = new BitmapImage(new Uri(@"filterImage1.png", UriKind.Relative ));
                    FrameworkElementFactory icon = new FrameworkElementFactory(typeof(Image));
                    icon.SetValue(Image.SourceProperty, bmp);
                    icon.SetValue(Image.WidthProperty, 12.0);
                    icon.SetValue(Image.HeightProperty, 12.0);
                    icon.SetValue(Image.MarginProperty, new Thickness(0, 0, 5, 0));

                    icon.SetValue(Image.CursorProperty, System.Windows.Input.Cursors.Hand);
                    icon.AddHandler(Image.MouseLeftButtonDownEvent, new System.Windows.Input.MouseButtonEventHandler(FilterImage_MouseLeftButtonDownEvent));

                    blockFactory.AppendChild(icon);
                    blockFactory.AppendChild(_txtBlkChild);

                    template.VisualTree = blockFactory;

                    col.HeaderTemplate = template;
                }
                else
                {
                    col.HeaderTemplate = null;
                    col.Header = headerText;
                }
            }

        }

        private void FilterImage_MouseLeftButtonDownEvent(object sender, EventArgs e)
        {
            if (!this.IsFilterableGrid)
            {
                return;
            }

            DependencyObject dependencyObject = (DependencyObject)sender;
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
        }

        private void SetValueForEmptyColumnsName()
        {
            foreach(DataGridColumn col in this.Columns.Where(c=> c.SortMemberPath == null || c.SortMemberPath.Trim() == string.Empty).ToList())
            {
                this.Columns[col.DisplayIndex].SortMemberPath = "EmptyCol"+col.DisplayIndex;
            }
        }
        
        void AutofilterDataGrid_ColumnReordered(object sender, DataGridColumnEventArgs e)
        {
            if (!string.IsNullOrEmpty(this.Name))
            {
                UpdateColumnConfigurationList(this.Name);
                this.ColumnsConfigurationController.Save();
            }
            else
            {
                throw new Exception("AutofilterDataGrid Name property should assign to include if column-config need to persist");
            }
        }

        public void SaveColumnsConfiguration()
        {
            if (IsColumnConfigurationIncluded)
            {
                ColumnsConfigurationController.Save();
            }
        }

        internal void UpdateColumnConfigurationList(string gridName)
        {
            LogUtil.WriteLog(LogLevel.DEBUG, "AutofilterDataGrid.RMSDataGrid.UpdateColumnConfigurationList(formName) ...");

            ColumnsConfigurationController.SetColumnsConfigurationList(gridName, this);
        }
    }
}
