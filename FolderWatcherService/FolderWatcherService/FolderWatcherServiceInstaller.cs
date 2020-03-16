using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration.Install;
using System.Linq;
using System.ServiceProcess;
using System.Threading.Tasks;

namespace FolderWatcherService
{
    [RunInstaller(true)]
    public partial class FolderWatcherServiceInstaller : Installer
    {
        public FolderWatcherServiceInstaller()
        {
            //InitializeComponent();
            serviceProcessInstaller1 = new ServiceProcessInstaller();
            serviceProcessInstaller1.Account = ServiceAccount.LocalSystem;
            serviceInstaller1 = new ServiceInstaller();
            serviceInstaller1.ServiceName = "FolderWatcherService";
            serviceInstaller1.DisplayName = "FolderWatcherService";
            serviceInstaller1.Description = "Win Service as Client to WcfDbEchoService";
            serviceInstaller1.StartType = ServiceStartMode.Automatic;
            Installers.Add(serviceProcessInstaller1);
            Installers.Add(serviceInstaller1);
        }
    }
}
