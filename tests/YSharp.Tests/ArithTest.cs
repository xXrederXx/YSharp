using Xunit;
using YSharp.Common;
using YSharp.Runtime;
using YSharp.Runtime.Collections.List;
using YSharp.Runtime.Primatives.Number;
using YSharp.Util;

namespace YSharp.Tests;

using RunResult = Result<Value, Common.Error>;

public class ErrorTest
{
    private readonly RunClass _runClass = new();

    [Theory]
    [InlineData(0, 2)]
    [InlineData(100, 2)]
    [InlineData(0.5, 20)]
    [InlineData(0.5, -20)]
    [InlineData(0.5, -0.354)]
    public void Test_Add(double x, double y)
    {
        Check_Arith(_runClass.Run("TEST", $"{x:f}+{y:f}"), x + y);
    }

    [Theory]
    [InlineData(0, 2)]
    [InlineData(100, 2)]
    [InlineData(0.5, 20)]
    [InlineData(0.5, -20)]
    [InlineData(0.5, -0.354)]
    public void Test_Add_Pretty(double x, double y)
    {
        Check_Arith(_runClass.Run("TEST", $"{x:f} + {y:f}"), x + y);
    }

    [Theory]
    [InlineData(0, 2)]
    [InlineData(100, 2)]
    [InlineData(0.5, 20)]
    [InlineData(0.5, -20)]
    [InlineData(0.5, -0.354)]
    public void Test_Div(double x, double y)
    {
        Check_Arith(_runClass.Run("TEST", $"{x:f}/{y:f}"), x / y);
    }

    [Theory]
    [InlineData(0, 2)]
    [InlineData(100, 2)]
    [InlineData(0.5, 20)]
    [InlineData(0.5, -20)]
    [InlineData(0.5, -0.354)]
    public void Test_Div_Pretty(double x, double y)
    {
        Check_Arith(_runClass.Run("TEST", $"{x:f} / {y:f}"), x / y);
    }

    [Fact]
    public void Test_Divide_By_Zero()
    {
        RunResult res = _runClass.Run("TEST", "1 / 0");
        Assert.True(res.IsFailed);
        if(res.IsFailed)
            Assert.IsType<DivisionByZeroError>(res.GetError());
    }

    [Theory]
    [InlineData(0, 2)]
    [InlineData(100, 2)]
    [InlineData(0.5, 20)]
    [InlineData(0.5, -20)]
    [InlineData(0.5, -0.354)]
    public void Test_Mult(double x, double y)
    {
        Check_Arith(_runClass.Run("TEST", $"{x:f}*{y:f}"), x * y);
    }

    [Theory]
    [InlineData(0, 2)]
    [InlineData(100, 2)]
    [InlineData(0.5, 20)]
    [InlineData(0.5, -20)]
    [InlineData(0.5, -0.354)]
    public void Test_Mult_Pretty(double x, double y)
    {
        Check_Arith(_runClass.Run("TEST", $"{x:f} * {y:f}"), x * y);
    }

    [Theory]
    [InlineData(0, 2)]
    [InlineData(100, 2)]
    [InlineData(0.5, 20)]
    [InlineData(0.5, -20)]
    [InlineData(0.5, -0.354)]
    public void Test_Sub(double x, double y)
    {
        RunResult res = _runClass.Run("TEST", $"{x:f}-{y:f}");
        if (y < 0)
            Assert.IsType<InvalidSyntaxError>(res.GetError());
        else
            Check_Arith(res, x - y);
    }

    [Theory]
    [InlineData(0, 2)]
    [InlineData(100, 2)]
    [InlineData(0.5, 20)]
    [InlineData(0.5, -20)]
    [InlineData(0.5, -0.354)]
    public void Test_Sub_Pretty(double x, double y)
    {
        Check_Arith(_runClass.Run("TEST", $"{x:f} - {y:f}"), x - y);
    }

    private void Check_Arith(RunResult res, double expected)
    {
        Assert.True(res.IsSuccess);
        Assert.IsType<VNumber>(((VList)res.GetValue()).value[0]);
    }
}
