using YSharp.Optimizer.NodeOptimizers;
using YSharp.Parser;
using YSharp.Parser.Nodes;

namespace YSharp.Optimizer;

public static class Optimizer
{
    private static readonly List<INodeOptimizer> optimizers =
    [
        new NumberConstantFolder(),
        new StringConstantFolder(),
        new UnaryConstantFolder()
    ];

    public static BaseNode Visit(BaseNode node)
    {
        BaseNode rebuilt =  node switch
        {
            NumberNode n => Visit_Number(n),
            StringNode n => Visit_String(n),
            ListNode n => Visit_List(n),
            BinOpNode n => Visit_BinaryOp(n),
            UnaryOpNode n => Visit_UnaryOp(n),
            VarAccessNode n => Visit_VarAccessNode(n),
            VarAssignNode n => Visit_VarAssignNode(n),
            DotVarAccessNode n => Visit_DotVarAccessNode(n),
            DotCallNode n => Visit_DotCallNode(n),
            IfNode n => Visit_IfNode(n),
            ForNode n => Visit_ForNode(n),
            WhileNode n => Visit_WhileNode(n),
            FuncDefNode n => Visit_FuncDefNode(n),
            CallNode n => Visit_CallNode(n),
            ReturnNode n => Visit_ReturnNode(n),
            ContinueNode n => Visit_ContinueNode(n),
            BreakNode n => Visit_BreakNode(n),
            TryCatchNode n => Visit_TryCatchNode(n),
            ImportNode n => Visit_ImportNode(n),
            SuffixAssignNode n => Visit_SuffixAssignNode(n),
            NodeNull n => n,
            _ => Vistit_ErrorNode(node),
        };
        foreach (var optimizer in optimizers.Where(x => x.IsOptimizable(rebuilt)))
        {
            rebuilt = optimizer.OptimizeNode(rebuilt);
        }
        return rebuilt;
    }

    private static BaseNode Visit_BinaryOp(BinOpNode node)
    {
        BaseNode left = Visit(node.LeftNode);
        BaseNode right = Visit(node.RightNode);
        BaseNode rebuilt = new BinOpNode(left, node.OpTok, right);
        return rebuilt;
    }

    private static BaseNode Visit_BreakNode(BreakNode node) => node;

    private static BaseNode Visit_CallNode(CallNode node)
    {
        BaseNode nodeToCall = Visit(node.NodeToCall);
        List<BaseNode> args = new(node.ArgNodes.Length);
        for (int i = 0; i < node.ArgNodes.Length; i++)
        {
            args.Add(Visit(node.ArgNodes[i]));
        }

        return new CallNode(nodeToCall, args);
    }

    private static BaseNode Visit_ContinueNode(ContinueNode node) => node;

    private static BaseNode Visit_DotCallNode(DotCallNode node)
    {
        List<BaseNode> argBaseNode = new(node.ArgNodes.Length);
        for (int i = 0; i < node.ArgNodes.Length; i++)
        {
            BaseNode _node = node.ArgNodes[i];
            BaseNode val = Visit(_node);

            argBaseNode.Add(val);
        }

        return new DotCallNode(node.FuncNameTok, argBaseNode, node.Parent);
    }

    private static BaseNode Visit_DotVarAccessNode(DotVarAccessNode node) => node;

    private static BaseNode Visit_ForNode(ForNode node)
    {
        BaseNode startBaseNode = Visit(node.StartValueNode);
        BaseNode endBaseNode = Visit(node.EndValueNode);
        BaseNode bodyBaseNode = Visit(node.BodyNode);
        BaseNode? stepBaseNode = node.StepValueNode is null
            ? node.StepValueNode
            : Visit(node.StepValueNode);

        return new ForNode(
            node.VarNameTok,
            startBaseNode,
            endBaseNode,
            stepBaseNode,
            bodyBaseNode,
            node.RetNull
        );
    }

    private static BaseNode Visit_FuncDefNode(FuncDefNode node)
    {
        BaseNode bodyNode = Visit(node.BodyNode);

        return new FuncDefNode(
            node.VarNameTok,
            node.ArgNameTokens.ToList(),
            bodyNode,
            node.RetNull
        );
    }

    private static BaseNode Visit_IfNode(IfNode node)
    {
        List<SubIfNode> subifs = [];
        for (int i = 0; i < node.Cases.Length; i++)
        {
            BaseNode condition = Visit(node.Cases[i].Condition);
            BaseNode expr = Visit(node.Cases[i].Expression);

            subifs.Add(new SubIfNode(condition, expr));
        }

        BaseNode elseBaseNode = Visit(node.ElseNode);

        return new IfNode(subifs, elseBaseNode);
    }

    private static BaseNode Visit_ImportNode(ImportNode node) => node;

    private static BaseNode Visit_List(ListNode node)
    {
        List<BaseNode> elements = new(node.ElementNodes.Length);
        for (int i = 0; i < node.ElementNodes.Length; i++)
        {
            BaseNode elementNode = node.ElementNodes[i];
            elements.Add(Visit(elementNode));
        }

        return new ListNode(elements, node.StartPos, node.EndPos);
    }

    private static BaseNode Visit_Number(NumberNode node) => node;

    private static BaseNode Visit_ReturnNode(ReturnNode node)
    {
        BaseNode? BaseNode = node.NodeToReturn is null
            ? node.NodeToReturn
            : Visit(node.NodeToReturn);
        return new ReturnNode(BaseNode, node.StartPos, node.EndPos);
    }

    private static BaseNode Visit_String(StringNode node) => node;

    private static BaseNode Visit_SuffixAssignNode(SuffixAssignNode node) => node;

    private static BaseNode Visit_TryCatchNode(TryCatchNode node)
    {
        BaseNode tryNode = Visit(node.TryNode);
        BaseNode catchNode = Visit(node.CatchNode);
        return new TryCatchNode(tryNode, catchNode, node.CatchVarName);
    }

    private static BaseNode Visit_UnaryOp(UnaryOpNode node)
    {
        BaseNode BaseNode = Visit(node.Node);

        return new UnaryOpNode(node.OpTok, BaseNode);
    }

    private static BaseNode Visit_VarAccessNode(VarAccessNode node) => node;

    private static BaseNode Visit_VarAssignNode(VarAssignNode node)
    {
        ParseResult res = new();
        BaseNode BaseNode = Visit(node.ValueNode);

        return new VarAssignNode(node.VarNameTok, BaseNode);
    }

    private static BaseNode Visit_WhileNode(WhileNode node)
    {
        ParseResult res = new();
        BaseNode conditionNode = Visit(node.ConditionNode);
        BaseNode bodyNode = Visit(node.BodyNode);
        return new WhileNode(conditionNode, bodyNode, node.RetNull);
    }

    private static BaseNode Vistit_ErrorNode(BaseNode node)
    {
        Console.WriteLine("No method found for " + node.GetType());
        return node;
    }
}
