using Xunit;
using YSharp.Common;
using YSharp.Runtime;
using YSharp.Runtime.Collections.List;
using YSharp.Runtime.Primitives.Number;

namespace YSharp.Tests;

using RunnerResult = Result<Value, Error>;
public class IfTest
{
    private readonly RuntimeEnviroment runner = new RuntimeEnviroment();

    [Fact]
    public void checkIf_whenConditionTrue_thenExecutesThenBlock()
    {
        RunnerResult result = runner.Run(
            "<TEST>",
            """
            VAR x = 0
            IF x == 0 THEN
                VAR x = 1
            END
            x # Moves it to out
            """,
            CliArgs.DefaultArgs
        );

        Assert.True(result.TryGetValue(out Value resultValue));
        VList list = Assert.IsType<VList>(resultValue);
        VNumber number = Assert.IsType<VNumber>(list.value.Last());
        Assert.Equal(1.0, number.value, TestingConstans.DOUBLE_PRECISION);
    }

    [Fact]
    public void checkIf_whenConditionFalse_thenSkipsThenBlock()
    {
        RunnerResult result = runner.Run(
            "<TEST>",
            """
            VAR x = 7
            IF x < 0 THEN
                VAR x = 1
            END
            x # Moves it to out
            """,
            CliArgs.DefaultArgs
        );

        Assert.True(result.TryGetValue(out Value resultValue));
        VList list = Assert.IsType<VList>(resultValue);
        VNumber number = Assert.IsType<VNumber>(list.value.Last());
        Assert.Equal(7.0, number.value, TestingConstans.DOUBLE_PRECISION);
    }

    [Fact]
    public void checkIf_whenConditionFalseAndElse_thenExecutesElseBlock()
    {
        RunnerResult result = runner.Run(
            "<TEST>",
            """
            VAR x = 7
            IF x == 0 OR x != 7 THEN
                VAR x = 1
            END
            ELSE
                VAR x = 2
            END
            x # Moves it to out
            """,
            CliArgs.DefaultArgs
        );

        Assert.True(result.TryGetValue(out Value resultValue));
        VList list = Assert.IsType<VList>(resultValue);
        VNumber number = Assert.IsType<VNumber>(list.value.Last());
        Assert.Equal(2.0, number.value, TestingConstans.DOUBLE_PRECISION);
    }

    [Fact]
    public void checkIf_whenElifTrue_thenExecutesElifBlock()
    {
        RunnerResult result = runner.Run(
            "<TEST>",
            """
            VAR x = 7
            IF x <= 0 THEN
                VAR x = 1
            END
            ELIF NOT x == 6 THEN
                VAR x = 2
            END
            ELSE
                VAR x = 3
            END
            x # Moves it to out
            """,
            CliArgs.DefaultArgs
        );
        Assert.True(result.TryGetValue(out Value resultValue));
        VList list = Assert.IsType<VList>(resultValue);
        VNumber number = Assert.IsType<VNumber>(list.value.Last());
        Assert.Equal(2.0, number.value, TestingConstans.DOUBLE_PRECISION);
    }
}
