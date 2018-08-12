// © Alexander Kozlenko. Licensed under the MIT License.

namespace System.Data.JsonRpc
{
    /// <summary>Represents JSON-RPC method parameters type.</summary>
    public enum JsonRpcParametersType
    {
        /// <summary>JSON-RPC method parameters are not provided.</summary>
        None,

        /// <summary>JSON-RPC method parameters are provided by position.</summary>
        ByPosition,

        /// <summary>JSON-RPC method parameters are provided by name.</summary>
        ByName
    }
}