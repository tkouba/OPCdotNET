

using System;
using System.Text;
using System.Collections;
using System.Runtime.InteropServices;
using System.Reflection;

using OPC.Common;
using OPC.Common.Interface;
using OPC.Data.Interface;
using System.Diagnostics;
using System.Runtime.InteropServices.ComTypes;
using System.Collections.Generic;

namespace OPC.Data
{
    /// <summary>
    /// OPC Server object wrapper class
    /// </summary>
    /// <remarks>
    /// This is the primary object for OPC Server connection. 
    /// After connection use <see cref="AddGroup(string, bool, int)"/> to add new OPC Group and access to items.
    /// </remarks>
    public class OpcServer : IOPCShutdown, IDisposable
    {

        private event ShutdownRequestEventHandler shutdownRequested;
        /// <summary>
        /// OPC Server shutdown requested
        /// </summary>
        public event ShutdownRequestEventHandler ShutdownRequested
        {
            add { shutdownRequested += value; }
            remove { shutdownRequested -= value; }
        }

        /// <summary>
        /// Object is disposed.
        /// </summary>
        public bool IsDisposed { get; private set; }

        private object opcServerObject = null;

        private IConnectionPoint shutdownConnectionPoint = null;
        private int shutdownCookie = 0;

        private IOPCServer opcServer { get { return (IOPCServer)opcServerObject; } }
        private IOPCCommon opcCommon { get { return (IOPCCommon)opcServerObject; } }
        private IOPCBrowseServerAddressSpace opcBrowseServer { get { return (IOPCBrowseServerAddressSpace)opcServerObject; } }
        private IOPCItemProperties opcItemProperties { get { return (IOPCItemProperties)opcServerObject; } }
        private IConnectionPointContainer connectionPointContainer { get { return (IConnectionPointContainer)opcServerObject; } }

        /// <summary>
        /// Constructor
        /// </summary>
        public OpcServer()
        {
            IsDisposed = false;
        }

        /// <summary>
        /// Finalizer
        /// </summary>
        ~OpcServer()
        {
            try
            {
                InternalDisconnect();
            }
            catch { /* NOP */ }
        }

        /// <summary>
        /// Connect to the local OPC Server with the specified CLSID.
        /// </summary>
        /// <param name="clsID">CLSID of the OPC Server. Available CLSID could be listed by <see cref="OpcServerList.ListAllData20"/></param>
        /// <remarks>
        /// The Disconnect method needs to be called to release the OPC server before the client program exits.
        /// </remarks>
        public void Connect(Guid clsID)
        {
            ThrowIfDisposed();
            InternalDisconnect();
            Connect(Type.GetTypeFromCLSID(clsID));
        }

        /// <summary>
        /// Connect to the remote OPC Server with the specified CLSID.
        /// </summary>
        /// <param name="clsID">CLSID of the OPC Server. Available CLSID could be listed by <see cref="OpcServerList.ListAllData20"/></param>
        /// <param name="host">Network name of the computer on which the OPC server is to be accessed.</param>
        /// <remarks>
        /// The Disconnect method needs to be called to release the OPC server before the client program exits.
        /// </remarks>
        [Obsolete("Network access is not recommended.")]
        public void Connect(Guid clsID, string host)
        {
            ThrowIfDisposed();
            InternalDisconnect();
            Connect(Type.GetTypeFromCLSID(clsID, host));
        }

        /// <summary>
        /// Connect to the local OPC Server with the specified ProgID.
        /// </summary>
        /// <param name="progID">ProgID of the OPC Server. Available ProgID could be listed by <see cref="OpcServerList.ListAllData20"/></param>
        /// <remarks>
        /// The Disconnect method needs to be called to release the OPC server before the client program exits.
        /// </remarks>
        public void Connect(string progID)
        {
            ThrowIfDisposed();
            InternalDisconnect();
            Connect(Type.GetTypeFromProgID(progID));
        }

