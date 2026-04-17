using BenchmarkDotNet.Attributes;
using YSharp.Common;
using YSharp.Util;

namespace YSharp.Benchmarks;


public class RunTimeBench
{

    [Benchmark]
    public void RTBenchmarkL()
    {
        RuntimeEnviroment runClass = new();
        runClass.Run("BENCHMARK", BenchHelp.Text50000Char, CliArgs.DefaultArgs);
    }

    [Benchmark]
    public void RTBenchmarkM()
    {
        RuntimeEnviroment runClass = new();
        runClass.Run("BENCHMARK", BenchHelp.Text10000Char, CliArgs.DefaultArgs);
    }

    [Benchmark]
    public void RTBenchmarkS()
    {
        RuntimeEnviroment runClass = new();
        runClass.Run("BENCHMARK", BenchHelp.Text5000Char, CliArgs.DefaultArgs);
    }

    [Benchmark]
    public void RTBenchmarkXL()
    {
        RuntimeEnviroment runClass = new();
        runClass.Run("BENCHMARK", BenchHelp.Text100000Char, CliArgs.DefaultArgs);
    }
}
