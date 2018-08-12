// © Alexander Kozlenko. Licensed under the MIT License.

namespace System.Data.JsonRpc
{
    /// <summary>Represents JSON-RPC method parameters type.</summary>
    public enum JsonRpcParametersType
    {
        /// <summary>JSON-RPC method parameters are not provided.</summary>
        None= 0x00,

        /// <summary>JSON-RPC method parameters are provided by position.</summary>
        ByPosition = 0x01,

        /// <summary>JSON-RPC method parameters are provided by name.</summary>
        ByName = 0x02
    }
}