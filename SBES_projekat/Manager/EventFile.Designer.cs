﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Manager {
    using System;
    
    
    /// <summary>
    ///   A strongly-typed resource class, for looking up localized strings, etc.
    /// </summary>
    // This class was auto-generated by the StronglyTypedResourceBuilder
    // class via a tool like ResGen or Visual Studio.
    // To add or remove a member, edit your .ResX file then rerun ResGen
    // with the /str option, or rebuild your VS project.
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "16.0.0.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    internal class EventFile {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal EventFile() {
        }
        
        /// <summary>
        ///   Returns the cached ResourceManager instance used by this class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("Manager.EventFile", typeof(EventFile).Assembly);
                    resourceMan = temp;
                }
                return resourceMan;
            }
        }
        
        /// <summary>
        ///   Overrides the current thread's CurrentUICulture property for all
        ///   resource lookups using this strongly typed resource class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Globalization.CultureInfo Culture {
            get {
                return resourceCulture;
            }
            set {
                resourceCulture = value;
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Uspesno kreiran sertifikat bez sifre {0}.
        /// </summary>
        internal static string CertificateCreated {
            get {
                return ResourceManager.GetString("CertificateCreated", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Sertifikat bez sifre nije kreiran {0}.
        /// </summary>
        internal static string CertificateFailed {
            get {
                return ResourceManager.GetString("CertificateFailed", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Uspesno kreiran sertifikat sa sifrom {0}.
        /// </summary>
        internal static string CertificatePasswordCreated {
            get {
                return ResourceManager.GetString("CertificatePasswordCreated", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Sertifikat sa sifrom nije kreiran {0}.
        /// </summary>
        internal static string CertificatePasswordFailed {
            get {
                return ResourceManager.GetString("CertificatePasswordFailed", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Uspesno je povucen sertifikat {0}.
        /// </summary>
        internal static string CertificateRevoked {
            get {
                return ResourceManager.GetString("CertificateRevoked", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Sertifikat nije povucen {0}.
        /// </summary>
        internal static string CertificateRevokeFailed {
            get {
                return ResourceManager.GetString("CertificateRevokeFailed", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Zatvaranje konekcije sa strane klijenta.
        /// </summary>
        internal static string ClientConnectionClosed {
            get {
                return ResourceManager.GetString("ClientConnectionClosed", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Konekcija je registrovana u CMS.
        /// </summary>
        internal static string ConnectionRegistered {
            get {
                return ResourceManager.GetString("ConnectionRegistered", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Konekcija nije registrovana u CMS.
        /// </summary>
        internal static string ConnectionRegisterFailed {
            get {
                return ResourceManager.GetString("ConnectionRegisterFailed", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Zatvaranje konekcije sa strane servera.
        /// </summary>
        internal static string ServerConnectionClosed {
            get {
                return ResourceManager.GetString("ServerConnectionClosed", resourceCulture);
            }
        }
    }
}
