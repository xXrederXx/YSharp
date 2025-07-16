using System;
using BenchmarkDotNet.Attributes;
using YSharp.Internal;

namespace YSharp.Benchmarks;

[SimpleJob]
[MemoryDiagnoser]
public class LexerBench
{
    public bool x;
    [Benchmark]
    public void LexerBenchmarkS()
    {
        Lexer lexer = new(BenchHelp.Text5000Char, "BENCHMARK");
        var tokens = lexer.MakeTokens();
        x = tokens.Item2.IsError;
    }

    [Benchmark]
    public void LexerBenchmarkM()
    {
        Lexer lexer = new(BenchHelp.Text10000Char, "BENCHMARK");
        var tokens = lexer.MakeTokens();
        x = tokens.Item2.IsError;
    }

    [Benchmark]
    public void LexerBenchmarkL()
    {
        Lexer lexer = new(BenchHelp.Text50000Char, "BENCHMARK");
        var tokens = lexer.MakeTokens();
        x = tokens.Item2.IsError;
    }

    [Benchmark]
    public void LexerBenchmarkXL()
    {
        Lexer lexer = new(BenchHelp.Text100000Char, "BENCHMARK");
        var tokens = lexer.MakeTokens();
        x = tokens.Item2.IsError;
    }
}
/* 
| Method          | Mean      | Error    | StdDev   | Gen0   | Gen1   | Allocated |
|---------------- |----------:|---------:|---------:|-------:|-------:|----------:|
| Benchmark5000   |  15.65 us | 0.228 us | 0.263 us | 2.5024 | 0.1831 |  41.34 KB |
| Benchmark10000  |  23.14 us | 0.454 us | 0.637 us | 2.5024 | 0.1831 |  41.34 KB |
| Benchmark50000  |  73.85 us | 0.701 us | 0.621 us | 2.4414 | 0.1221 |  41.34 KB |
| Benchmark100000 | 153.86 us | 0.841 us | 0.702 us | 2.4414 |      - |  41.34 KB | 
*/