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

namespace QStrategyWPF.View
{
    /// <summary>
    /// Interaction logic for AlertView.xaml
    /// </summary>
    public partial class AlertView : Window
    {
        public static AlertActionReturnType AlertReturnType;
        public static string AlertReturnSymbols;
        private AlertType alertType;
        public AlertView()
        {
            InitializeComponent();
        }

        public AlertView(AlertType _alertType,string strategy, string messageLine2, bool isSummaryRowAlert)
        {
            InitializeComponent();
            AlertView.AlertReturnSymbols = string.Empty;
            if (isSummaryRowAlert)
            {
                this.rdoBtnAllSymbol.Visibility = System.Windows.Visibility.Visible;
                this.TxtMessageLine2.Visibility = System.Windows.Visibility.Visible; 
                this.rdoBtnAllSymbol.IsChecked = true;
                this.rdoBtnAllSymbol.Content = "All symbols";
                this.TxtMessageLine3.IsReadOnly = false;
                TxtMessageLine2.Text = "Strategy: " + strategy;
                TxtMessageLine3.Text = messageLine2;
            }
            else
            {
                TxtMessageLine2.Text = messageLine2;
                this.rdoBtnAllSymbol.Visibility = System.Windows.Visibility.Collapsed;
                this.TxtMessageLine3.Visibility = System.Windows.Visibility.Collapsed;
                this.rdoBtnSelectedSymbol.Visibility = System.Windows.Visibility.Collapsed;
                this.TxtMessageLine2.Visibility = System.Windows.Visibility.Visible;
                this.rdoBtnAllSymbol.Content = "All symbols";
                this.rdoBtnSelectedSymbol.IsChecked = false;
            }

            alertType = _alertType;
            AlertView.AlertReturnType = AlertActionReturnType.CANCEL;

            if (_alertType == AlertType.START)
            {
                TxtMessageLine1.Text = "Are you sure you want to start?";
                this.Title = "START";
            }
            else if (_alertType == AlertType.STOP)
            {
                TxtMessageLine1.Text = "Are you sure you want to stop?";
                this.Title = "STOP";
            }
            else if (_alertType == AlertType.CANCEL)
            {
                TxtMessageLine1.Text = "Are you sure you want to Cancel-All order?";
                this.Title = "CANCEL-ALL";
                this.btnCancel.Content = "Cancel";
            }
            else if (_alertType == AlertType.LOCK)
            {
                TxtMessageLine1.Text = "Are you sure you want to lock?";
                this.Title = "LOCK";
            }
            else if (_alertType == AlertType.UNLOCK)
            {
                TxtMessageLine1.Text = "Are you sure you want to unlock?";
                this.Title = "UNLOCK";
            }
            else if (_alertType == AlertType.BUY)
            {
                TxtMessageLine1.Text = "Are you sure you want to set Trading Mode: Buy ?";
                this.Title = "Buy";
            }
            else if (_alertType == AlertType.SELL)
            {
                TxtMessageLine1.Text = "Are you sure you want to set Trading Mode: Sell ?";
                this.Title = "Sell";
            }
            else if (_alertType == AlertType.BOTH)
            {
                TxtMessageLine1.Text = "Are you sure you want to set Trading Mode: Both ?";
                this.Title = "Both";
            }
        }

        private void btnYes_Click(object sender, RoutedEventArgs e)
        {
            if (alertType == AlertType.START || alertType == AlertType.STOP 
                || alertType == AlertType.LOCK || alertType == AlertType.UNLOCK
                || alertType == AlertType.BUY || alertType == AlertType.SELL || alertType == AlertType.BOTH)
            {
                AlertView.AlertReturnType = AlertActionReturnType.YES;
                if (this.rdoBtnAllSymbol.IsChecked != null && this.rdoBtnAllSymbol.IsChecked.Value == true)
                {
                    AlertView.AlertReturnSymbols = "ALL";
                }
                else
                {
                    AlertView.AlertReturnSymbols = this.TxtMessageLine3.Text.Trim();
                }
            }
            else if (alertType == AlertType.CANCEL)
            {
                AlertView.AlertReturnType = AlertActionReturnType.CANCEL_ALL;

                if (this.rdoBtnAllSymbol.IsChecked != null && this.rdoBtnAllSymbol.IsChecked.Value == true)
                {
                    AlertView.AlertReturnSymbols = "ALL";
                }
                else
                {
                    AlertView.AlertReturnSymbols = this.TxtMessageLine3.Text.Trim();
                }
            }
            this.DialogResult = true;
            this.Close();

        }


        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            AlertView.AlertReturnType = AlertActionReturnType.CANCEL;
            this.DialogResult = null;
            this.Close();

        }

        private void rdoBtnSelectedSymbol_Checked(object sender, RoutedEventArgs e)
        {
            this.TxtMessageLine3.IsEnabled = true;
        }

        private void rdoBtnSelectedSymbol_Unchecked(object sender, RoutedEventArgs e)
        {
            this.TxtMessageLine3.IsEnabled = false;
        }
    }

    public enum AlertActionReturnType
    {
        CANCEL,
        YES,
        NO,
        CANCEL_ALL,
    }

    public enum AlertType
    {
        START,
        STOP,
        CANCEL,
        LOCK,
        UNLOCK,
        BUY,
        SELL,
        BOTH
    }
}
