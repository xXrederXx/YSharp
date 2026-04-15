using Xunit;
using YSharp.Common;
using YSharp.Runtime;
using YSharp.Runtime.Collections.List;
using YSharp.Runtime.Primitives.Number;
using YSharp.Util;

namespace YSharp.Tests;

using RunnerResult = Result<Value, Error>;

public class ForTests
{
    private readonly RunClass runner = new RunClass();

    [Fact]
    void checkFor_whenValidLoopWithStep_succes()
    {
        RunnerResult result = runner.Run(
            "<TEST>",
            """
            VAR x = 0
            FOR i = 0 TO 10 STEP 0.5 THEN
                VAR x += i
            END
            x # Moves it to out
            """,
            CliArgs.DefaultArgs
        );

        Assert.True(result.TryGetValue(out Value resultValue));
        VList list = Assert.IsType<VList>(resultValue);
        VNumber number = Assert.IsType<VNumber>(list.value.Last());
        Assert.Equal(95.0, number.value, 1e-9);
    }

    [Fact]
    void checkFor_whenValidLoopNoStep_succes()
    {
        RunnerResult result = runner.Run(
            "<TEST>",
            """
            VAR x = 0
            FOR i = 0 TO 10 THEN
                VAR x += i
            END
            x # Moves it to out
            """,
            CliArgs.DefaultArgs
        );

        Assert.True(result.TryGetValue(out Value resultValue));
        VList list = Assert.IsType<VList>(resultValue);
        VNumber number = Assert.IsType<VNumber>(list.value.Last());
        Assert.Equal(45.0, number.value, 1e-9);
    }

    [Fact]
    void checkFor_whenNoIdentifier_fail()
    {
        RunnerResult result = runner.Run(
            "<TEST>",
            """
            VAR x = 0
            FOR 0 TO 10 THEN
                VAR x += i
            END
            x # Moves it to out
            """,
            CliArgs.DefaultArgs
        );

        Assert.True(result.IsFailed);
        Assert.IsType<ExpectedIdnetifierError>(result.GetError());
    }

    [Fact]
    void checkFor_whenNoEqual_fail()
    {
        RunnerResult result = runner.Run(
            "<TEST>",
            """
            VAR x = 0
            FOR x 0 TO 10 THEN
                VAR x += i
            END
            x # Moves it to out
            """,
            CliArgs.DefaultArgs
        );

        Assert.True(result.IsFailed);
        ExpectedTokenError err = Assert.IsType<ExpectedTokenError>(result.GetError());
        Assert.Contains("=", err.ToString());
    }

    [Fact]
    void checkFor_whenNoTO_fail()
    {
        RunnerResult result = runner.Run(
            "<TEST>",
            """
            VAR x = 0
            FOR x = 0  10 THEN
                VAR x += i
            END
            x # Moves it to out
            """,
            CliArgs.DefaultArgs
        );
        Assert.True(result.IsFailed);
        ExpectedKeywordError err = Assert.IsType<ExpectedKeywordError>(result.GetError());
        Assert.Contains("TO", err.ToString());
    }

    [Fact]
    void checkFor_whenNoTHEN_fail()
    {
        RunnerResult result = runner.Run(
            "<TEST>",
            """
            VAR x = 0
            FOR x = 0  TO 10
                VAR x += i
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
    void checkFor_whenNoEND_fail()
    {
        RunnerResult result = runner.Run(
            "<TEST>",
            """
            VAR x = 0
            FOR x = 0 TO 10 THEN
                VAR x += i
            """,
            CliArgs.DefaultArgs
        );
        Assert.True(result.IsFailed);
        ExpectedKeywordError err = Assert.IsType<ExpectedKeywordError>(result.GetError());
        Assert.Contains("END", err.ToString());
    }

    [Fact]
    void checkFor_whenInvalidStartType_WrongTypeError()
    {
        RunnerResult result = runner.Run(
            "<TEST>",
            """
            VAR x = 0
            FOR x = "0" TO 10 THEN
                VAR x += i
            END
            """,
            CliArgs.DefaultArgs
        );
        Assert.True(result.IsFailed);
        Assert.IsType<WrongTypeError>(result.GetError());
    }

    [Fact]
    void checkFor_whenInvalidEndType_WrongTypeError()
    {
        RunnerResult result = runner.Run(
            "<TEST>",
            """
            VAR x = 0
            FOR x = 0 TO "10" THEN
                VAR x += i
            END
            """,
            CliArgs.DefaultArgs
        );
        Assert.True(result.IsFailed);
        Assert.IsType<WrongTypeError>(result.GetError());
    }

    [Fact]
    void checkFor_whenInvalidStepType_WrongTypeError()
    {
        RunnerResult result = runner.Run(
            "<TEST>",
            """
            VAR x = 0
            FOR x = 0 TO 10 STEP "1" THEN
                VAR x += i
            END
            """,
            CliArgs.DefaultArgs
        );
        Assert.True(result.IsFailed);
        Assert.IsType<WrongTypeError>(result.GetError());
    }
}
