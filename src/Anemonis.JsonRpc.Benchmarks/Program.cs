using System.Linq;
using Anemonis.JsonRpc.Benchmarks.Framework;
using Anemonis.JsonRpc.Benchmarks.TestSuites;
using BenchmarkDotNet.Columns;
using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Diagnosers;
using BenchmarkDotNet.Horology;
using BenchmarkDotNet.Jobs;
using BenchmarkDotNet.Loggers;
using BenchmarkDotNet.Reports;
using BenchmarkDotNet.Running;
using BenchmarkDotNet.Toolchains.InProcess.Emit;

namespace Anemonis.JsonRpc.Benchmarks
{
    public static class Program
    {
        public static void Main()
        {
            var configuration = ManualConfig.CreateEmpty();

            configuration.Add(Job.Default.With(InProcessEmitToolchain.Instance));
            configuration.Add(MemoryDiagnoser.Default);
            configuration.Add(DefaultConfig.Instance.GetColumnProviders().ToArray());
            configuration.Add(ConsoleLogger.Default);
            configuration.Add(new SimpleBenchmarkExporter());
            configuration.SummaryStyle = SummaryStyle.Default.WithTimeUnit(TimeUnit.Nanosecond).WithSizeUnit(SizeUnit.B);

            BenchmarkRunner.Run<JsonRpcIdBenchmarks>(configuration);
            BenchmarkRunner.Run<JsonRpcSerializerSerializeBenchmarks>(configuration);
            BenchmarkRunner.Run<JsonRpcSerializerDeserializeBenchmarks>(configuration);
        }
    }
}