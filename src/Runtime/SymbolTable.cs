using YSharp.Common;
using YSharp.Runtime.Functions;
using YSharp.Runtime.Primitives.Bool;
using YSharp.Runtime.Utils.Math;

namespace YSharp.Runtime;

public class SymbolTable
{
    public SymbolTable? Parent { private get; init; } = null;
    private Dictionary<string, Symbol> Symbols { get; init; } = [];

    public Value Get(string? name) => Get(name, ValueNull.Instance);

    public Value Get(string? name, Value defaultValue)
    {
        if (string.IsNullOrEmpty(name))
            return defaultValue;

        return Symbols.TryGetValue(name, out Symbol? symbol)
            ? symbol.StoredValue
            : GetFromParent(name, defaultValue);
    }

    ///<summary>
    /// Try to add an entry into the symbol table.
    /// If successfull it returns an ErrorNull.
    ///</summary>
    public Error Set(string name, Symbol symbol, Position pos, Context ctx)
    {
        if (Symbols.TryGetValue(name, out Symbol? stored) && stored.IsReadonly)
        {
            return new AssignmentToConstantError(pos, name, ctx);
        }
        Symbols[name] = symbol;
        return ErrorNull.Instance;
    }

    public Error Update(string name, Value value, Position pos, Context ctx)
    {
        if (!Symbols.TryGetValue(name, out Symbol? symbol))
        {
            return new VarNotFoundError(pos, name, ctx);
        }
        if (symbol.IsReadonly)
        {
            return new AssignmentToConstantError(pos, name, ctx);
        }
        symbol.StoredValue = value;
        return ErrorNull.Instance;
    }

    private Value GetFromParent(string name, Value defaultValue) =>
        Parent is not null ? Parent.Get(name, defaultValue) : defaultValue;

    public static SymbolTable GenerateGlobalSymboltable()
    {
        Dictionary<string, Symbol> Symbols = [];

        Symbols.Add("TRUE", new Symbol(new VBool(true), true));
        Symbols.Add("FALSE", new Symbol(new VBool(false), true));

        Symbols.Add("MATH", new Symbol(new VMath(), true));
        Symbols.Add("PRINT", new Symbol(BuiltInFunctionsTable.print, true));
        Symbols.Add("INPUT", new Symbol(BuiltInFunctionsTable.input, true));
        Symbols.Add("RUN", new Symbol(BuiltInFunctionsTable.run, true));
        Symbols.Add("TIMETORUN", new Symbol(BuiltInFunctionsTable.timetorun, true));
        Symbols.Add("TIME", new Symbol(BuiltInFunctionsTable.time, true));
        return new SymbolTable() { Symbols = Symbols };
    }
}
