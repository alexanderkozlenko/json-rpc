// © Alexander Kozlenko. Licensed under the MIT License.

using System;

namespace Anemonis.JsonRpc
{
    /// <summary>Represents an error that occurs during JSON-RPC message serialization or deserialization.</summary>
    public sealed class JsonRpcSerializationException : JsonRpcException
    {
        private readonly long _errorCode;
        private readonly JsonRpcId _messageId;

        internal JsonRpcSerializationException(long errorCode, string message)
            : base(message)
        {
            _errorCode = errorCode;
        }

        internal JsonRpcSerializationException(long errorCode, string message, in JsonRpcId messageId)
            : base(message)
        {
            _errorCode = errorCode;
            _messageId = messageId;
        }

        internal JsonRpcSerializationException(long errorCode, string message, in JsonRpcId messageId, Exception innerException)
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