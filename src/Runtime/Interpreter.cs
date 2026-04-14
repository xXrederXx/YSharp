using YSharp.Common;
using YSharp.Lexer;
using YSharp.Parser.Nodes;
using YSharp.Runtime.Collections.List;
using YSharp.Runtime.Functions;
using YSharp.Runtime.Primitives.Number;
using YSharp.Runtime.Primitives.String;
using YSharp.Util;

namespace YSharp.Runtime;

public static class Interpreter
{
    public static RunTimeResult Visit(BaseNode node, Context context)
    {
        return node switch
        {
            NumberNode n => Visit_Number(n, context),
            StringNode n => Visit_String(n, context),
            ListNode n => Visit_List(n, context),
            BinOpNode n => Visit_BinaryOp(n, context),
            UnaryOpNode n => Visit_UnaryOp(n, context),
            VarAccessNode n => Visit_VarAccessNode(n, context),
            VarAssignNode n => Visit_VarAssignNode(n, context),
            DotVarAccessNode n => Visit_DotVarAccessNode(n, context),
            DotCallNode n => Visit_DotCallNode(n, context),
            IfNode n => Visit_IfNode(n, context),
            ForNode n => Visit_ForNode(n, context),
            WhileNode n => Visit_WhileNode(n, context),
            FuncDefNode n => Visit_FuncDefNode(n, context),
            CallNode n => Visit_CallNode(n, context),
            ReturnNode n => Visit_ReturnNode(n, context),
            ContinueNode => Visit_ContinueNode(),
            BreakNode => Visit_BreakNode(),
            TryCatchNode n => Visit_TryCatchNode(n, context),
            ImportNode n => Visit_ImportNode(n, context),
            SuffixAssignNode n => Visit_SuffixAssignNode(n, context),
            _ => new RunTimeResult().Failure(new InternalInterpreterError($"Tryed to operate on an unknown Node:\n{node}\n{context}")),
        };
    }

    private static RunTimeResult Visit_BinaryOp(BinOpNode node, Context context)
    {
        RunTimeResult res = new();
        Value left = res.Register(Visit(node.LeftNode, context));
        Value right = res.Register(Visit(node.RightNode, context));

        if (res.ShouldReturn())
            return res;

        Result<Value, Error> KeywordHandeler()
        {
            if (node.OpTok is Token<KeywordType> keywordTok)
            {
                if (keywordTok.ValueEquals(KeywordType.AND))
                    return left.AndedTo(right);

                if (keywordTok.ValueEquals(KeywordType.OR))
                    return left.OredTo(right);
            }

            throw new Exception("operator token type is wrong: " + node.OpTok.Type);
        }

        Result<Value, Error> result = node.OpTok.Type switch
        {
            TokenType.PLUS => left.AddedTo(right),
            TokenType.MINUS => left.SubedTo(right),
            TokenType.MUL => left.MuledTo(right),
            TokenType.DIV => left.DivedTo(right),
            TokenType.POW => left.PowedTo(right),
            TokenType.EE => left.GetComparisonEQ(right),
            TokenType.NE => left.GetComparisonNE(right),
            TokenType.LT => left.GetComparisonLT(right),
            TokenType.GT => left.GetComparisonGT(right),
            TokenType.LTE => left.GetComparisonLTE(right),
            TokenType.GTE => left.GetComparisonGTE(right),
            TokenType.KEYWORD => KeywordHandeler(),
            _ => throw new Exception("operator token type is wrong: " + node.OpTok.Type),
        };

        if (result.IsFailed)
            return res.Failure(result.GetError());

        return res.Success(result.GetValue().SetPos(node.StartPos, node.StartPos));
    }

    private static RunTimeResult Visit_BreakNode() => new RunTimeResult().SuccessBreak();

