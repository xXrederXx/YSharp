using System;
using BenchmarkDotNet.Attributes;
using YSharp.Internal;

namespace YSharp.Benchmarks;

public class LexerBench
{
    [Benchmark]
    public void Benchmark5000()
    {
        Lexer lexer = new(BenchHelp.Text5000Char, "BENCHMARK");
        var tokens = lexer.MakeTokens();
    }

    [Benchmark]
    public void Benchmark10000()
    {
        Lexer lexer = new(BenchHelp.Text10000Char, "BENCHMARK");
        var tokens = lexer.MakeTokens();
    }

    [Benchmark]
    public void Benchmark50000()
    {
        Lexer lexer = new(BenchHelp.Text50000Char, "BENCHMARK");
        var tokens = lexer.MakeTokens();
    }

    [Benchmark]
    public void Benchmark100000()
    {
        Lexer lexer = new(BenchHelp.Text100000Char, "BENCHMARK");
        var tokens = lexer.MakeTokens();
    }
}
/* 
| Method          | Mean      | Error    | StdDev   | Median    |
|---------------- |----------:|---------:|---------:|----------:|
| Benchmark5000   |  15.44 us | 0.306 us | 0.511 us |  15.20 us |
| Benchmark10000  |  23.38 us | 0.217 us | 0.192 us |  23.32 us |
| Benchmark50000  |  80.42 us | 0.707 us | 0.661 us |  80.34 us |
| Benchmark100000 | 152.55 us | 1.599 us | 1.496 us | 151.76 us | 
*/