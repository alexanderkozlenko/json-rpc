// © Alexander Kozlenko. Licensed under the MIT License.

using System;

namespace Anemonis.JsonRpc
{
    /// <summary>Represents a JSON-RPC message deserialization result.</summary>
    /// <typeparam name="T">The type of the JSON-RPC message.</typeparam>
    public readonly struct JsonRpcMessageInfo<T> : IEquatable<JsonRpcMessageInfo<T>>
        where T : JsonRpcMessage
    {
        private readonly object _value;

        internal JsonRpcMessageInfo(object value)
        {
            _value = value;
        }

        /// <inheritdoc />
        public override bool Equals(object obj)
        {
            return (obj is JsonRpcMessageInfo<T> other) && Equals(other);
        }

        /// <summary>Indicates whether the current <see cref="JsonRpcMessageInfo{T}" /> is equal to another <see cref="JsonRpcMessageInfo{T}" />.</summary>
        /// <param name="other">A <see cref="JsonRpcMessageInfo{T}" /> to compare with the current <see cref="JsonRpcMessageInfo{T}" />.</param>
        /// <returns><see langword="true" /> if the current <see cref="JsonRpcMessageInfo{T}" /> is equal to the other <see cref="JsonRpcMessageInfo{T}" />; otherwise, <see langword="false" />.</returns>
        public bool Equals(JsonRpcMessageInfo<T> other)
        {
            return object.ReferenceEquals(_value, other._value);
        }

        /// <inheritdoc />
        public override int GetHashCode()
        {
            return HashCode.Combine(_value);
        }

        /// <summary>Indicates whether the left <see cref="JsonRpcMessageInfo{T}" /> is equal to the right <see cref="JsonRpcMessageInfo{T}" />.</summary>
        /// <param name="obj1">The left <see cref="JsonRpcMessageInfo{T}" /> operand.</param>
        /// <param name="obj2">The right <see cref="JsonRpcMessageInfo{T}" /> operand.</param>
        /// <returns><see langword="true" /> if the left <see cref="JsonRpcMessageInfo{T}" /> is equal to the right <see cref="JsonRpcMessageInfo{T}" />; otherwise, <see langword="false" />.</returns>
        public static bool operator ==(JsonRpcMessageInfo<T> obj1, JsonRpcMessageInfo<T> obj2)
        {
            return obj1.Equals(obj2);
        }

        /// <summary>Indicates whether the left <see cref="JsonRpcMessageInfo{T}" /> is not equal to the right <see cref="JsonRpcMessageInfo{T}" />.</summary>
        /// <param name="obj1">The left <see cref="JsonRpcMessageInfo{T}" /> operand.</param>
        /// <param name="obj2">The right <see cref="JsonRpcMessageInfo{T}" /> operand.</param>
        /// <returns><see langword="true" /> if the left <see cref="JsonRpcMessageInfo{T}" /> is not equal to the right <see cref="JsonRpcMessageInfo{T}" />; otherwise, <see langword="false" />.</returns>
        public static bool operator !=(JsonRpcMessageInfo<T> obj1, JsonRpcMessageInfo<T> obj2)
        {
            return !obj1.Equals(obj2);
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
