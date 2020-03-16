using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WcfDbEchoLib
{
    


    public class ChequeService : IChequeServiceContract
    {
        private ChequeRepository _reposytory;
        public ChequeService()
        {
            _reposytory  = new ChequeRepository();
        }      

        public List<Cheque> GetLastCheques(int lastChequeCount)
        {
            return _reposytory.GetLastCheques(lastChequeCount);
        }

        public void SaveCheque(Cheque cheque)
        {
            _reposytory.SaveCheque(cheque);
        }


    }
}
