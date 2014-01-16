using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Windows;

namespace QStrategyWPF
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public static ApplicationManager AppManager
        {
            get
            {
                if (App.Current == null)
                {
                    return null;
                }
                else
                {
                    return App.Current.Resources["AppManager"] as ApplicationManager;
                }
            }
        }

        public App()
            : base()
        {
            Application.Current.DispatcherUnhandledException += new System.Windows.Threading.DispatcherUnhandledExceptionEventHandler(Current_DispatcherUnhandledException);
        }

        bool isShuttingDown = false;
        void Current_DispatcherUnhandledException(object sender, System.Windows.Threading.DispatcherUnhandledExceptionEventArgs e)
        {
            try
            {
                if (isShuttingDown)
                    return;

                if (App.Current == null)
                    return;

                Exception exp = e.Exception;
                e.Handled = true;
//                AppManager.HandelError(exp);
            }
            catch (Exception)
            {
//                AppManager.HandelError(exp);
            }
        }

        protected override void OnExit(ExitEventArgs e)
        {
            isShuttingDown = true;
            Application.Current.Shutdown();
            base.OnExit(e);
        }

    }
}
