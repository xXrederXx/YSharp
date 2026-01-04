using FastEnumUtility;
using YSharp.Lexer;
using YSharp.Parser.Nodes;

namespace YSharp.Optimizer.NodeOptimizers;

public sealed class NumberConstantFolder : NodeOptimizer<BinOpNode, NumberNode>
{
    public override bool IsOptimizable(BinOpNode node)
    {
        if(node.OpTok.Type == TokenType.DIV && node.RightNode is NumberNode r && r.Tok.Value == 0) return false;

        return node.LeftNode is NumberNode
            && node.RightNode is NumberNode
            && node.OpTok.Type
                is TokenType.PLUS
                    or TokenType.MINUS
                    or TokenType.MUL 
                    or TokenType.DIV
                    or TokenType.POW;
    }

    public override NumberNode OptimizeNode(BinOpNode node)
    {
        NumberNode left = (NumberNode)node.LeftNode;
        NumberNode right = (NumberNode)node.RightNode;

        double num1 = left.Tok.Value;
        double num2 = right.Tok.Value;

        double result = node.OpTok.Type switch
        {
            TokenType.PLUS => num1 + num2,
            TokenType.MINUS => num1 - num2,
            TokenType.MUL => num1 * num2,
            TokenType.DIV => num1 / num2,
            TokenType.POW => Math.Pow(num1, num2),
            _ => throw new InvalidOperationException(
                "Tokentype not supportet. Type: " + node.OpTok.Type.FastToString()
            ),
        };

        return new NumberNode(
            new Token<double>(TokenType.NUMBER, result, node.StartPos, node.EndPos)
        );
    }
}
