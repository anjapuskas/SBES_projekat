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
using System.Threading.Tasks;

namespace Server
{
    public class WCFHost
    {
        ServiceHost host;
        public WCFHost()
        {
            try
            {
                NetTcpBinding binding = new NetTcpBinding();
                binding.Security.Transport.ClientCredentialType = TcpClientCredentialType.Certificate;
                binding.OpenTimeout = new TimeSpan(0, 10, 0);
                binding.CloseTimeout = new TimeSpan(0, 10, 0);
                binding.SendTimeout = new TimeSpan(0, 10, 0);
                binding.ReceiveTimeout = new TimeSpan(0, 10, 0);

                string address = "net.tcp://localhost:9000/Server";

                host = new ServiceHost(typeof(CommunicationImplementation));
                host.AddServiceEndpoint(typeof(ICommunication), binding, address);

                host.Credentials.ClientCertificate.Authentication.CertificateValidationMode = X509CertificateValidationMode.ChainTrust;
                host.Credentials.ClientCertificate.Authentication.RevocationMode = X509RevocationMode.NoCheck;


                string srvName = Formatter.ParseName(WindowsIdentity.GetCurrent().Name);

                host.Credentials.ServiceCertificate.Certificate = CertManager.GetCertificateFromStorage(StoreName.My, StoreLocation.LocalMachine, srvName);
            } catch (Exception e)
            {
                Console.WriteLine("Neuspesna konekcija, generisite sertifikat pa probajte ponovo");
            }


        }

        public void OpenServer()
        {
            host.Open();
        }
        public void CloseServer()
        {           
            try
            {
                host.Close();

            }
            catch (Exception e)
            {
            }

        }
    }
}
