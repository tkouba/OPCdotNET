using System;
using System.Diagnostics;

namespace OPC.Data
{
    /// <summary>
    /// Data changed event handler
    /// </summary>
    /// <param name="sender">Sender</param>
    /// <param name="e">Data changed event arguments</param>
    public delegate void DataChangeEventHandler(object sender, DataChangeEventArgs e);

    /// <summary>
    /// Data changed event arguments
    /// </summary>
    [DebuggerDisplay("{DebuggerDisplay,nq}")]
    public class DataChangeEventArgs : EventArgs
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
        /// Master quality
        /// </summary>
        public int MasterQuality { get; set; }
        /// <summary>
        /// Master error
        /// </summary>
        public int MasterError { get; set; }

        /// <summary>
        /// Item states <seealso cref="OpcItemState"/>
        /// </summary>
        public OpcItemState[] ItemStates { get; set; }

        /// <summary>
        /// Constructor
        /// </summary>
        public DataChangeEventArgs()
        {
        }

        internal string DebuggerDisplay
        {
            get { return $"{GetType().Name}:: TrID:{TransactionID}, GhC:{GroupHandleClient}, MQ:{MasterQuality}, ME:{MasterError}"; }
        }
    }
}
