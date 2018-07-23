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

        [Benchmark(Description = "CompareTo-TYPE=N")]
        public int CompareToNone()
        {
            return _idNone.CompareTo(_idNone);
        }

        [Benchmark(Description = "CompareTo-TYPE=S")]
        public int CompareToString()
        {
            return _idString.CompareTo(_idString);
        }

        [Benchmark(Description = "CompareTo-TYPE=I")]
        public int CompareToInteger()
        {
            return _idInteger.CompareTo(_idInteger);
        }

        [Benchmark(Description = "CompareTo-TYPE=F")]
        public int CompareToFloat()
        {
            return _idFloat.CompareTo(_idFloat);
        }

        [Benchmark(Description = "ToString-TYPE=N")]
        public string ToStringNone()
        {
            return _idNone.ToString();
        }

        [Benchmark(Description = "ToString-TYPE=S")]
        public string ToStringString()
        {
            return _idString.ToString();
        }

        [Benchmark(Description = "ToString-TYPE=I")]
        public string ToStringInteger()
        {
            return _idInteger.ToString();
        }

        [Benchmark(Description = "ToString-TYPE=F")]
        public string ToStringFloat()
        {
            return _idFloat.ToString();
        }
    }
}