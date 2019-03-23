using System.Collections.Generic;
using BenchmarkDotNet.Attributes;

namespace Anemonis.JsonRpc.Benchmarks.TestSuites
{
    public class JsonRpcSerializerSerializeBenchmarks
    {
        private static readonly IReadOnlyDictionary<string, JsonRpcRequest> _requests = CreateRequestDictionary();
        private static readonly IReadOnlyDictionary<string, JsonRpcResponse> _responses = CreateResponseDictionary();
        private static readonly IReadOnlyDictionary<string, IReadOnlyList<JsonRpcRequest>> _requestBatches = CreateRequestBatchesDictionary();
        private static readonly IReadOnlyDictionary<string, IReadOnlyList<JsonRpcResponse>> _responseBatches = CreateResponseBatchesDictionary();

        private readonly JsonRpcSerializer _serializer = new JsonRpcSerializer();

        private static IReadOnlyDictionary<string, JsonRpcRequest> CreateRequestDictionary()
        {
            return new Dictionary<string, JsonRpcRequest>
            {
                ["request_params_by_name"] = CreateRequestParamsByName(),
                ["request_params_by_position"] = CreateRequestParamsByPosition(),
                ["request_params_none"] = CreateRequestParamsNone(),
            };
        }

        private static IReadOnlyDictionary<string, IReadOnlyList<JsonRpcRequest>> CreateRequestBatchesDictionary()
        {
            return new Dictionary<string, IReadOnlyList<JsonRpcRequest>>
            {
                ["request_params_by_name"] = new[] { CreateRequestParamsByName() },
                ["request_params_by_position"] = new[] { CreateRequestParamsByPosition() },
                ["request_params_none"] = new[] { CreateRequestParamsNone() },
            };
        }

        private static IReadOnlyDictionary<string, JsonRpcResponse> CreateResponseDictionary()
        {
            return new Dictionary<string, JsonRpcResponse>
            {
                ["response_error"] = CreateResponseError(),
                ["response_error_with_data"] = CreateResponseErrorWithData(),
                ["response_success"] = CreateResponseSuccess(),
            };
        }

        private static IReadOnlyDictionary<string, IReadOnlyList<JsonRpcResponse>> CreateResponseBatchesDictionary()
        {
            return new Dictionary<string, IReadOnlyList<JsonRpcResponse>>
            {
                ["response_error"] = new[] { CreateResponseError() },
                ["response_error_with_data"] = new[] { CreateResponseErrorWithData() },
                ["response_success"] = new[] { CreateResponseSuccess() },
            };
        }

        private static JsonRpcRequest CreateRequestParamsNone()
        {
            return new JsonRpcRequest(0L, "m");
        }

        private static JsonRpcRequest CreateRequestParamsByName()
        {
            var parameters = new Dictionary<string, object>
            {
                ["p"] = 0L
            };

            return new JsonRpcRequest(0L, "m", parameters);
        }

        private static JsonRpcRequest CreateRequestParamsByPosition()
        {
            var parameters = new object[]
            {
                0L
            };

            return new JsonRpcRequest(0L, "m", parameters);
        }

        private static JsonRpcResponse CreateResponseSuccess()
        {
            return new JsonRpcResponse(0L, 0L);
        }

        private static JsonRpcResponse CreateResponseError()
        {
            return new JsonRpcResponse(0L, new JsonRpcError(0L, "m"));
        }

        private static JsonRpcResponse CreateResponseErrorWithData()
        {
            return new JsonRpcResponse(0L, new JsonRpcError(0L, "m", 0L));
        }

        [Benchmark(Description = "SerializeRequest-PARAMS=U")]
        public object SerializeRequestParamsNone()
        {
            return _serializer.SerializeRequest(_requests["request_params_none"]);
        }

        [Benchmark(Description = "SerializeRequest-PARAMS=N")]
        public object SerializeRequestParamsByName()
        {
            return _serializer.SerializeRequest(_requests["request_params_by_name"]);
        }

        [Benchmark(Description = "SerializeRequest-PARAMS=P")]
        public object SerializeRequestParamsByPosition()
        {
            return _serializer.SerializeRequest(_requests["request_params_by_position"]);
        }

        [Benchmark(Description = "SerializeRequests-PARAMS=U")]
        public object SerializeRequestParamsNoneBatch()
        {
            return _serializer.SerializeRequests(_requestBatches["request_params_none"]);
        }

        [Benchmark(Description = "SerializeRequests-PARAMS=N")]
        public object SerializeRequestParamsByNameBatch()
        {
            return _serializer.SerializeRequests(_requestBatches["request_params_by_name"]);
        }

        [Benchmark(Description = "SerializeRequests-PARAMS=P")]
        public object SerializeRequestParamsByPositionBatch()
        {
            return _serializer.SerializeRequests(_requestBatches["request_params_by_position"]);
        }

        [Benchmark(Description = "SerializeResponse-ERROR=N-DATA=Y")]
        public object SerializeResponseSuccess()
        {
            return _serializer.SerializeResponse(_responses["response_success"]);
        }

        [Benchmark(Description = "SerializeResponse-ERROR=Y-DATA=N")]
        public object SerializeResponseError()
        {
            return _serializer.SerializeResponse(_responses["response_error"]);
        }

        [Benchmark(Description = "SerializeResponse-ERROR=Y-DATA=Y")]
        public object SerializeResponseErrorData()
        {
            return _serializer.SerializeResponse(_responses["response_error_with_data"]);
        }

        [Benchmark(Description = "SerializeResponses-ERROR=N-DATA=Y")]
        public object SerializeResponsesSuccess()
        {
            return _serializer.SerializeResponses(_responseBatches["response_success"]);
        }

        [Benchmark(Description = "SerializeResponses-ERROR=Y-DATA=N")]
        public object SerializeResponsesError()
        {
            return _serializer.SerializeResponses(_responseBatches["response_error"]);
        }

        [Benchmark(Description = "SerializeResponses-ERROR=Y-DATA=Y")]
        public object SerializeResponsesErrorData()
        {
            return _serializer.SerializeResponses(_responseBatches["response_error_with_data"]);
        }
    }
}