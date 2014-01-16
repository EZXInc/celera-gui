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
using System.Windows.Navigation;
using System.Windows.Shapes;
using QStrategyWPF.ViewModel;
using QStrategyGUILib;
using System.Collections.ObjectModel;
using QStrategyWPF.GUIUtils;
using System.Collections;
using System.ComponentModel;
using QStrategyWPF.Converters;
using AutoFilterDataGrid.Model;

namespace QStrategyWPF.View.QStrategyUserControls
{
    /// <summary>
    /// Interaction logic for OrderBlotterUserControl.xaml
    /// </summary>
    public partial class OrderBlotterUserControl : UserControl
    {
        public OrderBlotterUserControlVM VM
        {
            get
            {
                return this.DataContext as OrderBlotterUserControlVM;
            }
        }

        private ArrayList ColumnsNotToGenerate = new ArrayList();


        public OrderBlotterUserControl()
        {
            InitializeComponent();

            Init();
            this.VM.Init();
        }

        private void Init()
        {
            this.ckhEnableHungSymbol.IsChecked = Properties.Settings.Default.ShowHungPopup;

            AvoidManuallyCreatedColumns();
            //this.GenerateColumns(typeof(StrategyOrderInfo).GetProperties());
            this.GenerateColumns(typeof(StrategyOrderInfo).GetProperties(), this.fltdg, false);
            this.GenerateColumns(typeof(StrategyOrderInfo).GetProperties(), this.fltdgSummary, true);

            this.fltdg.FiterUpdated += new EventHandler(fltdg_FiterUpdated);
            App.AppManager.DataMgr.DataUpdateCompleted += new DataManager.DataUpdateHandler(DataMgr_DataUpdateCompleted);

            //Set filename and Directory path where configuration need to save
            this.fltdg.SetColConfigFileBaseDirectoryAndFile(App.AppManager.BaseConfigurationDirectory);

            //Listen to Column Order Update Event
            this.fltdg.ColumnUpdated += new EventHandler(dgAggregate_ColumnUpdated);

        }

        void dgAggregate_ColumnUpdated(object sender, EventArgs e)
        {
            UpdateColumnsLayout(this.fltdg, this.fltdgSummary);
        }

        void DataMgr_DataUpdateCompleted(object sender, EventArgs e)
        {
            this.UpdateSummaryRow(this.fltdg.Items);
        }

        void fltdg_FiterUpdated(object sender, EventArgs e)
        {
            this.UpdateSummaryRow(this.fltdg.Items);
        }

        public void UpdateSummaryRow(ItemCollection _itemCollection)
        {
            StrategyOrderInfo summaryRow = new StrategyOrderInfo();
            summaryRow.IsSummaryRow = true;
            summaryRow.Symbol = "Totals:";
            for (int i = 0; i < _itemCollection.Count; i++)
            {
                StrategyOrderInfo orderInfo = _itemCollection[i] as StrategyOrderInfo;
                if (orderInfo != null)
                {
                    summaryRow.Max_Loss += orderInfo.Max_Loss;
                    summaryRow.Number_Of_Open_Orders += orderInfo.Number_Of_Open_Orders;
                    summaryRow.PnL += orderInfo.PnL;
                    summaryRow.UR_PnL += orderInfo.UR_PnL;
                    summaryRow.Position_Shares += orderInfo.Position_Shares;
                    summaryRow.Position_Amount += orderInfo.Position_Amount;
                    summaryRow.Rebate_Revenue += orderInfo.Rebate_Revenue;
                    summaryRow.SeedRemaining += orderInfo.SeedRemaining;
                    summaryRow.Trading_Revenue += orderInfo.Trading_Revenue;
                    summaryRow.Volume += orderInfo.Volume;
                }
            }

            if (summaryRow.Volume != 0)
            {
                summaryRow.PnL_Per_Share = summaryRow.PnL / summaryRow.Volume;
            }
            else
            {
                summaryRow.PnL_Per_Share = 0;
            }

            App.AppManager.RunOnDispatcherThread(() =>
            {
                if (App.AppManager.DataMgr.AggregateStrategyOrderInfo.Count > 0)
                {
                    App.AppManager.DataMgr.AggregateStrategyOrderInfo[0] = summaryRow;
                }
                //App.AppManager.DataMgr.AggregateStrategyOrderInfo.Clear();
                //App.AppManager.DataMgr.AggregateStrategyOrderInfo.Add(summaryRow);
            });
        }



