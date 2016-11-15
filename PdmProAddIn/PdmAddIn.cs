using EdmLib;
using PdmProAddIn.Models;
using PdmProAddIn.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows;

namespace PdmProAddIn
{
    /// <summary>
    /// The main add-in implementation class.
    /// </summary>
    [ComVisible(true), Guid("11AB19CA-A61C-48EB-9A14-7D6A9DA93519")]
    public class PdmAddIn : IEdmAddIn5
    {
        public void GetAddInInfo(ref EdmAddInInfo poInfo, IEdmVault5 poVault, IEdmCmdMgr5 poCmdMgr)
        {
            poInfo.mbsAddInName =  nameof(PdmAddIn);  //"PdmAddIn";
            poInfo.mbsDescription = "Bare bones PDM add-in";
            poInfo.mbsCompany = "ACME Corp";
            poInfo.mlAddInVersion = 1;
            poInfo.mlRequiredVersionMajor = 15;
            poInfo.mlRequiredVersionMinor = 0;

            poCmdMgr.AddCmd(1, "AA Atach Files", (int)(
                EdmMenuFlags.EdmMenu_OnlyInContextMenu | 
                EdmMenuFlags.EdmMenu_OnlyFiles | 
                EdmMenuFlags.EdmMenu_OnlySingleSelection)); // Only supporting single selection for now.

            // Example of adding a hook
            poCmdMgr.AddHook(EdmCmdType.EdmCmd_PostAdd);
        }

        public void OnCmd(ref EdmCmd poCmd, ref Array ppoData)
        {
            //// TODO: This is useful for showing child windows properly
            //poCmd.mlParentWnd;

            switch (poCmd.meCmdType)
            {
                case EdmCmdType.EdmCmd_Menu:
                    if (poCmd.mlCmdID == 1)
                    {
                        //// TODO: Only supporting single selection for now.
                        //foreach(EdmCmdData d in ppoData)
                        //{
                        //    d.mbsStrData1  = 
                        //}

                        var fileData = (EdmCmdData)ppoData.GetValue(0);


                        string fileName = fileData.mbsStrData1;
                        int fileId = fileData.mlObjectID1;
                        int folderId = fileData.mlObjectID2;

                        IEdmVault16 vault = (IEdmVault16)poCmd.mpoVault;
                        IEdmFile5 file = (IEdmFile5)vault.GetObject(EdmObjectType.EdmObject_File, fileId);
                        IEdmFolder5 parentFolder = (IEdmFolder5)vault.GetObject(EdmObjectType.EdmObject_Folder, folderId);

                        // The "would be" search results (for now just pass empty to view model)
                        var results = Enumerable.Empty<AAFileRef>();

                        // TODO: Instead of the above (empty), do actual PDM API search and get the "real" results mapped into the following array.
                        //var results = new[] {
                        //    new AAFileRef
                        //    {
                        //        FileId = ...,
                        //        ParentFileId = fileId,
                        //        Path = "C:\foo\bar.pdf"
                        //    }
                        //};

                        var window = new AAFileRefsWindow();
                        window.DataContext = new AAFileRefsViewModel(file.GetLocalPath(folderId), Enumerable.Empty<AAFileRef>());
                        window.ShowDialog();
                    }
                    break;
                case EdmCmdType.EdmCmd_PostAdd:
                    break;
                default:
                    break;
            }
        }
    }
}
