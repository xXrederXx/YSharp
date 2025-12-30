using YSharp.Parser;
using BenchmarkDotNet.Attributes;

namespace YSharp.Benchmarks;

public class ParserBench
{
    [Benchmark]
    public void ParserBenchmarkL()
    {
        Parser.Parser parser = new(BenchHelp.TokenL);
        ParseResult tokens = parser.Parse();
    }

    [Benchmark]
    public void ParserBenchmarkM()
    {
        Parser.Parser parser = new(BenchHelp.TokenM);
        ParseResult tokens = parser.Parse();
    }

    [Benchmark]
    public void ParserBenchmarkS()
    {
        Parser.Parser parser = new(BenchHelp.TokenS);
        ParseResult tokens = parser.Parse();
    }

    [Benchmark]
    public void ParserBenchmarkXL()
    {
        Parser.Parser parser = new(BenchHelp.TokenXL);
        ParseResult tokens = parser.Parse();
    }
}