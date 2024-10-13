namespace YSharp_2._0;

public class RunTimeResult
{
    public Value value = ValueNull.Instance;
    public Error error = NoError.Instance;
    public Value funcReturnValue = ValueNull.Instance;
    public bool loopContinue = false;
    public bool loopBreak = false;

    public void Reset()
    {
        value = ValueNull.Instance;
        error = NoError.Instance;
        funcReturnValue = ValueNull.Instance;
        loopContinue = false;
        loopBreak = false;
    }
    public Value Regrister(RunTimeResult res)
    {
        error = res.error;
        funcReturnValue = res.funcReturnValue;
        loopContinue = res.loopContinue;
        loopBreak = res.loopBreak;
        return res.value;
    }

    public RunTimeResult Success(Value value)
    {
        if (error.IsError) Console.WriteLine("error deleted:\n" + error.ToString());
        Reset();
        this.value = value;
        return this;
    }
    public RunTimeResult SuccessReturn(Value value)
    {
        Reset();
        funcReturnValue = value;
        return this;
    }
    public RunTimeResult SuccessContinue()
    {
        Reset();
        loopContinue = true;
        return this;
    }

    public RunTimeResult SuccessBreak()
    {
        Reset();
        loopBreak = true;
        return this;
    }
    public RunTimeResult Failure(Error error)
    {
        Reset();
        this.error = error;
        return this;
    }

    public bool ShouldReturn()
    {
        return error.IsError || funcReturnValue is not ValueNull || loopContinue || loopBreak;
    }
}

public class Context
{
    public readonly string? displayName;
    public readonly Context? parent;
    public readonly Position parentEntryPos;
    public SymbolTable? symbolTable;

    public Context(string? displayName, Context? parent, Position parentEntryPos)
    {
        this.displayName = displayName;
        this.parentEntryPos = parentEntryPos;
        this.parent = parent;
    }
    public Context()
    {
        displayName = null;
        parent = null;
        parentEntryPos = new();
    }
}

public class SymbolTable
{
    public Dictionary<string, Value> symbols = new Dictionary<string, Value>();
    public SymbolTable? parent = null;

    private Value GetFromParent(string name, Value defaultValue)
    {
        return parent != null ? parent.Get(name, defaultValue) : defaultValue;
    }

    public Value Get(string? name)
    {
        if (string.IsNullOrEmpty(name))
        {
            return ValueNull.Instance;
        }

        return symbols.TryGetValue(name, out var value)
            ? value
            : GetFromParent(name, ValueNull.Instance);
    }

    public Value Get(string? name, Value defaultValue)
    {
        if (string.IsNullOrEmpty(name))
        {
            return defaultValue;
        }

        return symbols.TryGetValue(name, out var value)
            ? value
            : GetFromParent(name, defaultValue);
    }

    public void Set(string name, Value value)
    {
        symbols[name] = value;
    }

    public void Remove(string name)
    {
        symbols.Remove(name);
    }
}

