using YSharp.Parser.Nodes;

namespace YSharp.Optimizer.NodeOptimizers;

public sealed class NumberConstantFolder : INodeOptimizer<BinOpNode, NumberNode>
{
    public NumberNode OptimizeNode(BinOpNode node)
    {
        throw new NotImplementedException();
    }
}