using System;
using System.Collections.Generic;

using Anemonis.JsonRpc.Resources;

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

        private static JsonRpcSerializer CreateSerializerRequestParams0()
        {
            var resolver = new JsonRpcContractResolver();
            var serializer = new JsonRpcSerializer(resolver);

            resolver.AddRequestContract("m", new JsonRpcRequestContract());

            return serializer;
        }

        private static JsonRpcSerializer CreateSerializerRequestParams2()
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

        private static JsonRpcSerializer CreateSerializerRequestParams1()
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
                ["s0"] = CreateSerializerRequestParams0(),
                ["s1"] = CreateSerializerRequestParams1(),
                ["s2"] = CreateSerializerRequestParams2(),
                ["s3"] = CreateSerializerResponse()
            };
        }

        private static IEnumerable<string> GetResourceCodes()
        {
            return new[]
            {
                "req_b0i1p0",
                "req_b0i1p1",
                "req_b0i1p2",
                "req_b1i1p0",
                "req_b1i1p1",
                "req_b1i1p2",
                "res_b0i1e0d0",
                "res_b0i1e1d0",
                "res_b0i1e1d1",
                "res_b1i1e0d0",
                "res_b1i1e1d0",
                "res_b1i1e1d1"
            };
        }

        [Benchmark(Description = "DeserializeRequestData-PARAMS=U-BATCH=N")]
        public object DeserializeRequestDataB0I1P0()
        {
            return _serializers["s0"].DeserializeRequestData(_resources["req_b0i1p0"]);
        }

        [Benchmark(Description = "DeserializeRequestData-PARAMS=U-BATCH=Y")]
        public object DeserializeRequestDataB1I1P0()
        {
            return _serializers["s0"].DeserializeRequestData(_resources["req_b1i1p0"]);
        }

        [Benchmark(Description = "DeserializeRequestData-PARAMS=P-BATCH=N")]
        public object DeserializeRequestDataB0I1P1()
        {
            return _serializers["s1"].DeserializeRequestData(_resources["req_b0i1p1"]);
        }

        [Benchmark(Description = "DeserializeRequestData-PARAMS=P-BATCH=Y")]
        public object DeserializeRequestDataB1I1P1()
        {
            return _serializers["s1"].DeserializeRequestData(_resources["req_b1i1p1"]);
        }

        [Benchmark(Description = "DeserializeRequestData-PARAMS=N-BATCH=N")]
        public object DeserializeRequestDataB0I1P2()
        {
            return _serializers["s2"].DeserializeRequestData(_resources["req_b0i1p2"]);
        }

        [Benchmark(Description = "DeserializeRequestData-PARAMS=N-BATCH=Y")]
        public object DeserializeRequestDataB1I1P2()
        {
            return _serializers["s2"].DeserializeRequestData(_resources["req_b1i1p2"]);
        }

        [Benchmark(Description = "DeserializeResponseData-ERROR=N-DATA=Y-BATCH=N")]
        public object DeserializeResponseDataB0I1E0D0()
        {
            return _serializers["s3"].DeserializeResponseData(_resources["res_b0i1e0d0"]);
        }

        [Benchmark(Description = "DeserializeResponseData-ERROR=N-DATA=Y-BATCH=Y")]
        public object DeserializeResponseDataB1I1E0D0()
        {
            return _serializers["s3"].DeserializeResponseData(_resources["res_b1i1e0d0"]);
        }

        [Benchmark(Description = "DeserializeResponseData-ERROR=Y-DATA=N-BATCH=N")]
        public object DeserializeResponseDataB0I1E1D0()
        {
            return _serializers["s3"].DeserializeResponseData(_resources["res_b0i1e1d0"]);
        }

        [Benchmark(Description = "DeserializeResponseData-ERROR=Y-DATA=N-BATCH=Y")]
        public object DeserializeResponseDataB1I1E1D0()
        {
            return _serializers["s3"].DeserializeResponseData(_resources["res_b1i1e1d0"]);
        }

        [Benchmark(Description = "DeserializeResponseData-ERROR=Y-DATA=Y-BATCH=N")]
        public object DeserializeResponseDataB0I1E1D1()
        {
            return _serializers["s3"].DeserializeResponseData(_resources["res_b0i1e1d1"]);
        }

        [Benchmark(Description = "DeserializeResponseData-ERROR=Y-DATA=Y-BATCH=Y")]
        public object DeserializeResponseDataB1I1E1D1()
        {
            return _serializers["s3"].DeserializeResponseData(_resources["res_b1i1e1d1"]);
        }
    }
}
