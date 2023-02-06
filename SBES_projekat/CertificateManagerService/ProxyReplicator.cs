using Common;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace CertificateManagerService
{
    public class ProxyReplicator : ChannelFactory<IReplikatorService>, IReplikatorService, IDisposable
    {
        IReplikatorService factory;

        public ProxyReplicator(NetTcpBinding binding, EndpointAddress address) : base(binding, address)
        {
            factory = this.CreateChannel();
        }

        public void UpisCertificateList(String certificate)
        {
            factory.UpisCertificateList(certificate);
        }

        public void UpisRevocationList(string thumbprint)
        {
            Console.WriteLine("Saljem na replikator");
            factory.UpisRevocationList(thumbprint);
        }
    }
}
