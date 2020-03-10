using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GettingStartedClientNew.ServiceReference1;

namespace GettingStartedClientNew
{
    class Program
    {
        static void Main(string[] args)
        {

            ChequeServiceClient client = new ChequeServiceClient();

            var result = client.GetLastCheques(4);
            Debugger.Break();
            foreach (var e in result) {
                Console.WriteLine($"{e.Id} {e.Number} {e.Summ} {e.Discount} {e.Articles[0]} {e.Articles[1]} {e.Articles[2]} " );
            }
        
            result[0].Id = new Guid("0067c87b-AAF8-4F12-A3A5-048de444eedd");
            result[0].Number += 9999;
            client.SaveCheque(result[0]); 

            Debugger.Break();

        }
    }
}
