using System;
using System.Runtime.InteropServices;

namespace OPC.Data.Interface
{
    /// <summary>
    /// OPC Enum Item Attributes
    /// </summary>
    [ComImport]
    [Guid("39c13a55-011e-11d0-9675-0020afd8adb3")]
    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    internal interface IEnumOPCItemAttributes
    {
        void Next([In] int celt, [Out] out IntPtr ppItemArray, [Out] out int pceltFetched);

        void Skip([In] int celt);

        void Reset();

        void Clone([Out, MarshalAs(UnmanagedType.IUnknown)] out object ppUnk);

    }
}
