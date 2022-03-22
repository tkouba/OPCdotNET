using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;
using System.Text;
using OPC.Common;
using OPC.Data.Interface;

namespace OPC.Data
{

    /// <summary>
    /// OPC Group
    /// </summary>
    public class OpcGroup : IOPCDataCallback, IDisposable, IEnumerable<OpcItemAttributes>
    {
        // marshaling helpers:
        private readonly static Type typeOPCITEMDEF = typeof(InternalOPCITEMDEF);
        private readonly static int sizeOPCITEMDEF = Marshal.SizeOf(typeOPCITEMDEF);
        private readonly static Type typeOPCITEMRESULT = typeof(InternalOPCITEMRESULT);
        private readonly static int sizeOPCITEMRESULT = Marshal.SizeOf(typeOPCITEMRESULT);
        private readonly static int sizeOPCREADRESULT = 32;

        private IOPCServer parentServer = null;

        private object opcGroupObject;

        private IConnectionPoint callbackConnectionPoint = null;
        private int callbackCookie = 0;

        private bool statePublic;
        private int stateUpdateRate;
        private bool stateActive;
        private int stateTimeBias;
        private float statePercentDeadband;
        private int stateLocaleID;
        private int stateHandleClient;
        private int stateHandleServer;

        private IOPCGroupStateMgt opcGroupStateMgt { get { return (IOPCGroupStateMgt)opcGroupObject; } }
        private IOPCItemMgt opcItemMgt { get { return (IOPCItemMgt)opcGroupObject; } }
        private IOPCSyncIO opcSyncIO { get { return (IOPCSyncIO)opcGroupObject; } }
        private IOPCAsyncIO2 opcAsyncIO { get { return (IOPCAsyncIO2)opcGroupObject; } }
        private IConnectionPointContainer connectionPointContainer { get { return (IConnectionPointContainer)opcGroupObject; } }

        private event DataChangeEventHandler dataChanged;
        /// <summary>
        /// OPC Data Changed
        /// </summary>
        public event DataChangeEventHandler DataChanged
        {
            add { dataChanged += value; }
            remove { dataChanged -= value; }
        }

        private event ReadCompleteEventHandler readCompleted;
        /// <summary>
        /// OPC Async Read Completed
        /// </summary>
        public event ReadCompleteEventHandler ReadCompleted
        {
            add { readCompleted += value; }
            remove { readCompleted -= value; }
        }

        private event WriteCompleteEventHandler writeCompleted;
        /// <summary>
        /// OPC Async Write Completed
        /// </summary>
        public event WriteCompleteEventHandler WriteCompleted
        {
            add { writeCompleted += value; }
            remove { writeCompleted -= value; }
        }

        private event CancelCompleteEventHandler cancelCompleted;
        /// <summary>
        /// 
        /// </summary>
        public event CancelCompleteEventHandler CancelCompleted
        {
            add { cancelCompleted += value; }
            remove { cancelCompleted -= value; }
        }

        /// <summary>
        /// 
        /// </summary>
        public bool IsDisposed { get; private set; }

        private string stateName;
        /// <summary>
        /// OPC Group Name
        /// </summary>
        /// <returns>String</returns>
        public string Name
        {
            get { return stateName; }
            set
            {
                opcGroupStateMgt.SetName(value);
                stateName = value;
            }
        }

        /// <summary>
        /// Active
        /// </summary>
        /// <returns>Bool</returns>
        public bool Active
        {
            get { return stateActive; }
            set
            {
                opcGroupStateMgt.SetState(null, out stateUpdateRate, new bool[1] { value }, null, null, null, null);
                stateActive = value;
            }
        }

        /// <summary>
        /// Public
        /// </summary>
        /// <returns>Bool</returns>
        public bool Public
        {
            get { return statePublic; }
        }

        /// <summary>
        /// Update rate
        /// </summary>
        /// <returns>Int</returns>
        public int UpdateRate
        {
            get { return stateUpdateRate; }
            set
            {
                opcGroupStateMgt.SetState(new int[1] { value }, out stateUpdateRate, null, null, null, null, null);
            }
        }

        /// <summary>
        /// Time bias
        /// </summary>
        /// <returns>Int</returns>
        public int TimeBias
        {
            get { return stateTimeBias; }
            set
            {
                opcGroupStateMgt.SetState(null, out stateUpdateRate, null, new int[1] { value }, null, null, null);
                stateTimeBias = value;
            }
        }

        /// <summary>
        /// Percent deadband
        /// </summary>
        /// <returns>Float</returns>
        public float PercentDeadband
        {
            get { return statePercentDeadband; }
            set
            {
                opcGroupStateMgt.SetState(null, out stateUpdateRate, null, null, new float[1] { value }, null, null);
                statePercentDeadband = value;
            }
        }

        /// <summary>
        /// Locale ID
        /// </summary>
        /// <returns>Int</returns>
        public int LocaleID
        {
            get { return stateLocaleID; }
            set
            {
                opcGroupStateMgt.SetState(null, out stateUpdateRate, null, null, null, new int[1] { value }, null);
                stateLocaleID = value;
            }
        }

