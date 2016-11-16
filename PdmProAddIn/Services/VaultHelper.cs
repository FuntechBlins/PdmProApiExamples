using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

#if PDM_STANDALONE
using EPDM.Interop.epdm;
#else
using EdmLib;
#endif

namespace PdmProAddIn.Services
{

    /// <summary>
    /// 
    /// </summary>
    public class VaultHelper
    {
        readonly IEdmVault13 _vault;

        public VaultHelper(IEdmVault13 vault)
        {
            _vault = vault;
        }

        /// <summary>
        /// Wraps call to <see cref="IEdmVault5.LoginAuto(string, int)"/> in a try/catch and handles <see cref="COMException"/> (see error param).
        /// </summary>"
        /// <param name="error">The value of the Message property of the exception (if a <see cref="COMException"/> was thrown).</param>
        /// <returns></returns>
        public bool TryLoginAuto(string vaultName, out string error)
        {
            error = null;

            // done if already loggged in
            if (_vault.IsLoggedIn)
                return true;

            // Login... 
            try
            {
                // A real world application will usually need to implement a try/catch pattern around LoginAuto becuase even something seemingly 
                // benign like the user clicking the X button in the subsequently displayed PDM login dialog (blocks thread) box WILL cause 
                // LoginAuto to throw a COMException. This is the intended behavior of the API, many different members defined on various 
                // interface types across the PDM API are designed to relay COM HRESULT values to .NET like this by throwing a new/related COMException.

                _vault.LoginAuto(vaultName, 0);

                return true; // no exception thrown...
            }
            catch (COMException ex)
            {
                error = ex.Message;
                return false;
            }
        }
    }
}
