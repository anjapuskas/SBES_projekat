using Common;
using Manager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace Client
{
    public class CertificateCallBack : ICertificateCallback
    {
        public void NotifyClients(string msg, string serverName)
        {
            X509Certificate2 servCert = CertManager.GetCertificateFromStorage(StoreName.TrustedPeople, StoreLocation.LocalMachine, serverName);
            if (servCert != null)
            {
                if (servCert.Thumbprint == msg)
                {
                    Program.closeConnection(serverName); //zatvaranje konekcije sa serverom
                }
            }

            else
            {
                //check if that's my certificate
                string myName = Formatter.ParseName(WindowsIdentity.GetCurrent().Name);
                X509Certificate2 myCert = CertManager.GetCertificateFromStorage(StoreName.My, StoreLocation.LocalMachine, myName);
                if (myCert != null)
                {
                    if (myCert.Thumbprint == msg) //sertifikat je komptomitovan
                    {
                        Program.closeConnection(serverName);
                    }
                }
            }
        }
    }
}
