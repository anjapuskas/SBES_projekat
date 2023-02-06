using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Security.Cryptography.X509Certificates;
using System.Security.Principal;
using System.ServiceModel;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Common;
using Manager;

namespace Client
{
    public class Program
    {

        static WCFClient wcfClient;
        static WCFConnect wcfConnect;

        static void Main(string[] args)
        {

            EventLogger.Initialize();

            NetTcpBinding binding = new NetTcpBinding();

            string address = "net.tcp://localhost:9999/Service";
            //string addressCert = "net.tcp://localhost:9999/ServiceCert";


            binding.Security.Mode = SecurityMode.Transport;
            binding.Security.Transport.ClientCredentialType = TcpClientCredentialType.Windows;
            binding.Security.Transport.ProtectionLevel = System.Net.Security.ProtectionLevel.EncryptAndSign;

            EndpointAddress endpointAddress = new EndpointAddress(new Uri(address));
            ClientCallback clientCallback = new ClientCallback();

            wcfClient = new WCFClient(clientCallback, binding, endpointAddress);
            wcfClient.Register();



            UserInterface();

            Console.WriteLine("Konekcija ugasena.");
            Console.ReadLine();

        }

        public static void UserInterface()
        {

            int option = 0;
            do
            {
                Console.WriteLine("1. Izgenerisi sertifikat sa privatnim kljucem");
                Console.WriteLine("2. Izgenerisi sertifikat bez privatnog kljuca");
                Console.WriteLine("3. Povuci sertifikate");
                Console.WriteLine("4. Konektuj se na server");
                Console.WriteLine("5. Pinguj server");
                Console.WriteLine("6. KRAJ");
                int.TryParse(Console.ReadLine(), out option);
                string root;

                switch (option)
                {
                    case 1:
                        Console.WriteLine("Unesite root: ");
                        root = Console.ReadLine();
                        wcfClient.CertificateWithPvk(root);
                        break;
                    case 2:
                        Console.WriteLine("Unesite root: ");
                        root = Console.ReadLine();
                        wcfClient.CertificateWithoutPvk(root);
                        break;
                    case 3:
                        wcfClient.RevokeCertificate();
                        break;
                    case 4: //exit program
                        ConnectToServer();
                        break;
                    case 5: //exit program
                        PingServer();
                        break;
                    case 6: //exit program
                        break;
                    default:
                        break;
                }
            } while (option < 6);
        }

        private static void ConnectToServer()
        {
            NetTcpBinding binding = new NetTcpBinding();

            string address = "net.tcp://localhost:9000/Server";
            //string addressCert = "net.tcp://localhost:9999/ServiceCert";


            binding.Security.Transport.ClientCredentialType = TcpClientCredentialType.Certificate;
            binding.OpenTimeout = new TimeSpan(0, 10, 0);
            binding.CloseTimeout = new TimeSpan(0, 10, 0);
            binding.SendTimeout = new TimeSpan(0, 10, 0);
            binding.ReceiveTimeout = new TimeSpan(0, 10, 0);

            Console.WriteLine("Unesite ime servera: ");
            string serverName = Console.ReadLine();
            X509Certificate2 servCert;
            try
            {
                servCert = CertManager.GetCertificateFromStorage(StoreName.TrustedPeople, StoreLocation.LocalMachine, serverName);
                if (servCert == null)
                {
                    Console.WriteLine("Uneli ste servera koji ne postoji, molimo vas probajte ponovo");
                    return;
                }

            }
            catch (Exception e)
            {
                Console.WriteLine("Uneli ste servera koji ne postoji, molimo vas probajte ponovo");
                return;

            }

            if (proveriRevoke())
            {

                WCFConnect proxyWcf;
                try
                {
                    EndpointAddress addressServer = new EndpointAddress(new Uri(address), new X509CertificateEndpointIdentity(servCert));

                    proxyWcf = new WCFConnect(binding, addressServer);

                    proxyWcf.TestCommunication();

                    Console.WriteLine("Uspostavljena je komunikacija");


                }
                catch (Exception e)
                {
                    Console.WriteLine("Neuspesna konekcija, generisite sertifikat pa probajte ponovo");
                    return;

                }
               

                wcfConnect = proxyWcf;
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
            return wcfClient.ProveriRevoke(clientCert.Thumbprint);

        }

        private static void PingServer()
        {
            if (wcfConnect == null)
            {
                Console.WriteLine("Prvo se konektujte na server");
                return;
            }
            Random r = new Random();
            try
            {
                while (true)
                {
                    Thread.Sleep(r.Next(1, 10) * 1000); //sleep 1-10s
                    wcfConnect.PingServer(DateTime.Now);

                    Console.WriteLine("Saljem poruku");

                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public static void closeConnection()
        {
            if (wcfConnect != null && wcfConnect.State == CommunicationState.Opened)
            {
                try
                {
                    wcfConnect.Close();
                } catch(Exception e)
                {}
                
            }

        }
    }
}
