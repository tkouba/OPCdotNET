using System;

namespace OPC.Data
{
    /// <summary>
    /// OPC item engineering units
    /// </summary>
    public enum OPCEUTYPE
    {
        /// <summary>
        /// No EU information available (EUInfo will be VT_EMPTY).
        /// </summary>
        OPC_NOENUM = 0,
        /// <summary>
        /// Analog - EUInfo will contain a SAFEARRAY of exactly two doubles (VT_ARRAY | VT_R8) corresponding to the LOW and HI EU range.
        /// </summary>
        OPC_ANALOG = 1,
        /// <summary>
        /// Enumerated - EUInfo will contain a SAFEARRAY of strings (VT_ARRAY | VT_BSTR) which 
        /// contains a list of strings (Example: “OPEN”, “CLOSE”, “IN TRANSIT”, etc.) corresponding to
        /// sequential numeric values (0, 1, 2, etc.)
        /// </summary>
        OPC_ENUMERATED = 2
    }
}
