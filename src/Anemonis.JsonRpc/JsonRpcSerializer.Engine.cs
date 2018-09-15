// © Alexander Kozlenko. Licensed under the MIT License.

using System.Collections.Generic;
using Anemonis.JsonRpc.Resources;
using System.Threading;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;

namespace Anemonis.JsonRpc
{
    public partial class JsonRpcSerializer
    {
        private static readonly JsonLoadSettings _jsonLoadSettings = CreateJsonLoadSettings();
        private static readonly JsonSerializerSettings _jsonSerializerSettings = CreateJsonSerializerSettings();

        private readonly IJsonRpcContractResolver _contractResolver;
        private readonly JsonSerializer _jsonSerializer;
        private readonly JsonRpcCompatibilityLevel _compatibilityLevel;

        private static JsonLoadSettings CreateJsonLoadSettings()
        {
            return new JsonLoadSettings
            {
                LineInfoHandling = LineInfoHandling.Ignore
            };
        }

        private static JsonSerializerSettings CreateJsonSerializerSettings()
        {
            return new JsonSerializerSettings
            {
                MetadataPropertyHandling = MetadataPropertyHandling.Ignore
            };
        }

        private JsonRpcData<JsonRpcRequest> DeserializeRequestData(JsonTextReader reader, CancellationToken cancellationToken = default)
        {
            if (_contractResolver == null)
            {
                throw new InvalidOperationException(Strings.GetString("core.deserialize.resolver.undefined"));
            }

            var messages = default(List<JsonRpcMessageInfo<JsonRpcRequest>>);

            while (reader.Read())
            {
                if (reader.TokenType == JsonToken.StartObject)
                {
                    if (messages == null)
                    {
                        return new JsonRpcData<JsonRpcRequest>(DeserializeRequest(reader));
                    }
                    else
                    {
                        cancellationToken.ThrowIfCancellationRequested();
                        messages.Add(DeserializeRequest(reader));
                    }
                }
                else if (reader.TokenType == JsonToken.StartArray)
                {
                    messages = new List<JsonRpcMessageInfo<JsonRpcRequest>>();
                }
                else if (reader.TokenType == JsonToken.EndArray)
                {
                    if (messages.Count == 0)
                    {
                        throw new JsonRpcSerializationException(JsonRpcErrorCode.InvalidMessage, Strings.GetString("core.batch.empty"));
                    }

                    return new JsonRpcData<JsonRpcRequest>(messages.ToArray());
                }
                else
                {
                    if (messages == null)
                    {
                        break;
                    }
                    else
                    {
                        cancellationToken.ThrowIfCancellationRequested();

                        var exceptionMessage = string.Format(Strings.GetString("core.batch.invalid_item"), messages.Count);
                        var exception = new JsonRpcSerializationException(JsonRpcErrorCode.InvalidMessage, exceptionMessage);

                        messages.Add(new JsonRpcMessageInfo<JsonRpcRequest>(exception));
                    }
                }
            }

            if (reader.TokenType == JsonToken.None)
            {
                throw new JsonReaderException(Strings.GetString("core.deserialize.json_issue"));
            }
            else
            {
                throw new JsonRpcSerializationException(JsonRpcErrorCode.InvalidMessage, Strings.GetString("core.deserialize.input.invalid_structure"));
            }
        }

