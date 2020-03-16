using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration.Install;
using System.Linq;
using System.Threading.Tasks;
using System.ServiceProcess;

namespace WcfDbEchoServiceHost
{
    [RunInstaller(true)]
    public partial class WcfEchoServiceInstaller : Installer
    {
        public WcfEchoServiceInstaller()
        {
            //здесь не используется 
            //InitializeComponent();
            serviceProcessInstaller1 = new ServiceProcessInstaller();
            serviceProcessInstaller1.Account = ServiceAccount.LocalSystem;
            serviceInstaller1 = new ServiceInstaller();
            serviceInstaller1.ServiceName = "WcfDbEchoService";
            serviceInstaller1.DisplayName = "WcfDbEchoService";
            serviceInstaller1.Description = "WCF Service Hosted by Windows Service, Kupryashkin D.V. Sample";
            serviceInstaller1.StartType = ServiceStartMode.Automatic;
            Installers.Add(serviceProcessInstaller1);
            Installers.Add(serviceInstaller1);
        }
    }
}
