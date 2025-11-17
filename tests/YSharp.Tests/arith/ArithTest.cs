using Xunit;
using YSharp.Types.Common;
using YSharp.Types.Interpreter;
using YSharp.Types.Interpreter.Collection;
using YSharp.Types.Interpreter.Primitives;
using YSharp.Utils;

namespace YSharp.Tests.Arith;


public class ErrorTest
{
    private readonly RunClass _runClass = new();

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
    public void Test_Sub_Pretty(double x, double y)
    {
        Check_Arith(_runClass.Run("TEST", $"{x:f} - {y:f}"), x - y);
    }

    [Theory]
    [InlineData(0, 2)]
    [InlineData(100, 2)]
    [InlineData(0.5, 20)]
    [InlineData(0.5, -20)]
    [InlineData(0.5, -0.354)]
    public void Test_Sub(double x, double y)
    {
        (Value val, Error err) res = _runClass.Run("TEST", $"{x:f}-{y:f}");
        if (y < 0)
            Assert.IsType<InvalidSyntaxError>(res.err);
        else
            Check_Arith(res, x - y);
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
    public void Test_Div_Pretty(double x, double y)
    {
        Check_Arith(_runClass.Run("TEST", $"{x:f} / {y:f}"), x / y);
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

    [Fact]
    public void Test_Divide_By_Zero()
    {
        (Value _, Error err) = _runClass.Run("TEST", "1 / 0");
        Assert.IsType<DivisionByZeroError>(err);
    }

    private void Check_Arith((Value, Error) res, double expected)
    {
        Assert.NotNull(res.Item2);
        Assert.IsType<ErrorNull>(res.Item2);
        Assert.IsType<VNumber>(((VList)res.Item1).value[0]);
        if (res.Item1 is VNumber num)
        {
            Assert.Equal(expected, num.value);
        }
    }
}