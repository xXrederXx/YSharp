using Xunit;
using YSharp.Common;
using YSharp.Runtime;
using YSharp.Runtime.Collections.List;
using YSharp.Runtime.Primitives.Number;
using YSharp.Runtime.Primitives.String;
using YSharp.Util;

namespace YSharp.Tests;

using RunResult = Result<Value, Error>;

public class DotAccessTest
{
    private readonly RuntimeEnviroment _runClass = new();

    [Fact]
    public void checkCall()
    {
        RunResult result = _runClass.Run("<test>", $"VAR x = MATH.SIN(1);", CliArgs.DefaultArgs);

        Assert.True(result.TryGetValue(out Value value));
        VList list = Assert.IsType<VList>(value);
        VNumber number = Assert.IsType<VNumber>(list.value[0]);
        Assert.Equal(Math.Sin(1), number.value, TestingConstans.DOUBLE_PRECISION);
    }

    [Fact]
    public void checkGetVariable()
    {
        RunResult result = _runClass.Run("<test>", $"VAR x = MATH.PI;", CliArgs.DefaultArgs);

        Assert.True(result.TryGetValue(out Value value));
        VList list = Assert.IsType<VList>(value);
        VNumber number = Assert.IsType<VNumber>(list.value[0]);
        Assert.Equal(Math.PI, number.value, TestingConstans.DOUBLE_PRECISION);
    }
}
