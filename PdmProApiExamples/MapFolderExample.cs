using System;
using System.Diagnostics;
using PdmProStandAlone.Models;
using PdmProStandAlone.Services;
using EPDM.Interop.epdm;
using PdmProAddIn.Services;

namespace PdmProStandAlone
{
    class MapFolderExample : VaultServiceBase
    {
        public MapFolderExample() : base(VaultSingleton.Instance) { }

        public void MapFolder()
        {
            // Traverse folders example
            Folder rootFolder = FileFolderService.GetFolderTree(Vault.RootFolder);
            rootFolder.Traverse(x =>
            {
                Debug.WriteLine(x.Path);

                // TODO: ...
            });

            // Traverse folders example
            var fileRefsService = new FileReferencesService(Vault);
            FileReference fileRefTree = fileRefsService.GetFileReference(@"C:\EPDMVaults\Training\Built Parts\Universal Joint_&.SLDASM");

            // traverse the tree in order to populate batch listing
            IEdmBatchListing2 listing = (IEdmBatchListing2)((IEdmBatchListing)Vault.CreateUtility(EdmUtility.EdmUtil_BatchList));
            fileRefTree.Traverse(x =>
            {
                Debug.WriteLine(x.File.Path);
                listing.AddFileCfg(x.File.Path, DateTime.Now, x.File.Id, "@", (int)EdmListFileFlags.EdmList_Nothing);
            });
        }
    }
}
