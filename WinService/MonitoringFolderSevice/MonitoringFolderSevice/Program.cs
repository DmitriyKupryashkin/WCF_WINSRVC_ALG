using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using MonitoringFolderSevice.ServiceReference1;
using MonitoringFolderSevice.Logger;

namespace MonitoringFolderSevice
{

    
    static class Program
    {

        static void Main()
        {
            MyLogger.InitLogger();
            MyLogger.Log.Info("logger initialization");
            

            ServiceBase[] ServicesToRun;
            ServicesToRun = new ServiceBase[]
            {
                new Service1()
            };
            try
            {
                ServiceBase.Run(ServicesToRun);
                MyLogger.Log.Info("Run Service");
            }
            catch
            {
                MyLogger.Log.Error("Service not running");
            }
        }
    }
}


