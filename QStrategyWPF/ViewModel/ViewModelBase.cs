using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Windows.Threading;
using EZXWPFLibrary.Helpers;
using System.Windows;

namespace QStrategyWPF.ViewModel
{
    public abstract class ViewModelBase : ObservableBase
    {
        Dispatcher currentDispatcher = null;

        public ViewModelBase()
        {
            currentDispatcher = Dispatcher.CurrentDispatcher;
        }

        protected ApplicationManager AppManager
        {
            get { return App.AppManager; }
        }

        public bool IsDesignMode
        {
            get { return (bool)(DesignerProperties.IsInDesignModeProperty.GetMetadata(typeof(DependencyObject)).DefaultValue); }
        }
        protected void RunOnDispatcherThread(Action action)
        {
            if (currentDispatcher == null)
            {
                throw new Exception("currentDispatcher == null");
            }
            //invoke
            currentDispatcher.Invoke(action, null);
        }


    }
}
