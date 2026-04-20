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

        Assert.Equal(value, result.value);
        Assert.Equal(ValueNull.Instance, result.funcReturnValue);
        Assert.Equal(ErrorNull.Instance, result.error);
        Assert.False(result.loopBreak);
        Assert.False(result.loopContinue);
    }

    [Fact]
    public void checkRunTimeResult_whenFailure_thenSetsErrorAndResetsFields()
    {
        RunTimeResult result = new RunTimeResult();
        ExpectedTokenError error = new ExpectedTokenError(Position.Null, "tok");

        result.Failure(error);

        Assert.Equal(error, result.error);
        Assert.Equal(ValueNull.Instance, result.value);
        Assert.Equal(ValueNull.Instance, result.funcReturnValue);
        Assert.False(result.loopBreak);
        Assert.False(result.loopContinue);
    }

    [Fact]
    public void SuccessReturn_ShouldSetFuncReturnValue()
    {
        RunTimeResult result = new RunTimeResult();
        Value value = new Value();

        result.SuccessReturn(value);

        Assert.Equal(value, result.funcReturnValue);
        Assert.True(result.ShouldReturn());
    }

    [Fact]
    public void checkRunTimeResult_whenSuccessBreak_thenSetsLoopBreak()
    {
        RunTimeResult result = new RunTimeResult();

        result.SuccessBreak();

        Assert.True(result.loopBreak);
        Assert.True(result.ShouldReturn());
    }

    [Fact]
    public void checkRunTimeResult_whenSuccessContinue_thenSetsLoopContinue()
    {
        RunTimeResult result = new RunTimeResult();

        result.SuccessContinue();

        Assert.True(result.loopContinue);
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
        Assert.Equal(error, target.error);

        source.SuccessReturn(value);
        target.Register(source);
        Assert.Equal(value, target.funcReturnValue);

        source.SuccessBreak();
        target.Register(source);
        Assert.True(target.loopBreak);

        source.SuccessContinue();
        target.Register(source);
        Assert.True(target.loopContinue);
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

        Assert.Equal(ValueNull.Instance, result.value);
        Assert.Equal(ErrorNull.Instance, result.error);
        Assert.Equal(ValueNull.Instance, result.funcReturnValue);
        Assert.False(result.loopBreak);
        Assert.False(result.loopContinue);
    }

    [Fact]
    public void checkRunTimeResult_whenMultipleOperations_thenNoStateLeakage()
    {
        RunTimeResult result = new RunTimeResult();

        result.Success(new Value());
        result.SuccessBreak();

        Assert.True(result.loopBreak);
        Assert.Equal(ValueNull.Instance, result.value); // reset happened
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

        Assert.Equal(source.error, result.error);
    }
}
