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

        private static readonly Encoding _defaultJsonEncoding = new UTF8Encoding(false, true);

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

        /// <summary>Deserializes the specified JSON string to JSON-RPC request data.</summary>
        /// <param name="input">The JSON string to deserialize.</param>
        /// <returns>A JSON-RPC request data deserialization result.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="input" /> is <see langword="null" />.</exception>
        /// <exception cref="JsonException">An error occurred during JSON deserialization.</exception>
        /// <exception cref="JsonRpcSerializationException">An error occurred during JSON-RPC deserialization.</exception>
        public JsonRpcData<JsonRpcRequest> DeserializeRequestData(string input)
        {
            if (input == null)
            {
                throw new ArgumentNullException(nameof(input));
            }

            using (var stringReader = new StringReader(input))
            {
                return DeserializeRequestData(stringReader);
            }
        }

        /// <summary>Deserializes a UTF-8 encoded JSON string from the specified stream to JSON-RPC request data.</summary>
        /// <param name="input">The stream to deserialize from.</param>
        /// <returns>A JSON-RPC request data deserialization result.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="input" /> is <see langword="null" />.</exception>
        /// <exception cref="JsonException">An error occurred during JSON deserialization.</exception>
        /// <exception cref="JsonRpcSerializationException">An error occurred during JSON-RPC deserialization.</exception>
        public JsonRpcData<JsonRpcRequest> DeserializeRequestData(Stream input)
        {
            if (input == null)
            {
                throw new ArgumentNullException(nameof(input));
            }

            using (var streamReader = new StreamReader(input, _defaultJsonEncoding, false, _defaultStreamBufferSize, true))
            {
                return DeserializeRequestData(streamReader);
            }
        }

        /// <summary>Deserializes a JSON string using the specified reader to JSON-RPC request data.</summary>
        /// <param name="input">The reader to use for deserializing.</param>
        /// <returns>A JSON-RPC request data deserialization result.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="input" /> is <see langword="null" />.</exception>
        /// <exception cref="JsonException">An error occurred during JSON deserialization.</exception>
        /// <exception cref="JsonRpcSerializationException">An error occurred during JSON-RPC deserialization.</exception>
        public JsonRpcData<JsonRpcRequest> DeserializeRequestData(TextReader input)
        {
            if (input == null)
            {
                throw new ArgumentNullException(nameof(input));
            }

            using (var jsonReader = new JsonTextReader(input))
            {
                SetupJsonReader(jsonReader);

                return DeserializeRequestData(jsonReader, default);
            }
        }

        /// <summary>Deserializes the specified JSON string to JSON-RPC request data as an asynchronous operation.</summary>
        /// <param name="input">The JSON string to deserialize.</param>
        /// <param name="cancellationToken">The cancellation token for canceling the operation.</param>
        /// <returns>A task that represents the asynchronous operation. The task result is a JSON-RPC request data deserialization result.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="input" /> is <see langword="null" />.</exception>
        /// <exception cref="JsonException">An error occurred during JSON deserialization.</exception>
        /// <exception cref="JsonRpcSerializationException">An error occurred during JSON-RPC deserialization.</exception>
        /// <exception cref="OperationCanceledException">The operation was canceled.</exception>
        public ValueTask<JsonRpcData<JsonRpcRequest>> DeserializeRequestDataAsync(string input, CancellationToken cancellationToken = default)
        {
            if (input == null)
            {
                throw new ArgumentNullException(nameof(input));
            }

            cancellationToken.ThrowIfCancellationRequested();

            using (var stringReader = new StringReader(input))
            {
                return DeserializeRequestDataAsync(stringReader, cancellationToken);
            }
        }

        /// <summary>Deserializes a UTF-8 encoded JSON string from the specified stream to JSON-RPC request data as an asynchronous operation.</summary>
        /// <param name="input">The stream to deserialize from.</param>
        /// <param name="cancellationToken">The cancellation token for canceling the operation.</param>
        /// <returns>A task that represents the asynchronous operation. The task result is a JSON-RPC request data deserialization result.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="input" /> is <see langword="null" />.</exception>
        /// <exception cref="JsonException">An error occurred during JSON deserialization.</exception>
        /// <exception cref="JsonRpcSerializationException">An error occurred during JSON-RPC deserialization.</exception>
        /// <exception cref="OperationCanceledException">The operation was canceled.</exception>
        public ValueTask<JsonRpcData<JsonRpcRequest>> DeserializeRequestDataAsync(Stream input, CancellationToken cancellationToken = default)
        {
            if (input == null)
            {
                throw new ArgumentNullException(nameof(input));
            }

            cancellationToken.ThrowIfCancellationRequested();

            using (var streamReader = new StreamReader(input, _defaultJsonEncoding, false, _defaultStreamBufferSize, true))
            {
                return DeserializeRequestDataAsync(streamReader, cancellationToken);
            }
        }

        /// <summary>Deserializes a JSON string using the specified reader to JSON-RPC request data as an asynchronous operation.</summary>
        /// <param name="input">The reader to use for deserializing.</param>
        /// <param name="cancellationToken">The cancellation token for canceling the operation.</param>
        /// <returns>A task that represents the asynchronous operation. The task result is a JSON-RPC request data deserialization result.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="input" /> is <see langword="null" />.</exception>
        /// <exception cref="JsonException">An error occurred during JSON deserialization.</exception>
        /// <exception cref="JsonRpcSerializationException">An error occurred during JSON-RPC deserialization.</exception>
        /// <exception cref="OperationCanceledException">The operation was canceled.</exception>
        public ValueTask<JsonRpcData<JsonRpcRequest>> DeserializeRequestDataAsync(TextReader input, CancellationToken cancellationToken = default)
        {
            if (input == null)
            {
                throw new ArgumentNullException(nameof(input));
            }

            cancellationToken.ThrowIfCancellationRequested();

            using (var jsonReader = new JsonTextReader(input))
            {
                SetupJsonReader(jsonReader);

                return new ValueTask<JsonRpcData<JsonRpcRequest>>(DeserializeRequestData(jsonReader, cancellationToken));
            }
        }

        /// <summary>Deserializes the specified JSON string to JSON-RPC response data.</summary>
        /// <param name="input">The JSON string to deserialize.</param>
        /// <returns>A JSON-RPC response data deserialization result.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="input" /> is <see langword="null" />.</exception>
        /// <exception cref="JsonException">An error occurred during JSON deserialization.</exception>
        /// <exception cref="JsonRpcSerializationException">An error occurred during JSON-RPC deserialization.</exception>
        public JsonRpcData<JsonRpcResponse> DeserializeResponseData(string input)
        {
            if (input == null)
            {
                throw new ArgumentNullException(nameof(input));
            }

            using (var stringReader = new StringReader(input))
            {
                return DeserializeResponseData(stringReader);
            }
        }

        /// <summary>Deserializes a UTF-8 encoded JSON string from the specified stream to JSON-RPC response data.</summary>
        /// <param name="input">The stream to deserialize from.</param>
        /// <returns>A JSON-RPC response data deserialization result.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="input" /> is <see langword="null" />.</exception>
        /// <exception cref="JsonException">An error occurred during JSON deserialization.</exception>
        /// <exception cref="JsonRpcSerializationException">An error occurred during JSON-RPC deserialization.</exception>
        public JsonRpcData<JsonRpcResponse> DeserializeResponseData(Stream input)
        {
            if (input == null)
            {
                throw new ArgumentNullException(nameof(input));
            }

            using (var streamReader = new StreamReader(input, _defaultJsonEncoding, false, _defaultStreamBufferSize, true))
            {
                return DeserializeResponseData(streamReader);
            }
        }

        /// <summary>Deserializes a JSON string using the specified reader to JSON-RPC response data.</summary>
        /// <param name="input">The reader to use for deserializing.</param>
        /// <returns>A JSON-RPC response data deserialization result.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="input" /> is <see langword="null" />.</exception>
        /// <exception cref="JsonException">An error occurred during JSON deserialization.</exception>
        /// <exception cref="JsonRpcSerializationException">An error occurred during JSON-RPC deserialization.</exception>
        public JsonRpcData<JsonRpcResponse> DeserializeResponseData(TextReader input)
        {
            if (input == null)
            {
                throw new ArgumentNullException(nameof(input));
            }

            using (var jsonReader = new JsonTextReader(input))
            {
                SetupJsonReader(jsonReader);

                return DeserializeResponseData(jsonReader, default);
            }
        }

        /// <summary>Deserializes the specified JSON string to JSON-RPC response data as an asynchronous operation.</summary>
        /// <param name="input">The JSON string to deserialize.</param>
        /// <param name="cancellationToken">The cancellation token for canceling the operation.</param>
        /// <returns>A task that represents the asynchronous operation. The task result is a JSON-RPC response data deserialization result.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="input" /> is <see langword="null" />.</exception>
        /// <exception cref="JsonException">An error occurred during JSON deserialization.</exception>
        /// <exception cref="JsonRpcSerializationException">An error occurred during JSON-RPC deserialization.</exception>
        /// <exception cref="OperationCanceledException">The operation was canceled.</exception>
        public ValueTask<JsonRpcData<JsonRpcResponse>> DeserializeResponseDataAsync(string input, CancellationToken cancellationToken = default)
        {
            if (input == null)
            {
                throw new ArgumentNullException(nameof(input));
            }

            cancellationToken.ThrowIfCancellationRequested();

            using (var stringReader = new StringReader(input))
            {
                return DeserializeResponseDataAsync(stringReader, cancellationToken);
            }
        }

        /// <summary>Deserializes a UTF-8 encoded JSON string from the specified stream to JSON-RPC response data as an asynchronous operation.</summary>
        /// <param name="input">The stream to deserialize from.</param>
        /// <param name="cancellationToken">The cancellation token for canceling the operation.</param>
        /// <returns>A task that represents the asynchronous operation. The task result is a JSON-RPC response data deserialization result.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="input" /> is <see langword="null" />.</exception>
        /// <exception cref="JsonException">An error occurred during JSON deserialization.</exception>
        /// <exception cref="JsonRpcSerializationException">An error occurred during JSON-RPC deserialization.</exception>
        /// <exception cref="OperationCanceledException">The operation was canceled.</exception>
        public ValueTask<JsonRpcData<JsonRpcResponse>> DeserializeResponseDataAsync(Stream input, CancellationToken cancellationToken = default)
        {
            if (input == null)
            {
                throw new ArgumentNullException(nameof(input));
            }

            cancellationToken.ThrowIfCancellationRequested();

            using (var streamReader = new StreamReader(input, _defaultJsonEncoding, false, _defaultStreamBufferSize, true))
            {
                return DeserializeResponseDataAsync(streamReader, cancellationToken);
            }
        }

        /// <summary>Deserializes a JSON string using the specified reader to JSON-RPC response data as an asynchronous operation.</summary>
        /// <param name="input">The reader to use for deserializing.</param>
        /// <param name="cancellationToken">The cancellation token for canceling the operation.</param>
        /// <returns>A task that represents the asynchronous operation. The task result is a JSON-RPC response data deserialization result.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="input" /> is <see langword="null" />.</exception>
        /// <exception cref="JsonException">An error occurred during JSON deserialization.</exception>
        /// <exception cref="JsonRpcSerializationException">An error occurred during JSON-RPC deserialization.</exception>
        /// <exception cref="OperationCanceledException">The operation was canceled.</exception>
        public ValueTask<JsonRpcData<JsonRpcResponse>> DeserializeResponseDataAsync(TextReader input, CancellationToken cancellationToken = default)
        {
            if (input == null)
            {
                throw new ArgumentNullException(nameof(input));
            }

            cancellationToken.ThrowIfCancellationRequested();

            using (var jsonReader = new JsonTextReader(input))
            {
                SetupJsonReader(jsonReader);

                return new ValueTask<JsonRpcData<JsonRpcResponse>>(DeserializeResponseData(jsonReader, cancellationToken));
            }
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

            using (var stringWriter = new StringWriter(new StringBuilder(_messageBufferSize), CultureInfo.InvariantCulture))
            {
                SerializeRequest(request, stringWriter);

                return stringWriter.ToString();
            }
        }

        /// <summary>Serializes the specified JSON-RPC request as a UTF-8 encoded JSON string to the specified stream.</summary>
        /// <param name="request">The JSON-RPC request to serialize.</param>
        /// <param name="output">The stream to deserialize to.</param>
        /// <exception cref="ArgumentNullException"><paramref name="request" /> or <paramref name="output" /> is <see langword="null" />.</exception>
        /// <exception cref="JsonException">An error occurred during JSON serialization.</exception>
        /// <exception cref="JsonRpcSerializationException">An error occurred during JSON-RPC serialization.</exception>
        public void SerializeRequest(JsonRpcRequest request, Stream output)
        {
            if (request == null)
            {
                throw new ArgumentNullException(nameof(request));
            }
            if (output == null)
            {
                throw new ArgumentNullException(nameof(output));
            }

            using (var streamWriter = new StreamWriter(output, _defaultJsonEncoding, _defaultStreamBufferSize, true))
            {
                SerializeRequest(request, streamWriter);
            }
        }

        /// <summary>Serializes the specified JSON-RPC request as a JSON string using the specified writer.</summary>
        /// <param name="request">The JSON-RPC request to serialize.</param>
        /// <param name="output">The writer to use for deserializing.</param>
        /// <exception cref="ArgumentNullException"><paramref name="request" /> or <paramref name="output" /> is <see langword="null" />.</exception>
        /// <exception cref="JsonException">An error occurred during JSON serialization.</exception>
        /// <exception cref="JsonRpcSerializationException">An error occurred during JSON-RPC serialization.</exception>
        public void SerializeRequest(JsonRpcRequest request, TextWriter output)
        {
            if (request == null)
            {
                throw new ArgumentNullException(nameof(request));
            }
            if (output == null)
            {
                throw new ArgumentNullException(nameof(output));
            }

            using (var jsonWriter = new JsonTextWriter(output))
            {
                SetupJsonWriter(jsonWriter);
                SerializeRequest(jsonWriter, request);
            }
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

            using (var stringWriter = new StringWriter(new StringBuilder(_messageBufferSize), CultureInfo.InvariantCulture))
            {
                await SerializeRequestAsync(request, stringWriter, cancellationToken).ConfigureAwait(false);

                return stringWriter.ToString();
            }
        }

        /// <summary>Serializes the specified JSON-RPC request as a UTF-8 encoded JSON string to the specified stream as an asynchronous operation.</summary>
        /// <param name="request">The JSON-RPC request to serialize.</param>
        /// <param name="output">The stream to deserialize to.</param>
        /// <param name="cancellationToken">The cancellation token for canceling the operation.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="request" /> or <paramref name="output" /> is <see langword="null" />.</exception>
        /// <exception cref="JsonException">An error occurred during JSON serialization.</exception>
        /// <exception cref="JsonRpcSerializationException">An error occurred during JSON-RPC serialization.</exception>
        /// <exception cref="OperationCanceledException">The operation was canceled.</exception>
        public ValueTask SerializeRequestAsync(JsonRpcRequest request, Stream output, CancellationToken cancellationToken = default)
        {
            if (request == null)
            {
                throw new ArgumentNullException(nameof(request));
            }
            if (output == null)
            {
                throw new ArgumentNullException(nameof(output));
            }

            cancellationToken.ThrowIfCancellationRequested();

            using (var streamWriter = new StreamWriter(output, _defaultJsonEncoding, _defaultStreamBufferSize, true))
            {
                return SerializeRequestAsync(request, streamWriter, cancellationToken);
            }
        }

        /// <summary>Serializes the specified JSON-RPC request as a JSON string using the specified writer as an asynchronous operation.</summary>
        /// <param name="request">The JSON-RPC request to serialize.</param>
        /// <param name="output">The writer to use for deserializing.</param>
        /// <param name="cancellationToken">The cancellation token for canceling the operation.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="request" /> or <paramref name="output" /> is <see langword="null" />.</exception>
        /// <exception cref="JsonException">An error occurred during JSON serialization.</exception>
        /// <exception cref="JsonRpcSerializationException">An error occurred during JSON-RPC serialization.</exception>
        /// <exception cref="OperationCanceledException">The operation was canceled.</exception>
        public ValueTask SerializeRequestAsync(JsonRpcRequest request, TextWriter output, CancellationToken cancellationToken = default)
        {
            if (request == null)
            {
                throw new ArgumentNullException(nameof(request));
            }
            if (output == null)
            {
                throw new ArgumentNullException(nameof(output));
            }

            cancellationToken.ThrowIfCancellationRequested();

            using (var jsonWriter = new JsonTextWriter(output))
            {
                SetupJsonWriter(jsonWriter);
                SerializeRequest(jsonWriter, request);
            }

            return default;
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

            using (var stringWriter = new StringWriter(new StringBuilder(_messageBufferSize * requests.Count), CultureInfo.InvariantCulture))
            {
                SerializeRequests(requests, stringWriter);

                return stringWriter.ToString();
            }
        }

        /// <summary>Serializes the specified collection of JSON-RPC requests as a UTF-8 encoded JSON string to the specified stream.</summary>
        /// <param name="requests">The collection of JSON-RPC requests to serialize.</param>
        /// <param name="output">The stream to deserialize to.</param>
        /// <exception cref="ArgumentNullException"><paramref name="requests" /> or <paramref name="output" /> is <see langword="null" />.</exception>
        /// <exception cref="JsonException">An error occurred during JSON serialization.</exception>
        /// <exception cref="JsonRpcSerializationException">An error occurred during JSON-RPC serialization.</exception>
        public void SerializeRequests(IReadOnlyList<JsonRpcRequest> requests, Stream output)
        {
            if (requests == null)
            {
                throw new ArgumentNullException(nameof(requests));
            }
            if (output == null)
            {
                throw new ArgumentNullException(nameof(output));
            }

            using (var streamWriter = new StreamWriter(output, _defaultJsonEncoding, _defaultStreamBufferSize, true))
            {
                SerializeRequests(requests, streamWriter);
            }
        }

        /// <summary>Serializes the specified collection of JSON-RPC requests as a JSON string using the specified writer.</summary>
        /// <param name="requests">The collection of JSON-RPC requests to serialize.</param>
        /// <param name="output">The writer for deserializing.</param>
        /// <exception cref="ArgumentNullException"><paramref name="requests" /> or <paramref name="output" /> is <see langword="null" />.</exception>
        /// <exception cref="JsonException">An error occurred during JSON serialization.</exception>
        /// <exception cref="JsonRpcSerializationException">An error occurred during JSON-RPC serialization.</exception>
        public void SerializeRequests(IReadOnlyList<JsonRpcRequest> requests, TextWriter output)
        {
            if (requests == null)
            {
                throw new ArgumentNullException(nameof(requests));
            }
            if (output == null)
            {
                throw new ArgumentNullException(nameof(output));
            }

            using (var jsonWriter = new JsonTextWriter(output))
            {
                SetupJsonWriter(jsonWriter);
                SerializeRequests(jsonWriter, requests, default);
            }
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

            using (var stringWriter = new StringWriter(new StringBuilder(_messageBufferSize * requests.Count), CultureInfo.InvariantCulture))
            {
                await SerializeRequestsAsync(requests, stringWriter, cancellationToken).ConfigureAwait(false);

                return stringWriter.ToString();
            }
        }

        /// <summary>Serializes the specified collection of JSON-RPC requests as a UTF-8 encoded JSON string to the specified stream as an asynchronous operation.</summary>
        /// <param name="requests">The collection of JSON-RPC requests to serialize.</param>
        /// <param name="output">The stream to deserialize to.</param>
        /// <param name="cancellationToken">The cancellation token for canceling the operation.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="requests" /> or <paramref name="output" /> is <see langword="null" />.</exception>
        /// <exception cref="JsonException">An error occurred during JSON serialization.</exception>
        /// <exception cref="JsonRpcSerializationException">An error occurred during JSON-RPC serialization.</exception>
        /// <exception cref="OperationCanceledException">The operation was canceled.</exception>
        public ValueTask SerializeRequestsAsync(IReadOnlyList<JsonRpcRequest> requests, Stream output, CancellationToken cancellationToken = default)
        {
            if (requests == null)
            {
                throw new ArgumentNullException(nameof(requests));
            }
            if (output == null)
            {
                throw new ArgumentNullException(nameof(output));
            }

            cancellationToken.ThrowIfCancellationRequested();

            using (var streamWriter = new StreamWriter(output, _defaultJsonEncoding, _defaultStreamBufferSize, true))
            {
                return SerializeRequestsAsync(requests, streamWriter, cancellationToken);
            }
        }

        /// <summary>Serializes the specified collection of JSON-RPC requests as a JSON string using the specified writer as an asynchronous operation.</summary>
        /// <param name="requests">The collection of JSON-RPC requests to serialize.</param>
        /// <param name="output">The writer for deserializing.</param>
        /// <param name="cancellationToken">The cancellation token for canceling the operation.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="requests" /> or <paramref name="output" /> is <see langword="null" />.</exception>
        /// <exception cref="JsonException">An error occurred during JSON serialization.</exception>
        /// <exception cref="JsonRpcSerializationException">An error occurred during JSON-RPC serialization.</exception>
        /// <exception cref="OperationCanceledException">The operation was canceled.</exception>
        public ValueTask SerializeRequestsAsync(IReadOnlyList<JsonRpcRequest> requests, TextWriter output, CancellationToken cancellationToken = default)
        {
            if (requests == null)
            {
                throw new ArgumentNullException(nameof(requests));
            }
            if (output == null)
            {
                throw new ArgumentNullException(nameof(output));
            }

            cancellationToken.ThrowIfCancellationRequested();

            using (var jsonWriter = new JsonTextWriter(output))
            {
                SetupJsonWriter(jsonWriter);
                SerializeRequests(jsonWriter, requests, cancellationToken);
            }

            return default;
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

            using (var stringWriter = new StringWriter(new StringBuilder(_messageBufferSize), CultureInfo.InvariantCulture))
            {
                SerializeResponse(response, stringWriter);

                return stringWriter.ToString();
            }
        }

        /// <summary>Serializes the specified JSON-RPC response as a UTF-8 encoded JSON string to the specified stream.</summary>
        /// <param name="response">The JSON-RPC response to serialize.</param>
        /// <param name="output">The stream to deserialize to.</param>
        /// <exception cref="ArgumentNullException"><paramref name="response" /> or <paramref name="output" /> is <see langword="null" />.</exception>
        /// <exception cref="JsonException">An error occurred during JSON serialization.</exception>
        /// <exception cref="JsonRpcSerializationException">An error occurred during JSON-RPC serialization.</exception>
        public void SerializeResponse(JsonRpcResponse response, Stream output)
        {
            if (response == null)
            {
                throw new ArgumentNullException(nameof(response));
            }
            if (output == null)
            {
                throw new ArgumentNullException(nameof(output));
            }

            using (var streamWriter = new StreamWriter(output, _defaultJsonEncoding, _defaultStreamBufferSize, true))
            {
                SerializeResponse(response, streamWriter);
            }
        }

        /// <summary>Serializes the specified JSON-RPC response as a JSON string using the specified writer.</summary>
        /// <param name="response">The JSON-RPC response to serialize.</param>
        /// <param name="output">The writer for deserializing.</param>
        /// <exception cref="ArgumentNullException"><paramref name="response" /> or <paramref name="output" /> is <see langword="null" />.</exception>
        /// <exception cref="JsonException">An error occurred during JSON serialization.</exception>
        /// <exception cref="JsonRpcSerializationException">An error occurred during JSON-RPC serialization.</exception>
        public void SerializeResponse(JsonRpcResponse response, TextWriter output)
        {
            if (response == null)
            {
                throw new ArgumentNullException(nameof(response));
            }
            if (output == null)
            {
                throw new ArgumentNullException(nameof(output));
            }

            using (var jsonWriter = new JsonTextWriter(output))
            {
                SetupJsonWriter(jsonWriter);
                SerializeResponse(jsonWriter, response);
            }
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

            using (var stringWriter = new StringWriter(new StringBuilder(_messageBufferSize), CultureInfo.InvariantCulture))
            {
                await SerializeResponseAsync(response, stringWriter, cancellationToken).ConfigureAwait(false);

                return stringWriter.ToString();
            }
        }

        /// <summary>Serializes the specified JSON-RPC response as a UTF-8 encoded JSON string to the specified stream as an asynchronous operation.</summary>
        /// <param name="response">The JSON-RPC response to serialize.</param>
        /// <param name="output">The stream to deserialize to.</param>
        /// <param name="cancellationToken">The cancellation token for canceling the operation.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="response" /> or <paramref name="output" /> is <see langword="null" />.</exception>
        /// <exception cref="JsonException">An error occurred during JSON serialization.</exception>
        /// <exception cref="JsonRpcSerializationException">An error occurred during JSON-RPC serialization.</exception>
        /// <exception cref="OperationCanceledException">The operation was canceled.</exception>
        public ValueTask SerializeResponseAsync(JsonRpcResponse response, Stream output, CancellationToken cancellationToken = default)
        {
            if (response == null)
            {
                throw new ArgumentNullException(nameof(response));
            }
            if (output == null)
            {
                throw new ArgumentNullException(nameof(output));
            }

            cancellationToken.ThrowIfCancellationRequested();

            using (var streamWriter = new StreamWriter(output, _defaultJsonEncoding, _defaultStreamBufferSize, true))
            {
                return SerializeResponseAsync(response, streamWriter, cancellationToken);
            }
        }

        /// <summary>Serializes the specified JSON-RPC response as a JSON string using the specified writer as an asynchronous operation.</summary>
        /// <param name="response">The JSON-RPC response to serialize.</param>
        /// <param name="output">The writer for deserializing.</param>
        /// <param name="cancellationToken">The cancellation token for canceling the operation.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="response" /> or <paramref name="output" /> is <see langword="null" />.</exception>
        /// <exception cref="JsonException">An error occurred during JSON serialization.</exception>
        /// <exception cref="JsonRpcSerializationException">An error occurred during JSON-RPC serialization.</exception>
        /// <exception cref="OperationCanceledException">The operation was canceled.</exception>
        public ValueTask SerializeResponseAsync(JsonRpcResponse response, TextWriter output, CancellationToken cancellationToken = default)
        {
            if (response == null)
            {
                throw new ArgumentNullException(nameof(response));
            }
            if (output == null)
            {
                throw new ArgumentNullException(nameof(output));
            }

            cancellationToken.ThrowIfCancellationRequested();

            using (var jsonWriter = new JsonTextWriter(output))
            {
                SetupJsonWriter(jsonWriter);
                SerializeResponse(jsonWriter, response);
            }

            return default;
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

            using (var stringWriter = new StringWriter(new StringBuilder(_messageBufferSize * responses.Count), CultureInfo.InvariantCulture))
            {
                SerializeResponses(responses, stringWriter);

                return stringWriter.ToString();
            }
        }

        /// <summary>Serializes the specified collection of JSON-RPC responses as a UTF-8 encoded JSON string to the specified stream.</summary>
        /// <param name="responses">The collection of JSON-RPC responses to serialize.</param>
        /// <param name="output">The stream to deserialize to.</param>
        /// <exception cref="ArgumentNullException"><paramref name="responses" /> or <paramref name="output" /> is <see langword="null" />.</exception>
        /// <exception cref="JsonException">An error occurred during JSON serialization.</exception>
        /// <exception cref="JsonRpcSerializationException">An error occurred during JSON-RPC serialization.</exception>
        public void SerializeResponses(IReadOnlyList<JsonRpcResponse> responses, Stream output)
        {
            if (responses == null)
            {
                throw new ArgumentNullException(nameof(responses));
            }
            if (output == null)
            {
                throw new ArgumentNullException(nameof(output));
            }

            using (var streamWriter = new StreamWriter(output, _defaultJsonEncoding, _defaultStreamBufferSize, true))
            {
                SerializeResponses(responses, streamWriter);
            }
        }

        /// <summary>Serializes the specified collection of JSON-RPC responses as a JSON string using the specified writer.</summary>
        /// <param name="responses">The collection of JSON-RPC responses to serialize.</param>
        /// <param name="output">The writer for deserializing.</param>
        /// <exception cref="ArgumentNullException"><paramref name="responses" /> or <paramref name="output" /> is <see langword="null" />.</exception>
        /// <exception cref="JsonException">An error occurred during JSON serialization.</exception>
        /// <exception cref="JsonRpcSerializationException">An error occurred during JSON-RPC serialization.</exception>
        public void SerializeResponses(IReadOnlyList<JsonRpcResponse> responses, TextWriter output)
        {
            if (responses == null)
            {
                throw new ArgumentNullException(nameof(responses));
            }
            if (output == null)
            {
                throw new ArgumentNullException(nameof(output));
            }

            using (var jsonWriter = new JsonTextWriter(output))
            {
                SetupJsonWriter(jsonWriter);
                SerializeResponses(jsonWriter, responses, default);
            }
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

            using (var stringWriter = new StringWriter(new StringBuilder(_messageBufferSize * responses.Count), CultureInfo.InvariantCulture))
            {
                await SerializeResponsesAsync(responses, stringWriter, cancellationToken).ConfigureAwait(false);

                return stringWriter.ToString();
            }
        }

        /// <summary>Serializes the specified collection of JSON-RPC responses as a UTF-8 encoded JSON string to the specified stream as an asynchronous operation.</summary>
        /// <param name="responses">The collection of JSON-RPC responses to serialize.</param>
        /// <param name="output">The stream to deserialize to.</param>
        /// <param name="cancellationToken">The cancellation token for canceling the operation.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="responses" /> or <paramref name="output" /> is <see langword="null" />.</exception>
        /// <exception cref="JsonException">An error occurred during JSON serialization.</exception>
        /// <exception cref="JsonRpcSerializationException">An error occurred during JSON-RPC serialization.</exception>
        /// <exception cref="OperationCanceledException">The operation was canceled.</exception>
        public ValueTask SerializeResponsesAsync(IReadOnlyList<JsonRpcResponse> responses, Stream output, CancellationToken cancellationToken = default)
        {
            if (responses == null)
            {
                throw new ArgumentNullException(nameof(responses));
            }
            if (output == null)
            {
                throw new ArgumentNullException(nameof(output));
            }

            cancellationToken.ThrowIfCancellationRequested();

            using (var streamWriter = new StreamWriter(output, _defaultJsonEncoding, _defaultStreamBufferSize, true))
            {
                return SerializeResponsesAsync(responses, streamWriter, cancellationToken);
            }
        }

        /// <summary>Serializes the specified collection of JSON-RPC responses as a JSON string using the specified writer as an asynchronous operation.</summary>
        /// <param name="responses">The collection of JSON-RPC responses to serialize.</param>
        /// <param name="output">The writer for deserializing.</param>
        /// <param name="cancellationToken">The cancellation token for canceling the operation.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="responses" /> or <paramref name="output" /> is <see langword="null" />.</exception>
        /// <exception cref="JsonException">An error occurred during JSON serialization.</exception>
        /// <exception cref="JsonRpcSerializationException">An error occurred during JSON-RPC serialization.</exception>
        /// <exception cref="OperationCanceledException">The operation was canceled.</exception>
        public ValueTask SerializeResponsesAsync(IReadOnlyList<JsonRpcResponse> responses, TextWriter output, CancellationToken cancellationToken = default)
        {
            if (responses == null)
            {
                throw new ArgumentNullException(nameof(responses));
            }
            if (output == null)
            {
                throw new ArgumentNullException(nameof(output));
            }

            cancellationToken.ThrowIfCancellationRequested();

            using (var jsonWriter = new JsonTextWriter(output))
            {
                SetupJsonWriter(jsonWriter);
                SerializeResponses(jsonWriter, responses, cancellationToken);
            }

            return default;
        }
    }
}
