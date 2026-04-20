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
    private readonly RuntimeEnviroment runner = new RuntimeEnviroment();

    [Fact]
    public void checkFor_whenValidLoopWithStep_thenSumsTo95()
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
        Assert.Equal(95.0, number.value, TestingConstans.DOUBLE_PRECISION);
    }

    [Fact]
    public void checkFor_whenValidLoopNoStep_thenSumsTo45()
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
        Assert.Equal(45.0, number.value, TestingConstans.DOUBLE_PRECISION);
    }

    [Fact(Skip = "Issue#43")]
    public void checkForWithContinue_whenValidLoopNoStep_thenSkipsAllIterations()
    {
        RunnerResult result = runner.Run(
            "<TEST>",
            """
            VAR x = 0
            FOR i = 0 TO 10 THEN
                CONTINUE;
                VAR x += i
            END
            x # Moves it to out
            """,
            CliArgs.DefaultArgs
        );

        Assert.True(result.TryGetValue(out Value resultValue));
        VList list = Assert.IsType<VList>(resultValue);
        VNumber number = Assert.IsType<VNumber>(list.value.Last());
        Assert.Equal(0.0, number.value, TestingConstans.DOUBLE_PRECISION);
    }

    [Fact(Skip = "Issue#43")]
    public void checkForWithBreak_whenValidLoopNoStep_thenStopsAtFirstIteration()
    {
        RunnerResult result = runner.Run(
            "<TEST>",
            """
            VAR x = 0
            FOR i = 0 TO 10 THEN
                BREAK;
                VAR x += i
            END
            x # Moves it to out
            """,
            CliArgs.DefaultArgs
        );

        Assert.True(result.TryGetValue(out Value resultValue));
        VList list = Assert.IsType<VList>(resultValue);
        VNumber number = Assert.IsType<VNumber>(list.value.Last());
        Assert.Equal(0.0, number.value, TestingConstans.DOUBLE_PRECISION);
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

        Assert.True(result.TryGetError(out Error error));
        Assert.IsType<ExpectedIdnetifierError>(error);
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

        Assert.True(result.TryGetError(out Error error));
        ExpectedTokenError err = Assert.IsType<ExpectedTokenError>(error);
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
        Assert.True(result.TryGetError(out Error error));
        ExpectedKeywordError err = Assert.IsType<ExpectedKeywordError>(error);
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
        Assert.True(result.TryGetError(out Error error));
        ExpectedKeywordError err = Assert.IsType<ExpectedKeywordError>(error);
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
        Assert.True(result.TryGetError(out Error error));
        ExpectedKeywordError err = Assert.IsType<ExpectedKeywordError>(error);
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
        Assert.True(result.TryGetError(out Error error));
        Assert.IsType<WrongTypeError>(error);
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
        Assert.True(result.TryGetError(out Error error));
        Assert.IsType<WrongTypeError>(error);
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
        Assert.True(result.TryGetError(out Error error));
        Assert.IsType<WrongTypeError>(error);
    }
}
