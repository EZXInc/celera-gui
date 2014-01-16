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
using System.Windows.Threading;
using QStrategyWPF.Model;
using EZXWPFLibrary.Utils;
using QStrategyWPF.View;
using QStrategyGUILib;

namespace QStrategyWPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private DispatcherTimer LoginTimer;
        private DispatcherTimer GetUpdatedDataTimer;

        StrategyStatusView strategyStatusView = new StrategyStatusView();

        public MainWindow()
        {
            string logMessage = string.Format("Class: {0}, Method: {1}", "MainWindow", "MainWindow(...)");
            LogUtil.WriteLog(LogLevel.INFO, logMessage);

            InitializeComponent();
            App.AppManager.ConnectionMode = ApplicationConnectionMode.LOGOUT;

            bool isConnected = ShowLoginView();
            if (isConnected)
            {
                App.AppManager.Init();
            }
            LoginTimer = new DispatcherTimer();
            LoginTimer.Tick += new EventHandler(LoginTimer_Tick);
            LoginTimer.Interval = new TimeSpan(0, 0, 2);
            LoginTimer.Start();

            GetUpdatedDataTimer = new DispatcherTimer();
            GetUpdatedDataTimer.Tick += new EventHandler(GetUpdatedDataTimer_Tick);
            GetUpdatedDataTimer.Interval = new TimeSpan(0, 0, 1);

            if (Properties.Settings.Default.Maximized)
            {
                WindowState = WindowState.Maximized;
                Properties.Settings.Default.Top = RestoreBounds.Top;
                Properties.Settings.Default.Left = RestoreBounds.Left;
                Properties.Settings.Default.Height = RestoreBounds.Height;
                Properties.Settings.Default.Width = RestoreBounds.Width;
                Properties.Settings.Default.Maximized = true;
            }
            else
            {
                this.Top = Properties.Settings.Default.Top;
                this.Left = Properties.Settings.Default.Left;
                this.Height = Properties.Settings.Default.Height;
                this.Width = Properties.Settings.Default.Width;
                Properties.Settings.Default.Maximized = false;
            }

            this.Closed += new EventHandler(MainWindow_Closed);

            App.AppManager.GetSymbolUpdatedDataCompleted += new ApplicationManager.GetSymbolDataUpdateHandler(AppManager_GetSymbolUpdatedDataCompleted);

        }

        void strategyStatusView_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            StrategyStatusView.IsOpen = false;

            string logMessage = string.Format("Class: {0}, Method: {1}", "MainWindow", "strategyStatusView_Closing(...)");
            LogUtil.WriteLog(LogLevel.INFO, logMessage);
            if (strategyStatusView.WindowState == WindowState.Maximized)
            {
                // Use the RestoreBounds as the current values will be 0, 0 and the size of the screen
                Properties.Settings.Default.popUpTop = strategyStatusView.RestoreBounds.Top;
                Properties.Settings.Default.popUpLeft = strategyStatusView.RestoreBounds.Left;
                Properties.Settings.Default.popUpHeight = strategyStatusView.RestoreBounds.Height;
                Properties.Settings.Default.popUpWidth = strategyStatusView.RestoreBounds.Width;
                Properties.Settings.Default.popUpMaximized = true;
            }
            else
            {
                Properties.Settings.Default.popUpTop = strategyStatusView.Top;
                Properties.Settings.Default.popUpLeft = strategyStatusView.Left;
                Properties.Settings.Default.popUpHeight = strategyStatusView.Height;
                Properties.Settings.Default.popUpWidth = strategyStatusView.Width;
                Properties.Settings.Default.popUpMaximized = false;
            }

            Properties.Settings.Default.Save();


        }

        void strategyStatusView_Closed(object sender, EventArgs e)
        {
            StrategyStatusView.IsOpen = false;
        }

        void GetUpdatedDataTimer_Tick(object sender, EventArgs e)
        {
            string strategyStatus = "Hung";
            bool isRecordExist = IsStrategyStatusExists(strategyStatus);


            if (StrategyStatusView.IsOpen == true)
            {
                if (!isRecordExist)
                {
                    strategyStatusView.Close();
                    GetUpdatedDataTimer.Stop();
                    StrategyStatusView.IsOpen = false;
                }
            }
            else
            {
                if (isRecordExist)
                {
                    if (Properties.Settings.Default.ShowHungPopup == false)
                    {
                        return;
                    } 
                    
                    strategyStatusView = new StrategyStatusView();
                    strategyStatusView.Closing += new System.ComponentModel.CancelEventHandler(strategyStatusView_Closing);
                    strategyStatusView.Init(strategyStatus);
                    strategyStatusView.Show();
                    StrategyStatusView.IsOpen = true;
                }
            }
        }

        private static bool IsStrategyStatusExists(string strategyStatus)
        {
            foreach (string strategyId in App.AppManager.DataMgr.StrategyOrderDictionary.Keys)
            {
                foreach (string symbol in App.AppManager.DataMgr.StrategyOrderDictionary[strategyId].Keys)
                {
                    StrategyOrderInfo orderInfo = App.AppManager.DataMgr.StrategyOrderDictionary[strategyId][symbol];

                    if (orderInfo != null && !string.IsNullOrEmpty(orderInfo.Status) && orderInfo.Status.ToUpper().Equals(strategyStatus.ToUpper()))
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        void AppManager_GetSymbolUpdatedDataCompleted(object sender, EventArgs e)
        {
            GetUpdatedDataTimer.Start();
        }

        void LoginTimer_Tick(object sender, EventArgs e)
        {
            if (App.AppManager.ConnectionMode == ApplicationConnectionMode.LOGOUT)
            {
                string logMessage = string.Format("Class: {0}, Method: {1}, ApplicationConnectionMode.LOGOUT occurred!", "MainWindow", "LoginTimer_Tick(...)");
                LogUtil.WriteLog(LogLevel.INFO, logMessage);

                LoginTimer.Stop();
                bool isConnected = ShowLoginView();
                LoginTimer.Start();
            }
        }

        public bool ShowLoginView()
        {
            string logMessage = string.Format("Class: {0}, Method: {1}", "MainWindow", "ShowLoginView(...)");
            LogUtil.WriteLog(LogLevel.INFO, logMessage);

            LoginView loginView = new LoginView();
            if (loginView.ShowDialog() == true)
            {
                App.AppManager.ConnectionMode = ApplicationConnectionMode.CONNECTED;
                return true;
            }
            else
            {
                App.AppManager.ConnectionMode = ApplicationConnectionMode.SUSPENDLOGIN;
                App.Current.Shutdown();
                return false;
            }
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            string logMessage = string.Format("Class: {0}, Method: {1}", "MainWindow", "Window_Closing(...)");
            LogUtil.WriteLog(LogLevel.INFO, logMessage);
            if (WindowState == WindowState.Maximized)
            {
                // Use the RestoreBounds as the current values will be 0, 0 and the size of the screen
                Properties.Settings.Default.Top = RestoreBounds.Top;
                Properties.Settings.Default.Left = RestoreBounds.Left;
                Properties.Settings.Default.Height = RestoreBounds.Height;
                Properties.Settings.Default.Width = RestoreBounds.Width;
                Properties.Settings.Default.Maximized = true;
            }
            else
            {
                Properties.Settings.Default.Top = this.Top;
                Properties.Settings.Default.Left = this.Left;
                Properties.Settings.Default.Height = this.Height;
                Properties.Settings.Default.Width = this.Width;
                Properties.Settings.Default.Maximized = false;
            }

            Properties.Settings.Default.Save();

        }

        void MainWindow_Closed(object sender, EventArgs e)
        {
            App.Current.Shutdown();
        }

    }
}
