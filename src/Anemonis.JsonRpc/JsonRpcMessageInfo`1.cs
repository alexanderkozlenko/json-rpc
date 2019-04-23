// © Alexander Kozlenko. Licensed under the MIT License.

namespace Anemonis.JsonRpc
{
    /// <summary>Represents a JSON-RPC message deserialization result.</summary>
    /// <typeparam name="T">The type of the JSON-RPC message.</typeparam>
    public readonly struct JsonRpcMessageInfo<T>
        where T : JsonRpcMessage
    {
        private readonly object _value;

        internal JsonRpcMessageInfo(object value)
        {
            _value = value;
        }

        /// <summary>Gets a JSON-RPC message for successful deserialization.</summary>
        public T Message
        {
            get => _value as T;
        }

        /// <summary>Gets an exception for unsuccessful deserialization.</summary>
        public JsonRpcSerializationException Exception
        {
            get => _value as JsonRpcSerializationException;
        }

        /// <summary>Gets a value indicating whether the deserialization was successful.</summary>
        public bool IsValid
        {
            get => _value is JsonRpcMessage;
        }
    }
}
