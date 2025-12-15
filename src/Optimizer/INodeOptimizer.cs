using YSharp.Parser.Nodes;
namespace YSharp.Optimizer;
interface INodeOptimizer
{
    bool IsOptimizable(BaseNode node);
    BaseNode OptimizeNode(BaseNode node);
}
