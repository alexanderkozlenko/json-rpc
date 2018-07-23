// © Alexander Kozlenko. Licensed under the MIT License.

using System.Collections.Generic;

namespace System.Data.JsonRpc
{
    /// <summary>Represents an RPC request message.</summary>
    public sealed class JsonRpcRequest : JsonRpcMessage
    {
        private readonly string _method;
        private readonly JsonRpcParametersType _parametersType;
        private readonly IReadOnlyDictionary<string, object> _parametersByName;
        private readonly IReadOnlyList<object> _parametersByPosition;

        /// <summary>Initializes a new instance of the <see cref="JsonRpcRequest" /> class.</summary>
        /// <param name="method">The string containing the name of the method to be invoked.</param>
        /// <param name="id">The identifier established by the client.</param>
        /// <exception cref="ArgumentNullException"><paramref name="method" /> is <see langword="null" />.</exception>
        public JsonRpcRequest(string method, in JsonRpcId id = default)
            : base(id)
        {
            if (method == null)
            {
                throw new ArgumentNullException(nameof(method));
            }

            _method = method;
        }

        /// <summary>Initializes a new instance of the <see cref="JsonRpcRequest" /> class.</summary>
        /// <param name="method">The string containing the name of the method to be invoked.</param>
        /// <param name="id">The identifier established by the client.</param>
        /// <param name="parameters">The parameters to be used during the invocation of the method, provided by position.</param>
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
        /// <param name="method">The string containing the name of the method to be invoked.</param>
        /// <param name="parameters">The parameters to be used during the invocation of the method, provided by position.</param>
        /// <exception cref="ArgumentNullException"><paramref name="method" /> or <paramref name="parameters" /> is <see langword="null" />.</exception>
        public JsonRpcRequest(string method, IReadOnlyList<object> parameters)
            : this(method, default, parameters)
        {
        }

        /// <summary>Initializes a new instance of the <see cref="JsonRpcRequest" /> class.</summary>
        /// <param name="method">The string containing the name of the method to be invoked.</param>
        /// <param name="id">The identifier established by the client.</param>
        /// <param name="parameters">The parameters to be used during the invocation of the method, provided by name.</param>
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

        /// <summary>Initializes a new instance of the <see cref="JsonRpcRequest" /> class.</summary>
        /// <param name="method">The string containing the name of the method to be invoked.</param>
        /// <param name="parameters">The parameters to be used during the invocation of the method, provided by name.</param>
        /// <exception cref="ArgumentNullException"><paramref name="method" /> or <paramref name="parameters" /> is <see langword="null" />.</exception>
        public JsonRpcRequest(string method, IReadOnlyDictionary<string, object> parameters)
            : this(method, default, parameters)
        {
        }

        /// <summary>Checks whether the method is a system extension method.</summary>
        /// <param name="method">The method name.</param>
        /// <returns><see langword="true" /> if the specified method is a system extension method; otherwise, <see langword="false" />.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="method" /> is <see langword="null" />.</exception>
        public static bool IsSystemMethod(string method)
        {
            if (method == null)
            {
                throw new ArgumentNullException(nameof(method));
            }

            return
                ((method.Length >= 4)) &&
                ((method[0] == 'r') || (method[0] == 'R')) &&
                ((method[1] == 'p') || (method[1] == 'P')) &&
                ((method[2] == 'c') || (method[2] == 'C')) &&
                ((method[3] == '.'));
        }

        /// <summary>Gets a string containing the name of the method to be invoked.</summary>
        public string Method
        {
            get => _method;
        }

        /// <summary>Gets parameters type.</summary>
        public JsonRpcParametersType ParametersType
        {
            get => _parametersType;
        }

        /// <summary>Gets parameters, provided by name.</summary>
        public IReadOnlyDictionary<string, object> ParametersByName
        {
            get => _parametersByName;
        }

        /// <summary>Gets parameters, provided by position.</summary>
        public IReadOnlyList<object> ParametersByPosition
        {
            get => _parametersByPosition;
        }

        /// <summary>Gets a value indicating whether the message is a notification.</summary>
        public bool IsNotification
        {
            get => Id.Type == JsonRpcIdType.None;
        }

        /// <summary>Gets a value indicating whether the message is a system extension.</summary>
        public bool IsSystem
        {
            get => IsSystemMethod(_method);
        }
    }
}