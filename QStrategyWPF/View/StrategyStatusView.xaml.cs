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
    /// Interaction logic for StrategyStatusView.xaml
    /// </summary>
    public partial class StrategyStatusView : Window
    {
        private static bool isOpen = false;

        private static List<string> symbolList = new List<string>();

        public static List<string> SymbolList
        {
            get { return StrategyStatusView.symbolList; }
            set { StrategyStatusView.symbolList = value; }
        }

        private static double popUpWidth = 0.00;
        private static double popUpHeight = 0.00;

        public static double PopUpWidth
        {
            get { return popUpWidth; }
            set { popUpWidth = value; }
        }
        public static double PopUpHeight
        {
            get { return popUpHeight; }
            set { popUpHeight = value; }
        }


        public static bool IsOpen
        {
            get { return StrategyStatusView.isOpen; }
            set { StrategyStatusView.isOpen = value; }
        }

        public StrategyStatusView()
        {
            InitializeComponent();

            if (Properties.Settings.Default.popUpMaximized)
            {
                this.WindowState = WindowState.Maximized;
                Properties.Settings.Default.popUpTop = RestoreBounds.Top;
                Properties.Settings.Default.popUpLeft = RestoreBounds.Left;
                Properties.Settings.Default.popUpHeight = RestoreBounds.Height;
                Properties.Settings.Default.popUpWidth = RestoreBounds.Width;
                Properties.Settings.Default.popUpMaximized = true;
            }
            else
            {
                if (Properties.Settings.Default.popUpTop != 0)
                {
                    this.Top = Properties.Settings.Default.popUpTop;
                }

                if (Properties.Settings.Default.popUpLeft > 0)
                {
                    this.Left = Properties.Settings.Default.popUpLeft;
                }

                if (Properties.Settings.Default.popUpHeight > 0)
                {
                    this.Height = Properties.Settings.Default.popUpHeight;
                }

                if (Properties.Settings.Default.popUpWidth > 0)
                {
                    this.Width = Properties.Settings.Default.popUpWidth;
                }
                Properties.Settings.Default.popUpMaximized = false;
            }

        }

        internal void Init(string strategyStatus)
        {
            this.strategyStatusUC.Init(strategyStatus);
            this.Title = string.Format("{0} Symbol(s)", strategyStatus);
        }
    }
}
