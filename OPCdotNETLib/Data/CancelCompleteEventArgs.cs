using System;
using System.Diagnostics;

namespace OPC.Data
{
    /// <summary>
    /// Cancel completed event handler
    /// </summary>
    /// <param name="sender">Sender</param>
    /// <param name="e">Cancel completed event arguments</param>
    public delegate void CancelCompleteEventHandler(object sender, CancelCompleteEventArgs e);

    /// <summary>
    /// Cancel completed event arguments
    /// </summary>
    [DebuggerDisplay("{DebuggerDisplay,nq}")]
    public class CancelCompleteEventArgs : EventArgs
    {
        /// <summary>
        /// Client defined trnsaction ID
        /// </summary>
        public int TransactionID { get; private set; }
        /// <summary>
        /// Client group handle
        /// </summary>
        public int GroupHandleClient { get; private set; }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="transactionID">Client defined transaction ID</param>
        /// <param name="groupHandleClient">Client group handle</param>
        public CancelCompleteEventArgs(int transactionID, int groupHandleClient)
        {
            TransactionID = transactionID;
            GroupHandleClient = groupHandleClient;
        }

        internal string DebuggerDisplay
        {
            get { return $"{GetType().Name}:: TrID:{TransactionID}, GhC:{GroupHandleClient}"; }
        }

    }
}
