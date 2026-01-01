using YSharp.Runtime.Functions;
using YSharp.Runtime.Primatives.Bool;
using YSharp.Runtime.Utils.Math;

namespace YSharp.Runtime;

public class SymbolTable
{
    public SymbolTable? parent = null;
    public Dictionary<string, Value> symbols = [];

    public Value Get(string? name)
    {
        if (string.IsNullOrEmpty(name)) return ValueNull.Instance;

        return symbols.TryGetValue(name, out Value? value)
            ? value
            : GetFromParent(name, ValueNull.Instance);
    }

    public Value Get(string? name, Value defaultValue)
    {
        if (string.IsNullOrEmpty(name)) return defaultValue;

        return symbols.TryGetValue(name, out Value? value)
            ? value
            : GetFromParent(name, defaultValue);
    }

    public void Remove(string name)
    {
        symbols.Remove(name);
    }

    public void Set(string name, Value value)
    {
        symbols[name] = value;
    }

    private Value GetFromParent(string name, Value defaultValue) =>
        parent is not null ? parent.Get(name, defaultValue) : defaultValue;

    public static SymbolTable GenerateGlobalSymboltable()
    {
        SymbolTable SampleSymbolTable = new();

        SampleSymbolTable.Set("TRUE", new VBool(true));
        SampleSymbolTable.Set("FALSE", new VBool(false));

        SampleSymbolTable.Set("MATH", new VMath());
        SampleSymbolTable.Set("PRINT", BuiltInFunctionsTable.print);
        SampleSymbolTable.Set("INPUT", BuiltInFunctionsTable.input);
        SampleSymbolTable.Set("RUN", BuiltInFunctionsTable.run);
        SampleSymbolTable.Set("TIMETORUN", BuiltInFunctionsTable.timetorun);
        SampleSymbolTable.Set("TIME", BuiltInFunctionsTable.time);

        return SampleSymbolTable;
    }
}