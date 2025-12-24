using FastEnumUtility;
using YSharp.Lexer;
using YSharp.Tools.Debug;

namespace YSharp.Parser.Nodes;


public sealed class UnaryOpNode : BaseNode
{
    public readonly BaseNode Node;
    public readonly BaseToken OpTok;
    public override NodeDebugInfo DebugInfo => new($"UnOp: {OpTok.Type.FastToString()}", NodeDebugShape.Ellipse, []);

    public UnaryOpNode(BaseToken opTok, BaseNode node)
        : base(opTok.StartPos, node.EndPos)
    {
        this.OpTok = opTok;
        this.Node = node;
    }

    public override string ToString() => $"({OpTok}, {Node})";
}