using BenchmarkDotNet.Attributes;

namespace System.Data.JsonRpc.Benchmarks.TestSuites
{
    public sealed class JsonRpcIdBenchmarks
    {
        private static readonly JsonRpcId _idNone = new JsonRpcId();
        private static readonly JsonRpcId _idString = new JsonRpcId("1");
        private static readonly JsonRpcId _idInteger = new JsonRpcId(1L);
        private static readonly JsonRpcId _idFloat = new JsonRpcId(1D);

        [Benchmark(Description = "GetHashCode-TYPE=N")]
        public int GetHashCodeNone()
        {
            return _idNone.GetHashCode();
        }

        [Benchmark(Description = "GetHashCode-TYPE=S")]
        public int GetHashCodeString()
        {
            return _idString.GetHashCode();
        }

        [Benchmark(Description = "GetHashCode-TYPE=I")]
        public int GetHashCodeInteger()
        {
            return _idInteger.GetHashCode();
        }

        [Benchmark(Description = "GetHashCode-TYPE=F")]
        public int GetHashCodeFloat()
        {
            return _idFloat.GetHashCode();
        }

        [Benchmark(Description = "Equals-TYPE=N")]
        public bool EqualsNone()
        {
            return _idNone.Equals(_idNone);
        }

        [Benchmark(Description = "Equals-TYPE=S")]
        public bool EqualsString()
        {
            return _idString.Equals(_idString);
        }

        [Benchmark(Description = "Equals-TYPE=I")]
        public bool EqualsInteger()
        {
            return _idInteger.Equals(_idInteger);
        }

        [Benchmark(Description = "Equals-TYPE=F")]
        public bool EqualsFloat()
        {
            return _idFloat.Equals(_idFloat);
        }
    }
}