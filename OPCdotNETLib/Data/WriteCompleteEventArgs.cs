using System;

namespace OPC.Data
{
    // IOPCAsyncIO2

    public delegate void WriteCompleteEventHandler(object sender, WriteCompleteEventArgs e);

    /// <summary>
    /// 
    /// </summary>
    public class WriteCompleteEventArgs : EventArgs
    {
        /// <summary>
        /// Client defined trnsaction ID
        /// </summary>
        public int TransactionID { get; set; }
        /// <summary>
        /// Client group handle
        /// </summary>
        public int GroupHandleClient { get; set; }
        /// <summary>
        /// Master error
        /// </summary>
        public int MasterError { get; set; }

        /// <summary>
        /// Item write results <seealso cref="OpcWriteResult"/>
        /// </summary>
        public OpcWriteResult[] WriteResults { get; set; }

        /// <summary>
        /// Constructor
        /// </summary>
        public WriteCompleteEventArgs()
        {
        }

        internal string DebuggerDisplay
        {
            get { return $"{GetType().Name}:: TrID:{TransactionID}, GhC:{GroupHandleClient}, ME:{MasterError}"; }
        }

    }
}