    private static RunTimeResult Visit_CallNode(CallNode node, Context context)
    {
        RunTimeResult res = new();
        List<Value> args = new(node.ArgNodes.Length);

        if (node.NodeToCall is VarAccessNode varAccessNode)
        {
            // just for error handeling
            varAccessNode.FromCall = true;
        }

        Value valueToCall = res.Register(Visit(node.NodeToCall, context));
        if (res.ShouldReturn())
            return res;

        if (valueToCall is not VBaseFunction function)
            return res.Failure(
                new InternalInterpreterError(
                    "The type of valueToCall is not supported. Type: " + valueToCall.GetType()
                )
            );

        VBaseFunction functionToCall = (VBaseFunction)
            function.Copy().SetPos(node.StartPos, node.EndPos);

        for (int i = 0; i < node.ArgNodes.Length; i++)
        {
            args.Add(res.Register(Visit(node.ArgNodes[i], context)));
            if (res.ShouldReturn())
                return res;
        }

        Value ret = res.Register(functionToCall.Execute(args));

        if (res.ShouldReturn())
            return res;

        try
        {
            ret = ret.Copy().SetPos(node.StartPos, node.EndPos).SetContext(context);
            return res.Success(ret);
        }
        catch
        {
            // the function returns an empty value, which can't be copied
            return res.Success(ValueNull.Instance);
        }
    }

    private static RunTimeResult Visit_ContinueNode() => new RunTimeResult().SuccessContinue();

    private static RunTimeResult Visit_DotCallNode(DotCallNode node, Context context)
    {
        RunTimeResult res = new();

        string funcName = node.FuncNameTok.Value;

        List<Value> argValue = new(node.ArgNodes.Length);
        for (int i = 0; i < node.ArgNodes.Length; i++)
        {
            BaseNode _node = node.ArgNodes[i];
            Value val = res.Register(Visit(_node, context));
            if (res.ShouldReturn())
                return res;

            argValue.Add(val);
        }

        Result<Value, Error> value = res.Register(Visit(node.Parent, context))
            .GetFunc(funcName ?? "null", argValue);

        if (res.ShouldReturn())
            return res;

        if (value.IsFailed)
            return res.Failure(value.GetError());

        try
        {
            return res.Success(
                value.GetValue().Copy().SetPos(node.StartPos, node.EndPos).SetContext(context)
            );
        }
        catch
        {
            // the function returns an empty value, which can't be copied
            return res.Success(value.GetValue());
        }
    }

    private static RunTimeResult Visit_DotVarAccessNode(DotVarAccessNode node, Context context)
    {
        RunTimeResult res = new();

        string varName = node.VarNameTok.Value;
        Result<Value, Error> value = res.Register(Visit(node.Parent, context)).GetVar(varName);
        if (res.ShouldReturn())
            return res;

        if (value.IsFailed)
            return res.Failure(value.GetError());

        if (value.GetValue() is null)
            return res.Failure(new VarNotFoundError(node.StartPos, varName, context));

        return res.Success(
            value.GetValue().Copy().SetPos(node.StartPos, node.EndPos).SetContext(context)
        );
    }

    private static RunTimeResult Visit_ForNode(ForNode node, Context context)
    {
        RunTimeResult res = new();
        Value startValue = res.Register(Visit(node.StartValueNode, context));
        Value endValue = res.Register(Visit(node.EndValueNode, context));

        if (res.ShouldReturn())
            return res;

        Value stepValue = res.Register(Visit(node.StepValueNode, context));
        if (res.ShouldReturn())
            return res;

        double StartNumber;
        double EndNumber;
        double StepNumber;

        if (startValue is not VNumber numStart)
        {
            return res.Failure(
                new WrongTypeError(startValue.StartPos, "The Start Number is not a Number", context)
            );
        }

        if (endValue is not VNumber numEnd)
        {
            return res.Failure(
                new WrongTypeError(startValue.StartPos, "The End Number is not a Number", context)
            );
        }

        if (stepValue is not VNumber numStep)
        {
            return res.Failure(
                new WrongTypeError(startValue.StartPos, "The Step Number is not a Number", context)
            );
        }

        StartNumber = numStart.value;
        EndNumber = numEnd.value;
        StepNumber = numStep.value;

        double i = StartNumber;
        Func<double, bool> condition;
        if (StepNumber >= 0)
            condition = i => i < EndNumber;
        else
            condition = i => i > EndNumber;

        if (context.SymbolTable is null)
        {
            return res.Failure(new InternalSymbolTableError(context));
        }

        string varName = node.VarNameTok.Value;

        while (condition(i))
        {
            context.SymbolTable.Set(varName, new VNumber(i));
            i += StepNumber;

            res.Register(Visit(node.BodyNode, context));
            if (res.ShouldReturn() && !res.loopContinue && res.loopBreak)
                return res;

            if (res.loopContinue)
                continue;

            if (res.loopBreak)
                break;
        }

        return res.Success(ValueNull.Instance);
    }

