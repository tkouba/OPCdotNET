using System;


namespace OPC.Common
{
    /// <summary>
    /// HRESULTS - OPC error codes
    /// </summary>
    public static class HRESULTS
    {
        /// <summary>
        /// Failed
        /// </summary>
        /// <param name="hResultCode">hResult code</param>
        /// <returns>Bool</returns>
        public static bool Failed(int hResultCode)
        {
            return (hResultCode < 0);
        }

        /// <summary>
        /// Succeeded
        /// </summary>
        /// <param name="hResultCode">hResult code</param>
        /// <returns>Bool</returns>
        public static bool Succeeded(int hResultCode)
        {
            return (hResultCode >= 0);
        }

        /// <summary>
        /// Operation successful.
        /// </summary>
        public const int S_OK = 0x00000000;
        /// <summary>
        /// Operation successful. False result.
        /// </summary>
        public const int S_FALSE = 0x00000001;

        #region winerror.h
        /// <summary>
        /// Not implemented.
        /// </summary>
        public const int E_NOTIMPL = unchecked((int)0x80004001);		
        /// <summary>
        /// No such interface supported.
        /// </summary>
        public const int E_NOINTERFACE = unchecked((int)0x80004002);
        /// <summary>
        /// Operation aborted.
        /// </summary>
        public const int E_ABORT = unchecked((int)0x80004004);
        /// <summary>
        /// Unspecified failure.
        /// </summary>
        public const int E_FAIL = unchecked((int)0x80004005);
        /// <summary>
        /// Failed to allocate necessary memory.
        /// </summary>
        public const int E_OUTOFMEMORY = unchecked((int)0x8007000E);
        /// <summary>
        /// One or more arguments are not valid.
        /// </summary>
        public const int E_INVALIDARG = unchecked((int)0x80070057);
        #endregion

        #region olectl.h
        /// <summary>
        /// There is no connection for this connection id.
        /// </summary>
        public const int CONNECT_E_NOCONNECTION = unchecked((int)0x80040200);	
        /// <summary>
        /// This implementation's limit for advisory connections has been reached
        /// </summary>
        public const int CONNECT_E_ADVISELIMIT = unchecked((int)0x80040201);
        /// <summary>
        /// Connection attempt failed
        /// </summary>
        public const int CONNECT_E_CANNOTCONNECT = unchecked((int)0x80040202);
        /// <summary>
        /// Must use a derived interface to connect
        /// </summary>
        public const int CONNECT_E_OVERRIDDEN = unchecked((int)0x80040203);
        #endregion

        #region opcerror.h
        /// <summary>
        /// The value of the handle is invalid.
        /// </summary>
        public const int OPC_E_INVALIDHANDLE = unchecked((int)0xC0040001);
        /// <summary>
        /// The server cannot convert the data between the specified format and/or requested data type and the canonical data type. 
        /// </summary>
        public const int OPC_E_BADTYPE = unchecked((int)0xC0040004);
        /// <summary>
        /// The requested operation cannot be done on a public group.
        /// </summary>
        public const int OPC_E_PUBLIC = unchecked((int)0xC0040005);
        /// <summary>
        /// The item's access rights do not allow the operation.
        /// </summary>
        public const int OPC_E_BADRIGHTS = unchecked((int)0xC0040006);
        /// <summary>
        /// The item ID is not defined in the server address space or no longer exists in the server address space.
        /// </summary>
        public const int OPC_E_UNKNOWNITEMID = unchecked((int)0xC0040007);
        /// <summary>
        /// The item ID does not conform to the server's syntax.
        /// </summary>
        public const int OPC_E_INVALIDITEMID = unchecked((int)0xC0040008);
        /// <summary>
        /// The filter string was not valid.
        /// </summary>
        public const int OPC_E_INVALIDFILTER = unchecked((int)0xC0040009);
        /// <summary>
        /// The item's access path is not known to the server.
        /// </summary>
        public const int OPC_E_UNKNOWNPATH = unchecked((int)0xC004000A);
        /// <summary>
        /// The value was out of range.
        /// </summary>
        public const int OPC_E_RANGE = unchecked((int)0xC004000B);
        /// <summary>
        /// Duplicate name not allowed.
        /// </summary>
        public const int OPC_E_DUPLICATENAME = unchecked((int)0xC004000C);
        /// <summary>
        /// The server does not support the requested data rate but will use the closest available rate.
        /// </summary>
        public const int OPC_S_UNSUPPORTEDRATE = unchecked((int)0x0004000D);
        /// <summary>
        /// A value passed to write was accepted but the output was clamped.
        /// </summary>
        public const int OPC_S_CLAMP = unchecked((int)0x0004000E);
        /// <summary>
        /// The operation cannot be performed because the object is bering referenced.
        /// </summary>
        public const int OPC_S_INUSE = unchecked((int)0x0004000F);
        /// <summary>
        /// The server's configuration file is an invalid format.
        /// </summary>
        public const int OPC_E_INVALIDCONFIGFILE = unchecked((int)0xC0040010);
        /// <summary>
        /// The requested object (e.g. a public group) was not found.
        /// </summary>
        public const int OPC_E_NOTFOUND = unchecked((int)0xC0040011);
        /// <summary>
        /// The specified property ID is not valid for the item.
        /// </summary>
        public const int OPC_E_INVALID_PID = unchecked((int)0xC0040203);
        /// <summary>
        /// The item deadband has not been set for this item.
        /// </summary>
        public const int OPC_E_DEADBANDNOTSET = unchecked((int)0xC0040400);
        /// <summary>
        /// The item does not support deadband.
        /// </summary>
        public const int OPC_E_DEADBANDNOTSUPPORTED = unchecked((int)0xC0040401);
        /// <summary>
        /// The server does not support buffering of data items that are collected at a faster rate than the group update rate.
        /// </summary>
        public const int OPC_E_NOBUFFERING = unchecked((int)0xC0040402);
        /// <summary>
        /// The continuation point is not valid.
        /// </summary>
        public const int OPC_E_INVALIDCONTINUATIONPOINT = unchecked((int)0xC0040403);
        /// <summary>
        /// Not every detected change has been returned since the server's buffer reached its limit and had to purge out the oldest data.
        /// </summary>
        public const int OPC_S_DATAQUEUEOVERFLOW = unchecked((int)0x00040404);
        /// <summary>
        /// There is no sampling rate set for the specified item.  
        /// </summary>
        public const int OPC_E_RATENOTSET = unchecked((int)0xC0040405);
        /// <summary>
        /// The server does not support writing of quality and/or timestamp.
        /// </summary>
        public const int OPC_E_NOTSUPPORTED = unchecked((int)0xC0040406);
        /// <summary>
        /// The dictionary and/or type description for the item has changed.
        /// </summary>
        public const int OPCCPX_E_TYPE_CHANGED = unchecked((int)0xC0040407);
        /// <summary>
        /// A data filter item with the specified name already exists. 
        /// </summary>
        public const int OPCCPX_E_FILTER_DUPLICATE = unchecked((int)0xC0040408);
        /// <summary>
        /// The data filter value does not conform to the server's syntax.
        /// </summary>
        public const int OPCCPX_E_FILTER_INVALID = unchecked((int)0xC0040409);
        /// <summary>
        /// An error occurred when the filter value was applied to the source data.
        /// </summary>
        public const int OPCCPX_E_FILTER_ERROR = unchecked((int)0xC004040A);
        /// <summary>
        /// The item value is empty because the data filter has excluded all fields.
        /// </summary>
        public const int OPCCPX_S_FILTER_NO_DATA = unchecked((int)0x0004040B);
        #endregion

    }
}
