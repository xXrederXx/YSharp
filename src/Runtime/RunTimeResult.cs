using YSharp.Common;

namespace YSharp.Runtime;

public class RunTimeResult
{
    public Error Error { get; private set; } = ErrorNull.Instance;
    public Value FuncReturnValue { get; private set; } = ValueNull.Instance;
    public bool LoopBreak { get; private set; }
    public bool LoopContinue { get; private set; }
    public Value Value { get; private set; } = ValueNull.Instance;

    public RunTimeResult Failure(Error error)
    {
        Reset();
        this.Error = error;
        return this;
    }

    public Value Register(RunTimeResult res)
    {
        Error = res.Error;
        FuncReturnValue = res.FuncReturnValue;
        LoopContinue = res.LoopContinue;
        LoopBreak = res.LoopBreak;
        return res.Value;
    }

    public void Reset()
    {
        Value = ValueNull.Instance;
        Error = ErrorNull.Instance;
        FuncReturnValue = ValueNull.Instance;
        LoopContinue = false;
        LoopBreak = false;
    }

    public bool ShouldReturn() =>
        Error.IsError || FuncReturnValue is not ValueNull || LoopContinue || LoopBreak;

    public RunTimeResult Success(Value value)
    {
        Reset();
        this.Value = value;
        return this;
    }

    public RunTimeResult SuccessBreak()
    {
        Reset();
        LoopBreak = true;
        return this;
    }

    public RunTimeResult SuccessContinue()
    {
        Reset();
        LoopContinue = true;
        return this;
    }

    public RunTimeResult SuccessReturn(Value value)
    {
        Reset();
        FuncReturnValue = value;
        return this;
    }
}
