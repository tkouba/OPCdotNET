using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Text;


namespace OPC.Common
{
    /// <summary>
    /// Opc Server definition
    /// </summary>
    [DebuggerDisplay("{DebuggerDisplay,nq}")]
    public class OpcServerDefinition
    {
        /// <summary>
        /// Class identifier (CLSID)
        /// </summary>
        public Guid ClsID;
        /// <summary>
        /// Program identifier (ProgID)
        /// </summary>
        public string ProgID;
        /// <summary>
        /// Server name
        /// </summary>
        public string Name;

        internal string DebuggerDisplay
        {
            get { return $"'{Name}' ID={ProgID} [{ClsID}]"; }
        }
    } 
} 
