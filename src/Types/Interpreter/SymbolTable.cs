namespace YSharp.Types.Interpreter;

public class SymbolTable
{
    public Dictionary<string, Value> symbols = [];
    public SymbolTable? parent = null;

    private Value GetFromParent(string name, Value defaultValue)
    {
        return parent is not null ? parent.Get(name, defaultValue) : defaultValue;
    }

    public Value Get(string? name)
    {
        if (string.IsNullOrEmpty(name))
        {
            return ValueNull.Instance;
        }

        return symbols.TryGetValue(name, out Value? value)
            ? value
            : GetFromParent(name, ValueNull.Instance);
    }

    public Value Get(string? name, Value defaultValue)
    {
        if (string.IsNullOrEmpty(name))
        {
            return defaultValue;
        }

        return symbols.TryGetValue(name, out Value? value)
            ? value
            : GetFromParent(name, defaultValue);
    }

    public void Set(string name, Value value)
    {
        symbols[name] = value;
    }

    public void Remove(string name)
    {
        symbols.Remove(name);
    }
}
