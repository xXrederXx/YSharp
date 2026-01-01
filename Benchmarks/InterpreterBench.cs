using BenchmarkDotNet.Attributes;
using YSharp.Runtime;

namespace YSharp.Benchmarks;

public class InterpreterBench
{
    [Benchmark]
    public void InterpreterBenchmarkL()
    {
        Interpreter.Visit(
            BenchHelp.astL.Node,
            new Context { symbolTable = SymbolTable.GenerateGlobalSymboltable() }
        );
    }

    [Benchmark]
    public void InterpreterBenchmarkM()
    {
        Interpreter.Visit(
            BenchHelp.astM.Node,
            new Context { symbolTable = SymbolTable.GenerateGlobalSymboltable() }
        );
    }

    [Benchmark]
    public void InterpreterBenchmarkS()
    {
        Interpreter.Visit(
            BenchHelp.astS.Node,
            new Context { symbolTable = SymbolTable.GenerateGlobalSymboltable() }
        );
    }

    [Benchmark]
    public void InterpreterBenchmarkXL()
    {
        Interpreter.Visit(
            BenchHelp.astXL.Node,
            new Context { symbolTable = SymbolTable.GenerateGlobalSymboltable() }
        );
    }
}
