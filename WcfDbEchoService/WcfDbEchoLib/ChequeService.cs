using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using log4net;
using WcfDbEchoLib.Logger;

namespace WcfDbEchoLib
{
    public class ChequeService : IChequeServiceContract
    {
        static private IChequeRepository _reposytory;
         static ChequeService()
        {
            _reposytory = GetRepository();
        }
        public ChequeService()
        {
            MyLogger.InitLogger();
            MyLogger.Log.Info($"Cheque service initialization");  
        }

        public List<Cheque> GetChequesPack(int lastChequeCount)
        {
            try
            {
                return _reposytory.GetChequesPack(lastChequeCount);
            }
            catch (Exception ex) { MyLogger.Log.Error($"Cant getting Cheques from repository {ex}"); return null; }
        }

        public void SaveCheque(Cheque cheque)
        {
            try
            {
                MyLogger.Log.Info($"Saving cheques");
                _reposytory.SaveCheque(cheque);
            }
            catch (Exception ex) { MyLogger.Log.Error($"Cant saved Cheques to repository {ex}"); }
        }

        private static IChequeRepository GetRepository() {
            

            if (ConfigurationManager.AppSettings.Get("UseFakeRepository") == "1")
            {
                MyLogger.Log.Info($"Use FakeChequeRepository ");
                return new FakeChequeRepository();
            }
            else {
                MyLogger.Log.Info($"Use ChequeRepository ");
                return  new ChequeRepository();
            }
            

        }
    }
}
