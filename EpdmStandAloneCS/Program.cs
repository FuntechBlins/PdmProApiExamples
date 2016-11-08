using EPDM.Interop.epdm;
using EpdmStandAloneCS.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows.Forms;

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
            try
            {
                vault.LoginAuto("Training", 0);
            }
            catch (COMException comEx)
            {
                MessageBox.Show(comEx.Message);
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
