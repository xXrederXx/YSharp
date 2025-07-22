using BenchmarkDotNet.Attributes;

namespace YSharp.Benchmarks;

[SimpleJob]
[MemoryDiagnoser]
public class RunTimeBench
{
    [Benchmark]
    public void RTBenchmarkS()
    {
        RunClass runClass = new();
        runClass.Run("BENCHMARK", BenchHelp.Text5000Char);
    }

    [Benchmark]
    public void RTBenchmarkM()
    {
        RunClass runClass = new();
        runClass.Run("BENCHMARK", BenchHelp.Text10000Char);
    }

    [Benchmark]
    public void RTBenchmarkL()
    {
        RunClass runClass = new();
        runClass.Run("BENCHMARK", BenchHelp.Text50000Char);
    }

    [Benchmark]
    public void RTBenchmarkXL()
    {
        RunClass runClass = new();
        runClass.Run("BENCHMARK", BenchHelp.Text100000Char);
    }
}
