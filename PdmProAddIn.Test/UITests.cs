using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PdmProAddIn.ViewModels;
using System.Collections.ObjectModel;
using PdmProAddIn.Models;
using PdmProAddIn.Services;
using EdmLib;
using System.Linq;

namespace PdmProAddIn.Test
{
    [TestClass]
    public class UITests
    {
        //[TestMethod]
        //public void RealTest()
        //{

        //    Assert.Fail();
        //}

        public void ShowWindowTest()
        {
            var window = new AAFileRefsWindow();

            window.DataContext = new AAFileRefsViewModel(@"C:\foo\bar.sldprt", new AAFileRef[]
                {
                    new AAFileRef
                    {
                        Path = @"C:\foo\baz.pdf"
                    },
                    new AAFileRef
                    {
                        Path = @"C:\foo\quux.pdf"
                    },
                    new AAFileRef
                    {
                        Path = @"C:\foo\bar.pdf"
                    }
                }, 
                () => window.Close())
            {
                ParentFilePath = @"C:\foo\bar.sldprt"
            };

            window.ShowDialog();
        }

        public void ShowWindowPdmTest()
        {
            string parentFilePath = @"C:\EPDMVaults\Training\Built Parts\Block1.sldprt";
            string varName = "Document Number";
            string vaultName = "Training";

            IEdmVault13 vault = VaultSingleton.Instance;

            VaultHelper h = new VaultHelper(vault);

            string loginError;

            if (!h.TryLoginAuto(vaultName, out loginError))
                Assert.Fail();

            IEdmFolder5 parentFolder;
            IEdmFile5 file = (IEdmFile5)vault.GetFileFromPath(parentFilePath, out parentFolder);

            var fileVars = (IEdmEnumeratorVariable10)file.GetEnumeratorVariable();

            object oVal;
            bool success = fileVars.GetVar2(varName, "@", parentFolder.ID, out oVal);

            var window = new AAFileRefsWindow();

            // do search and gret results...

            var search = new VaultSearch(vault); 

            AAFileRef[] results = search.SearchForFileRefs(oVal.ToString());

            var vm = new AAFileRefsViewModel(parentFilePath, results, () => window.Close());

            window.DataContext = vm;

            window.ShowDialog();

            if (vm.OkWasClicked)
            {
                var fileRefsSvc = new CustomRefsService(vault);

                string[] chilPaths = vm.Results
                    .Where(x => x.IsIncluded)
                    .Select(x => x.Path).ToArray();

                fileRefsSvc.AddCustomReferences(file.ID, chilPaths);
            }
        }
    }
}