        /// <summary>
        /// Client Handle
        /// </summary>
        /// <returns>Int</returns>
        public int HandleClient
        {
            get { return stateHandleClient; }
            set
            {
                opcGroupStateMgt.SetState(null, out stateUpdateRate, null, null, null, null, new int[1] { value });
                stateHandleClient = value;
            }
        }

        /// <summary>
        /// Server Handle
        /// </summary>
        /// <returns>Int</returns>
        public int HandleServer
        {
            get { return stateHandleServer; }
        }

        /// <summary>
        /// Create OPC Group
        /// </summary>
        /// <param name="opcServer">If server link</param>
        /// <param name="isPublic">Is public</param>
        /// <param name="groupName">Group name</param>
        /// <param name="setActive">Set active</param>
        /// <param name="requestedUpdateRate">Requested update rate</param>
        /// <param name="biasTime">Bias time</param>
        /// <param name="percentDeadband">Percent deadband</param>
        /// <param name="localeID">Locale ID</param>
        internal OpcGroup(IOPCServer opcServer, bool isPublic, string groupName, bool setActive, int requestedUpdateRate,
            int? biasTime, float? percentDeadband, int localeID)
        {
            IsDisposed = false;

            parentServer = opcServer;

            stateName = groupName;
            statePublic = isPublic;
            stateUpdateRate = requestedUpdateRate;
            stateActive = setActive;
            stateTimeBias = 0;
            statePercentDeadband = 0.0f;
            stateLocaleID = 0;
            stateHandleClient = this.GetHashCode();
            stateHandleServer = 0;

            Type typGrpMgt = typeof(IOPCGroupStateMgt);
            Guid guidGrpTst = typGrpMgt.GUID;

            if (statePublic)
            {
                IOPCServerPublicGroups ifPubGrps = null;
                ifPubGrps = (IOPCServerPublicGroups)parentServer;
                if (ifPubGrps == null)
                    Marshal.ThrowExceptionForHR(HRESULTS.E_NOINTERFACE);

                ifPubGrps.GetPublicGroupByName(stateName, ref guidGrpTst, out opcGroupObject);
                ifPubGrps = null;
            }
            else
            {
                int[] biasTimeArray = biasTime == null ? null : new int[1] { biasTime.Value };
                float[] percentDeadbandArray = percentDeadband == null ? null : new float[1] { percentDeadband.Value };
                parentServer.AddGroup(stateName, stateActive, stateUpdateRate, stateHandleClient, biasTimeArray,
                    percentDeadbandArray, stateLocaleID, out stateHandleServer, out stateUpdateRate, ref guidGrpTst, out opcGroupObject);
            }
            if (opcGroupObject == null)
                Marshal.ThrowExceptionForHR(HRESULTS.E_NOINTERFACE);

            RefreshState();

            // Advise callback
            Guid sinkGuid = typeof(IOPCDataCallback).GUID;
            connectionPointContainer.FindConnectionPoint(ref sinkGuid, out callbackConnectionPoint);
            if (callbackConnectionPoint != null)
                callbackConnectionPoint.Advise(this, out callbackCookie);

        }

        /// <summary>
        /// OPC Group Destructor
        /// </summary>
        ~OpcGroup()
        {
            try
            {
                InternalRemove(false);
            }
            catch { /* NOP */ }
        }

        /// <summary>
        /// Remove OPC Group from OPC Server
        /// </summary>
        /// <param name="force">Force remove group</param>
        public void Remove(bool force)
        {
            ThrowIfDisposed();
            InternalRemove(force);
        }

        private void InternalRemove(bool force)
        {
            if (!(callbackConnectionPoint == null))
            {
                if (callbackCookie != 0)
                {
                    callbackConnectionPoint.Unadvise(callbackCookie);
                    callbackCookie = 0;
                }
                int rc = Marshal.ReleaseComObject(callbackConnectionPoint);
                callbackConnectionPoint = null;
            }

            if (!(opcGroupObject == null))
            {
                int rc = Marshal.ReleaseComObject(opcGroupObject);
                opcGroupObject = null;
            }

            if (!(parentServer == null))
            {
                if (!statePublic)
                    parentServer.RemoveGroup(stateHandleServer, force);
                parentServer = null;
            }

            stateHandleServer = 0;
        }

        #region IOPCServerPublicGroups + IOPCPublicGroupStateMgt
        /// <summary>
        /// Delete public OPC group
        /// </summary>
        /// <param name="force">Force remove</param>
        public void DeletePublic(bool force)
        {
            ThrowIfDisposed();
            if (!statePublic)
                Marshal.ThrowExceptionForHR(HRESULTS.E_FAIL);

            IOPCServerPublicGroups ifPubGrps = parentServer as IOPCServerPublicGroups;
            if (ifPubGrps == null)
                Marshal.ThrowExceptionForHR(HRESULTS.E_NOINTERFACE);
            int serverhandle = stateHandleServer;
            InternalRemove(false);
            ifPubGrps.RemovePublicGroup(serverhandle, force);
            ifPubGrps = null;
        }

