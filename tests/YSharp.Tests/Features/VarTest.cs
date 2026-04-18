using Xunit;
using YSharp.Common;
using YSharp.Runtime;
using YSharp.Runtime.Collections.List;
using YSharp.Runtime.Primitives.Number;
using YSharp.Runtime.Primitives.String;
using YSharp.Util;

namespace YSharp.Tests;

using RunResult = Result<Value, Error>;

public class VarTest
{
    private readonly RuntimeEnviroment _runClass = new();

    [Fact]
    public void checkAssignVariable_whenInt()
    {
        RunResult result = _runClass.Run("<test>", $"VAR x = 5; x", CliArgs.DefaultArgs);

        Assert.True(result.TryGetValue(out Value value));
        VList list = Assert.IsType<VList>(value);
        VNumber number = Assert.IsType<VNumber>(list.value[0]);
        Assert.Equal(5, number.value, TestingConstans.DOUBLE_PRECISION);
    }

    [Fact]
    public void checkAssignVariable_whenDouble()
    {
        RunResult result = _runClass.Run("<test>", $"VAR x = 6.7; x", CliArgs.DefaultArgs);

        Assert.True(result.TryGetValue(out Value value));
        VList list = Assert.IsType<VList>(value);
        VNumber number = Assert.IsType<VNumber>(list.value[0]);
        Assert.Equal(6.7, number.value, TestingConstans.DOUBLE_PRECISION);
    }

    [Fact]
    public void checkAssignVariable_whenString()
    {
        RunResult result = _runClass.Run("<test>", $"VAR x = \"Hi\"; x", CliArgs.DefaultArgs);

        Assert.True(result.TryGetValue(out Value value));
        VList list = Assert.IsType<VList>(value);
        VString stringValue = Assert.IsType<VString>(list.value[0]);
        Assert.Equal("Hi", stringValue.value);
    }

    [Fact]
    public void checkAssignVariable_whenList()
    {
        RunResult result = _runClass.Run("<test>", $"VAR x = [1, 2]; x", CliArgs.DefaultArgs);

        Assert.True(result.TryGetValue(out Value value));
        VList list = Assert.IsType<VList>(value);
        VList valueList = Assert.IsType<VList>(list.value[0]);

        VNumber number1 = Assert.IsType<VNumber>(valueList.value[0]);
        Assert.Equal(1, number1.value, TestingConstans.DOUBLE_PRECISION);

        VNumber number2 = Assert.IsType<VNumber>(valueList.value[1]);
        Assert.Equal(2, number2.value, TestingConstans.DOUBLE_PRECISION);
    }

    [Fact]
    void checkIncrement()
    {
        RunResult result = _runClass.Run("<test>", $"VAR x = 1; VAR x++; x", CliArgs.DefaultArgs);

        Assert.True(result.TryGetValue(out Value value));
        VList list = Assert.IsType<VList>(value);
        VNumber number = Assert.IsType<VNumber>(list.value.Last());
        Assert.Equal(2, number.value, TestingConstans.DOUBLE_PRECISION);
    }

    [Fact]
    void checkDecrement()
    {
        RunResult result = _runClass.Run("<test>", $"VAR x = 1; VAR x--; x", CliArgs.DefaultArgs);

        Assert.True(result.TryGetValue(out Value value));
        VList list = Assert.IsType<VList>(value);
        VNumber number = Assert.IsType<VNumber>(list.value.Last());
        Assert.Equal(0, number.value, TestingConstans.DOUBLE_PRECISION);
    }

    [Fact]
    void checkIncrement_whenString()
    {
        RunResult result = _runClass.Run(
            "<test>",
            $"VAR x = \"tst\"; VAR x++; x",
            CliArgs.DefaultArgs
        );

        Assert.True(result.IsFailed);
        Assert.IsType<WrongTypeError>(result.GetError());
    }

    [Fact]
    void checkDecrement_whenString()
    {
        RunResult result = _runClass.Run(
            "<test>",
            $"VAR x = \"tst\"; VAR x--; x",
            CliArgs.DefaultArgs
        );

        Assert.True(result.IsFailed);
        Assert.IsType<WrongTypeError>(result.GetError());
    }
}
