using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

#if PDM_STANDALONE
using EPDM.Interop.epdm;
#else
using EdmLib;
#endif

namespace PdmProAddIn.Services
{
    /// <summary>
    /// Singleton wrapper to provide an application-wide single EdmVault instance.
    /// This is nice when you want make sure your application only ever creates one vault instance.
    /// </summary>
    public sealed class VaultSingleton
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
