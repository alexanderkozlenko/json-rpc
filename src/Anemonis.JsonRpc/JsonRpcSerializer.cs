// © Alexander Kozlenko. Licensed under the MIT License.

using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using Newtonsoft.Json;

namespace Anemonis.JsonRpc
{
    /// <summary>Serializes and deserializes JSON-RPC messages into and from the JSON format.</summary>
    public sealed partial class JsonRpcSerializer
    {
        private const int _messageBufferSize = 64;
        private const int _defaultStreamBufferSize = 1024;

        private static readonly Encoding _utf8Encoding = new UTF8Encoding(false, true);

        /// <summary>Initializes a new instance of the <see cref="JsonRpcSerializer" /> class.</summary>
        /// <param name="contractResolver">The JSON-RPC message contract resolver instance.</param>
        /// <param name="jsonSerializer">The JSON serializer instance.</param>
        /// <param name="compatibilityLevel">The JSON-RPC protocol compatibility level.</param>
        public JsonRpcSerializer(IJsonRpcContractResolver contractResolver = null, JsonSerializer jsonSerializer = null, JsonRpcCompatibilityLevel compatibilityLevel = default)
        {
            _contractResolver = contractResolver;
            _jsonSerializer = jsonSerializer ?? JsonSerializer.CreateDefault(_jsonSerializerSettings);
            _compatibilityLevel = compatibilityLevel;
        }

        private JsonRpcData<JsonRpcRequest> DeserializeRequestData(TextReader textReader)
        {
            using var jsonReader = new JsonTextReader(textReader);

            jsonReader.DateParseHandling = _jsonSerializer.DateParseHandling;
            jsonReader.ArrayPool = _jsonBufferPool;

            return DeserializeRequestData(jsonReader, default);
        }

        private ValueTask<JsonRpcData<JsonRpcRequest>> DeserializeRequestDataAsync(TextReader textReader, CancellationToken cancellationToken)
        {
            using var jsonReader = new JsonTextReader(textReader);

            jsonReader.DateParseHandling = _jsonSerializer.DateParseHandling;
            jsonReader.ArrayPool = _jsonBufferPool;

            return new ValueTask<JsonRpcData<JsonRpcRequest>>(DeserializeRequestData(jsonReader, cancellationToken));
        }

        private JsonRpcData<JsonRpcResponse> DeserializeResponseData(TextReader textReader)
        {
            using var jsonReader = new JsonTextReader(textReader);

            jsonReader.DateParseHandling = _jsonSerializer.DateParseHandling;
            jsonReader.ArrayPool = _jsonBufferPool;

            return DeserializeResponseData(jsonReader, default);
        }

        private ValueTask<JsonRpcData<JsonRpcResponse>> DeserializeResponseDataAsync(TextReader textReader, CancellationToken cancellationToken)
        {
            using var jsonReader = new JsonTextReader(textReader);

            jsonReader.DateParseHandling = _jsonSerializer.DateParseHandling;
            jsonReader.ArrayPool = _jsonBufferPool;

            return new ValueTask<JsonRpcData<JsonRpcResponse>>(DeserializeResponseData(jsonReader, cancellationToken));
        }

        private void SerializeRequest(JsonRpcRequest request, TextWriter textWriter)
        {
            using var jsonWriter = new JsonTextWriter(textWriter);

            jsonWriter.AutoCompleteOnClose = false;
            jsonWriter.ArrayPool = _jsonBufferPool;

            SerializeRequest(jsonWriter, request);
        }

        private ValueTask SerializeRequestAsync(JsonRpcRequest request, TextWriter textWriter)
        {
            using var jsonWriter = new JsonTextWriter(textWriter);

            jsonWriter.AutoCompleteOnClose = false;
            jsonWriter.ArrayPool = _jsonBufferPool;

            SerializeRequest(jsonWriter, request);

            return default;
        }

        private void SerializeRequests(IReadOnlyList<JsonRpcRequest> requests, TextWriter textWriter)
        {
            using var jsonWriter = new JsonTextWriter(textWriter);

            jsonWriter.AutoCompleteOnClose = false;
            jsonWriter.ArrayPool = _jsonBufferPool;

            SerializeRequests(jsonWriter, requests, default);
        }

        private ValueTask SerializeRequestsAsync(IReadOnlyList<JsonRpcRequest> requests, TextWriter textWriter, CancellationToken cancellationToken)
        {
            using var jsonWriter = new JsonTextWriter(textWriter);

            jsonWriter.AutoCompleteOnClose = false;
            jsonWriter.ArrayPool = _jsonBufferPool;

            SerializeRequests(jsonWriter, requests, cancellationToken);

            return default;
        }

        private void SerializeResponse(JsonRpcResponse response, TextWriter textWriter)
        {
            using var jsonWriter = new JsonTextWriter(textWriter);

            jsonWriter.AutoCompleteOnClose = false;
            jsonWriter.ArrayPool = _jsonBufferPool;

            SerializeResponse(jsonWriter, response);
        }

        private ValueTask SerializeResponseAsync(JsonRpcResponse response, TextWriter textWriter)
        {
            using var jsonWriter = new JsonTextWriter(textWriter);

            jsonWriter.AutoCompleteOnClose = false;
            jsonWriter.ArrayPool = _jsonBufferPool;

            SerializeResponse(jsonWriter, response);

            return default;
        }

        private void SerializeResponses(IReadOnlyList<JsonRpcResponse> responses, TextWriter textWriter)
        {
            using var jsonWriter = new JsonTextWriter(textWriter);

            jsonWriter.AutoCompleteOnClose = false;
            jsonWriter.ArrayPool = _jsonBufferPool;

            SerializeResponses(jsonWriter, responses, default);
        }

        private ValueTask SerializeResponsesAsync(IReadOnlyList<JsonRpcResponse> responses, TextWriter textWriter, CancellationToken cancellationToken)
        {
            using var jsonWriter = new JsonTextWriter(textWriter);

            jsonWriter.AutoCompleteOnClose = false;
            jsonWriter.ArrayPool = _jsonBufferPool;

            SerializeResponses(jsonWriter, responses, cancellationToken);

            return default;
        }

