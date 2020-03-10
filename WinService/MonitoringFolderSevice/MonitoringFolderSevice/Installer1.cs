using MonitoringFolderSevice.Logger;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration.Install;
using System.Linq;
using System.ServiceProcess;
using System.Threading.Tasks;



namespace MonitoringFolderSevice
{

    /// <summary>
    /// Установщик Службы , без него не установить самописную службу
    /// </summary>

    // указывает на то, что класс Installer1 должен вызываться при установке сборки, то есть службы.
    [RunInstaller(true)]
    public partial class Installer1 : System.Configuration.Install.Installer
    {
        ServiceInstaller serviceInstaller;
        ServiceProcessInstaller processInstaller;
        public Installer1()
        {
            MyLogger.Log.Info("Installer1 initialization");
            InitializeComponent();
            serviceInstaller = new ServiceInstaller();
            processInstaller = new ServiceProcessInstaller();

            processInstaller.Account = ServiceAccount.LocalSystem;
            serviceInstaller.StartType = ServiceStartMode.Manual;
            serviceInstaller.ServiceName = "Service1";
            Installers.Add(processInstaller);
            Installers.Add(serviceInstaller);
        }
    }
}
