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

namespace AutoFilterDataGrid.View
{
    /// <summary>
    /// Interaction logic for ShowHideColumnManagerView.xaml
    /// </summary>
    public partial class ShowHideColumnManagerView : Window
    {
        public List<string> visibleItems 
        {
            get
            {
                return this.VM.VisibleItems.ToList();
            }
        }
        private ShowHideColumnManagerVM VM
        {
            get
            {
                return this.DataContext as ShowHideColumnManagerVM;
            }
        }

        public ShowHideColumnManagerView(IEnumerable<string> availableItems, IEnumerable<string> visibleItems)
        {
            InitializeComponent();
            VM.Init(availableItems, visibleItems);
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
            this.Close();
        }

        private void btnOk_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
            this.Close();
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
