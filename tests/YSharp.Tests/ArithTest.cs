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
    [MemberData(nameof(TestCases))]
    public void Test_Add(CliArgs arg, double x, double y)
    {
        Check_Arith(_runClass.Run("TEST", $"{x:f}+{y:f}", arg), x + y);
    }

    [Theory]
    [MemberData(nameof(TestCases))]
    public void Test_Add_Pretty(CliArgs arg, double x, double y)
    {
        Check_Arith(_runClass.Run("TEST", $"{x:f} + {y:f}", arg), x + y);
    }

    [Theory]
    [MemberData(nameof(TestCases))]
    public void Test_Div(CliArgs arg, double x, double y)
    {
        Check_Arith(_runClass.Run("TEST", $"{x:f}/{y:f}", arg), x / y);
    }

    [Theory]
    [MemberData(nameof(TestCases))]
    public void Test_Div_Pretty(CliArgs arg, double x, double y)
    {
        Check_Arith(_runClass.Run("TEST", $"{x:f} / {y:f}", arg), x / y);
    }

    [Theory]
    [MemberData(nameof(TestCases))]
    public void Test_Divide_By_Zero(CliArgs arg, double x, double y)
    {
        RunResult res = _runClass.Run("TEST", "1 / 0", arg);
        Assert.True(res.IsFailed);
        if(res.IsFailed)
            Assert.IsType<DivisionByZeroError>(res.GetError());
    }

    [Theory]
    [MemberData(nameof(TestCases))]
    public void Test_Mult(CliArgs arg, double x, double y)
    {
        Check_Arith(_runClass.Run("TEST", $"{x:f}*{y:f}", arg), x * y);
    }

    [Theory]    
    [MemberData(nameof(TestCases))]
    public void Test_Mult_Pretty(CliArgs arg, double x, double y)
    {
        Check_Arith(_runClass.Run("TEST", $"{x:f} * {y:f}", arg), x * y);
    }

    [Theory]
    [MemberData(nameof(TestCases))]
    public void Test_Sub(CliArgs arg, double x, double y)
    {
        RunResult res = _runClass.Run("TEST", $"{x:f}-{y:f}", arg);
        if (y < 0)
            Assert.IsType<InvalidSyntaxError>(res.GetError());
        else
            Check_Arith(res, x - y);
    }

    [Theory]
    [MemberData(nameof(TestCases))]
    public void Test_Sub_Pretty(CliArgs arg, double x, double y)
    {
        Check_Arith(_runClass.Run("TEST", $"{x:f} - {y:f}", arg), x - y);
    }

    private void Check_Arith(RunResult res, double expected)
    {
        Assert.True(res.IsSuccess);
        Assert.IsType<VNumber>(((VList)res.GetValue()).value[0]);
    }
    
    public static IEnumerable<object[]> TestCases()
    {
        (double, double)[] values =
        [
            (0.0, 2.0),
            (100.0, 2.0),
            (0.5, 20.0),
            (0.5, -20.0),
            (0.5, -0.354)
        ];

        foreach (CliArgs mode in new[] { CliArgs.ArgsNoOptimization, CliArgs.ArgsWithOptimization })
        {
            foreach ((double x, double y) in values)
            {
                yield return [mode, x, y];
            }
        }
    }
}
