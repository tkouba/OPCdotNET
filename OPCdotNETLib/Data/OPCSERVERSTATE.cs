using System;

namespace OPC.Data
{
    /// <summary>
    /// OPC server state
    /// </summary>
    public enum OPCSERVERSTATE
    {
        /// <summary>
        /// The server is running normally. This is the usual state for a server.
        /// </summary>
        OPC_STATUS_RUNNING = 1,
        /// <summary>
        /// A vendor specific fatal error has occurred within the server. The
        /// server is no longer functioning. The recovery procedure from this
        /// situation is vendor specific. An error code of E_FAIL should
        /// generally be returned from any other server method.
        /// </summary>
        OPC_STATUS_FAILED = 2,
        /// <summary>
        /// The server is running but has no configuration information loaded
        /// and thus cannot function normally. Note this state implies that the
        /// server needs configuration information in order to function.
        /// Servers which do not require configuration information should
        /// not return this state.
        /// </summary>
        OPC_STATUS_NOCONFIG = 3,
        /// <summary>
        /// The server has been temporarily suspended via some vendor
        /// specific method and is not getting or sending data. Note that
        /// Quality will be returned as OPC_QUALITY_OUT_OF_SERVICE.
        /// </summary>
        OPC_STATUS_SUSPENDED = 4,
        /// <summary>
        /// The server is in Test Mode. The outputs are disconnected from
        /// the real hardware but the server will otherwise behave normally.
        /// Inputs may be real or may be simulated depending on the vendor
        /// implementation. Quality will generally be returned normally.
        /// </summary>
        OPC_STATUS_TEST = 5
    }
}
