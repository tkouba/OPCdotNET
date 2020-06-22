using System;

namespace OPC.Data
{
    /// <summary>
    /// OPC Quality Status flags --SSSS--
    /// </summary>
    [Flags]
    public enum OPC_QUALITY_STATUS : short
    {
        /// <summary>
        /// Bad [Non-Specific]
        /// </summary>
        BAD = 0x0000,
        /// <summary>
        /// Bad [Configuration Error]
        /// </summary>
        CONFIG_ERROR = 0x0004,
        /// <summary>
        /// Bad [Not Connected]
        /// </summary>
        NOT_CONNECTED = 0x0008,
        /// <summary>
        /// Bad [Device Failure]
        /// </summary>
        DEVICE_FAILURE = 0x000c,
        /// <summary>
        /// Bad [Sensor Failure]
        /// </summary>
        SENSOR_FAILURE = 0x0010,
        /// <summary>
        /// Bad [Last Known Value]
        /// </summary>
        LAST_KNOWN = 0x0014,
        /// <summary>
        /// Bad [Communication Failure]
        /// </summary>
        COMM_FAILURE = 0x0018,
        /// <summary>
        /// Bad [Out of Service]
        /// </summary>
        OUT_OF_SERVICE = 0x001C,

        /// <summary>
        /// Uncertain [Non-Specific]
        /// </summary>
        UNCERTAIN = 0x0040,
        /// <summary>
        /// Uncertain [Non-Specific] (Low Limited)
        /// </summary>
        LIMIT_LOW = 0x0041,
        /// <summary>
        /// Uncertain [Non-Specific] (High Limited)
        /// </summary>
        LIMIT_HIGH = 0x0042,
        /// <summary>
        /// Uncertain [Non-Specific] (Constant)
        /// </summary>
        LIMIT_CONST = 0x0043,
        /// <summary>
        /// Uncertain [Last Usable]
        /// </summary>
        LAST_USABLE = 0x0044,
        /// <summary>
        /// Uncertain [Sensor Not Accurate]
        /// </summary>
        SENSOR_CAL = 0x0050,
        /// <summary>
        /// Uncertain [EU Exceeded]
        /// </summary>
        EGU_EXCEEDED = 0x0054,
        /// <summary>
        /// Uncertain [Sub-Normal]
        /// </summary>
        SUB_NORMAL = 0x0058,

        /// <summary>
        /// Good [Non-Specific]
        /// </summary>
        OK = 0x00C0,
        /// <summary>
        /// Good [Local Override]
        /// </summary>
        LOCAL_OVERRIDE = 0x00D8
    }
}
