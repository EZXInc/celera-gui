using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Threading;
using System.ComponentModel;
using System.Windows;
using EZXWPFLibrary.Helpers;
using EZXWPFLibrary;

namespace AutoFilterDataGrid.ViewModel
{
    public abstract class ViewModelBase : ObservableBase
    {
        Dispatcher currentDispatcher = null;

        public ViewModelBase()
        {
            currentDispatcher = Dispatcher.CurrentDispatcher;
        }


        public bool IsDesignMode
        {
            get { return (bool)(DesignerProperties.IsInDesignModeProperty.GetMetadata(typeof(DependencyObject)).DefaultValue); }
        }

        protected void RunOnDispatcherThread(Action action)
        {
            if (currentDispatcher == null)
            {
                throw new Exception("currentDispatcher is null");
            }
            //invoke
            currentDispatcher.Invoke(action, null);
        }


    }
}
