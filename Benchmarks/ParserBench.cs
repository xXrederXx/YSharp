using System;
using BenchmarkDotNet.Attributes;
using YSharp.Internal;

namespace YSharp.Benchmarks;

[SimpleJob]
[MemoryDiagnoser]
public class ParserBench
{
    [Benchmark]
    public void ParserBenchmarkS()
    {
        Parser parser = new(BenchHelp.TokenS);
        var tokens = parser.Parse();
    }

    [Benchmark]
    public void ParserBenchmarkM()
    {
        Parser parser = new(BenchHelp.TokenM);
        var tokens = parser.Parse();
    }

    [Benchmark]
    public void ParserBenchmarkL()
    {
        Parser parser = new(BenchHelp.TokenL);
        var tokens = parser.Parse();
    }

    [Benchmark]
    public void ParserBenchmarkXL()
    {
        Parser parser = new(BenchHelp.TokenXL);
        var tokens = parser.Parse();
    }
}