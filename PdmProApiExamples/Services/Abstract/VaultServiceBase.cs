using EPDM.Interop.epdm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PdmProStandAlone
{
    /// <summary>
    /// Base class for all examples (see other concrete Example types).
    /// </summary>
    abstract class VaultServiceBase
    {
        public VaultServiceBase(IEdmVault13 vault)
        {
            _vault = vault;
        }

        protected readonly IEdmVault13 _vault;

        protected IEdmVault13 Vault
        {
            get
            {
                return _vault;
            }
        }
    }
}
