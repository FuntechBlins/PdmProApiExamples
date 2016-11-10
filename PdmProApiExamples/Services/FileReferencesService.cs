using EPDM.Interop.epdm;
using PdmProStandAlone.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace PdmProStandAlone
{
    /// <summary>
    /// Service to access file references.
    /// </summary>
    sealed class FileReferencesService : VaultServiceBase
    {
        public FileReferencesService(IEdmVault13 vault) : base(vault) { }

        private FileReference GetFileReferencesRecursive(IEdmReference5 edmFileRef, string projectName, int level = 0)
        {
            // Map PDM reference to our file reference type
            FileReference fileRef = new FileReference
            {
                 File = new  File
                 {
                     Id = edmFileRef.FileID,
                     FolderId = edmFileRef.FolderID,
                     Name = edmFileRef.Name,
                     Path = edmFileRef.FoundPath
                 }
            };

            // Recurse for each child
            IEdmPos5 pos = edmFileRef.GetFirstChildPosition(ref projectName, level == 0, true, edmFileRef.VersionRef);
            while (!pos.IsNull)
            {
                IEdmReference5 edmChildRef = edmFileRef.GetNextChild(pos);

                fileRef.Children.Add(
                     GetFileReferencesRecursive(edmChildRef, projectName, level + 1));
            }

            return fileRef;
        }

        public FileReference GetFileReference(string filePath)
        {
            IEdmFolder5 folder;
            IEdmFile5 file = _vault.GetFileFromPath(filePath, out folder);
            IEdmReference5 fileRef = file.GetReferenceTree(folder.ID);

            return GetFileReferencesRecursive(fileRef, "A");
        }

        public FileReference GetFileReference(string filePath, string[] variableNames, string config)
        {
            FileReference fileRef = GetFileReference(filePath);

            // traverse the tree in order to populate batch listing
            IEdmBatchListing2 listing = (IEdmBatchListing2)((IEdmBatchListing)Vault.CreateUtility(EdmUtility.EdmUtil_BatchList));
            fileRef.Traverse(x =>
            {
                Debug.WriteLine(x.File.Path);
                listing.AddFileCfg(x.File.Path, DateTime.Now, x.File.Id, config, (int)EdmListFileFlags.EdmList_Nothing);
            });

            // create the list
            EdmListCol[] lstCols = null;
            listing.CreateListEx("\n" + string.Join("\n", variableNames), (int)EdmCreateListExFlags.Edmclef_MayReadFiles, ref lstCols);

            // populate the listing
            EdmListFile[] lstFiles = null;
            listing.GetFiles(ref lstFiles);

            int totalNodes = fileRef.Enumerate().Count();
            Debug.WriteLine(totalNodes);
            foreach (var f in fileRef.Enumerate())
            {
                Debug.WriteLine(f.File.Path);
            }

            Debug.Assert(lstFiles.Length == totalNodes);

            foreach (EdmListFile lf in lstFiles)
            {
                // Get the FileReference data model instance that corresponds to the current EdmListFile result instance in the loop.
                // The Enumerate member of FileReference was created for this purpose (supporting LINQ/Lamba expressions).
                var fileRefForLf = fileRef.Enumerate().Single(x => x.File.Id == lf.mlFileID && lf.mlFolderID == x.File.FolderId);

                // iterate over current file's variable values

                Debug.WriteLine("file (id: " + lf.mlFileID + ") variable values:");
                string[] values = lf.moColumnData as string[];
                for (int i = 0; i < values.Length; i++)
                {
                    Debug.WriteLine("   " + variableNames[i] + ": " + values[i]);

                    // Map the variable value results to the correct properties of File instance held by the corresponding FileReference instance.
                    fileRefForLf.File.Description = variableNames[0];
                    fileRefForLf.File.Number = variableNames[1];
                }
            }

            return fileRef;
        }
    }
}
