using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace GettingStartedLib
{
    public class ChequeService : IChequeService
    {
        private ChequeReposytory reposytory = new ChequeReposytory();

        public List<Cheque> GetLastCheques(int lastChequeCount)
        {
           return reposytory.GetLastCheques(lastChequeCount);         
        }

        public void SaveCheque(Cheque cheque)
        {
            reposytory.SaveCheque(cheque);
        }
    }
}
