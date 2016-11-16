using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PdmProAddIn.Models
{
    public class AAFileRef
    {
        public int ParentFolderId { get; set; }
        public int FileId { get; set; }
        public string Path { get; set; }
    }
}
