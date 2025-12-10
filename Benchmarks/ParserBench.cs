using BenchmarkDotNet.Attributes;
using YSharp.Core;
using YSharp.Types.AST;

namespace YSharp.Benchmarks;

[SimpleJob]
[MemoryDiagnoser]
public class ParserBench
{
    [Benchmark]
    public void ParserBenchmarkL()
    {
        Parser parser = new(BenchHelp.TokenL);
        ParseResult tokens = parser.Parse();
    }

    [Benchmark]
    public void ParserBenchmarkM()
    {
        Parser parser = new(BenchHelp.TokenM);
        ParseResult tokens = parser.Parse();
    }

    [Benchmark]
    public void ParserBenchmarkS()
    {
        Parser parser = new(BenchHelp.TokenS);
        ParseResult tokens = parser.Parse();
    }

    [Benchmark]
    public void ParserBenchmarkXL()
    {
        Parser parser = new(BenchHelp.TokenXL);
        ParseResult tokens = parser.Parse();
    }
}