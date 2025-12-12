using YSharp.Types.Lexer;

namespace YSharp.Types.AST;

public sealed class UnaryOpNode : BaseNode
{
    public readonly BaseNode Node;
    public readonly IToken OpTok;

    public UnaryOpNode(IToken opTok, BaseNode node)
        : base(opTok.StartPos, node.EndPos)
    {
        this.OpTok = opTok;
        this.Node = node;
    }

    public override string ToString() => $"({OpTok}, {Node})";
}