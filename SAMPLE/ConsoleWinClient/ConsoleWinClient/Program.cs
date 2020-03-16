using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ConsoleWinClient.ServiceReference1;
namespace ConsoleWinClient
{
    class Program
    {
        static void Main(string[] args)
        {
            MessageCollectorClient porxy = new MessageCollectorClient();
            Message msg = new Message() {SenderName="ConsoleClient", Description = "Hello World", SendingTime = DateTime.Now  };

            Console.WriteLine("Запук консольного клиента для WCFWindowsMessageCollectorServiceSample");
            Console.WriteLine("Для отправки сообщения нажмите любую кнопку.");
            Console.ReadKey();
            Console.WriteLine($"{porxy.Talk(msg)}");
            Console.ReadKey();
        }
    }
}
