using System;
using System.Runtime.InteropServices;
using OPC.Data;

namespace OPC.Common
{
    /// <summary>
    /// Variant extensions
    /// </summary>
    public static class Extensions
    {

        /// <summary>
        /// VT_TYPEMASK
        /// </summary>
        public const short VT_TYPEMASK = 0x0fff;
        /// <summary>
        /// VT_VECTOR
        /// </summary>
        public const short VT_VECTOR = 0x1000;
        /// <summary>
        /// VT_ARRAY
        /// </summary>
        public const short VT_ARRAY = 0x2000;
        /// <summary>
        /// VT_BYREF
        /// </summary>
        public const short VT_BYREF = 0x4000;
        /// <summary>
        /// VT_ILLEGAL
        /// </summary>
        public const short VT_ILLEGAL = unchecked((short)0xffff);

        /// <summary>
        /// Native variant size (16 bytes)
        /// </summary>
        public const int NativeVariantSize = 16;

        #region oleaut32.dll
        /// <summary>
        /// Clears a variant and free or release memory.
        /// </summary>
        /// <param name="intPtr">Address of a COM VARIANT to clear.</param>
        /// <returns>S_OK on Success.</returns>
        [DllImport("oleaut32.dll")]
        public static extern int VariantClear(IntPtr intPtr);
        #endregion

        /// <summary>
        /// Clears a variant and free or release memory.
        /// </summary>
        /// <param name="intPtr">A pointer to a COM VARIANT array</param>
        /// <param name="index">The index of COM VARIANT in array</param>
        /// <returns>S_OK on Success.</returns>
        public static int VariantClear(IntPtr intPtr, int index)
        {
            return VariantClear(IntPtr.Add(intPtr, index * Extensions.NativeVariantSize));
        }

        /// <summary>
        /// Variable enum to string
        /// </summary>
        /// <param name="varEnum">Vevt</param>
        /// <returns>String</returns>
        public static string VarEnumToString(VarEnum varEnum)
        {
            string strvt = "";
            short vtshort = (short)varEnum;
            if (vtshort == VT_ILLEGAL)
                return "VT_ILLEGAL";

            if ((vtshort & VT_ARRAY) != 0)
                strvt += "VT_ARRAY | ";

            if ((vtshort & VT_BYREF) != 0)
                strvt += "VT_BYREF | ";

            if ((vtshort & VT_VECTOR) != 0)
                strvt += "VT_VECTOR | ";

            VarEnum vtbase = (VarEnum)(vtshort & VT_TYPEMASK);
            strvt += vtbase.ToString();
            return strvt;
        }

        /// <summary>
        /// VarEnum to .NET type
        /// </summary>
        /// <param name="varEnum">VarEnum</param>
        /// <returns>.NET type for VarEnum</returns>
        public static Type ToType(this VarEnum varEnum)
        {
            switch (varEnum)
            {
                case VarEnum.VT_EMPTY:
                    return null;
                case VarEnum.VT_I1:
                    return typeof(sbyte);
                case VarEnum.VT_UI1:
                    return typeof(byte);
                case VarEnum.VT_I2:
                    return typeof(short);
                case VarEnum.VT_UI2:
                    return typeof(ushort);
                case VarEnum.VT_I4:
                    return typeof(int);
                case VarEnum.VT_UI4:
                    return typeof(uint);
                case VarEnum.VT_I8:
                    return typeof(long);
                case VarEnum.VT_UI8:
                    return typeof(ulong);
                case VarEnum.VT_R4:
                    return typeof(float);
                case VarEnum.VT_R8:
                    return typeof(double);
                case VarEnum.VT_CY:
                    return typeof(decimal);
                case VarEnum.VT_BOOL:
                    return typeof(bool);
                case VarEnum.VT_DATE:
                    return typeof(DateTime);
                case VarEnum.VT_BSTR:
                    return typeof(string);
                case VarEnum.VT_VARIANT:
                    return typeof(object);
                case VarEnum.VT_ARRAY | VarEnum.VT_I1:
                    return typeof(sbyte[]);
                case VarEnum.VT_ARRAY | VarEnum.VT_UI1:
                    return typeof(byte[]);
                case VarEnum.VT_ARRAY | VarEnum.VT_I2:
                    return typeof(short[]);
                case VarEnum.VT_ARRAY | VarEnum.VT_UI2:
                    return typeof(ushort[]);
                case VarEnum.VT_ARRAY | VarEnum.VT_I4:
                    return typeof(int[]);
                case VarEnum.VT_ARRAY | VarEnum.VT_UI4:
                    return typeof(uint[]);
                case VarEnum.VT_ARRAY | VarEnum.VT_I8:
                    return typeof(long[]);
                case VarEnum.VT_ARRAY | VarEnum.VT_UI8:
                    return typeof(ulong[]);
                case VarEnum.VT_ARRAY | VarEnum.VT_R4:
                    return typeof(float[]);
                case VarEnum.VT_ARRAY | VarEnum.VT_R8:
                    return typeof(double[]);
                case VarEnum.VT_ARRAY | VarEnum.VT_CY:
                    return typeof(decimal[]);
                case VarEnum.VT_ARRAY | VarEnum.VT_BOOL:
                    return typeof(bool[]);
                case VarEnum.VT_ARRAY | VarEnum.VT_DATE:
                    return typeof(DateTime[]);
                case VarEnum.VT_ARRAY | VarEnum.VT_BSTR:
                    return typeof(string[]);
                case VarEnum.VT_ARRAY | VarEnum.VT_VARIANT:
                    return typeof(object[]);
                default:
                    return null;
            }
        }

