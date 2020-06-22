using System;

namespace OPC.Data
{
    /// <summary>
    /// OPC Enumerator Scope
    /// </summary>
    public enum OPCENUMSCOPE
    {
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
        OPC_ENUM_PRIVATE_CONNECTIONS = 1,
        OPC_ENUM_PUBLIC_CONNECTIONS = 2,
        OPC_ENUM_ALL_CONNECTIONS = 3,
        OPC_ENUM_PRIVATE = 4,
        OPC_ENUM_PUBLIC = 5,
        OPC_ENUM_ALL = 6
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
    }
}
