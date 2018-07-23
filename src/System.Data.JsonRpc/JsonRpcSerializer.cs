// © Alexander Kozlenko. Licensed under the MIT License.

using System.Collections.Generic;
using System.Data.JsonRpc.Resources;
using System.Globalization;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace System.Data.JsonRpc
{
    /// <summary>Serializes and deserializes JSON-RPC messages into and from the JSON format.</summary>
    public sealed partial class JsonRpcSerializer : IDisposable
    {
        private const int _minimumMessageSize = 32;
        private const int _streamBufferSize = 1024;

        private static readonly Encoding _streamEncoding = new UTF8Encoding(false);
        private static readonly IArrayPool<char> _jsonBufferPool = new JsonBufferPool();

        private readonly IDictionary<string, JsonRpcRequestContract> _requestContracts;
        private readonly IDictionary<string, JsonRpcResponseContract> _responseContracts;
        private readonly IDictionary<JsonRpcId, string> _staticResponseBindings;
        private readonly IDictionary<JsonRpcId, JsonRpcResponseContract> _dynamicResponseBindings;

        private Type _defaultErrorDataType;
        private JsonRpcCompatibilityLevel _compatibilityLevel;

        /// <summary>Initializes a new instance of the <see cref="JsonRpcSerializer" /> class.</summary>
        /// <param name="requestContracts">The request contracts.</param>
        /// <param name="responseContracts">The response contracts.</param>
        /// <param name="staticResponseBindings">The static response bindings.</param>
        /// <param name="dynamicResponseBindings">The dynamic response bindings.</param>
        public JsonRpcSerializer(
            IDictionary<string, JsonRpcRequestContract> requestContracts = null,
            IDictionary<string, JsonRpcResponseContract> responseContracts = null,
            IDictionary<JsonRpcId, string> staticResponseBindings = null,
            IDictionary<JsonRpcId, JsonRpcResponseContract> dynamicResponseBindings = null)
        {
            _requestContracts = requestContracts ?? new Dictionary<string, JsonRpcRequestContract>(StringComparer.Ordinal);
            _responseContracts = responseContracts ?? new Dictionary<string, JsonRpcResponseContract>(StringComparer.Ordinal);
            _staticResponseBindings = staticResponseBindings ?? new Dictionary<JsonRpcId, string>();
            _dynamicResponseBindings = dynamicResponseBindings ?? new Dictionary<JsonRpcId, JsonRpcResponseContract>();
            _compatibilityLevel = JsonRpcCompatibilityLevel.Level2;
        }

        /// <summary>Deserializes the specified JSON string to request data.</summary>
        /// <param name="json">The JSON string to deserialize.</param>
        /// <returns>RPC information about requests.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="json" /> is <see langword="null" />.</exception>
        /// <exception cref="JsonRpcException">An error occurred during request(s) deserialization.</exception>
        public JsonRpcData<JsonRpcRequest> DeserializeRequestData(string json)
        {
            if (json == null)
            {
                throw new ArgumentNullException(nameof(json));
            }

            using (var stringReader = new StringReader(json))
            {
                try
                {
                    using (var jsonReader = new JsonTextReader(stringReader))
                    {
                        jsonReader.ArrayPool = _jsonBufferPool;

                        return DeserializeRequestData(jsonReader);
                    }
                }
                catch (JsonException e)
                {
                    throw new JsonRpcException(JsonRpcErrorCodes.InvalidJson, Strings.GetString("core.deserialize.json_issue"), default, e);
                }
            }
        }

        /// <summary>Deserializes the specified stream with a JSON string to request data.</summary>
        /// <param name="stream">The stream with a JSON string to deserialize.</param>
        /// <returns>RPC information about requests.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="stream" /> is <see langword="null" />.</exception>
        /// <exception cref="JsonRpcException">An error occurred during request(s) deserialization.</exception>
        public JsonRpcData<JsonRpcRequest> DeserializeRequestData(Stream stream)
        {
            if (stream == null)
            {
                throw new ArgumentNullException(nameof(stream));
            }

            using (var streamReader = new StreamReader(stream, _streamEncoding, false, _streamBufferSize, true))
            {
                try
                {
                    using (var jsonReader = new JsonTextReader(streamReader))
                    {
                        jsonReader.ArrayPool = _jsonBufferPool;

                        return DeserializeRequestData(jsonReader);
                    }
                }
                catch (JsonException e)
                {
                    throw new JsonRpcException(JsonRpcErrorCodes.InvalidJson, Strings.GetString("core.deserialize.json_issue"), default, e);
                }
            }
        }

        /// <summary>Deserializes the specified stream with a JSON string to request data as an asynchronous operation.</summary>
        /// <param name="stream">The stream with a JSON string to deserialize.</param>
        /// <param name="cancellationToken">The cancellation token for canceling the operation.</param>
        /// <returns>A task that represents the asynchronous operation. The task result is RPC information about requests.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="stream" /> is <see langword="null" />.</exception>
        /// <exception cref="JsonRpcException">An error occurred during request(s) deserialization.</exception>
        /// <exception cref="OperationCanceledException">The operation was canceled.</exception>
        public Task<JsonRpcData<JsonRpcRequest>> DeserializeRequestDataAsync(Stream stream, CancellationToken cancellationToken = default)
        {
            if (stream == null)
            {
                throw new ArgumentNullException(nameof(stream));
            }

            using (var streamReader = new StreamReader(stream, _streamEncoding, false, _streamBufferSize, true))
            {
                try
                {
                    using (var jsonReader = new JsonTextReader(streamReader))
                    {
                        jsonReader.ArrayPool = _jsonBufferPool;

                        return Task.FromResult(DeserializeRequestData(jsonReader, cancellationToken));
                    }
                }
                catch (JsonException e)
                {
                    throw new JsonRpcException(JsonRpcErrorCodes.InvalidJson, Strings.GetString("core.deserialize.json_issue"), default, e);
                }
            }
        }

        /// <summary>Deserializes the specified JSON string to response data.</summary>
        /// <param name="json">The JSON string to deserialize.</param>
        /// <returns>RPC information about responses.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="json" /> is <see langword="null" />.</exception>
        /// <exception cref="JsonRpcException">An error occurred during response(s) deserialization.</exception>
        public JsonRpcData<JsonRpcResponse> DeserializeResponseData(string json)
        {
            if (json == null)
            {
                throw new ArgumentNullException(nameof(json));
            }

            using (var stringReader = new StringReader(json))
            {
                try
                {
                    using (var jsonReader = new JsonTextReader(stringReader))
                    {
                        jsonReader.ArrayPool = _jsonBufferPool;

                        return DeserializeResponseData(jsonReader);
                    }
                }
                catch (JsonException e)
                {
                    throw new JsonRpcException(JsonRpcErrorCodes.InvalidJson, Strings.GetString("core.deserialize.json_issue"), default, e);
                }
            }
        }

        /// <summary>Deserializes the specified stream with a JSON string to response data.</summary>
        /// <param name="stream">The stream with a JSON string to deserialize.</param>
        /// <returns>RPC information about responses.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="stream" /> is <see langword="null" />.</exception>
        /// <exception cref="JsonRpcException">An error occurred during response(s) deserialization.</exception>
        public JsonRpcData<JsonRpcResponse> DeserializeResponseData(Stream stream)
        {
            if (stream == null)
            {
                throw new ArgumentNullException(nameof(stream));
            }

            using (var streamReader = new StreamReader(stream, _streamEncoding, false, _streamBufferSize, true))
            {
                try
                {
                    using (var jsonReader = new JsonTextReader(streamReader))
                    {
                        jsonReader.ArrayPool = _jsonBufferPool;

                        return DeserializeResponseData(jsonReader);
                    }
                }
                catch (JsonException e)
                {
                    throw new JsonRpcException(JsonRpcErrorCodes.InvalidJson, Strings.GetString("core.deserialize.json_issue"), default, e);
                }
            }
        }

        /// <summary>Deserializes the specified stream with a JSON string to response data as an asynchronous operation.</summary>
        /// <param name="stream">The stream with a JSON string to deserialize.</param>
        /// <param name="cancellationToken">The cancellation token for canceling the operation.</param>
        /// <returns>A task that represents the asynchronous operation. The task result is RPC information about responses.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="stream" /> is <see langword="null" />.</exception>
        /// <exception cref="JsonRpcException">An error occurred during response(s) deserialization.</exception>
        /// <exception cref="OperationCanceledException">The operation was canceled.</exception>
        public Task<JsonRpcData<JsonRpcResponse>> DeserializeResponseDataAsync(Stream stream, CancellationToken cancellationToken = default)
        {
            if (stream == null)
            {
                throw new ArgumentNullException(nameof(stream));
            }

            using (var streamReader = new StreamReader(stream, _streamEncoding, false, _streamBufferSize, true))
            {
                try
                {
                    using (var jsonReader = new JsonTextReader(streamReader))
                    {
                        jsonReader.ArrayPool = _jsonBufferPool;

                        return Task.FromResult(DeserializeResponseData(jsonReader, cancellationToken));
                    }
                }
                catch (JsonException e)
                {
                    throw new JsonRpcException(JsonRpcErrorCodes.InvalidJson, Strings.GetString("core.deserialize.json_issue"), default, e);
                }
            }
        }

        /// <summary>Serializes the specified request to a JSON string.</summary>
        /// <param name="request">The request to serialize.</param>
        /// <returns>A JSON string representation of the specified request.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="request" /> is <see langword="null" />.</exception>
        /// <exception cref="JsonRpcException">An error occurred during request serialization.</exception>
        public string SerializeRequest(JsonRpcRequest request)
        {
            if (request == null)
            {
                throw new ArgumentNullException(nameof(request));
            }

            using (var stringWriter = new StringWriter(new StringBuilder(_minimumMessageSize), CultureInfo.InvariantCulture))
            {
                try
                {
                    using (var jsonWriter = new JsonTextWriter(stringWriter))
                    {
                        jsonWriter.AutoCompleteOnClose = false;
                        jsonWriter.ArrayPool = _jsonBufferPool;

                        SerializeRequest(jsonWriter, request);
                    }
                }
                catch (JsonException e)
                {
                    throw new JsonRpcException(JsonRpcErrorCodes.InvalidOperation, Strings.GetString("core.serialize.json_issue"), request.Id, e);
                }

                return stringWriter.ToString();
            }
        }

        /// <summary>Serializes the specified request to the specified stream.</summary>
        /// <param name="request">The request to serialize.</param>
        /// <param name="stream">The stream for a JSON string.</param>
        /// <exception cref="ArgumentNullException"><paramref name="request" /> or <paramref name="stream" /> is <see langword="null" />.</exception>
        /// <exception cref="JsonRpcException">An error occurred during request serialization.</exception>
        public void SerializeRequest(JsonRpcRequest request, Stream stream)
        {
            if (request == null)
            {
                throw new ArgumentNullException(nameof(request));
            }
            if (stream == null)
            {
                throw new ArgumentNullException(nameof(stream));
            }

            using (var streamWriter = new StreamWriter(stream, _streamEncoding, _streamBufferSize, true))
            {
                try
                {
                    using (var jsonWriter = new JsonTextWriter(streamWriter))
                    {
                        jsonWriter.AutoCompleteOnClose = false;
                        jsonWriter.ArrayPool = _jsonBufferPool;

                        SerializeRequest(jsonWriter, request);
                    }
                }
                catch (JsonException e)
                {
                    throw new JsonRpcException(JsonRpcErrorCodes.InvalidOperation, Strings.GetString("core.serialize.json_issue"), request.Id, e);
                }
            }
        }

        /// <summary>Serializes the specified request to the specified stream as an asynchronous operation.</summary>
        /// <param name="request">The request to serialize.</param>
        /// <param name="stream">The stream for a JSON string.</param>
        /// <param name="cancellationToken">The cancellation token for canceling the operation.</param>
        /// <returns>A task that represents the asynchronous operation. The task result is a JSON string representation of the specified request.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="request" /> or <paramref name="stream" /> is <see langword="null" />.</exception>
        /// <exception cref="JsonRpcException">An error occurred during request serialization.</exception>
        /// <exception cref="OperationCanceledException">The operation was canceled.</exception>
        public Task SerializeRequestAsync(JsonRpcRequest request, Stream stream, CancellationToken cancellationToken = default)
        {
            if (request == null)
            {
                throw new ArgumentNullException(nameof(request));
            }
            if (stream == null)
            {
                throw new ArgumentNullException(nameof(stream));
            }

            cancellationToken.ThrowIfCancellationRequested();

            using (var streamWriter = new StreamWriter(stream, _streamEncoding, _streamBufferSize, true))
            {
                try
                {
                    using (var jsonWriter = new JsonTextWriter(streamWriter))
                    {
                        jsonWriter.AutoCompleteOnClose = false;
                        jsonWriter.ArrayPool = _jsonBufferPool;

                        SerializeRequest(jsonWriter, request);
                    }
                }
                catch (JsonException e)
                {
                    throw new JsonRpcException(JsonRpcErrorCodes.InvalidOperation, Strings.GetString("core.serialize.json_issue"), request.Id, e);
                }
            }

            return Task.FromResult<object>(null);
        }

        /// <summary>Serializes the specified collection of requests to a JSON string.</summary>
        /// <param name="requests">The collection of requests to serialize.</param>
        /// <returns>A JSON string representation of the specified collection of requests.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="requests" /> is <see langword="null" />.</exception>
        /// <exception cref="JsonRpcException">An error occurred during requests serialization.</exception>
        public string SerializeRequests(IReadOnlyList<JsonRpcRequest> requests)
        {
            if (requests == null)
            {
                throw new ArgumentNullException(nameof(requests));
            }

            using (var stringWriter = new StringWriter(new StringBuilder(_minimumMessageSize * requests.Count), CultureInfo.InvariantCulture))
            {
                try
                {
                    using (var jsonWriter = new JsonTextWriter(stringWriter))
                    {
                        jsonWriter.AutoCompleteOnClose = false;
                        jsonWriter.ArrayPool = _jsonBufferPool;

                        SerializeRequests(jsonWriter, requests);
                    }
                }
                catch (JsonException e)
                {
                    throw new JsonRpcException(JsonRpcErrorCodes.InvalidOperation, Strings.GetString("core.serialize.json_issue"), default, e);
                }

                return stringWriter.ToString();
            }
        }

        /// <summary>Serializes the specified collection of requests to the specified stream.</summary>
        /// <param name="requests">The collection of requests to serialize.</param>
        /// <param name="stream">The stream for a JSON string.</param>
        /// <exception cref="ArgumentNullException"><paramref name="requests" /> or <paramref name="stream" /> is <see langword="null" />.</exception>
        /// <exception cref="JsonRpcException">An error occurred during requests serialization.</exception>
        public void SerializeRequests(IReadOnlyList<JsonRpcRequest> requests, Stream stream)
        {
            if (requests == null)
            {
                throw new ArgumentNullException(nameof(requests));
            }
            if (stream == null)
            {
                throw new ArgumentNullException(nameof(stream));
            }

            using (var streamWriter = new StreamWriter(stream, _streamEncoding, _streamBufferSize, true))
            {
                try
                {
                    using (var jsonWriter = new JsonTextWriter(streamWriter))
                    {
                        jsonWriter.AutoCompleteOnClose = false;
                        jsonWriter.ArrayPool = _jsonBufferPool;

                        SerializeRequests(jsonWriter, requests);
                    }
                }
                catch (JsonException e)
                {
                    throw new JsonRpcException(JsonRpcErrorCodes.InvalidOperation, Strings.GetString("core.serialize.json_issue"), default, e);
                }
            }
        }

        /// <summary>Serializes the specified collection of requests to the specified stream as an asynchronous operation.</summary>
        /// <param name="requests">The collection of requests to serialize.</param>
        /// <param name="stream">The stream for a JSON string.</param>
        /// <param name="cancellationToken">The cancellation token for canceling the operation.</param>
        /// <returns>A task that represents the asynchronous operation. The task result is a JSON string representation of the specified collection of requests.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="requests" /> or <paramref name="stream" /> is <see langword="null" />.</exception>
        /// <exception cref="JsonRpcException">An error occurred during requests serialization.</exception>
        /// <exception cref="OperationCanceledException">The operation was canceled.</exception>
        public Task SerializeRequestsAsync(IReadOnlyList<JsonRpcRequest> requests, Stream stream, CancellationToken cancellationToken = default)
        {
            if (requests == null)
            {
                throw new ArgumentNullException(nameof(requests));
            }
            if (stream == null)
            {
                throw new ArgumentNullException(nameof(stream));
            }

            cancellationToken.ThrowIfCancellationRequested();

            using (var streamWriter = new StreamWriter(stream, _streamEncoding, _streamBufferSize, true))
            {
                try
                {
                    using (var jsonWriter = new JsonTextWriter(streamWriter))
                    {
                        jsonWriter.AutoCompleteOnClose = false;
                        jsonWriter.ArrayPool = _jsonBufferPool;

                        SerializeRequests(jsonWriter, requests, cancellationToken);
                    }
                }
                catch (JsonException e)
                {
                    throw new JsonRpcException(JsonRpcErrorCodes.InvalidOperation, Strings.GetString("core.serialize.json_issue"), default, e);
                }
            }

            return Task.FromResult<object>(null);
        }

        /// <summary>Serializes the specified response to a JSON string.</summary>
        /// <param name="response">The response to serialize.</param>
        /// <returns>A JSON string representation of the specified response.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="response" /> is <see langword="null" />.</exception>
        /// <exception cref="JsonRpcException">An error occurred during response serialization.</exception>
        public string SerializeResponse(JsonRpcResponse response)
        {
            if (response == null)
            {
                throw new ArgumentNullException(nameof(response));
            }

            using (var stringWriter = new StringWriter(new StringBuilder(_minimumMessageSize), CultureInfo.InvariantCulture))
            {
                try
                {
                    using (var jsonWriter = new JsonTextWriter(stringWriter))
                    {
                        jsonWriter.AutoCompleteOnClose = false;
                        jsonWriter.ArrayPool = _jsonBufferPool;

                        SerializeResponse(jsonWriter, response);
                    }
                }
                catch (JsonException e)
                {
                    throw new JsonRpcException(JsonRpcErrorCodes.InvalidOperation, Strings.GetString("core.serialize.json_issue"), response.Id, e);
                }

                return stringWriter.ToString();
            }
        }

        /// <summary>Serializes the specified response to the specified stream.</summary>
        /// <param name="response">The response to serialize.</param>
        /// <param name="stream">The stream for a JSON string.</param>
        /// <exception cref="ArgumentNullException"><paramref name="response" /> or <paramref name="stream" /> is <see langword="null" />.</exception>
        /// <exception cref="JsonRpcException">An error occurred during response serialization.</exception>
        public void SerializeResponse(JsonRpcResponse response, Stream stream)
        {
            if (response == null)
            {
                throw new ArgumentNullException(nameof(response));
            }
            if (stream == null)
            {
                throw new ArgumentNullException(nameof(stream));
            }

            using (var streamWriter = new StreamWriter(stream, _streamEncoding, _streamBufferSize, true))
            {
                try
                {
                    using (var jsonWriter = new JsonTextWriter(streamWriter))
                    {
                        jsonWriter.AutoCompleteOnClose = false;
                        jsonWriter.ArrayPool = _jsonBufferPool;

                        SerializeResponse(jsonWriter, response);
                    }
                }
                catch (JsonException e)
                {
                    throw new JsonRpcException(JsonRpcErrorCodes.InvalidOperation, Strings.GetString("core.serialize.json_issue"), response.Id, e);
                }
            }
        }

        /// <summary>Serializes the specified response to the specified stream as an asynchronous operation.</summary>
        /// <param name="response">The response to serialize.</param>
        /// <param name="stream">The stream for a JSON string.</param>
        /// <param name="cancellationToken">The cancellation token for canceling the operation.</param>
        /// <returns>A task that represents the asynchronous operation. The task result is a JSON string representation of the specified response.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="response" /> or <paramref name="stream" /> is <see langword="null" />.</exception>
        /// <exception cref="JsonRpcException">An error occurred during response serialization.</exception>
        /// <exception cref="OperationCanceledException">The operation was canceled.</exception>
        public Task SerializeResponseAsync(JsonRpcResponse response, Stream stream, CancellationToken cancellationToken = default)
        {
            if (response == null)
            {
                throw new ArgumentNullException(nameof(response));
            }
            if (stream == null)
            {
                throw new ArgumentNullException(nameof(stream));
            }

            cancellationToken.ThrowIfCancellationRequested();

            using (var streamWriter = new StreamWriter(stream, _streamEncoding, _streamBufferSize, true))
            {
                try
                {
                    using (var jsonWriter = new JsonTextWriter(streamWriter))
                    {
                        jsonWriter.AutoCompleteOnClose = false;
                        jsonWriter.ArrayPool = _jsonBufferPool;

                        SerializeResponse(jsonWriter, response);
                    }
                }
                catch (JsonException e)
                {
                    throw new JsonRpcException(JsonRpcErrorCodes.InvalidOperation, Strings.GetString("core.serialize.json_issue"), response.Id, e);
                }
            }

            return Task.FromResult<object>(null);
        }

        /// <summary>Serializes the specified collection of responses to a JSON string.</summary>
        /// <param name="responses">The collection of responses to serialize.</param>
        /// <returns>A JSON string representation of the specified collection of responses.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="responses" /> is <see langword="null" />.</exception>
        /// <exception cref="JsonRpcException">An error occurred during responses serialization.</exception>
        public string SerializeResponses(IReadOnlyList<JsonRpcResponse> responses)
        {
            if (responses == null)
            {
                throw new ArgumentNullException(nameof(responses));
            }

            using (var stringWriter = new StringWriter(new StringBuilder(_minimumMessageSize * responses.Count), CultureInfo.InvariantCulture))
            {
                try
                {
                    using (var jsonWriter = new JsonTextWriter(stringWriter))
                    {
                        jsonWriter.AutoCompleteOnClose = false;
                        jsonWriter.ArrayPool = _jsonBufferPool;

                        SerializeResponses(jsonWriter, responses);
                    }
                }
                catch (JsonException e)
                {
                    throw new JsonRpcException(JsonRpcErrorCodes.InvalidOperation, Strings.GetString("core.serialize.json_issue"), default, e);
                }

                return stringWriter.ToString();
            }
        }

        /// <summary>Serializes the specified collection of responses to the specified stream.</summary>
        /// <param name="responses">The collection of responses to serialize.</param>
        /// <param name="stream">The stream for a JSON string.</param>
        /// <exception cref="ArgumentNullException"><paramref name="responses" /> or <paramref name="stream" /> is <see langword="null" />.</exception>
        /// <exception cref="JsonRpcException">An error occurred during responses serialization.</exception>
        public void SerializeResponses(IReadOnlyList<JsonRpcResponse> responses, Stream stream)
        {
            if (responses == null)
            {
                throw new ArgumentNullException(nameof(responses));
            }
            if (stream == null)
            {
                throw new ArgumentNullException(nameof(stream));
            }

            using (var streamWriter = new StreamWriter(stream, _streamEncoding, _streamBufferSize, true))
            {
                try
                {
                    using (var jsonWriter = new JsonTextWriter(streamWriter))
                    {
                        jsonWriter.AutoCompleteOnClose = false;
                        jsonWriter.ArrayPool = _jsonBufferPool;

                        SerializeResponses(jsonWriter, responses);
                    }
                }
                catch (JsonException e)
                {
                    throw new JsonRpcException(JsonRpcErrorCodes.InvalidOperation, Strings.GetString("core.serialize.json_issue"), default, e);
                }
            }
        }

        /// <summary>Serializes the specified collection of responses to the specified stream as an asynchronous operation.</summary>
        /// <param name="responses">The collection of responses to serialize.</param>
        /// <param name="stream">The stream for a JSON string.</param>
        /// <param name="cancellationToken">The cancellation token for canceling the operation.</param>
        /// <returns>A task that represents the asynchronous operation. The task result is a JSON string representation of the specified collection of responses.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="responses" /> or <paramref name="stream" /> is <see langword="null" />.</exception>
        /// <exception cref="JsonRpcException">An error occurred during responses serialization.</exception>
        /// <exception cref="OperationCanceledException">The operation was canceled.</exception>
        public Task SerializeResponsesAsync(IReadOnlyList<JsonRpcResponse> responses, Stream stream, CancellationToken cancellationToken = default)
        {
            if (responses == null)
            {
                throw new ArgumentNullException(nameof(responses));
            }
            if (stream == null)
            {
                throw new ArgumentNullException(nameof(stream));
            }

            cancellationToken.ThrowIfCancellationRequested();

            using (var streamWriter = new StreamWriter(stream, _streamEncoding, _streamBufferSize, true))
            {
                try
                {
                    using (var jsonWriter = new JsonTextWriter(streamWriter))
                    {
                        jsonWriter.AutoCompleteOnClose = false;
                        jsonWriter.ArrayPool = _jsonBufferPool;

                        SerializeResponses(jsonWriter, responses, cancellationToken);
                    }
                }
                catch (JsonException e)
                {
                    throw new JsonRpcException(JsonRpcErrorCodes.InvalidOperation, Strings.GetString("core.serialize.json_issue"), default, e);
                }
            }

            return Task.FromResult<object>(null);
        }

        private JsonRpcResponseContract GetResponseContract(in JsonRpcId identifier)
        {
            if (!_dynamicResponseBindings.TryGetValue(identifier, out var contract))
            {
                if (_staticResponseBindings.TryGetValue(identifier, out var method) && (method != null))
                {
                    _responseContracts.TryGetValue(method, out contract);
                }
            }

            if (contract == null)
            {
                throw new JsonRpcException(JsonRpcErrorCodes.InvalidOperation, Strings.GetString("core.deserialize.response.method.contract.undefined"), identifier);
            }

            return contract;
        }

        /// <summary>Clears response bindings of the current instance.</summary>
        public void Dispose()
        {
            _dynamicResponseBindings.Clear();
            _staticResponseBindings.Clear();
        }

        /// <summary>Gets the request contracts.</summary>
        public IDictionary<string, JsonRpcRequestContract> RequestContracts
        {
            get => _requestContracts;
        }

        /// <summary>Gets the response contracts.</summary>
        public IDictionary<string, JsonRpcResponseContract> ResponseContracts
        {
            get => _responseContracts;
        }

        /// <summary>Gets the dynamic response bindings.</summary>
        public IDictionary<JsonRpcId, JsonRpcResponseContract> DynamicResponseBindings
        {
            get => _dynamicResponseBindings;
        }

        /// <summary>Gets the static response bindings.</summary>
        public IDictionary<JsonRpcId, string> StaticResponseBindings
        {
            get => _staticResponseBindings;
        }

        /// <summary>Gets or sets a type of error data for deserializing an unsuccessful response with empty identifier.</summary>
        public Type DefaultErrorDataType
        {
            get => _defaultErrorDataType;
            set => _defaultErrorDataType = value;
        }

        /// <summary>Gets or sets the protocol compatibility level.</summary>
        public JsonRpcCompatibilityLevel CompatibilityLevel
        {
            get => _compatibilityLevel;
            set => _compatibilityLevel = value;
        }
    }
}