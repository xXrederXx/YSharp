
namespace YSharp_2._0;

public class RunTimeResult
{
    public Value value = ValueNull.Instance;
    public Error error = ErrorNull.Instance;
    public Value funcReturnValue = ValueNull.Instance;
    public bool loopContinue = false;
    public bool loopBreak = false;
    public void Reset()
    {
        value = ValueNull.Instance;
        error = ErrorNull.Instance;
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
    public readonly string displayName;
    public readonly Context? parent;
    public readonly Position parentEntryPos;
    public SymbolTable? symbolTable;

    public Context(string displayName, Context? parent, Position parentEntryPos)
    {
        this.displayName = displayName;
        this.parentEntryPos = parentEntryPos;
        this.parent = parent;
    }
    public Context()
    {
        displayName = string.Empty;
        parent = null;
        parentEntryPos = new();
    }
}

public class SymbolTable
{
    public Dictionary<string, Value> symbols = [];
    public SymbolTable? parent = null;

    private Value GetFromParent(string name, Value defaultValue)
    {
        return parent is not null ? parent.Get(name, defaultValue) : defaultValue;
    }

    public Value Get(string? name)
    {
        if (string.IsNullOrEmpty(name))
        {
            return ValueNull.Instance;
        }

        return symbols.TryGetValue(name, out Value? value)
            ? value
            : GetFromParent(name, ValueNull.Instance);
    }

    public Value Get(string? name, Value defaultValue)
    {
        if (string.IsNullOrEmpty(name))
        {
            return defaultValue;
        }

        return symbols.TryGetValue(name, out Value? value)
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

public static class Interpreter
{
    public static RunTimeResult Visit(INode node, Context context)
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
            _ => throw new Exception("No method found for " + node.GetType())
        };
    }

