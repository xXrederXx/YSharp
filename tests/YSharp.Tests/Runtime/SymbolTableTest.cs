using Xunit;
using YSharp.Common;
using YSharp.Runtime;
using YSharp.Runtime.Primitives.Number;

namespace YSharp.Tests;

public class SymbolTableTest
{
    [Fact]
    public void checkSet_whenNewSymbol_thenIsSet()
    {
        SymbolTable symbolTable = new SymbolTable();
        Error res = symbolTable.Set(
            "test",
            new Symbol(new VNumber(3), false),
            Position.Null,
            new Context()
        );
        Assert.IsType<ErrorNull>(res);
        Value v = symbolTable.Get("test");
        VNumber num = Assert.IsType<VNumber>(v);
        Assert.Equal(3, num.value);
    }

    [Fact]
    public void checkSet_whenSymbolExists_thenIsSet()
    {
        SymbolTable symbolTable = new SymbolTable();
        symbolTable.Set("test", new Symbol(new VNumber(3), false), Position.Null, new Context());
        Error res = symbolTable.Set(
            "test",
            new Symbol(new VNumber(6), false),
            Position.Null,
            new Context()
        );
        Assert.IsType<ErrorNull>(res);
        Value v = symbolTable.Get("test");
        VNumber num = Assert.IsType<VNumber>(v);
        Assert.Equal(6, num.value);
    }

    [Fact]
    public void checkSet_whenSymbolExistsReadonly_thenReadonlyAssignError()
    {
        SymbolTable symbolTable = new SymbolTable();
        symbolTable.Set("test", new Symbol(new VNumber(3), true), Position.Null, new Context());
        Error res = symbolTable.Set(
            "test",
            new Symbol(new VNumber(6), false),
            Position.Null,
            new Context()
        );
        Assert.IsType<AssignmentToConstantError>(res);
    }

    [Fact]
    public void checkUpdate_whenNewSymbol_thenNotFoundError()
    {
        SymbolTable symbolTable = new SymbolTable();
        Error res = symbolTable.Update("test", new VNumber(3), Position.Null, new Context());
        Assert.IsType<VarNotFoundError>(res);
    }

    [Fact]
    public void checkUpdate_whenSymbolExists_thenIsUpdated()
    {
        SymbolTable symbolTable = new SymbolTable();
        symbolTable.Set("test", new Symbol(new VNumber(3), false), Position.Null, new Context());
        Error res = symbolTable.Update("test", new VNumber(6), Position.Null, new Context());
        Assert.IsType<ErrorNull>(res);
        Value v = symbolTable.Get("test");
        VNumber num = Assert.IsType<VNumber>(v);
        Assert.Equal(6, num.value);
    }

    [Fact]
    public void checkUpdate_whenSymbolExistsReadonly_thenReadonlyAssignError()
    {
        SymbolTable symbolTable = new SymbolTable();
        symbolTable.Set("test", new Symbol(new VNumber(3), true), Position.Null, new Context());
        Error res = symbolTable.Update("test", new VNumber(6), Position.Null, new Context());
        Assert.IsType<AssignmentToConstantError>(res);
    }
}
