using System;
using EPDM.Interop.epdm;
using PdmProStandAlone.Models;

namespace PdmProStandAlone
{
    /// <summary>
    /// Examples of PDM vault folder/file traversal.
    /// </summary>
    public static class FolderTraversal
    {
        /// <summary>
        /// Traverses an argument vault folder object recursively and returns a <see cref="Folder"/> instance 
        /// representing the complete hierarchical tree structure of the vault folder in question. 
        /// This method could be VERY SLOW depending on the size and depth of the folder structure being traversed.
        /// </summary>
        /// <param name="folder"></param>
        /// <returns></returns>
        public static Folder GetFolderTree(IEdmFolder5 folder)
        {
            Folder folderOut = new Folder()
            {
                Name = folder.Name,
                Path = folder.LocalPath,
            };

            IEdmPos5 pos = folder.GetFirstFilePosition();

            while (!pos.IsNull)
            {
                IEdmFile5 edmFile = folder.GetNextFile(pos);

                var file = new File()
                {
                    Name = edmFile.Name,
                    Path = edmFile.GetLocalPath(folder.ID)
                    // TODO: AcmePartNo = ....
                };

                folderOut.Files.Add(file);
            }

            pos = folder.GetFirstSubFolderPosition();
            while (!pos.IsNull)
            {
                IEdmFolder5 subFolder = folder.GetNextSubFolder(pos);

                folderOut.Subfolders.Add(
                    GetFolderTree(subFolder));
            }

            return folderOut;
        }
    }
}
