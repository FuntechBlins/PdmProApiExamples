using EdmLib;
using PdmProAddIn.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PdmProAddIn.Services
{
    public class VaultSearch
    {
        IEdmVault13 _vault;
        IEdmSearch6 _search;

        public VaultSearch(IEdmVault13 vault)
        {
            _vault = vault;
            _search = (IEdmSearch6)((IEdmSearch5)_vault.CreateUtility(EdmUtility.EdmUtil_Search));
        }

        public AAFileRef[] SearchForFileRefs(string variableValue)
        {
            // return value
            var results = new List<AAFileRef>();
            _search.SetToken(EdmSearchToken.Edmstok_FindFiles, true);
            object oName = "Document Number";
            object oValue = variableValue;
            _search.AddVariable(ref oName, ref oValue);
            IEdmSearchResult5 res = _search.GetFirstResult();
            while(res != null)
            {
                results.Add(new AAFileRef
                {
                    FileId = res.ID,
                    Path = res.Path,
                    ParentFolderId = res.ParentFolderID
                });

                res = _search.GetNextResult();
            }
            return results.ToArray();
        }
    }
}