        private void AvoidManuallyCreatedColumns()
        {
            ColumnsNotToGenerate.Add("Strategy");
            ColumnsNotToGenerate.Add("StrategyId");
            ColumnsNotToGenerate.Add("User_Message");
            ColumnsNotToGenerate.Add("InProcess");
            ColumnsNotToGenerate.Add("IsSummaryRow");
            ColumnsNotToGenerate.Add("IsHitByMaxLoss");
        }


        private void GenerateColumns(System.Reflection.PropertyInfo[] propertyInfo, AutoFilterDataGrid.AutofilterDataGrid autofilterDataGrid, bool isSummaryRowGrid)
        {
            ObservableCollection<DataGridColumn> DataGridColumnList = new ObservableCollection<DataGridColumn>();
            foreach (System.Reflection.PropertyInfo column in propertyInfo)
            {
                //Avoid manually created columns from code
                if (ColumnsNotToGenerate.Contains(column.Name))
                {
                    continue;
                }

                bool isReadOnlyProperty = true;
                DataGridTemplateColumn dgCol = new DataGridTemplateColumn();

                // DataGridTextColumn dgCol = new DataGridTextColumn();
                dgCol.SortMemberPath = column.Name;
                dgCol.Header = GUIUtilityClass.GetTextFormRersources(column.Name, true);

                Binding binding = new Binding(column.Name);
                //dgCol.Binding = binding;
                dgCol.IsReadOnly = isReadOnlyProperty;

                Binding colorBinding = new Binding(column.Name);
                colorBinding.Converter = App.Current.Resources["NumberToColorConverter"] as NumberToColorConverter;

                dgCol.ClipboardContentBinding = new Binding(column.Name);

                string columnDataType = column.PropertyType.Name;

                FrameworkElementFactory textBlock = new FrameworkElementFactory(typeof(TextBlock));
                textBlock.SetValue(TextBlock.ForegroundProperty, new SolidColorBrush(Colors.Black));

                if (columnDataType.Contains("Int") || columnDataType.Contains("Float")
                    || columnDataType.Contains("Double") || columnDataType.Contains("Decimal")
                    || columnDataType.Contains("DateTime"))
                {
                    //dgCol.ElementStyle = FindResource("RightAlignTextBox") as Style;
                    textBlock.SetValue(TextBlock.HorizontalAlignmentProperty, HorizontalAlignment.Right);

                    textBlock.SetBinding(TextBlock.ForegroundProperty, colorBinding);
                }
                else
                {
                    textBlock.SetValue(TextBlock.HorizontalAlignmentProperty, HorizontalAlignment.Left);
                }

                if (column.Name.Equals("Symbol") && isSummaryRowGrid)
                {
                    textBlock.SetValue(TextBlock.FontWeightProperty, FontWeights.Bold);
                    textBlock.SetValue(TextBlock.HorizontalAlignmentProperty, HorizontalAlignment.Right);
                }
                else if (column.Name.Equals("Status"))
                {
                    Binding statusBackgroundColorBinding = new Binding(column.Name);
                    Binding isHitByMaxLossBackgroundColorBinding = new Binding("IsHitByMaxLoss");

                    MultiBinding statusBackgroundColorMultiBinding = new MultiBinding();
                    statusBackgroundColorMultiBinding.Bindings.Add(statusBackgroundColorBinding);
                    statusBackgroundColorMultiBinding.Bindings.Add(isHitByMaxLossBackgroundColorBinding);

                    statusBackgroundColorMultiBinding.Converter = App.Current.Resources["StrategyStatusToColorConverter"] as StrategyStatusToColorConverter;
                    statusBackgroundColorMultiBinding.ConverterParameter = "SYMBOLSTATUS";
                    textBlock.SetBinding(TextBlock.BackgroundProperty, statusBackgroundColorMultiBinding);
                    textBlock.SetValue(TextBlock.WidthProperty, 100.00);
                    textBlock.SetValue(TextBlock.ForegroundProperty, new SolidColorBrush(Colors.White));
                    dgCol.Width = 75;
                    dgCol.MinWidth = 75;
                }
                else if (column.Name.Equals("SeedRemaining"))
                {
                    textBlock.SetValue(TextBlock.HorizontalAlignmentProperty, HorizontalAlignment.Right);

                    MultiBinding multiBinding = new MultiBinding();
                    multiBinding.Converter = App.Current.Resources["SeedRemainingThresholdToColorConverter"] as SeedRemainingThresholdToColorConverter;

                    Binding seedRemainingBinding = new Binding(column.Name);
                    multiBinding.Bindings.Add(seedRemainingBinding);

                    Binding seedThresholdBinding = new Binding();
                    seedThresholdBinding.Source = App.AppManager.StgEngine;
                    seedThresholdBinding.Path = new PropertyPath("SeedQtyThreshold");
                    multiBinding.Bindings.Add(seedThresholdBinding);

                    textBlock.SetBinding(TextBlock.BackgroundProperty, multiBinding);
                    binding.StringFormat = "###,##0";

                    textBlock.SetValue(TextBlock.PaddingProperty, new Thickness(250, 0, 0, 0));
                    dgCol.Width = 120;
                }
                else if (column.Name.Equals("PnL_Per_Share"))
                {
                    binding.StringFormat = "0.00000";
                    dgCol.ClipboardContentBinding.StringFormat = "0.00000";
                }
                else if (column.Name.Equals("PnL")
                    || (column.Name.Equals("UR_PnL")
                    || column.Name.Equals("Position_Amount")
                    || column.Name.Equals("Trading_Revenue")
                    || column.Name.Equals("Rebate_Revenue")
                    || column.Name.Equals("Max_Loss")))
                {
                    binding.StringFormat = "C2";
                    dgCol.ClipboardContentBinding.StringFormat = "C2";
                }
                else if (column.Name.Equals("Volume")
                    || column.Name.Equals("Position_Shares")
                    || column.Name.Equals("SeedRemaining"))
                {
                    binding.StringFormat = "###,##0";
                }


                textBlock.SetBinding(TextBlock.TextProperty, binding);

                FrameworkElementFactory factory = new FrameworkElementFactory(typeof(Grid));
                factory.AppendChild(textBlock);
                DataTemplate cellTemplate = new DataTemplate();
                cellTemplate.VisualTree = factory;
                dgCol.CellTemplate = cellTemplate;

                autofilterDataGrid.Columns.Add(dgCol);
            }

            ManageLastColumnMinWidth();

        }


