// © Alexander Kozlenko. Licensed under the MIT License.

using System;

namespace Anemonis.JsonRpc
{
    /// <summary>Represents a type contract for JSON-RPC response deserialization.</summary>
    public sealed class JsonRpcResponseContract : JsonRpcMessageContract
    {
        private readonly Type _resultType;
        private readonly Type _errorDataType;

        /// <summary>Initializes a new instance of the <see cref="JsonRpcResponseContract" /> class.</summary>
        /// <param name="resultType">The type of method result.</param>
        public JsonRpcResponseContract(Type resultType)
        {
            _resultType = resultType;
        }

        /// <summary>Initializes a new instance of the <see cref="JsonRpcResponseContract" /> class.</summary>
        /// <param name="resultType">The type of method result.</param>
        /// <param name="errorDataType">The type of JSON-RPC error optional data.</param>
        public JsonRpcResponseContract(Type resultType, Type errorDataType)
            : this(resultType)
        {
            _errorDataType = errorDataType;
        }

        /// <summary>Gets a type of method result.</summary>
        public Type ResultType
        {
            get => _resultType;
        }

        /// <summary>Gets a type of JSON-RPC error optional data.</summary>
        public Type ErrorDataType
        {
            get => _errorDataType;
        }
    }
}