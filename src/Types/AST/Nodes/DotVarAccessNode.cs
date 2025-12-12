using YSharp.Types.Lexer;

namespace YSharp.Types.AST;

public sealed class DotVarAccessNode : BaseNode
{
    public readonly BaseNode Parent;
    public readonly Token<string> VarNameTok;

    public DotVarAccessNode(Token<string> varNameTok, BaseNode parent)
        : base(varNameTok.StartPos, varNameTok.EndPos)
    {
        this.Parent = parent;
        this.VarNameTok = varNameTok;
    }

    public override string ToString() => $"Access {VarNameTok.Value} from {Parent}";
}