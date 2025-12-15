using YSharp.Parser.Nodes;

namespace YSharp.Optimizer;
public abstract class NodeOptimizer<TIn, TOut> : INodeOptimizer
    where TIn : BaseNode
    where TOut : BaseNode
{
    public abstract bool IsOptimizable(TIn node);
    public abstract TOut OptimizeNode(TIn node);

    public BaseNode OptimizeNode(BaseNode node)
    {
        if (node is TIn binOpNode)
            return OptimizeNode(binOpNode);
        throw new ArgumentException("node is not of right type");
    }

    public bool IsOptimizable(BaseNode node)
    {
        return node is TIn binOpNode && IsOptimizable(binOpNode);
    }
}