        /// <summary>
        /// .NET type to VarEnum
        /// </summary>
        /// <param name="type">The .NET type</param>
        /// <returns>VarEnum</returns>
        public static VarEnum ToVarEnum(this Type type)
        {
            if (type == null)
                return VarEnum.VT_EMPTY;

            if (type == typeof(sbyte))
                return VarEnum.VT_I1;

            if (type == typeof(byte))
                return VarEnum.VT_UI1;

            if (type == typeof(short))
                return VarEnum.VT_I2;

            if (type == typeof(ushort))
                return VarEnum.VT_UI2;

            if (type == typeof(int))
                return VarEnum.VT_I4;

            if (type == typeof(uint))
                return VarEnum.VT_UI4;

            if (type == typeof(long))
                return VarEnum.VT_I8;

            if (type == typeof(ulong))
                return VarEnum.VT_UI8;

            if (type == typeof(float))
                return VarEnum.VT_R4;

            if (type == typeof(double))
                return VarEnum.VT_R8;

            if (type == typeof(decimal))
                return VarEnum.VT_CY;

            if (type == typeof(bool))
                return VarEnum.VT_BOOL;

            if (type == typeof(DateTime))
                return VarEnum.VT_DATE;

            if (type == typeof(string))
                return VarEnum.VT_BSTR;

            if (type == typeof(object))
                return VarEnum.VT_VARIANT;

            if (type == typeof(sbyte[]))
                return VarEnum.VT_ARRAY | VarEnum.VT_I1;

            if (type == typeof(byte[]))
                return VarEnum.VT_ARRAY | VarEnum.VT_UI1;

            if (type == typeof(short[]))
                return VarEnum.VT_ARRAY | VarEnum.VT_I2;

            if (type == typeof(ushort[]))
                return VarEnum.VT_ARRAY | VarEnum.VT_UI2;

            if (type == typeof(int[]))
                return VarEnum.VT_ARRAY | VarEnum.VT_I4;

            if (type == typeof(uint[]))
                return VarEnum.VT_ARRAY | VarEnum.VT_UI4;

            if (type == typeof(long[]))
                return VarEnum.VT_ARRAY | VarEnum.VT_I8;

            if (type == typeof(ulong[]))
                return VarEnum.VT_ARRAY | VarEnum.VT_UI8;

            if (type == typeof(float[]))
                return VarEnum.VT_ARRAY | VarEnum.VT_R4;

            if (type == typeof(double[]))
                return VarEnum.VT_ARRAY | VarEnum.VT_R8;

            if (type == typeof(decimal[]))
                return VarEnum.VT_ARRAY | VarEnum.VT_CY;

            if (type == typeof(bool[]))
                return VarEnum.VT_ARRAY | VarEnum.VT_BOOL;

            if (type == typeof(DateTime[]))
                return VarEnum.VT_ARRAY | VarEnum.VT_DATE;

            if (type == typeof(string[]))
                return VarEnum.VT_ARRAY | VarEnum.VT_BSTR;

            if (type == typeof(object[]))
                return VarEnum.VT_ARRAY | VarEnum.VT_VARIANT;

            return VarEnum.VT_EMPTY;
        }

