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
using System.Collections;
using QStrategyWPF.Model;
using System.Collections.ObjectModel;
using QStrategyWPF.GUIUtils;
using System.ComponentModel;
using QStrategyWPF.Converters;
using QStrategyGUILib;
using System.Windows.Threading;


namespace QStrategyWPF.View.QStrategyUserControls
{
    /// <summary>
    /// Interaction logic for SummaryUserControl.xaml
    /// </summary>
    public partial class SummaryUserControl : UserControl
    {
        private ArrayList ColumnsNotToGenerate = new ArrayList();

        public SummaryUserControl()
        {
            InitializeComponent();
            Init();
        }

        private void Init()
        {
            AvoidManuallyCreatedColumns();
            this.GenerateColumns(typeof(SummaryOrder).GetProperties(), this.dgAggregate, false);
            this.GenerateColumns(typeof(SummaryOrder).GetProperties(), this.dgAggregateSummary, true);

            this.dgAggregate.FiterUpdated += new EventHandler(dgAggregate_FiterUpdated);
            App.AppManager.DataMgr.DataUpdateCompleted += new DataManager.DataUpdateHandler(DataMgr_DataUpdateCompleted);

            //Set filename and Directory path where configuration need to save
            this.dgAggregate.SetColConfigFileBaseDirectoryAndFile(App.AppManager.BaseConfigurationDirectory);

            //Listen to Column Order Update Event
            this.dgAggregate.ColumnUpdated += new EventHandler(dgAggregate_ColumnUpdated);

        }


        void dgAggregate_ColumnUpdated(object sender, EventArgs e)
        {
            UpdateColumnsLayout(this.dgAggregate, this.dgAggregateSummary);
        }

        void DataMgr_DataUpdateCompleted(object sender, EventArgs e)
        {
            this.UpdateSummaryRow(this.dgAggregate.Items);
        }

        void dgAggregate_FiterUpdated(object sender, EventArgs e)
        {
            this.UpdateSummaryRow(this.dgAggregate.Items);
        }

        public void UpdateSummaryRow(ItemCollection _itemCollection)
        {
            SummaryOrder summaryRow = new SummaryOrder();
            summaryRow.IsAggregatedRow = true;
            summaryRow.StrategyName = "Totals:";
            summaryRow.Status = string.Empty;
            for (int i = 0; i < _itemCollection.Count; i++)
            {
                SummaryOrder orderInfo = _itemCollection[i] as SummaryOrder;
                if (orderInfo != null)
                {
                    summaryRow.OpenOrders += orderInfo.OpenOrders;
                    summaryRow.PnL += orderInfo.PnL;
                    summaryRow.UR_PnL += orderInfo.UR_PnL;
                    summaryRow.PositionShares += orderInfo.PositionShares;
                    summaryRow.PositionAmount += orderInfo.PositionAmount;
                    summaryRow.RebateRevenue += orderInfo.RebateRevenue;
                    summaryRow.TradingRevenue += orderInfo.TradingRevenue;
                    summaryRow.Volume += orderInfo.Volume;
                    summaryRow.TotalCount += orderInfo.TotalCount;
                    summaryRow.TradingCount += orderInfo.TradingCount;
                    summaryRow.MaxLoss += orderInfo.MaxLoss;
                    summaryRow.SeedRemaining += orderInfo.SeedRemaining;
                }
            }

            summaryRow.SymbolTrading = string.Format("{0} of {1}", summaryRow.TradingCount, summaryRow.TotalCount);

            if (summaryRow.Volume != 0)
            {
                summaryRow.PnLPerShares = summaryRow.PnL / summaryRow.Volume;
            }
            else
            {
                summaryRow.PnLPerShares = 0;
            }

            App.AppManager.RunOnDispatcherThread(() =>
            {
                if (App.AppManager.DataMgr.AggregateSummaryInfo.Count > 0)
                {
                    App.AppManager.DataMgr.AggregateSummaryInfo[0] = summaryRow;
                }
                //App.AppManager.DataMgr.AggregateSummaryInfo.Clear();
                //App.AppManager.DataMgr.AggregateSummaryInfo.Add(summaryRow);
            });
        }