        /// <summary>
        /// Move private OPC group to public
        /// </summary>
        public void MoveToPublic()
        {
            ThrowIfDisposed();
            if (statePublic)
                Marshal.ThrowExceptionForHR(HRESULTS.E_FAIL);

            IOPCPublicGroupStateMgt ifPubMgt = opcGroupObject as IOPCPublicGroupStateMgt;
            if (ifPubMgt == null)
                Marshal.ThrowExceptionForHR(HRESULTS.E_NOINTERFACE);
            ifPubMgt.MoveToPublic();
            ifPubMgt.GetState(out statePublic);
            ifPubMgt = null;
        }
        #endregion

        #region IOPCGroupStateMgt
        /// <summary>
        /// Refresh OPC Group State
        /// </summary>
        public void RefreshState()
        {
            ThrowIfDisposed();
            opcGroupStateMgt.GetState(out stateUpdateRate, out stateActive, out stateName, out stateTimeBias, out statePercentDeadband,
                            out stateLocaleID, out stateHandleClient, out stateHandleServer);
        }
        #endregion

        #region IOPCItemMgt
        /// <summary>
        /// Add items to group
        /// </summary>
        /// <param name="opcItemDefinitions">OPC Item Definitions Array</param>
        /// <param name="opcItemResults">OPC Item Result Array</param>
        /// <returns>Bool</returns>
        public bool AddItems(OpcItemDefinition[] opcItemDefinitions, out OpcItemResult[] opcItemResults)
        {
            ThrowIfDisposed();
            opcItemResults = null;
            bool hasBlobs = false;

            int hResult;
            IntPtr ptrRes = IntPtr.Zero;
            IntPtr ptrErr = IntPtr.Zero;

            IntPtr ptrDef = Marshal.AllocCoTaskMem(opcItemDefinitions.Length * sizeOPCITEMDEF);

            try
            {
                InternalOPCITEMDEF itemDefintion = new InternalOPCITEMDEF() { wReserved = 0 };
                for (int i = 0; i < opcItemDefinitions.Length; i++)
                {
                    itemDefintion.szAccessPath = opcItemDefinitions[i].AccessPath;
                    itemDefintion.szItemID = opcItemDefinitions[i].ItemID;
                    itemDefintion.bActive = opcItemDefinitions[i].Active;
                    itemDefintion.hClient = opcItemDefinitions[i].HandleClient;
                    itemDefintion.vtRequestedDataType = (short)opcItemDefinitions[i].RequestedDataType;
                    itemDefintion.dwBlobSize = 0; itemDefintion.pBlob = IntPtr.Zero;
                    if (opcItemDefinitions[i].Blob != null)
                    {
                        itemDefintion.dwBlobSize = opcItemDefinitions[i].Blob.Length;
                        if (itemDefintion.dwBlobSize > 0)
                        {
                            hasBlobs = true;
                            itemDefintion.pBlob = Marshal.AllocCoTaskMem(itemDefintion.dwBlobSize);
                            Marshal.Copy(opcItemDefinitions[i].Blob, 0, itemDefintion.pBlob, itemDefintion.dwBlobSize);
                        }
                    }
                    Marshal.StructureToPtr(itemDefintion, IntPtr.Add(ptrDef, i * sizeOPCITEMDEF), false);
                }

                try
                {
                    hResult = opcItemMgt.AddItems(opcItemDefinitions.Length, ptrDef, out ptrRes, out ptrErr);
                }
                finally
                {
                    for (int i = 0; i < opcItemDefinitions.Length; i++)
                    {
                        if (hasBlobs)
                        {
                            IntPtr blob = (IntPtr)Marshal.ReadInt32(IntPtr.Add(ptrDef, (i * sizeOPCITEMDEF) + 20));
                            if (blob != IntPtr.Zero)
                                Marshal.FreeCoTaskMem(blob);
                        }
                        Marshal.DestroyStructure(IntPtr.Add(ptrDef, i * sizeOPCITEMDEF), typeOPCITEMDEF);
                    }
                }
            }
            finally
            {
                Marshal.FreeCoTaskMem(ptrDef);
            }

            if (HRESULTS.Failed(hResult))
                Marshal.ThrowExceptionForHR(hResult);
            if ((ptrRes == IntPtr.Zero) || (ptrErr == IntPtr.Zero))
                Marshal.ThrowExceptionForHR(HRESULTS.E_ABORT);

            try
            {
                opcItemResults = new OpcItemResult[opcItemDefinitions.Length];
                for (int i = 0; i < opcItemResults.Length; i++)
                {
                    opcItemResults[i] = new OpcItemResult
                    {
                        Error = Extensions.ReadInt32(ptrErr, i)
                    };
                    if (!HRESULTS.Failed(opcItemResults[i].Error))
                    {
                        opcItemResults[i].HandleServer = Marshal.ReadInt32(ptrRes, i * sizeOPCITEMRESULT);
                        opcItemResults[i].CanonicalDataType = (VarEnum)(int)Marshal.ReadInt16(ptrRes, i * sizeOPCITEMRESULT + 4);
                        opcItemResults[i].AccessRights = (OPCACCESSRIGHTS)Marshal.ReadInt32(ptrRes, i * sizeOPCITEMRESULT + 8);

                        int ptrblob = Marshal.ReadInt32(ptrRes, i * sizeOPCITEMRESULT + 16);
                        if ((ptrblob != 0))
                        {
                            int blobsize = Marshal.ReadInt32(ptrRes, i * sizeOPCITEMRESULT + 12);
                            if (blobsize > 0)
                            {
                                opcItemResults[i].Blob = new byte[blobsize];
                                Marshal.Copy((IntPtr)ptrblob, opcItemResults[i].Blob, 0, blobsize);
                            }
                            Marshal.FreeCoTaskMem((IntPtr)ptrblob);
                        }
                    }
                }
            }
            finally
            {
                if (ptrRes != IntPtr.Zero)
                    Marshal.FreeCoTaskMem(ptrRes);
                if (ptrErr != IntPtr.Zero)
                    Marshal.FreeCoTaskMem(ptrErr);
            }
            return hResult == HRESULTS.S_OK;
        }

