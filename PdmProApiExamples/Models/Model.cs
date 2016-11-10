using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PdmProStandAlone.Models
{
    /// <summary>
    /// Abstract base extended by File and Folder.
    /// </summary>
    public abstract class FileFolderBase
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Path { get; set; }
    }

    /// <summary>
    /// A file object in the business sense.
    /// </summary>
    public class File : FileFolderBase
    {
        public int FolderId { get; set; }
        public string Number { get; set; }
        public string Description { get; set; }
    }

    /// <summary>
    /// A folder object in he business sense.
    /// </summary>
    public class Folder : FileFolderBase 
    {
        public int? ParentFolderId { get; set; }
        public List<File> Files { get; set; } = new List<File>();
        public List<Folder> Subfolders { get; set; } = new List<Folder>();

        /// <summary>
        /// Private recursive traversal method.
        /// </summary>
        /// <param name="folder"></param>
        /// <param name="action"></param>
        private void TraverseRecursive(Folder folder, Action<FileFolderBase> action)
        {
            // NOTE: The order of the two foreach loops below as well as where precisely the recursive call occurs directly has a direct effect on the order that the objects will be traversed in. 
            
            // Currently, the fact that the subfolders loop is first, means that the overall recursion will be such that we will walk 
            // down each subfolder path from the top-level to full depth (until there are no subfolders) then bounce back 
            // up ONE level (second-to-deepest level), move to the next subfolder and so on.

            // I.e., this is pretty much the simplest code that is garaunteed to touch everything albeit the order in which it traverses things is arguably couterintuitive depending on the need.

            // traverse current level subfolders   
            foreach (Folder sf in folder.Subfolders)
            {
                // recurse
                TraverseRecursive(sf, action);

                // invoke delegate
                action(sf);
            }

            // traverse current level files
            foreach (File f in folder.Files)
                action(f);
        }

        /// <summary>
        /// Traverses instance and calls the argument delegate on each/every object (file or folder) in the hierarchical structure.
        /// </summary>
        /// <param name="action"></param>
        public void Traverse(Action<FileFolderBase> action)
        {
            TraverseRecursive(this, action);
        }
    }

    /// <summary>
    /// Encapsulates an individual parent/child file reference.
    /// </summary>
    public class FileReference
    {
        //public string Name { get; set; }
        //public string Path { get; set; }
        //public string Number { get; set; }
        //public string Description { get; set; }

        // No need to redefine the above just have File property
        public File File { get; set; }

        public List<FileReference> Children { get; set; } = new List<FileReference>();

        /// <summary>
        /// Executes an action for each reference in the argument tree recursively.
        /// </summary>
        /// <param name="fileRef"></param>
        /// <param name="action"></param>
        private void TraverseRecursive(FileReference fileRef, Action<FileReference> action)
        {
            // invoke delegate
            action(fileRef);

            // recurse for each child
            foreach (FileReference childRef in fileRef.Children)
                TraverseRecursive(childRef, action);
        }

        /// <summary>
        /// Executes an action for each reference in the tree recursively, beginning with the instance itself.
        /// </summary>
        /// <param name="fileRef"></param>
        /// <param name="action"></param>
        public void Traverse(Action<FileReference> action)
        {
            // just call private overload on this
            TraverseRecursive(this, action);
        }

        /// <summary>
        /// Provides deferred iteration over all the nodes of the tree of the argument FileReference node recursively.
        /// This (private) was designed simply to be called from public overload.
        /// </summary>
        /// <param name="fileRef"></param>
        /// <returns></returns>
        private IEnumerable<FileReference> Enumerate(FileReference fileRef)
        {
            yield return fileRef;

            // recurse for each child
            // the nested foreach is required to achieve recursion w/yield
            foreach (FileReference childRef in fileRef.Children)
                foreach (var grandChild in Enumerate(childRef)) // <-- recursion here...
                    yield return grandChild;
        }

        /// <summary>
        /// Provides deferred iteration over all the nodes of in this node's tree starting with the instance itself.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<FileReference> Enumerate()
        {
            return Enumerate(this);
        }
    }
}
