using YSharp.Types.AST;
using YSharp.Types.Common;
using YSharp.Types.Interpreter;
using YSharp.Types.Interpreter.Collection;
using YSharp.Types.Interpreter.Function;
using YSharp.Types.Interpreter.Internal;
using YSharp.Types.Interpreter.Primitives;
using YSharp.Types.Lexer;
using YSharp.Utils;

namespace YSharp.Core;

public static class Interpreter{
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
            _ => Vistit_ErrorNode(node, context)
        };
    }

    private static RunTimeResult Visit_BinaryOp(BinOpNode node, Context context)
    {
        RunTimeResult res = new();
        Value left = res.Regrister(Visit(node.leftNode, context));
        Value right = res.Regrister(Visit(node.rightNode, context));

        if (res.ShouldReturn()) return res;

        ValueAndError KeywordHandeler()
        {
            if (node.opTok.IsMatchingKeyword(KeywordType.AND)) return left.AndedTo(right);

            if (node.opTok.IsMatchingKeyword(KeywordType.OR)) return left.OredTo(right);

            throw new Exception("operator token type is wrong: " + node.opTok.Type);
        }

        ValueAndError result = node.opTok.Type switch
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
            _ => throw new Exception("operator token type is wrong: " + node.opTok.Type)
        };

        if (result.Error.IsError) return res.Failure(result.Error);

        return res.Success(result.Value.SetPos(node.StartPos, node.StartPos));
    }

    private static RunTimeResult Visit_BreakNode() => new RunTimeResult().SuccessBreak();

    private static RunTimeResult Visit_CallNode(CallNode node, Context context)
    {
        RunTimeResult res = new();
        List<Value> args = new(node.argNodes.Length);

        if (node.nodeToCall is VarAccessNode varAccessNode)
        {
            // just for error handeling
            varAccessNode.fromCall = true;
        }

        Value? valueToCall = res.Regrister(Visit(node.nodeToCall, context));

        if (res.ShouldReturn()) return res;

        switch (valueToCall)
        {
            case VBuiltInFunction function:
                valueToCall = function.Copy();
                break;

            case VFunction function:
                valueToCall = function.Copy();
                break;

            default:
                return res.Failure(new InternalInterpreterError(
                    "The type of valueToCall is not supported. Type: " +
                    valueToCall.GetType()));
        }



        valueToCall.SetPos(node.StartPos, node.EndPos);

        for (int i = 0; i < node.argNodes.Length; i++)
        {
            args.Add(res.Regrister(Visit(node.argNodes[i], context)));
            if (res.ShouldReturn()) return res;
        }

        Value ret;
        if (valueToCall is VBuiltInFunction _BIfunction)
            ret = res.Regrister(_BIfunction.Execute(args));
        else if (valueToCall is VFunction _function)
            ret = res.Regrister(_function.Execute(args));
        else
            ret = ValueNull.Instance;

        if (res.ShouldReturn()) return res;

        try
        {
            ret = ret.Copy().SetPos(node.StartPos, node.EndPos).SetContext(context);
            return res.Success(ret);
        }
        catch
        {
            // the function returns an emptie value, which cnat be copied
            return res.Success(ValueNull.Instance);
        }
    }

    private static RunTimeResult Visit_ContinueNode() => new RunTimeResult().SuccessContinue();

    private static RunTimeResult Visit_DotCallNode(DotCallNode node, Context context)
    {
        RunTimeResult res = new();

        string funcName = node.funcNameTok.Value;

        List<Value> argValue = new(node.argNodes.Length);
        for (int i = 0; i < node.argNodes.Length; i++)
        {
            BaseNode _node = node.argNodes[i];
            Value val = res.Regrister(Visit(_node, context));
            if (res.ShouldReturn()) return res;

            argValue.Add(val);
        }

        ValueAndError value = res.Regrister(Visit(node.parent, context))
            .GetFunc(funcName ?? "null", argValue);

        if (res.ShouldReturn()) return res;

        if (value.Error.IsError) return res.Failure(value.Error);

        try
        {
            return res.Success(
                value.Value.Copy().SetPos(node.StartPos, node.EndPos).SetContext(context)
            );
        }
        catch
        {
            // the function returns an emptie value, which cnat be copied
            return res.Success(value.Value);
        }
    }

    private static RunTimeResult Visit_DotVarAccessNode(DotVarAccessNode node, Context context)
    {
        RunTimeResult res = new();

        string varName = node.varNameTok.Value;
        ValueAndError value = res.Regrister(Visit(node.parent, context)).GetVar(varName ?? "null");
        if (res.ShouldReturn()) return res;

        if (value.Error.IsError) return res.Failure(value.Error);

        if (value.ValueIsNull) return res.Failure(new VarNotFoundError(node.StartPos, varName!, context));

        return res.Success(
            value.Value.Copy().SetPos(node.StartPos, node.EndPos).SetContext(context)
        );
    }

    private static RunTimeResult Visit_ForNode(ForNode node, Context context)
    {
        RunTimeResult res = new();
        Value startValue = res.Regrister(Visit(node.startValueNode, context));
        Value endValue = res.Regrister(Visit(node.endValueNode, context));

        if (res.ShouldReturn()) return res;

        Value stepValue;
        if (node.stepValueNode is not null)
        {
            stepValue = res.Regrister(Visit(node.stepValueNode, context));
            if (res.ShouldReturn()) return res;
        }
        else
            stepValue = new VNumber(1);

        double StartNumber;
        double EndNumber;
        double StepNumber;

        if (startValue is not VNumber numStart)
        {
            return res.Failure(
                new WrongTypeError(startValue.startPos, "The Start Number is not a Number", context)
            );
        }

        if (endValue is not VNumber numEnd)
        {
            return res.Failure(
                new WrongTypeError(startValue.startPos, "The End Number is not a Number", context)
            );
        }

        if (stepValue is not VNumber numStep)
        {
            return res.Failure(
                new WrongTypeError(startValue.startPos, "The Step Number is not a Number", context)
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

        if (context.symbolTable is null)
        {
            return res.Failure(
                new InternalSymbolTableError(context)
            );
        }

        string varName = node.varNameTok.Value;

        while (condition(i))
        {
            context.symbolTable.Set(varName, new VNumber(i));
            i += StepNumber;

            res.Regrister(Visit(node.bodyNode, context));
            if (res.ShouldReturn() && !res.loopContinue && res.loopBreak) return res;

            if (res.loopContinue) continue;

            if (res.loopBreak) break;
        }

        return res.Success(ValueNull.Instance);
    }

    private static RunTimeResult Visit_FuncDefNode(FuncDefNode node, Context context)
    {
        RunTimeResult res = new();
        string funcName = node.varNameTok.Value;
        BaseNode bodyNode = node.bodyNode;

        List<string> argNames = new(node.argNameToks.Length);
        for (int i = 0; i < node.argNameToks.Length; i++)
        {
            Token<string> tok = (Token<string>)node.argNameToks[i];
            argNames.Add(tok.Value);
        }

        VFunction funcValue = new(funcName, bodyNode, argNames, node.retNull);
        funcValue.SetContext(context).SetPos(node.StartPos, node.EndPos);

        context.symbolTable?.Set(funcName, funcValue);
        return res.Success(funcValue);
    }

    private static RunTimeResult Visit_IfNode(IfNode node, Context context)
    {
        RunTimeResult res = new();

        for (int i = 0; i < node.cases.Length; i++)
        {
            BaseNode condition = node.cases[i].condition;
            BaseNode expr = node.cases[i].expression;
            Value conditionValue = res.Regrister(Visit(condition, context));
            if (res.ShouldReturn()) return res;

            if (conditionValue.IsTrue())
            {
                Value exprValue = res.Regrister(Visit(expr, context));
                if (res.ShouldReturn()) return res;

                return res.Success(exprValue);
            }
        }

        if (node.elseNode is not null and not NodeNull)
        {
            Value elseValue = res.Regrister(Visit(node.elseNode, context));
            if (res.ShouldReturn()) return res;

            return res.Success(elseValue);
        }

        return res.Success(ValueNull.Instance);
    }

    private static RunTimeResult Visit_ImportNode(ImportNode n, Context context)
    {
        RunTimeResult res = new();

        string filePath = n.PathTok.Value;
        if (Path.GetExtension(filePath) != ".dll") filePath += ".dll";

        if (Path.IsPathRooted(filePath)) filePath = ImportUtil.DefaultPath + filePath;

        if (!File.Exists(filePath))
        {
            res.Failure(
                new FileNotFoundError(
                    n.StartPos,
                    "The packege at the import path "
                    + n.PathTok.Value
                    + " was calculated to be at "
                    + filePath
                    + " sadly, there is no such file.",
                    context
                )
            );
        }

        List<ExposedClassData> exposeds = ImportUtil.Load(filePath, out string err);
        if (err != string.Empty) return res.Failure(new InvalidLoadedModuleError(n.StartPos, err, context));

        //TODO: Implement call logic

        return res;
    }

    private static RunTimeResult Visit_List(ListNode node, Context context)
    {
        RunTimeResult res = new();
        List<Value> elements = new(node.elementNodes.Length);
        for (int i = 0; i < node.elementNodes.Length; i++)
        {
            BaseNode elementNode = node.elementNodes[i];
            elements.Add(res.Regrister(Visit(elementNode, context)));
            if (res.ShouldReturn()) return res;
        }

        return res.Success(
            new VList(elements).SetContext(context).SetPos(node.StartPos, node.EndPos)
        );
    }

    private static RunTimeResult Visit_Number(NumberNode node, Context context) =>
        new RunTimeResult().Success(
            new VNumber(node.tok.Value).SetContext(context).SetPos(node.StartPos, node.EndPos)
        );

    private static RunTimeResult Visit_ReturnNode(ReturnNode node, Context context)
    {
        RunTimeResult res = new();
        Value value;
        if (node.nodeToReturn is not null)
        {
            value = res.Regrister(Visit(node.nodeToReturn, context));
            if (res.ShouldReturn()) return res;
        }
        else
            value = ValueNull.Instance;

        return res.SuccessReturn(value);
    }

    private static RunTimeResult Visit_String(StringNode node, Context context) =>
        new RunTimeResult().Success(
            new VString(node.tok.Value).SetContext(context).SetPos(node.StartPos, node.EndPos)
        );

    private static RunTimeResult Visit_SuffixAssignNode(SuffixAssignNode node, Context context)
    {
        RunTimeResult res = new();
        string varName = node.varName;

        if (context.symbolTable is null)
        {
            return res.Failure(
                new InternalSymbolTableError(context)
            );
        }

        Value oldVal = context.symbolTable.Get(varName);
        if (oldVal is not VNumber numNode)
        {
            return res.Failure(
                new WrongTypeError(
                    node.StartPos,
                    "You can only use this operateor for Numbers",
                    context
                )
            );
        }

        ValueAndError newValue;
        if (node.isAdd)
            newValue = numNode.AddedTo(new VNumber(1));
        else
            newValue = numNode.SubedTo(new VNumber(1));

        if (newValue.Error.IsError)
            return res.Failure(newValue.Error);

        context.symbolTable.Set(varName, newValue.Value);
        return res.Success(newValue.Value);
    }

    private static RunTimeResult Visit_TryCatchNode(TryCatchNode node, Context context)
    {
        RunTimeResult res = new();

        Value val = res.Regrister(Visit(node.TryNode, context));
        if (!res.error.IsError) return res.Success(val);

        if (context.symbolTable is null)
        {
            return res.Failure(
                new InternalSymbolTableError(context)
            );
        }

        if (node.ChatchVarName is not null)
            context.symbolTable.Set(node.ChatchVarName.Value, new VString(res.error.ToString()));

        Value catchVal = res.Regrister(Visit(node.CatchNode, context));
        if (res.ShouldReturn()) return res;

        return res.Success(catchVal);
    }

    private static RunTimeResult Visit_UnaryOp(UnaryOpNode node, Context context)
    {
        RunTimeResult res = new();
        Value value = res.Regrister(Visit(node.node, context));
        if (res.ShouldReturn()) return res;

        ValueAndError result;
        if (node.opTok.IsType(TokenType.MINUS) && value is VNumber)
            result = value.MuledTo(new VNumber(-1));
        else if (node.opTok.IsMatchingKeyword(KeywordType.NOT))
            result = value.Notted();
        else
            result = (value, ErrorNull.Instance);

        if (result.Error.IsError) return res.Failure(result.Error);

        return res.Success(result.Value.SetPos(node.StartPos, node.StartPos));
    }

    private static RunTimeResult Visit_VarAccessNode(VarAccessNode node, Context context)
    {
        RunTimeResult res = new();

        if (context.symbolTable is null)
        {
            return res.Failure(
                new InternalSymbolTableError(context)
            );
        }

        string varName = node.varNameTok.Value;
        Value value = context.symbolTable.Get(varName);

        if (value is ValueNull)
        {
            if (node.fromCall)
                return res.Failure(new FuncNotFoundError(node.StartPos, varName, context));
            return res.Failure(new VarNotFoundError(node.StartPos, varName, context));
        }

        return res.Success(value.Copy().SetPos(node.StartPos, node.EndPos).SetContext(context));
    }

    private static RunTimeResult Visit_VarAssignNode(VarAssignNode node, Context context)
    {
        RunTimeResult res = new();
        string varName = node.varNameTok.Value;
        Value value = res.Regrister(Visit(node.valueNode, context));
        if (res.ShouldReturn()) return res;

        if (context.symbolTable is null)
        {
            return res.Failure(
                new InternalSymbolTableError(context)
            );
        }

        context.symbolTable.Set(varName, value);
        return res.Success(value);
    }

    private static RunTimeResult Visit_WhileNode(WhileNode node, Context context)
    {
        RunTimeResult res = new();

        while (true)
        {
            Value condition = res.Regrister(Visit(node.conditionNode, context));
            if (res.ShouldReturn()) return res;

            if (!condition.IsTrue()) break;

            res.Regrister(Visit(node.bodyNode, context));
            if (res.ShouldReturn() && !res.loopContinue && res.loopBreak) return res;

            if (res.loopContinue) continue;

            if (res.loopBreak) break;
        }

        return res.Success(ValueNull.Instance);
    }

    private static RunTimeResult Vistit_ErrorNode(BaseNode node, Context ctx)
    {
        Console.WriteLine("No method found for " + node.GetType() + ctx);
        return new RunTimeResult().Success(ValueNull.Instance);
    }
}