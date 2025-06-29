using System;
using BenchmarkDotNet.Attributes;
using YSharp.Internal;

namespace YSharp.Benchmarks;

[SimpleJob]
[MemoryDiagnoser]
public class ParserBench
{
    [Benchmark]
    public void BenchmarkS()
    {
        Parser parser = new(BenchHelp.TokenS);
        var tokens = parser.Parse();
    }

    [Benchmark]
    public void BenchmarkM()
    {
        Parser parser = new(BenchHelp.TokenM);
        var tokens = parser.Parse();
    }

    [Benchmark]
    public void BenchmarkL()
    {
        Parser parser = new(BenchHelp.TokenL);
        var tokens = parser.Parse();
    }

    [Benchmark]
    public void BenchmarkXL()
    {
        Parser parser = new(BenchHelp.TokenXL);
        var tokens = parser.Parse();
    }
}
/* 
| Method      | Mean     | Error    | StdDev   | Gen0   | Gen1   | Allocated |
|------------ |---------:|---------:|---------:|-------:|-------:|----------:|
| BenchmarkS  | 12.57 us | 0.158 us | 0.148 us | 3.4637 | 0.1373 |  56.79 KB |
| BenchmarkM  | 12.85 us | 0.186 us | 0.165 us | 3.4637 | 0.1373 |  56.79 KB |
| BenchmarkL  | 12.95 us | 0.065 us | 0.057 us | 3.4637 | 0.1373 |  56.79 KB |
| BenchmarkXL | 12.66 us | 0.186 us | 0.156 us | 3.4637 | 0.1373 |  56.79 KB | */