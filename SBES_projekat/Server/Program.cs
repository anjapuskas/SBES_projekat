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
    class Program
    {
        private static WCFHost wcfHost;
        private static WCFServer wcfServer;
        static void Main(string[] args)
        {

            EventLogger.Initialize();

            NetTcpBinding binding = new NetTcpBinding();

            string address = "net.tcp://localhost:9999/Service";

            binding.Security.Mode = SecurityMode.Transport;
            binding.Security.Transport.ClientCredentialType = TcpClientCredentialType.Windows;
            binding.Security.Transport.ProtectionLevel = System.Net.Security.ProtectionLevel.EncryptAndSign;

            NetTcpBinding bindingCert = new NetTcpBinding();

            bindingCert.Security.Transport.ClientCredentialType = TcpClientCredentialType.Certificate;

            EndpointAddress endpointAddress = new EndpointAddress(new Uri(address));

            ServerCallback serverCallback = new ServerCallback();
            WCFServer proxyWcf = new WCFServer(serverCallback, binding, endpointAddress);
            proxyWcf.Register();
            wcfServer = proxyWcf;



            UserInterface(proxyWcf);

            Console.WriteLine("Konekcija ugasena.");
            Console.ReadLine();

        }

        public static void UserInterface(WCFServer proxyWcf)
        {

            int option = 0;
            do
            {
                Console.WriteLine("1. Izgenerisi sertifikat sa privatnim kljucem");
                Console.WriteLine("2. Izgenerisi sertifikat bez privatnog kljuca");
                Console.WriteLine("3. Povuci sertifikate");
                Console.WriteLine("4. Startuj Host Server");
                Console.WriteLine("5. KRAJ");
                int.TryParse(Console.ReadLine(), out option);
                string root;
                switch (option)
                {
                    case 1:
                        Console.WriteLine("Unesite root: ");
                        root = Console.ReadLine();
                        proxyWcf.CertificateWithPvk(root);
                        break;
                    case 2:
                        Console.WriteLine("Unesite root: ");
                        root = Console.ReadLine();
                        proxyWcf.CertificateWithoutPvk(root);
                        break;
                    case 3:
                        proxyWcf.RevokeCertificate();
                        break;
                    case 4:
                        startujHostServer();
                        break;
                    case 5: //exit program
                        break;
                    default:
                        Console.WriteLine("Greska ");
                        break;
                }
            } while (option < 5);
        }

        private static void startujHostServer()
        {
            try
            {
                if (proveriRevoke()) {
                    wcfHost = new WCFHost();
                    wcfHost.OpenServer();
                    Console.WriteLine("WCFHost je startovan");
                    Console.ReadLine();
                }

            }
            catch (Exception e)
            {
                Console.WriteLine("Neuspesna konekcija, generisite sertifikat pa probajte ponovo");
            }
        }

        public static void closeConnection()
        {
            if (wcfHost != null)
            {
                wcfHost.CloseServer();
            }
        }

        private static bool proveriRevoke()
        {

            string myName = Formatter.ParseName(WindowsIdentity.GetCurrent().Name);
            X509Certificate2 clientCert = CertManager.GetCertificateFromStorage(StoreName.My, StoreLocation.LocalMachine, myName);
            Console.WriteLine("Proveravam revoke");
            if (clientCert == null)
            {
                Console.WriteLine("Neuspesna konekcija, generisite sertifikat pa probajte ponovo");
                return false;
            }
            return wcfServer.ProveriRevoke(clientCert.Thumbprint);

        }
    }
}