        /// <summary>
        /// Validate OPC Items
        /// </summary>
        /// <param name="opcItemDefinitions">OPC Item Definitions Array</param>
        /// <param name="blobUpdate">Update OPC Item Blob</param>
        /// <param name="opcItemResults">OPC Item Result Array</param>
        /// <returns>Bool</returns>
        public bool ValidateItems(OpcItemDefinition[] opcItemDefinitions, bool blobUpdate, out OpcItemResult[] opcItemResults)
        {
            ThrowIfDisposed();
            opcItemResults = null;
            bool hasBlobs = false;
            int count = opcItemDefinitions.Length;

            int hResult;
            IntPtr ptrRes = IntPtr.Zero;
            IntPtr ptrErr = IntPtr.Zero;

            IntPtr ptrDef = Marshal.AllocCoTaskMem(count * sizeOPCITEMDEF);

            try
            {
                InternalOPCITEMDEF itemDefintion = new InternalOPCITEMDEF() { wReserved = 0 };
                for (int i = 0; i < opcItemDefinitions.Length; i++)
                {
                    itemDefintion.szAccessPath = opcItemDefinitions[i].AccessPath;
                    itemDefintion.szItemID = opcItemDefinitions[i].ItemID;
                    itemDefintion.bActive = opcItemDefinitions[i].Active;
                    itemDefintion.hClient = opcItemDefinitions[i].HandleClient;
                    itemDefintion.vtRequestedDataType = (short)opcItemDefinitions[i].RequestedDataType;
                    itemDefintion.dwBlobSize = 0;
                    itemDefintion.pBlob = IntPtr.Zero;
                    if (opcItemDefinitions[i].Blob != null)
                    {
                        itemDefintion.dwBlobSize = opcItemDefinitions[i].Blob.Length;
                        if (itemDefintion.dwBlobSize > 0)
                        {
                            hasBlobs = true;
                            itemDefintion.pBlob = Marshal.AllocCoTaskMem(itemDefintion.dwBlobSize);
                            Marshal.Copy(opcItemDefinitions[i].Blob, 0, itemDefintion.pBlob, itemDefintion.dwBlobSize);
                        }
                    }
                    Marshal.StructureToPtr(itemDefintion, IntPtr.Add(ptrDef, i * sizeOPCITEMDEF), false);
                }
                try
                {
                    hResult = opcItemMgt.ValidateItems(count, ptrDef, blobUpdate, out ptrRes, out ptrErr);
                }
                finally
                {
                    for (int i = 0; i < opcItemDefinitions.Length; i++)
                    {
                        if (hasBlobs)
                        {
                            IntPtr blob = (IntPtr)Marshal.ReadInt32(IntPtr.Add(ptrDef, (i * sizeOPCITEMDEF) + 20));
                            if (blob != IntPtr.Zero)
                                Marshal.FreeCoTaskMem(blob);
                        }
                        Marshal.DestroyStructure(IntPtr.Add(ptrDef, i * sizeOPCITEMDEF), typeOPCITEMDEF);
                    }
                }
            }
            finally
            {
                Marshal.FreeCoTaskMem(ptrDef);
            }

            if (HRESULTS.Failed(hResult))
                Marshal.ThrowExceptionForHR(hResult);
            if ((ptrRes == IntPtr.Zero) || (ptrErr == IntPtr.Zero))
                Marshal.ThrowExceptionForHR(HRESULTS.E_ABORT);

            try
            {
                opcItemResults = new OpcItemResult[opcItemDefinitions.Length];
                for (int i = 0; i < opcItemResults.Length; i++)
                {
                    opcItemResults[i] = new OpcItemResult
                    {
                        Error = Extensions.ReadInt32(ptrErr, i)
                    };
                    if (!HRESULTS.Failed(opcItemResults[i].Error))
                    {
                        opcItemResults[i].HandleServer = Marshal.ReadInt32(ptrRes, i * sizeOPCITEMRESULT);
                        opcItemResults[i].CanonicalDataType = (VarEnum)(int)Marshal.ReadInt16(ptrRes, i * sizeOPCITEMRESULT + 4);
                        opcItemResults[i].AccessRights = (OPCACCESSRIGHTS)Marshal.ReadInt32(ptrRes, i * sizeOPCITEMRESULT + 8);

                        int ptrblob = Marshal.ReadInt32(ptrRes, i * sizeOPCITEMRESULT + 16);
                        if ((ptrblob != 0))
                        {
                            int blobsize = Marshal.ReadInt32(ptrRes, i * sizeOPCITEMRESULT + 12);
                            if (blobsize > 0)
                            {
                                opcItemResults[i].Blob = new byte[blobsize];
                                Marshal.Copy((IntPtr)ptrblob, opcItemResults[i].Blob, 0, blobsize);
                            }
                            Marshal.FreeCoTaskMem((IntPtr)ptrblob);
                        }
                    }
                }
            }
            finally
            {
                if (ptrRes != IntPtr.Zero)
                    Marshal.FreeCoTaskMem(ptrRes);
                if (ptrErr != IntPtr.Zero)
                    Marshal.FreeCoTaskMem(ptrErr);
            }
            return hResult == HRESULTS.S_OK;
        }

