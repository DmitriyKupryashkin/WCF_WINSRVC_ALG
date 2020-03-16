using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using System.ServiceModel;
using System.ServiceModel.Description;

namespace WcfDbEchoServiceHost
{
    public partial class WcfEchoHostService : ServiceBase
    {
        private ServiceHost _service_host = null;
        public WcfEchoHostService()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            if (_service_host != null) _service_host.Close();

            string address_HTTP = "http://localhost:9001/WcfServiceHostService";
          
            string address_TCP = "net.tcp://localhost:9002/WcfServiceHostService";

            Uri[] address_base = { new Uri(address_HTTP), new Uri(address_TCP) };
            _service_host = new ServiceHost(typeof(WcfDbEchoLib.ChequeService), address_base); 

            ServiceMetadataBehavior behavior = new ServiceMetadataBehavior();
            _service_host.Description.Behaviors.Add(behavior);

            BasicHttpBinding binding_http = new BasicHttpBinding();
            _service_host.AddServiceEndpoint(typeof(WcfDbEchoLib.IChequeServiceContract), binding_http, address_HTTP); 
            _service_host.AddServiceEndpoint(typeof(IMetadataExchange), MetadataExchangeBindings.CreateMexHttpBinding(), "mex");

            NetTcpBinding binding_tcp = new NetTcpBinding();
            binding_tcp.Security.Mode = SecurityMode.Transport;
            binding_tcp.Security.Transport.ClientCredentialType = TcpClientCredentialType.Windows;
            binding_tcp.Security.Message.ClientCredentialType = MessageCredentialType.Windows;
            binding_tcp.Security.Transport.ProtectionLevel = System.Net.Security.ProtectionLevel.EncryptAndSign;
            _service_host.AddServiceEndpoint(typeof(WcfDbEchoLib.IChequeServiceContract), binding_tcp, address_TCP);
            _service_host.AddServiceEndpoint(typeof(IMetadataExchange), MetadataExchangeBindings.CreateMexTcpBinding(), "mex");

            _service_host.Open();
        }

        protected override void OnStop()
        {
            if (_service_host != null)
            {
                _service_host.Close();
                _service_host = null;
            }
        }
    }
}
