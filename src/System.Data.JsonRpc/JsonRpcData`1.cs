// © Alexander Kozlenko. Licensed under the MIT License.

using System.Collections.Generic;

namespace System.Data.JsonRpc
{
    /// <summary>Represents a JSON-RPC data deserialization result.</summary>
    /// <typeparam name="T">The type of the JSON-RPC message.</typeparam>
    public sealed class JsonRpcData<T>
        where T : JsonRpcMessage
    {
        private readonly JsonRpcItem<T> _item;
        private readonly IReadOnlyList<JsonRpcItem<T>> _items;

        internal JsonRpcData(JsonRpcItem<T> item)
        {
            _item = item;
        }

        internal JsonRpcData(IReadOnlyList<JsonRpcItem<T>> items)
        {
            _items = items;
        }

        /// <summary>Gets a value indicating whether the data is a batch.</summary>
        public bool IsBatch
        {
            get => _items != null;
        }

        /// <summary>Gets a JSON-RPC message deserialization result for non-batch data.</summary>
        public JsonRpcItem<T> Item
        {
            get => _item;
        }

        /// <summary>Gets a collection of JSON-RPC message deserialization results for batch data.</summary>
        public IReadOnlyList<JsonRpcItem<T>> Items
        {
            get => _items;
        }
    }
}