        private void Unfilter_Click(object sender, RoutedEventArgs e)
        {
            //List<string> unFilterColumn = new List<string>();
            //unFilterColumn.Add("StrategyId");
            //this.fltdg.RemoveFilters(unFilterColumn);
            this.fltdg.RemoveFilters();
            string selectedStrategyId = cmbStrategy.SelectedValue as string;
            if (selectedStrategyId != null)
            {
                ApplyStrategyIdFilter(selectedStrategyId);
            }
        }

        private void cmbStrategy_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string selectedStrategyId = (sender as ComboBox).SelectedValue as string;
            ApplyStrategyIdFilter(selectedStrategyId);
        }

        private void ApplyStrategyIdFilter(string selectedStrategyId)
        {
            if (!string.IsNullOrEmpty(selectedStrategyId))
            {
                ObservableCollection<FilterItem> filterItemList = new ObservableCollection<FilterItem>();

                if (selectedStrategyId.Equals("-1"))
                {
                    for (int i = 0; i < App.AppManager.DataMgr.StrategyList.Count; i++)
                    {
                        FilterItem selectedFilterItem = new FilterItem();
                        selectedFilterItem.IsSelected = true;
                        selectedFilterItem.Data = App.AppManager.DataMgr.StrategyList[i].StrategyId;
                        filterItemList.Add(selectedFilterItem);
                    }
                }
                else
                {
                    FilterItem selectedFilterItem = new FilterItem();
                    selectedFilterItem.IsSelected = true;
                    selectedFilterItem.Data = selectedStrategyId;
                    filterItemList.Add(selectedFilterItem);
                }
                this.fltdg.FilterDataGrid("StrategyId", filterItemList);
            }
        }

        private void AutoRefresh_Click(object sender, RoutedEventArgs e)
        {
            if (App.AppManager.AutoUpdateData)
            {
                App.AppManager.AutoUpdateData = false;
            }
            else
            {
                App.AppManager.AutoUpdateData = true;
            }

        }

