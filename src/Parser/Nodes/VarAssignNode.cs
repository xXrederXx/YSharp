using YSharp.Lexer;
using YSharp.Tools.Debug;

namespace YSharp.Parser.Nodes;


public sealed class VarAssignNode : BaseNode
{
    public readonly BaseNode ValueNode;
    public readonly Token<string> VarNameTok;
    public override NodeDebugInfo DebugInfo =>
        new(
            $"VarAssign ({VarNameTok.Value})",
            NodeDebugShape.Ellipse,
            [(ValueNode.DebugInfo, "value")]
        );

    public VarAssignNode(Token<string> varNameTok, BaseNode valueNode)
        : base(varNameTok.StartPos, valueNode.EndPos)
    {
        this.VarNameTok = varNameTok;
        this.ValueNode = valueNode;
    }

    public override string ToString() => VarNameTok.Value + " = " + ValueNode;
}
