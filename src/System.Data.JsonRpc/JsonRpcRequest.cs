// © Alexander Kozlenko. Licensed under the MIT License.

using System.Collections.Generic;

namespace System.Data.JsonRpc
{
    /// <summary>Represents a JSON-RPC request message.</summary>
    public sealed class JsonRpcRequest : JsonRpcMessage
    {
        private readonly string _method;
        private readonly JsonRpcParametersType _parametersType;
        private readonly IReadOnlyDictionary<string, object> _parametersByName;
        private readonly IReadOnlyList<object> _parametersByPosition;

        /// <summary>Initializes a new instance of the <see cref="JsonRpcRequest" /> class.</summary>
        /// <param name="method">The string containing the name of the JSON-RPC method to be invoked.</param>
        /// <exception cref="ArgumentNullException"><paramref name="method" /> is <see langword="null" />.</exception>
        public JsonRpcRequest(string method)
        {
            if (method == null)
            {
                throw new ArgumentNullException(nameof(method));
            }

            _method = method;
        }

        /// <summary>Initializes a new instance of the <see cref="JsonRpcRequest" /> class.</summary>
        /// <param name="method">The string containing the name of the JSON-RPC method to be invoked.</param>
        /// <param name="parameters">The parameters to be used during the invocation of the JSON-RPC method, provided by position.</param>
        /// <exception cref="ArgumentNullException"><paramref name="method" /> or <paramref name="parameters" /> is <see langword="null" />.</exception>
        public JsonRpcRequest(string method, IReadOnlyList<object> parameters)
            : this(method)
        {
            if (parameters == null)
            {
                throw new ArgumentNullException(nameof(parameters));
            }

            _parametersType = JsonRpcParametersType.ByPosition;
            _parametersByPosition = parameters;
        }

        /// <summary>Initializes a new instance of the <see cref="JsonRpcRequest" /> class.</summary>
        /// <param name="method">The string containing the name of the JSON-RPC method to be invoked.</param>
        /// <param name="parameters">The parameters to be used during the invocation of the JSON-RPC method, provided by name.</param>
        /// <exception cref="ArgumentNullException"><paramref name="method" /> or <paramref name="parameters" /> is <see langword="null" />.</exception>
        public JsonRpcRequest(string method, IReadOnlyDictionary<string, object> parameters)
            : this(method)
        {
            if (parameters == null)
            {
                throw new ArgumentNullException(nameof(parameters));
            }

            _parametersType = JsonRpcParametersType.ByName;
            _parametersByName = parameters;
        }

        /// <summary>Initializes a new instance of the <see cref="JsonRpcRequest" /> class.</summary>
        /// <param name="method">The string containing the name of the JSON-RPC method to be invoked.</param>
        /// <param name="id">The identifier established by the client.</param>
        /// <exception cref="ArgumentNullException"><paramref name="method" /> is <see langword="null" />.</exception>
        public JsonRpcRequest(string method, in JsonRpcId id)
            : base(id)
        {
            if (method == null)
            {
                throw new ArgumentNullException(nameof(method));
            }

            _method = method;
        }

        /// <summary>Initializes a new instance of the <see cref="JsonRpcRequest" /> class.</summary>
        /// <param name="method">The string containing the name of the JSON-RPC method to be invoked.</param>
        /// <param name="id">The identifier established by the client.</param>
        /// <param name="parameters">The parameters to be used during the invocation of the JSON-RPC method, provided by position.</param>
        /// <exception cref="ArgumentNullException"><paramref name="method" /> or <paramref name="parameters" /> is <see langword="null" />.</exception>
        public JsonRpcRequest(string method, in JsonRpcId id, IReadOnlyList<object> parameters)
            : this(method, id)
        {
            if (parameters == null)
            {
                throw new ArgumentNullException(nameof(parameters));
            }

            _parametersType = JsonRpcParametersType.ByPosition;
            _parametersByPosition = parameters;
        }

        /// <summary>Initializes a new instance of the <see cref="JsonRpcRequest" /> class.</summary>
        /// <param name="method">The string containing the name of the JSON-RPC method to be invoked.</param>
        /// <param name="id">The identifier established by the client.</param>
        /// <param name="parameters">The parameters to be used during the invocation of the JSON-RPC method, provided by name.</param>
        /// <exception cref="ArgumentNullException"><paramref name="method" /> or <paramref name="parameters" /> is <see langword="null" />.</exception>
        public JsonRpcRequest(string method, in JsonRpcId id, IReadOnlyDictionary<string, object> parameters)
            : this(method, id)
        {
            if (parameters == null)
            {
                throw new ArgumentNullException(nameof(parameters));
            }

            _parametersType = JsonRpcParametersType.ByName;
            _parametersByName = parameters;
        }

        /// <summary>Gets a string containing the name of the JSON-RPC method to be invoked.</summary>
        public string Method
        {
            get => _method;
        }

        /// <summary>Gets the JSON-RPC method parameters type.</summary>
        public JsonRpcParametersType ParametersType
        {
            get => _parametersType;
        }

        /// <summary>Gets the JSON-RPC method parameters, provided by name.</summary>
        public IReadOnlyDictionary<string, object> ParametersByName
        {
            get => _parametersByName;
        }

        /// <summary>Gets the JSON-RPC method parameters, provided by position.</summary>
        public IReadOnlyList<object> ParametersByPosition
        {
            get => _parametersByPosition;
        }

        /// <summary>Gets a value indicating whether the JSON-RPC request is a notification.</summary>
        public bool IsNotification
        {
            get => Id.Type == JsonRpcIdType.None;
        }

        /// <summary>Gets a value indicating whether the JSON-RPC request is a system extension request.</summary>
        public bool IsSystem
        {
            get => JsonRpcProtocol.IsSystemMethod(_method);
        }
    }
}