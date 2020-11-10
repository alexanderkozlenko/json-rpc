// © Alexander Kozlenko. Licensed under the MIT License.

using System.Collections.Generic;

namespace Anemonis.JsonRpc
{
    /// <summary>Represents a JSON-RPC data deserialization result.</summary>
    /// <typeparam name="T">The type of the JSON-RPC message.</typeparam>
    public sealed class JsonRpcData<T>
        where T : JsonRpcMessage
    {
        private readonly JsonRpcMessageInfo<T> _message;
        private readonly IReadOnlyList<JsonRpcMessageInfo<T>> _messages;

        internal JsonRpcData(JsonRpcMessageInfo<T> message)
        {
            _message = message;
        }

        internal JsonRpcData(IReadOnlyList<JsonRpcMessageInfo<T>> messages)
        {
            _messages = messages;
        }

        /// <summary>Gets a value indicating whether the data is a batch.</summary>
        public bool IsBatch
        {
            get => _messages is not null;
        }

        /// <summary>Gets a JSON-RPC message deserialization result for non-batch data.</summary>
        public JsonRpcMessageInfo<T> Item
        {
            get => _message;
        }

        /// <summary>Gets a collection of JSON-RPC message deserialization results for batch data.</summary>
        public IReadOnlyList<JsonRpcMessageInfo<T>> Items
        {
            get => _messages;
        }
    }
}
