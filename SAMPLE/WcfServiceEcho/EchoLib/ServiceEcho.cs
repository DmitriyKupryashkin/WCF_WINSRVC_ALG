using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.ComponentModel;
using System.ServiceModel;
using System.ServiceProcess;
using System.Configuration;
using System.Configuration.Install;
using System.IO;

namespace EchoLib
{


    [ServiceContract(Namespace = "http://MyApp.ServiceModel.Samples")]
    public interface IMessageCollector
    {

        /// <summary>
        /// будет принимать сообщение формата MESSAGE записывать в файл и отправлять ответ
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        [OperationContract]
         string Talk(Message message);

        [OperationContract]
        string SendAnsewer(Message message);

        [OperationContract]
        void CollectMessage (Message message);
    }

    [DataContract]
    public class Message
    {
        [DataMember]
        public string SenderName { get; set; }
        [DataMember]
        public string Description { get; set; }
        [DataMember]
        public DateTime SendingTime { get; set; }
        
    }



    // Implement the IMessageCollector service contract in a service class.
    public class MessageCollectorService : IMessageCollector
    {
        string writePath = @"F:\WcfServiceMessageCollection.txt";

        public string Talk(Message message)
        {
            CollectMessage(message);
            return SendAnsewer(message);
        }

        public void CollectMessage(Message message)
        {
            try
            {
                using (StreamWriter streamWriter = new StreamWriter(writePath, true, System.Text.Encoding.Default))
                {
                    streamWriter.WriteLine($"{message.SendingTime.ToString("F")} " +
                                           $"from {message.SenderName } \n" +
                                           $"desc: {message.Description} \n");
                }
            }
            catch (IOException) { throw; }
            catch (Exception){ throw; }
            SendAnsewer(message);
        }

        public string SendAnsewer(Message message)
        {
            return $"Hello from WCF_SERVICE! Dear '{message.SenderName}', I got and collect your message to {writePath} !";
        }


    }

   
}
