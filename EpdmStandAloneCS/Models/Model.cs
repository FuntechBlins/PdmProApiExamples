using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EpdmStandAloneCS.Models
{
    /// <summary>
    /// Abstract base extended by File and Folder.
    /// </summary>
    public abstract class FileFolderBase
    {
        public string Name { get; set; }
        public string Path { get; set; }
    }

    /// <summary>
    /// A file object in the business sense.
    /// </summary>
    public class File : FileFolderBase
    {
        public string AcmePartNo { get; set; }
    }

    /// <summary>
    /// A folder object in he business sense.
    /// </summary>
    public class Folder : FileFolderBase 
    {
        public List<File> Files { get; set; } = new List<File>();
        public List<Folder> Subfolders { get; set; } = new List<Folder>();

        private void TraverseRecursive(Folder subfolder, Action<FileFolderBase> action)
        {
            action(subfolder);

            foreach (File f in subfolder.Files)
                action(f);

            TraverseRecursive(subfolder, action);
        }

        public void Traverse(Action<FileFolderBase> action)
        {
            foreach(Folder sf in this.Subfolders)
                TraverseRecursive(sf, action);
        }
    }
}
