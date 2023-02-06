using Common;
using Manager;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Security;
using System.Security.Cryptography.X509Certificates;
using System.Security.Principal;
using System.ServiceModel;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CertificateManagerService
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.PerSession, ConcurrencyMode = ConcurrencyMode.Reentrant)]
    public class Service : IService
    {
        private static List<ICallback> clients = new List<ICallback>();
        public bool CertificateWithPvk(string root)
        {
            IIdentity identity = Thread.CurrentPrincipal.Identity;
            WindowsIdentity windowsIdentity = identity as WindowsIdentity;

            string commonName = Formatter.ParseName(windowsIdentity.Name);

            string groups = GetUserGroups(windowsIdentity);

            if (!File.Exists(root + ".cer"))
            {
                EventLogger.CertificatePasswordFailed(commonName);
                Console.WriteLine("Ne postoji root sa tim imenom: " + root);
                return false;
            }

            try
            {
                string password = "1234";

                string cmd = "/c makecert -sv " + commonName + ".pvk -iv " + root + ".pvk -n \"CN=" + commonName + ",OU=" + groups + "\" -pe -ic " + root + ".cer " + commonName + ".cer -sr localmachine -ss My -sky exchange";
                System.Diagnostics.Process.Start("cmd.exe", cmd).WaitForExit();

                string cmd2 = "/c pvk2pfx.exe /pvk " + commonName + ".pvk /pi " + password + " /spc " + commonName + ".cer /pfx " + commonName + ".pfx";
                System.Diagnostics.Process.Start("cmd.exe", cmd2).WaitForExit();


                UpisiSertifikat(commonName, password);

                if (File.Exists(commonName + ".cer"))
                {
                    EventLogger.CertificatePasswordCreated(commonName);

                    return true;
                }
                else
                {
                    EventLogger.CertificatePasswordFailed(commonName);
                    return false;
                }




            }
            catch (Exception e)
            {
                EventLogger.CertificatePasswordFailed(commonName);

                Console.WriteLine("Greska prilikom kreiranja sertifikata sa sifrom " + e.Message);
                return false;
            }

        }

        public bool CertificateWithoutPvk(string root)
        {
            IIdentity identity = Thread.CurrentPrincipal.Identity;
            WindowsIdentity windowsIdentity = identity as WindowsIdentity;

            string commonName = Formatter.ParseName(windowsIdentity.Name);
            string groups = GetUserGroups(windowsIdentity);

            if (!File.Exists(root + ".cer"))
            {
                Console.WriteLine("Ne postoji root sa tim imenom: " + root);
                EventLogger.CertificateFailed(commonName);

                return false;
            }
            try
            {

                string cmd = "/c makecert -iv " + root + ".pvk -n \"CN=" + commonName + ",OU=" + groups + "\" -ic " + root + ".cer " + commonName + ".cer -sr localmachine -ss My -sky exchange";
                System.Diagnostics.Process.Start("cmd.exe", cmd).WaitForExit();

                UpisiSertifikat(commonName, "");

                if (File.Exists(commonName + ".cer"))
                {
                    EventLogger.CertificateCreated(commonName);

                    return true;
                }
                else
                {
                    EventLogger.CertificateFailed(commonName);
                    return false;
                }
            }
            catch (Exception e)
            {
                EventLogger.CertificateFailed(commonName);

                Console.WriteLine("Greska prilikom kreiranja sertifikata bez sifre " + e.Message);
                return false;
            }

        }


        public bool RevokeCertificate()
        {
            IIdentity identity = Thread.CurrentPrincipal.Identity;
            WindowsIdentity windowsIdentity = identity as WindowsIdentity;

            string commonName = Formatter.ParseName(windowsIdentity.Name);
            try
            {


                X509Certificate2 certificate = CertManager.GetCertificateFromStorage(StoreName.My, StoreLocation.LocalMachine, commonName);
                if (certificate == null)
                    return false;

                using (StreamWriter sw = new StreamWriter("RevocationList.txt", true))
                {
                    sw.WriteLine(certificate.Thumbprint);
                }
                Console.WriteLine("Dodajem u revocation list");

                Program.proxyReplicator.UpisRevocationList(certificate.Thumbprint);


                if (File.Exists(commonName + ".cer"))
                {
                    File.Delete(commonName + ".cer");
                }
                if (File.Exists(commonName + ".pvk"))
                {
                    File.Delete(commonName + ".pvk");
                }
                if (File.Exists(commonName + ".pfx"))
                {
                    File.Delete(commonName + ".pfx");
                }

                EventLogger.CertificateRevoked(commonName);


                foreach (var item in clients)
                {
                    try
                    {
                        item.NotifyClients(certificate.Thumbprint, commonName);
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine("Desila se greska prilikom obavestavanja klijenata " + e.Message);
                    }
                }



                return true;
            }
            catch (Exception e)
            {
                EventLogger.CertificateRevokeFailed(commonName);

                return false;
            }
        }

        private void UpisiSertifikat(string commonName, string password)
        {
            try
            {
                X509Certificate2 certificate;
                if (password == "")
                    certificate = new X509Certificate2(commonName + ".cer");
                else
                    certificate = new X509Certificate2(commonName + ".cer", password);

                Program.proxyReplicator.UpisCertificateList(certificate.Subject + ", thumbprint: " + certificate.Thumbprint);
            }
            catch (Exception e)
            {

                Console.WriteLine("Greska prilikom repliciranja sertifikata", commonName, e.Message);
            }
        }

        private string GetUserGroups(WindowsIdentity windowsIdentity)
        {
            string groups = "";
            foreach (IdentityReference group in windowsIdentity.Groups)
            {
                SecurityIdentifier sid = (SecurityIdentifier)group.Translate(typeof(SecurityIdentifier));
                var name = sid.Translate(typeof(NTAccount)).ToString();

                if (name.Contains('\\'))
                    name = name.Split('\\')[1];

                if (name == "RegionNorth" || name == "RegionSouth" || name == "RegionWest" || name == "RegionEast")
                {
                    if (groups != "")
                        groups += "_" + name;
                    else
                        groups = name;
                }
            }

            return groups;
        }

        public bool Register()
        {
            try
            {
                clients.Add(OperationContext.Current.GetCallbackChannel<ICallback>());

                EventLogger.ConnectionRegistered();

                return true;
            }
            catch (Exception e)
            {

                EventLogger.ConnectionRegisterFailed();
                Console.WriteLine("Desila se greska prilikom registrovanja " + e.Message);
                return false;
            }


        }

        public bool ProveriRevoke(string myThumbprint)
        {
            if (File.Exists("RevocationList.txt"))
            {
                using (StreamReader sr = new StreamReader("RevocationList.txt"))
                {
                    string contents = sr.ReadToEnd();
                    if (contents.Contains(myThumbprint))
                    {
                        Console.WriteLine("Sertifikat je povucen");
                        return false;
                    }
                }
            }
            return true;
        }
    }
}