        private void mnuStart_Click(object sender, RoutedEventArgs e)
        {
            Dictionary<string, List<string>> selectedStrategyAndSymbol = new Dictionary<string, List<string>>();

            GetSelectedStrategyAndSymbol(selectedStrategyAndSymbol, ProcessType.START);

            string messageTextLine2 = CreateMessageText(selectedStrategyAndSymbol);

            AlertView view = new AlertView(AlertType.START, string.Empty, messageTextLine2, false);
            bool? status = view.ShowDialog();
            if (status == true && AlertView.AlertReturnType == AlertActionReturnType.YES)
            {
                App.AppManager.Start(selectedStrategyAndSymbol, false);
            }
            else
            {
                ClearSelectedStrategyAndSymbol();
            }
        }

        private void mnuStop_Click(object sender, RoutedEventArgs e)
        {
            Dictionary<string, List<string>> selectedStrategyAndSymbol = new Dictionary<string, List<string>>();

            GetSelectedStrategyAndSymbol(selectedStrategyAndSymbol, ProcessType.STOP);

            string messageTextLine2 = CreateMessageText(selectedStrategyAndSymbol);

            AlertView view = new AlertView(AlertType.STOP, string.Empty, messageTextLine2, false);
            bool? status = view.ShowDialog();
            if (status == true && AlertView.AlertReturnType == AlertActionReturnType.YES)
            {
                App.AppManager.Stop(selectedStrategyAndSymbol, false);
            }
            else
            {
                ClearSelectedStrategyAndSymbol();
            }
        }

        private void mnuCancelAll_Click(object sender, RoutedEventArgs e)
        {
            Dictionary<string, List<string>> selectedStrategyAndSymbol = new Dictionary<string, List<string>>();
            GetSelectedStrategyAndSymbol(selectedStrategyAndSymbol, ProcessType.CANCELALL);
            string messageTextLine2 = CreateMessageText(selectedStrategyAndSymbol);

            AlertView view = new AlertView(AlertType.CANCEL,string.Empty, messageTextLine2, false);
            bool? status = view.ShowDialog();
            if (AlertView.AlertReturnType == AlertActionReturnType.CANCEL_ALL)
            {
                App.AppManager.Cancel(selectedStrategyAndSymbol, false);
            }
            else
            {
                ClearSelectedStrategyAndSymbol();
            }
        }

        private void mnuUnwind_Click(object sender, RoutedEventArgs e)
        {
            Dictionary<string, List<string>> selectedStrategyAndSymbol = new Dictionary<string, List<string>>();
            GetSelectedStrategyAndSymbol(selectedStrategyAndSymbol, ProcessType.UNWIND);
            string messageTextLine2 = CreateMessageText(selectedStrategyAndSymbol);

            App.AppManager.Unwind(selectedStrategyAndSymbol, false);
        }

        private void mnuUnLock_Click(object sender, RoutedEventArgs e)
        {
            Dictionary<string, List<string>> selectedStrategyAndSymbol = new Dictionary<string, List<string>>();

            GetSelectedStrategyAndSymbol(selectedStrategyAndSymbol, ProcessType.UNLOCK);

            //string messageTextLine2 = CreateMessageText(selectedStrategyAndSymbol);

            //AlertView view = new AlertView(AlertType.UNLOCK, messageTextLine2);
            //bool? status = view.ShowDialog();
            //if (status == true && AlertView.AlertReturnType == AlertActionReturnType.YES)
            //{
                App.AppManager.Unlock(selectedStrategyAndSymbol, false);
            //}
            //else
            //{
            //    ClearSelectedStrategyAndSymbol();
            //}

        }

        private void mnuLock_Click(object sender, RoutedEventArgs e)
        {
            Dictionary<string, List<string>> selectedStrategyAndSymbol = new Dictionary<string, List<string>>();

            GetSelectedStrategyAndSymbol(selectedStrategyAndSymbol, ProcessType.LOCK);

            //string messageTextLine2 = CreateMessageText(selectedStrategyAndSymbol);

            //AlertView view = new AlertView(AlertType.LOCK, messageTextLine2);
            //bool? status = view.ShowDialog();
            //if (status == true && AlertView.AlertReturnType == AlertActionReturnType.YES)
            //{
                App.AppManager.Lock(selectedStrategyAndSymbol, false);
            //}
            //else
            //{
            //    ClearSelectedStrategyAndSymbol();
            //}

        }