        private void AvoidManuallyCreatedColumns()
        {
            ColumnsNotToGenerate.Add("StrategyId");
            ColumnsNotToGenerate.Add("TradingCount");
            ColumnsNotToGenerate.Add("TotalCount");
            ColumnsNotToGenerate.Add("IsAggregatedRow");
            ColumnsNotToGenerate.Add("SeedRemaining");
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

                dgCol.SortMemberPath = column.Name;
                dgCol.Header = GUIUtilityClass.GetTextFormRersources(column.Name, true);

                Binding binding = new Binding(column.Name);
                dgCol.IsReadOnly = isReadOnlyProperty;


                dgCol.ClipboardContentBinding = new Binding(column.Name);


                string columnDataType = column.PropertyType.Name;

                FrameworkElementFactory textBlock = new FrameworkElementFactory(typeof(TextBlock));
                textBlock.SetValue(TextBlock.ForegroundProperty, new SolidColorBrush(Colors.Black));

                if (columnDataType.Contains("Int") || columnDataType.Contains("Float")
                    || columnDataType.Contains("Double") || columnDataType.Contains("Decimal")
                    || columnDataType.Contains("DateTime"))
                {
                    Binding colorBinding = new Binding(column.Name);
                    colorBinding.Converter = App.Current.Resources["NumberToColorConverter"] as NumberToColorConverter;
                    textBlock.SetValue(TextBlock.HorizontalAlignmentProperty, HorizontalAlignment.Right);
                    textBlock.SetBinding(TextBlock.ForegroundProperty, colorBinding);
                }
                else
                {
                    textBlock.SetValue(TextBlock.HorizontalAlignmentProperty, HorizontalAlignment.Left);
                }

                if (column.Name.Equals("StrategyName") && isSummaryRowGrid)
                {
                    textBlock.SetValue(TextBlock.FontWeightProperty, FontWeights.Bold);
                    textBlock.SetValue(TextBlock.HorizontalAlignmentProperty, HorizontalAlignment.Right);
                }
                else if (column.Name.Equals("Status"))
                {
                    Binding statusBackgroundColorBinding = new Binding(column.Name);
                    statusBackgroundColorBinding.Converter = App.Current.Resources["EngineStatusToStatusColorConverter"] as EngineStatusToStatusColorConverter;
                    statusBackgroundColorBinding.ConverterParameter = "SUMMARYSTATUS";
                    textBlock.SetBinding(TextBlock.BackgroundProperty, statusBackgroundColorBinding);
                    textBlock.SetValue(TextBlock.WidthProperty, 60.00);
                    textBlock.SetValue(TextBlock.ForegroundProperty, new SolidColorBrush(Colors.White));
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

                    textBlock.SetValue(TextBlock.PaddingProperty, new Thickness(50, 0, 0, 0));
                }
                else if (column.Name.Equals("SymbolTrading"))
                {
                    textBlock.SetValue(TextBlock.HorizontalAlignmentProperty, HorizontalAlignment.Center);
                }
                else if (column.Name.Equals("PnLPerShares"))
                {
                    binding.StringFormat = "0.00000";
                }
                else if (column.Name.Equals("PnL")
                    || column.Name.Equals("UR_PnL") 
                    || column.Name.Equals("PositionAmount")
                    || column.Name.Equals("TradingRevenue")
                    || column.Name.Equals("RebateRevenue")
                    || column.Name.Equals("MaxLoss"))
                {
                    binding.StringFormat = "C2";
                }
                else if (column.Name.Equals("Volume") || column.Name.Equals("PositionShares"))
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
        }


