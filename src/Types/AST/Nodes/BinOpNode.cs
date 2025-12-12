using YSharp.Types.Lexer;

namespace YSharp.Types.AST;

public sealed class BinOpNode : BaseNode
{
    public readonly BaseNode LeftNode;
    public readonly IToken OpTok;
    public readonly BaseNode RightNode;

    public BinOpNode(BaseNode leftNode, IToken opTok, BaseNode rightNode)
        : base(leftNode.StartPos, rightNode.EndPos)
    {
        this.LeftNode = leftNode;
        this.OpTok = opTok;
        this.RightNode = rightNode;
    }

    public override string ToString() => $"({LeftNode}, {OpTok}, {RightNode})";
}