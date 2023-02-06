using Common;
using Manager;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Replikator
{
    public class ReplicatorService : IReplikatorService
    {
        public void UpisCertificateList(String thumbprint)
        {
            using (StreamWriter sw = new StreamWriter("CertificateListBackup.txt", true))
            {
                sw.WriteLine(thumbprint);
            }
            Console.WriteLine("[" + DateTime.Now.ToString() + "] Upisano u CertificateListBackup!");
            Console.WriteLine("\t Thumbprint: " + thumbprint);
        }

        public void UpisRevocationList(String thumbprint)
        {
           
            using (StreamWriter sw = new StreamWriter("RevocationListBackup.txt", true))
            {
                sw.WriteLine(thumbprint);
            }
            Console.WriteLine("[" + DateTime.Now.ToString() + "] Upisano u RevocationListBackup!");
            Console.WriteLine("\t Thumbprint: " + thumbprint);
        }
    }
}
