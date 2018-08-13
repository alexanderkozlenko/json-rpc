// © Alexander Kozlenko. Licensed under the MIT License.

namespace System.Data.JsonRpc
{
    /// <summary>Represents JSON-RPC message identifier type.</summary>
    public enum JsonRpcIdType
    {
        /// <summary>Undefined JSON-RPC message identifier.</summary>
        None = 0x00,

        /// <summary>JSON-RPC message identifier of string type.</summary>
        String = 0x01,

        /// <summary>JSON-RPC message identifier of integer type.</summary>
        Integer = 0x02,

        /// <summary>JSON-RPC message identifier of float type.</summary>
        Float = 0x03
    }
}