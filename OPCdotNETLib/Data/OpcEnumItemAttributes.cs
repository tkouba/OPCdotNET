using System;
using System.Text;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;
using System.Reflection;
using System.Diagnostics;
using OPC.Common;
using OPC.Data.Interface;
using System.Collections.Generic;

namespace OPC.Data
{
    /// <summary>
    /// OPC enum item attributes
    /// </summary>
    public class OpcEnumItemAttributes : IEnumerator<OpcItemAttributes>
    {
        private IEnumOPCItemAttributes enumItemAttributes;

        /// <summary>
        ///  Gets the element in the OpcItemAttributes collection at the current position of the enumerator.
        /// </summary>
        public OpcItemAttributes Current { get; private set; }

        object System.Collections.IEnumerator.Current { get { return Current; } }

        /// <summary>
        /// Create OPC enum item attributes enumerator
        /// </summary>
        /// <param name="enumItemAttributes">If enump</param>
        internal OpcEnumItemAttributes(IEnumOPCItemAttributes enumItemAttributes)
        {
            this.enumItemAttributes = enumItemAttributes;
            Current = null;
        }

        /// <summary>
        /// ~OPC enum item attributes desctructor
        /// </summary>
        ~OpcEnumItemAttributes()
        {
            try
            {
                ReleaseComObject();
            }
            catch { /* NOP */ }
        }

        /// <summary>
        /// Dispose
        /// </summary>
        private void ReleaseComObject()
        {
            if (enumItemAttributes != null)
                Marshal.ReleaseComObject(enumItemAttributes);
            enumItemAttributes = null;
        }

        /// <summary>
        /// Advances the enumerator to the next element of the collection.
        /// </summary>
        /// <returns>
        /// true if the enumerator was successfully advanced to the next element; 
        /// false if the enumerator has passed the end of the collection.
        /// </returns>
        public bool MoveNext()
        {
            IntPtr ptrAtt = IntPtr.Zero;
            
            enumItemAttributes.Next(1, out ptrAtt, out int count);

            if ((ptrAtt == IntPtr.Zero) || (count != 1))
                return false;
            try
            {
                IntPtr ptrString;
                Current = new OpcItemAttributes();

                ptrString = (IntPtr)Marshal.ReadInt32(ptrAtt);
                Current.AccessPath = Marshal.PtrToStringUni(ptrString);
                Marshal.FreeCoTaskMem(ptrString);

                ptrString = (IntPtr)Marshal.ReadInt32(ptrAtt, 4);
                Current.ItemID = Marshal.PtrToStringUni(ptrString);
                Marshal.FreeCoTaskMem(ptrString);

                Current.Active = (Marshal.ReadInt32(ptrAtt, 8)) != 0;
                Current.HandleClient = Marshal.ReadInt32(ptrAtt, 12);
                Current.HandleServer = Marshal.ReadInt32(ptrAtt, 16);
                Current.AccessRights = (OPCACCESSRIGHTS)Marshal.ReadInt32(ptrAtt, 20);
                Current.RequestedDataType = (VarEnum)Marshal.ReadInt16(ptrAtt, 32);
                Current.CanonicalDataType = (VarEnum)Marshal.ReadInt16(ptrAtt, 34);

                Current.EUType = (OPCEUTYPE)Marshal.ReadInt32(ptrAtt, 36);
                Current.EUInfo = Marshal.GetObjectForNativeVariant(IntPtr.Add(ptrAtt, 40));
                Extensions.VariantClear(IntPtr.Add(ptrAtt, 40));

                IntPtr ptrBlob = (IntPtr)Marshal.ReadInt32(ptrAtt, 28);
                if (ptrBlob != IntPtr.Zero)
                {
                    int blobSize = Marshal.ReadInt32(ptrAtt, 24);
                    if (blobSize > 0)
                    {
                        Current.Blob = new byte[blobSize];
                        Marshal.Copy(ptrBlob, Current.Blob, 0, blobSize);
                    }
                    Marshal.FreeCoTaskMem(ptrBlob);
                }
            }
            finally
            {
                if (ptrAtt != IntPtr.Zero)
                    Marshal.FreeCoTaskMem(ptrAtt);
            }
            return true;
        }

        /// <summary>
        /// Skip <paramref name="celt"/> elements and sets current element to next element of the collection.
        /// </summary>
        /// <param name="celt">Count of elements to skip.</param>
        /// <returns>
        /// true if the enumerator was successfully skip element; 
        /// false if the enumerator has passed the end of the collection.
        /// </returns>
        public bool Skip(int celt)
        {
            enumItemAttributes.Skip(celt);
            return MoveNext();
        }

        /// <summary>
        ///  Sets the enumerator to its initial position, which is before the first element in the collection.
        /// </summary>
        public void Reset()
        {
            enumItemAttributes.Reset();
            Current = null;
        }

        void IDisposable.Dispose()
        {
            ReleaseComObject();
        }
    }
}
