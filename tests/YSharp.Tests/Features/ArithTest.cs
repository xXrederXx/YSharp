using Xunit;
using YSharp.Common;
using YSharp.Runtime;
using YSharp.Runtime.Collections.List;
using YSharp.Runtime.Primitives.Number;

namespace YSharp.Tests;

using RunResult = Result<Value, Error>;

public class ErrorTest
{
    private readonly RuntimeEnviroment _runClass = new();

    [Theory]
    [MemberData(nameof(TestCases))]
    public void checkAdd_whenNoSpace(CliArgs arg, double x, double y)
    {
        RunResult result = _runClass.Run(
            "TEST",
            $"{x.ToString(StaticConfig.numberCulture)}+{y.ToString(StaticConfig.numberCulture)}",
            arg
        );

        Assert.True(result.TryGetValue(out Value value));
        VList list = Assert.IsType<VList>(value);
        VNumber number = Assert.IsType<VNumber>(list.value[0]);
        Assert.Equal(x + y, number.value, TestingConstans.DOUBLE_PRECISION);
    }

    [Theory]
    [MemberData(nameof(TestCases))]
    public void checkAdd_whenPretty(CliArgs arg, double x, double y)
    {
        RunResult result = _runClass.Run(
            "TEST",
            $"{x.ToString(StaticConfig.numberCulture)} + {y.ToString(StaticConfig.numberCulture)}",
            arg
        );

        Assert.True(result.TryGetValue(out Value value));
        VList list = Assert.IsType<VList>(value);
        VNumber number = Assert.IsType<VNumber>(list.value[0]);
        Assert.Equal(x + y, number.value, TestingConstans.DOUBLE_PRECISION);
    }

    [Theory]
    [MemberData(nameof(TestCases))]
    public void checkDiv_whenNoSpace(CliArgs arg, double x, double y)
    {
        RunResult result = _runClass.Run(
            "TEST",
            $"{x.ToString(StaticConfig.numberCulture)}/{y.ToString(StaticConfig.numberCulture)}",
            arg
        );

        Assert.True(result.TryGetValue(out Value value));
        VList list = Assert.IsType<VList>(value);
        VNumber number = Assert.IsType<VNumber>(list.value[0]);
        Assert.Equal(x / y, number.value, TestingConstans.DOUBLE_PRECISION);
    }

    [Theory]
    [MemberData(nameof(TestCases))]
    public void checkDiv_whenPretty(CliArgs arg, double x, double y)
    {
        RunResult result = _runClass.Run(
            "TEST",
            $"{x.ToString(StaticConfig.numberCulture)} / {y.ToString(StaticConfig.numberCulture)}",
            arg
        );

        Assert.True(result.TryGetValue(out Value value));
        VList list = Assert.IsType<VList>(value);
        VNumber number = Assert.IsType<VNumber>(list.value[0]);
        Assert.Equal(x / y, number.value, TestingConstans.DOUBLE_PRECISION);
    }

    [Theory]
    [MemberData(nameof(TestCases))]
#pragma warning disable xUnit1026 // Theory methods should use all of their parameters
    public void checkDiv_whenDivBy0_returnError(CliArgs arg, double x, double y) // TEST BREAKES WHEN x / y ARE REMOVED
#pragma warning restore xUnit1026 // Theory methods should use all of their parameters
    {
        RunResult res = _runClass.Run("TEST", "1 / 0", arg);

        Assert.True(res.IsFailed);
        Assert.IsType<DivisionByZeroError>(res.GetError());
    }

    [Theory]
    [MemberData(nameof(TestCases))]
    public void checkMult_whenNoSpace(CliArgs arg, double x, double y)
    {
        RunResult result = _runClass.Run(
            "TEST",
            $"{x.ToString(StaticConfig.numberCulture)}*{y.ToString(StaticConfig.numberCulture)}",
            arg
        );

        Assert.True(result.TryGetValue(out Value value));
        VList list = Assert.IsType<VList>(value);
        VNumber number = Assert.IsType<VNumber>(list.value[0]);
        Assert.Equal(x * y, number.value, TestingConstans.DOUBLE_PRECISION);
    }

    [Theory]
    [MemberData(nameof(TestCases))]
    public void checkMult_whenPretty(CliArgs arg, double x, double y)
    {
        RunResult result = _runClass.Run(
            "TEST",
            $"{x.ToString(StaticConfig.numberCulture)} * {y.ToString(StaticConfig.numberCulture)}",
            arg
        );

        Assert.True(result.TryGetValue(out Value value));
        VList list = Assert.IsType<VList>(value);
        VNumber number = Assert.IsType<VNumber>(list.value[0]);
        Assert.Equal(x * y, number.value, TestingConstans.DOUBLE_PRECISION);
    }

