using Manager;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Manager
{
    public class EventLogger
    {
        private static EventLog log = new EventLog();


        public static void Initialize()
        {
            try
            {
                if (!EventLog.SourceExists("CMSEvents"))
                {
                    EventLog.CreateEventSource("CMSEvents", "CMSLog");


                    Console.WriteLine("Napravljen je event log");
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Problem u podizanju event loga " + e.Message);
            }


            log.Source = "CMSEvents";
            log.Log = "CMSLog";
        }


        public static void CertificateCreated(string certName)
        {
            //TO DO

            if (log != null)
            {
                string CertificateCreated =
                    AuditEvents.CertificateCreated;
                string message = String.Format(CertificateCreated,
                    certName);
                log.WriteEntry(message);
            }
            else
            {
                throw new ArgumentException(string.Format("Error while trying to write event (eventid = {0}) to event log.",
                    (int)AuditEventTypes.CertificateCreated));
            }
        }

        public static void CertificateFailed(string certName)
        {
            //TO DO

            if (log != null)
            {
                string CertificateFailed =
                    AuditEvents.CertificateFailed;
                string message = String.Format(CertificateFailed,
                    certName);
                log.WriteEntry(message);
            }
            else
            {
                throw new ArgumentException(string.Format("Error while trying to write event (eventid = {0}) to event log.",
                    (int)AuditEventTypes.CertificateFailed));
            }
        }

        public static void CertificatePasswordCreated(string certName)
        {
            //TO DO

            if (log != null)
            {
                string CertificatePasswordCreated =
                    AuditEvents.CertificatePasswordCreated;
                string message = String.Format(CertificatePasswordCreated,
                    certName);
                log.WriteEntry(message);
            }
            else
            {
                throw new ArgumentException(string.Format("Error while trying to write event (eventid = {0}) to event log.",
                    (int)AuditEventTypes.CertificatePasswordCreated));
            }
        }

        public static void CertificatePasswordFailed(string certName)
        {
            //TO DO

            if (log != null)
            {
                string CertificatePasswordFailed =
                    AuditEvents.CertificatePasswordFailed;
                string message = String.Format(CertificatePasswordFailed,
                    certName);
                log.WriteEntry(message);
            }
            else
            {
                throw new ArgumentException(string.Format("Error while trying to write event (eventid = {0}) to event log.",
                    (int)AuditEventTypes.CertificatePasswordFailed));
            }
        }

        public static void CertificateRevoked(string certName)
        {
            //TO DO

            if (log != null)
            {
                string CertificateRevoked =
                    AuditEvents.CertificateRevoked;
                string message = String.Format(CertificateRevoked,
                    certName);
                log.WriteEntry(message);
            }
            else
            {
                throw new ArgumentException(string.Format("Error while trying to write event (eventid = {0}) to event log.",
                    (int)AuditEventTypes.CertificateRevoked));
            }
        }

        public static void CertificateRevokeFailed(string certName)
        {
            //TO DO

            if (log != null)
            {
                string CertificateRevokeFailed =
                    AuditEvents.CertificateRevokeFailed;
                string message = String.Format(CertificateRevokeFailed,
                    certName);
                log.WriteEntry(message);
            }
            else
            {
                throw new ArgumentException(string.Format("Error while trying to write event (eventid = {0}) to event log.",
                    (int)AuditEventTypes.CertificateRevokeFailed));
            }
        }

        public static void ClientConnectionClosed(string certName)
        {
            //TO DO

            if (log != null)
            {
                string ClientConnectionClosed =
                    AuditEvents.ClientConnectionClosed;
                string message = String.Format(ClientConnectionClosed,
                    certName);
                log.WriteEntry(message);
            }
            else
            {
                throw new ArgumentException(string.Format("Error while trying to write event (eventid = {0}) to event log.",
                    (int)AuditEventTypes.ClientConnectionClosed));
            }
        }

        public static void ServerConnectionClosed(string certName)
        {
            //TO DO

            if (log != null)
            {
                string ServerConnectionClosed =
                    AuditEvents.ServerConnectionClosed;
                string message = String.Format(ServerConnectionClosed,
                    certName);
                log.WriteEntry(message);
            }
            else
            {
                throw new ArgumentException(string.Format("Error while trying to write event (eventid = {0}) to event log.",
                    (int)AuditEventTypes.ServerConnectionClosed));
            }
        }

        public static void ConnectionRegistered()
        {
            //TO DO

            if (log != null)
            {
                string ConnectionRegistered =
                    AuditEvents.ConnectionRegistered;
                log.WriteEntry(ConnectionRegistered);
            }
            else
            {
                throw new ArgumentException(string.Format("Error while trying to write event (eventid = {0}) to event log.",
                    (int)AuditEventTypes.ConnectionRegistered));
            }
        }

        public static void ConnectionRegisterFailed()
        {
            //TO DO

            if (log != null)
            {
                string ConnectionRegisterFailed =
                    AuditEvents.ConnectionRegisterFailed;
                log.WriteEntry(ConnectionRegisterFailed);
            }
            else
            {
                throw new ArgumentException(string.Format("Error while trying to write event (eventid = {0}) to event log.",
                    (int)AuditEventTypes.ConnectionRegisterFailed));
            }
        }

    }
}
