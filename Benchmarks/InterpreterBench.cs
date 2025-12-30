using YSharp.Runtime;
using BenchmarkDotNet.Attributes;

namespace YSharp.Benchmarks;


public class InterpreterBench
{
    [Benchmark]
    public void InterpreterBenchmarkL()
    {
        Interpreter.Visit(
            BenchHelp.astL.Node,
            new Context { symbolTable = BenchHelp.GetSymbolTable() }
        );
    }

    [Benchmark]
    public void InterpreterBenchmarkM()
    {
        Interpreter.Visit(
            BenchHelp.astM.Node,
            new Context { symbolTable = BenchHelp.GetSymbolTable() }
        );
    }

    [Benchmark]
    public void InterpreterBenchmarkS()
    {
        Interpreter.Visit(
            BenchHelp.astS.Node,
            new Context { symbolTable = BenchHelp.GetSymbolTable() }
        );
    }

    [Benchmark]
    public void InterpreterBenchmarkXL()
    {
        Interpreter.Visit(
            BenchHelp.astXL.Node,
            new Context { symbolTable = BenchHelp.GetSymbolTable() }
        );
    }
}