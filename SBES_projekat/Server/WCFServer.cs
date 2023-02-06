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

namespace Server
{
    public class WCFServer : DuplexChannelFactory<IService>, IService, IDisposable
    {
        IService factory;

        public WCFServer(object serverCallback, NetTcpBinding binding, EndpointAddress address) : base(serverCallback, binding, address)
        {

            string cltCertCN = Formatter.ParseName(WindowsIdentity.GetCurrent().Name);

            this.Credentials.ServiceCertificate.Authentication.CertificateValidationMode = X509CertificateValidationMode.ChainTrust;
            this.Credentials.ServiceCertificate.Authentication.RevocationMode = X509RevocationMode.NoCheck;
            this.Credentials.ClientCertificate.Certificate = CertManager.GetCertificateFromStorage(StoreName.My, StoreLocation.LocalMachine, cltCertCN);

            factory = this.CreateChannel();
        }


        public bool CertificateWithPvk(string root)
        {


            if (!factory.CertificateWithPvk(root))
            {

                Console.WriteLine("Neuspjesno  generisanje sertifikata sa kljucem. Pokusajte ponovo!");
                return false;
            }
            else
            {

                Console.WriteLine("Instalirajte sertifikate u mmc i kliknite enter da nastavite.");
                Console.ReadLine();

                Console.WriteLine("Uspjesno generisanje sertifikata sa kljucem!");

                return true;
            }

        }


        public bool CertificateWithoutPvk(string root)
        {
            if (!factory.CertificateWithoutPvk(root))
            {

                Console.WriteLine("Neuspjesno  generisanje sertifikata bez kljuca. Pokusajte ponovo!");
                return false;
            }
            else
            {

                Console.WriteLine("Instalirajte sertifikate u mmc i kliknite enter da nastavite.");
                Console.ReadLine();

                Console.WriteLine("Uspjesno generisanje sertifikata bez kljuca!");

                return true;
            }

        }

        public bool RevokeCertificate()
        {
            if (!factory.RevokeCertificate())
            {

                Console.WriteLine("Neuspjesno brisanje sertifikata! Pokusajte ponovo");
                return false;
            }
            else
            {

                Console.WriteLine("Sertifikati su povuceni i upisani u RevocationList.txt. Pritisnite enter da nastavite");
                Console.ReadLine();

                return true;
            }

        }

        public bool Register()
        {
            if (!factory.Register())
            {

                Console.WriteLine("Korisnik nije registrovan na CMS-u");
                return false;
            }
            else
            {

                Console.WriteLine("Korisnik je uspesno registrovan na CMS-u");

                return true;
            }
        }

        public bool ProveriRevoke(string myThumbprint)
        {
            if (!factory.ProveriRevoke(myThumbprint))
            {

                Console.WriteLine("Vas sertifikat je povucen, molimo generisite novi");
                return false;
            }
            else
            {

                return true;
            }
        }
    }
}
