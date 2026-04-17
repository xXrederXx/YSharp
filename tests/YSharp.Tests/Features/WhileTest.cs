using Xunit;
using YSharp.Common;
using YSharp.Runtime;
using YSharp.Runtime.Collections.List;
using YSharp.Runtime.Primitives.Number;

namespace YSharp.Tests;

using RunnerResult = Result<Value, Error>;

public class WhileTest
{
    private readonly RuntimeEnviroment runner = new RuntimeEnviroment();

    [Fact]
    void checkWhile_whenValidLoop_success()
    {
        RunnerResult result = runner.Run(
            "<TEST>",
            """
            VAR x = 0
            WHILE x < 10 THEN
                VAR x += 1
            END
            x # Moves it to out
            """,
            CliArgs.DefaultArgs
        );

        Assert.True(result.TryGetValue(out Value resultValue));
        VList list = Assert.IsType<VList>(resultValue);
        VNumber number = Assert.IsType<VNumber>(list.value.Last());
        Assert.Equal(10.0, number.value, 1e-9);
    }

    [Fact]
    void checkWhile_whenNoTHEN_fail()
    {
        RunnerResult result = runner.Run(
            "<TEST>",
            """
            VAR x = 0
            WHILE x < 10
                VAR x += 1
            END
            x # Moves it to out
            """,
            CliArgs.DefaultArgs
        );

        Assert.True(result.IsFailed);
        ExpectedKeywordError err = Assert.IsType<ExpectedKeywordError>(result.GetError());
        Assert.Contains("THEN", err.ToString());
    }

    [Fact]
    void checkWhile_whenNoEND_fail()
    {
        RunnerResult result = runner.Run(
            "<TEST>",
            """
            VAR x = 0
            WHILE x < 10 THEN
                VAR x += 1
            """,
            CliArgs.DefaultArgs
        );

        Assert.True(result.IsFailed);
        ExpectedKeywordError err = Assert.IsType<ExpectedKeywordError>(result.GetError());
        Assert.Contains("END", err.ToString());
    }
}
