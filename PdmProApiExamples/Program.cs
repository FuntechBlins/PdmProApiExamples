using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using EPDM.Interop.epdm;
using PdmProStandAlone.Models;
using System.Collections.Generic;

namespace PdmProStandAlone
{
    public class MyObject
    {
        public int Id { get; set; }
        public string Something { get; set; }
        public string SomethingElse { get; set; }
    }

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

            // Traverse folders example
            Folder rootFolder = FolderTraversal.GetFolderTree(vault.RootFolder);
            rootFolder.Traverse(x =>
            {
                Debug.WriteLine(x.Path);

                // TODO: ...
            });

            // Traverse folders example
            var fileRefsService = new FileReferenceTraversalService(vault);
            FileReference fileRefTree = fileRefsService.GetFileReferenceTree(@"C:\EPDMVaults\Training\Built Parts\Universal Joint_&.SLDASM");

            fileRefTree.Traverse(x =>
           {
               Debug.WriteLine(x.File.Path);
               Debug.WriteLine(x.Children.Count);

               // TODO: ...
           });
        }
    }
}