        private JsonRpcMessageInfo<JsonRpcRequest> DeserializeRequest(JsonTextReader reader)
        {
            try
            {
                var jsonPropertyName = default(string);
                var requestProtocolVersion = default(string);
                var requestMethod = default(string);
                var requestId = default(JsonRpcId);
                var requestParamsetersToken = default(JContainer);

                while (reader.Read())
                {
                    if (reader.TokenType == JsonToken.PropertyName)
                    {
                        jsonPropertyName = (string)reader.Value;
                        reader.Read();

                        switch (jsonPropertyName)
                        {
                            case "jsonrpc":
                                {
                                    if (_compatibilityLevel != JsonRpcCompatibilityLevel.Level1)
                                    {
                                        if (reader.TokenType != JsonToken.String)
                                        {
                                            throw new JsonRpcSerializationException(JsonRpcErrorCode.InvalidMessage, Strings.GetString("core.deserialize.request.protocol.invalid_property"));
                                        }

                                        requestProtocolVersion = (string)reader.Value;
                                    }
                                }
                                break;
                            case "method":
                                {
                                    if (reader.TokenType != JsonToken.String)
                                    {
                                        throw new JsonRpcSerializationException(JsonRpcErrorCode.InvalidMessage, Strings.GetString("core.deserialize.request.method.invalid_property"));
                                    }

                                    requestMethod = (string)reader.Value;
                                }
                                break;
                            case "id":
                                {
                                    switch (reader.TokenType)
                                    {
                                        case JsonToken.Null:
                                            {
                                            }
                                            break;
                                        case JsonToken.String:
                                            {
                                                requestId = new JsonRpcId((string)reader.Value);
                                            }
                                            break;
                                        case JsonToken.Integer:
                                            {
                                                requestId = new JsonRpcId((long)reader.Value);
                                            }
                                            break;
                                        case JsonToken.Float:
                                            {
                                                requestId = new JsonRpcId((double)reader.Value);
                                            }
                                            break;
                                        default:
                                            {
                                                throw new JsonRpcSerializationException(JsonRpcErrorCode.InvalidMessage, Strings.GetString("core.deserialize.request.id.invalid_property"));
                                            }
                                    }
                                }
                                break;
                            case "params":
                                {
                                    switch (reader.TokenType)
                                    {
                                        case JsonToken.StartArray:
                                            {
                                                requestParamsetersToken = JArray.Load(reader, _jsonLoadSettings);
                                            }
                                            break;
                                        case JsonToken.StartObject:
                                            {
                                                requestParamsetersToken = JObject.Load(reader, _jsonLoadSettings);
                                            }
                                            break;
                                        default:
                                            {
                                                throw new JsonRpcSerializationException(JsonRpcErrorCode.InvalidMessage, Strings.GetString("core.deserialize.request.params.invalid_property"), requestId);
                                            }
                                    }
                                }
                                break;
                            default:
                                {
                                    reader.Skip();
                                }
                                break;
                        }
                    }
                    else if (reader.TokenType == JsonToken.EndObject)
                    {
                        break;
                    }
                    else
                    {
                        reader.Skip();
                    }
                }

                if (_compatibilityLevel != JsonRpcCompatibilityLevel.Level1)
                {
                    if (requestProtocolVersion != "2.0")
                    {
                        throw new JsonRpcSerializationException(JsonRpcErrorCode.InvalidMessage, Strings.GetString("core.deserialize.request.protocol.invalid_property"), requestId);
                    }
                }
                if (requestMethod == null)
                {
                    throw new JsonRpcSerializationException(JsonRpcErrorCode.InvalidMessage, Strings.GetString("core.deserialize.request.method.invalid_property"), requestId);
                }

                var requestContract = _contractResolver.GetRequestContract(requestMethod);

                if (requestContract == null)
                {
                    var exceptionMessage = string.Format(Strings.GetString("core.deserialize.request.method.unsupported"), requestMethod);

                    throw new JsonRpcSerializationException(JsonRpcErrorCode.InvalidMethod, exceptionMessage, requestId);
                }

                var request = default(JsonRpcRequest);

                switch (requestContract.ParametersType)
                {
                    case JsonRpcParametersType.ByPosition:
                        {
                            if (!(requestParamsetersToken is JArray requestParametersArrayToken))
                            {
                                throw new JsonRpcSerializationException(JsonRpcErrorCode.InvalidParameters, Strings.GetString("core.deserialize.request.params.invalid_structure"), requestId);
                            }
                            if (requestParametersArrayToken.Count < requestContract.ParametersByPosition.Count)
                            {
                                throw new JsonRpcSerializationException(JsonRpcErrorCode.InvalidParameters, Strings.GetString("core.deserialize.request.params.invalid_count"), requestId);
                            }

                            var requestParameters = new object[requestContract.ParametersByPosition.Count];

                            try
                            {
                                for (var i = 0; i < requestParameters.Length; i++)
                                {
                                    requestParameters[i] = requestParametersArrayToken[i].ToObject(requestContract.ParametersByPosition[i]);
                                }
                            }
                            catch (Exception e)
                            {
                                throw new JsonSerializationException(Strings.GetString("core.deserialize.json_issue"), e);
                            }

                            request = new JsonRpcRequest(requestMethod, requestId, requestParameters);
                        }
                        break;
                    case JsonRpcParametersType.ByName:
                        {
                            if (!(requestParamsetersToken is JObject requestParametersObjectToken))
                            {
                                throw new JsonRpcSerializationException(JsonRpcErrorCode.InvalidParameters, Strings.GetString("core.deserialize.request.params.invalid_structure"), requestId);
                            }

                            var requestParameters = new Dictionary<string, object>(requestContract.ParametersByName.Count, StringComparer.Ordinal);

                            try
                            {
                                foreach (var kvp in requestContract.ParametersByName)
                                {
                                    if (!requestParametersObjectToken.TryGetValue(kvp.Key, StringComparison.Ordinal, out var requestParameterToken))
                                    {
                                        continue;
                                    }

                                    requestParameters[kvp.Key] = requestParameterToken.ToObject(kvp.Value);
                                }
                            }
                            catch (Exception e)
                            {
                                throw new JsonSerializationException(Strings.GetString("core.deserialize.json_issue"), e);
                            }

                            request = new JsonRpcRequest(requestMethod, requestId, requestParameters);
                        }
                        break;
                    default:
                        {
                            request = new JsonRpcRequest(requestMethod, requestId);
                        }
                        break;
                }

                return new JsonRpcMessageInfo<JsonRpcRequest>(request);
            }
            catch (JsonRpcSerializationException e)
            {
                return new JsonRpcMessageInfo<JsonRpcRequest>(e);
            }
        }

