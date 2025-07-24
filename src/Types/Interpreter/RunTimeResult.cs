using YSharp.Types.Common;

namespace YSharp.Types.Interpreter;

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

    public bool Regrister(RunTimeResult res, out Value val)
    {
        val = Regrister(res);
        return ShouldReturn();
    }

    public RunTimeResult Success(Value value)
    {
        if (error.IsError)
            Console.WriteLine("error deleted:\n" + error.ToString());
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

