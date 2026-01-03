using YSharp.Lexer;
using YSharp.Parser.Nodes;

namespace YSharp.Optimizer.NodeOptimizers;

public sealed class UnaryConstantFolder : NodeOptimizer<UnaryOpNode, NumberNode>
{
    public override bool IsOptimizable(UnaryOpNode node)
    {
        return node.Node is NumberNode && node.OpTok.Type is TokenType.MINUS;
    }

    public override NumberNode OptimizeNode(UnaryOpNode node)
    {
        NumberNode num = (NumberNode)node.Node;

        return new NumberNode(
            new Token<double>(TokenType.NUMBER, num.Tok.Value * -1, node.StartPos, node.EndPos)
        );
    }
}