        private void startStopRowButton_Click(object sender, RoutedEventArgs e)
        {
            App.AppManager.DataMgr.ClearProcessSelectionIndication();
            AlertType alertType = AlertType.START;
            if (dgAggregate.SelectedItem != null)
            {
                if (dgAggregate.SelectedItem is SummaryOrder)
                {
                    SummaryOrder order = dgAggregate.SelectedItem as SummaryOrder;
                    string key = order.StrategyId;
                    string strategy = order.StrategyName;

                    Dictionary<string, List<string>> selectedStrategyAndSymbol = new Dictionary<string, List<string>>();


                    bool? status = true;
                    AlertView.AlertReturnType = AlertActionReturnType.YES;
                    if (order.Status.Equals("Running"))
                    {
                        alertType = AlertType.STOP;

                        GetSelectedStrategyAndSymbol(key, selectedStrategyAndSymbol, alertType);

                        string messageTextLine3 = CreateMessageText(selectedStrategyAndSymbol);
                        string messageTextLine2 = string.Format("{0}", App.AppManager.DataMgr.StrategyList.Where(s => s.StrategyId == key).Select(s => s.StrategyName).FirstOrDefault());


                        AlertView view = new AlertView(alertType, messageTextLine2, messageTextLine3, true);
                        status = view.ShowDialog();
                    }
                    else
                    {
                        GetSelectedStrategyAndSymbol(key, selectedStrategyAndSymbol, alertType);
                    }

                    if (status == true && AlertView.AlertReturnType == AlertActionReturnType.YES)
                    {
                        if (alertType == AlertType.START)
                        {
                            App.AppManager.Start(selectedStrategyAndSymbol, true);
                            order.Status = App.AppManager.StgEngine.StrategyEngineStatus;
                        }
                        else
                        {
                            if (!string.IsNullOrEmpty(AlertView.AlertReturnSymbols))
                            {
                                if (AlertView.AlertReturnSymbols.Equals("ALL"))
                                {
                                    App.AppManager.Stop(selectedStrategyAndSymbol, true);
                                    order.Status = App.AppManager.StgEngine.StrategyEngineStatus;
                                }
                                else
                                {
                                    selectedStrategyAndSymbol = new Dictionary<string, List<string>>();
                                    selectedStrategyAndSymbol[key] = EZXWPFLibrary.Utils.StringUtils.StringTextToList(AlertView.AlertReturnSymbols, ',');
                                    App.AppManager.Stop(selectedStrategyAndSymbol, false);
                                    order.Status = App.AppManager.StgEngine.StrategyEngineStatus;
                                }
                            }
                            else
                            {
                                MessageBox.Show("No symbol is selected");
                            }
                        }
                    }
                    else
                    {
                        ClearSelectedStrategyAndSymbol(key);
                    }
                }
            }
        }

        private void cancelAllRowButton_Click(object sender, RoutedEventArgs e)
        {
            App.AppManager.DataMgr.ClearProcessSelectionIndication();
            AlertType alertType = AlertType.CANCEL;
            if (dgAggregate.SelectedItem != null)
            {
                if (dgAggregate.SelectedItem is SummaryOrder)
                {
                    SummaryOrder order = dgAggregate.SelectedItem as SummaryOrder;
                    string key = order.StrategyId;

                    Dictionary<string, List<string>> selectedStrategyAndSymbol = new Dictionary<string, List<string>>();

                    GetSelectedStrategyAndSymbol(key, selectedStrategyAndSymbol, AlertType.CANCEL);

                    string messageTextLine3 = CreateMessageText(selectedStrategyAndSymbol);
                    string messageTextLine2 = string.Format("{0}", App.AppManager.DataMgr.StrategyList.Where(s => s.StrategyId == key).Select(s => s.StrategyName).FirstOrDefault());

                    AlertView view = new AlertView(alertType, messageTextLine2, messageTextLine3, true);
                    bool? status = view.ShowDialog();
                    if (AlertView.AlertReturnType == AlertActionReturnType.CANCEL_ALL)
                    {
                        if (!string.IsNullOrEmpty(AlertView.AlertReturnSymbols))
                        {
                            if (AlertView.AlertReturnSymbols.Equals("ALL"))
                            {
                                App.AppManager.Cancel(selectedStrategyAndSymbol, true);
                            }
                            else
                            {
                                selectedStrategyAndSymbol = new Dictionary<string, List<string>>();
                                selectedStrategyAndSymbol[key] = EZXWPFLibrary.Utils.StringUtils.StringTextToList(AlertView.AlertReturnSymbols, ',');
                                App.AppManager.Cancel(selectedStrategyAndSymbol, false);
                            }
                        }
                        else
                        {
                            MessageBox.Show("No symbol is selected");
                        }
                    }
                    else
                    {
                        ClearSelectedStrategyAndSymbol(key);
                    }
                }
            }

        }

        private void lockUnLockRowButton_Click(object sender, RoutedEventArgs e)
        {
            App.AppManager.DataMgr.ClearProcessSelectionIndication();
            AlertType alertType = AlertType.LOCK;
            if (dgAggregate.SelectedItem != null)
            {
                if (dgAggregate.SelectedItem is SummaryOrder)
                {
                    SummaryOrder order = dgAggregate.SelectedItem as SummaryOrder;
                    string key = order.StrategyId;

                    if (order.Status.Equals("Lock"))
                    {
                        alertType = AlertType.UNLOCK;
                    }
                    Dictionary<string, List<string>> selectedStrategyAndSymbol = new Dictionary<string, List<string>>();

                    GetSelectedStrategyAndSymbol(key, selectedStrategyAndSymbol, alertType);
                    string messageTextLine3 = CreateMessageText(selectedStrategyAndSymbol);

                    string messageTextLine2 = string.Format("{0}", App.AppManager.DataMgr.StrategyList.Where(s => s.StrategyId == key).Select(s => s.StrategyName).FirstOrDefault());

                    AlertView view = new AlertView(alertType, messageTextLine2, messageTextLine3, true);
                    bool? status = view.ShowDialog();
                    if (status == true && AlertView.AlertReturnType == AlertActionReturnType.YES)
                    {
                        if (alertType == AlertType.LOCK)
                        {
                            App.AppManager.Lock(selectedStrategyAndSymbol, true);
                        }
                        else
                        {
                            App.AppManager.Unlock(selectedStrategyAndSymbol, true);
                        }
                    }
                    else
                    {
                        ClearSelectedStrategyAndSymbol(key);
                    }
                }
            }

        }