        /// <summary>
        ///  Connect to the remote OPC Server with the specified ProgID.
        /// </summary>
        /// <param name="progID">ProgID of the OPC Server. Available ProgID could be listed by <see cref="OpcServerList.ListAllData20"/></param>
        /// <param name="host">Network name of the computer on which the OPC server is to be accessed.</param>
        /// <remarks>
        /// The Disconnect method needs to be called to release the OPC server before the client program exits.
        /// </remarks>
        [Obsolete("Network access is not recommended.")]
        public void Connect(string progID, string host)
        {
            ThrowIfDisposed();
            InternalDisconnect();
            Connect(Type.GetTypeFromProgID(progID, host));
        }

        /// <summary>
        /// Internal connect to OPC Server by specified type returned 
        /// from <see cref="Type.GetTypeFromCLSID(Guid, string)"/> or <see cref="Type.GetTypeFromProgID(string, string)"/>
        /// </summary>
        /// <param name="typeofOPCserver"></param>
        private void Connect(Type typeofOPCserver)
        {
            if (typeofOPCserver == null)
                Marshal.ThrowExceptionForHR(HRESULTS.OPC_E_NOTFOUND);

            opcServerObject = Activator.CreateInstance(typeofOPCserver);
            if (opcServer == null)
                Marshal.ThrowExceptionForHR(HRESULTS.CONNECT_E_NOCONNECTION);

            // Advice IOPCShutdown
            Guid sinkGuid = typeof(IOPCShutdown).GUID;
            connectionPointContainer.FindConnectionPoint(ref sinkGuid, out shutdownConnectionPoint);
            if (shutdownConnectionPoint != null)
                shutdownConnectionPoint.Advise(this, out shutdownCookie);
        }

        /// <summary>
        /// Disconnect from the OPC server. The unmanaged resources such as COM interface are released.
        /// </summary>
        public void Disconnect()
        {
            ThrowIfDisposed();
            InternalDisconnect();
        }

        private void InternalDisconnect()
        {
            if (!(shutdownConnectionPoint == null))
            {
                if (shutdownCookie != 0)
                {
                    shutdownConnectionPoint.Unadvise(shutdownCookie);
                    shutdownCookie = 0;
                }
                int rc = Marshal.ReleaseComObject(shutdownConnectionPoint);
                shutdownConnectionPoint = null;
            }

            if (!(opcServerObject == null))
            {
                int rc = Marshal.ReleaseComObject(opcServerObject);
                opcServerObject = null;
            }
        }

        /// <summary>
        /// Get the current status of the OPC Server.
        /// </summary>
        /// <returns><see cref="SERVERSTATUS"/> object for current session.</returns>
        public SERVERSTATUS GetStatus()
        {
            ThrowIfDisposed();
            opcServer.GetStatus(out SERVERSTATUS serverStatus);
            return serverStatus;
        }

        /// <summary>
        /// Returns the description for the specified error.
        /// </summary>
        /// <param name="errorCode">Error code.</param>
        /// <param name="localeID">Requested LocaleID</param>
        /// <returns>Error description</returns>
        public string GetErrorString(int errorCode, int localeID)
        {
            ThrowIfDisposed();
            opcServer.GetErrorString(errorCode, localeID, out string errorString);
            return errorString;
        }

        #region OPC Group
        /// <summary>
        /// Add a new <see cref="OpcGroup"/> with default bias time, deadband and locale ID.
        /// </summary>
        /// <param name="groupName">Name of the new group. The name must be unique for this client. 
        /// If the name is <see cref="String.Empty"/> the server will generate an unique name.</param>
        /// <param name="setActive">Initial active state. true for active and false for inactive.</param>
        /// <param name="requestedUpdateRate">The fastest rate (milliseconds) at which data changes may be sent for items in this group. 
        /// Zero indicates the fastest practical rate.</param>
        /// <returns><see cref="OpcGroup"/> object</returns>
        /// <remarks>
        /// Requseted update rate and server polling rate of the hardware internally is depends on server implementation and could be diffferent.
        /// </remarks>
        public OpcGroup AddGroup(string groupName, bool setActive, int requestedUpdateRate)
        {
            return AddGroup(groupName, setActive, requestedUpdateRate, null, null, 0);
        }