        private JsonRpcData<JsonRpcResponse> DeserializeResponseData(JsonTextReader reader, CancellationToken cancellationToken = default)
        {
            if (_contractResolver == null)
            {
                throw new InvalidOperationException(Strings.GetString("core.deserialize.resolver.undefined"));
            }

            var messages = default(List<JsonRpcMessageInfo<JsonRpcResponse>>);

            while (reader.Read())
            {
                if (reader.TokenType == JsonToken.StartObject)
                {
                    if (messages == null)
                    {
                        return new JsonRpcData<JsonRpcResponse>(DeserializeResponse(reader));
                    }
                    else
                    {
                        cancellationToken.ThrowIfCancellationRequested();
                        messages.Add(DeserializeResponse(reader));
                    }
                }
                else if (reader.TokenType == JsonToken.StartArray)
                {
                    messages = new List<JsonRpcMessageInfo<JsonRpcResponse>>();
                }
                else if (reader.TokenType == JsonToken.EndArray)
                {
                    if (messages.Count == 0)
                    {
                        throw new JsonRpcSerializationException(JsonRpcErrorCode.InvalidMessage, Strings.GetString("core.batch.empty"));
                    }

                    return new JsonRpcData<JsonRpcResponse>(messages.ToArray());
                }
                else
                {
                    if (messages == null)
                    {
                        break;
                    }
                    else
                    {
                        cancellationToken.ThrowIfCancellationRequested();

                        var exceptionMessage = string.Format(Strings.GetString("core.batch.invalid_item"), messages.Count);
                        var exception = new JsonRpcSerializationException(JsonRpcErrorCode.InvalidMessage, exceptionMessage);

                        messages.Add(new JsonRpcMessageInfo<JsonRpcResponse>(exception));
                    }
                }
            }

            if (reader.TokenType == JsonToken.None)
            {
                throw new JsonReaderException(Strings.GetString("core.deserialize.json_issue"));
            }
            else
            {
                throw new JsonRpcSerializationException(JsonRpcErrorCode.InvalidMessage, Strings.GetString("core.deserialize.input.invalid_structure"));
            }
        }

