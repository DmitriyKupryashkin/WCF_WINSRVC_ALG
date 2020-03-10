using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using MonitoringFolderSevice.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using MonitoringFolderSevice.ServiceReference1;
using MonitoringFolderSevice.Logger;

namespace MonitoringFolderSevice
{
    /// <summary>
    /// Служба для мониторинга изменения в папке файлов.
    /// </summary>

    public partial class Service1 : ServiceBase
    {
        FolderWatcher folderWatcher; 
        public Service1()
        {
            InitializeComponent();
            this.CanStop = true;
            this.CanPauseAndContinue = true;
            this.AutoLog = true;
        }

        protected override void OnStart(string[] args)
        {

            folderWatcher = new FolderWatcher();
            try
            {
                Thread folderWatcherThread = new Thread(new ThreadStart(folderWatcher.Start));
                folderWatcherThread.Start();
                MyLogger.Log.Info("Service start in new thread ");
            }
            catch { 
                MyLogger.Log.Error("Service cant start in new thread ");
            }
        }

        protected override void OnStop()
        {
           
            folderWatcher.Stop();
            Thread.Sleep(1000);
            MyLogger.Log.Info("Service thread stoped");
        }
    }

}
