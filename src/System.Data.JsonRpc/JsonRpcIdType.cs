// © Alexander Kozlenko. Licensed under the MIT License.

namespace System.Data.JsonRpc
{
    /// <summary>Represents RPC message identifier type.</summary>
    public enum JsonRpcIdType
    {
        /// <summary>Undefined identifier.</summary>
        None,

        /// <summary>Identifier of string type.</summary>
        String,

        /// <summary>Identifier of integer type.</summary>
        Integer,

        /// <summary>Identifier of float type.</summary>
        Float
    }
}