        private JsonRpcMessageInfo<JsonRpcResponse> DeserializeResponse(JsonTextReader reader)
        {
            try
            {
                var jsonPropertyName = default(string);
                var responseProtocolVersion = default(string);
                var responseId = default(JsonRpcId);
                var responseResultSet = false;
                var responseResultToken = default(JToken);
                var responseErrorSet = false;
                var responseErrorSetAsNull = false;
                var responseErrorCode = default(long?);
                var responseErrorMessage = default(string);
                var responseErrorDataToken = default(JToken);

                while (reader.Read())
                {
                    if (reader.TokenType == JsonToken.PropertyName)
                    {
                        jsonPropertyName = (string)reader.Value;
                        reader.Read();

                        switch (jsonPropertyName)
                        {
                            case "jsonrpc":
                                {
                                    if (_compatibilityLevel != JsonRpcCompatibilityLevel.Level1)
                                    {
                                        if (reader.TokenType != JsonToken.String)
                                        {
                                            throw new JsonRpcSerializationException(JsonRpcErrorCode.InvalidMessage, Strings.GetString("core.deserialize.response.protocol.invalid_property"));
                                        }

                                        responseProtocolVersion = (string)reader.Value;
                                    }
                                }
                                break;
                            case "id":
                                {
                                    switch (reader.TokenType)
                                    {
                                        case JsonToken.Null:
                                            {
                                            }
                                            break;
                                        case JsonToken.String:
                                            {
                                                responseId = new JsonRpcId((string)reader.Value);
                                            }
                                            break;
                                        case JsonToken.Integer:
                                            {
                                                responseId = new JsonRpcId((long)reader.Value);
                                            }
                                            break;
                                        case JsonToken.Float:
                                            {
                                                responseId = new JsonRpcId((double)reader.Value);
                                            }
                                            break;
                                        default:
                                            {
                                                throw new JsonRpcSerializationException(JsonRpcErrorCode.InvalidMessage, Strings.GetString("core.deserialize.response.id.invalid_property"));
                                            }
                                    }
                                }
                                break;
                            case "result":
                                {
                                    responseResultSet = true;
                                    responseResultToken = JToken.ReadFrom(reader, _jsonLoadSettings);
                                }
                                break;
                            case "error":
                                {
                                    responseErrorSet = true;

                                    switch (reader.TokenType)
                                    {
                                        case JsonToken.StartObject:
                                            {
                                                while (reader.Read())
                                                {
                                                    if (reader.TokenType == JsonToken.PropertyName)
                                                    {
                                                        jsonPropertyName = (string)reader.Value;
                                                        reader.Read();

                                                        switch (jsonPropertyName)
                                                        {
                                                            case "code":
                                                                {
                                                                    if (reader.TokenType != JsonToken.Integer)
                                                                    {
                                                                        if (_compatibilityLevel != JsonRpcCompatibilityLevel.Level1)
                                                                        {
                                                                            throw new JsonRpcSerializationException(JsonRpcErrorCode.InvalidMessage, Strings.GetString("core.deserialize.response.error.code.invalid_property"));
                                                                        }
                                                                    }
                                                                    else
                                                                    {
                                                                        responseErrorCode = (long)reader.Value;
                                                                    }
                                                                }
                                                                break;
                                                            case "message":
                                                                {
                                                                    if (reader.TokenType != JsonToken.String)
                                                                    {
                                                                        if (_compatibilityLevel != JsonRpcCompatibilityLevel.Level1)
                                                                        {
                                                                            throw new JsonRpcSerializationException(JsonRpcErrorCode.InvalidMessage, Strings.GetString("core.deserialize.response.error.message.invalid_property"));
                                                                        }
                                                                    }
                                                                    else
                                                                    {
                                                                        responseErrorMessage = (string)reader.Value;
                                                                    }
                                                                }
                                                                break;
                                                            case "data":
                                                                {
                                                                    responseErrorDataToken = JToken.ReadFrom(reader, _jsonLoadSettings);
                                                                }
                                                                break;
                                                            default:
                                                                {
                                                                    reader.Skip();
                                                                }
                                                                break;
                                                        }
                                                    }
                                                    else if (reader.TokenType == JsonToken.EndObject)
                                                    {
                                                        break;
                                                    }
                                                    else
                                                    {
                                                        reader.Skip();
                                                    }
                                                }
                                            }
                                            break;
                                        case JsonToken.Null:
                                            {
                                                responseErrorSetAsNull = true;
                                            }
                                            break;
                                        default:
                                            {
                                                reader.Skip();
                                            }
                                            break;
                                    }
                                }
                                break;
                            default:
                                {
                                    reader.Skip();
                                }
                                break;
                        }
                    }
                    else if (reader.TokenType == JsonToken.EndObject)
                    {
                        break;
                    }
                    else
                    {
                        reader.Skip();
                    }
                }

                var responseSuccess = false;

                if (_compatibilityLevel != JsonRpcCompatibilityLevel.Level1)
                {
                    if (responseProtocolVersion != "2.0")
                    {
                        throw new JsonRpcSerializationException(JsonRpcErrorCode.InvalidMessage, Strings.GetString("core.deserialize.response.protocol.invalid_property"), responseId);
                    }
                    if (!(responseResultSet ^ responseErrorSet))
                    {
                        throw new JsonRpcSerializationException(JsonRpcErrorCode.InvalidMessage, Strings.GetString("core.deserialize.response.invalid_properties"), responseId);
                    }

                    responseSuccess = responseResultSet;
                }
                else
                {
                    if (!responseResultSet || !responseErrorSet)
                    {
                        throw new JsonRpcSerializationException(JsonRpcErrorCode.InvalidMessage, Strings.GetString("core.deserialize.response.invalid_properties"), responseId);
                    }

                    responseSuccess = responseErrorSetAsNull;
                }

                var response = default(JsonRpcResponse);

                if (responseSuccess)
                {
                    if (responseId.Type == JsonRpcIdType.None)
                    {
                        throw new JsonRpcSerializationException(JsonRpcErrorCode.InvalidMessage, Strings.GetString("core.deserialize.response.invalid_properties"), responseId);
                    }

                    var responseContract = _contractResolver.GetResponseContract(responseId);

                    if (responseContract == null)
                    {
                        throw new JsonRpcSerializationException(JsonRpcErrorCode.InvalidMethod, Strings.GetString("core.deserialize.response.method.contract.undefined"), responseId);
                    }

                    var responseResult = default(object);

                    if (responseContract.ResultType != null)
                    {
                        try
                        {
                            responseResult = responseResultToken.ToObject(responseContract.ResultType);
                        }
                        catch (Exception e)
                        {
                            throw new JsonSerializationException(Strings.GetString("core.deserialize.json_issue"), e);
                        }
                    }

                    response = new JsonRpcResponse(responseResult, responseId);
                }
                else
                {
                    var responseError = default(JsonRpcError);

                    if (!responseErrorSetAsNull)
                    {
                        if (_compatibilityLevel != JsonRpcCompatibilityLevel.Level1)
                        {
                            if (responseErrorCode == null)
                            {
                                throw new JsonRpcSerializationException(JsonRpcErrorCode.InvalidMessage, Strings.GetString("core.deserialize.response.error.code.invalid_property"), responseId);
                            }
                            if (responseErrorMessage == null)
                            {
                                throw new JsonRpcSerializationException(JsonRpcErrorCode.InvalidMessage, Strings.GetString("core.deserialize.response.error.message.invalid_property"), responseId);
                            }
                        }
                        else
                        {
                            if (responseErrorCode == null)
                            {
                                responseErrorCode = 0L;
                            }
                            if (responseErrorMessage == null)
                            {
                                responseErrorMessage = string.Empty;
                            }
                        }

                        if (responseErrorDataToken != null)
                        {
                            var responseErrorDataType = default(Type);

                            if (responseId.Type == JsonRpcIdType.None)
                            {
                                responseErrorDataType = _contractResolver.GetResponseContract(default)?.ErrorDataType;
                            }
                            else
                            {
                                var responseContract = _contractResolver.GetResponseContract(responseId);

                                if (responseContract == null)
                                {
                                    throw new JsonRpcSerializationException(JsonRpcErrorCode.InvalidMethod, Strings.GetString("core.deserialize.response.method.contract.undefined"), responseId);
                                }

                                responseErrorDataType = responseContract.ErrorDataType;
                            }

                            var responseErrorData = default(object);

                            if (responseErrorDataType != null)
                            {
                                try
                                {
                                    responseErrorData = responseErrorDataToken.ToObject(responseErrorDataType);
                                }
                                catch (Exception e)
                                {
                                    throw new JsonSerializationException(Strings.GetString("core.deserialize.json_issue"), e);
                                }
                            }

                            try
                            {
                                responseError = new JsonRpcError(responseErrorCode.Value, responseErrorMessage, responseErrorData);
                            }
                            catch (ArgumentOutOfRangeException e)
                            {
                                throw new JsonRpcSerializationException(JsonRpcErrorCode.InvalidMessage, Strings.GetString("core.deserialize.response.error.code.invalid_range"), responseId, e);
                            }
                        }
                        else
                        {
                            try
                            {
                                responseError = new JsonRpcError(responseErrorCode.Value, responseErrorMessage);
                            }
                            catch (ArgumentOutOfRangeException e)
                            {
                                throw new JsonRpcSerializationException(JsonRpcErrorCode.InvalidMessage, Strings.GetString("core.deserialize.response.error.code.invalid_range"), responseId, e);
                            }
                        }
                    }
                    else
                    {
                        if (_compatibilityLevel != JsonRpcCompatibilityLevel.Level1)
                        {
                            throw new JsonRpcSerializationException(JsonRpcErrorCode.InvalidMessage, Strings.GetString("core.deserialize.response.error.invalid_type"), responseId);
                        }
                        else
                        {
                            responseError = new JsonRpcError(default, string.Empty);
                        }
                    }

                    response = new JsonRpcResponse(responseError, responseId);
                }

                return new JsonRpcMessageInfo<JsonRpcResponse>(response);
            }
            catch (JsonRpcSerializationException e)
            {
                return new JsonRpcMessageInfo<JsonRpcResponse>(e);
            }
        }