        /// <summary>
        /// Add a new <see cref="OpcGroup"/>
        /// </summary>
        /// <param name="groupName">Name of the new group. The name must be unique for this client. 
        /// If the name is <see cref="String.Empty"/> the server will generate an unique name.</param>
        /// <param name="setActive">Initial active state. true for active and false for inactive.</param>
        /// <param name="requestedUpdateRate">The fastest rate (milliseconds) at which data changes may be sent for items in this group. 
        /// Zero indicates the fastest practical rate.</param>
        /// <param name="biasTime">Provides the information needed to convert the time stamp on the data 
        /// back to the local time of the device.</param>
        /// <param name="percentDeadband">DeadBand value in percent.</param>
        /// <param name="localeID">Requested LocaleID or zero.</param>
        /// <returns><see cref="OpcGroup"/> object</returns>
        /// <remarks>
        /// Requseted update rate and server polling rate of the hardware internally is depends on server implementation and could be diffferent.
        /// </remarks>
        public OpcGroup AddGroup(string groupName, bool setActive, int requestedUpdateRate,
            int? biasTime, float? percentDeadband, int localeID)
        {
            ThrowIfDisposed();
            if (opcServer == null)
                Marshal.ThrowExceptionForHR(HRESULTS.E_ABORT);

            return new OpcGroup(opcServer, false, groupName, setActive, requestedUpdateRate, biasTime, percentDeadband, localeID);
        }

        /// <summary>
        /// Get public group by name (indirect)
        /// </summary>
        /// <param name="groupName"></param>
        /// <returns></returns>
        public OpcGroup GetPublicGroup(string groupName)
        {
            ThrowIfDisposed();
            if (opcServer == null)
                Marshal.ThrowExceptionForHR(HRESULTS.E_ABORT);

            return new OpcGroup(opcServer, true, groupName, false, 1000, null, null, 0);
        }
        #endregion

        #region IOPCCommon
        /// <summary>
        /// Set LocaleID for this session.
        /// </summary>
        /// <param name="lcid"></param>
        public void SetLocaleID(int lcid)
        {
            ThrowIfDisposed();
            opcCommon.SetLocaleID(lcid);
        }

        /// <summary>
        /// Get the current LocaleID.
        /// </summary>
        /// <param name="lcid"></param>
        public void GetLocaleID(out int lcid)
        {
            ThrowIfDisposed();
            opcCommon.GetLocaleID(out lcid);
        }

        /// <summary>
        /// Query the LocaleIDs supported by the OPC Server. 
        /// </summary>
        /// <returns>An array with LocaleIDs</returns>
        public int[] QueryAvailableLocaleIDs()
        {
            ThrowIfDisposed();
            int[] lcids;
            IntPtr ptrIds = IntPtr.Zero;
            try
            {
                int hResult = opcCommon.QueryAvailableLocaleIDs(out int count, out ptrIds);
                if (HRESULTS.Failed(hResult))
                    Marshal.ThrowExceptionForHR(hResult);
                if (((int)ptrIds) == 0)
                    return new int[0];
                if (count < 1)
                    return new int[0];

                lcids = new int[count];
                Marshal.Copy(ptrIds, lcids, 0, count);
            }
            finally
            {
                if (ptrIds != IntPtr.Zero)
                    Marshal.FreeCoTaskMem(ptrIds);
            }
            return lcids;
        }

