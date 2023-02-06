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

namespace Server
{
    public class ServerCallback : ICallback
    {
        public void NotifyClients(string msg, string serverName)
        {
            Console.WriteLine("Obrisan je sertifikat: " + serverName);
            string myName = Formatter.ParseName(WindowsIdentity.GetCurrent().Name);
            X509Certificate2 servCert = CertManager.GetCertificateFromStorage(StoreName.My, StoreLocation.LocalMachine, myName);
            if (servCert != null)
            {
                if (servCert.Thumbprint == msg)
                {
                    Console.WriteLine("Zatvaranje konekcije sa strane servera");
                    Program.closeConnection();

                    EventLogger.ServerConnectionClosed(serverName);
                }
            }
        }
    }
}
