using YSharp.Common;

namespace YSharp.Runtime;

public class Context
{
    public string DisplayName { get; }
    public Context? Parent { get; }
    public Position ParentEntryPos { get; }
    public SymbolTable SymbolTable { get; }

    public Context(
        string displayName,
        Context? parent,
        in Position parentEntryPos,
        SymbolTable symbolTable
    )
    {
        DisplayName = displayName;
        ParentEntryPos = parentEntryPos;
        Parent = parent;
        SymbolTable = symbolTable;
    }

    public Context()
    {
        DisplayName = string.Empty;
        Parent = null;
        ParentEntryPos = Position.Null;
        SymbolTable = new SymbolTable();
    }

    public override string ToString() =>
        $"DisplayName {DisplayName} / SymbolTable {SymbolTable} / Parent Entry {ParentEntryPos} / parent {Parent}";
}