        /// <summary>Deserializes the specified JSON string to JSON-RPC request data.</summary>
        /// <param name="json">The JSON string to deserialize.</param>
        /// <returns>A JSON-RPC request data deserialization result.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="json" /> is <see langword="null" />.</exception>
        /// <exception cref="JsonException">An error occurred during JSON deserialization.</exception>
        /// <exception cref="JsonRpcSerializationException">An error occurred during JSON-RPC deserialization.</exception>
        public JsonRpcData<JsonRpcRequest> DeserializeRequestData(string json)
        {
            if (json == null)
            {
                throw new ArgumentNullException(nameof(json));
            }

            using var stringReader = new StringReader(json);

            return DeserializeRequestData(stringReader);
        }

        /// <summary>Deserializes a UTF-8 encoded JSON string from the specified stream to JSON-RPC request data.</summary>
        /// <param name="jsonStream">The stream to deserialize from.</param>
        /// <returns>A JSON-RPC request data deserialization result.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="jsonStream" /> is <see langword="null" />.</exception>
        /// <exception cref="JsonException">An error occurred during JSON deserialization.</exception>
        /// <exception cref="JsonRpcSerializationException">An error occurred during JSON-RPC deserialization.</exception>
        public JsonRpcData<JsonRpcRequest> DeserializeRequestData(Stream jsonStream)
        {
            if (jsonStream == null)
            {
                throw new ArgumentNullException(nameof(jsonStream));
            }

            using var streamReader = new StreamReader(jsonStream, _utf8Encoding, false, _defaultStreamBufferSize, true);

            return DeserializeRequestData(streamReader);
        }

        /// <summary>Deserializes a UTF-8 encoded JSON string from the specified stream to JSON-RPC request data.</summary>
        /// <param name="jsonStream">The stream to deserialize from.</param>
        /// <param name="encoding">The character encoding to use.</param>
        /// <returns>A JSON-RPC request data deserialization result.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="jsonStream" /> or <paramref name="encoding" /> is <see langword="null" />.</exception>
        /// <exception cref="JsonException">An error occurred during JSON deserialization.</exception>
        /// <exception cref="JsonRpcSerializationException">An error occurred during JSON-RPC deserialization.</exception>
        public JsonRpcData<JsonRpcRequest> DeserializeRequestData(Stream jsonStream, Encoding encoding)
        {
            if (jsonStream == null)
            {
                throw new ArgumentNullException(nameof(jsonStream));
            }
            if (encoding == null)
            {
                throw new ArgumentNullException(nameof(encoding));
            }

            using var streamReader = new StreamReader(jsonStream, encoding, false, _defaultStreamBufferSize, true);

            return DeserializeRequestData(streamReader);
        }

        /// <summary>Deserializes the specified JSON string to JSON-RPC request data as an asynchronous operation.</summary>
        /// <param name="json">The JSON string to deserialize.</param>
        /// <param name="cancellationToken">The cancellation token for canceling the operation.</param>
        /// <returns>A task that represents the asynchronous operation. The task result is a JSON-RPC request data deserialization result.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="json" /> is <see langword="null" />.</exception>
        /// <exception cref="JsonException">An error occurred during JSON deserialization.</exception>
        /// <exception cref="JsonRpcSerializationException">An error occurred during JSON-RPC deserialization.</exception>
        /// <exception cref="OperationCanceledException">The operation was canceled.</exception>
        public ValueTask<JsonRpcData<JsonRpcRequest>> DeserializeRequestDataAsync(string json, CancellationToken cancellationToken = default)
        {
            if (json == null)
            {
                throw new ArgumentNullException(nameof(json));
            }

            cancellationToken.ThrowIfCancellationRequested();

            using var stringReader = new StringReader(json);

            return DeserializeRequestDataAsync(stringReader, cancellationToken);
        }

        /// <summary>Deserializes a UTF-8 encoded JSON string from the specified stream to JSON-RPC request data as an asynchronous operation.</summary>
        /// <param name="jsonStream">The stream to deserialize from.</param>
        /// <param name="cancellationToken">The cancellation token for canceling the operation.</param>
        /// <returns>A task that represents the asynchronous operation. The task result is a JSON-RPC request data deserialization result.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="jsonStream" /> is <see langword="null" />.</exception>
        /// <exception cref="JsonException">An error occurred during JSON deserialization.</exception>
        /// <exception cref="JsonRpcSerializationException">An error occurred during JSON-RPC deserialization.</exception>
        /// <exception cref="OperationCanceledException">The operation was canceled.</exception>
        public ValueTask<JsonRpcData<JsonRpcRequest>> DeserializeRequestDataAsync(Stream jsonStream, CancellationToken cancellationToken = default)
        {
            if (jsonStream == null)
            {
                throw new ArgumentNullException(nameof(jsonStream));
            }

            cancellationToken.ThrowIfCancellationRequested();

            using var streamReader = new StreamReader(jsonStream, _utf8Encoding, false, _defaultStreamBufferSize, true);

            return DeserializeRequestDataAsync(streamReader, cancellationToken);
        }

        /// <summary>Deserializes a UTF-8 encoded JSON string from the specified stream to JSON-RPC request data as an asynchronous operation.</summary>
        /// <param name="jsonStream">The stream to deserialize from.</param>
        /// <param name="encoding">The character encoding to use.</param>
        /// <param name="cancellationToken">The cancellation token for canceling the operation.</param>
        /// <returns>A task that represents the asynchronous operation. The task result is a JSON-RPC request data deserialization result.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="jsonStream" /> or <paramref name="encoding" /> is <see langword="null" />.</exception>
        /// <exception cref="JsonException">An error occurred during JSON deserialization.</exception>
        /// <exception cref="JsonRpcSerializationException">An error occurred during JSON-RPC deserialization.</exception>
        /// <exception cref="OperationCanceledException">The operation was canceled.</exception>
        public ValueTask<JsonRpcData<JsonRpcRequest>> DeserializeRequestDataAsync(Stream jsonStream, Encoding encoding, CancellationToken cancellationToken = default)
        {
            if (jsonStream == null)
            {
                throw new ArgumentNullException(nameof(jsonStream));
            }
            if (encoding == null)
            {
                throw new ArgumentNullException(nameof(encoding));
            }

            cancellationToken.ThrowIfCancellationRequested();

            using var streamReader = new StreamReader(jsonStream, encoding, false, _defaultStreamBufferSize, true);

            return DeserializeRequestDataAsync(streamReader, cancellationToken);
        }

