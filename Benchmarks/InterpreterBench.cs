using BenchmarkDotNet.Attributes;
using YSharp.Core;
using YSharp.Types.Interpreter.Internal;

namespace YSharp.Benchmarks;

[SimpleJob]
[MemoryDiagnoser]
public class InterpreterBench
{
    [Benchmark]
    public void InterpreterBenchmarkS()
    {
        Interpreter.Visit(
            BenchHelp.astS.Node,
            new Context() { symbolTable = BenchHelp.GetSymbolTable() }
        );
    }

    [Benchmark]
    public void InterpreterBenchmarkM()
    {
        Interpreter.Visit(
            BenchHelp.astM.Node,
            new Context() { symbolTable = BenchHelp.GetSymbolTable() }
        );
    }

    [Benchmark]
    public void InterpreterBenchmarkL()
    {
        Interpreter.Visit(
            BenchHelp.astL.Node,
            new Context() { symbolTable = BenchHelp.GetSymbolTable() }
        );
    }

    [Benchmark]
    public void InterpreterBenchmarkXL()
    {
        Interpreter.Visit(
            BenchHelp.astXL.Node,
            new Context() { symbolTable = BenchHelp.GetSymbolTable() }
        );
    }
}
