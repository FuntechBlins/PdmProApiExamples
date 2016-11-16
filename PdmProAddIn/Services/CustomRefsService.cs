using EdmLib;
using PdmProAddIn.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PdmProAddIn.Services
{
    public class CustomRefsService
    {
        IEdmVault13 _vault;

        public CustomRefsService(IEdmVault13 vault)
        {
            _vault = vault;
        }

        public void AddCustomReferences(int parentFileId, string[] paths)
        {
            IEdmAddCustomRefs2 cr = (IEdmAddCustomRefs2)((IEdmAddCustomRefs2)_vault.CreateUtility(EdmUtility.EdmUtil_AddCustomRefs));

            Array qtyArr = paths.Select(x => 1).ToArray();
            Array aPaths = paths;

            cr.AddReferencesPath2(parentFileId, ref aPaths, ref qtyArr);
            cr.CreateTree((int)EdmCreateReferenceFlags.Ecrf_Nothing);
            cr.CreateReferences();
        }
    }
}
