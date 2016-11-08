using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using EPDM.Interop.epdm;
using EpdmStandAloneCS.Models;

namespace EpdmStandAloneCS
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            IEdmVault13 vault = (IEdmVault13)new EdmVault5();

            // Login... 
            try
            {
                // A real world application will usually need to implement a try/catch pattern around LoginAuto becuase even something seemingly 
                // benign like the user clicking the X button in the subsequently displayed PDM login dialog (blocks thread) box WILL cause 
                // LoginAuto to throw a COMException. This is the intended behavior of the API, many different members defined on various 
                // interface types across the PDM API are designed to relay COM HRESULT values to .NET like this by throwing a new/related COMException.

                vault.LoginAuto("Training", 0);
            }
            catch (COMException comEx)
            {
                Debug.WriteLine(comEx.Message);
            }

            Folder rootFolder = Traversal.GetFolderTree(vault.RootFolder);

            rootFolder.Traverse(x =>
            {
                Debug.WriteLine(x.Path);

                // TODO: ...
            });
        }
    }
}