        private void SerializeRequests(JsonTextWriter writer, IReadOnlyList<JsonRpcRequest> requests, CancellationToken cancellationToken = default)
        {
            if (requests.Count == 0)
            {
                throw new JsonRpcSerializationException(JsonRpcErrorCode.InvalidMessage, Strings.GetString("core.batch.empty"));
            }

            writer.WriteStartArray();

            for (var i = 0; i < requests.Count; i++)
            {
                if (requests[i] == null)
                {
                    throw new JsonRpcSerializationException(JsonRpcErrorCode.InvalidMessage, string.Format(Strings.GetString("core.batch.invalid_item"), i));
                }

                cancellationToken.ThrowIfCancellationRequested();

                SerializeRequest(writer, requests[i]);
            }

            writer.WriteEndArray();
        }

        private void SerializeRequest(JsonTextWriter writer, JsonRpcRequest request)
        {
            writer.WriteStartObject();

            if (_compatibilityLevel != JsonRpcCompatibilityLevel.Level1)
            {
                writer.WritePropertyName("jsonrpc");
                writer.WriteValue("2.0");
            }

            ref readonly var requestId = ref request.Id;

            switch (requestId.Type)
            {
                case JsonRpcIdType.None:
                    {
                        if (_compatibilityLevel == JsonRpcCompatibilityLevel.Level1)
                        {
                            writer.WritePropertyName("id");
                            writer.WriteNull();
                        }
                    }
                    break;
                case JsonRpcIdType.String:
                    {
                        writer.WritePropertyName("id");
                        writer.WriteValue(requestId.UnsafeAsString());
                    }
                    break;
                case JsonRpcIdType.Integer:
                    {
                        writer.WritePropertyName("id");
                        writer.WriteValue(requestId.UnsafeAsInteger());
                    }
                    break;
                case JsonRpcIdType.Float:
                    {
                        writer.WritePropertyName("id");
                        writer.WriteValue(requestId.UnsafeAsFloat());
                    }
                    break;
            }

            writer.WritePropertyName("method");
            writer.WriteValue(request.Method);

            switch (request.ParametersType)
            {
                case JsonRpcParametersType.ByPosition:
                    {
                        if (request.ParametersByPosition.Count != 0)
                        {
                            writer.WritePropertyName("params");
                            writer.WriteStartArray();

                            try
                            {
                                for (var i = 0; i < request.ParametersByPosition.Count; i++)
                                {
                                    _jsonSerializer.Serialize(writer, request.ParametersByPosition[i]);
                                }
                            }
                            catch (Exception e)
                            {
                                throw new JsonSerializationException(Strings.GetString("core.serialize.json_issue"), e);
                            }

                            writer.WriteEndArray();
                        }
                        else
                        {
                            if (_compatibilityLevel == JsonRpcCompatibilityLevel.Level1)
                            {
                                writer.WritePropertyName("params");
                                writer.WriteStartArray();
                                writer.WriteEndArray();
                            }
                        }
                    }
                    break;
                case JsonRpcParametersType.ByName:
                    {
                        if (_compatibilityLevel == JsonRpcCompatibilityLevel.Level1)
                        {
                            throw new JsonRpcSerializationException(JsonRpcErrorCode.InvalidMessage, Strings.GetString("core.serialize.request.params.unsupported_structure"), requestId);
                        }

                        writer.WritePropertyName("params");
                        writer.WriteStartObject();

                        try
                        {
                            foreach (var kvp in request.ParametersByName)
                            {
                                writer.WritePropertyName(kvp.Key);

                                _jsonSerializer.Serialize(writer, kvp.Value);
                            }
                        }
                        catch (Exception e)
                        {
                            throw new JsonSerializationException(Strings.GetString("core.serialize.json_issue"), e);
                        }

                        writer.WriteEndObject();
                    }
                    break;
                case JsonRpcParametersType.None:
                    {
                        if (_compatibilityLevel == JsonRpcCompatibilityLevel.Level1)
                        {
                            writer.WritePropertyName("params");
                            writer.WriteStartArray();
                            writer.WriteEndArray();
                        }
                    }
                    break;
            }

            writer.WriteEndObject();
        }

