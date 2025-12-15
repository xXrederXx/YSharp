using YSharp.Parser.Nodes;

interface INodeOptimizer<TIn, TOut> where TIn : BaseNode where TOut : BaseNode
{
    TOut OptimizeNode(TIn node);
}