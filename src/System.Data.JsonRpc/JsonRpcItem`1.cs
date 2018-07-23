// © Alexander Kozlenko. Licensed under the MIT License.

namespace System.Data.JsonRpc
{
    /// <summary>Represents information about an RPC message.</summary>
    /// <typeparam name="T">The type of the message.</typeparam>
    public readonly struct JsonRpcItem<T>
        where T : JsonRpcMessage
    {
        private readonly object _value;

        internal JsonRpcItem(object value)
        {
            _value = value;
        }

        /// <summary>Gets a message for the valid item.</summary>
        public T Message
        {
            get => _value as T;
        }

        /// <summary>Gets an exception for the invalid item.</summary>
        public JsonRpcException Exception
        {
            get => _value as JsonRpcException;
        }

        /// <summary>Gets a value indicating whether the item represents a valid message.</summary>
        public bool IsValid
        {
            get => _value is JsonRpcMessage;
        }
    }
}