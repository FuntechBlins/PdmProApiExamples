using EPDM.Interop.epdm;
using PdmProStandAlone.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PdmProStandAlone
{
    public class FileReferenceTraversalService 
    {
        IEdmVault5 _vault;

        public FileReferenceTraversalService(IEdmVault5 vault)
        {
            _vault = vault;
        }

        public FileReference GetFileReferenceTree(string filePath)
        {
            IEdmFolder5 folder;
            IEdmFile5 file = _vault.GetFileFromPath(filePath, out folder);
            IEdmReference5 fileRef = file.GetReferenceTree(folder.ID);

            return GetFileReferencesRecursive(fileRef, "A");
        }

        private FileReference GetFileReferencesRecursive(IEdmReference5 edmFileRef, string projectName, int level = 0)
        {
            // Map PDM reference to our file reference type
            FileReference fileRef = new FileReference
            {
                 File = new  File
                 {
                     Name = edmFileRef.Name,
                     Path = edmFileRef.FoundPath
                 }
            };

            // Recurse for each child
            IEdmPos5 pos = edmFileRef.GetFirstChildPosition(ref projectName, level == 0, true, edmFileRef.VersionRef);
            while (!pos.IsNull)
            {
                IEdmReference5 edmChildRef = edmFileRef.GetNextChild(pos);

                // TODO: ...

                fileRef.Children.Add(
                     GetFileReferencesRecursive(edmChildRef, projectName, level + 1));
            }

            return fileRef;
        }
    }
}
