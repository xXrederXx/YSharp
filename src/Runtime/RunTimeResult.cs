using YSharp.Common;

namespace YSharp.Runtime;

public class RunTimeResult
{
    public Error error { get; private set; } = ErrorNull.Instance;
    public Value funcReturnValue { get; private set; } = ValueNull.Instance;
    public bool loopBreak { get; private set; }
    public bool loopContinue { get; private set; }
    public Value value { get; private set; } = ValueNull.Instance;

    public RunTimeResult Failure(Error error)
    {
        Reset();
        this.error = error;
        return this;
    }

    public Value Register(RunTimeResult res)
    {
        error = res.error;
        funcReturnValue = res.funcReturnValue;
        loopContinue = res.loopContinue;
        loopBreak = res.loopBreak;
        return res.value;
    }

    public void Reset()
    {
        value = ValueNull.Instance;
        error = ErrorNull.Instance;
        funcReturnValue = ValueNull.Instance;
        loopContinue = false;
        loopBreak = false;
    }

    public bool ShouldReturn() =>
        error.IsError || funcReturnValue is not ValueNull || loopContinue || loopBreak;

    public RunTimeResult Success(Value value)
    {
        Reset();
        this.value = value;
        return this;
    }

    public RunTimeResult SuccessBreak()
    {
        Reset();
        loopBreak = true;
        return this;
    }

    public RunTimeResult SuccessContinue()
    {
        Reset();
        loopContinue = true;
        return this;
    }

    public RunTimeResult SuccessReturn(Value value)
    {
        Reset();
        funcReturnValue = value;
        return this;
    }
}
