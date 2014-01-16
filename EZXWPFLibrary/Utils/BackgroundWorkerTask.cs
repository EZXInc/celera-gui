using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace EZXWPFLibrary.Utils
{
    public class BackgroundWorkerTask
    {
        public BackgroundWorker Bw;
        public delegate void BackgroundWorkerDelegate();
        public BackgroundWorkerDelegate backgroundMethod;

        public BackgroundWorkerTask(BackgroundWorkerDelegate newFunction)
        {
            backgroundMethod = newFunction;
            Bw = new BackgroundWorker();
            Bw.DoWork += new DoWorkEventHandler(Bw_DoWork);
            Bw.RunWorkerCompleted += new RunWorkerCompletedEventHandler(Bw_RunWorkerCompleted);
        }

        void Bw_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            LogUtil.WriteLog(LogLevel.INFO, "Bw_RunWorkerCompleted");
        }

        void Bw_DoWork(object sender, DoWorkEventArgs e)
        {
            if (this.backgroundMethod != null)
            {
                this.backgroundMethod();
            }
        }

        public void RunProcess()
        {
            if (this.Bw.IsBusy == false)
            {
                this.Bw.RunWorkerAsync();
            }
        }

        public void StopProcess()
        {
            if (this.Bw.IsBusy)
            {
                this.Bw.CancelAsync();
            }
        }
    }
}
