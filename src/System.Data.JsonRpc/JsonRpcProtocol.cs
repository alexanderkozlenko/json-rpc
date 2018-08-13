// © Alexander Kozlenko. Licensed under the MIT License.

namespace System.Data.JsonRpc
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
                ((method[0] == 'r') || (method[0] == 'R')) &&
                ((method[1] == 'p') || (method[1] == 'P')) &&
                ((method[2] == 'c') || (method[2] == 'C')) &&
                ((method[3] == '.'));
        }
    }
}