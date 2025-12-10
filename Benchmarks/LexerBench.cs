using BenchmarkDotNet.Attributes;
using YSharp.Core;
using YSharp.Types.Common;
using YSharp.Types.Lexer;

namespace YSharp.Benchmarks;

[SimpleJob]
[MemoryDiagnoser]
public class LexerBench
{
    public bool x;

    [Benchmark]
    public void LexerBenchmarkL()
    {
        Lexer lexer = new(BenchHelp.Text50000Char, "BENCHMARK");
        (List<IToken>, Error) tokens = lexer.MakeTokens();
        x = tokens.Item2.IsError;
    }

    [Benchmark]
    public void LexerBenchmarkM()
    {
        Lexer lexer = new(BenchHelp.Text10000Char, "BENCHMARK");
        (List<IToken>, Error) tokens = lexer.MakeTokens();
        x = tokens.Item2.IsError;
    }

    [Benchmark]
    public void LexerBenchmarkS()
    {
        Lexer lexer = new(BenchHelp.Text5000Char, "BENCHMARK");
        (List<IToken>, Error) tokens = lexer.MakeTokens();
        x = tokens.Item2.IsError;
    }

    [Benchmark]
    public void LexerBenchmarkXL()
    {
        Lexer lexer = new(BenchHelp.Text100000Char, "BENCHMARK");
        (List<IToken>, Error) tokens = lexer.MakeTokens();
        x = tokens.Item2.IsError;
    }
}