using Xunit;
using YSharp.Common;
using YSharp.Runtime;
using YSharp.Runtime.Collections.List;
using YSharp.Runtime.Primitives.Number;

namespace YSharp.Tests;

using RunResult = Result<Value, Error>;

public class FunctionDefinitonTest
{
    private readonly RuntimeEnviroment _runClass = new();

    [Fact]
    void checkFuncDef_whenValidWithName_thenWork()
    {
        RunResult result = _runClass.Run(
            "TEST",
            """
            FUN foo():
                RETURN 1
            END
            foo()
            """,
            CliArgs.DefaultArgs
        );

        Assert.True(result.TryGetValue(out Value value));
        VList list = Assert.IsType<VList>(value);
        VNumber number = Assert.IsType<VNumber>(list.value.Last());
        Assert.Equal(1, number.value, TestingConstans.DOUBLE_PRECISION);
    }

    [Fact]
    void checkFuncDef_whenValidNoName_thenWork()
    {
        RunResult result = _runClass.Run(
            "TEST",
            """
            VAR x = FUN ():
                RETURN 1
            END
            x()
            """,
            CliArgs.DefaultArgs
        );

        Assert.True(result.TryGetValue(out Value value));
        VList list = Assert.IsType<VList>(value);
        VNumber number = Assert.IsType<VNumber>(list.value.Last());
        Assert.Equal(1, number.value, TestingConstans.DOUBLE_PRECISION);
    }

    [Fact]
    void checkFuncDef_whenValidMultipleArgs_thenWork()
    {
        RunResult result = _runClass.Run(
            "TEST",
            """
            FUN add(x,y):
                RETURN x+y
            END
            add(6,7)
            """,
            CliArgs.DefaultArgs
        );

        Assert.True(result.TryGetValue(out Value value));
        VList list = Assert.IsType<VList>(value);
        VNumber number = Assert.IsType<VNumber>(list.value.Last());
        Assert.Equal(13, number.value, TestingConstans.DOUBLE_PRECISION);
    }
}
