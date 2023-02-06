using Common;
using Manager;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Security.Principal;
using System.ServiceModel;
using System.ServiceModel.Security;
using System.Text;
using System.Threading.Tasks;

namespace CertificateManagerService
{
    class Program
    {
        public static ProxyReplicator proxyReplicator = null;

        static void Main(string[] args)
        {

            EventLogger.Initialize();

            NetTcpBinding binding = new NetTcpBinding();
            string address = "net.tcp://localhost:9999/Service";


            binding.Security.Mode = SecurityMode.Transport;
            binding.Security.Transport.ClientCredentialType = TcpClientCredentialType.Windows;
            binding.Security.Transport.ProtectionLevel = System.Net.Security.ProtectionLevel.EncryptAndSign;

            binding.OpenTimeout = new TimeSpan(0, 10, 0);
            binding.CloseTimeout = new TimeSpan(0, 10, 0);
            binding.SendTimeout = new TimeSpan(0, 10, 0);
            binding.ReceiveTimeout = new TimeSpan(0, 10, 0);




            ServiceHost host = new ServiceHost(typeof(Service));
            host.AddServiceEndpoint(typeof(IService), binding, address);

            //Replication proxy
            NetTcpBinding bindingReprication = new NetTcpBinding();
            string addressReplication = "net.tcp://localhost:9997/ReplicatorService";

            bindingReprication.Security.Mode = SecurityMode.Transport;
            bindingReprication.Security.Transport.ClientCredentialType = TcpClientCredentialType.Windows;
            bindingReprication.Security.Transport.ProtectionLevel = System.Net.Security.ProtectionLevel.EncryptAndSign;


            EndpointAddress endpointAddress = new EndpointAddress(new Uri(addressReplication));
            proxyReplicator = new ProxyReplicator(binding, endpointAddress);

            napraviRoot(proxyReplicator);


            try
            {
                host.Open();
                Console.WriteLine("CMS servis je pokrenut.\nKliknite enter da zaustavite ...");
                Console.ReadLine();
            }
            catch (Exception e)
            {
                Console.WriteLine("Desila se greska" + e.Message);
                Console.ReadLine();
            }
            finally
            {
                host.Close();
            }

        }

        private static void napraviRoot(ProxyReplicator proxyReplicator)
        {
            Console.WriteLine("Unesite ime root-a : ");
            string root = Console.ReadLine();
            try
            {


                if (File.Exists(root + ".cer"))
                {
                    Console.WriteLine("Vec postoji sertifikat " + root);
                    return;
                }

                string cmd = "/c makecert -n \"CN =" + root + "\" -r -sv " + root + ".pvk " + root + ".cer";
                System.Diagnostics.Process.Start("cmd.exe", cmd).WaitForExit();

                Console.WriteLine("Kreiran je root sertifikat");

                X509Certificate2 certificate = new X509Certificate2(root + ".cer");

                Program.proxyReplicator.UpisCertificateList(certificate.Subject + ", thumbprint: " + certificate.Thumbprint);


                EventLogger.CertificatePasswordCreated(root);

            }
            catch (Exception e)
            {
                EventLogger.CertificatePasswordFailed(root);
            }
        }
    }
}
