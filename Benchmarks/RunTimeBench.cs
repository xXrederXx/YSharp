using BenchmarkDotNet.Attributes;
using YSharp.Common;
using YSharp.Util;

namespace YSharp.Benchmarks;


public class RunTimeBench
{
    
    [Benchmark]
    public void RTBenchmarkL()
    {
        RunClass runClass = new();
        runClass.Run("BENCHMARK", BenchHelp.Text50000Char, CliArgs.DefaultArgs);
    }

    [Benchmark]
    public void RTBenchmarkM()
    {
        RunClass runClass = new();
        runClass.Run("BENCHMARK", BenchHelp.Text10000Char, CliArgs.DefaultArgs);
    }

    [Benchmark]
    public void RTBenchmarkS()
    {
        RunClass runClass = new();
        runClass.Run("BENCHMARK", BenchHelp.Text5000Char, CliArgs.DefaultArgs);
    }

    [Benchmark]
    public void RTBenchmarkXL()
    {
        RunClass runClass = new();
        runClass.Run("BENCHMARK", BenchHelp.Text100000Char, CliArgs.DefaultArgs);
    }
}
