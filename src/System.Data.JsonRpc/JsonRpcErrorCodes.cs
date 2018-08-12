// © Alexander Kozlenko. Licensed under the MIT License.

namespace System.Data.JsonRpc
{
    /// <summary>Defines standard JSON-RPC error codes and error code ranges.</summary>
    public static class JsonRpcErrorCodes
    {
        /// <summary>The error code which specifies, that the provided JSON is invalid.</summary>
        public const long InvalidJson = -32700L;

        /// <summary>The error code which specifies, that an error occurred during processing the message.</summary>
        public const long InvalidOperation = -32603L;

        /// <summary>The error code which specifies, that the specified method parameters are invalid.</summary>
        public const long InvalidParameters = -32602L;

        /// <summary>The error code which specifies, that the specified method does not exist or is not available.</summary>
        public const long InvalidMethod = -32601L;

        /// <summary>The error code which specifies, that the provided message is not valid.</summary>
        public const long InvalidMessage = -32600L;

        /// <summary>The lower boundary of the implementation-defined server error code range.</summary>
        public const long ServerErrorsLowerBoundary = -32099L;

        /// <summary>The upper boundary of the implementation-defined server error code range.</summary>
        public const long ServerErrorsUpperBoundary = -32000L;

        /// <summary>The lower boundary of the standard error code range.</summary>
        public const long StandardErrorsLowerBoundary = -32768L;

        /// <summary>The upper boundary of the standard error code range.</summary>
        public const long StandardErrorsUpperBoundary = -32000L;
    }
}