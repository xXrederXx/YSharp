using FastEnumUtility;
using YSharp.Types.Lexer;
using YSharp.Utils.Dot;

namespace YSharp.Types.AST;

public sealed class BinOpNode : BaseNode
{
    public readonly BaseNode LeftNode;
    public readonly IToken OpTok;
    public readonly BaseNode RightNode;
    public override NodeDebugInfo DebugInfo =>
        new NodeDebugInfo(
            $"BinOp: {OpTok.Type.FastToString()}",
            NodeDebugShape.Hexagon,
            [(LeftNode.DebugInfo, "left"), (RightNode.DebugInfo, "right")]
        );

    public BinOpNode(BaseNode leftNode, IToken opTok, BaseNode rightNode)
        : base(leftNode.StartPos, rightNode.EndPos)
    {
        this.LeftNode = leftNode;
        this.OpTok = opTok;
        this.RightNode = rightNode;
    }

    public override string ToString() => $"({LeftNode}, {OpTok}, {RightNode})";
}
