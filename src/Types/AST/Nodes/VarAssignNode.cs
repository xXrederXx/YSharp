using YSharp.Types.Lexer;

namespace YSharp.Types.AST;

public sealed class VarAssignNode : BaseNode
{
    public readonly BaseNode ValueNode;
    public readonly Token<string> VarNameTok;

    public VarAssignNode(Token<string> varNameTok, BaseNode valueNode)
        : base(varNameTok.StartPos, valueNode.EndPos)
    {
        this.VarNameTok = varNameTok;
        this.ValueNode = valueNode;
    }

    public override string ToString() => VarNameTok.Value + " = " + ValueNode;
}