        private void SerializeResponses(JsonTextWriter writer, IReadOnlyList<JsonRpcResponse> responses, CancellationToken cancellationToken = default)
        {
            if (responses.Count == 0)
            {
                throw new JsonRpcSerializationException(JsonRpcErrorCode.InvalidMessage, Strings.GetString("core.batch.empty"));
            }

            writer.WriteStartArray();

            for (var i = 0; i < responses.Count; i++)
            {
                if (responses[i] == null)
                {
                    throw new JsonRpcSerializationException(JsonRpcErrorCode.InvalidMessage, string.Format(Strings.GetString("core.batch.invalid_item"), i));
                }

                cancellationToken.ThrowIfCancellationRequested();

                SerializeResponse(writer, responses[i]);
            }

            writer.WriteEndArray();
        }

        private void SerializeResponse(JsonWriter writer, JsonRpcResponse response)
        {
            writer.WriteStartObject();

            if (_compatibilityLevel != JsonRpcCompatibilityLevel.Level1)
            {
                writer.WritePropertyName("jsonrpc");
                writer.WriteValue("2.0");
            }

            ref readonly var responseId = ref response.Id;

            switch (responseId.Type)
            {
                case JsonRpcIdType.None:
                    {
                        writer.WritePropertyName("id");
                        writer.WriteNull();
                    }
                    break;
                case JsonRpcIdType.String:
                    {
                        writer.WritePropertyName("id");
                        writer.WriteValue(responseId.UnsafeAsString());
                    }
                    break;
                case JsonRpcIdType.Integer:
                    {
                        writer.WritePropertyName("id");
                        writer.WriteValue(responseId.UnsafeAsInteger());
                    }
                    break;
                case JsonRpcIdType.Float:
                    {
                        writer.WritePropertyName("id");
                        writer.WriteValue(responseId.UnsafeAsFloat());
                    }
                    break;
            }

            if (response.Success)
            {
                writer.WritePropertyName("result");

                try
                {
                    _jsonSerializer.Serialize(writer, response.Result);
                }
                catch (Exception e)
                {
                    throw new JsonSerializationException(Strings.GetString("core.serialize.json_issue"), e);
                }

                if (_compatibilityLevel == JsonRpcCompatibilityLevel.Level1)
                {
                    writer.WritePropertyName("error");
                    writer.WriteNull();
                }
            }
            else
            {
                if (_compatibilityLevel == JsonRpcCompatibilityLevel.Level1)
                {
                    writer.WritePropertyName("result");
                    writer.WriteNull();
                }

                writer.WritePropertyName("error");
                writer.WriteStartObject();
                writer.WritePropertyName("code");
                writer.WriteValue(response.Error.Code);
                writer.WritePropertyName("message");
                writer.WriteValue(response.Error.Message);

                if (response.Error.HasData)
                {
                    writer.WritePropertyName("data");

                    try
                    {
                        _jsonSerializer.Serialize(writer, response.Error.Data);
                    }
                    catch (Exception e)
                    {
                        throw new JsonSerializationException(Strings.GetString("core.serialize.json_issue"), e);
                    }
                }

                writer.WriteEndObject();
            }

            writer.WriteEndObject();
        }
    }
}