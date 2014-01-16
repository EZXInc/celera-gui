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
using QStrategyWPF.ViewModel;
using System.Configuration;
using System.Windows.Threading;
using EZXWPFLibrary.Utils;

namespace QStrategyWPF.View
{
    /// <summary>
    /// Interaction logic for LoginView.xaml
    /// </summary>
    public partial class LoginView : Window
    {
        private LoginVM VM
        {
            get
            {
                return this.DataContext as LoginVM;
            }
        }

        public LoginView()
        {
            string logMessage = string.Format("Class: {0}, Method: {1}", "LoginView", "LoginView(...)");
            LogUtil.WriteLog(LogLevel.INFO, logMessage);

            InitializeComponent();
            this.passwordBox1.Password = VM.Password;
            this.button1.Focus();

            SetConnectionSetting();
        }

        private void SetConnectionSetting()
        {
            string logMessage = string.Format("Class: {0}, Method: {1}", "LoginView", "SetConnectionSetting(), Get value from config and set in TextBox Host/Port");
            LogUtil.WriteLog(LogLevel.DEBUG, logMessage);

            if ((ConfigurationManager.AppSettings.AllKeys.Contains("QSTRATEGY_SERVICE_URL")))
            {
                string ServerURL = ConfigurationManager.AppSettings["QSTRATEGY_SERVICE_URL"];

                logMessage = string.Format("Class: {0}, Method: {1}, ServerURL: {2}", "LoginView", "SetConnectionSetting()", ServerURL);
                LogUtil.WriteLog(LogLevel.DEBUG, logMessage);
                
                int indexOfPortStart = ServerURL.LastIndexOf(":");
                string ServerPort = ServerURL.Substring(indexOfPortStart + 1);
                ServerURL = ServerURL.Replace(":" + ServerPort, "");
                if (ServerPort.Contains("/"))
                {
                    ServerPort = ServerPort.Substring(0, ServerPort.IndexOf("/"));
                }
                if (ServerPort.Contains(@"\"))
                {
                    ServerPort = ServerPort.Substring(0, ServerPort.IndexOf(@"\"));
                }

                ServerURL = ServerURL.Replace("http", "");
                ServerURL = ServerURL.Replace("https", "");
                ServerURL = ServerURL.Replace("/", "");
                ServerURL = ServerURL.Replace(":", "");
                ServerURL = ServerURL.Replace(@"\", "");
                this.txtHost.Text = ServerURL;
                this.txtPort.Text = ServerPort;
            }
            else
            {
                this.txtHost.Text = string.Empty;
                this.txtHost.Text = string.Empty;
            }

        }


        private static void UpdateSetting(string key, string value)
        {
            string logMessage = string.Format("Class: {0}, Method: UpdateSetting(string {1}, string {2})", "LoginView", key, value);
            LogUtil.WriteLog(LogLevel.DEBUG, logMessage);

            Configuration configuration = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            configuration.AppSettings.Settings[key].Value = value;
            configuration.Save();
            ConfigurationManager.RefreshSection("appSettings");
        }

        private void button1_Click(object sender, RoutedEventArgs e)
        {
            string logMessage = string.Format("Class: {0}, Method: {1}", "LoginView", "button1_Click(...)");
            LogUtil.WriteLog(LogLevel.INFO, logMessage);

            if (!(ConfigurationManager.AppSettings.AllKeys.Contains("QSTRATEGY_SERVICE_URL")))
            {
                ConfigurationManager.AppSettings.Add("QSTRATEGY_SERVICE_URL", string.Empty);
            }


            string url = "http://" + this.txtHost.Text;
            if (!string.IsNullOrEmpty(this.txtPort.Text))
            {
                url += ":" + this.txtPort.Text;
            }
            url += "/qs";

            UpdateSetting("QSTRATEGY_SERVICE_URL", url);

            ConfigurationManager.AppSettings["QSTRATEGY_SERVICE_URL"] = url;


            if ((ConfigurationManager.AppSettings.AllKeys.Contains("QSTRATEGY_SERVICE_URL")))
            {
                string ServerURL = ConfigurationManager.AppSettings["QSTRATEGY_SERVICE_URL"];
                int indexOfPortStart = ServerURL.LastIndexOf(":");
                string ServerPort = ServerURL.Substring(indexOfPortStart + 1);
                ServerURL = ServerURL.Replace(":" + ServerPort, "");
                if (ServerPort.Contains("/"))
                {
                    ServerPort = ServerPort.Substring(0, ServerPort.IndexOf("/"));
                }
                if (ServerPort.Contains(@"\"))
                {
                    ServerPort = ServerPort.Substring(0, ServerPort.IndexOf(@"\"));
                }

                this.VM.ErroMessage = string.Empty;
                this.textBlock1.Text = string.Empty;
                this.textBlock2.Text = string.Format("Please wait while checking connection ({0}:{1})", ServerURL, ServerPort);
                AllowUIToUpdate();
                VM.Password = this.passwordBox1.Password;
                if (VM.DoLogin() == true)
                {
                    this.DialogResult = true;
                    this.Close();
                }
                this.textBlock1.Text = this.VM.ErroMessage;
                this.textBlock2.Text = string.Empty;
            }
            else
            {
                this.VM.ErroMessage = "Error: key [QSTRATEGY_SERVICE_URL] is not found config file!";

                logMessage = string.Format("Class: {0}, Method: {1}, Message: {2}", "LoginView", "button1_Click(...)", this.VM.ErroMessage);
                LogUtil.WriteLog(LogLevel.ERROR, logMessage);
            }
        }

        void AllowUIToUpdate()
        {
            DispatcherFrame frame = new DispatcherFrame();

            Dispatcher.CurrentDispatcher.BeginInvoke(DispatcherPriority.Render, new DispatcherOperationCallback(delegate(object parameter)
            {
                frame.Continue = false;
                System.Threading.Thread.Sleep(2000);
                return null;
            }), null);

            Dispatcher.PushFrame(frame);

        }

        private void BtnApply_Click(object sender, RoutedEventArgs e)
        {
            this.txtHost.Text = this.txtHost.Text.Replace("http", "");
            this.txtHost.Text = this.txtHost.Text.Replace("https", "");
            this.txtHost.Text = this.txtHost.Text.Replace(":", "");
            this.txtHost.Text = this.txtHost.Text.Replace("/", "");
            this.txtHost.Text = this.txtHost.Text.Replace(@"\", "");
            this.txtHost.Text = this.txtHost.Text.Replace(@"\", "");
            string url = "http://" + this.txtHost.Text;
            if (!string.IsNullOrEmpty(this.txtPort.Text))
            {
                url += ":" + this.txtPort.Text;
            }
            url += "/qs";
            UpdateSetting("QSTRATEGY_SERVICE_URL", url);
        }

        private void Expander_Collapsed(object sender, RoutedEventArgs e)
        {
            this.Height = 240;
        }

        private void Expander_Expanded(object sender, RoutedEventArgs e)
        {
            this.Height = 300;
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
