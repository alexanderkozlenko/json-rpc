// © Alexander Kozlenko. Licensed under the MIT License.

namespace System.Data.JsonRpc
{
    /// <summary>Represents RPC method parameters type.</summary>
    public enum JsonRpcParametersType
    {
        /// <summary>Parameters are not provided.</summary>
        None,

        /// <summary>Parameters are provided by position.</summary>
        ByPosition,

        /// <summary>Parameters are provided by name.</summary>
        ByName
    }
}