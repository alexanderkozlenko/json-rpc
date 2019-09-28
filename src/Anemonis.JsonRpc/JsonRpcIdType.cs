// © Alexander Kozlenko. Licensed under the MIT License.

#pragma warning disable CA1720

namespace Anemonis.JsonRpc
{
    /// <summary>Represents JSON-RPC message identifier type.</summary>
    public enum JsonRpcIdType
    {
        /// <summary>Undefined JSON-RPC message identifier.</summary>
        None = 0x00,

        /// <summary>JSON-RPC message identifier of string type.</summary>
        String = 0x01,

        /// <summary>JSON-RPC message identifier of number type.</summary>
        Number = 0x02
    }
}
