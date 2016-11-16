using System;
using System.Linq;
using System.Diagnostics;
using EPDM.Interop.epdm;
using PdmProStandAlone.Models;
using PdmProStandAlone.Services;
using PdmProAddIn.Services;

namespace PdmProStandAlone
{
    class MapFileReferenceExample : VaultServiceBase
    {
        public MapFileReferenceExample() : base(VaultSingleton.Instance) { }

        public void MapFileReferenceWithVariableValues()
        {
            // define the list of variables to fetch for each file in the tree
            // TODO: maybe get these from a "real" source
            string[] variableNames =
            {
                "Description",
                "Number"
            };

            var fileRefsService = new FileReferencesService(Vault);

            fileRefsService.GetFileReference(@"C:\EPDMVaults\Training\Built Parts\Universal Joint_&.SLDASM", variableNames, "@");
        }
    }
}
