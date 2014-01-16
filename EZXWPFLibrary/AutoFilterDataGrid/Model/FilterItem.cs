using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EZXWPFLibrary.Helpers;

namespace AutoFilterDataGrid.Model
{
    public class FilterItem : ObservableBase
    {
        private string data;
        private bool isSelected;

        public string DataDisplay
        {
            get 
            {
                int tempInt;
                if (int.TryParse(this.Data, out tempInt))
                {
                    string dataText = tempInt.ToString("N0");
                    return dataText;
                }
                else
                {
                    double tempValue;
                    if (Double.TryParse(this.Data, out tempValue))
                    {
                        string dataText = tempValue.ToString("N5");
                        return dataText;
                    }
                }
                return Data; 
            }
        }

        public string Data
        {
            get { return data; }
            set
            {
                data = value;
                this.RaisePropertyChanged(p => p.Data);
            }
        }

        public bool IsSelected
        {
            get { return isSelected; }
            set
            {
                isSelected = value;
                this.RaisePropertyChanged(p => p.IsSelected);
            }
        }
    }

}