        /// <summary>Deserializes the specified JSON string to JSON-RPC response data.</summary>
        /// <param name="json">The JSON string to deserialize.</param>
        /// <returns>A JSON-RPC response data deserialization result.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="json" /> is <see langword="null" />.</exception>
        /// <exception cref="JsonException">An error occurred during JSON deserialization.</exception>
        /// <exception cref="JsonRpcSerializationException">An error occurred during JSON-RPC deserialization.</exception>
        public JsonRpcData<JsonRpcResponse> DeserializeResponseData(string json)
        {
            if (json == null)
            {
                throw new ArgumentNullException(nameof(json));
            }

            using var stringReader = new StringReader(json);

            return DeserializeResponseData(stringReader);
        }

        /// <summary>Deserializes a UTF-8 encoded JSON string from the specified stream to JSON-RPC response data.</summary>
        /// <param name="jsonStream">The stream to deserialize from.</param>
        /// <returns>A JSON-RPC response data deserialization result.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="jsonStream" /> is <see langword="null" />.</exception>
        /// <exception cref="JsonException">An error occurred during JSON deserialization.</exception>
        /// <exception cref="JsonRpcSerializationException">An error occurred during JSON-RPC deserialization.</exception>
        public JsonRpcData<JsonRpcResponse> DeserializeResponseData(Stream jsonStream)
        {
            if (jsonStream == null)
            {
                throw new ArgumentNullException(nameof(jsonStream));
            }

            using var streamReader = new StreamReader(jsonStream, _utf8Encoding, false, _defaultStreamBufferSize, true);

            return DeserializeResponseData(streamReader);
        }

        /// <summary>Deserializes a UTF-8 encoded JSON string from the specified stream to JSON-RPC response data.</summary>
        /// <param name="jsonStream">The stream to deserialize from.</param>
        /// <param name="encoding">The character encoding to use.</param>
        /// <returns>A JSON-RPC response data deserialization result.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="jsonStream" /> or <paramref name="encoding" /> is <see langword="null" />.</exception>
        /// <exception cref="JsonException">An error occurred during JSON deserialization.</exception>
        /// <exception cref="JsonRpcSerializationException">An error occurred during JSON-RPC deserialization.</exception>
        public JsonRpcData<JsonRpcResponse> DeserializeResponseData(Stream jsonStream, Encoding encoding)
        {
            if (jsonStream == null)
            {
                throw new ArgumentNullException(nameof(jsonStream));
            }
            if (encoding == null)
            {
                throw new ArgumentNullException(nameof(encoding));
            }

            using var streamReader = new StreamReader(jsonStream, encoding, false, _defaultStreamBufferSize, true);

            return DeserializeResponseData(streamReader);
        }

        /// <summary>Deserializes the specified JSON string to JSON-RPC response data as an asynchronous operation.</summary>
        /// <param name="json">The JSON string to deserialize.</param>
        /// <param name="cancellationToken">The cancellation token for canceling the operation.</param>
        /// <returns>A task that represents the asynchronous operation. The task result is a JSON-RPC response data deserialization result.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="json" /> is <see langword="null" />.</exception>
        /// <exception cref="JsonException">An error occurred during JSON deserialization.</exception>
        /// <exception cref="JsonRpcSerializationException">An error occurred during JSON-RPC deserialization.</exception>
        /// <exception cref="OperationCanceledException">The operation was canceled.</exception>
        public ValueTask<JsonRpcData<JsonRpcResponse>> DeserializeResponseDataAsync(string json, CancellationToken cancellationToken = default)
        {
            if (json == null)
            {
                throw new ArgumentNullException(nameof(json));
            }

            cancellationToken.ThrowIfCancellationRequested();

            using var stringReader = new StringReader(json);

            return DeserializeResponseDataAsync(stringReader, cancellationToken);
        }

        /// <summary>Deserializes a UTF-8 encoded JSON string from the specified stream to JSON-RPC response data as an asynchronous operation.</summary>
        /// <param name="jsonStream">The stream to deserialize from.</param>
        /// <param name="cancellationToken">The cancellation token for canceling the operation.</param>
        /// <returns>A task that represents the asynchronous operation. The task result is a JSON-RPC response data deserialization result.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="jsonStream" /> is <see langword="null" />.</exception>
        /// <exception cref="JsonException">An error occurred during JSON deserialization.</exception>
        /// <exception cref="JsonRpcSerializationException">An error occurred during JSON-RPC deserialization.</exception>
        /// <exception cref="OperationCanceledException">The operation was canceled.</exception>
        public ValueTask<JsonRpcData<JsonRpcResponse>> DeserializeResponseDataAsync(Stream jsonStream, CancellationToken cancellationToken = default)
        {
            if (jsonStream == null)
            {
                throw new ArgumentNullException(nameof(jsonStream));
            }

            cancellationToken.ThrowIfCancellationRequested();

            using var streamReader = new StreamReader(jsonStream, _utf8Encoding, false, _defaultStreamBufferSize, true);

            return DeserializeResponseDataAsync(streamReader, cancellationToken);
        }

