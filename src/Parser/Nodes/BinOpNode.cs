using FastEnumUtility;
using YSharp.Lexer;
using YSharp.Tools.Debug;

namespace YSharp.Parser.Nodes;

public sealed class BinOpNode : BaseNode
{
    public readonly BaseNode LeftNode;
    public readonly BaseToken OpTok;
    public readonly BaseNode RightNode;
    public override NodeDebugInfo DebugInfo =>
        new NodeDebugInfo(
            $"BinOp: {OpTok.Type.FastToString()}",
            NodeDebugShape.Hexagon,
            [(LeftNode.DebugInfo, "left"), (RightNode.DebugInfo, "right")]
        );

    public BinOpNode(BaseNode leftNode, BaseToken opTok, BaseNode rightNode)
        : base(leftNode.StartPos, rightNode.EndPos)
    {
        this.LeftNode = leftNode;
        this.OpTok = opTok;
        this.RightNode = rightNode;
    }

    public override string ToString() => $"({LeftNode}, {OpTok}, {RightNode})";
}
