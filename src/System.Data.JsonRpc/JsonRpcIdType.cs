// © Alexander Kozlenko. Licensed under the MIT License.

namespace System.Data.JsonRpc
{
    /// <summary>Represents JSON-RPC message identifier type.</summary>
    public enum JsonRpcIdType
    {
        /// <summary>Undefined identifier.</summary>
        None = 0x00,

        /// <summary>Identifier of string type.</summary>
        String = 0x01,

        /// <summary>Identifier of integer type.</summary>
        Integer = 0x02,

        /// <summary>Identifier of float type.</summary>
        Float = 0x03
    }
}