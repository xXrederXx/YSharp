namespace YSharp.Runtime;

public class Symbol
{
    public Value StoredValue { get; set; }
    public readonly bool IsReadonly;

    public Symbol(Value value, bool isReadonly)
    {
        (StoredValue, IsReadonly) = (value, isReadonly);
    }
}
