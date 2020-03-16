using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;

namespace WinServiceClientLib
{
    static class Program
    {
        /// <summary>
        /// http://localhost:9001/WcfServiceHostService
        /// ServiceReferenceCustomWCFService
        /// Служба Windows (проект win службы)представляющеая собой клиента для другой службы win service
        /// Бужет отправлять даныне на другую службу внутри которой будет служба wcf
        /// MessageSenderService из решения (WinServiceClient)  -> WCFWindowsMessageCollectorServiceSample из решения WinServiceEcho
        /// </summary>
        static void Main()
        {
            ServiceBase[] ServicesToRun;
            ServicesToRun = new ServiceBase[]
            {
                new MessageSenderService()
            };
            ServiceBase.Run(ServicesToRun);
        }
    }
}