        /// <summary>
        /// Set a client name.
        /// </summary>
        /// <param name="name">new client name</param>
        /// <remarks>Allows the client to set optional name for debugging purposes.</remarks>
        public void SetClientName(string name)
        {
            ThrowIfDisposed();
            opcCommon.SetClientName(name);
        }
        #endregion

        #region IOPCBrowseServerAddressSpace
        /// <summary>
        /// Query the server items organization.
        /// </summary>
        /// <returns></returns>
        public OPCNAMESPACETYPE QueryOrganization()
        {
            ThrowIfDisposed();
            opcBrowseServer.QueryOrganization(out OPCNAMESPACETYPE ns);
            return ns;
        }

        /// <summary>
        /// Change the current browse position.
        /// </summary>
        /// <param name="direction">Browse direction</param>
        /// <param name="name"></param>
        public void ChangeBrowsePosition(OPCBROWSEDIRECTION direction, string name)
        {
            ThrowIfDisposed();
            opcBrowseServer.ChangeBrowsePosition(direction, name);
        }

        /// <summary>
        /// Get the full item name of the specified name in the current branch.
        /// </summary>
        /// <param name="itemID">Item ID</param>
        /// <returns>Fully qualified item ID</returns>
        public string GetItemID(string itemID)
        {
            ThrowIfDisposed();
            opcBrowseServer.GetItemID(itemID, out string fullItemID);
            return fullItemID;
        }

        /// <summary>
        /// Browse available access paths for item ID.
        /// </summary>
        /// <param name="itemID">Item ID</param>
        /// <returns>Available access paths</returns>
        public string[] BrowseAccessPaths(string itemID)
        {
            ThrowIfDisposed();
            object enumerator = null;
            try
            {
                opcBrowseServer.BrowseAccessPaths(itemID, out enumerator);
                return IEnumStringToArray((IEnumString)enumerator);
            }
            finally
            {
                if (enumerator != null)
                    Marshal.ReleaseComObject(enumerator);
            }
        }

        /// <summary>
        /// Browse items
        /// </summary>
        /// <param name="filterType"><see cref="OPCBROWSETYPE"/> filter, ignored for <see cref="OPCNAMESPACETYPE.OPC_NS_FLAT"/> address space.
        /// </param>
        public string[] BrowseItemIDs(OPCBROWSETYPE filterType)
        {
            return BrowseItemIDs(filterType, String.Empty, VarEnum.VT_EMPTY, 0);
        }

        /// <summary>
        /// Browse items
        /// </summary>
        /// <param name="filterType"><see cref="OPCBROWSETYPE"/> filter, ignored for <see cref="OPCNAMESPACETYPE.OPC_NS_FLAT"/> address space.</param>
        /// <param name="filterCriteria">A server specific filter string.</param>
        /// <returns></returns>
        public string[] BrowseItemIDs(OPCBROWSETYPE filterType, string filterCriteria)
        {
            return BrowseItemIDs(filterType, filterCriteria, VarEnum.VT_EMPTY, OPCACCESSRIGHTS.OPC_UNKNOWN);
        }

        /// <summary>
        /// Browse items
        /// </summary>
        /// <param name="filterType"><see cref="OPCBROWSETYPE"/> filter, ignored for <see cref="OPCNAMESPACETYPE.OPC_NS_FLAT"/> address space.</param>
        /// <param name="filterCriteria">A server specific filter string.</param>
        /// <param name="dataTypeFilter">Data type filter or <see cref="VarEnum.VT_EMPTY"/></param>
        /// <param name="accessRightsFilter"><see cref="OPCACCESSRIGHTS"/> filter.</param>
        /// <returns></returns>
        public string[] BrowseItemIDs(OPCBROWSETYPE filterType, string filterCriteria,
            VarEnum dataTypeFilter, OPCACCESSRIGHTS accessRightsFilter)
        {
            ThrowIfDisposed();
            object enumerator = null;
            try
            {
                opcBrowseServer.BrowseOPCItemIDs(filterType, filterCriteria, (short)dataTypeFilter, accessRightsFilter, out enumerator);
                return IEnumStringToArray((IEnumString)enumerator);
            }
            finally
            {
                if (enumerator != null)
                    Marshal.ReleaseComObject(enumerator);
            }
        }
        #endregion

