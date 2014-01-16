using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EZXWPFLibrary.AutoFilterDataGrid.Model;
using EZXWPFLibrary.Utils;
using EZXWPFLibrary.Helpers;
using System.IO;
using AutoFilterDataGrid;
using System.Windows.Controls;

namespace EZXWPFLibrary.AutoFilterDataGrid.Controller
{
    public class ColumnsConfigurationController
    {
        private string baseDirectory;
        private string fileName = "AutoFilterGridColumnsConfig.xml";

        public string BaseDirectory
        {
            get 
            {
                if (string.IsNullOrEmpty(baseDirectory))
                {
                    return System.IO.Directory.GetCurrentDirectory();
                } 
                return baseDirectory; 
            }
            set 
            { 
                baseDirectory = value; 
            }
        }
        public string FileName
        {
            get 
            {
                if (string.IsNullOrEmpty(fileName))
                {
                    return "AutoFilterGridColumnsConfig.xml";
                } 
                return fileName; 
            }
            set { fileName = value; }
        }

        public virtual List<ColumnsConfigInfo> ColumnsConfig { get; set; }

        public void LoadColumnsConfigurationController()
        {
            LogUtil.WriteLog(LogLevel.DEBUG, "ColumnsConfigurationController.LoadColumnsConfigurationController() started");
            ColumnsConfig = XmlHelper.ReadFromFile<List<ColumnsConfigInfo>>(string.Format("{0}{1}{2}", BaseDirectory, Path.DirectorySeparatorChar, FileName));
            if (ColumnsConfig == null)
            {
                ColumnsConfig = new List<ColumnsConfigInfo>();
            }

            LogUtil.WriteLog(LogLevel.DEBUG, "ColumnsConfigurationController.LoadColumnsConfigurationController() finished");
        }
 
        public virtual void Save()
        {
            LogUtil.WriteLog(LogLevel.DEBUG, "ColumnsConfigurationController.Save() started");

            XmlHelper.WriteToFile<List<ColumnsConfigInfo>>(ColumnsConfig, string.Format("{0}{1}{2}", BaseDirectory, Path.DirectorySeparatorChar, FileName));

            LogUtil.WriteLog(LogLevel.DEBUG, "ColumnsConfigurationController.Save() finished");
        }

        virtual public List<ColumnsConfigInfo> GetColumnsConfigurationList(string gridName)
        {
            LogUtil.WriteLog(LogLevel.DEBUG, "GetColumnsConfigurationList ...");

            var columnsConfiguration = from colsConfig in ColumnsConfig
                                       where colsConfig.GridName == gridName
                                       select colsConfig;
            return columnsConfiguration.ToList();
        }

        virtual public void SetColumnsConfigurationList(string gridName, AutofilterDataGrid dgGrid)
        {
            LogUtil.WriteLog(LogLevel.DEBUG, "SetColumnsConfigurationList started");

            ColumnsConfig.RemoveAll(col => col.GridName== gridName);
            foreach (DataGridColumn col in dgGrid.Columns)
            {
                ColumnsConfig.Add(new ColumnsConfigInfo()
                {
                    GridName = gridName,
                    ColumnWidth = new ColumnWidth(col.ActualWidth),
                    ColumnDisplayIndex = col.DisplayIndex,
                    //ColumnHeader = col.Header.ToString(),
                    ColumnSortMemberPath = col.SortMemberPath,
                    ColumnVisibility = col.Visibility
                });
            }

            LogUtil.WriteLog(LogLevel.DEBUG, "SetColumnsConfigurationList finished");
        }
    }
}
