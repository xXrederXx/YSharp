using System;
using BenchmarkDotNet.Attributes;
using YSharp.Internal;

namespace YSharp.Benchmarks;

[SimpleJob]
[MemoryDiagnoser]
public class InterpreterBench
{
    [Benchmark]
    public void BenchmarkS()
    {
        Interpreter.Visit(
            BenchHelp.astS.Node,
            new Context() { symbolTable = BenchHelp.GetSymbolTable() }
        );
    }

    [Benchmark]
    public void BenchmarkM()
    {
        Interpreter.Visit(
            BenchHelp.astM.Node,
            new Context() { symbolTable = BenchHelp.GetSymbolTable() }
        );
    }

    [Benchmark]
    public void BenchmarkL()
    {
        Interpreter.Visit(
            BenchHelp.astL.Node,
            new Context() { symbolTable = BenchHelp.GetSymbolTable() }
        );
    }

    [Benchmark]
    public void BenchmarkXL()
    {
        Interpreter.Visit(
            BenchHelp.astXL.Node,
            new Context() { symbolTable = BenchHelp.GetSymbolTable() }
        );
    }
}