    private static RunTimeResult Visit_Number(NumberNode node, Context context)
    {
        return new RunTimeResult().Success(new VNumber(node.tok.Value).SetContext(context).SetPos(node.StartPos, node.EndPos));
    }
    private static RunTimeResult Visit_String(StringNode node, Context context)
    {
        string? value = (string?)node.tok.Value;
        return new RunTimeResult().Success(new VString(value ?? "").SetContext(context).SetPos(node.StartPos, node.EndPos));
    }
    private static RunTimeResult Visit_List(ListNode node, Context context)
    {
        RunTimeResult res = new();
        List<Value> elements = new(node.elementNodes.Count);
        for (int i = 0; i < node.elementNodes.Count; i++)
        {
            INode elementNode = node.elementNodes[i];
            elements.Add(res.Regrister(Visit(elementNode, context)));
            if (res.ShouldReturn())
            {
                return res;
            }
        }
        return res.Success(new VList(elements).SetContext(context).SetPos(node.StartPos, node.EndPos));
    }
    private static RunTimeResult Visit_BinaryOp(BinOpNode node, Context context)
    {
        RunTimeResult res = new();

        // Run both left and right in parallel
        Task<RunTimeResult> leftTask = Task.Run(() => Visit(node.leftNode, context));
        Task<RunTimeResult> rightTask = Task.Run(() => Visit(node.rightNode, context));
        
        Task.WaitAll(leftTask, rightTask);

        Value left = res.Regrister(leftTask.Result);
        if (res.ShouldReturn())
        {
            return res;
        }
        
        Value right = res.Regrister(rightTask.Result);
        if (res.ShouldReturn())
        {
            return res;
        }

        ValueError result;
        // Arethmetic
        switch (node.opTok.Type)
        {
            case TokenType.PLUS:
                result = left.AddedTo(right);
                break;
            case TokenType.MINUS:
                result = left.SubedTo(right);
                break;
            case TokenType.MUL:
                result = left.MuledTo(right);
                break;
            case TokenType.DIV:
                result = left.DivedTo(right);
                break;
            case TokenType.POW:
                result = left.PowedTo(right);
                break;
            // comparison
            case TokenType.EE:
                result = left.GetComparisonEQ(right);
                break;
            case TokenType.NE:
                result = left.GetComparisonNE(right);
                break;
            case TokenType.LT:
                result = left.GetComparisonLT(right);
                break;
            case TokenType.GT:
                result = left.GetComparisonGT(right);
                break;
            case TokenType.LTE:
                result = left.GetComparisonLTE(right);
                break;
            case TokenType.GTE:
                result = left.GetComparisonGTE(right);
                break;
            default:
                if (node.opTok.Matches(TokenType.KEYWORD, "AND"))
                {
                    result = left.AndedTo(right);
                }
                else if (node.opTok.Matches(TokenType.KEYWORD, "OR"))
                {
                    result = left.OredTo(right);
                }
                else
                {
                    throw new Exception("operator token type is wrong: " + node.opTok.Type);
                }

                break;
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
    private static RunTimeResult Visit_UnaryOp(UnaryOpNode node, Context context)
    {
        RunTimeResult res = new();
        Value value = res.Regrister(Visit(node.node, context));
        if (res.ShouldReturn())
        {
            return res;
        }
        else if (value is null)
        {
            return res.Failure(new InternalError("Cast in Visit_UnaryOp failed"));
        }

        ValueError result;
        if (node.opTok.Type == TokenType.MINUS && value is VNumber)
        {
            result = value.MuledTo(new VNumber(-1));
        }
        else if (node.opTok.Matches(TokenType.KEYWORD, "NOT"))
        {
            result = value.Notted();
        }
        else
        {
            result = (value, ErrorNull.Instance);
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

        if (context.symbolTable is null)
        {
            return res.Failure(new InternalError("There is no SymbolTable in this context"));
        }

        string? varName = (string?)node.varNameTok.Value;
        Value value = context.symbolTable.Get(varName);

        if (value is null or ValueNull)
        {
            if(node.fromCall) return res.Failure(new FuncNotFoundError(node.StartPos, $"{varName} is not defined", context));
            return res.Failure(new VarNotFoundError(node.StartPos, $"{varName} is not defined", context));
        }

        return res.Success(value.Copy().SetPos(node.StartPos, node.EndPos).SetContext(context));
    }
    private static RunTimeResult Visit_VarAssignNode(VarAssignNode node, Context context)
    {
        RunTimeResult res = new();
        string? varName = (string?)node.varNameTok.Value;
        Value value = res.Regrister(Visit(node.valueNode, context));
        if (res.ShouldReturn())
        {
            return res;
        }
        if (varName is null)
        {
            return res.Failure(new InternalError("Var name is null"));
        }

        if (context.symbolTable is null)
        {
            return res.Failure(new InternalError("Symbol Table is null"));
        }

        context.symbolTable.Set(varName, value);
        return res.Success(value);
    }
    private static RunTimeResult Visit_DotVarAccessNode(DotVarAccessNode node, Context context)
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
            return res.Failure(new VarNotFoundError(node.StartPos, $"{varName} var is not defined", context));
        }
        /* if (value is (ValueNull)){
            return res.success(value);
        } */
        return res.Success(value.value.Copy().SetPos(node.StartPos, node.EndPos).SetContext(context));
    }
    private static RunTimeResult Visit_DotCallNode(DotCallNode node, Context context)
    {
        RunTimeResult res = new();


        string? funcName = (string?)node.funcNameTok.Value;

        List<Value> argValue = new(node.argNodes.Count);
        for (int i = 0; i < node.argNodes.Count; i++)
        {
            INode _node = node.argNodes[i];
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
            return res.Success(value.value.Copy().SetPos(node.StartPos, node.EndPos).SetContext(context));
        }
        catch
        { // the function returns an emptie value, which cnat be copied
            return res.Success(value.value);
        }
    }
    private static RunTimeResult Visit_IfNode(IfNode node, Context context)
    {
        RunTimeResult res = new();

        for (int i = 0; i < node.cases.Count; i++)
        {
            INode condition = node.cases[i].condition;
            INode expr = node.cases[i].expresion;
            bool retNull = node.cases[i].returnNull;

            Value conditionValue = res.Regrister(Visit(condition, context));
            if (res.ShouldReturn())
            {
                return res;
            }

            if (conditionValue.IsTrue())
            {
                Value exprValue = res.Regrister(Visit(expr, context));
                if (res.ShouldReturn())
                {
                    return res;
                }
                return res.Success(retNull ? ValueNull.Instance : exprValue);
            }
        }

        if (node.elseCase.Node is not null && node.elseCase.Bool is not null)
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
    private static RunTimeResult Visit_ForNode(ForNode node, Context context)
    {
        RunTimeResult res = new();
        // calculate in parallel
        Task<RunTimeResult> startValueTask = Task.Run(() => Visit(node.startValueNode, context));
        Task<RunTimeResult> endValueTask = Task.Run(() => Visit(node.endValueNode, context));

        // wait till both are executed
        Task.WhenAll(startValueTask, endValueTask);

        Value startValue = res.Regrister(startValueTask.Result);
        if (res.ShouldReturn())
        {
            return res;
        }

        Value endValue = res.Regrister(endValueTask.Result);
        if (res.ShouldReturn())
        {
            return res;
        }

        Value stepValue;
        if (node.stepValueNode is not null)
        {
            stepValue = res.Regrister(Visit(node.stepValueNode, context));
            if (res.ShouldReturn())
            {
                return res;
            }
        }
        else
        {
            stepValue = new VNumber(1);
        }

        if (startValue is null || endValue is null || stepValue is null)
        {
            return res.Failure(new InternalError($"startValue is null {startValue is null} endValue is null {endValue is null} stepValue is null {stepValue is null}"));
        }
        
        double StartNumber;
        double EndNumber;
        double StepNumber;

        if(startValue is VNumber numStart){
            StartNumber = numStart.value;
        } else {
            return res.Failure(new WrongFormatError(startValue.startPos, "The Start Number is not a Number", context));
        }
        if(endValue is VNumber numEnd){
            EndNumber = numEnd.value;
        } else {
            return res.Failure(new WrongFormatError(startValue.startPos, "The End Number is not a Number", context));
        }
        if(stepValue is VNumber numStep){
            StepNumber = numStep.value;
        } else {
            return res.Failure(new WrongFormatError(startValue.startPos, "The Step Number is not a Number", context));
        }

        double i = StartNumber;
        Func<double, bool> condition;
        if (StepNumber >= 0)
        {
            condition = i => i < EndNumber;
        }
        else
        {
            condition = i => i > EndNumber;
        }

        if (context.symbolTable is null)
        {
            return res.Failure(new InternalError("No symbol Table"));
        }
        string? varName = (string?)node.varNameTok.Value;
        if (varName is null)
        {
            return res.Failure(new InternalError($"No value for varName -> {varName}"));
        }
        while (condition(i))
        {
            context.symbolTable.Set(varName, new VNumber(i));
            i += StepNumber;

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
    private static RunTimeResult Visit_WhileNode(WhileNode node, Context context)
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
            if (!condition.IsTrue())
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
        if (node.varNameTok is not null)
        {
            funcName = (string?)node.varNameTok.Value;
        }
        INode bodyNode = node.bodyNode;

        List<string> argNames = new(node.argNameToks.Count);
        for (int i = 0; i < node.argNameToks.Count; i++)
        {
            Token<string> tok = (Token<string>)node.argNameToks[i];
            argNames.Add(tok.Value);
        }

        VFunction funcValue = new(funcName, bodyNode, argNames, node.retNull);
        funcValue.SetContext(context).SetPos(node.StartPos, node.EndPos);

        if (node.varNameTok is not null && context.symbolTable is not null && funcName is not null)
        {
            context.symbolTable.Set(funcName, funcValue);
        }
        return res.Success(funcValue);
    }
    private static RunTimeResult Visit_CallNode(CallNode node, Context context)
    {
        RunTimeResult res = new();
        List<Value> args = new(node.argNodes.Count);

        if(node.nodeToCall is VarAccessNode varAccessNode){ // just for error handeling
            varAccessNode.fromCall = true;
        }

        Value valueToCall = res.Regrister(Visit(node.nodeToCall, context));
        if (valueToCall is null)
        {
            return res.Failure(new InternalError("cast failed for valueToCall "));
        }
        if (res.ShouldReturn())
        {
            return res;
        }
        if (valueToCall is VBuiltInFunction BIfunction)
        {
            valueToCall = BIfunction.Copy();
        }
        else if (valueToCall is VFunction function)
        {
            valueToCall = function.Copy();
        }
        else
        {
            Console.WriteLine("Type of valueToCall not supported :" + valueToCall.GetType());
        }


        valueToCall.SetPos(node.StartPos, node.EndPos);

        for (int i = 0; i < node.argNodes.Count; i++)
        {
            INode arg = node.argNodes[i];
            args.Add(res.Regrister(Visit(arg, context)));
            if (res.ShouldReturn())
            {
                return res;
            }
        }
        Value ret;
        if (valueToCall is VBuiltInFunction _BIfunction)
        {
            ret = res.Regrister(_BIfunction.execute(args));
        }
        else if (valueToCall is VFunction _function)
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
            ret = ret.Copy().SetPos(node.StartPos, node.EndPos).SetContext(context);
            return res.Success(ret);
        }
        catch
        { // the function returns an emptie value, which cnat be copied
            return res.Success(ValueNull.Instance);
        }
    }
    private static RunTimeResult Visit_ReturnNode(ReturnNode node, Context context)
    {
        RunTimeResult res = new();
        Value value;
        if (node.nodeToReturn is not null)
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
    private static RunTimeResult Visit_ContinueNode()
    {
        return new RunTimeResult().SuccessContinue();
    }
    private static RunTimeResult Visit_BreakNode()
    {
        return new RunTimeResult().SuccessBreak();
    }
    private static RunTimeResult Visit_TryCatchNode(TryCatchNode node, Context context)
    {
        RunTimeResult res = new();

        Value val = res.Regrister(Visit(node.TryNode, context));
        if (!res.error.IsError)
        {
            return res.Success(val);
        }

        if(context.symbolTable is null){
            return res.Failure(new InternalError("The symboltable is null"));
        }

        if(node.ChatchVarName is not null){
            if (node.ChatchVarName.Value is not string varName)
            {
                return res.Failure(new InternalError("The var Name is null"));
            }
            context.symbolTable.Set(varName, new VString(res.error.ToString()));
        }

        Value catchVal = res.Regrister(Visit(node.CatchNode, context));
        if (res.ShouldReturn())
        {
            return res;
        }
        return res.Success(catchVal);
    }
}

