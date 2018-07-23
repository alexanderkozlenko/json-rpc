using System.Data.JsonRpc.Benchmarks.Framework;
using System.Data.JsonRpc.Benchmarks.TestSuites;
using System.Linq;
using BenchmarkDotNet.Columns;
using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Diagnosers;
using BenchmarkDotNet.Horology;
using BenchmarkDotNet.Jobs;
using BenchmarkDotNet.Loggers;
using BenchmarkDotNet.Reports;
using BenchmarkDotNet.Running;
using BenchmarkDotNet.Toolchains.InProcess;

namespace System.Data.JsonRpc.Benchmarks
{
    public static class Program
    {
        public static void Main()
        {
            var configuration = ManualConfig.CreateEmpty();

            configuration.Add(Job.Default.With(InProcessToolchain.Instance));
            configuration.Add(MemoryDiagnoser.Default);
            configuration.Add(DefaultConfig.Instance.GetColumnProviders().ToArray());
            configuration.Add(ConsoleLogger.Default);
            configuration.Add(new SimpleBenchmarkExporter());
            configuration.Set(SummaryStyle.Default.WithTimeUnit(TimeUnit.Nanosecond).WithSizeUnit(SizeUnit.B));

            BenchmarkRunner.Run<JsonRpcIdBenchmarks>(configuration);
            BenchmarkRunner.Run<JsonRpcSerializerSerializeBenchmarks>(configuration);
            BenchmarkRunner.Run<JsonRpcSerializerDeserializeBenchmarks>(configuration);
        }
    }
}