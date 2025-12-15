using YSharp.Lexer;
using YSharp.Parser.Nodes;

namespace YSharp.Optimizer.NodeOptimizers;

public sealed class StringConstantFolder : NodeOptimizer<BinOpNode, StringNode>
{
    public override bool IsOptimizable(BinOpNode node)
    {
        return node.LeftNode is StringNode
            && node.RightNode is StringNode
            && node.OpTok.Type == TokenType.PLUS;
    }

    public override StringNode OptimizeNode(BinOpNode node)
    {
        StringNode left = (StringNode)node.LeftNode;
        StringNode right = (StringNode)node.RightNode;

        string leftStr = left.Tok.Value;
        string rightStr = right.Tok.Value;

        return new StringNode(
            new Token<string>(TokenType.STRING, leftStr + rightStr, node.StartPos, node.EndPos)
        );
    }
}