        #region IOPCItemProperties
        /// <summary>
        /// Query available item properties
        /// </summary>
        /// <param name="itemID">Item ID to query</param>
        /// <returns>Available properties</returns>
        public OpcProperty[] QueryAvailableProperties(string itemID)
        {
            ThrowIfDisposed();
            IntPtr ptrID = IntPtr.Zero;
            IntPtr ptrDesc = IntPtr.Zero;
            IntPtr ptrTyp = IntPtr.Zero;
            try
            {
                opcItemProperties.QueryAvailableProperties(itemID, out int count, out ptrID, out ptrDesc, out ptrTyp);

                if ((count == 0) || (count > 0xFFF))
                    return new OpcProperty[0];

                if ((ptrID == IntPtr.Zero) || (ptrDesc == IntPtr.Zero) || (ptrTyp == IntPtr.Zero))
                    Marshal.ThrowExceptionForHR(HRESULTS.E_ABORT);

                OpcProperty[] opcProperties = new OpcProperty[count];

                IntPtr ptrString;
                for (int i = 0; i < count; i++)
                {
                    ptrString = (IntPtr)Extensions.ReadInt32(ptrDesc, i);
                    opcProperties[i] = new OpcProperty
                    {
                        PropertyID = Extensions.ReadInt32(ptrID, i),
                        Description = Marshal.PtrToStringUni(ptrString),
                        DataType = (VarEnum)Extensions.ReadInt16(ptrTyp, i)
                    };
                    Marshal.FreeCoTaskMem(ptrString);
                }
                return opcProperties;
            }
            finally
            {
                if (ptrID != IntPtr.Zero)
                    Marshal.FreeCoTaskMem(ptrID);
                if (ptrDesc != IntPtr.Zero)
                    Marshal.FreeCoTaskMem(ptrDesc);
                if (ptrTyp != IntPtr.Zero)
                    Marshal.FreeCoTaskMem(ptrTyp);
            }
        }

        /// <summary>
        /// Get item property values
        /// </summary>
        /// <param name="itemID">Item ID</param>
        /// <param name="propertyIDs">Property IDs to get values for</param>
        /// <returns>Property data</returns>
        public OpcPropertyData[] GetItemProperties(string itemID, int[] propertyIDs)
        {
            ThrowIfDisposed();
            int count = propertyIDs.Length;
            if (count < 1)
                return new OpcPropertyData[0];

            IntPtr ptrDat = IntPtr.Zero;
            IntPtr ptrErr = IntPtr.Zero;
            try
            {
                int hresult = opcItemProperties.GetItemProperties(itemID, count, propertyIDs, out ptrDat, out ptrErr);
                if (HRESULTS.Failed(hresult))
                    Marshal.ThrowExceptionForHR(hresult);

                if ((ptrDat == IntPtr.Zero) || (ptrErr == IntPtr.Zero))
                    Marshal.ThrowExceptionForHR(HRESULTS.E_ABORT);

                OpcPropertyData[] propertiesData = new OpcPropertyData[count];

                for (int i = 0; i < count; i++)
                {
                    propertiesData[i] = new OpcPropertyData
                    {
                        PropertyID = propertyIDs[i],
                        Error = Extensions.ReadInt32(ptrErr, i)
                    };

                    if (propertiesData[i].Error == HRESULTS.S_OK)
                    {
                        propertiesData[i].Data = Extensions.GetObjectForNativeVariant(ptrDat, i);
                        Extensions.VariantClear(ptrDat, i);
                    }
                    else
                        propertiesData[i].Data = null;
                }
                return propertiesData;
            }
            finally
            {
                if (ptrDat != IntPtr.Zero)
                    Marshal.FreeCoTaskMem(ptrDat);
                if (ptrErr != IntPtr.Zero)
                    Marshal.FreeCoTaskMem(ptrErr);
            }
        }

