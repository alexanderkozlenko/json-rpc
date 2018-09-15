// © Alexander Kozlenko. Licensed under the MIT License.

using System;
using System.Collections.Generic;

namespace Anemonis.JsonRpc
{
    /// <summary>Represents a type contract for JSON-RPC request deserialization.</summary>
    public sealed class JsonRpcRequestContract : JsonRpcMessageContract
    {
        private readonly JsonRpcParametersType _parametersType;
        private readonly IReadOnlyDictionary<string, Type> _parametersByName;
        private readonly IReadOnlyList<Type> _parametersByPosition;

        /// <summary>Initializes a new instance of the <see cref="JsonRpcRequestContract" /> class.</summary>
        public JsonRpcRequestContract()
        {
        }

        /// <summary>Initializes a new instance of the <see cref="JsonRpcRequestContract" /> class.</summary>
        /// <param name="parameters">The contract for parameters, provided by position.</param>
        /// <exception cref="ArgumentNullException"><paramref name="parameters" /> is <see langword="null" />.</exception>
        public JsonRpcRequestContract(IReadOnlyList<Type> parameters)
        {
            if (parameters == null)
            {
                throw new ArgumentNullException(nameof(parameters));
            }

            _parametersType = JsonRpcParametersType.ByPosition;
            _parametersByPosition = parameters;
        }

        /// <summary>Initializes a new instance of the <see cref="JsonRpcRequestContract" /> class.</summary>
        /// <param name="parameters">The contract for parameters, provided by name.</param>
        /// <exception cref="ArgumentNullException"><paramref name="parameters" /> is <see langword="null" />.</exception>
        public JsonRpcRequestContract(IReadOnlyDictionary<string, Type> parameters)
        {
            if (parameters == null)
            {
                throw new ArgumentNullException(nameof(parameters));
            }

            _parametersType = JsonRpcParametersType.ByName;
            _parametersByName = parameters;
        }

        /// <summary>Gets the JSON-RPC method parameters type.</summary>
        public JsonRpcParametersType ParametersType
        {
            get => _parametersType;
        }

        /// <summary>Gets the types of JSON-RPC method parameters, provided by name.</summary>
        public IReadOnlyDictionary<string, Type> ParametersByName
        {
            get => _parametersByName;
        }

        /// <summary>Gets the types of JSON-RPC method parameters, provided by position.</summary>
        public IReadOnlyList<Type> ParametersByPosition
        {
            get => _parametersByPosition;
        }
    }
}