        private void GetSelectedStrategyAndSymbol(Dictionary<string, List<string>> selectedStrategyAndSymbol, ProcessType _processType)
        {
            if (fltdg.SelectedItems != null)
            {
                for (int i = 0; i < fltdg.SelectedItems.Count; i++)
                {
                    StrategyOrderInfo orderInfo = fltdg.SelectedItems[i] as StrategyOrderInfo;
                    if (orderInfo != null)
                    {
                        if (_processType == ProcessType.BUY || _processType == ProcessType.SELL || _processType == ProcessType.BOTH)
                        {
                            if (orderInfo.Status.Equals("Trading"))
                            {
                                orderInfo.InProcess = true;
                                if (!selectedStrategyAndSymbol.ContainsKey(orderInfo.StrategyId))
                                {
                                    selectedStrategyAndSymbol[orderInfo.StrategyId] = new List<string>();
                                }
                                selectedStrategyAndSymbol[orderInfo.StrategyId].Add(orderInfo.Symbol);
                            }
                        }
                        else if (_processType == ProcessType.UNLOCK)
                        {
                            if (orderInfo.Status.Equals("Locked"))
                            {
                                orderInfo.InProcess = true;
                                if (!selectedStrategyAndSymbol.ContainsKey(orderInfo.StrategyId))
                                {
                                    selectedStrategyAndSymbol[orderInfo.StrategyId] = new List<string>();
                                }
                                selectedStrategyAndSymbol[orderInfo.StrategyId].Add(orderInfo.Symbol);
                            }
                        }
                        else if (_processType == ProcessType.START)
                        {
                            if (!orderInfo.Status.Equals("Trading") && !orderInfo.Status.Equals("Locked"))
                            {
                                orderInfo.InProcess = true;
                                if (!selectedStrategyAndSymbol.ContainsKey(orderInfo.StrategyId))
                                {
                                    selectedStrategyAndSymbol[orderInfo.StrategyId] = new List<string>();
                                }
                                selectedStrategyAndSymbol[orderInfo.StrategyId].Add(orderInfo.Symbol);
                            }
                        }
                        else if (_processType == ProcessType.STOP)
                        {
                            if (!orderInfo.Status.Equals("Stopped") && !orderInfo.Status.Equals("Locked"))
                            {
                                orderInfo.InProcess = true;
                                if (!selectedStrategyAndSymbol.ContainsKey(orderInfo.StrategyId))
                                {
                                    selectedStrategyAndSymbol[orderInfo.StrategyId] = new List<string>();
                                }
                                selectedStrategyAndSymbol[orderInfo.StrategyId].Add(orderInfo.Symbol);
                            }
                        }
                        else if (orderInfo.Status.Equals("Locked"))
                        {
                            orderInfo.InProcess = true;
                            if (!selectedStrategyAndSymbol.ContainsKey(orderInfo.StrategyId))
                            {
                                selectedStrategyAndSymbol[orderInfo.StrategyId] = new List<string>();
                            }
                            selectedStrategyAndSymbol[orderInfo.StrategyId].Add(orderInfo.Symbol);
                        }
                        else
                        {
                            orderInfo.InProcess = true;
                            if (!selectedStrategyAndSymbol.ContainsKey(orderInfo.StrategyId))
                            {
                                selectedStrategyAndSymbol[orderInfo.StrategyId] = new List<string>();
                            }
                            selectedStrategyAndSymbol[orderInfo.StrategyId].Add(orderInfo.Symbol);
                        }
                    }
                }
            }
        }

        private void ClearSelectedStrategyAndSymbol()
        {
            if (fltdg.SelectedItems != null)
            {
                for (int i = 0; i < fltdg.SelectedItems.Count; i++)
                {
                    StrategyOrderInfo orderInfo = fltdg.SelectedItems[i] as StrategyOrderInfo;
                    if (orderInfo != null)
                    {
                        orderInfo.InProcess = false;
                    }
                }
            }
        }

        private static string CreateMessageText(Dictionary<string, List<string>> selectedStrategyAndSymbol)
        {
            string messageTextLine2 = string.Empty;
            foreach (string key in selectedStrategyAndSymbol.Keys)
            {
                List<string> symbolList = selectedStrategyAndSymbol[key];
                string symbolText = EZXWPFLibrary.Utils.StringUtils.StringListToText(symbolList, ", ");
                string strategyName = App.AppManager.DataMgr.StrategyList.Where(s => s.StrategyId == key).Select(s => s.StrategyName).FirstOrDefault();
                messageTextLine2 = string.Format("{0}Strategy: {1}, Symbol(s): {2}\n", messageTextLine2, strategyName, symbolText);
            }
            return messageTextLine2;
        }

        #region Column-Resize and Reordring