        /// <summary>Deserializes a UTF-8 encoded JSON string from the specified stream to JSON-RPC response data as an asynchronous operation.</summary>
        /// <param name="jsonStream">The stream to deserialize from.</param>
        /// <param name="encoding">The character encoding to use.</param>
        /// <param name="cancellationToken">The cancellation token for canceling the operation.</param>
        /// <returns>A task that represents the asynchronous operation. The task result is a JSON-RPC response data deserialization result.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="jsonStream" /> or <paramref name="encoding" /> is <see langword="null" />.</exception>
        /// <exception cref="JsonException">An error occurred during JSON deserialization.</exception>
        /// <exception cref="JsonRpcSerializationException">An error occurred during JSON-RPC deserialization.</exception>
        /// <exception cref="OperationCanceledException">The operation was canceled.</exception>
        public ValueTask<JsonRpcData<JsonRpcResponse>> DeserializeResponseDataAsync(Stream jsonStream, Encoding encoding, CancellationToken cancellationToken = default)
        {
            if (jsonStream == null)
            {
                throw new ArgumentNullException(nameof(jsonStream));
            }
            if (encoding == null)
            {
                throw new ArgumentNullException(nameof(encoding));
            }

            cancellationToken.ThrowIfCancellationRequested();

            using var streamReader = new StreamReader(jsonStream, encoding, false, _defaultStreamBufferSize, true);

            return DeserializeResponseDataAsync(streamReader, cancellationToken);
        }

        /// <summary>Serializes the specified JSON-RPC request to a JSON string.</summary>
        /// <param name="request">The JSON-RPC request to serialize.</param>
        /// <returns>A JSON string representation of the specified request.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="request" /> is <see langword="null" />.</exception>
        /// <exception cref="JsonException">An error occurred during JSON serialization.</exception>
        /// <exception cref="JsonRpcSerializationException">An error occurred during JSON-RPC serialization.</exception>
        public string SerializeRequest(JsonRpcRequest request)
        {
            if (request == null)
            {
                throw new ArgumentNullException(nameof(request));
            }

            using var stringWriter = new StringWriter(new StringBuilder(_messageBufferSize), CultureInfo.InvariantCulture);

            SerializeRequest(request, stringWriter);

            return stringWriter.ToString();
        }

        /// <summary>Serializes the specified JSON-RPC request as a UTF-8 encoded JSON string to the specified stream.</summary>
        /// <param name="request">The JSON-RPC request to serialize.</param>
        /// <param name="jsonStream">The stream to deserialize to.</param>
        /// <exception cref="ArgumentNullException"><paramref name="request" /> or <paramref name="jsonStream" /> is <see langword="null" />.</exception>
        /// <exception cref="JsonException">An error occurred during JSON serialization.</exception>
        /// <exception cref="JsonRpcSerializationException">An error occurred during JSON-RPC serialization.</exception>
        public void SerializeRequest(JsonRpcRequest request, Stream jsonStream)
        {
            if (request == null)
            {
                throw new ArgumentNullException(nameof(request));
            }
            if (jsonStream == null)
            {
                throw new ArgumentNullException(nameof(jsonStream));
            }

            using var streamWriter = new StreamWriter(jsonStream, _utf8Encoding, _defaultStreamBufferSize, true);

            SerializeRequest(request, streamWriter);
        }

        /// <summary>Serializes the specified JSON-RPC request as a UTF-8 encoded JSON string to the specified stream.</summary>
        /// <param name="request">The JSON-RPC request to serialize.</param>
        /// <param name="jsonStream">The stream to deserialize to.</param>
        /// <param name="encoding">The character encoding to use.</param>
        /// <exception cref="ArgumentNullException"><paramref name="request" />, <paramref name="jsonStream" />, or <paramref name="encoding" /> is <see langword="null" />.</exception>
        /// <exception cref="JsonException">An error occurred during JSON serialization.</exception>
        /// <exception cref="JsonRpcSerializationException">An error occurred during JSON-RPC serialization.</exception>
        public void SerializeRequest(JsonRpcRequest request, Stream jsonStream, Encoding encoding)
        {
            if (request == null)
            {
                throw new ArgumentNullException(nameof(request));
            }
            if (jsonStream == null)
            {
                throw new ArgumentNullException(nameof(jsonStream));
            }
            if (encoding == null)
            {
                throw new ArgumentNullException(nameof(encoding));
            }

            using var streamWriter = new StreamWriter(jsonStream, encoding, _defaultStreamBufferSize, true);

            SerializeRequest(request, streamWriter);
        }

        /// <summary>Serializes the specified JSON-RPC request to a JSON string as an asynchronous operation.</summary>
        /// <param name="request">The JSON-RPC request to serialize.</param>
        /// <param name="cancellationToken">The cancellation token for canceling the operation.</param>
        /// <returns>A task that represents the asynchronous operation. The task result is a JSON string representation of the specified request.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="request" /> is <see langword="null" />.</exception>
        /// <exception cref="JsonException">An error occurred during JSON serialization.</exception>
        /// <exception cref="JsonRpcSerializationException">An error occurred during JSON-RPC serialization.</exception>
        /// <exception cref="OperationCanceledException">The operation was canceled.</exception>
        public async ValueTask<string> SerializeRequestAsync(JsonRpcRequest request, CancellationToken cancellationToken = default)
        {
            if (request == null)
            {
                throw new ArgumentNullException(nameof(request));
            }

            cancellationToken.ThrowIfCancellationRequested();

            using var stringWriter = new StringWriter(new StringBuilder(_messageBufferSize), CultureInfo.InvariantCulture);

            await SerializeRequestAsync(request, stringWriter).ConfigureAwait(false);

            return stringWriter.ToString();
        }

        /// <summary>Serializes the specified JSON-RPC request as a UTF-8 encoded JSON string to the specified stream as an asynchronous operation.</summary>
        /// <param name="request">The JSON-RPC request to serialize.</param>
        /// <param name="jsonStream">The stream to deserialize to.</param>
        /// <param name="cancellationToken">The cancellation token for canceling the operation.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="request" /> or <paramref name="jsonStream" /> is <see langword="null" />.</exception>
        /// <exception cref="JsonException">An error occurred during JSON serialization.</exception>
        /// <exception cref="JsonRpcSerializationException">An error occurred during JSON-RPC serialization.</exception>
        /// <exception cref="OperationCanceledException">The operation was canceled.</exception>
        public ValueTask SerializeRequestAsync(JsonRpcRequest request, Stream jsonStream, CancellationToken cancellationToken = default)
        {
            if (request == null)
            {
                throw new ArgumentNullException(nameof(request));
            }
            if (jsonStream == null)
            {
                throw new ArgumentNullException(nameof(jsonStream));
            }

            cancellationToken.ThrowIfCancellationRequested();

            using var streamWriter = new StreamWriter(jsonStream, _utf8Encoding, _defaultStreamBufferSize, true);

            return SerializeRequestAsync(request, streamWriter);
        }

