using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ServiceModel;

namespace WcfDbEchoLib
{
    [ServiceContract(Namespace = "http://MyWcfSrvs.ServiceModel.Samples")]
    public interface IChequeServiceContract
    {
        [OperationContract]
        void SaveCheque(Cheque cheque);
        [OperationContract]
        List<Cheque> GetLastCheques(int lastChequeCount);
    }
}
