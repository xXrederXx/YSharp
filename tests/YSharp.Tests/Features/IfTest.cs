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
    void checkIF_whenFirstTrue_success()
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
        Assert.Equal(1.0, number.value, 1e-9);
    }

    [Fact]
    void checkIF_whenFirstFalse_success()
    {
        RunnerResult result = runner.Run(
            "<TEST>",
            """
            VAR x = 7
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
        Assert.Equal(7.0, number.value, 1e-9);
    }

    [Fact]
    void checkIF_whenFirstFalseAndElse_success()
    {
        RunnerResult result = runner.Run(
            "<TEST>",
            """
            VAR x = 7
            IF x == 0 THEN
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
        Assert.Equal(2.0, number.value, 1e-9);
    }
}
