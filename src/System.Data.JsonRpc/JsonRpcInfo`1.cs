// © Alexander Kozlenko. Licensed under the MIT License.

using System.Collections.Generic;

namespace System.Data.JsonRpc
{
    /// <summary>Represents a JSON-RPC data deserialization result.</summary>
    /// <typeparam name="T">The type of the JSON-RPC message.</typeparam>
    public sealed class JsonRpcInfo<T>
        where T : JsonRpcMessage
    {
        private readonly JsonRpcMessageInfo<T> _message;
        private readonly IReadOnlyList<JsonRpcMessageInfo<T>> _messages;

        internal JsonRpcInfo(JsonRpcMessageInfo<T> message)
        {
            _message = message;
        }

        internal JsonRpcInfo(IReadOnlyList<JsonRpcMessageInfo<T>> messages)
        {
            _messages = messages;
        }

        /// <summary>Gets a value indicating whether the data is a batch.</summary>
        public bool IsBatch
        {
            get => _messages != null;
        }

        /// <summary>Gets a JSON-RPC message deserialization result for non-batch data.</summary>
        public JsonRpcMessageInfo<T> Message
        {
            get => _message;
        }

        /// <summary>Gets a collection of JSON-RPC message deserialization results for batch data.</summary>
        public IReadOnlyList<JsonRpcMessageInfo<T>> Messages
        {
            get => _messages;
        }
    }
}