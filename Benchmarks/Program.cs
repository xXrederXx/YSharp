using BenchmarkDotNet.Environments;
using YSharp.Benchmarks;

public class Program {
    static void Main(string[] args)
    {
        BenchHelp.Run<LexerBench>();
        BenchHelp.Run<ParserBench>();
        BenchHelp.Run<InterpreterBench>();
        BenchHelp.Run<RunTimeBench>();
    }
}