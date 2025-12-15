using YSharp.Parser.Nodes;

interface INodeOptimizer<TIn, TOut> where TIn : BaseNode where TOut : BaseNode
{
    bool IsOptimizable(TIn node);
    TOut OptimizeNode(TIn node);
}