        private void fltdg_Loaded(object sender, RoutedEventArgs e)
        {
            RegisterForColumnWidthsChanged(this.fltdg);
            UpdateColumnsLayout(this.fltdg, this.fltdgSummary);


            DataGrid dg = sender as DataGrid;
            Border border = VisualTreeHelper.GetChild(dg, 0) as Border;
            ScrollViewer scrollViewer = VisualTreeHelper.GetChild(border, 0) as ScrollViewer;
            Grid grid = VisualTreeHelper.GetChild(scrollViewer, 0) as Grid;
            Button buttonSelectAll = VisualTreeHelper.GetChild(grid, 0) as Button;

            if (buttonSelectAll != null && buttonSelectAll.Command != null && buttonSelectAll.Command == DataGrid.SelectAllCommand)
            {
                buttonSelectAll.Click += new RoutedEventHandler(buttonSelectAll_Click);
            }   
        }

        void buttonSelectAll_Click(object sender, RoutedEventArgs e)
        {
            this.fltdg.Focus();
        }

        private int selectedReorderingColumnIndex;

        private void fltdg_ColumnReordering(object sender, DataGridColumnReorderingEventArgs e)
        {
            selectedReorderingColumnIndex = e.Column.DisplayIndex;
        }

        private void fltdg_ColumnReordered(object sender, DataGridColumnEventArgs e)
        {
            foreach (DataGridColumn column in this.fltdgSummary.Columns)
            {
                if (column.DisplayIndex == selectedReorderingColumnIndex)
                {
                    column.DisplayIndex = e.Column.DisplayIndex;
                    break;
                }                
            }

            ManageLastColumnMinWidth();

            selectedReorderingColumnIndex = 0;

        }

        private void ManageLastColumnMinWidth()
        {
            int lastColumnIndex = fltdg.Columns.Count - 1;
            foreach (DataGridColumn column in this.fltdg.Columns)
            {
                if (column.DisplayIndex == lastColumnIndex)
                {
                    double colMinWidth = GUIUtilityClass.GetColumnMinWidth(column.SortMemberPath);
                    if (colMinWidth > 30)
                    {
                        column.MinWidth = colMinWidth;
                        if (column.Width.Value < column.MinWidth)
                        {
                            column.Width = column.MinWidth;
                        }
                    }
                }
                else
                {
                    column.MinWidth = 15;
                }
            }
        }

        public void RegisterForColumnWidthsChanged(DataGrid dg)
        {
            foreach (var col in dg.Columns)
            {
                DependencyPropertyDescriptor dpd = DependencyPropertyDescriptor.FromProperty(DataGridColumn.ActualWidthProperty, typeof(DataGridColumn));
                if (dpd != null)
                {
                    dpd.AddValueChanged(col, OnWidthChanged);
                }
            }
        }

        public void OnWidthChanged(object sender, EventArgs ea)
        {
            ManageLastColumnMinWidth();

            DataGridColumn col = sender as DataGridColumn;
            UpdateColumnsLayout(this.fltdg, this.fltdgSummary);
        }

