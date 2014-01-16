using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using EZXWPFLibrary.Helpers;

namespace EZXWPFLibrary.AutoFilterDataGrid.Model
{
    public class ColumnsConfigInfo : ObservableBase
    {
        private string gridName;
        private string columnSortMemberPath;
        private string columnHeader;
        private ColumnWidth columnWidth = new ColumnWidth();
        private int columnDisplayIndex;
        private Visibility columnVisibility;

        public string GridName
        {
            get { return gridName; }
            set
            {
                gridName = value;
                this.RaisePropertyChanged(p => p.GridName);
            }
        }
        public string ColumnSortMemberPath
        {
            get { return columnSortMemberPath; }
            set
            {
                columnSortMemberPath = value;
                this.RaisePropertyChanged(p => p.ColumnSortMemberPath);
            }
        }
        public string ColumnHeader
        {
            get { return columnHeader; }
            set
            {
                columnHeader = value;
                this.RaisePropertyChanged(p => p.ColumnHeader);
            }
        }
        public ColumnWidth ColumnWidth
        {
            get { return columnWidth; }
            set
            {
                columnWidth = value;
                this.RaisePropertyChanged(p => p.ColumnWidth);
            }
        }
        public int ColumnDisplayIndex
        {
            get { return columnDisplayIndex; }
            set
            {
                columnDisplayIndex = value;
                this.RaisePropertyChanged(p => p.ColumnDisplayIndex);
            }
        }
        public Visibility ColumnVisibility
        {
            get { return columnVisibility; }
            set 
            { 
                columnVisibility = value;
                this.RaisePropertyChanged(p => p.ColumnVisibility);
            }
        }   
    }

    public enum LengthType
    {
        UNIT,
        AUTO,
        START,
    }

    public class ColumnWidth : ObservableBase
    {
        private LengthType columnWidthType = LengthType.AUTO;
        private double? width;
        private double? minWidth;
        private double? maxWidth;

        public LengthType ColumnWidthType
        {
            get { return columnWidthType; }
            set 
            { 
                columnWidthType = value; 
                this.RaisePropertyChanged(p => p.ColumnWidthType); 
            }
        }
        public double? Width
        {
            get
            {
                return width;
            }
            set
            {
                width = value; 
                this.RaisePropertyChanged(p => p.Width);
            }
        }
        public double? MinWidth
        {
            get { return minWidth; }
            set
            {
                minWidth = value;
                this.RaisePropertyChanged(p => p.MinWidth);
            }
        }
        public double? MaxWidth
        {
            get { return maxWidth; }
            set
            {
                maxWidth = value; this.RaisePropertyChanged(p => p.Width);
            }
        }

        public ColumnWidth()
        {
        }

        public ColumnWidth(double colWidth)
        {
            this.ColumnWidthType = LengthType.UNIT;
            this.Width = colWidth;
        }
     }
}
