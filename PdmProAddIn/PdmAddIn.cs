using EdmLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

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
        }

        public void OnCmd(ref EdmCmd poCmd, ref Array ppoData)
        {
            throw new NotImplementedException();
        }
    }
}
