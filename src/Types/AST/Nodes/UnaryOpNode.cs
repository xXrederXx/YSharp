using FastEnumUtility;
using YSharp.Types.Lexer;
using YSharp.Utils.Dot;

namespace YSharp.Types.AST;

public sealed class UnaryOpNode : BaseNode
{
    public readonly BaseNode Node;
    public readonly IToken OpTok;
    public override NodeDebugInfo DebugInfo => new($"UnOp: {OpTok.Type.FastToString()}", NodeDebugShape.Ellipse, []);

    public UnaryOpNode(IToken opTok, BaseNode node)
        : base(opTok.StartPos, node.EndPos)
    {
        this.OpTok = opTok;
        this.Node = node;
    }

    public override string ToString() => $"({OpTok}, {Node})";
}