    private static RunTimeResult Visit_FuncDefNode(FuncDefNode node, Context context)
    {
        RunTimeResult res = new();
        string funcName = node.VarNameTok.Value;
        BaseNode bodyNode = node.BodyNode;

        List<string> argNames = new(node.ArgNameTokens.Length);
        for (int i = 0; i < node.ArgNameTokens.Length; i++)
        {
            Token<string> tok = (Token<string>)node.ArgNameTokens[i];
            argNames.Add(tok.Value);
        }

        VFunction funcValue = new(funcName, bodyNode, argNames, node.RetNull);
        funcValue.SetContext(context).SetPos(node.StartPos, node.EndPos);

        context.SymbolTable?.Set(funcName, funcValue);
        return res.Success(funcValue);
    }

    private static RunTimeResult Visit_IfNode(IfNode node, Context context)
    {
        RunTimeResult res = new();

        for (int i = 0; i < node.Cases.Length; i++)
        {
            BaseNode condition = node.Cases[i].Condition;
            BaseNode expr = node.Cases[i].Expression;
            Value conditionValue = res.Register(Visit(condition, context));
            if (res.ShouldReturn())
                return res;

            if (conditionValue.IsTrue())
            {
                Value exprValue = res.Register(Visit(expr, context));
                if (res.ShouldReturn())
                    return res;

                return res.Success(exprValue);
            }
        }

        if (node.ElseNode is not null and not NodeNull)
        {
            Value elseValue = res.Register(Visit(node.ElseNode, context));
            if (res.ShouldReturn())
                return res;

            return res.Success(elseValue);
        }

        return res.Success(ValueNull.Instance);
    }

    private static RunTimeResult Visit_ImportNode(ImportNode n, Context context)
    {
        RunTimeResult res = new();

        string filePath = n.PathTok.Value;
        if (Path.GetExtension(filePath) != ".dll")
            filePath += ".dll";

        if (!Path.IsPathRooted(filePath))
            filePath = ImportUtil.DefaultPath + filePath;

        if (!File.Exists(filePath))
        {
            res.Failure(
                new FileNotFoundError(
                    n.StartPos,
                    "The package at the import path "
                        + n.PathTok.Value
                        + " was calculated to be at "
                        + filePath
                        + " sadly, there is no such file.",
                    context
                )
            );
        }

        List<ExposedClassData> exposeds = ImportUtil.Load(filePath, out string err);
        if (err != string.Empty)
            return res.Failure(new InvalidLoadedModuleError(n.StartPos, err, context));

        //TODO: Implement call logic
        System.Console.WriteLine(exposeds.Count);
        System.Console.WriteLine(string.Join(", ", exposeds));

        return res;
    }

    private static RunTimeResult Visit_List(ListNode node, Context context)
    {
        RunTimeResult res = new();
        List<Value> elements = new(node.ElementNodes.Length);
        for (int i = 0; i < node.ElementNodes.Length; i++)
        {
            BaseNode elementNode = node.ElementNodes[i];
            elements.Add(res.Register(Visit(elementNode, context)));
            if (res.ShouldReturn())
                return res;
        }

        return res.Success(
            new VList(elements).SetContext(context).SetPos(node.StartPos, node.EndPos)
        );
    }

    private static RunTimeResult Visit_Number(NumberNode node, Context context) =>
        new RunTimeResult().Success(
            new VNumber(node.Tok.Value).SetContext(context).SetPos(node.StartPos, node.EndPos)
        );

    private static RunTimeResult Visit_ReturnNode(ReturnNode node, Context context)
    {
        RunTimeResult res = new();
        Value value;

        value = res.Register(Visit(node.NodeToReturn, context));
        if (res.ShouldReturn())
            return res;

        return res.SuccessReturn(value);
    }

    private static RunTimeResult Visit_String(StringNode node, Context context) =>
        new RunTimeResult().Success(
            new VString(node.Tok.Value).SetContext(context).SetPos(node.StartPos, node.EndPos)
        );

