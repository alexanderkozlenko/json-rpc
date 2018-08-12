// © Alexander Kozlenko. Licensed under the MIT License.

using System.Data.JsonRpc.Resources;

namespace System.Data.JsonRpc
{
    /// <summary>Represents a JSON-RPC error.</summary>
    public sealed class JsonRpcError
    {
        private readonly long _code;
        private readonly string _message;
        private readonly object _data;
        private readonly bool _hasData;

        /// <summary>Initializes a new instance of the <see cref="JsonRpcError" /> class.</summary>
        /// <param name="code">The number that indicates the error type that occurred.</param>
        /// <param name="message">The string providing a short description of the error.</param>
        /// <exception cref="ArgumentNullException"><paramref name="message" /> is <see langword="null" />.</exception>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="code" /> is outside the allowable range.</exception>
        public JsonRpcError(long code, string message)
        {
            if ((code >= JsonRpcErrorCodes.StandardErrorsLowerBoundary) && (code <= JsonRpcErrorCodes.StandardErrorsUpperBoundary))
            {
                if ((code < JsonRpcErrorCodes.ServerErrorsLowerBoundary) || (code > JsonRpcErrorCodes.ServerErrorsUpperBoundary))
                {
                    if ((code != JsonRpcErrorCodes.InvalidJson) &&
                        (code != JsonRpcErrorCodes.InvalidOperation) &&
                        (code != JsonRpcErrorCodes.InvalidParameters) &&
                        (code != JsonRpcErrorCodes.InvalidMethod) &&
                        (code != JsonRpcErrorCodes.InvalidMessage))
                    {
                        throw new ArgumentOutOfRangeException(nameof(code), code, Strings.GetString("error.code.invalid_range"));
                    }
                }
            }
            if (message == null)
            {
                throw new ArgumentNullException(nameof(message));
            }

            _code = code;
            _message = message;
        }

        /// <summary>Initializes a new instance of the <see cref="JsonRpcError" /> class.</summary>
        /// <param name="code">The number that indicates the error type that occurred.</param>
        /// <param name="message">The string providing a short description of the error.</param>
        /// <param name="data">The primitive or structured value that contains additional information about the error.</param>
        /// <exception cref="ArgumentNullException"><paramref name="message" /> is <see langword="null" />.</exception>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="code" /> is outside the allowable range.</exception>
        public JsonRpcError(long code, string message, object data)
            : this(code, message)
        {
            _data = data;
            _hasData = true;
        }

        /// <summary>Gets a number that indicates the error type that occurred.</summary>
        public long Code
        {
            get => _code;
        }

        /// <summary>Gets a string providing a short description of the error.</summary>
        public string Message
        {
            get => _message;
        }

        /// <summary>Gets an optional value that contains additional information about the error.</summary>
        public object Data
        {
            get => _data;
        }

        /// <summary>Gets a value indicating whether the additional information about the error is specified.</summary>
        public bool HasData
        {
            get => _hasData;
        }
    }
}