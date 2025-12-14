using YSharp.Lexer;
using YSharp.Parser;
using YSharp.Parser.Nodes;

namespace YSharp.Optimizer;

public static class Optimizer
{
    public static ParseResult Visit(BaseNode node)
    {
        return node switch
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
            NodeNull n => new ParseResult().Success(n),
            _ => Vistit_ErrorNode(node),
        };
    }

    private static ParseResult Visit_BinaryOp(BinOpNode node)
    {
        ParseResult res = new();
        BaseNode left = res.Register(Visit(node.LeftNode));
        BaseNode right = res.Register(Visit(node.RightNode));

        if (res.HasError)
            return res;

        if (
            left is StringNode leftStr
            && right is StringNode rightStr
            && node.OpTok.Type == TokenType.PLUS
        )
            return res.Success(
                new StringNode(
                    new Token<string>(
                        leftStr.Tok.Type,
                        leftStr.Tok.Value + rightStr.Tok.Value,
                        leftStr.StartPos,
                        rightStr.EndPos
                    )
                )
            );

        if (left is not NumberNode leftNum || right is not NumberNode rightNum)
            return res.Success(new BinOpNode(left, node.OpTok, right));

        double num1 = leftNum.Tok.Value;
        double num2 = rightNum.Tok.Value;
        if (num2 == 0)
            return res.Success(new BinOpNode(left, node.OpTok, right));

        double? result = node.OpTok.Type switch
        {
            TokenType.PLUS => num1 + num2,
            TokenType.MINUS => num1 - num2,
            TokenType.MUL => num1 * num2,
            TokenType.DIV => num1 / num2,
            TokenType.POW => Math.Pow(num1, num2),
            _ => null,
        };
        if (result is not null)
        {
            return res.Success(
                new NumberNode(
                    new Token<double>(
                        TokenType.FLOAT,
                        (double)result,
                        leftNum.StartPos,
                        rightNum.EndPos
                    )
                )
            );
        }

        return res.Success(new BinOpNode(left, node.OpTok, right));
    }

    private static ParseResult Visit_BreakNode(BreakNode node) => new ParseResult().Success(node);

    private static ParseResult Visit_CallNode(CallNode node)
    {
        ParseResult res = new();
        BaseNode nodeToCall = res.Register(Visit(node.NodeToCall));
        List<BaseNode> args = new(node.ArgNodes.Length);
        for (int i = 0; i < node.ArgNodes.Length; i++)
        {
            args.Add(res.Register(Visit(node.ArgNodes[i])));
            if (res.HasError)
                return res;
        }

        return res.Success(new CallNode(nodeToCall, args));
    }

    private static ParseResult Visit_ContinueNode(ContinueNode node) =>
        new ParseResult().Success(node);

    private static ParseResult Visit_DotCallNode(DotCallNode node)
    {
        ParseResult res = new();
        List<BaseNode> argBaseNode = new(node.ArgNodes.Length);
        for (int i = 0; i < node.ArgNodes.Length; i++)
        {
            BaseNode _node = node.ArgNodes[i];
            BaseNode val = res.Register(Visit(_node));
            if (res.HasError)
                return res;
            argBaseNode.Add(val);
        }

        return res.Success(new DotCallNode(node.FuncNameTok, argBaseNode, node.Parent));
    }

    private static ParseResult Visit_DotVarAccessNode(DotVarAccessNode node) =>
        new ParseResult().Success(node);

    private static ParseResult Visit_ForNode(ForNode node)
    {
        ParseResult res = new();
        BaseNode startBaseNode = res.Register(Visit(node.StartValueNode));
        BaseNode endBaseNode = res.Register(Visit(node.EndValueNode));
        BaseNode bodyBaseNode = res.Register(Visit(node.BodyNode));
        BaseNode? stepBaseNode = node.StepValueNode is null
            ? node.StepValueNode
            : res.Register(Visit(node.StepValueNode));

        if (res.HasError)
            return res;

        return res.Success(
            new ForNode(
                node.VarNameTok,
                startBaseNode,
                endBaseNode,
                stepBaseNode,
                bodyBaseNode,
                node.RetNull
            )
        );
    }

    private static ParseResult Visit_FuncDefNode(FuncDefNode node)
    {
        ParseResult res = new();
        BaseNode bodyNode = res.Register(Visit(node.BodyNode));

        return res.Success(
            new FuncDefNode(node.VarNameTok, node.ArgNameTokens.ToList(), bodyNode, node.RetNull)
        );
    }

    private static ParseResult Visit_IfNode(IfNode node)
    {
        ParseResult res = new();
        List<SubIfNode> subifs = [];
        for (int i = 0; i < node.Cases.Length; i++)
        {
            BaseNode condition = res.Register(Visit(node.Cases[i].Condition));
            BaseNode expr = res.Register(Visit(node.Cases[i].Expression));
            if (res.HasError)
                return res;
            subifs.Add(new SubIfNode(condition, expr));
        }

        BaseNode elseBaseNode = res.Register(Visit(node.ElseNode));

        return res.Success(new IfNode(subifs, elseBaseNode));
    }

    private static ParseResult Visit_ImportNode(ImportNode node) => new ParseResult().Success(node);

    private static ParseResult Visit_List(ListNode node)
    {
        ParseResult res = new();
        List<BaseNode> elements = new(node.ElementNodes.Length);
        for (int i = 0; i < node.ElementNodes.Length; i++)
        {
            BaseNode elementNode = node.ElementNodes[i];
            elements.Add(res.Register(Visit(elementNode)));
            if (res.HasError)
                return res;
        }

        return res.Success(new ListNode(elements, node.StartPos, node.EndPos));
    }

    private static ParseResult Visit_Number(NumberNode node) => new ParseResult().Success(node);

    private static ParseResult Visit_ReturnNode(ReturnNode node)
    {
        ParseResult res = new();
        BaseNode? BaseNode = node.NodeToReturn is null
            ? node.NodeToReturn
            : res.Register(Visit(node.NodeToReturn));
        return res.Success(new ReturnNode(BaseNode, node.StartPos, node.EndPos));
    }

    private static ParseResult Visit_String(StringNode node) => new ParseResult().Success(node);

    private static ParseResult Visit_SuffixAssignNode(SuffixAssignNode node) =>
        new ParseResult().Success(node);

    private static ParseResult Visit_TryCatchNode(TryCatchNode node)
    {
        ParseResult res = new();
        BaseNode tryNode = res.Register(Visit(node.TryNode));
        BaseNode catchNode = res.Register(Visit(node.CatchNode));
        return res.Success(new TryCatchNode(tryNode, catchNode, node.CatchVarName));
    }

    private static ParseResult Visit_UnaryOp(UnaryOpNode node)
    {
        ParseResult res = new();
        BaseNode BaseNode = res.Register(Visit(node.Node));
        if (res.HasError)
            return res;

        return res.Success(new UnaryOpNode(node.OpTok, BaseNode));
    }

    private static ParseResult Visit_VarAccessNode(VarAccessNode node) =>
        new ParseResult().Success(node);

    private static ParseResult Visit_VarAssignNode(VarAssignNode node)
    {
        ParseResult res = new();
        BaseNode BaseNode = res.Register(Visit(node.ValueNode));
        if (res.HasError)
            return res;
        return res.Success(new VarAssignNode(node.VarNameTok, BaseNode));
    }

    private static ParseResult Visit_WhileNode(WhileNode node)
    {
        ParseResult res = new();
        BaseNode conditionNode = res.Register(Visit(node.ConditionNode));
        BaseNode bodyNode = res.Register(Visit(node.BodyNode));
        return res.Success(new WhileNode(conditionNode, bodyNode, node.RetNull));
    }

    private static ParseResult Vistit_ErrorNode(BaseNode node)
    {
        Console.WriteLine("No method found for " + node.GetType());
        return new ParseResult().Success(node);
    }
}
