using BenchmarkDotNet.Attributes;

namespace Anemonis.JsonRpc.Benchmarks.TestSuites
{
    public class JsonRpcIdBenchmarks
    {
        private static readonly JsonRpcId _id0 = new JsonRpcId();
        private static readonly JsonRpcId _id1 = new JsonRpcId("1");
        private static readonly JsonRpcId _id2 = new JsonRpcId(1L);
        private static readonly JsonRpcId _id3 = new JsonRpcId(1D);

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

        [Benchmark(Description = "GetHashCode-TYPE=F")]
        public int GetHashCodeFloat()
        {
            return _id3.GetHashCode();
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

        [Benchmark(Description = "Equals-TYPE=F")]
        public bool EqualsFloat()
        {
            return _id3.Equals(_id3);
        }
    }
}
