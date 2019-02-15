using System;
using System.Collections.Generic;
using Anemonis.JsonRpc.Benchmarks.Resources;
using BenchmarkDotNet.Attributes;

namespace Anemonis.JsonRpc.Benchmarks.TestSuites
{
    public class JsonRpcSerializerDeserializeBenchmarks
    {
        private static readonly IReadOnlyDictionary<string, string> _resources = CreateResourceDictionary();
        private static readonly IReadOnlyDictionary<string, JsonRpcSerializer> _serializers = CreatSerializers();

        private static IReadOnlyDictionary<string, string> CreateResourceDictionary()
        {
            var resources = new Dictionary<string, string>(StringComparer.Ordinal);

            foreach (var code in GetResourceCodes())
            {
                resources[code] = EmbeddedResourceManager.GetString($"Assets.{code}.json");
            }

            return resources;
        }

        private static JsonRpcSerializer CreateSerializerRequestParamsNone()
        {
            var resolver = new JsonRpcContractResolver();
            var serializer = new JsonRpcSerializer(resolver);

            resolver.AddRequestContract("m", new JsonRpcRequestContract());

            return serializer;
        }

        private static JsonRpcSerializer CreateSerializerRequestParamsByName()
        {
            var resolver = new JsonRpcContractResolver();
            var serializer = new JsonRpcSerializer(resolver);

            var parameters = new Dictionary<string, Type>
            {
                ["p"] = typeof(long)
            };

            resolver.AddRequestContract("m", new JsonRpcRequestContract(parameters));

            return serializer;
        }

        private static JsonRpcSerializer CreateSerializerRequestParamsByPosition()
        {
            var resolver = new JsonRpcContractResolver();
            var serializer = new JsonRpcSerializer(resolver);

            var parameters = new[]
            {
                typeof(long)
            };

            resolver.AddRequestContract("m", new JsonRpcRequestContract(parameters));

            return serializer;
        }

        private static JsonRpcSerializer CreateSerializerResponse()
        {
            var resolver = new JsonRpcContractResolver();
            var serializer = new JsonRpcSerializer(resolver);

            resolver.AddResponseContract(0L, new JsonRpcResponseContract(typeof(long), typeof(long)));

            return serializer;
        }

        private static IReadOnlyDictionary<string, JsonRpcSerializer> CreatSerializers()
        {
            return new Dictionary<string, JsonRpcSerializer>
            {
                ["request_params_by_name"] = CreateSerializerRequestParamsByName(),
                ["request_params_by_position"] = CreateSerializerRequestParamsByPosition(),
                ["request_params_none"] = CreateSerializerRequestParamsNone(),
                ["response"] = CreateSerializerResponse(),
            };
        }

        private static IEnumerable<string> GetResourceCodes()
        {
            return new[]
            {
                "request_params_by_name",
                "request_params_by_name_batch",
                "request_params_by_position",
                "request_params_by_position_batch",
                "request_params_none",
                "request_params_none_batch",
                "response_error",
                "response_error_batch",
                "response_error_with_data",
                "response_error_with_data_batch",
                "response_success",
                "response_success_batch"
            };
        }

        [Benchmark(Description = "DeserializeRequestData-PARAMS=U-BATCH=N")]
        public object DeserializeRequestDataParamsNone()
        {
            return _serializers["request_params_none"].DeserializeRequestData(_resources["request_params_none"]);
        }

        [Benchmark(Description = "DeserializeRequestData-PARAMS=U-BATCH=Y")]
        public object DeserializeRequestDataParamsNoneBatch()
        {
            return _serializers["request_params_none"].DeserializeRequestData(_resources["request_params_none_batch"]);
        }

        [Benchmark(Description = "DeserializeRequestData-PARAMS=N-BATCH=N")]
        public object DeserializeRequestDataParamsByName()
        {
            return _serializers["request_params_by_name"].DeserializeRequestData(_resources["request_params_by_name"]);
        }

        [Benchmark(Description = "DeserializeRequestData-PARAMS=N-BATCH=Y")]
        public object DeserializeRequestDataParamsByNameBatch()
        {
            return _serializers["request_params_by_name"].DeserializeRequestData(_resources["request_params_by_name_batch"]);
        }

        [Benchmark(Description = "DeserializeRequestData-PARAMS=P-BATCH=N")]
        public object DeserializeRequestDataParamsByPosition()
        {
            return _serializers["request_params_by_position"].DeserializeRequestData(_resources["request_params_by_position"]);
        }

        [Benchmark(Description = "DeserializeRequestData-PARAMS=P-BATCH=Y")]
        public object DeserializeRequestDataaramsByPositionBatchP()
        {
            return _serializers["request_params_by_position"].DeserializeRequestData(_resources["request_params_by_position_batch"]);
        }

        [Benchmark(Description = "DeserializeResponseData-ERROR=N-DATA=Y-BATCH=N")]
        public object DeserializeResponseDataSuccess()
        {
            return _serializers["response"].DeserializeResponseData(_resources["response_success"]);
        }

        [Benchmark(Description = "DeserializeResponseData-ERROR=N-DATA=Y-BATCH=Y")]
        public object DeserializeResponseDataSuccessBatch()
        {
            return _serializers["response"].DeserializeResponseData(_resources["response_success_batch"]);
        }

        [Benchmark(Description = "DeserializeResponseData-ERROR=Y-DATA=N-BATCH=N")]
        public object DeserializeResponseDataError()
        {
            return _serializers["response"].DeserializeResponseData(_resources["response_error"]);
        }

        [Benchmark(Description = "DeserializeResponseData-ERROR=Y-DATA=N-BATCH=Y")]
        public object DeserializeResponseDataErrorBatch()
        {
            return _serializers["response"].DeserializeResponseData(_resources["response_error_batch"]);
        }

        [Benchmark(Description = "DeserializeResponseData-ERROR=Y-DATA=Y-BATCH=N")]
        public object DeserializeResponseDataErrorData()
        {
            return _serializers["response"].DeserializeResponseData(_resources["response_error_with_data"]);
        }

        [Benchmark(Description = "DeserializeResponseData-ERROR=Y-DATA=Y-BATCH=Y")]
        public object DeserializeResponseDataErrorDataBatch()
        {
            return _serializers["response"].DeserializeResponseData(_resources["response_error_with_data_batch"]);
        }
    }
}