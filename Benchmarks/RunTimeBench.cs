using System;
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
/*
| Method          | Mean      | Error    | StdDev   | Gen0   | Gen1   | Allocated |
|---------------- |----------:|---------:|---------:|-------:|-------:|----------:|
| Benchmark5000   |  28.78 us | 0.540 us | 0.530 us | 6.1340 | 0.7629 | 100.23 KB |
| Benchmark10000  |  37.08 us | 0.699 us | 0.687 us | 6.1035 | 0.7935 | 100.23 KB |
| Benchmark50000  |  99.31 us | 1.948 us | 2.464 us | 6.1035 | 0.7324 | 100.23 KB |
| Benchmark100000 | 170.43 us | 2.569 us | 2.403 us | 6.1035 | 0.7324 | 100.23 KB | */
