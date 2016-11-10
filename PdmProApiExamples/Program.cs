using System;
using System.Diagnostics;
using EPDM.Interop.epdm;
using PdmProStandAlone.Models;
using System.Linq;
using PdmProStandAlone.Services;

namespace PdmProStandAlone
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            IEdmVault13 vault = VaultSingleton.Instance;

            VaultHelper vaultHelper = new VaultHelper(vault);

            // everything below here is predicated on being logged in...
            string error = null;
            if (!vaultHelper.TryLoginAuto(out error))
            {
                Debug.WriteLine(error);
            }
            else
            {
                // Login success (or EdmServer was already logged in)

                var ex1 = new MapFolderExample();
                ex1.MapFolder();

                var ex2 = new MapFileReferenceExample();
                ex2.MapFileReferenceWithVariableValues();            
            }
        }
    }
}