    [Theory]
    [MemberData(nameof(TestCases))]
    public void checkSub_whenNoSpace(CliArgs arg, double x, double y)
    {
        RunResult result = _runClass.Run(
            "TEST",
            $"{x.ToString(StaticConfig.numberCulture)}-{y.ToString(StaticConfig.numberCulture)}",
            arg
        );

        if (y < 0)
        {
            Assert.True(result.IsFailed);
            Assert.IsType<InvalidSyntaxError>(result.GetError());
        }
        else
        {
            Assert.True(result.TryGetValue(out Value value));
            VList list = Assert.IsType<VList>(value);
            VNumber number = Assert.IsType<VNumber>(list.value[0]);
            Assert.Equal(x - y, number.value, TestingConstans.DOUBLE_PRECISION);
        }
    }

    [Theory]
    [MemberData(nameof(TestCases))]
    public void checkSub_whenPretty(CliArgs arg, double x, double y)
    {
        RunResult result = _runClass.Run(
            "TEST",
            $"{x.ToString(StaticConfig.numberCulture)} - {y.ToString(StaticConfig.numberCulture)}",
            arg
        );

        Assert.True(result.TryGetValue(out Value value));
        VList list = Assert.IsType<VList>(value);
        VNumber number = Assert.IsType<VNumber>(list.value[0]);
        Assert.Equal(x - y, number.value, TestingConstans.DOUBLE_PRECISION);
    }

    [Theory]
    [MemberData(nameof(TestCases))]
    public void checkOrderOfOperation_whenAddAndMult(CliArgs arg, double x, double y)
    {
        RunResult result = _runClass.Run(
            "TEST",
            $"{x.ToString(StaticConfig.numberCulture)} + {y.ToString(StaticConfig.numberCulture)} * {x.ToString(StaticConfig.numberCulture)}",
            arg
        );

        Assert.True(result.TryGetValue(out Value value));
        VList list = Assert.IsType<VList>(value);
        VNumber number = Assert.IsType<VNumber>(list.value[0]);
        Assert.Equal(x + y * x, number.value, TestingConstans.DOUBLE_PRECISION);
    }

    [Theory]
    [MemberData(nameof(TestCases))]
    public void checkOrderOfOperation_whenAddAndDiv(CliArgs arg, double x, double y)
    {
        RunResult result = _runClass.Run(
            "TEST",
            $"{x.ToString(StaticConfig.numberCulture)} + {x.ToString(StaticConfig.numberCulture)} / {y.ToString(StaticConfig.numberCulture)}",
            arg
        );

        Assert.True(result.TryGetValue(out Value value));
        VList list = Assert.IsType<VList>(value);
        VNumber number = Assert.IsType<VNumber>(list.value[0]);
        Assert.Equal(x + x / y, number.value, TestingConstans.DOUBLE_PRECISION);
    }

    [Theory]
    [MemberData(nameof(TestCases))]
    public void checkOrderOfOperation_whenParams(CliArgs arg, double x, double y)
    {
        RunResult result = _runClass.Run(
            "TEST",
            $"{x.ToString(StaticConfig.numberCulture)} * ({y.ToString(StaticConfig.numberCulture)} + {x.ToString(StaticConfig.numberCulture)})",
            arg
        );

        Assert.True(result.TryGetValue(out Value value));
        VList list = Assert.IsType<VList>(value);
        VNumber number = Assert.IsType<VNumber>(list.value[0]);
        Assert.Equal(x * (y + x), number.value, TestingConstans.DOUBLE_PRECISION);
    }

    [Theory]
    [MemberData(nameof(TestCases))]
    public void checkOrderOfOperation_whenInvalidParams_thenError(CliArgs arg, double x, double y)
    {
        RunResult result = _runClass.Run(
            "TEST",
            $"{x.ToString(StaticConfig.numberCulture)} * ({y.ToString(StaticConfig.numberCulture)} + {x.ToString(StaticConfig.numberCulture)}",
            arg
        );

        Assert.True(result.IsFailed);
        InvalidSyntaxError err = Assert.IsType<InvalidSyntaxError>(result.GetError());
        Assert.Contains('(', err.ToString());
    }

    public static TheoryData<CliArgs, double, double> TestCases()
    {
        (double, double)[] values =
        [
            (0.0, 2.0),
            (100.0, 2.0),
            (0.5, 20.0),
            (0.5, -20.0),
            (0.5, -0.354),
        ];

        TheoryData<CliArgs, double, double> data = new TheoryData<CliArgs, double, double>();

        foreach (CliArgs mode in new[] { CliArgs.ArgsNoOptimization, CliArgs.ArgsWithOptimization })
        {
            foreach ((double x, double y) in values)
            {
                data.Add(mode, x, y);
            }
        }

        return data;
    }
}
