using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Threading;

namespace WinServiceClientLib
{
    public partial class MessageSenderService : ServiceBase
    {
        MessageSender messageSender;
        public MessageSenderService()
        {
            InitializeComponent();
            this.CanStop = true;
            this.CanPauseAndContinue = true;
            this.AutoLog = true;
        }

        protected override void OnStart(string[] args)
        {

            messageSender = new MessageSender();
            Thread messageSenderThread = new Thread(new ThreadStart(messageSender.Start));
            messageSenderThread.Start();
        }

        protected override void OnStop()
        {
            messageSender.Stop();
            Thread.Sleep(1000);
        }
    }

   
}
