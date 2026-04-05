using YSharp.Runtime.Functions;
using YSharp.Runtime.Primatives.Bool;
using YSharp.Runtime.Utils.Math;

namespace YSharp.Runtime;

public class SymbolTable
{
    public SymbolTable? Parent { private get; init; } = null;
    private Dictionary<string, Value> Symbols = [];

    public Value Get(string? name) => Get(name, ValueNull.Instance);

    public Value Get(string? name, Value defaultValue)
    {
        if (string.IsNullOrEmpty(name)) return defaultValue;

        return Symbols.TryGetValue(name, out Value? value)
            ? value
            : GetFromParent(name, defaultValue);
    }

    public void Remove(string name)
    {
        Symbols.Remove(name);
    }

    public void Set(string name, Value value)
    {
        Symbols[name] = value;
    }

    private Value GetFromParent(string name, Value defaultValue) =>
        Parent is not null ? Parent.Get(name, defaultValue) : defaultValue;

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