        /// <summary>
        /// Get a list of <see cref="OpcPropertyItem"/> for the specified item ID.
        /// </summary>
        /// <param name="itemID">The item ID to get the list of properties</param>
        /// <param name="propertyIDs">An array of requested property IDs, <see cref="QueryAvailableProperties"/>.</param>
        /// <returns></returns>
        public OpcPropertyItem[] LookupItemIDs(string itemID, int[] propertyIDs)
        {
            ThrowIfDisposed();
            int count = propertyIDs.Length;
            if (count < 1)
                return new OpcPropertyItem[0];

            IntPtr ptrErr = IntPtr.Zero;
            IntPtr ptrIds = IntPtr.Zero;
            try
            {
                int hresult = opcItemProperties.LookupItemIDs(itemID, count, propertyIDs, out ptrIds, out ptrErr);
                if (HRESULTS.Failed(hresult))
                    Marshal.ThrowExceptionForHR(hresult);

                if ((ptrIds == IntPtr.Zero) || (ptrErr == IntPtr.Zero))
                    Marshal.ThrowExceptionForHR(HRESULTS.E_ABORT);

                OpcPropertyItem[] propertyItems = new OpcPropertyItem[count];

                IntPtr ptrString;
                for (int i = 0; i < count; i++)
                {
                    propertyItems[i] = new OpcPropertyItem
                    {
                        PropertyID = propertyIDs[i],
                        Error = Extensions.ReadInt32(ptrErr, i)
                    };

                    if (propertyItems[i].Error == HRESULTS.S_OK)
                    {
                        ptrString = (IntPtr)Extensions.ReadInt32(ptrIds, i);
                        propertyItems[i].NewItemID = Marshal.PtrToStringUni(ptrString);
                        Marshal.FreeCoTaskMem(ptrString);
                    }
                    else
                        propertyItems[i].NewItemID = null;

                }
                return propertyItems;
            }
            finally
            {
                if (ptrIds != IntPtr.Zero)
                    Marshal.FreeCoTaskMem(ptrIds);
                if (ptrErr != IntPtr.Zero)
                    Marshal.FreeCoTaskMem(ptrErr);
            }

        }
        #endregion

        #region IOPCShutdown
        void IOPCShutdown.ShutdownRequest(string shutdownReason)
        {
            shutdownRequested?.Invoke(this, new ShutdownRequestEventArgs(shutdownReason));
        }
        #endregion

        #region IDisposable Support
        private void ThrowIfDisposed()
        {
            if (IsDisposed)
                throw new ObjectDisposedException(nameof(OpcServer));
        }

        void IDisposable.Dispose()
        {
            InternalDisconnect();
            IsDisposed = true;
            GC.SuppressFinalize(this);
        }
        #endregion

        private string[] IEnumStringToArray(IEnumString enumerator)
        {
            List<string> lst = new List<string>();
            if (enumerator != null)
            {
                int cft;
                string[] strF = new string[100];
                int hresult;
                IntPtr intPtr = Marshal.AllocCoTaskMem(sizeof(int));
                try
                {
                    do
                    {
                        hresult = enumerator.Next(100, strF, intPtr);
                        if (HRESULTS.Failed(hresult))
                            Marshal.ThrowExceptionForHR(hresult);
                        cft = Marshal.ReadInt32(intPtr);
                        if (cft > 0)
                        {
                            for (int i = 0; i < cft; i++)
                                lst.Add(strF[i]);
                        }
                    }
                    while (hresult == HRESULTS.S_OK);
                }
                finally
                {
                    if (intPtr != IntPtr.Zero)
                        Marshal.FreeCoTaskMem(intPtr);
                }
            }
            return lst.ToArray();
        }

    }

}
