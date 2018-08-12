// © Alexander Kozlenko. Licensed under the MIT License.

namespace System.Data.JsonRpc
{
    /// <summary>Represents errors that occur during JSON-RPC message processing.</summary>
    public sealed class JsonRpcException : Exception
    {
        private readonly long _errorCode;
        private readonly JsonRpcId _messageId;

        internal JsonRpcException(long errorCode, string message)
            : base(message)
        {
            _errorCode = errorCode;
        }

        internal JsonRpcException(long errorCode, string message, in JsonRpcId messageId)
            : base(message)
        {
            _errorCode = errorCode;
            _messageId = messageId;
        }

        internal JsonRpcException(long errorCode, string message, in JsonRpcId messageId, Exception innerException)
            : base(message, innerException)
        {
            _errorCode = errorCode;
            _messageId = messageId;
        }

        /// <summary>Gets the identifier of the related JSON-RPC message.</summary>
        public ref readonly JsonRpcId MessageId
        {
            get => ref _messageId;
        }

        /// <summary>Gets the corresponding JSON-RPC error code.</summary>
        public long ErrorCode
        {
            get => _errorCode;
        }
    }
}