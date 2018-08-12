// © Alexander Kozlenko. Licensed under the MIT License.

namespace System.Data.JsonRpc
{
    /// <summary>Represents JSON-RPC protocol compatibility level.</summary>
    public enum JsonRpcCompatibilityLevel
    {
        /// <summary>Compatibility level that matches the highest JSON-RPC protocol version.</summary>
        Default = 0x00,

        /// <summary>Compatibility level that matches JSON-RPC protocol version 1.0.</summary>
        Level1 = 0x01,

        /// <summary>Compatibility level that matches JSON-RPC protocol version 2.0.</summary>
        Level2 = 0x02
    }
}