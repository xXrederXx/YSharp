using YSharp.Lexer;
using YSharp.Parser.Nodes;

namespace YSharp.Optimizer.NodeOptimizers;

public class AlgebraicSimplification : NodeOptimizer<BinOpNode, VarAccessNode>
{
    public override bool IsOptimizable(BinOpNode node)
    {
        return (
                (node.LeftNode is VarAccessNode && node.RightNode is NumberNode)
                || (node.RightNode is VarAccessNode && node.LeftNode is NumberNode)
            )
            && node.OpTok.Type
                is TokenType.PLUS
                    or TokenType.MINUS
                    or TokenType.MUL
                    or TokenType.DIV
                    or TokenType.POW;
    }

    public override VarAccessNode OptimizeNode(BinOpNode node)
    {
        //? Add built in variables for 1 and 0
        //? Like ONE and ZERO
        // x + 0  → x
        // x * 1  → x
        // x * 0  → 0
        // x - 0  → x
        // x / 1  → x
        // x ^ 1 -> x
        // x ^ 0 -> 1
        throw new NotImplementedException();
    }
}
