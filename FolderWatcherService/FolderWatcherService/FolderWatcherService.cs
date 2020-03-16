using FolderWatcherService.Logger;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace FolderWatcherService
{
    public partial class FolderWatcherService : ServiceBase
    {
        InputFolderWatcher folderWatcher; 
        public FolderWatcherService()
        {
            InitializeComponent();
            this.CanStop = true;
            this.CanPauseAndContinue = true;
            this.AutoLog = true;
        }

        protected override void OnStart(string[] args)
        {

            folderWatcher = new InputFolderWatcher();
            try
            {
                Thread folderWatcherThread = new Thread(new ThreadStart(folderWatcher.Start));
                folderWatcherThread.Start();
                MyLogger.Log.Info("Service start in new thread ");
            }
            catch
            {
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