        /// <summary>Serializes the specified JSON-RPC request as a UTF-8 encoded JSON string to the specified stream as an asynchronous operation.</summary>
        /// <param name="request">The JSON-RPC request to serialize.</param>
        /// <param name="jsonStream">The stream to deserialize to.</param>
        /// <param name="encoding">The character encoding to use.</param>
        /// <param name="cancellationToken">The cancellation token for canceling the operation.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="request" />, <paramref name="jsonStream" />, or <paramref name="encoding" /> is <see langword="null" />.</exception>
        /// <exception cref="JsonException">An error occurred during JSON serialization.</exception>
        /// <exception cref="JsonRpcSerializationException">An error occurred during JSON-RPC serialization.</exception>
        /// <exception cref="OperationCanceledException">The operation was canceled.</exception>
        public ValueTask SerializeRequestAsync(JsonRpcRequest request, Stream jsonStream, Encoding encoding, CancellationToken cancellationToken = default)
        {
            if (request == null)
            {
                throw new ArgumentNullException(nameof(request));
            }
            if (jsonStream == null)
            {
                throw new ArgumentNullException(nameof(jsonStream));
            }
            if (encoding == null)
            {
                throw new ArgumentNullException(nameof(encoding));
            }

            cancellationToken.ThrowIfCancellationRequested();

            using var streamWriter = new StreamWriter(jsonStream, encoding, _defaultStreamBufferSize, true);

            return SerializeRequestAsync(request, streamWriter);
        }

        /// <summary>Serializes the specified collection of JSON-RPC requests to a JSON string.</summary>
        /// <param name="requests">The collection of JSON-RPC requests to serialize.</param>
        /// <returns>A JSON string representation of the specified collection of JSON-RPC requests.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="requests" /> is <see langword="null" />.</exception>
        /// <exception cref="JsonException">An error occurred during JSON serialization.</exception>
        /// <exception cref="JsonRpcSerializationException">An error occurred during JSON-RPC serialization.</exception>
        public string SerializeRequests(IReadOnlyList<JsonRpcRequest> requests)
        {
            if (requests == null)
            {
                throw new ArgumentNullException(nameof(requests));
            }

            using var stringWriter = new StringWriter(new StringBuilder(_messageBufferSize * requests.Count), CultureInfo.InvariantCulture);

            SerializeRequests(requests, stringWriter);

            return stringWriter.ToString();
        }

        /// <summary>Serializes the specified collection of JSON-RPC requests as a UTF-8 encoded JSON string to the specified stream.</summary>
        /// <param name="requests">The collection of JSON-RPC requests to serialize.</param>
        /// <param name="jsonStream">The stream to deserialize to.</param>
        /// <exception cref="ArgumentNullException"><paramref name="requests" /> or <paramref name="jsonStream" /> is <see langword="null" />.</exception>
        /// <exception cref="JsonException">An error occurred during JSON serialization.</exception>
        /// <exception cref="JsonRpcSerializationException">An error occurred during JSON-RPC serialization.</exception>
        public void SerializeRequests(IReadOnlyList<JsonRpcRequest> requests, Stream jsonStream)
        {
            if (requests == null)
            {
                throw new ArgumentNullException(nameof(requests));
            }
            if (jsonStream == null)
            {
                throw new ArgumentNullException(nameof(jsonStream));
            }

            using var streamWriter = new StreamWriter(jsonStream, _utf8Encoding, _defaultStreamBufferSize, true);

            SerializeRequests(requests, streamWriter);
        }

        /// <summary>Serializes the specified collection of JSON-RPC requests as a UTF-8 encoded JSON string to the specified stream.</summary>
        /// <param name="requests">The collection of JSON-RPC requests to serialize.</param>
        /// <param name="jsonStream">The stream to deserialize to.</param>
        /// <param name="encoding">The character encoding to use.</param>
        /// <exception cref="ArgumentNullException"><paramref name="requests" />, <paramref name="jsonStream" />, or <paramref name="encoding" /> is <see langword="null" />.</exception>
        /// <exception cref="JsonException">An error occurred during JSON serialization.</exception>
        /// <exception cref="JsonRpcSerializationException">An error occurred during JSON-RPC serialization.</exception>
        public void SerializeRequests(IReadOnlyList<JsonRpcRequest> requests, Stream jsonStream, Encoding encoding)
        {
            if (requests == null)
            {
                throw new ArgumentNullException(nameof(requests));
            }
            if (jsonStream == null)
            {
                throw new ArgumentNullException(nameof(jsonStream));
            }
            if (encoding == null)
            {
                throw new ArgumentNullException(nameof(encoding));
            }

            using var streamWriter = new StreamWriter(jsonStream, encoding, _defaultStreamBufferSize, true);

            SerializeRequests(requests, streamWriter);
        }

        /// <summary>Serializes the specified collection of JSON-RPC requests to a JSON string as an asynchronous operation.</summary>
        /// <param name="requests">The collection of JSON-RPC requests to serialize.</param>
        /// <param name="cancellationToken">The cancellation token for canceling the operation.</param>
        /// <returns>A task that represents the asynchronous operation. The task result is a JSON string representation of the specified collection of JSON-RPC requests.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="requests" /> is <see langword="null" />.</exception>
        /// <exception cref="JsonException">An error occurred during JSON serialization.</exception>
        /// <exception cref="JsonRpcSerializationException">An error occurred during JSON-RPC serialization.</exception>
        /// <exception cref="OperationCanceledException">The operation was canceled.</exception>
        public async ValueTask<string> SerializeRequestsAsync(IReadOnlyList<JsonRpcRequest> requests, CancellationToken cancellationToken = default)
        {
            if (requests == null)
            {
                throw new ArgumentNullException(nameof(requests));
            }

            cancellationToken.ThrowIfCancellationRequested();

            using var stringWriter = new StringWriter(new StringBuilder(_messageBufferSize * requests.Count), CultureInfo.InvariantCulture);

            await SerializeRequestsAsync(requests, stringWriter, cancellationToken).ConfigureAwait(false);

            return stringWriter.ToString();
        }

