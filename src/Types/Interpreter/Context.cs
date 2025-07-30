using YSharp.Types.Common;

namespace YSharp.Types.Interpreter;

public class Context
{
    public readonly string displayName;
    public readonly Context? parent;
    public readonly Position parentEntryPos;
    public SymbolTable? symbolTable;

    public Context(string displayName, Context? parent, in Position parentEntryPos)
    {
        this.displayName = displayName;
        this.parentEntryPos = parentEntryPos;
        this.parent = parent;
    }

    public Context()
    {
        displayName = string.Empty;
        parent = null;
        parentEntryPos = new();
    }

    public override string ToString() =>
        $"DisplayName {displayName} / SymbolTable {symbolTable} / Parent Entry {parentEntryPos} / parent {parent}";
}

