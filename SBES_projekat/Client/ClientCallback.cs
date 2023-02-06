using Common;
using Manager;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace Client
{
    public class ClientCallback : ICallback
    {
        public void NotifyClients(string msg, string serverName)
        {
            Console.WriteLine("Obrisan je sertifikat: " + serverName);
            string myName = Formatter.ParseName(WindowsIdentity.GetCurrent().Name);
            X509Certificate2 clientCert = CertManager.GetCertificateFromStorage(StoreName.My, StoreLocation.LocalMachine, myName);
            if (clientCert != null)
            {
                if (clientCert.Thumbprint == msg)
                {

                    Console.WriteLine("Zatvaranje konekcije sa strane klijenta");
                    Program.closeConnection();

                    EventLogger.ClientConnectionClosed(serverName);

                }
            }

            X509Certificate2 servCert = CertManager.GetCertificateFromStorage(StoreName.TrustedPeople, StoreLocation.LocalMachine, serverName);
            if (servCert != null)
            {
                if (servCert.Thumbprint == msg)
                {

                    Console.WriteLine("Zatvaranje konekcije sa strane servera");
                    Program.closeConnection();
                    Console.WriteLine("Zatvorena konekcija");

                    EventLogger.ServerConnectionClosed(serverName);
                }
            }
        }
    }
}
