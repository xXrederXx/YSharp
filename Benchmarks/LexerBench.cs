using BenchmarkDotNet.Attributes;
using YSharp.Common;
using YSharp.Lexer;

namespace YSharp.Benchmarks;

using LexerResult = Result<List<BaseToken>, Error>;

public class LexerBench
{
    public LexerResult x;

    [Benchmark]
    public void LexerBenchmarkL()
    {
        Lexer.Lexer lexer = new(BenchHelp.Text50000Char, "BENCHMARK");
        LexerResult res = lexer.MakeTokens();
        x = res;
    }

    [Benchmark]
    public void LexerBenchmarkM()
    {
        Lexer.Lexer lexer = new(BenchHelp.Text10000Char, "BENCHMARK");
        LexerResult res = lexer.MakeTokens();
        x = res;
    }

    [Benchmark]
    public void LexerBenchmarkS()
    {
        Lexer.Lexer lexer = new(BenchHelp.Text5000Char, "BENCHMARK");
        LexerResult res = lexer.MakeTokens();
        x = res;
    }

    [Benchmark]
    public void LexerBenchmarkXL()
    {
        Lexer.Lexer lexer = new(BenchHelp.Text100000Char, "BENCHMARK");
        LexerResult res = lexer.MakeTokens();
        x = res;
    }
}
