// © Alexander Kozlenko. Licensed under the MIT License.

namespace System.Data.JsonRpc
{
    /// <summary>Represents a JSON-RPC message deserialization result.</summary>
    /// <typeparam name="T">The type of the JSON-RPC message.</typeparam>
    public readonly struct JsonRpcItem<T>
        where T : JsonRpcMessage
    {
        private readonly object _value;

        internal JsonRpcItem(object value)
        {
            _value = value;
        }

        /// <summary>Gets a JSON-RPC message for successful deserialization result.</summary>
        public T Message
        {
            get => _value as T;
        }

        /// <summary>Gets an exception for unsuccessful deserialization result.</summary>
        public JsonRpcException Exception
        {
            get => _value as JsonRpcException;
        }

        /// <summary>Gets a value indicating whether the deserialization was successful.</summary>
        public bool IsValid
        {
            get => _value is JsonRpcMessage;
        }
    }
}