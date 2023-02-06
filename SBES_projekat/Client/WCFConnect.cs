using Common;
using Manager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Security.Principal;
using System.ServiceModel;
using System.ServiceModel.Security;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Client
{
    public class WCFConnect : ChannelFactory<ICommunication>, ICommunication, IDisposable
    {
        ICommunication factory;

        public WCFConnect(NetTcpBinding binding, EndpointAddress address) : base(binding, address)
        {

            string cltCertCN = Formatter.ParseName(WindowsIdentity.GetCurrent().Name);

            this.Credentials.ServiceCertificate.Authentication.CertificateValidationMode = X509CertificateValidationMode.ChainTrust;
            this.Credentials.ServiceCertificate.Authentication.RevocationMode = X509RevocationMode.NoCheck;
            this.Credentials.ClientCertificate.Certificate = CertManager.GetCertificateFromStorage(StoreName.My, StoreLocation.LocalMachine, cltCertCN);

            factory = this.CreateChannel();
        }


        public void TestCommunication()
        {
            try
            {
                factory.TestCommunication();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                Console.WriteLine("Problem u komunikaciji izmedju servera i klijenta");
            }
        }

        public void PingServer(DateTime vreme)
        {
            try
            {
                factory.PingServer(vreme);
            }
            catch (Exception e)
            {
                Console.WriteLine("Problem u slanju poruka ka serveru");
            }
        }

    }
}