        private void unwindRowButton_Click(object sender, RoutedEventArgs e)
        {
            App.AppManager.DataMgr.ClearProcessSelectionIndication();
        }

        private void GetSelectedStrategyAndSymbol(string strategyId, Dictionary<string, List<string>> selectedStrategyAndSymbol, AlertType _alertType)
        {
            if (App.AppManager.DataMgr.StrategyOrderDictionary.ContainsKey(strategyId))
            {
                selectedStrategyAndSymbol[strategyId] = new List<string>();
                selectedStrategyAndSymbol[strategyId] = App.AppManager.DataMgr.StrategyOrderDictionary[strategyId].Keys.ToList();
                foreach (string symbol in App.AppManager.DataMgr.StrategyOrderDictionary[strategyId].Keys)
                {
                    if ((_alertType == AlertType.STOP && 
                        (App.AppManager.DataMgr.StrategyOrderDictionary[strategyId][symbol].Status == "Stopped")
                        || App.AppManager.DataMgr.StrategyOrderDictionary[strategyId][symbol].Status == "Locked"))
                    {
                        selectedStrategyAndSymbol[strategyId].Remove(symbol);
                        continue;
                    }
                    App.AppManager.DataMgr.StrategyOrderDictionary[strategyId][symbol].InProcess = true;
                }


            }
        }

        private void ClearSelectedStrategyAndSymbol(string strategyId)
        {
            foreach (string symbol in App.AppManager.DataMgr.StrategyOrderDictionary[strategyId].Keys)
            {
                App.AppManager.DataMgr.StrategyOrderDictionary[strategyId][symbol].InProcess = false;
            }
        }

        private static string CreateMessageText(Dictionary<string, List<string>> selectedStrategyAndSymbol)
        {
            string messageTextLine2 = string.Empty;
            int x = 0;
            foreach (string key in selectedStrategyAndSymbol.Keys)
            {
                List<string> symbolList = selectedStrategyAndSymbol[key];
                string symbolText = EZXWPFLibrary.Utils.StringUtils.StringListToText(symbolList, ", ");
                string strategyName = App.AppManager.DataMgr.StrategyList.Where(s => s.StrategyId == key).Select(s => s.StrategyName).FirstOrDefault();
                if (x == 0)
                {
                    messageTextLine2 = symbolText;
                }
                else
                {
                    messageTextLine2 = string.Format("{0},{1}", messageTextLine2, symbolText);
                }
            }
            return messageTextLine2;
        }


        #region Column-Resize and Reordring

        private void dgAggregate_Loaded(object sender, RoutedEventArgs e)
        {
            RegisterForColumnWidthsChanged(this.dgAggregate);
            UpdateColumnsLayout(this.dgAggregate, this.dgAggregateSummary);

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
            App.AppManager.DataMgr.ClearProcessSelectionIndication();
            this.dgAggregate.Focus();
        }

        private int selectedReorderingColumnIndex;

        private void dgAggregate_ColumnReordering(object sender, DataGridColumnReorderingEventArgs e)
        {
            selectedReorderingColumnIndex = e.Column.DisplayIndex;
        }

        private void dgAggregate_ColumnReordered(object sender, DataGridColumnEventArgs e)
        {
            foreach (DataGridColumn column in this.dgAggregateSummary.Columns)
            {
                if (column.DisplayIndex == selectedReorderingColumnIndex)
                {
                    column.DisplayIndex = e.Column.DisplayIndex;
                    break;
                }
            }

            selectedReorderingColumnIndex = 0;

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
            DataGridColumn col = sender as DataGridColumn;
            UpdateColumnsLayout(this.dgAggregate, this.dgAggregateSummary);
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
    }
}
