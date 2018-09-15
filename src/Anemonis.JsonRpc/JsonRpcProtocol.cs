// © Alexander Kozlenko. Licensed under the MIT License.

using System;

namespace Anemonis.JsonRpc
{
    /// <summary>Defines JSON-RPC protocol auxiliary methods.</summary>
    public static class JsonRpcProtocol
    {
        /// <summary>Checks whether the JSON-RPC method is a JSON-RPC system extension method.</summary>
        /// <param name="method">The name of a JSON-RPC method.</param>
        /// <returns><see langword="true" /> if the specified JSON-RPC method is a JSON-RPC system extension method; otherwise, <see langword="false" />.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="method" /> is <see langword="null" />.</exception>
        public static bool IsSystemMethod(string method)
        {
            if (method == null)
            {
                throw new ArgumentNullException(nameof(method));
            }

            return
                ((method.Length >= 4)) &&
                ((method[3] == '.')) &&
                ((method[0] == 'r') || (method[0] == 'R')) &&
                ((method[1] == 'p') || (method[1] == 'P')) &&
                ((method[2] == 'c') || (method[2] == 'C'));
        }

        /// <summary>Checks whether the JSON-RPC error code is in range of JSON-RPC system error codes.</summary>
        /// <param name="code">The JSON-RPC error code.</param>
        /// <returns><see langword="true" /> if the specified JSON-RPC error code is in range of JSON-RPC system error codes; otherwise, <see langword="false" />.</returns>
        public static bool IsSystemErrorCode(long code)
        {
            return (JsonRpcErrorCode.SystemErrorsLowerBoundary <= code) && (code <= JsonRpcErrorCode.SystemErrorsUpperBoundary);
        }

        /// <summary>Checks whether the JSON-RPC error code is in range of JSON-RPC server error codes.</summary>
        /// <param name="code">The JSON-RPC error code.</param>
        /// <returns><see langword="true" /> if the specified JSON-RPC error code is in range of JSON-RPC server error codes; otherwise, <see langword="false" />.</returns>
        public static bool IsServerErrorCode(long code)
        {
            return (JsonRpcErrorCode.ServerErrorsLowerBoundary <= code) && (code <= JsonRpcErrorCode.ServerErrorsUpperBoundary);
        }

        /// <summary>Checks whether the JSON-RPC error code is one of the standard JSON-RPC system error codes.</summary>
        /// <param name="code">The JSON-RPC error code.</param>
        /// <returns><see langword="true" /> if the specified JSON-RPC error code is one of the standard JSON-RPC system error codes; otherwise, <see langword="false" />.</returns>
        public static bool IsStandardErrorCode(long code)
        {
            return
                (code == JsonRpcErrorCode.InvalidFormat) ||
                (code == JsonRpcErrorCode.InvalidOperation) ||
                (code == JsonRpcErrorCode.InvalidParameters) ||
                (code == JsonRpcErrorCode.InvalidMethod) ||
                (code == JsonRpcErrorCode.InvalidMessage);
        }
    }
}