        /// <summary>Serializes the specified collection of JSON-RPC requests as a UTF-8 encoded JSON string to the specified stream as an asynchronous operation.</summary>
        /// <param name="requests">The collection of JSON-RPC requests to serialize.</param>
        /// <param name="jsonStream">The stream to deserialize to.</param>
        /// <param name="cancellationToken">The cancellation token for canceling the operation.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="requests" /> or <paramref name="jsonStream" /> is <see langword="null" />.</exception>
        /// <exception cref="JsonException">An error occurred during JSON serialization.</exception>
        /// <exception cref="JsonRpcSerializationException">An error occurred during JSON-RPC serialization.</exception>
        /// <exception cref="OperationCanceledException">The operation was canceled.</exception>
        public ValueTask SerializeRequestsAsync(IReadOnlyList<JsonRpcRequest> requests, Stream jsonStream, CancellationToken cancellationToken = default)
        {
            if (requests == null)
            {
                throw new ArgumentNullException(nameof(requests));
            }
            if (jsonStream == null)
            {
                throw new ArgumentNullException(nameof(jsonStream));
            }

            cancellationToken.ThrowIfCancellationRequested();

            using var streamWriter = new StreamWriter(jsonStream, _utf8Encoding, _defaultStreamBufferSize, true);

            return SerializeRequestsAsync(requests, streamWriter, cancellationToken);
        }

        /// <summary>Serializes the specified collection of JSON-RPC requests as a UTF-8 encoded JSON string to the specified stream as an asynchronous operation.</summary>
        /// <param name="requests">The collection of JSON-RPC requests to serialize.</param>
        /// <param name="jsonStream">The stream to deserialize to.</param>
        /// <param name="encoding">The character encoding to use.</param>
        /// <param name="cancellationToken">The cancellation token for canceling the operation.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="requests" />, <paramref name="jsonStream" />, or <paramref name="encoding" /> is <see langword="null" />.</exception>
        /// <exception cref="JsonException">An error occurred during JSON serialization.</exception>
        /// <exception cref="JsonRpcSerializationException">An error occurred during JSON-RPC serialization.</exception>
        /// <exception cref="OperationCanceledException">The operation was canceled.</exception>
        public ValueTask SerializeRequestsAsync(IReadOnlyList<JsonRpcRequest> requests, Stream jsonStream, Encoding encoding, CancellationToken cancellationToken = default)
        {
            if (requests == null)
            {
                throw new ArgumentNullException(nameof(requests));
            }
            if (jsonStream == null)
            {
                throw new ArgumentNullException(nameof(jsonStream));
            }
            if (encoding == null)
            {
                throw new ArgumentNullException(nameof(encoding));
            }

            cancellationToken.ThrowIfCancellationRequested();

            using var streamWriter = new StreamWriter(jsonStream, encoding, _defaultStreamBufferSize, true);

            return SerializeRequestsAsync(requests, streamWriter, cancellationToken);
        }

        /// <summary>Serializes the specified JSON-RPC response to a JSON string.</summary>
        /// <param name="response">The JSON-RPC response to serialize.</param>
        /// <returns>A JSON string representation of the specified response.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="response" /> is <see langword="null" />.</exception>
        /// <exception cref="JsonException">An error occurred during JSON serialization.</exception>
        /// <exception cref="JsonRpcSerializationException">An error occurred during JSON-RPC serialization.</exception>
        public string SerializeResponse(JsonRpcResponse response)
        {
            if (response == null)
            {
                throw new ArgumentNullException(nameof(response));
            }

            using var stringWriter = new StringWriter(new StringBuilder(_messageBufferSize), CultureInfo.InvariantCulture);

            SerializeResponse(response, stringWriter);

            return stringWriter.ToString();
        }

        /// <summary>Serializes the specified JSON-RPC response as a UTF-8 encoded JSON string to the specified stream.</summary>
        /// <param name="response">The JSON-RPC response to serialize.</param>
        /// <param name="jsonStream">The stream to deserialize to.</param>
        /// <exception cref="ArgumentNullException"><paramref name="response" /> or <paramref name="jsonStream" /> is <see langword="null" />.</exception>
        /// <exception cref="JsonException">An error occurred during JSON serialization.</exception>
        /// <exception cref="JsonRpcSerializationException">An error occurred during JSON-RPC serialization.</exception>
        public void SerializeResponse(JsonRpcResponse response, Stream jsonStream)
        {
            if (response == null)
            {
                throw new ArgumentNullException(nameof(response));
            }
            if (jsonStream == null)
            {
                throw new ArgumentNullException(nameof(jsonStream));
            }

            using var streamWriter = new StreamWriter(jsonStream, _utf8Encoding, _defaultStreamBufferSize, true);

            SerializeResponse(response, streamWriter);
        }

        /// <summary>Serializes the specified JSON-RPC response as a UTF-8 encoded JSON string to the specified stream.</summary>
        /// <param name="response">The JSON-RPC response to serialize.</param>
        /// <param name="jsonStream">The stream to deserialize to.</param>
        /// <param name="encoding">The character encoding to use.</param>
        /// <exception cref="ArgumentNullException"><paramref name="response" />, <paramref name="jsonStream" />, or <paramref name="encoding" /> is <see langword="null" />.</exception>
        /// <exception cref="JsonException">An error occurred during JSON serialization.</exception>
        /// <exception cref="JsonRpcSerializationException">An error occurred during JSON-RPC serialization.</exception>
        public void SerializeResponse(JsonRpcResponse response, Stream jsonStream, Encoding encoding)
        {
            if (response == null)
            {
                throw new ArgumentNullException(nameof(response));
            }
            if (jsonStream == null)
            {
                throw new ArgumentNullException(nameof(jsonStream));
            }
            if (encoding == null)
            {
                throw new ArgumentNullException(nameof(encoding));
            }

            using var streamWriter = new StreamWriter(jsonStream, encoding, _defaultStreamBufferSize, true);

            SerializeResponse(response, streamWriter);
        }

