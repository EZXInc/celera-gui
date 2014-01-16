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
using AutoFilterDataGrid.Model;
using System.Collections.ObjectModel;

namespace AutoFilterDataGrid.View
{
    /// <summary>
    /// Interaction logic for NumericFilterView.xaml
    /// </summary>
    public partial class NumericFilterView : Window
    {
        public NumericFilterVM VM
        {
            get
            {
                return this.DataContext as NumericFilterVM;
            }
        }

        public NumericFilterView()
        {
            InitializeComponent();
        }

        FilterColumn Column = new FilterColumn();

        public NumericFilterView(string _fieldName, string _fieldTitle, NumericFilterSelectionType _type, FilterColumn _column)
            :this()
        {
            Column = _column;
            if (_type == NumericFilterSelectionType.RANGE)
            {
                this.txtAND.Visibility = System.Windows.Visibility.Visible;
                this.Condition2Operator.Visibility = System.Windows.Visibility.Visible;
                this.Condition2Value.Visibility = System.Windows.Visibility.Visible;
            }
            else if (_type == NumericFilterSelectionType.CUSTOM)
            {
                this.SktPnlLogic.Visibility = System.Windows.Visibility.Visible;
                this.Condition2Operator.Visibility = System.Windows.Visibility.Visible;
                this.Condition2Value.Visibility = System.Windows.Visibility.Visible;
            }

            this.VM.Init(_fieldName, _fieldTitle, _type, _column);
        }

        private void btnOK_Click(object sender, RoutedEventArgs e)
        {
            bool status = ValidateField();
            if (status == true)
            {
                this.Column.ConditionList = new List<Model.Condition>();
                SetColumnFilterType();
                if (this.VM.NumericFilterType == NumericFilterSelectionType.RANGE || this.VM.NumericFilterType == NumericFilterSelectionType.CUSTOM)
                {
                    Model.Condition parentCondition = new Model.Condition(Column.ColumnName, OperatorType.NA);
                    parentCondition.LogicalOperatorOfIncludedCondition = LogicOperatorType.AND;
                    this.VM.FirstCondition.ParentCondition = parentCondition;
                    this.VM.SecondCondition.ParentCondition = parentCondition;
                    parentCondition.IncludedConditionList.Add(this.VM.FirstCondition);
                    parentCondition.IncludedConditionList.Add(this.VM.SecondCondition);
                    if (this.VM.NumericFilterType == NumericFilterSelectionType.CUSTOM)
                    {
                        if (this.rdoAND.IsChecked != null && (this.rdoAND.IsChecked.Value == true))
                        {
                            parentCondition.LogicalOperatorOfIncludedCondition = LogicOperatorType.AND;
                        }
                        else if (this.rdoOR.IsChecked != null && (this.rdoOR.IsChecked.Value == true))
                        {
                            parentCondition.LogicalOperatorOfIncludedCondition = LogicOperatorType.OR;
                        }
                        else
                        {
                            parentCondition = new Model.Condition();
                            this.VM.FirstCondition.ParentCondition = null;
                            parentCondition = this.VM.FirstCondition;
                        }
                    }
                    this.Column.ConditionList.Add(parentCondition);
                }
                else
                {
                    this.Column.ConditionList.Add(this.VM.FirstCondition);
                }
                this.DialogResult = true;
                this.Close();
            }
        }

