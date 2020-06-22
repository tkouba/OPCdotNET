using System;
using System.Text;
using System.Runtime.InteropServices;
using System.Reflection;


namespace OPC.Common.Interface
{
    /// <summary>
    /// Enum GUID Interface
    /// </summary>
    [ComImport]
    [Guid("0002E000-0000-0000-C000-000000000046")]
    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    internal interface IEnumGUID
    {
        void Next(
            [In]											int celt,
            [In]											IntPtr rgelt,				// ptr to Out-Values!!
            [Out]										out int pceltFetched);

        void Skip(
            [In]											int celt);

        void Reset();

        void Clone(
            [Out, MarshalAs(UnmanagedType.IUnknown)]	out	object ppUnk);

    }	
} 
