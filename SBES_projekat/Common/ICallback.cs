using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    public interface ICallback
    {
        [OperationContract]
        void NotifyClients(string msg, string serverName);
    }
}