        /// <summary>
        /// Remove OPC items from group
        /// </summary>
        /// <param name="serverHandles">Server handles of OPC items to remove</param>
        /// <param name="errors">Errors removing items</param>
        /// <returns>Bool</returns>
        public bool RemoveItems(int[] serverHandles, out int[] errors)
        {
            ThrowIfDisposed();
            errors = null;

            int hResult = opcItemMgt.RemoveItems(serverHandles.Length, serverHandles, out IntPtr ptrErr);
            if (HRESULTS.Failed(hResult))
                Marshal.ThrowExceptionForHR(hResult);

            errors = new int[serverHandles.Length];
            Marshal.Copy(ptrErr, errors, 0, errors.Length);
            Marshal.FreeCoTaskMem(ptrErr);
            return hResult == HRESULTS.S_OK;
        }

        /// <summary>
        /// Set active state
        /// </summary>
        /// <param name="serverHandles">Server handles of OPC items to set active</param>
        /// <param name="activate">Activate</param>
        /// <param name="errors">Errors set active items</param>
        /// <returns>Bool</returns>
        public bool SetActiveState(int[] serverHandles, bool activate, out int[] errors)
        {
            ThrowIfDisposed();
            errors = null;

            int hResult = opcItemMgt.SetActiveState(serverHandles.Length, serverHandles, activate, out IntPtr ptrErr);
            if (HRESULTS.Failed(hResult))
                Marshal.ThrowExceptionForHR(hResult);

            errors = new int[serverHandles.Length];
            Marshal.Copy(ptrErr, errors, 0, errors.Length);
            Marshal.FreeCoTaskMem(ptrErr);
            return hResult == HRESULTS.S_OK;
        }

        /// <summary>
        /// Set client handles
        /// </summary>
        /// <param name="serverHandles">Server handles</param>
        /// <param name="clientHandles">New client handles</param>
        /// <param name="errors">Errors</param>
        /// <returns>Bool</returns>
        public bool SetClientHandles(int[] serverHandles, int[] clientHandles, out int[] errors)
        {
            ThrowIfDisposed();
            errors = null;
            if (serverHandles.Length != clientHandles.Length)
                Marshal.ThrowExceptionForHR(HRESULTS.E_ABORT);

            int hResult = opcItemMgt.SetClientHandles(serverHandles.Length, serverHandles, clientHandles, out IntPtr ptrErr);
            if (HRESULTS.Failed(hResult))
                Marshal.ThrowExceptionForHR(hResult);

            errors = new int[serverHandles.Length];
            Marshal.Copy(ptrErr, errors, 0, errors.Length);
            Marshal.FreeCoTaskMem(ptrErr);
            return hResult == HRESULTS.S_OK;
        }

        /// <summary>
        /// Set datatypes
        /// </summary>
        /// <param name="serverHandles">Arr h srv</param>
        /// <param name="dataTypes">Arr VT</param>
        /// <param name="errors">Arr err</param>
        /// <returns>Bool</returns>
        public bool SetDatatypes(int[] serverHandles, VarEnum[] dataTypes, out int[] errors)
        {
            ThrowIfDisposed();
            errors = null;
            if (serverHandles.Length != dataTypes.Length)
                Marshal.ThrowExceptionForHR(HRESULTS.E_ABORT);

            int hResult;
            IntPtr ptrErr = IntPtr.Zero;

            IntPtr ptrVT = Marshal.AllocCoTaskMem(serverHandles.Length * sizeof(short));
            try
            {
                for (int i = 0; i < dataTypes.Length; i++)
                {
                    Extensions.WriteInt16(ptrVT, i, (short)dataTypes[i]);
                }

                hResult = opcItemMgt.SetDataTypes(serverHandles.Length, serverHandles, ptrVT, out ptrErr);
            }
            finally
            {
                Marshal.FreeCoTaskMem(ptrVT);
            }

            if (HRESULTS.Failed(hResult))
                Marshal.ThrowExceptionForHR(hResult);

            errors = new int[serverHandles.Length];
            Marshal.Copy(ptrErr, errors, 0, errors.Length);
            Marshal.FreeCoTaskMem(ptrErr);
            return hResult == HRESULTS.S_OK;
        }

