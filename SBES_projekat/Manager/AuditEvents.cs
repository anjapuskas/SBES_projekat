using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Resources;
using System.Text;
using System.Threading.Tasks;

namespace Manager
{
	public enum AuditEventTypes
	{
		CertificateCreated = 0,
		CertificateFailed = 1,
		CertificatePasswordCreated = 2,
		CertificatePasswordFailed = 3,
		CertificateRevoked = 4,
		CertificateRevokeFailed = 5,
		ClientConnectionClosed = 6,
		ServerConnectionClosed = 7,
		ConnectionRegistered = 8,
		ConnectionRegisterFailed = 9
	}

	public class AuditEvents
	{
		private static ResourceManager resourceManager = null;
		private static object resourceLock = new object();

		private static ResourceManager ResourceMgr
		{
			get
			{
				lock (resourceLock)
				{
					if (resourceManager == null)
					{
						resourceManager = new ResourceManager
							(typeof(EventFile).ToString(),
							Assembly.GetExecutingAssembly());
					}
					return resourceManager;
				}
			}
		}

		public static string CertificateCreated
		{
			get
			{
				// TO DO
				return ResourceMgr.GetString(AuditEventTypes.CertificateCreated.ToString());
			}
		}

		public static string CertificateFailed
		{
			get
			{
				//TO DO
				return ResourceMgr.GetString(AuditEventTypes.CertificateFailed.ToString());
			}
		}

		public static string CertificatePasswordCreated
		{
			get
			{
				//TO DO
				return ResourceMgr.GetString(AuditEventTypes.CertificatePasswordCreated.ToString());
			}
		}

		public static string CertificatePasswordFailed
		{
			get
			{
				//TO DO
				return ResourceMgr.GetString(AuditEventTypes.CertificatePasswordFailed.ToString());
			}
		}

		public static string CertificateRevoked
		{
			get
			{
				//TO DO
				return ResourceMgr.GetString(AuditEventTypes.CertificateRevoked.ToString());
			}
		}

		public static string CertificateRevokeFailed
		{
			get
			{
				//TO DO
				return ResourceMgr.GetString(AuditEventTypes.CertificateRevokeFailed.ToString());
			}
		}

		public static string ClientConnectionClosed
		{
			get
			{
				//TO DO
				return ResourceMgr.GetString(AuditEventTypes.ClientConnectionClosed.ToString());
			}
		}

		public static string ServerConnectionClosed
		{
			get
			{
				//TO DO
				return ResourceMgr.GetString(AuditEventTypes.ServerConnectionClosed.ToString());
			}
		}

		public static string ConnectionRegistered
		{
			get
			{
				//TO DO
				return ResourceMgr.GetString(AuditEventTypes.ConnectionRegistered.ToString());
			}
		}

		public static string ConnectionRegisterFailed
		{
			get
			{
				//TO DO
				return ResourceMgr.GetString(AuditEventTypes.ConnectionRegisterFailed.ToString());
			}
		}

	}
}
