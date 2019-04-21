using System.Collections.Generic;

using BenchmarkDotNet.Attributes;

namespace Anemonis.JsonRpc.Benchmarks.TestSuites
{
    public class JsonRpcSerializerSerializeBenchmarks
    {
        private static readonly IReadOnlyDictionary<string, JsonRpcRequest> _reqsb0 = CreateRequestDictionary();
        private static readonly IReadOnlyDictionary<string, JsonRpcResponse> _ressb0 = CreateResponseDictionary();
        private static readonly IReadOnlyDictionary<string, IReadOnlyList<JsonRpcRequest>> _reqsb1 = CreateRequestBatchesDictionary();
        private static readonly IReadOnlyDictionary<string, IReadOnlyList<JsonRpcResponse>> _ressb1 = CreateResponseBatchesDictionary();

        private readonly JsonRpcSerializer _serializer = new JsonRpcSerializer();

        private static IReadOnlyDictionary<string, JsonRpcRequest> CreateRequestDictionary()
        {
            return new Dictionary<string, JsonRpcRequest>
            {
                ["req_b0i1p2"] = CreateRequestParamsByName(),
                ["req_b0i1p1"] = CreateRequestParamsByPosition(),
                ["req_b0i1p0"] = CreateRequestParamsNone(),
            };
        }

        private static IReadOnlyDictionary<string, IReadOnlyList<JsonRpcRequest>> CreateRequestBatchesDictionary()
        {
            return new Dictionary<string, IReadOnlyList<JsonRpcRequest>>
            {
                ["req_b1i1p2"] = new[] { CreateRequestParamsByName() },
                ["req_b1i1p1"] = new[] { CreateRequestParamsByPosition() },
                ["req_b1i1p0"] = new[] { CreateRequestParamsNone() },
            };
        }

        private static IReadOnlyDictionary<string, JsonRpcResponse> CreateResponseDictionary()
        {
            return new Dictionary<string, JsonRpcResponse>
            {
                ["res_b0i1e1d0"] = CreateResponseError(),
                ["res_b0i1e1d1"] = CreateResponseErrorWithData(),
                ["res_b0i1e0d0"] = CreateResponseSuccess(),
            };
        }

        private static IReadOnlyDictionary<string, IReadOnlyList<JsonRpcResponse>> CreateResponseBatchesDictionary()
        {
            return new Dictionary<string, IReadOnlyList<JsonRpcResponse>>
            {
                ["res_b1i1e1d0"] = new[] { CreateResponseError() },
                ["res_b1i1e1d1"] = new[] { CreateResponseErrorWithData() },
                ["res_b1i1e0d0"] = new[] { CreateResponseSuccess() },
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
        public object SerializeRequestB0I1P0()
        {
            return _serializer.SerializeRequest(_reqsb0["req_b0i1p0"]);
        }

        [Benchmark(Description = "SerializeRequest-PARAMS=P")]
        public object SerializeRequestB0I1P1()
        {
            return _serializer.SerializeRequest(_reqsb0["req_b0i1p1"]);
        }

        [Benchmark(Description = "SerializeRequest-PARAMS=N")]
        public object SerializeRequestB0I1P2()
        {
            return _serializer.SerializeRequest(_reqsb0["req_b0i1p2"]);
        }

        [Benchmark(Description = "SerializeRequests-PARAMS=U")]
        public object SerializeRequestB1I1P0()
        {
            return _serializer.SerializeRequests(_reqsb1["req_b1i1p0"]);
        }

        [Benchmark(Description = "SerializeRequests-PARAMS=P")]
        public object SerializeRequestB1I1P1()
        {
            return _serializer.SerializeRequests(_reqsb1["req_b1i1p1"]);
        }

        [Benchmark(Description = "SerializeRequests-PARAMS=N")]
        public object SerializeRequestB1I1P2()
        {
            return _serializer.SerializeRequests(_reqsb1["req_b1i1p2"]);
        }

        [Benchmark(Description = "SerializeResponse-ERROR=N-DATA=Y")]
        public object SerializeResponseB0E0D0()
        {
            return _serializer.SerializeResponse(_ressb0["res_b0i1e0d0"]);
        }

        [Benchmark(Description = "SerializeResponse-ERROR=Y-DATA=N")]
        public object SerializeResponseB0E1D0()
        {
            return _serializer.SerializeResponse(_ressb0["res_b0i1e1d0"]);
        }

        [Benchmark(Description = "SerializeResponse-ERROR=Y-DATA=Y")]
        public object SerializeResponseB0E1D1()
        {
            return _serializer.SerializeResponse(_ressb0["res_b0i1e1d1"]);
        }

        [Benchmark(Description = "SerializeResponses-ERROR=N-DATA=Y")]
        public object SerializeResponsesB1E0D0()
        {
            return _serializer.SerializeResponses(_ressb1["res_b1i1e0d0"]);
        }

        [Benchmark(Description = "SerializeResponses-ERROR=Y-DATA=N")]
        public object SerializeResponsesB1E1D0()
        {
            return _serializer.SerializeResponses(_ressb1["res_b1i1e1d0"]);
        }

        [Benchmark(Description = "SerializeResponses-ERROR=Y-DATA=Y")]
        public object SerializeResponsesB1E1D1()
        {
            return _serializer.SerializeResponses(_ressb1["res_b1i1e1d1"]);
        }
    }
}