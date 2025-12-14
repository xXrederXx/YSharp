using YSharp.Lexer;
using YSharp.Tools.Debug;

namespace YSharp.Parser.Nodes;


public sealed class DotVarAccessNode : BaseNode
{
    public readonly BaseNode Parent;
    public readonly Token<string> VarNameTok;
    public override NodeDebugInfo DebugInfo =>
        new($"DotAcc ({VarNameTok.Value})", NodeDebugShape.Ellipse, [(Parent.DebugInfo, "parent")]);

    public DotVarAccessNode(Token<string> varNameTok, BaseNode parent)
        : base(varNameTok.StartPos, varNameTok.EndPos)
    {
        this.Parent = parent;
        this.VarNameTok = varNameTok;
    }

    public override string ToString() => $"Access {VarNameTok.Value} from {Parent}";
}
