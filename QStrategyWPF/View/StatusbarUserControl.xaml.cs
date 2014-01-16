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

namespace QStrategyWPF.View
{
    /// <summary>
    /// Interaction logic for StatusbarUserControl.xaml
    /// </summary>
    public partial class StatusbarUserControl : UserControl
    {
        public StatusbarUserControl()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            string msg = "Connection error occurred!\nPlease check network-connection and strategy-engine status." + "\n\nException Details\n" + App.AppManager.StgEngine.LastInvalidSymbolUpdateMessage + "\n" + App.AppManager.StgEngine.LastInvalidSymbolUpdateMessageDetail;
            MessageBox.Show(msg, "Error occurred!");
            App.AppManager.StgEngine.ClearInvalidResponse();
        }
    }
}
