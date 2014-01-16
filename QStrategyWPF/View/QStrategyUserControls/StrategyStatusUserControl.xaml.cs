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
using System.Collections;
using QStrategyWPF.GUIUtils;
using System.Collections.ObjectModel;
using QStrategyWPF.Converters;
using QStrategyGUILib;

namespace QStrategyWPF.View.QStrategyUserControls
{
    /// <summary>
    /// Interaction logic for StrategyStatusView.xaml
    /// </summary>
    public partial class StrategyStatusUserControl : UserControl
    {
        private StrategyStatusVM VM
        {
            get
            {
                return (this.DataContext as StrategyStatusVM);
            }
        }
        private ArrayList ColumnsNotToGenerate = new ArrayList();

        public StrategyStatusUserControl()
        {
            InitializeComponent();
        }

        public void Init(string strategyStatus)
        {
            this.VM.Init(strategyStatus);

            AvoidManuallyCreatedColumns();
            //this.GenerateColumns(typeof(StrategyOrderInfo).GetProperties());
            this.GenerateColumns(typeof(StrategyOrderInfo).GetProperties(), this.fltdgStrategyStatus, false);

            //Set filename and Directory path where configuration need to save
            this.fltdgStrategyStatus.SetColConfigFileBaseDirectoryAndFile(App.AppManager.BaseConfigurationDirectory);
        }

        private void AvoidManuallyCreatedColumns()
        {
            ColumnsNotToGenerate.Add("Strategy");
            ColumnsNotToGenerate.Add("StrategyId");
            ColumnsNotToGenerate.Add("User_Message");
            ColumnsNotToGenerate.Add("InProcess");
            ColumnsNotToGenerate.Add("IsSummaryRow");
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

                dgCol.ClipboardContentBinding = new Binding(column.Name);

                Binding colorBinding = new Binding(column.Name);
                colorBinding.Converter = App.Current.Resources["NumberToColorConverter"] as NumberToColorConverter;

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
                }
                else if (column.Name.Equals("PnL")
                    || (column.Name.Equals("UR_PnL")
                    || column.Name.Equals("Position_Amount")
                    || column.Name.Equals("Trading_Revenue")
                    || column.Name.Equals("Rebate_Revenue")
                    || column.Name.Equals("Max_Loss")))
                {
                    binding.StringFormat = "C2";
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

        private void ManageLastColumnMinWidth()
        {
            int lastColumnIndex = fltdgStrategyStatus.Columns.Count - 1;
            foreach (DataGridColumn column in this.fltdgStrategyStatus.Columns)
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

        private void Unfilter_Click(object sender, RoutedEventArgs e)
        {

        }

        private void SelectAll_Click(object sender, RoutedEventArgs e)
        {
            this.fltdgStrategyStatus.SelectAll();
        }

        private void Start_Click(object sender, RoutedEventArgs e)
        {

            if (fltdgStrategyStatus.SelectedItems == null || fltdgStrategyStatus.SelectedItems.Count == 0)
            {
                return;
            }

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

        private void GetSelectedStrategyAndSymbol(Dictionary<string, List<string>> selectedStrategyAndSymbol, ProcessType _processType)
        {
            if (this.fltdgStrategyStatus.SelectedItems != null)
            {
                for (int i = 0; i < fltdgStrategyStatus.SelectedItems.Count; i++)
                {
                    StrategyOrderInfo orderInfo = fltdgStrategyStatus.SelectedItems[i] as StrategyOrderInfo;
                    if (orderInfo != null)
                    {
                        if (_processType == ProcessType.UNLOCK)
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
            if (fltdgStrategyStatus.SelectedItems != null)
            {
                for (int i = 0; i < fltdgStrategyStatus.SelectedItems.Count; i++)
                {
                    StrategyOrderInfo orderInfo = fltdgStrategyStatus.SelectedItems[i] as StrategyOrderInfo;
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

    }
}