        private void SetColumnFilterType()
        {
            this.Column.FilterType = FilterSelectionType.NUMERIC_CUST;
            if (this.VM.NumericFilterType == NumericFilterSelectionType.RANGE)
            {
                this.Column.FilterType = FilterSelectionType.NUMERIC_RNG;
            }
            else if (this.VM.FirstCondition.ConditionOperator == OperatorType.EQUAL)
            {
                this.Column.FilterType = FilterSelectionType.NUMERIC_EQ;
            }
            else if (this.VM.FirstCondition.ConditionOperator == OperatorType.NOT_EQUAL)
            {
                this.Column.FilterType = FilterSelectionType.NUMERIC_NE;
            }
            else if (this.VM.FirstCondition.ConditionOperator == OperatorType.GREATER_THAN)
            {
                this.Column.FilterType = FilterSelectionType.NUMERIC_GT;
            }
            else if (this.VM.FirstCondition.ConditionOperator == OperatorType.GREATER_THAN_OR_EQUAL)
            {
                this.Column.FilterType = FilterSelectionType.NUMERIC_GE;
            }
            else if (this.VM.FirstCondition.ConditionOperator == OperatorType.LESS_THAN)
            {
                this.Column.FilterType = FilterSelectionType.NUMERIC_LT;
            }
            else if (this.VM.FirstCondition.ConditionOperator == OperatorType.LESS_THAN_OR_EQUAL)
            {
                this.Column.FilterType = FilterSelectionType.NUMERIC_LE;
            }
        }

        private bool ValidateField()
        {
            bool status = true;
            string msg = string.Empty;
            if (string.IsNullOrEmpty(this.Condition1Value.Text))
            {
                msg = "Value could not be left blank for 1st Condition";
                status = false;
            }
            else
            {
                double tempCheckNumeric;
                if (!Double.TryParse(this.Condition1Value.Text, out tempCheckNumeric))
                {
                    msg = "Value is not numeric in 1st Condition!";
                    status = false;
                }
            }

            if (this.VM.NumericFilterType == NumericFilterSelectionType.RANGE || this.VM.NumericFilterType == NumericFilterSelectionType.CUSTOM)
            {
                if (((this.rdoAND.IsChecked != null && (this.rdoAND.IsChecked.Value == true)))
                    || ((this.rdoOR.IsChecked != null && (this.rdoOR.IsChecked.Value == true)))
                    || this.VM.NumericFilterType == NumericFilterSelectionType.RANGE)
                {
                    if (string.IsNullOrEmpty(this.Condition2Value.Text))
                    {
                        msg = msg + "\nValue could not be left blank for 2nd Condition";
                        status = false;
                    }
                    else
                    {
                        double tempCheckNumeric;
                        if (!Double.TryParse(this.Condition2Value.Text, out tempCheckNumeric))
                        {
                            msg = msg + "\nValue is not numeric in 2nd Condition!";
                            status = false;
                        }
                    }
                }
            }

            if (status == false)
            {
                MessageBox.Show(msg, "Value Error !");
            }

            return status;
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
            this.Close();
        }

        private void TextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (this.VM.FirstCondition.FieldToMatchValueType == ValueDataType.NUMERIC)
            {
                if (e.Key == Key.D0 || e.Key == Key.D1 || e.Key == Key.D2 || e.Key == Key.D3 || e.Key == Key.D4
                    || e.Key == Key.D5 || e.Key == Key.D6 || e.Key == Key.D7 || e.Key == Key.D8 || e.Key == Key.D9
                    || e.Key == Key.NumPad0 || e.Key == Key.NumPad1 || e.Key == Key.NumPad2 || e.Key == Key.NumPad3
                    || e.Key == Key.NumPad4 || e.Key == Key.NumPad5 || e.Key == Key.NumPad6 || e.Key == Key.NumPad7
                    || e.Key == Key.NumPad8 || e.Key == Key.NumPad9
                    || e.Key == Key.OemPeriod || e.Key == Key.Decimal
                    || e.Key == Key.OemMinus || e.Key == Key.Subtract)
                {
                }
                else if (e.Key == Key.Tab || e.Key == Key.Return
                || e.Key == Key.Left || e.Key == Key.LeftShift || e.Key == Key.LeftAlt || e.Key == Key.LeftCtrl
                || e.Key == Key.Right || e.Key == Key.RightShift || e.Key == Key.RightAlt || e.Key == Key.RightCtrl)
                {
                }
                else
                {
                    e.Handled = true;
                }
            }
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
