using Common;
using Manager;
using System;
using System.Collections.Generic;
using System.IdentityModel.Claims;
using System.IO;
using System.Linq;
using System.Security;
using System.Security.Cryptography.X509Certificates;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace Server
{
    public class CommunicationImplementation : ICommunication
    {
        public void PingServer(DateTime vreme)
        {

            

            X509Certificate2 clientCert = ((X509CertificateClaimSet)
                    OperationContext.Current.ServiceSecurityContext.AuthorizationContext.ClaimSets[0]).X509Certificate;

            string subjectName = clientCert.SubjectName.Name;
            string OU = subjectName.Split(',')[1];
           

            if (!(OU.Contains("RegionWest") || OU.Contains("RegionEast") || OU.Contains("RegionNorth") || OU.Contains("RegionSouth")))
            {
                Console.WriteLine("Korisnik koji pokusava da salje poruku nije deo zahtevane grupe");
                throw new SecurityException("Korisnik koji pokusava da salje poruku nije deo zahtevane grupe");

            }
           //throw new SecurityException("Korisnik koji pokusava da salje poruku nije deo zahtevane grupe");

            int commaIndex = clientCert.SubjectName.Name.IndexOf(',');
            string commonName = clientCert.SubjectName.Name.Remove(commaIndex);

            commonName = commonName.Substring(3);

            int id = 0;

           

            if (File.Exists("serverLog.txt"))
            {
                Console.WriteLine("Fajl postoji");
                string lastLine = File.ReadLines("serverLog.txt").Last();
                int index = lastLine.IndexOf(':');
                int lastID = int.Parse(lastLine.Substring(0, index));

                id = lastID;
            }


            using (StreamWriter sw = new StreamWriter("serverLog.txt", true))
            {
                id++;
                sw.WriteLine(id + ": " + String.Format("{0:g}", vreme) + "; " + commonName);

            }
            Console.WriteLine(id + ": " + String.Format("{0:g}", vreme) + "; " + commonName);
        }


        public void TestCommunication()
        {
            Console.WriteLine("Komunikacija je postavljena");
        }
    }

}
