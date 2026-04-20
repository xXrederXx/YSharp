using Xunit;
using YSharp.Common;
using YSharp.Runtime;

namespace YSharp.Tests;

public class RunTimeResultTests
{
    [Fact]
    public void checkRunTimeResult_whenSuccess_thenSetsValueAndResetsFields()
    {
        RunTimeResult result = new RunTimeResult();
        Value value = new Value();

        result.Success(value);

        Assert.Equal(value, result.Value);
        Assert.Equal(ValueNull.Instance, result.FuncReturnValue);
        Assert.Equal(ErrorNull.Instance, result.Error);
        Assert.False(result.LoopBreak);
        Assert.False(result.LoopContinue);
    }

    [Fact]
    public void checkRunTimeResult_whenFailure_thenSetsErrorAndResetsFields()
    {
        RunTimeResult result = new RunTimeResult();
        ExpectedTokenError error = new ExpectedTokenError(Position.Null, "tok");

        result.Failure(error);

        Assert.Equal(error, result.Error);
        Assert.Equal(ValueNull.Instance, result.Value);
        Assert.Equal(ValueNull.Instance, result.FuncReturnValue);
        Assert.False(result.LoopBreak);
        Assert.False(result.LoopContinue);
    }

    [Fact]
    public void SuccessReturn_ShouldSetFuncReturnValue()
    {
        RunTimeResult result = new RunTimeResult();
        Value value = new Value();

        result.SuccessReturn(value);

        Assert.Equal(value, result.FuncReturnValue);
        Assert.True(result.ShouldReturn());
    }

    [Fact]
    public void checkRunTimeResult_whenSuccessBreak_thenSetsLoopBreak()
    {
        RunTimeResult result = new RunTimeResult();

        result.SuccessBreak();

        Assert.True(result.LoopBreak);
        Assert.True(result.ShouldReturn());
    }

    [Fact]
    public void checkRunTimeResult_whenSuccessContinue_thenSetsLoopContinue()
    {
        RunTimeResult result = new RunTimeResult();

        result.SuccessContinue();

        Assert.True(result.LoopContinue);
        Assert.True(result.ShouldReturn());
    }

    [Fact]
    public void checkRunTimeResult_whenRegister_thenCopiesStateFromOther()
    {
        RunTimeResult source = new RunTimeResult();
        RunTimeResult target = new RunTimeResult();

        Value value = new Value();
        ExpectedTokenError error = new ExpectedTokenError(Position.Null, "tok");

        source.Success(value);
        Value returnedValue = target.Register(source);
        Assert.Equal(value, returnedValue);

        source.Failure(error);
        target.Register(source);
        Assert.Equal(error, target.Error);

        source.SuccessReturn(value);
        target.Register(source);
        Assert.Equal(value, target.FuncReturnValue);

        source.SuccessBreak();
        target.Register(source);
        Assert.True(target.LoopBreak);

        source.SuccessContinue();
        target.Register(source);
        Assert.True(target.LoopContinue);
    }

    [Fact]
    public void checkRunTimeResult_whenShouldReturnWithNoFlagsOrError_thenFalse()
    {
        RunTimeResult result = new RunTimeResult();

        Assert.False(result.ShouldReturn());
    }

    [Fact]
    public void checkRunTimeResult_whenShouldReturnWithError_thenTrue()
    {
        RunTimeResult result = new RunTimeResult().Failure(new ExpectedTokenError(Position.Null, "tok"));

        Assert.True(result.ShouldReturn());
    }

    [Fact]
    public void Reset_ShouldClearAllState()
    {
        RunTimeResult result = new RunTimeResult().Success(new Value());

        result.SuccessReturn(new Value());
        result.SuccessBreak();
        result.SuccessContinue();

        result.Reset();

        Assert.Equal(ValueNull.Instance, result.Value);
        Assert.Equal(ErrorNull.Instance, result.Error);
        Assert.Equal(ValueNull.Instance, result.FuncReturnValue);
        Assert.False(result.LoopBreak);
        Assert.False(result.LoopContinue);
    }

    [Fact]
    public void checkRunTimeResult_whenMultipleOperations_thenNoStateLeakage()
    {
        RunTimeResult result = new RunTimeResult();

        result.Success(new Value());
        result.SuccessBreak();

        Assert.True(result.LoopBreak);
        Assert.Equal(ValueNull.Instance, result.Value); // reset happened
    }

    [Fact]
    public void checkRunTimeResult_whenRegister_thenOverwritesExistingState()
    {
        RunTimeResult result = new RunTimeResult();
        RunTimeResult source = new RunTimeResult().Failure(
            new ExpectedTokenError(Position.Null, "tok")
        );

        result.Success(new Value());
        result.Register(source);

        Assert.Equal(source.Error, result.Error);
    }
}
