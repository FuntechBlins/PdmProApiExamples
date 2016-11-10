using EPDM.Interop.epdm;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace PdmProStandAlone.Services
{
    /// <summary>
    /// Singleton wrapper to provide an application-wide single EdmVault instance.
    /// This is nice when you want make sure your application only ever creates one vault instance.
    /// </summary>
    internal sealed class VaultSingleton
    {
        private static volatile IEdmVault13 instance;
        private static object syncRoot = new Object();

        private VaultSingleton() { }

        public static IEdmVault13 Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (syncRoot)
                    {
                        if (instance == null)
                            instance = (IEdmVault13)new EdmVault5();
                    }
                }

                return instance;
            }
        }
    }
}
