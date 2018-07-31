// © Alexander Kozlenko. Licensed under the MIT License.

using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace System.Data.JsonRpc
{
    /// <summary>Serializes and deserializes JSON-RPC messages into and from the JSON format.</summary>
    public sealed partial class JsonRpcSerializer
    {
        private const int _messageBufferSize = 64;
        private const int _streamBufferSize = 1024;

        private static readonly Encoding _streamEncoding = new UTF8Encoding(false);
        private static readonly IArrayPool<char> _jsonBufferPool = new JsonBufferPool();

        /// <summary>Initializes a new instance of the <see cref="JsonRpcSerializer" /> class.</summary>
        /// <param name="contractResolver">The JSON-RPC message contract resolver instance.</param>
        /// <param name="jsonSerializer">The JSON serializer instance.</param>
        /// <param name="compatibilityLevel">The JSON-RPC protocol compatibility level.</param>
        public JsonRpcSerializer(IJsonRpcContractResolver contractResolver = null, JsonSerializer jsonSerializer = null, JsonRpcCompatibilityLevel compatibilityLevel = JsonRpcCompatibilityLevel.Level2)
        {
            _contractResolver = contractResolver;
            _jsonSerializer = jsonSerializer ?? JsonSerializer.CreateDefault();
            _compatibilityLevel = compatibilityLevel;
        }

        /// <summary>Deserializes the specified JSON string to request data.</summary>
        /// <param name="json">The JSON string to deserialize.</param>
        /// <returns>RPC information about requests.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="json" /> is <see langword="null" />.</exception>
        /// <exception cref="JsonException">An error occurred during JSON deserialization.</exception>
        /// <exception cref="JsonRpcException">An error occurred during JSON-RPC deserialization.</exception>
        public JsonRpcData<JsonRpcRequest> DeserializeRequestData(string json)
        {
            if (json == null)
            {
                throw new ArgumentNullException(nameof(json));
            }

            using (var stringReader = new StringReader(json))
            {
                using (var jsonReader = new JsonTextReader(stringReader))
                {
                    jsonReader.ArrayPool = _jsonBufferPool;

                    return DeserializeRequestData(jsonReader);
                }
            }
        }

        /// <summary>Deserializes the specified stream with a JSON string to request data.</summary>
        /// <param name="stream">The stream with a JSON string to deserialize.</param>
        /// <returns>RPC information about requests.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="stream" /> is <see langword="null" />.</exception>
        /// <exception cref="JsonException">An error occurred during JSON deserialization.</exception>
        /// <exception cref="JsonRpcException">An error occurred during JSON-RPC deserialization.</exception>
        public JsonRpcData<JsonRpcRequest> DeserializeRequestData(Stream stream)
        {
            if (stream == null)
            {
                throw new ArgumentNullException(nameof(stream));
            }

            using (var streamReader = new StreamReader(stream, _streamEncoding, false, _streamBufferSize, true))
            {
                using (var jsonReader = new JsonTextReader(streamReader))
                {
                    jsonReader.ArrayPool = _jsonBufferPool;

                    return DeserializeRequestData(jsonReader);
                }
            }
        }

        /// <summary>Deserializes the specified stream with a JSON string to request data as an asynchronous operation.</summary>
        /// <param name="stream">The stream with a JSON string to deserialize.</param>
        /// <param name="cancellationToken">The cancellation token for canceling the operation.</param>
        /// <returns>A task that represents the asynchronous operation. The task result is RPC information about requests.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="stream" /> is <see langword="null" />.</exception>
        /// <exception cref="JsonException">An error occurred during JSON deserialization.</exception>
        /// <exception cref="JsonRpcException">An error occurred during JSON-RPC deserialization.</exception>
        /// <exception cref="OperationCanceledException">The operation was canceled.</exception>
        public Task<JsonRpcData<JsonRpcRequest>> DeserializeRequestDataAsync(Stream stream, CancellationToken cancellationToken = default)
        {
            if (stream == null)
            {
                throw new ArgumentNullException(nameof(stream));
            }

            using (var streamReader = new StreamReader(stream, _streamEncoding, false, _streamBufferSize, true))
            {
                using (var jsonReader = new JsonTextReader(streamReader))
                {
                    jsonReader.ArrayPool = _jsonBufferPool;

                    return Task.FromResult(DeserializeRequestData(jsonReader, cancellationToken));
                }
            }
        }

        /// <summary>Deserializes the specified JSON string to response data.</summary>
        /// <param name="json">The JSON string to deserialize.</param>
        /// <returns>RPC information about responses.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="json" /> is <see langword="null" />.</exception>
        /// <exception cref="JsonException">An error occurred during JSON deserialization.</exception>
        /// <exception cref="JsonRpcException">An error occurred during JSON-RPC deserialization.</exception>
        public JsonRpcData<JsonRpcResponse> DeserializeResponseData(string json)
        {
            if (json == null)
            {
                throw new ArgumentNullException(nameof(json));
            }

            using (var stringReader = new StringReader(json))
            {
                using (var jsonReader = new JsonTextReader(stringReader))
                {
                    jsonReader.ArrayPool = _jsonBufferPool;

                    return DeserializeResponseData(jsonReader);
                }
            }
        }

        /// <summary>Deserializes the specified stream with a JSON string to response data.</summary>
        /// <param name="stream">The stream with a JSON string to deserialize.</param>
        /// <returns>RPC information about responses.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="stream" /> is <see langword="null" />.</exception>
        /// <exception cref="JsonException">An error occurred during JSON deserialization.</exception>
        /// <exception cref="JsonRpcException">An error occurred during JSON-RPC deserialization.</exception>
        public JsonRpcData<JsonRpcResponse> DeserializeResponseData(Stream stream)
        {
            if (stream == null)
            {
                throw new ArgumentNullException(nameof(stream));
            }

            using (var streamReader = new StreamReader(stream, _streamEncoding, false, _streamBufferSize, true))
            {
                using (var jsonReader = new JsonTextReader(streamReader))
                {
                    jsonReader.ArrayPool = _jsonBufferPool;

                    return DeserializeResponseData(jsonReader);
                }
            }
        }

        /// <summary>Deserializes the specified stream with a JSON string to response data as an asynchronous operation.</summary>
        /// <param name="stream">The stream with a JSON string to deserialize.</param>
        /// <param name="cancellationToken">The cancellation token for canceling the operation.</param>
        /// <returns>A task that represents the asynchronous operation. The task result is RPC information about responses.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="stream" /> is <see langword="null" />.</exception>
        /// <exception cref="JsonException">An error occurred during JSON deserialization.</exception>
        /// <exception cref="JsonRpcException">An error occurred during JSON-RPC deserialization.</exception>
        /// <exception cref="OperationCanceledException">The operation was canceled.</exception>
        public Task<JsonRpcData<JsonRpcResponse>> DeserializeResponseDataAsync(Stream stream, CancellationToken cancellationToken = default)
        {
            if (stream == null)
            {
                throw new ArgumentNullException(nameof(stream));
            }

            using (var streamReader = new StreamReader(stream, _streamEncoding, false, _streamBufferSize, true))
            {
                using (var jsonReader = new JsonTextReader(streamReader))
                {
                    jsonReader.ArrayPool = _jsonBufferPool;

                    return Task.FromResult(DeserializeResponseData(jsonReader, cancellationToken));
                }
            }
        }

        /// <summary>Serializes the specified request to a JSON string.</summary>
        /// <param name="request">The request to serialize.</param>
        /// <returns>A JSON string representation of the specified request.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="request" /> is <see langword="null" />.</exception>
        /// <exception cref="JsonException">An error occurred during JSON serialization.</exception>
        /// <exception cref="JsonRpcException">An error occurred during JSON-RPC serialization.</exception>
        public string SerializeRequest(JsonRpcRequest request)
        {
            if (request == null)
            {
                throw new ArgumentNullException(nameof(request));
            }

            using (var stringWriter = new StringWriter(new StringBuilder(_messageBufferSize), CultureInfo.InvariantCulture))
            {
                using (var jsonWriter = new JsonTextWriter(stringWriter))
                {
                    jsonWriter.AutoCompleteOnClose = false;
                    jsonWriter.ArrayPool = _jsonBufferPool;

                    SerializeRequest(jsonWriter, request);
                }

                return stringWriter.ToString();
            }
        }

        /// <summary>Serializes the specified request to the specified stream.</summary>
        /// <param name="request">The request to serialize.</param>
        /// <param name="stream">The stream for a JSON string.</param>
        /// <exception cref="ArgumentNullException"><paramref name="request" /> or <paramref name="stream" /> is <see langword="null" />.</exception>
        /// <exception cref="JsonException">An error occurred during JSON serialization.</exception>
        /// <exception cref="JsonRpcException">An error occurred during JSON-RPC serialization.</exception>
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
                using (var jsonWriter = new JsonTextWriter(streamWriter))
                {
                    jsonWriter.AutoCompleteOnClose = false;
                    jsonWriter.ArrayPool = _jsonBufferPool;

                    SerializeRequest(jsonWriter, request);
                }
            }
        }

        /// <summary>Serializes the specified request to the specified stream as an asynchronous operation.</summary>
        /// <param name="request">The request to serialize.</param>
        /// <param name="stream">The stream for a JSON string.</param>
        /// <param name="cancellationToken">The cancellation token for canceling the operation.</param>
        /// <returns>A task that represents the asynchronous operation. The task result is a JSON string representation of the specified request.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="request" /> or <paramref name="stream" /> is <see langword="null" />.</exception>
        /// <exception cref="JsonException">An error occurred during JSON serialization.</exception>
        /// <exception cref="JsonRpcException">An error occurred during JSON-RPC serialization.</exception>
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
                using (var jsonWriter = new JsonTextWriter(streamWriter))
                {
                    jsonWriter.AutoCompleteOnClose = false;
                    jsonWriter.ArrayPool = _jsonBufferPool;

                    SerializeRequest(jsonWriter, request);
                }
            }

            return Task.FromResult<object>(null);
        }

        /// <summary>Serializes the specified collection of requests to a JSON string.</summary>
        /// <param name="requests">The collection of requests to serialize.</param>
        /// <returns>A JSON string representation of the specified collection of requests.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="requests" /> is <see langword="null" />.</exception>
        /// <exception cref="JsonException">An error occurred during JSON serialization.</exception>
        /// <exception cref="JsonRpcException">An error occurred during JSON-RPC serialization.</exception>
        public string SerializeRequests(IReadOnlyList<JsonRpcRequest> requests)
        {
            if (requests == null)
            {
                throw new ArgumentNullException(nameof(requests));
            }

            using (var stringWriter = new StringWriter(new StringBuilder(_messageBufferSize * requests.Count), CultureInfo.InvariantCulture))
            {
                using (var jsonWriter = new JsonTextWriter(stringWriter))
                {
                    jsonWriter.AutoCompleteOnClose = false;
                    jsonWriter.ArrayPool = _jsonBufferPool;

                    SerializeRequests(jsonWriter, requests);
                }

                return stringWriter.ToString();
            }
        }

        /// <summary>Serializes the specified collection of requests to the specified stream.</summary>
        /// <param name="requests">The collection of requests to serialize.</param>
        /// <param name="stream">The stream for a JSON string.</param>
        /// <exception cref="ArgumentNullException"><paramref name="requests" /> or <paramref name="stream" /> is <see langword="null" />.</exception>
        /// <exception cref="JsonException">An error occurred during JSON serialization.</exception>
        /// <exception cref="JsonRpcException">An error occurred during JSON-RPC serialization.</exception>
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
                using (var jsonWriter = new JsonTextWriter(streamWriter))
                {
                    jsonWriter.AutoCompleteOnClose = false;
                    jsonWriter.ArrayPool = _jsonBufferPool;

                    SerializeRequests(jsonWriter, requests);
                }
            }
        }

        /// <summary>Serializes the specified collection of requests to the specified stream as an asynchronous operation.</summary>
        /// <param name="requests">The collection of requests to serialize.</param>
        /// <param name="stream">The stream for a JSON string.</param>
        /// <param name="cancellationToken">The cancellation token for canceling the operation.</param>
        /// <returns>A task that represents the asynchronous operation. The task result is a JSON string representation of the specified collection of requests.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="requests" /> or <paramref name="stream" /> is <see langword="null" />.</exception>
        /// <exception cref="JsonException">An error occurred during JSON serialization.</exception>
        /// <exception cref="JsonRpcException">An error occurred during JSON-RPC serialization.</exception>
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
                using (var jsonWriter = new JsonTextWriter(streamWriter))
                {
                    jsonWriter.AutoCompleteOnClose = false;
                    jsonWriter.ArrayPool = _jsonBufferPool;

                    SerializeRequests(jsonWriter, requests, cancellationToken);
                }
            }

            return Task.FromResult<object>(null);
        }

        /// <summary>Serializes the specified response to a JSON string.</summary>
        /// <param name="response">The response to serialize.</param>
        /// <returns>A JSON string representation of the specified response.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="response" /> is <see langword="null" />.</exception>
        /// <exception cref="JsonException">An error occurred during JSON serialization.</exception>
        /// <exception cref="JsonRpcException">An error occurred during JSON-RPC serialization.</exception>
        public string SerializeResponse(JsonRpcResponse response)
        {
            if (response == null)
            {
                throw new ArgumentNullException(nameof(response));
            }

            using (var stringWriter = new StringWriter(new StringBuilder(_messageBufferSize), CultureInfo.InvariantCulture))
            {
                using (var jsonWriter = new JsonTextWriter(stringWriter))
                {
                    jsonWriter.AutoCompleteOnClose = false;
                    jsonWriter.ArrayPool = _jsonBufferPool;

                    SerializeResponse(jsonWriter, response);
                }

                return stringWriter.ToString();
            }
        }

        /// <summary>Serializes the specified response to the specified stream.</summary>
        /// <param name="response">The response to serialize.</param>
        /// <param name="stream">The stream for a JSON string.</param>
        /// <exception cref="ArgumentNullException"><paramref name="response" /> or <paramref name="stream" /> is <see langword="null" />.</exception>
        /// <exception cref="JsonException">An error occurred during JSON serialization.</exception>
        /// <exception cref="JsonRpcException">An error occurred during JSON-RPC serialization.</exception>
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
                using (var jsonWriter = new JsonTextWriter(streamWriter))
                {
                    jsonWriter.AutoCompleteOnClose = false;
                    jsonWriter.ArrayPool = _jsonBufferPool;

                    SerializeResponse(jsonWriter, response);
                }
            }
        }

        /// <summary>Serializes the specified response to the specified stream as an asynchronous operation.</summary>
        /// <param name="response">The response to serialize.</param>
        /// <param name="stream">The stream for a JSON string.</param>
        /// <param name="cancellationToken">The cancellation token for canceling the operation.</param>
        /// <returns>A task that represents the asynchronous operation. The task result is a JSON string representation of the specified response.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="response" /> or <paramref name="stream" /> is <see langword="null" />.</exception>
        /// <exception cref="JsonException">An error occurred during JSON serialization.</exception>
        /// <exception cref="JsonRpcException">An error occurred during JSON-RPC serialization.</exception>
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
                using (var jsonWriter = new JsonTextWriter(streamWriter))
                {
                    jsonWriter.AutoCompleteOnClose = false;
                    jsonWriter.ArrayPool = _jsonBufferPool;

                    SerializeResponse(jsonWriter, response);
                }
            }

            return Task.FromResult<object>(null);
        }

        /// <summary>Serializes the specified collection of responses to a JSON string.</summary>
        /// <param name="responses">The collection of responses to serialize.</param>
        /// <returns>A JSON string representation of the specified collection of responses.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="responses" /> is <see langword="null" />.</exception>
        /// <exception cref="JsonException">An error occurred during JSON serialization.</exception>
        /// <exception cref="JsonRpcException">An error occurred during JSON-RPC serialization.</exception>
        public string SerializeResponses(IReadOnlyList<JsonRpcResponse> responses)
        {
            if (responses == null)
            {
                throw new ArgumentNullException(nameof(responses));
            }

            using (var stringWriter = new StringWriter(new StringBuilder(_messageBufferSize * responses.Count), CultureInfo.InvariantCulture))
            {
                using (var jsonWriter = new JsonTextWriter(stringWriter))
                {
                    jsonWriter.AutoCompleteOnClose = false;
                    jsonWriter.ArrayPool = _jsonBufferPool;

                    SerializeResponses(jsonWriter, responses);
                }

                return stringWriter.ToString();
            }
        }

        /// <summary>Serializes the specified collection of responses to the specified stream.</summary>
        /// <param name="responses">The collection of responses to serialize.</param>
        /// <param name="stream">The stream for a JSON string.</param>
        /// <exception cref="ArgumentNullException"><paramref name="responses" /> or <paramref name="stream" /> is <see langword="null" />.</exception>
        /// <exception cref="JsonException">An error occurred during JSON serialization.</exception>
        /// <exception cref="JsonRpcException">An error occurred during JSON-RPC serialization.</exception>
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
                using (var jsonWriter = new JsonTextWriter(streamWriter))
                {
                    jsonWriter.AutoCompleteOnClose = false;
                    jsonWriter.ArrayPool = _jsonBufferPool;

                    SerializeResponses(jsonWriter, responses);
                }
            }
        }

        /// <summary>Serializes the specified collection of responses to the specified stream as an asynchronous operation.</summary>
        /// <param name="responses">The collection of responses to serialize.</param>
        /// <param name="stream">The stream for a JSON string.</param>
        /// <param name="cancellationToken">The cancellation token for canceling the operation.</param>
        /// <returns>A task that represents the asynchronous operation. The task result is a JSON string representation of the specified collection of responses.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="responses" /> or <paramref name="stream" /> is <see langword="null" />.</exception>
        /// <exception cref="JsonException">An error occurred during JSON serialization.</exception>
        /// <exception cref="JsonRpcException">An error occurred during JSON-RPC serialization.</exception>
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
                using (var jsonWriter = new JsonTextWriter(streamWriter))
                {
                    jsonWriter.AutoCompleteOnClose = false;
                    jsonWriter.ArrayPool = _jsonBufferPool;

                    SerializeResponses(jsonWriter, responses, cancellationToken);
                }
            }

            return Task.FromResult<object>(null);
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
    }
}