    private static RunTimeResult Visit_SuffixAssignNode(SuffixAssignNode node, Context context)
    {
        RunTimeResult res = new();
        string varName = node.VarName;

        if (context.SymbolTable is null)
        {
            return res.Failure(new InternalSymbolTableError(context));
        }

        Value oldVal = context.SymbolTable.Get(varName);
        if (oldVal is not VNumber numNode)
        {
            return res.Failure(
                new WrongTypeError(
                    node.StartPos,
                    "You can only use this operator for Numbers",
                    context
                )
            );
        }

        Result<Value, Error> newValue;
        if (node.IsAdd)
            newValue = numNode.AddedTo(new VNumber(1));
        else
            newValue = numNode.SubedTo(new VNumber(1));

        if (newValue.IsFailed)
            return res.Failure(newValue.GetError());

        context.SymbolTable.Set(varName, newValue.GetValue());
        return res.Success(newValue.GetValue());
    }

    private static RunTimeResult Visit_TryCatchNode(TryCatchNode node, Context context)
    {
        RunTimeResult res = new();

        Value val = res.Register(Visit(node.TryNode, context));
        if (!res.error.IsError)
            return res.Success(val);

        if (context.SymbolTable is null)
        {
            return res.Failure(new InternalSymbolTableError(context));
        }

        if (node.CatchVarName is not null)
            context.SymbolTable.Set(node.CatchVarName.Value, new VString(res.error.ToString()));

        Value catchVal = res.Register(Visit(node.CatchNode, context));
        if (res.ShouldReturn())
            return res;

        return res.Success(catchVal);
    }

    private static RunTimeResult Visit_UnaryOp(UnaryOpNode node, Context context)
    {
        RunTimeResult res = new();
        Value value = res.Register(Visit(node.Node, context));
        if (res.ShouldReturn())
            return res;

        Result<Value, Error> result;
        if (node.OpTok.IsType(TokenType.MINUS) && value is VNumber)
        {
            result = value.MuledTo(new VNumber(-1));
        }
        else if (node.OpTok.IsType(TokenType.PLUS) && value is VNumber)
        {
            result = Result<Value, Error>.Success(value);
        }
        else if (
            node.OpTok is Token<KeywordType> keywordTok
            && keywordTok.ValueEquals(KeywordType.NOT)
        )
        {
            result = value.Notted();
        }
        else
        {
            result = Result<Value, Error>.Fail(
                new InternalInterpreterError(
                    $"Invalid Unary Operation using {node.OpTok} and {node.Node}"
                )
            );
        }

        if (result.IsFailed)
            return res.Failure(result.GetError());

        return res.Success(result.GetValue().SetPos(node.StartPos, node.StartPos));
    }

    private static RunTimeResult Visit_VarAccessNode(VarAccessNode node, Context context)
    {
        RunTimeResult res = new();

        if (context.SymbolTable is null)
        {
            return res.Failure(new InternalSymbolTableError(context));
        }

        string varName = node.VarNameTok.Value;
        Value value = context.SymbolTable.Get(varName);

        if (value is ValueNull)
        {
            if (node.FromCall)
                return res.Failure(new FuncNotFoundError(node.StartPos, varName, context));
            return res.Failure(new VarNotFoundError(node.StartPos, varName, context));
        }

        return res.Success(value.Copy().SetPos(node.StartPos, node.EndPos).SetContext(context));
    }

    private static RunTimeResult Visit_VarAssignNode(VarAssignNode node, Context context)
    {
        RunTimeResult res = new();
        string varName = node.VarNameTok.Value;
        Value value = res.Register(Visit(node.ValueNode, context));
        if (res.ShouldReturn())
            return res;

        if (context.SymbolTable is null)
        {
            return res.Failure(new InternalSymbolTableError(context));
        }

        context.SymbolTable.Set(varName, value);
        return res.Success(value);
    }

    private static RunTimeResult Visit_WhileNode(WhileNode node, Context context)
    {
        RunTimeResult res = new();

        while (true)
        {
            Value condition = res.Register(Visit(node.ConditionNode, context));
            if (res.ShouldReturn())
                return res;

            if (!condition.IsTrue())
                break;

            res.Register(Visit(node.BodyNode, context));
            if (res.ShouldReturn() && !res.loopContinue && res.loopBreak)
                return res;

            if (res.loopContinue)
                continue;

            if (res.loopBreak)
                break;
        }

        return res.Success(ValueNull.Instance);
    }
}