        /// <summary>
        /// Quality to string
        /// </summary>
        /// <param name="Quality">Quality</param>
        /// <returns>String</returns>
        public static string QualityToString(short Quality)
        {
            OPC_QUALITY_MASTER oqm = (OPC_QUALITY_MASTER)(Quality & (short)OPC_QUALITY_MASKS.MASTER_MASK);
            OPC_QUALITY_STATUS oqs = (OPC_QUALITY_STATUS)(Quality & (short)OPC_QUALITY_MASKS.STATUS_MASK);
            OPC_QUALITY_LIMIT oql = (OPC_QUALITY_LIMIT)(Quality & (short)OPC_QUALITY_MASKS.LIMIT_MASK);
            return String.Format("{0}+{1}+{2}", oqm, oqs, oql);
        }

        /// <summary>
        /// Reads a 16-bit signed integer from array at a given index from unmanaged memory.
        /// </summary>
        /// <param name="intPtr">The base address in unmanaged memory array from which to read.</param>
        /// <param name="index">The index of value in unmanaged memory array.</param>
        /// <returns>The 16-bit signed integer read from unmanaged memory array at the given index.</returns>
        /// <exception cref="AccessViolationException">
        /// Base address (intPtr) plus offset byte (index * sizeof(Int16)) produces a null or invalid address.
        /// </exception>
        public static Int16 ReadInt16(IntPtr intPtr, int index)
        {
            return Marshal.ReadInt16(intPtr, index * sizeof(Int16));
        }

        /// <summary>
        /// Reads a 32-bit signed integer from array at a given index from unmanaged memory.
        /// </summary>
        /// <param name="intPtr">The base address in unmanaged memory array from which to read.</param>
        /// <param name="index">The index of value in unmanaged memory array.</param>
        /// <returns>The 32-bit signed integer read from unmanaged memory array at the given index.</returns>
        /// <exception cref="AccessViolationException">
        /// Base address (intPtr) plus offset byte (index * sizeof(Int32)) produces a null or invalid address.
        /// </exception>
        public static Int32 ReadInt32(IntPtr intPtr, int index)
        {
            return Marshal.ReadInt32(intPtr, index * sizeof(Int32));
        }

        /// <summary>
        /// Reads a 64-bit signed integer from array at a given index from unmanaged memory.
        /// </summary>
        /// <param name="intPtr">The base address in unmanaged memory array from which to read.</param>
        /// <param name="index">The index of value in unmanaged memory array.</param>
        /// <returns>The 64-bit signed integer read from unmanaged memory array at the given index.</returns>
        /// <exception cref="AccessViolationException">
        /// Base address (intPtr) plus offset byte (index * sizeof(Int64)) produces a null or invalid address.
        /// </exception>
        public static Int64 ReadInt64(IntPtr intPtr, int index)
        {
            return Marshal.ReadInt64(intPtr, index * sizeof(Int64));
        }

        /// <summary>
        /// Gets COM VARIANT from unmanaged array and converts it to an object.
        /// </summary>
        /// <param name="intPtr">A pointer to a COM VARIANT array</param>
        /// <param name="index">The index of COM VARIANT in array</param>
        /// <returns>An object that corresponds to the COM VARIANT.</returns>
        public static object GetObjectForNativeVariant(IntPtr intPtr, int index)
        {
            return Marshal.GetObjectForNativeVariant(IntPtr.Add(intPtr, index * Extensions.NativeVariantSize));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="structure"></param>
        /// <param name="intPtr"></param>
        /// <param name="fDeleteOld"></param>
        /// <param name="index"></param>
        public static void StructureToPtr(object structure, IntPtr intPtr, bool fDeleteOld, int index)
        {
            Marshal.StructureToPtr(structure, IntPtr.Add(intPtr, index * Marshal.SizeOf(structure)), fDeleteOld);
        }

        /// <summary>
        ///  Writes a 16-bit signed integer value into unmanaged memory short array at a specified index.
        /// </summary>
        /// <param name="intPtr">The base address in unmanaged memory array</param>
        /// <param name="index">The index of value in unmanaged memory array.</param>
        /// <param name="value">The value to write.</param>
        public static void WriteInt16(IntPtr intPtr, int index, short value)
        {
            Marshal.WriteInt16(intPtr, index * sizeof(Int16), value);
        }

    }
}
