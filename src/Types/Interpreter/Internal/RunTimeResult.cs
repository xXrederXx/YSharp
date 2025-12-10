using YSharp.Types.Common;

namespace YSharp.Types.Interpreter.Internal;

public class RunTimeResult{
    public Error error = ErrorNull.Instance;
    public Value funcReturnValue = ValueNull.Instance;
    public bool loopBreak;
    public bool loopContinue;
    public Value value = ValueNull.Instance;

    public RunTimeResult Failure(Error error)
    {
        Reset();
        this.error = error;
        return this;
    }

    public Value Regrister(RunTimeResult res)
    {
        error = res.error;
        funcReturnValue = res.funcReturnValue;
        loopContinue = res.loopContinue;
        loopBreak = res.loopBreak;
        return res.value;
    }

    public bool Regrister(RunTimeResult res, out Value val)
    {
        val = Regrister(res);
        return ShouldReturn();
    }

    public void Reset()
    {
        value = ValueNull.Instance;
        error = ErrorNull.Instance;
        funcReturnValue = ValueNull.Instance;
        loopContinue = false;
        loopBreak = false;
    }

    public bool ShouldReturn() => error.IsError || funcReturnValue is not ValueNull || loopContinue || loopBreak;

    public RunTimeResult Success(Value value)
    {
        if (error.IsError)
            Console.WriteLine("error deleted:\n" + error);
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