        /// <summary>Serializes the specified JSON-RPC response to a JSON string as an asynchronous operation.</summary>
        /// <param name="response">The JSON-RPC response to serialize.</param>
        /// <param name="cancellationToken">The cancellation token for canceling the operation.</param>
        /// <returns>A task that represents the asynchronous operation. The task result is a JSON string representation of the specified response.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="response" /> is <see langword="null" />.</exception>
        /// <exception cref="JsonException">An error occurred during JSON serialization.</exception>
        /// <exception cref="JsonRpcSerializationException">An error occurred during JSON-RPC serialization.</exception>
        /// <exception cref="OperationCanceledException">The operation was canceled.</exception>
        public async ValueTask<string> SerializeResponseAsync(JsonRpcResponse response, CancellationToken cancellationToken = default)
        {
            if (response == null)
            {
                throw new ArgumentNullException(nameof(response));
            }

            cancellationToken.ThrowIfCancellationRequested();

            using var stringWriter = new StringWriter(new StringBuilder(_messageBufferSize), CultureInfo.InvariantCulture);

            await SerializeResponseAsync(response, stringWriter).ConfigureAwait(false);

            return stringWriter.ToString();
        }

        /// <summary>Serializes the specified JSON-RPC response as a UTF-8 encoded JSON string to the specified stream as an asynchronous operation.</summary>
        /// <param name="response">The JSON-RPC response to serialize.</param>
        /// <param name="jsonStream">The stream to deserialize to.</param>
        /// <param name="cancellationToken">The cancellation token for canceling the operation.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="response" /> or <paramref name="jsonStream" /> is <see langword="null" />.</exception>
        /// <exception cref="JsonException">An error occurred during JSON serialization.</exception>
        /// <exception cref="JsonRpcSerializationException">An error occurred during JSON-RPC serialization.</exception>
        /// <exception cref="OperationCanceledException">The operation was canceled.</exception>
        public ValueTask SerializeResponseAsync(JsonRpcResponse response, Stream jsonStream, CancellationToken cancellationToken = default)
        {
            if (response == null)
            {
                throw new ArgumentNullException(nameof(response));
            }
            if (jsonStream == null)
            {
                throw new ArgumentNullException(nameof(jsonStream));
            }

            cancellationToken.ThrowIfCancellationRequested();

            using var streamWriter = new StreamWriter(jsonStream, _utf8Encoding, _defaultStreamBufferSize, true);

            return SerializeResponseAsync(response, streamWriter);
        }

        /// <summary>Serializes the specified JSON-RPC response as a UTF-8 encoded JSON string to the specified stream as an asynchronous operation.</summary>
        /// <param name="response">The JSON-RPC response to serialize.</param>
        /// <param name="jsonStream">The stream to deserialize to.</param>
        /// <param name="encoding">The character encoding to use.</param>
        /// <param name="cancellationToken">The cancellation token for canceling the operation.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="response" />, <paramref name="jsonStream" />, or <paramref name="encoding" /> is <see langword="null" />.</exception>
        /// <exception cref="JsonException">An error occurred during JSON serialization.</exception>
        /// <exception cref="JsonRpcSerializationException">An error occurred during JSON-RPC serialization.</exception>
        /// <exception cref="OperationCanceledException">The operation was canceled.</exception>
        public ValueTask SerializeResponseAsync(JsonRpcResponse response, Stream jsonStream, Encoding encoding, CancellationToken cancellationToken = default)
        {
            if (response == null)
            {
                throw new ArgumentNullException(nameof(response));
            }
            if (jsonStream == null)
            {
                throw new ArgumentNullException(nameof(jsonStream));
            }
            if (encoding == null)
            {
                throw new ArgumentNullException(nameof(encoding));
            }

            cancellationToken.ThrowIfCancellationRequested();

            using var streamWriter = new StreamWriter(jsonStream, encoding, _defaultStreamBufferSize, true);

            return SerializeResponseAsync(response, streamWriter);
        }

        /// <summary>Serializes the specified collection of JSON-RPC responses to a JSON string.</summary>
        /// <param name="responses">The collection of JSON-RPC responses to serialize.</param>
        /// <returns>A JSON string representation of the specified collection of JSON-RPC responses.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="responses" /> is <see langword="null" />.</exception>
        /// <exception cref="JsonException">An error occurred during JSON serialization.</exception>
        /// <exception cref="JsonRpcSerializationException">An error occurred during JSON-RPC serialization.</exception>
        public string SerializeResponses(IReadOnlyList<JsonRpcResponse> responses)
        {
            if (responses == null)
            {
                throw new ArgumentNullException(nameof(responses));
            }

            using var stringWriter = new StringWriter(new StringBuilder(_messageBufferSize * responses.Count), CultureInfo.InvariantCulture);

            SerializeResponses(responses, stringWriter);

            return stringWriter.ToString();
        }

        /// <summary>Serializes the specified collection of JSON-RPC responses as a UTF-8 encoded JSON string to the specified stream.</summary>
        /// <param name="responses">The collection of JSON-RPC responses to serialize.</param>
        /// <param name="jsonStream">The stream to deserialize to.</param>
        /// <exception cref="ArgumentNullException"><paramref name="responses" /> or <paramref name="jsonStream" /> is <see langword="null" />.</exception>
        /// <exception cref="JsonException">An error occurred during JSON serialization.</exception>
        /// <exception cref="JsonRpcSerializationException">An error occurred during JSON-RPC serialization.</exception>
        public void SerializeResponses(IReadOnlyList<JsonRpcResponse> responses, Stream jsonStream)
        {
            if (responses == null)
            {
                throw new ArgumentNullException(nameof(responses));
            }
            if (jsonStream == null)
            {
                throw new ArgumentNullException(nameof(jsonStream));
            }

            using var streamWriter = new StreamWriter(jsonStream, _utf8Encoding, _defaultStreamBufferSize, true);

            SerializeResponses(responses, streamWriter);
        }

