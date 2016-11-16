using EdmLib;
using PdmProAddIn.Models;
using PdmProAddIn.Services;
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
        const string VARIABLE_NAME = "Document Number";

        public void GetAddInInfo(ref EdmAddInInfo poInfo, IEdmVault5 poVault, IEdmCmdMgr5 poCmdMgr)
        {
            try
            {
                poInfo.mbsAddInName = nameof(PdmAddIn);  //"PdmAddIn";
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
            catch (Exception ex)
            {
#if Debug
                MessageBox.Show(ex.ToString());
#else
                MessageBox.Show(ex.Message);
#endif
            }
        }

        public void OnCmd(ref EdmCmd poCmd, ref Array ppoData)
        {
            try
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
                            int folderId = fileData.mlObjectID3;

                            IEdmVault16 vault = (IEdmVault16)poCmd.mpoVault;
                            IEdmFile5 file = (IEdmFile5)vault.GetObject(EdmObjectType.EdmObject_File, fileId);
                            IEdmFolder5 parentFolder = (IEdmFolder5)vault.GetObject(EdmObjectType.EdmObject_Folder, folderId);

                            string parentFilePath = file.GetLocalPath(parentFolder.ID);
                            var fileVars = (IEdmEnumeratorVariable10)file.GetEnumeratorVariable();

                            // get the variable
                            object oVal;
                            bool success = fileVars.GetVar2(VARIABLE_NAME, "@", parentFolder.ID, out oVal);

                            if(file.IsLocked)
                            {
                                MessageBox.Show("The file must be checked in.");
                            }
                            else if (oVal == null)
                            {
                                MessageBox.Show($"The variable '{VARIABLE_NAME}' has no value.");
                            }
                            else
                            {
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
                        break;

                    case EdmCmdType.EdmCmd_PostAdd:
                        break;
                    default:
                        break;
                }
            }
            catch (Exception ex)
            {
#if Debug
                MessageBox.Show(ex.ToString());
#else
                MessageBox.Show(ex.Message);
#endif
            }
        }
    }
}
