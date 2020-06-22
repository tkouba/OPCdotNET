using System;
using System.Diagnostics;

namespace OPC.Data
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    public delegate void ReadCompleteEventHandler(object sender, ReadCompleteEventArgs e);

    /// <summary>
    /// 
    /// </summary>
    [DebuggerDisplay("{DebuggerDisplay,nq}")]
    public class ReadCompleteEventArgs : EventArgs
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
        public ReadCompleteEventArgs()
        {
        }

        internal string DebuggerDisplay
        {
            get { return $"{GetType().Name}:: TrID:{TransactionID}, GhC:{GroupHandleClient}, MQ:{MasterQuality}, ME:{MasterError}"; }
        }

    }
}