        /// <summary>Serializes the specified collection of JSON-RPC responses as a UTF-8 encoded JSON string to the specified stream.</summary>
        /// <param name="responses">The collection of JSON-RPC responses to serialize.</param>
        /// <param name="jsonStream">The stream to deserialize to.</param>
        /// <param name="encoding">The character encoding to use.</param>
        /// <exception cref="ArgumentNullException"><paramref name="responses" />, <paramref name="jsonStream" />, or <paramref name="encoding" /> is <see langword="null" />.</exception>
        /// <exception cref="JsonException">An error occurred during JSON serialization.</exception>
        /// <exception cref="JsonRpcSerializationException">An error occurred during JSON-RPC serialization.</exception>
        public void SerializeResponses(IReadOnlyList<JsonRpcResponse> responses, Stream jsonStream, Encoding encoding)
        {
            if (responses == null)
            {
                throw new ArgumentNullException(nameof(responses));
            }
            if (jsonStream == null)
            {
                throw new ArgumentNullException(nameof(jsonStream));
            }
            if (encoding == null)
            {
                throw new ArgumentNullException(nameof(encoding));
            }

            using var streamWriter = new StreamWriter(jsonStream, encoding, _defaultStreamBufferSize, true);

            SerializeResponses(responses, streamWriter);
        }

        /// <summary>Serializes the specified collection of JSON-RPC responses to a JSON string as an asynchronous operation.</summary>
        /// <param name="responses">The collection of JSON-RPC responses to serialize.</param>
        /// <param name="cancellationToken">The cancellation token for canceling the operation.</param>
        /// <returns>A task that represents the asynchronous operation. The task result is a JSON string representation of the specified collection of JSON-RPC responses.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="responses" /> is <see langword="null" />.</exception>
        /// <exception cref="JsonException">An error occurred during JSON serialization.</exception>
        /// <exception cref="JsonRpcSerializationException">An error occurred during JSON-RPC serialization.</exception>
        /// <exception cref="OperationCanceledException">The operation was canceled.</exception>
        public async ValueTask<string> SerializeResponsesAsync(IReadOnlyList<JsonRpcResponse> responses, CancellationToken cancellationToken = default)
        {
            if (responses == null)
            {
                throw new ArgumentNullException(nameof(responses));
            }

            cancellationToken.ThrowIfCancellationRequested();

            using var stringWriter = new StringWriter(new StringBuilder(_messageBufferSize * responses.Count), CultureInfo.InvariantCulture);

            await SerializeResponsesAsync(responses, stringWriter, cancellationToken).ConfigureAwait(false);

            return stringWriter.ToString();
        }

        /// <summary>Serializes the specified collection of JSON-RPC responses as a UTF-8 encoded JSON string to the specified stream as an asynchronous operation.</summary>
        /// <param name="responses">The collection of JSON-RPC responses to serialize.</param>
        /// <param name="jsonStream">The stream to deserialize to.</param>
        /// <param name="cancellationToken">The cancellation token for canceling the operation.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="responses" /> or <paramref name="jsonStream" /> is <see langword="null" />.</exception>
        /// <exception cref="JsonException">An error occurred during JSON serialization.</exception>
        /// <exception cref="JsonRpcSerializationException">An error occurred during JSON-RPC serialization.</exception>
        /// <exception cref="OperationCanceledException">The operation was canceled.</exception>
        public ValueTask SerializeResponsesAsync(IReadOnlyList<JsonRpcResponse> responses, Stream jsonStream, CancellationToken cancellationToken = default)
        {
            if (responses == null)
            {
                throw new ArgumentNullException(nameof(responses));
            }
            if (jsonStream == null)
            {
                throw new ArgumentNullException(nameof(jsonStream));
            }

            cancellationToken.ThrowIfCancellationRequested();

            using var streamWriter = new StreamWriter(jsonStream, _utf8Encoding, _defaultStreamBufferSize, true);

            return SerializeResponsesAsync(responses, streamWriter, cancellationToken);
        }

        /// <summary>Serializes the specified collection of JSON-RPC responses as a UTF-8 encoded JSON string to the specified stream as an asynchronous operation.</summary>
        /// <param name="responses">The collection of JSON-RPC responses to serialize.</param>
        /// <param name="jsonStream">The stream to deserialize to.</param>
        /// <param name="encoding">The character encoding to use.</param>
        /// <param name="cancellationToken">The cancellation token for canceling the operation.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="responses" />, <paramref name="jsonStream" />, or <paramref name="encoding" /> is <see langword="null" />.</exception>
        /// <exception cref="JsonException">An error occurred during JSON serialization.</exception>
        /// <exception cref="JsonRpcSerializationException">An error occurred during JSON-RPC serialization.</exception>
        /// <exception cref="OperationCanceledException">The operation was canceled.</exception>
        public ValueTask SerializeResponsesAsync(IReadOnlyList<JsonRpcResponse> responses, Stream jsonStream, Encoding encoding, CancellationToken cancellationToken = default)
        {
            if (responses == null)
            {
                throw new ArgumentNullException(nameof(responses));
            }
            if (jsonStream == null)
            {
                throw new ArgumentNullException(nameof(jsonStream));
            }
            if (encoding == null)
            {
                throw new ArgumentNullException(nameof(encoding));
            }

            cancellationToken.ThrowIfCancellationRequested();

            using var streamWriter = new StreamWriter(jsonStream, encoding, _defaultStreamBufferSize, true);

            return SerializeResponsesAsync(responses, streamWriter, cancellationToken);
        }
    }
}
