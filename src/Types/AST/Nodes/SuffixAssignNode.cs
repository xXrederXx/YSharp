using YSharp.Types.Lexer;

namespace YSharp.Types.AST;

public sealed class SuffixAssignNode : BaseNode
{
    public readonly bool IsAdd;
    public readonly string VarName;

    public SuffixAssignNode(Token<string> varName, bool isAdd)
        : base(varName.StartPos, varName.EndPos)
    {
        this.VarName = varName.Value;
        this.IsAdd = isAdd;
    }
}