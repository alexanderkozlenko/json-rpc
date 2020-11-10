using BenchmarkDotNet.Attributes;

namespace Anemonis.JsonRpc.Benchmarks.TestSuites
{
    public class JsonRpcIdBenchmarks
    {
        private readonly JsonRpcId _id0 = new();
        private readonly JsonRpcId _id1 = new("1");
        private readonly JsonRpcId _id2 = new(1L);

        [Benchmark(Description = "GetHashCode-TYPE=N")]
        public int GetHashCodeNone()
        {
            return _id0.GetHashCode();
        }

        [Benchmark(Description = "GetHashCode-TYPE=S")]
        public int GetHashCodeString()
        {
            return _id1.GetHashCode();
        }

        [Benchmark(Description = "GetHashCode-TYPE=I")]
        public int GetHashCodeInteger()
        {
            return _id2.GetHashCode();
        }

        [Benchmark(Description = "Equals-TYPE=N")]
        public bool EqualsNone()
        {
            return _id0.Equals(_id0);
        }

        [Benchmark(Description = "Equals-TYPE=S")]
        public bool EqualsString()
        {
            return _id1.Equals(_id1);
        }

        [Benchmark(Description = "Equals-TYPE=I")]
        public bool EqualsInteger()
        {
            return _id2.Equals(_id2);
        }
    }
}