public class Interpreter
{
    public RunTimeResult Visit(Node node, Context context)
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
            ContinueNode n => Visit_ContinueNode(n, context),
            BreakNode n => Visit_BreakNode(n, context),
            _ => throw new Exception("No method found for " + node.GetType())
        };
    }

    // returns a number
    private static RunTimeResult Visit_Number(NumberNode node, Context context)
    {
        double value;
        if (node.tok.Value == null)
        {
            value = 0;
        }
        else
        {
            value = Convert.ToDouble(node.tok.Value);
        }
        return new RunTimeResult().Success(new Number(value).SetContext(context).SetPos(node.StartPos, node.EndPos));
    }
    private static RunTimeResult Visit_String(StringNode node, Context context)
    {
        string? value = (string?)node.tok.Value;
        return new RunTimeResult().Success(new String(value ?? "").SetContext(context).SetPos(node.StartPos, node.EndPos));
    }
    private RunTimeResult Visit_List(ListNode node, Context context)
    {
        RunTimeResult res = new();
        List<Value> elements = [];
        foreach (Node elementNode in node.elementNodes)
        {
            elements.Add(res.Regrister(Visit(elementNode, context)));
            if (res.ShouldReturn())
            {
                return res;
            }
        }
        return res.Success(new List(elements).SetContext(context).SetPos(node.StartPos, node.EndPos));
    }
    // returns a calculated number
    private RunTimeResult Visit_BinaryOp(BinOpNode node, Context context)
    {
        RunTimeResult res = new();

        Value left = res.Regrister(Visit(node.leftNode, context));
        if (res.ShouldReturn())
        {
            return res;
        }
        Value right = res.Regrister(Visit(node.rightNode, context));
        if (res.ShouldReturn())
        {
            return res;
        }

        ValueError result;
        // Arethmetic
        if (node.opTok.Type == TokenType.PLUS)
        {
            result = left.addedTo(right);
        }
        else if (node.opTok.Type == TokenType.MINUS)
        {
            result = left.subedTo(right);
        }
        else if (node.opTok.Type == TokenType.MUL)
        {
            result = left.muledTo(right);
        }
        else if (node.opTok.Type == TokenType.DIV)
        {
            result = left.divedTo(right);
        }
        else if (node.opTok.Type == TokenType.POW)
        {
            result = left.powedTo(right);
        }
        // comparison
        else if (node.opTok.Type == TokenType.EE)
        {
            result = left.getComparisonEQ(right);
        }
        else if (node.opTok.Type == TokenType.NE)
        {
            result = left.getComparisonNE(right);
        }
        else if (node.opTok.Type == TokenType.LT)
        {
            result = left.getComparisonLT(right);
        }
        else if (node.opTok.Type == TokenType.GT)
        {
            result = left.getComparisonGT(right);
        }
        else if (node.opTok.Type == TokenType.LTE)
        {
            result = left.getComparisonLTE(right);
        }
        else if (node.opTok.Type == TokenType.GTE)
        {
            result = left.getComparisonGTE(right);
        }
        else if (node.opTok.Matches(TokenType.KEYWORD, "AND"))
        {
            result = left.andedTo(right);
        }
        else if (node.opTok.Matches(TokenType.KEYWORD, "OR"))
        {
            result = left.oredTo(right);
        }
        else
        {
            throw new Exception("operator token type is wrong: " + node.opTok.Type);
        }

        if (result.error.IsError)
        {
            return res.Failure(result.error);
        }
        else
        {
            return res.Success(result.value.SetPos(node.StartPos, node.StartPos));
        }
    }
    private RunTimeResult Visit_UnaryOp(UnaryOpNode node, Context context)
    {
        RunTimeResult res = new();
        Value number = res.Regrister(Visit(node.node, context));
        if (res.ShouldReturn())
        {
            return res;
        }
        else if (number == null)
        {
            return res.Failure(new InternalError("Cast in Visit_UnaryOp failed"));
        }

        ValueError result;
        if (node.opTok.Type == TokenType.MINUS && number is Number)
        {
            result = number.muledTo(new Number(-1));
        }
        else if (node.opTok.Matches(TokenType.KEYWORD, "NOT"))
        {
            result = number.notted();
        }
        else
        {
            result = (number, NoError.Instance);
        }

        if (result.error.IsError)
        {
            return res.Failure(result.error);
        }
        else
        {
            return res.Success(result.value.SetPos(node.StartPos, node.StartPos));
        }
    }
    private static RunTimeResult Visit_VarAccessNode(VarAccessNode node, Context context)
    {
        RunTimeResult res = new();

        if (context.symbolTable == null)
        {
            return res.Failure(new InternalError("There is no SymbolTable in this context"));
        }

        string? varName = (string?)node.varNameTok.Value;
        Value value = context.symbolTable.Get(varName);

        if (value is null or ValueNull)
        {
            return res.Failure(new RunTimeError(node.StartPos, $"{varName} is not defined", context));
        }
        /* if (value is (ValueNull)){
            return res.success(value);
        } */
        return res.Success(value.copy().SetPos(node.StartPos, node.EndPos).SetContext(context));
    }
    private RunTimeResult Visit_VarAssignNode(VarAssignNode node, Context context)
    {
        RunTimeResult res = new();
        string? varName = (string?)node.varNameTok.Value;
        Value value = res.Regrister(Visit(node.valueNode, context));
        if (res.ShouldReturn())
        {
            return res;
        }
        if (varName == null)
        {
            return res.Failure(new InternalError("Var name is null"));
        }

        if (context.symbolTable == null)
        {
            return res.Failure(new InternalError("Symbol Table is null"));
        }

        context.symbolTable.Set(varName, value);
        return res.Success(value);
    }
    private RunTimeResult Visit_DotVarAccessNode(DotVarAccessNode node, Context context)
    {
        RunTimeResult res = new();


        string? varName = (string?)node.varNameTok.Value;
        ValueError value = res.Regrister(Visit(node.parent, context)).GetVar(varName ?? "null");
        if (res.ShouldReturn())
        {
            return res;
        }
        if (value.error.IsError)
        {
            return res.Failure(value.error);
        }
        if (value.ValueIsNull)
        {
            return res.Failure(new RunTimeError(node.StartPos, $"{varName} var is not defined", context));
        }
        /* if (value is (ValueNull)){
            return res.success(value);
        } */
        return res.Success(value.value.copy().SetPos(node.StartPos, node.EndPos).SetContext(context));
    }
    private RunTimeResult Visit_DotCallNode(DotCallNode node, Context context)
    {
        RunTimeResult res = new();


        string? funcName = (string?)node.funcNameTok.Value;

        List<Value> argValue = [];
        foreach (Node _node in node.argNodes)
        {
            Value val = res.Regrister(Visit(_node, context));
            if (res.ShouldReturn())
            {
                return res;
            }
            argValue.Add(val);
        }

        ValueError value = res.Regrister(Visit(node.parent, context)).GetFunc(funcName ?? "null", argValue);

        if (res.ShouldReturn())
        {
            return res;
        }
        if (value.error.IsError)
        {
            return res.Failure(value.error);
        }
        /* if (value is (ValueNull)){
            return res.success(value);
        } */
        try
        {
            return res.Success(value.value.copy().SetPos(node.StartPos, node.EndPos).SetContext(context));
        }
        catch
        { // the function returns an emptie value, which cnat be copied
            return res.Success(value.value);
        }
    }
    private RunTimeResult Visit_IfNode(IfNode node, Context context)
    {
        RunTimeResult res = new();

        for (int i = 0; i < node.cases.Count; i++)
        {
            Node condition = node.cases[i].condition;
            Node expr = node.cases[i].expresion;
            bool retNull = node.cases[i].returnNull;

            Value conditionValue = res.Regrister(Visit(condition, context));
            if (res.ShouldReturn())
            {
                return res;
            }

            if (conditionValue.isTrue())
            {
                Value exprValue = res.Regrister(Visit(expr, context));
                if (res.ShouldReturn())
                {
                    return res;
                }
                return res.Success(retNull ? ValueNull.Instance : exprValue);
            }
        }

        if (node.elseCase.Node != null && node.elseCase.Bool != null)
        {
            Value elseValue = res.Regrister(Visit(node.elseCase.Node, context));
            bool retNull = node.elseCase.Bool ?? true;
            if (res.ShouldReturn())
            {
                return res;
            }
            return res.Success(retNull ? ValueNull.Instance : elseValue);
        }
        return res.Success(ValueNull.Instance);
    }
    private RunTimeResult Visit_ForNode(ForNode node, Context context)
    {
        RunTimeResult res = new();
        Number? startValue = res.Regrister(Visit(node.startValueNode, context)) as Number;
        if (res.ShouldReturn())
        {
            return res;
        }

        Number? endValue = res.Regrister(Visit(node.endValueNode, context)) as Number;
        if (res.ShouldReturn())
        {
            return res;
        }

        Number? stepValue;
        if (node.stepValueNode != null)
        {
            stepValue = res.Regrister(Visit(node.stepValueNode, context)) as Number;
            if (res.ShouldReturn())
            {
                return res;
            }
        }
        else
        {
            stepValue = new Number(1);
        }

        if (startValue == null || endValue == null || stepValue == null)
        {
            return res.Failure(new InternalError($"startValue == null {startValue == null} endValue == null {endValue == null} stepValue == null {stepValue == null}"));
        }

        double i = startValue.value;
        Func<double, bool> condition;
        if (stepValue.value >= 0)
        {
            condition = i => i < endValue.value;
        }
        else
        {
            condition = i => i > endValue.value;
        }

        if (context.symbolTable == null)
        {
            return res.Failure(new InternalError("No symbol Table"));
        }
        string? varName = (string?)node.varNameTok.Value;
        if (varName == null)
        {
            return res.Failure(new InternalError($"No value for varName -> {varName}"));
        }
        while (condition(i))
        {
            context.symbolTable.Set(varName, new Number(i));
            i += stepValue.value;

            res.Regrister(Visit(node.bodyNode, context));
            if (res.ShouldReturn() && !res.loopContinue && res.loopBreak)
            {
                return res;
            }
            if (res.loopContinue)
            {
                continue;
            }
            if (res.loopBreak)
            {
                break;
            }
        }
        return res.Success(ValueNull.Instance);
    }
    private RunTimeResult Visit_WhileNode(WhileNode node, Context context)
    {
        RunTimeResult res = new();
        if (res.ShouldReturn())
        {
            return res;
        }

        while (true)
        {
            Value condition = res.Regrister(Visit(node.conditionNode, context));
            if (res.ShouldReturn())
            {
                return res;
            }
            if (!condition.isTrue())
            {
                break;
            }
            res.Regrister(Visit(node.bodyNode, context));
            if (res.ShouldReturn() && !res.loopContinue && res.loopBreak)
            {
                return res;
            }
            if (res.loopContinue)
            {
                continue;
            }
            if (res.loopBreak)
            {
                break;
            }
        }
        return res.Success(ValueNull.Instance);
    }
    private static RunTimeResult Visit_FuncDefNode(FuncDefNode node, Context context)
    {
        RunTimeResult res = new();
        string? funcName = null;
        if (node.varNameTok != null)
        {
            funcName = (string?)node.varNameTok.Value;
        }
        Node bodyNode = node.bodyNode;

        List<string> argNames = [];
        foreach (Token tok in node.argNameToks)
        {
            string? name = (string?)tok.Value;
            if (name != null)
            {
                argNames.Add(name);
            }
        }

        Function funcValue = new(funcName, bodyNode, argNames, node.retNull);
        funcValue.SetContext(context).SetPos(node.StartPos, node.EndPos);

        if (node.varNameTok != null && context.symbolTable != null && funcName != null)
        {
            context.symbolTable.Set(funcName, funcValue);
        }
        return res.Success(funcValue);
    }
    private RunTimeResult Visit_CallNode(CallNode node, Context context)
    {
        RunTimeResult res = new();
        List<Value> args = [];

        Value valueToCall = res.Regrister(Visit(node.nodeToCall, context));
        if (valueToCall == null)
        {
            return res.Failure(new InternalError("cast failed for valueToCall "));
        }
        if (res.ShouldReturn())
        {
            return res;
        }
        if (valueToCall is BuiltInFunction BIfunction)
        {
            valueToCall = BIfunction.copy();
        }
        else if (valueToCall is Function function)
        {
            valueToCall = function.copy();
        }
        else
        {
            Console.WriteLine("Type of valueToCall not supported :" + valueToCall.GetType());
        }


        valueToCall.SetPos(node.StartPos, node.EndPos);

        foreach (Node arg in node.argNodes)
        {
            args.Add(res.Regrister(Visit(arg, context)));
            if (res.ShouldReturn())
            {
                return res;
            }
        }
        Value ret;
        if (valueToCall is BuiltInFunction _BIfunction)
        {
            ret = res.Regrister(_BIfunction.execute(args));
        }
        else if (valueToCall is Function _function)
        {
            ret = res.Regrister(_function.Execute(args));
        }
        else
        {
            ret = ValueNull.Instance;
        }

        if (res.ShouldReturn())
        {
            return res;
        }
        try
        {
            ret = ret.copy().SetPos(node.StartPos, node.EndPos).SetContext(context);
            return res.Success(ret);
        }
        catch
        { // the function returns an emptie value, which cnat be copied
            return res.Success(ValueNull.Instance);
        }
    }
    private RunTimeResult Visit_ReturnNode(ReturnNode node, Context context)
    {
        RunTimeResult res = new();
        Value value;
        if (node.nodeToReturn != null)
        {
            value = res.Regrister(Visit(node.nodeToReturn, context));
            if (res.ShouldReturn())
            {
                return res;
            }
        }
        else
        {
            value = ValueNull.Instance;
        }
        return res.SuccessReturn(value);
    }
    private static RunTimeResult Visit_ContinueNode(ContinueNode node, Context context)
    {
        return new RunTimeResult().SuccessContinue();
    }
    private static RunTimeResult Visit_BreakNode(BreakNode node, Context context)
    {
        return new RunTimeResult().SuccessBreak();
    }
}

