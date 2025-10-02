using YSharp.Types.AST;
using YSharp.Types.Lexer;

namespace YSharp.Core;

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
            _ => Vistit_ErrorNode(node),
        };
    }

    private static ParseResult Vistit_ErrorNode(BaseNode node)
    {
        Console.WriteLine("No method found for " + node.GetType());
        return new ParseResult().Success(node);
    }

    private static ParseResult Visit_Number(NumberNode node)
    {
        return new ParseResult().Success(node);
    }

    private static ParseResult Visit_String(StringNode node)
    {
        return new ParseResult().Success(node);
    }

    private static ParseResult Visit_List(ListNode node)
    {
        ParseResult res = new();
        List<BaseNode> elements = new(node.elementNodes.Length);
        for (int i = 0; i < node.elementNodes.Length; i++)
        {
            BaseNode elementNode = node.elementNodes[i];
            elements.Add(res.Register(Visit(elementNode)));
            if (res.HasError)
            {
                return res;
            }
        }
        return res.Success(new ListNode(elements, node.StartPos, node.EndPos));
    }

    private static ParseResult Visit_BinaryOp(BinOpNode node)
    {
        ParseResult res = new();
        BaseNode left = res.Register(Visit(node.leftNode));
        BaseNode right = res.Register(Visit(node.rightNode));

        if (res.HasError)
        {
            return res;
        }

        if (left is StringNode leftStr && right is StringNode rightStr && node.opTok.Type == TokenType.PLUS)
        {
            return res.Success(new StringNode(new Token<string>(leftStr.tok.Type, leftStr.tok.Value + rightStr.tok.Value, leftStr.StartPos, rightStr.EndPos)));
        }

        if (left is not NumberNode leftNum || right is not NumberNode rightNum)
        {
            return res.Success(new BinOpNode(left, node.opTok, right));
        }

        double num1 = leftNum.tok.Value;
        double num2 = rightNum.tok.Value;
        if (num2 == 0)
        {
            return res.Success(new BinOpNode(left, node.opTok, right));
        }

        double? result = node.opTok.Type switch
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
        return res.Success(new BinOpNode(left, node.opTok, right));
    }

    private static ParseResult Visit_UnaryOp(UnaryOpNode node)
    {
        ParseResult res = new();
        BaseNode BaseNode = res.Register(Visit(node.node));
        if (res.HasError)
        {
            return res;
        }

        return res.Success(new UnaryOpNode(node.opTok, BaseNode));
    }

    private static ParseResult Visit_VarAccessNode(VarAccessNode node)
    {
        return new ParseResult().Success(node);
    }

    private static ParseResult Visit_VarAssignNode(VarAssignNode node)
    {
        ParseResult res = new();
        BaseNode BaseNode = res.Register(Visit(node.valueNode));
        if (res.HasError)
        {
            return res;
        }
        return res.Success(new VarAssignNode(node.varNameTok, BaseNode));
    }

    private static ParseResult Visit_DotVarAccessNode(DotVarAccessNode node)
    {
        return new ParseResult().Success(node);
    }

    private static ParseResult Visit_DotCallNode(DotCallNode node)
    {
        ParseResult res = new();
        List<BaseNode> argBaseNode = new(node.argNodes.Length);
        for (int i = 0; i < node.argNodes.Length; i++)
        {
            BaseNode _node = node.argNodes[i];
            BaseNode val = res.Register(Visit(_node));
            if (res.HasError)
            {
                return res;
            }
            argBaseNode.Add(val);
        }
        return res.Success(new DotCallNode(node.funcNameTok, argBaseNode, node.parent));
    }

    private static ParseResult Visit_IfNode(IfNode node)
    {
        ParseResult res = new();
        List<SubIfNode> subifs = [];
        for (int i = 0; i < node.cases.Length; i++)
        {
            BaseNode condition = res.Register(Visit(node.cases[i].condition));
            BaseNode expr = res.Register(Visit(node.cases[i].expression));
            if (res.HasError)
            {
                return res;
            }
            subifs.Add(new SubIfNode(condition, expr));
        }
        BaseNode elseBaseNode = res.Register(Visit(node.elseNode));

        return res.Success(new IfNode(subifs, elseBaseNode));
    }

    private static ParseResult Visit_ForNode(ForNode node)
    {
        ParseResult res = new();
        BaseNode startBaseNode = res.Register(Visit(node.startValueNode));
        BaseNode endBaseNode = res.Register(Visit(node.endValueNode));
        BaseNode bodyBaseNode = res.Register(Visit(node.bodyNode));
        BaseNode? stepBaseNode = node.stepValueNode is null
            ? node.stepValueNode
            : res.Register(Visit(node.stepValueNode));

        if (res.HasError)
        {
            return res;
        }

        return res.Success(
            new ForNode(
                node.varNameTok,
                startBaseNode,
                endBaseNode,
                stepBaseNode,
                bodyBaseNode,
                node.retNull
            )
        );
    }

    private static ParseResult Visit_WhileNode(WhileNode node)
    {
        ParseResult res = new();
        BaseNode conditionNode = res.Register(Visit(node.conditionNode));
        BaseNode bodyNode = res.Register(Visit(node.bodyNode));
        return res.Success(new WhileNode(conditionNode, bodyNode, node.retNull));
    }

    private static ParseResult Visit_FuncDefNode(FuncDefNode node)
    {
        ParseResult res = new();
        BaseNode bodyNode = res.Register(Visit(node.bodyNode));

        return res.Success(
            new FuncDefNode(node.varNameTok, node.argNameToks.ToList(), bodyNode, node.retNull)
        );
    }

    private static ParseResult Visit_CallNode(CallNode node)
    {
        ParseResult res = new();
        BaseNode nodeToCall = res.Register(Visit(node.nodeToCall));
        List<BaseNode> args = new(node.argNodes.Length);
        for (int i = 0; i < node.argNodes.Length; i++)
        {
            args.Add(res.Register(Visit(node.argNodes[i])));
            if (res.HasError)
            {
                return res;
            }
        }

        return res.Success(new CallNode(nodeToCall, args));
    }

    private static ParseResult Visit_ReturnNode(ReturnNode node)
    {
        ParseResult res = new();
        BaseNode? BaseNode = node.nodeToReturn is null ? node.nodeToReturn : res.Register(Visit(node.nodeToReturn));
        return res.Success(new ReturnNode(BaseNode, node.StartPos, node.EndPos));
    }

    private static ParseResult Visit_ContinueNode(ContinueNode node)
    {
        return new ParseResult().Success(node);
    }

    private static ParseResult Visit_BreakNode(BreakNode node)
    {
        return new ParseResult().Success(node);
    }

    private static ParseResult Visit_TryCatchNode(TryCatchNode node)
    {
        ParseResult res = new();
        BaseNode tryNode = res.Register(Visit(node.TryNode));
        BaseNode catchNode = res.Register(Visit(node.CatchNode));
        return res.Success(new TryCatchNode(tryNode, catchNode, node.ChatchVarName));
    }

    private static ParseResult Visit_ImportNode(ImportNode node)
    {
        return new ParseResult().Success(node);
    }

    private static ParseResult Visit_SuffixAssignNode(SuffixAssignNode node)
    {
        return new ParseResult().Success(node);
    }
}
