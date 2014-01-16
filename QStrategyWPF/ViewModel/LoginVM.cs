using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EZXWPFLibrary.Utils;

namespace QStrategyWPF.ViewModel
{
    public partial class LoginVM : ViewModelBase
    {
        private string username;
        private string password;
        private string erroMessage;
        private string statusMessage;


        public string Username
        {
            get { return username; }
            set { username = value; }
        }

        public string Password
        {
            get { return password; }
            set { password = value; }
        }

        public string ErroMessage
        {
            get { return erroMessage; }
            set
            {
                erroMessage = value;
                this.RaisePropertyChanged("ErroMessage");
            }
        }
        public string StatusMessage
        {
            get { return statusMessage; }
            set
            {
                statusMessage = value;
                this.RaisePropertyChanged("StatusMessage");
            }
        }

        public LoginVM()
            : base()
        {
            Username = "test";
            Password = "test";
            ErroMessage = string.Empty;
            StatusMessage = string.Empty;
        }

        public bool DoLogin()
        {
            App.AppManager.StgEngine.ComMgr.Init();//To createor modify service each time with latest Host/Port info
            string logMessage = string.Format("Class: {0}, Method: {1}", "LoginVM", "DoLogin(...)");
            LogUtil.WriteLog(LogLevel.INFO, logMessage);

            this.ErroMessage = string.Empty;
            if (Username.Equals("test") && Password.Equals("test"))
            {
                if (AppManager != null)
                {
                    bool status = ValidateConnectionStatus();
                    if (status == true)
                    {
                        AppManager.ConnectionMode = Model.ApplicationConnectionMode.CONNECTED;
                    }
                    else
                    {
                        return false;
                    }
                }
                return true;
            }
            else
            {
                this.StatusMessage = string.Empty;
                ErroMessage = "Incorrect Username and/or Password!";
                logMessage = string.Format("Class: {0}, Method: {1}, Message: {2}", "LoginVM", "DoLogin(...)", ErroMessage);
                LogUtil.WriteLog(LogLevel.WARN, logMessage);
                return false;
            }
        }

        private bool ValidateConnectionStatus()
        {
            string logMessage = string.Format("Class: {0}, Method: {1}", "LoginVM", "ValidateConnectionStatus(...)");
            LogUtil.WriteLog(LogLevel.DEBUG, logMessage);
            try
            {
                App.AppManager.CheckConnectionStatus();
                if (App.AppManager.StgEngine.IsConnected == false)
                {
                    throw new Exception("Could not connect to server. Please check network and/or server status.");
                }
            }
            catch (Exception ex)
            {
                logMessage = string.Format("Class: {0}, Method: {1}", "LoginVM", "ValidateConnectionStatus(...), Connection failed!" + ex.Message);
                this.ErroMessage = "Could not connect to server. Please check network and/or server status.";
                LogUtil.WriteLog(LogLevel.ERROR, logMessage);
                return false;
            }
            return true;
        }
    }

}
