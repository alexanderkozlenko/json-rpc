// © Alexander Kozlenko. Licensed under the MIT License.

using System.Collections.Generic;
using System.Data.JsonRpc.Resources;
using System.Globalization;
using System.Threading;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace System.Data.JsonRpc
{
    public partial class JsonRpcSerializer
    {
        private static readonly JsonLoadSettings _jsonLoadSettings = CreateJsonLoadSettings();
        private static readonly JsonSerializer _jsonSerializer = JsonSerializer.CreateDefault();

        private static JsonLoadSettings CreateJsonLoadSettings()
        {
            var jsonLoadSettings = new JsonLoadSettings();

            jsonLoadSettings.CommentHandling = CommentHandling.Ignore;
            jsonLoadSettings.LineInfoHandling = LineInfoHandling.Ignore;

            return jsonLoadSettings;
        }

        private JsonRpcData<JsonRpcRequest> DeserializeRequestData(JsonTextReader reader, CancellationToken cancellationToken = default)
        {
            var itemsBag = default(LinkedList<JsonRpcItem<JsonRpcRequest>>);

            while (reader.Read())
            {
                if (reader.TokenType == JsonToken.StartObject)
                {
                    if (itemsBag == null)
                    {
                        return new JsonRpcData<JsonRpcRequest>(DeserializeRequest(reader));
                    }
                    else
                    {
                        cancellationToken.ThrowIfCancellationRequested();
                        itemsBag.AddLast(DeserializeRequest(reader));
                    }
                }
                else if (reader.TokenType == JsonToken.StartArray)
                {
                    itemsBag = new LinkedList<JsonRpcItem<JsonRpcRequest>>();
                }
                else if (reader.TokenType == JsonToken.EndArray)
                {
                    if (itemsBag.Count == 0)
                    {
                        throw new JsonRpcException(JsonRpcErrorCodes.InvalidMessage, Strings.GetString("core.batch.empty"));
                    }

                    var items = new JsonRpcItem<JsonRpcRequest>[itemsBag.Count];

                    itemsBag.CopyTo(items, 0);

                    return new JsonRpcData<JsonRpcRequest>(items);
                }
                else
                {
                    if (itemsBag == null)
                    {
                        break;
                    }
                    else
                    {
                        cancellationToken.ThrowIfCancellationRequested();

                        var exceptionMessage = string.Format(CultureInfo.InvariantCulture, Strings.GetString("core.batch.invalid_item"), itemsBag.Count);
                        var exception = new JsonRpcException(JsonRpcErrorCodes.InvalidMessage, exceptionMessage);

                        itemsBag.AddLast(new JsonRpcItem<JsonRpcRequest>(exception));
                    }
                }
            }

            if (reader.TokenType == JsonToken.None)
            {
                throw new JsonRpcException(JsonRpcErrorCodes.InvalidJson, Strings.GetString("core.deserialize.json_issue"));
            }
            else
            {
                throw new JsonRpcException(JsonRpcErrorCodes.InvalidMessage, Strings.GetString("core.deserialize.input.invalid_structure"));
            }
        }

        private JsonRpcItem<JsonRpcRequest> DeserializeRequest(JsonTextReader reader)
        {
            try
            {
                var jsonPropertyName = default(string);
                var requestProtocolVersion = default(string);
                var requestMethod = default(string);
                var requestId = default(JsonRpcId);
                var requestParamsetersToken = default(JToken);

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
                                    if (_compatibilityLevel == JsonRpcCompatibilityLevel.Level2)
                                    {
                                        if (reader.TokenType != JsonToken.String)
                                        {
                                            throw new JsonRpcException(JsonRpcErrorCodes.InvalidMessage, Strings.GetString("core.deserialize.request.protocol.invalid_property"));
                                        }

                                        requestProtocolVersion = (string)reader.Value;
                                    }
                                }
                                break;
                            case "method":
                                {
                                    if (reader.TokenType != JsonToken.String)
                                    {
                                        throw new JsonRpcException(JsonRpcErrorCodes.InvalidMessage, Strings.GetString("core.deserialize.request.method.invalid_property"));
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
                                                requestId = (string)reader.Value;
                                            }
                                            break;
                                        case JsonToken.Integer:
                                            {
                                                requestId = (long)reader.Value;
                                            }
                                            break;
                                        case JsonToken.Float:
                                            {
                                                requestId = (double)reader.Value;
                                            }
                                            break;
                                        default:
                                            {
                                                throw new JsonRpcException(JsonRpcErrorCodes.InvalidMessage, Strings.GetString("core.deserialize.request.id.invalid_property"));
                                            }
                                    }
                                }
                                break;
                            case "params":
                                {
                                    requestParamsetersToken = JToken.ReadFrom(reader, _jsonLoadSettings);
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

                if (_compatibilityLevel == JsonRpcCompatibilityLevel.Level2)
                {
                    if (requestProtocolVersion != "2.0")
                    {
                        throw new JsonRpcException(JsonRpcErrorCodes.InvalidMessage, Strings.GetString("core.deserialize.request.protocol.invalid_property"), requestId);
                    }
                }
                if (requestMethod == null)
                {
                    throw new JsonRpcException(JsonRpcErrorCodes.InvalidMessage, Strings.GetString("core.deserialize.request.method.invalid_property"), requestId);
                }
                if (!_requestContracts.TryGetValue(requestMethod, out var requestContract))
                {
                    var exceptionMessage = string.Format(CultureInfo.InvariantCulture, Strings.GetString("core.deserialize.request.method.unsupported"), requestMethod);

                    throw new JsonRpcException(JsonRpcErrorCodes.InvalidMethod, exceptionMessage, requestId);
                }
                if (requestContract == null)
                {
                    var exceptionMessage = string.Format(CultureInfo.InvariantCulture, Strings.GetString("core.deserialize.request.method.contract.undefined"), requestMethod);

                    throw new JsonRpcException(JsonRpcErrorCodes.InvalidOperation, exceptionMessage, requestId);
                }
                if (requestParamsetersToken != null)
                {
                    if ((requestParamsetersToken.Type != JTokenType.Array) && (requestParamsetersToken.Type != JTokenType.Object))
                    {
                        throw new JsonRpcException(JsonRpcErrorCodes.InvalidMessage, Strings.GetString("core.deserialize.request.params.invalid_property"), requestId);
                    }
                }

                switch (requestContract.ParametersType)
                {
                    case JsonRpcParametersType.ByPosition:
                        {
                            if (requestParamsetersToken.Type != JTokenType.Array)
                            {
                                throw new JsonRpcException(JsonRpcErrorCodes.InvalidParameters, Strings.GetString("core.deserialize.request.params.invalid_structure"), requestId);
                            }

                            var requestParamsetersArrayToken = (JArray)requestParamsetersToken;

                            if (requestParamsetersArrayToken.Count < requestContract.ParametersByPosition.Count)
                            {
                                var exceptionMessage = string.Format(CultureInfo.InvariantCulture, Strings.GetString("core.deserialize.request.params.invalid_count"), requestParamsetersArrayToken.Count);

                                throw new JsonRpcException(JsonRpcErrorCodes.InvalidParameters, exceptionMessage, requestId);
                            }

                            var requestParameters = new object[requestContract.ParametersByPosition.Count];

                            try
                            {
                                for (var i = 0; i < requestParameters.Length; i++)
                                {
                                    requestParameters[i] = requestParamsetersArrayToken[i].ToObject(requestContract.ParametersByPosition[i]);
                                }
                            }
                            catch (Exception e)
                            {
                                throw new JsonRpcException(JsonRpcErrorCodes.InvalidOperation, Strings.GetString("core.deserialize.json_issue"), requestId, e);
                            }

                            var request = new JsonRpcRequest(requestMethod, requestId, requestParameters);

                            return new JsonRpcItem<JsonRpcRequest>(request);
                        }
                    case JsonRpcParametersType.ByName:
                        {
                            if (requestParamsetersToken.Type != JTokenType.Object)
                            {
                                throw new JsonRpcException(JsonRpcErrorCodes.InvalidParameters, Strings.GetString("core.deserialize.request.params.invalid_structure"), requestId);
                            }

                            var requestParametersObjectToken = (JObject)requestParamsetersToken;
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
                                throw new JsonRpcException(JsonRpcErrorCodes.InvalidOperation, Strings.GetString("core.deserialize.json_issue"), requestId, e);
                            }

                            var request = new JsonRpcRequest(requestMethod, requestId, requestParameters);

                            return new JsonRpcItem<JsonRpcRequest>(request);
                        }
                    default:
                        {
                            var request = new JsonRpcRequest(requestMethod, requestId);

                            return new JsonRpcItem<JsonRpcRequest>(request);
                        }
                }
            }
            catch (JsonRpcException e)
               when (e.ErrorCode != JsonRpcErrorCodes.InvalidOperation)
            {
                return new JsonRpcItem<JsonRpcRequest>(e);
            }
        }

        private JsonRpcData<JsonRpcResponse> DeserializeResponseData(JsonTextReader reader, CancellationToken cancellationToken = default)
        {
            var itemsBag = default(LinkedList<JsonRpcItem<JsonRpcResponse>>);

            while (reader.Read())
            {
                if (reader.TokenType == JsonToken.StartObject)
                {
                    if (itemsBag == null)
                    {
                        return new JsonRpcData<JsonRpcResponse>(DeserializeResponse(reader));
                    }
                    else
                    {
                        cancellationToken.ThrowIfCancellationRequested();
                        itemsBag.AddLast(DeserializeResponse(reader));
                    }
                }
                else if (reader.TokenType == JsonToken.StartArray)
                {
                    itemsBag = new LinkedList<JsonRpcItem<JsonRpcResponse>>();
                }
                else if (reader.TokenType == JsonToken.EndArray)
                {
                    if (itemsBag.Count == 0)
                    {
                        throw new JsonRpcException(JsonRpcErrorCodes.InvalidMessage, Strings.GetString("core.batch.empty"));
                    }

                    var items = new JsonRpcItem<JsonRpcResponse>[itemsBag.Count];

                    itemsBag.CopyTo(items, 0);

                    return new JsonRpcData<JsonRpcResponse>(items);
                }
                else
                {
                    if (itemsBag == null)
                    {
                        break;
                    }
                    else
                    {
                        cancellationToken.ThrowIfCancellationRequested();

                        var exceptionMessage = string.Format(CultureInfo.InvariantCulture, Strings.GetString("core.batch.invalid_item"), itemsBag.Count);
                        var exception = new JsonRpcException(JsonRpcErrorCodes.InvalidMessage, exceptionMessage);

                        itemsBag.AddLast(new JsonRpcItem<JsonRpcResponse>(exception));
                    }
                }
            }

            if (reader.TokenType == JsonToken.None)
            {
                throw new JsonRpcException(JsonRpcErrorCodes.InvalidJson, Strings.GetString("core.deserialize.json_issue"));
            }
            else
            {
                throw new JsonRpcException(JsonRpcErrorCodes.InvalidMessage, Strings.GetString("core.deserialize.input.invalid_structure"));
            }
        }

        private JsonRpcItem<JsonRpcResponse> DeserializeResponse(JsonTextReader reader)
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
                                    if (_compatibilityLevel == JsonRpcCompatibilityLevel.Level2)
                                    {
                                        if (reader.TokenType != JsonToken.String)
                                        {
                                            throw new JsonRpcException(JsonRpcErrorCodes.InvalidMessage, Strings.GetString("core.deserialize.response.protocol.invalid_property"));
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
                                                responseId = (string)reader.Value;
                                            }
                                            break;
                                        case JsonToken.Integer:
                                            {
                                                responseId = (long)reader.Value;
                                            }
                                            break;
                                        case JsonToken.Float:
                                            {
                                                responseId = (double)reader.Value;
                                            }
                                            break;
                                        default:
                                            {
                                                throw new JsonRpcException(JsonRpcErrorCodes.InvalidMessage, Strings.GetString("core.deserialize.response.id.invalid_property"));
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
                                                                        if (_compatibilityLevel == JsonRpcCompatibilityLevel.Level2)
                                                                        {
                                                                            throw new JsonRpcException(JsonRpcErrorCodes.InvalidMessage, Strings.GetString("core.deserialize.response.error.code.invalid_property"));
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
                                                                        if (_compatibilityLevel == JsonRpcCompatibilityLevel.Level2)
                                                                        {
                                                                            throw new JsonRpcException(JsonRpcErrorCodes.InvalidMessage, Strings.GetString("core.deserialize.response.error.message.invalid_property"));
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

                if (_compatibilityLevel == JsonRpcCompatibilityLevel.Level2)
                {
                    if (responseProtocolVersion != "2.0")
                    {
                        throw new JsonRpcException(JsonRpcErrorCodes.InvalidMessage, Strings.GetString("core.deserialize.response.protocol.invalid_property"), responseId);
                    }
                    if (!(responseResultSet ^ responseErrorSet))
                    {
                        throw new JsonRpcException(JsonRpcErrorCodes.InvalidMessage, Strings.GetString("core.deserialize.response.invalid_properties"), responseId);
                    }

                    responseSuccess = responseResultSet;
                }
                else
                {
                    if (!responseResultSet || !responseErrorSet)
                    {
                        throw new JsonRpcException(JsonRpcErrorCodes.InvalidMessage, Strings.GetString("core.deserialize.response.invalid_properties"), responseId);
                    }

                    responseSuccess = responseErrorSetAsNull;
                }

                if (responseSuccess)
                {
                    if (responseId.Type == JsonRpcIdType.None)
                    {
                        throw new JsonRpcException(JsonRpcErrorCodes.InvalidMessage, Strings.GetString("core.deserialize.response.invalid_properties"), responseId);
                    }

                    var responseContract = GetResponseContract(responseId);
                    var responseResult = default(object);

                    if (responseContract.ResultType != null)
                    {
                        try
                        {
                            responseResult = responseResultToken.ToObject(responseContract.ResultType);
                        }
                        catch (Exception e)
                        {
                            throw new JsonRpcException(JsonRpcErrorCodes.InvalidOperation, Strings.GetString("core.deserialize.json_issue"), responseId, e);
                        }
                    }

                    var response = new JsonRpcResponse(responseResult, responseId);

                    return new JsonRpcItem<JsonRpcResponse>(response);
                }
                else
                {
                    if (!responseErrorSetAsNull)
                    {
                        if (_compatibilityLevel == JsonRpcCompatibilityLevel.Level2)
                        {
                            if (responseErrorCode == null)
                            {
                                throw new JsonRpcException(JsonRpcErrorCodes.InvalidMessage, Strings.GetString("core.deserialize.response.error.code.invalid_property"), responseId);
                            }
                            if (responseErrorMessage == null)
                            {
                                throw new JsonRpcException(JsonRpcErrorCodes.InvalidMessage, Strings.GetString("core.deserialize.response.error.message.invalid_property"), responseId);
                            }
                        }

                        if (responseErrorDataToken != null)
                        {
                            var responseErrorDataType = default(Type);

                            if (responseId.Type == JsonRpcIdType.None)
                            {
                                responseErrorDataType = _defaultErrorDataType;
                            }
                            else
                            {
                                var responseContract = GetResponseContract(responseId);

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
                                    throw new JsonRpcException(JsonRpcErrorCodes.InvalidOperation, Strings.GetString("core.deserialize.json_issue"), responseId, e);
                                }
                            }

                            var responseError = default(JsonRpcError);

                            try
                            {
                                responseError = new JsonRpcError(responseErrorCode ?? 0L, responseErrorMessage ?? string.Empty, responseErrorData);
                            }
                            catch (ArgumentOutOfRangeException e)
                            {
                                throw new JsonRpcException(JsonRpcErrorCodes.InvalidMessage, Strings.GetString("core.deserialize.response.error.code.invalid_range"), responseId, e);
                            }

                            var response = new JsonRpcResponse(responseError, responseId);

                            return new JsonRpcItem<JsonRpcResponse>(response);
                        }
                        else
                        {
                            var responseError = default(JsonRpcError);

                            try
                            {
                                responseError = new JsonRpcError(responseErrorCode ?? 0L, responseErrorMessage ?? string.Empty);
                            }
                            catch (ArgumentOutOfRangeException e)
                            {
                                throw new JsonRpcException(JsonRpcErrorCodes.InvalidMessage, Strings.GetString("core.deserialize.response.error.code.invalid_range"), responseId, e);
                            }

                            var response = new JsonRpcResponse(responseError, responseId);

                            return new JsonRpcItem<JsonRpcResponse>(response);
                        }
                    }
                    else
                    {
                        if (_compatibilityLevel == JsonRpcCompatibilityLevel.Level2)
                        {
                            throw new JsonRpcException(JsonRpcErrorCodes.InvalidMessage, Strings.GetString("core.deserialize.response.error.invalid_type"), responseId);
                        }
                        else
                        {
                            var responseError = new JsonRpcError(default, string.Empty);
                            var response = new JsonRpcResponse(responseError, responseId);

                            return new JsonRpcItem<JsonRpcResponse>(response);
                        }
                    }
                }
            }
            catch (JsonRpcException e)
               when (e.ErrorCode != JsonRpcErrorCodes.InvalidOperation)
            {
                return new JsonRpcItem<JsonRpcResponse>(e);
            }
        }

        private void SerializeRequests(JsonTextWriter writer, IReadOnlyList<JsonRpcRequest> requests, CancellationToken cancellationToken = default)
        {
            if (requests.Count == 0)
            {
                throw new JsonRpcException(JsonRpcErrorCodes.InvalidMessage, Strings.GetString("core.batch.empty"));
            }

            writer.WriteStartArray();

            for (var i = 0; i < requests.Count; i++)
            {
                if (requests[i] == null)
                {
                    throw new JsonRpcException(JsonRpcErrorCodes.InvalidMessage, string.Format(CultureInfo.InvariantCulture, Strings.GetString("core.batch.invalid_item"), i));
                }

                cancellationToken.ThrowIfCancellationRequested();

                SerializeRequest(writer, requests[i]);
            }

            writer.WriteEndArray();
        }

        private void SerializeRequest(JsonTextWriter writer, JsonRpcRequest request)
        {
            writer.WriteStartObject();

            if (_compatibilityLevel == JsonRpcCompatibilityLevel.Level2)
            {
                writer.WritePropertyName("jsonrpc");
                writer.WriteValue("2.0");
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
                                    var parameterValue = request.ParametersByPosition[i];

                                    _jsonSerializer.Serialize(writer, parameterValue);
                                }
                            }
                            catch (JsonException e)
                            {
                                throw new JsonRpcException(JsonRpcErrorCodes.InvalidOperation, Strings.GetString("core.serialize.json_issue"), request.Id, e);
                            }

                            writer.WriteEndArray();
                        }
                        else
                        {
                            if (_compatibilityLevel != JsonRpcCompatibilityLevel.Level2)
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
                        if (_compatibilityLevel != JsonRpcCompatibilityLevel.Level2)
                        {
                            throw new JsonRpcException(JsonRpcErrorCodes.InvalidOperation, Strings.GetString("core.serialize.request.params.unsupported_structure"), request.Id);
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
                        catch (JsonException e)
                        {
                            throw new JsonRpcException(JsonRpcErrorCodes.InvalidOperation, Strings.GetString("core.serialize.json_issue"), request.Id, e);
                        }

                        writer.WriteEndObject();
                    }
                    break;
                case JsonRpcParametersType.None:
                    {
                        if (_compatibilityLevel != JsonRpcCompatibilityLevel.Level2)
                        {
                            writer.WritePropertyName("params");
                            writer.WriteStartArray();
                            writer.WriteEndArray();
                        }
                    }
                    break;
            }

            switch (request.Id.Type)
            {
                case JsonRpcIdType.None:
                    {
                        if (_compatibilityLevel != JsonRpcCompatibilityLevel.Level2)
                        {
                            writer.WritePropertyName("id");
                            writer.WriteNull();
                        }
                    }
                    break;
                case JsonRpcIdType.String:
                    {
                        writer.WritePropertyName("id");
                        writer.WriteValue((string)request.Id);
                    }
                    break;
                case JsonRpcIdType.Integer:
                    {
                        writer.WritePropertyName("id");
                        writer.WriteValue((long)request.Id);
                    }
                    break;
                case JsonRpcIdType.Float:
                    {
                        writer.WritePropertyName("id");
                        writer.WriteValue((double)request.Id);
                    }
                    break;
            }

            writer.WriteEndObject();
        }

        private void SerializeResponses(JsonTextWriter writer, IReadOnlyList<JsonRpcResponse> responses, CancellationToken cancellationToken = default)
        {
            if (responses.Count == 0)
            {
                throw new JsonRpcException(JsonRpcErrorCodes.InvalidMessage, Strings.GetString("core.batch.empty"));
            }

            writer.WriteStartArray();

            for (var i = 0; i < responses.Count; i++)
            {
                if (responses[i] == null)
                {
                    throw new JsonRpcException(JsonRpcErrorCodes.InvalidMessage, string.Format(CultureInfo.InvariantCulture, Strings.GetString("core.batch.invalid_item"), i));
                }

                cancellationToken.ThrowIfCancellationRequested();

                SerializeResponse(writer, responses[i]);
            }

            writer.WriteEndArray();
        }

        private void SerializeResponse(JsonWriter writer, JsonRpcResponse response)
        {
            writer.WriteStartObject();

            if (_compatibilityLevel == JsonRpcCompatibilityLevel.Level2)
            {
                writer.WritePropertyName("jsonrpc");
                writer.WriteValue("2.0");
            }

            if (response.Success)
            {
                try
                {
                    writer.WritePropertyName("result");

                    _jsonSerializer.Serialize(writer, response.Result);
                }
                catch (JsonException e)
                {
                    throw new JsonRpcException(JsonRpcErrorCodes.InvalidOperation, Strings.GetString("core.serialize.json_issue"), response.Id, e);
                }

                if (_compatibilityLevel != JsonRpcCompatibilityLevel.Level2)
                {
                    writer.WritePropertyName("error");
                    writer.WriteNull();
                }
            }
            else
            {
                writer.WritePropertyName("error");
                writer.WriteStartObject();
                writer.WritePropertyName("code");
                writer.WriteValue(response.Error.Code);
                writer.WritePropertyName("message");
                writer.WriteValue(response.Error.Message);

                if (response.Error.HasData)
                {
                    try
                    {
                        writer.WritePropertyName("data");

                        _jsonSerializer.Serialize(writer, response.Error.Data);
                    }
                    catch (JsonException e)
                    {
                        throw new JsonRpcException(JsonRpcErrorCodes.InvalidOperation, Strings.GetString("core.serialize.json_issue"), response.Id, e);
                    }
                }

                writer.WriteEndObject();

                if (_compatibilityLevel != JsonRpcCompatibilityLevel.Level2)
                {
                    writer.WritePropertyName("result");
                    writer.WriteNull();
                }
            }

            switch (response.Id.Type)
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
                        writer.WriteValue((string)response.Id);
                    }
                    break;
                case JsonRpcIdType.Integer:
                    {
                        writer.WritePropertyName("id");
                        writer.WriteValue((long)response.Id);
                    }
                    break;
                case JsonRpcIdType.Float:
                    {
                        writer.WritePropertyName("id");
                        writer.WriteValue((double)response.Id);
                    }
                    break;
            }

            writer.WriteEndObject();
        }
    }
}