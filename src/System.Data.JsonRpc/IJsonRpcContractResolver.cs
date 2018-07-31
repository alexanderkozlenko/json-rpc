// © Alexander Kozlenko. Licensed under the MIT License.

namespace System.Data.JsonRpc
{
    /// <summary>Defines a JSON-RPC message contract resolver.</summary>
    public interface IJsonRpcContractResolver
    {
        /// <summary>Gets a request contract.</summary>
        /// <param name="method">The name of a JSON-RPC method.</param>
        /// <returns>The corresponding request contract or <see langword="null" />.</returns>
        JsonRpcRequestContract GetRequestContract(string method);

        /// <summary>Gets a response contract.</summary>
        /// <param name="messageId">The identifier of a JSON-RPC message.</param>
        /// <returns>The corresponding response contract or <see langword="null" />.</returns>
        JsonRpcResponseContract GetResponseContract(in JsonRpcId messageId);

        /// <summary>Gets the type of error data for generic error response.</summary>
        /// <returns>An instance of <see cref="Type" /> type or <see langword="null" />.</returns>
        Type GetGenericErrorDataType();
    }
}