        private void UpdateColumnsLayout(AutoFilterDataGrid.AutofilterDataGrid fltdg, AutoFilterDataGrid.AutofilterDataGrid fltdgSummary)
        {
            try
            {
                for (int colIndex = 0; (colIndex <= fltdg.Columns.Count && colIndex <= fltdgSummary.Columns.Count); colIndex++)
                {
                    if (((colIndex + 1) <= fltdg.Columns.Count) && ((colIndex + 1) <= fltdgSummary.Columns.Count) && (fltdgSummary.Columns[colIndex] != null) && (fltdg.Columns[colIndex] != null))
                    {
                        if (colIndex == 0)
                        {
                            fltdg.Columns[colIndex].Width = fltdgSummary.Columns[colIndex].Width;
                        }
                        else
                        {
                            if (fltdgSummary.Columns != null && fltdgSummary.Columns[colIndex] != null)
                            {
                                fltdgSummary.Columns[colIndex].Width = fltdg.Columns[colIndex].Width;
                            }
                        }

                        fltdgSummary.Columns[colIndex].Visibility = fltdg.Columns[colIndex].Visibility;
                        if (fltdgSummary.Columns[colIndex].DisplayIndex >= 0 && fltdg.Columns[colIndex].DisplayIndex >= 0)
                        {
                            fltdgSummary.Columns[colIndex].DisplayIndex = fltdg.Columns[colIndex].DisplayIndex;
                        }
                    }
                    else
                    {
                        return;
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

        private void lockUnlockRowButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void fltdg_SelectedCellsChanged(object sender, SelectedCellsChangedEventArgs e)
        {
            if (this.fltdg.SelectedItems != null && this.fltdg.SelectedItems.Count > 0)
            {
                App.AppManager.DataMgr.SelectedSymbolOrderInfoList = new List<StrategyOrderInfo>();
                for (int x = 0; x < this.fltdg.SelectedItems.Count; x++)
                {
                    StrategyOrderInfo order = this.fltdg.SelectedItems[x] as StrategyOrderInfo;
                    if (order != null)
                    {
                        App.AppManager.DataMgr.SelectedSymbolOrderInfoList.Add(order);
                    }
                }
            }
            else
            {
                App.AppManager.DataMgr.SelectedSymbolOrderInfoList = new List<StrategyOrderInfo>();
            }
        }

        private void ckhEnableHungSymbol_Checked(object sender, RoutedEventArgs e)
        {
            Properties.Settings.Default.ShowHungPopup = true;
            Properties.Settings.Default.Save();
        }

        private void ckhEnableHungSymbol_Unchecked(object sender, RoutedEventArgs e)
        {
            Properties.Settings.Default.ShowHungPopup = false;
            Properties.Settings.Default.Save();

        }

        private void btnBuy_Click(object sender, RoutedEventArgs e)
        {
            Dictionary<string, List<string>> selectedStrategyAndSymbol = new Dictionary<string, List<string>>();
            this.fltdg.SelectAll();
            GetSelectedStrategyAndSymbol(selectedStrategyAndSymbol, ProcessType.BUY);
            string messageTextLine2 = CreateMessageText(selectedStrategyAndSymbol);
            this.fltdg.UnselectAll();
            App.AppManager.Buy(selectedStrategyAndSymbol, false);
        }

        private void btnSell_Click(object sender, RoutedEventArgs e)
        {
            Dictionary<string, List<string>> selectedStrategyAndSymbol = new Dictionary<string, List<string>>();
            this.fltdg.SelectAll();
            GetSelectedStrategyAndSymbol(selectedStrategyAndSymbol, ProcessType.SELL);
            string messageTextLine2 = CreateMessageText(selectedStrategyAndSymbol);
            this.fltdg.UnselectAll();
            App.AppManager.Sell(selectedStrategyAndSymbol, false);
        }

        private void btnBoth_Click(object sender, RoutedEventArgs e)
        {
            Dictionary<string, List<string>> selectedStrategyAndSymbol = new Dictionary<string, List<string>>();
            this.fltdg.SelectAll();
            GetSelectedStrategyAndSymbol(selectedStrategyAndSymbol, ProcessType.BOTH);
            string messageTextLine2 = CreateMessageText(selectedStrategyAndSymbol);
            this.fltdg.UnselectAll();
            App.AppManager.Both(selectedStrategyAndSymbol, false);
        }

        private void mnuBuy_Click(object sender, RoutedEventArgs e)
        {
            Dictionary<string, List<string>> selectedStrategyAndSymbol = new Dictionary<string, List<string>>();
            GetSelectedStrategyAndSymbol(selectedStrategyAndSymbol, ProcessType.BUY);
            string messageTextLine2 = CreateMessageText(selectedStrategyAndSymbol);
            App.AppManager.Buy(selectedStrategyAndSymbol, false);
        }

        private void mnuSell_Click(object sender, RoutedEventArgs e)
        {
            Dictionary<string, List<string>> selectedStrategyAndSymbol = new Dictionary<string, List<string>>();
            GetSelectedStrategyAndSymbol(selectedStrategyAndSymbol, ProcessType.SELL);
            string messageTextLine2 = CreateMessageText(selectedStrategyAndSymbol);
            App.AppManager.Sell(selectedStrategyAndSymbol, false);

        }

        private void mnuBoth_Click(object sender, RoutedEventArgs e)
        {
            Dictionary<string, List<string>> selectedStrategyAndSymbol = new Dictionary<string, List<string>>();
            GetSelectedStrategyAndSymbol(selectedStrategyAndSymbol, ProcessType.BOTH);
            string messageTextLine2 = CreateMessageText(selectedStrategyAndSymbol);
            App.AppManager.Both(selectedStrategyAndSymbol, false);
        }
    }
}