        /// <summary>
        /// Create OPC item attributes enumerator
        /// </summary>
        /// <returns>OPC item attributes enumerator</returns>
        public IEnumerator<OpcItemAttributes> GetEnumerator()
        {
            ThrowIfDisposed();
            Guid guidEnuAtt = typeof(IEnumOPCItemAttributes).GUID;

            int hResult = opcItemMgt.CreateEnumerator(ref guidEnuAtt, out object objtemp);
            if (HRESULTS.Failed(hResult))
                Marshal.ThrowExceptionForHR(hResult);
            if ((hResult == HRESULTS.S_FALSE) || (objtemp == null))
                return null;

            return new OpcEnumItemAttributes(objtemp as IEnumOPCItemAttributes);
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
        #endregion

        #region IOPCSyncIO
        /// <summary>
        /// Read values
        /// </summary>
        /// <param name="dataSource">Read data source</param>
        /// <param name="serverHandles">Server handles of OPC items to read</param>
        /// <param name="itemStates">OPC Item states array</param>
        /// <returns>Bool</returns>
        public bool Read(OPCDATASOURCE dataSource, int[] serverHandles, out OpcItemState[] itemStates)
        {
            ThrowIfDisposed();
            itemStates = null;

            int hResult = opcSyncIO.Read(dataSource, serverHandles.Length, serverHandles, out IntPtr ptrStat, out IntPtr ptrErr);

            if (HRESULTS.Failed(hResult))
                Marshal.ThrowExceptionForHR(hResult);

            if ((ptrErr == IntPtr.Zero) || (ptrStat == IntPtr.Zero))
                Marshal.ThrowExceptionForHR(HRESULTS.E_ABORT);
            try
            {
                itemStates = new OpcItemState[serverHandles.Length];
                for (int i = 0; i < serverHandles.Length; i++)
                {
                    itemStates[i] = new OpcItemState
                    {
                        Error = Extensions.ReadInt32(ptrErr, i),
                        HandleClient = Marshal.ReadInt32(ptrStat, i * sizeOPCREADRESULT)
                    };

                    if (HRESULTS.Succeeded(itemStates[i].Error))
                    {
                        short vt = Marshal.ReadInt16(ptrStat, i * sizeOPCREADRESULT + 16);
                        if (vt == (short)VarEnum.VT_ERROR)
                            itemStates[i].Error = Marshal.ReadInt32(ptrStat, i * sizeOPCREADRESULT + 24);

                        itemStates[i].TimeStamp = Marshal.ReadInt64(ptrStat, i * sizeOPCREADRESULT + 4);
                        itemStates[i].Quality = Marshal.ReadInt16(ptrStat, i * sizeOPCREADRESULT + 12);
                        itemStates[i].DataValue = Marshal.GetObjectForNativeVariant(IntPtr.Add(ptrStat, i * sizeOPCREADRESULT + 16));
                        Extensions.VariantClear(IntPtr.Add(ptrStat, i * sizeOPCREADRESULT + 16));
                    }
                    else
                        itemStates[i].DataValue = null;
                }
            }
            finally
            {
                Marshal.FreeCoTaskMem(ptrStat);
                Marshal.FreeCoTaskMem(ptrErr);
            }
            return hResult == HRESULTS.S_OK;
        }

        /// <summary>
        /// Write values
        /// </summary>
        /// <param name="serverHandles">Server handles of OPC items to write</param>
        /// <param name="values">Values to write</param>
        /// <param name="errors">Errors</param>
        /// <returns>Bool</returns>
        public bool Write(int[] serverHandles, object[] values, out int[] errors)
        {
            ThrowIfDisposed();
            errors = null;
            if (serverHandles.Length != values.Length)
                Marshal.ThrowExceptionForHR(HRESULTS.E_ABORT);

            int hResult = opcSyncIO.Write(serverHandles.Length, serverHandles, values, out IntPtr ptrErr);
            if (HRESULTS.Failed(hResult))
                Marshal.ThrowExceptionForHR(hResult);

            errors = new int[serverHandles.Length];
            Marshal.Copy(ptrErr, errors, 0, errors.Length);
            Marshal.FreeCoTaskMem(ptrErr);
            return hResult == HRESULTS.S_OK;
        }
        #endregion

        #region IOPCAsyncIO2
        /// <summary>
        /// Async read values
        /// </summary>
        /// <param name="serverHandles">Server handles of OPC items to read</param>
        /// <param name="transactionID">The Client generated transaction ID. 
        /// This is included in the ‘completion’ information provided to the OnWriteComplete.</param>
        /// <param name="cancelID">Place to return a Server generated ID to be used in case
        /// the operation needs to be canceled.</param>
        /// <param name="errors">Errors</param>
        /// <returns>Bool</returns>
        public bool Read(int[] serverHandles, int transactionID, out int cancelID, out int[] errors)
        {
            ThrowIfDisposed();
            errors = null;
            cancelID = 0;

            int hResult = opcAsyncIO.Read(serverHandles.Length, serverHandles, transactionID, out cancelID, out IntPtr ptrErr);
            if (HRESULTS.Failed(hResult))
                Marshal.ThrowExceptionForHR(hResult);

            errors = new int[serverHandles.Length];
            Marshal.Copy(ptrErr, errors, 0, errors.Length);
            Marshal.FreeCoTaskMem(ptrErr);
            return hResult == HRESULTS.S_OK;
        }

        /// <summary>
        /// Async write
        /// </summary>
        /// <param name="serverHandles">List of server items handles for the items to be written</param>
        /// <param name="values">List of values to be written. The value data types 
        /// are not required to match the requested or canonical item datatype but 
        /// must be ‘convertible’ to the canonical type.</param>
        /// <param name="transactionID">The Client generated transaction ID. 
        /// This is included in the ‘completion’ information provided to the OnWriteComplete.</param>
        /// <param name="cancelID">Place to return a Server generated ID to be used in case
        /// the operation needs to be canceled.</param>
        /// <param name="errors">Array of errors for each item - returned by the server.</param>
        /// <returns>Bool</returns>
        public bool Write(int[] serverHandles, object[] values, int transactionID, out int cancelID, out int[] errors)
        {
            ThrowIfDisposed();
            errors = null;
            cancelID = 0;

            if (serverHandles.Length != values.Length)
                Marshal.ThrowExceptionForHR(HRESULTS.E_ABORT);

            int hResult = opcAsyncIO.Write(serverHandles.Length, serverHandles, values, transactionID, out cancelID, out IntPtr ptrErr);
            if (HRESULTS.Failed(hResult))
                Marshal.ThrowExceptionForHR(hResult);

            if (ptrErr != null && ptrErr != IntPtr.Zero)
            {
                errors = new int[serverHandles.Length];
                Marshal.Copy(ptrErr, errors, 0, errors.Length);
                Marshal.FreeCoTaskMem(ptrErr);
            }
            return hResult == HRESULTS.S_OK;
        }

        /// <summary>
        /// Refresh 2
        /// </summary>
        /// <param name="dataSource">Source mode</param>
        /// <param name="transactionID">Transaction ID</param>
        /// <param name="cancelID">Cancel ID</param>
        public void Refresh2(OPCDATASOURCE dataSource, int transactionID, out int cancelID)
        {
            ThrowIfDisposed();
            opcAsyncIO.Refresh2(dataSource, transactionID, out cancelID);
        }

        /// <summary>
        /// Cancel 2
        /// </summary>
        /// <param name="cancelID">Cancel ID</param>
        public void Cancel2(int cancelID)
        {
            ThrowIfDisposed();
            opcAsyncIO.Cancel2(cancelID);
        }

        /// <summary>
        /// Set enable
        /// </summary>
        /// <param name="doEnable">Do enable</param>
        public void SetEnable(bool doEnable)
        {
            ThrowIfDisposed();
            opcAsyncIO.SetEnable(doEnable);
        }

        /// <summary>
        /// Get enable
        /// </summary>
        /// <param name="isEnabled">Is enabled</param>
        public void GetEnable(out bool isEnabled)
        {
            ThrowIfDisposed();
            opcAsyncIO.GetEnable(out isEnabled);
        }
        #endregion

        #region IOPCDataCallback
        void IOPCDataCallback.OnDataChange(
                int dwTransid, int hGroup, int hrMasterquality, int hrMastererror, int dwCount,
                IntPtr phClientItems, IntPtr pvValues, IntPtr pwQualities, IntPtr pftTimeStamps, IntPtr ppErrors)
        {
            Trace.WriteLine($"{nameof(OpcGroup)}.{nameof(IOPCDataCallback.OnDataChange)}, transID:{dwTransid}, count:{dwCount}, thread ID:{System.Threading.Thread.CurrentThread.ManagedThreadId}");

            if ((dwCount == 0) || (hGroup != stateHandleClient))
                return;

            DataChangeEventArgs eventArgs = new DataChangeEventArgs
            {
                TransactionID = dwTransid,
                GroupHandleClient = hGroup,
                MasterQuality = hrMasterquality,
                MasterError = hrMastererror,
                ItemStates = new OpcItemState[dwCount]
            };

            for (int i = 0; i < dwCount; i++)
            {
                eventArgs.ItemStates[i] = new OpcItemState
                {
                    Error = Extensions.ReadInt32(ppErrors, i),
                    HandleClient = Extensions.ReadInt32(phClientItems, i)
                };

                if (HRESULTS.Succeeded(eventArgs.ItemStates[i].Error))
                {
                    // Test if native variant is VT_ERROR
                    short vt = Marshal.ReadInt16(pvValues, i * Extensions.NativeVariantSize);
                    if (vt == (short)VarEnum.VT_ERROR)
                        eventArgs.ItemStates[i].Error = Marshal.ReadInt32(pvValues, (i * Extensions.NativeVariantSize) + 8);

                    eventArgs.ItemStates[i].DataValue = Extensions.GetObjectForNativeVariant(pvValues, i);
                    eventArgs.ItemStates[i].Quality = Extensions.ReadInt16(pwQualities, i);
                    eventArgs.ItemStates[i].TimeStamp = Extensions.ReadInt64(pftTimeStamps, i);
                }
            }

            dataChanged?.Invoke(this, eventArgs);
        }

        void IOPCDataCallback.OnReadComplete(
                int dwTransid, int hGroup, int hrMasterquality, int hrMastererror, int dwCount,
                IntPtr phClientItems, IntPtr pvValues, IntPtr pwQualities, IntPtr pftTimeStamps, IntPtr ppErrors)
        {
            Trace.WriteLine($"{nameof(OpcGroup)}.{nameof(IOPCDataCallback.OnReadComplete)}, transID:{dwTransid}, count:{dwCount}, thread ID:{System.Threading.Thread.CurrentThread.ManagedThreadId}");

            if ((dwCount == 0) || (hGroup != stateHandleClient))
                return;

            ReadCompleteEventArgs eventArgs = new ReadCompleteEventArgs
            {
                TransactionID = dwTransid,
                GroupHandleClient = hGroup,
                MasterQuality = hrMasterquality,
                MasterError = hrMastererror,
                ItemStates = new OpcItemState[dwCount]
            };

            for (int i = 0; i < dwCount; i++)
            {
                eventArgs.ItemStates[i] = new OpcItemState
                {
                    Error = Extensions.ReadInt32(ppErrors, i),
                    HandleClient = Extensions.ReadInt32(phClientItems, i)
                };
                if (HRESULTS.Succeeded(eventArgs.ItemStates[i].Error))
                {
                    // Test if native variant is VT_ERROR
                    short vt = Marshal.ReadInt16(pvValues, i * Extensions.NativeVariantSize);
                    if (vt == (short)VarEnum.VT_ERROR)
                        eventArgs.ItemStates[i].Error = Marshal.ReadInt32(pvValues, (i * Extensions.NativeVariantSize) + 8);

                    eventArgs.ItemStates[i].DataValue = Extensions.GetObjectForNativeVariant(pvValues, i);
                    eventArgs.ItemStates[i].Quality = Extensions.ReadInt16(pwQualities, i);
                    eventArgs.ItemStates[i].TimeStamp = Extensions.ReadInt64(pftTimeStamps, i);
                }
            }

            readCompleted?.Invoke(this, eventArgs);
        }

        void IOPCDataCallback.OnWriteComplete(
                int dwTransid, int hGroup, int hrMastererr, int dwCount,
                IntPtr pClienthandles, IntPtr ppErrors)
        {
            Trace.WriteLine($"{nameof(OpcGroup)}.{nameof(IOPCDataCallback.OnWriteComplete)}, transID:{dwTransid}, count:{dwCount}, thread ID:{System.Threading.Thread.CurrentThread.ManagedThreadId}");

            if ((dwCount == 0) || (hGroup != stateHandleClient))
                return;

            WriteCompleteEventArgs eventArgs = new WriteCompleteEventArgs()
            {
                TransactionID = dwTransid,
                GroupHandleClient = hGroup,
                MasterError = hrMastererr,
                WriteResults = new OpcWriteResult[dwCount]
            };

            for (int i = 0; i < dwCount; i++)
            {
                eventArgs.WriteResults[i] = new OpcWriteResult
                {
                    Error = Extensions.ReadInt32(ppErrors, i),
                    HandleClient = Extensions.ReadInt32(pClienthandles, i)
                };
            }

            writeCompleted?.Invoke(this, eventArgs);
        }

        void IOPCDataCallback.OnCancelComplete(int dwTransid, int hGroup)
        {
            Trace.WriteLine($"{nameof(OpcGroup)}.{nameof(IOPCDataCallback.OnCancelComplete)}, transID:{dwTransid}, thread ID:{System.Threading.Thread.CurrentThread.ManagedThreadId}");

            if (hGroup != stateHandleClient)
                return;

            CancelCompleteEventArgs eventArgs = new CancelCompleteEventArgs(dwTransid, hGroup);
            cancelCompleted?.Invoke(this, eventArgs);
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
            InternalRemove(false);
            IsDisposed = true;
            GC.SuppressFinalize(this);
        }
        #endregion

    }
}
