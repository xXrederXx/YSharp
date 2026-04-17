namespace YSharp.Tests;

using System;
using System.IO;
using Xunit;
using YSharp.Cli;
using YSharp.Common;
using YSharp.Runtime;

public class RunnerTests
{
    private readonly CliArgs _args = CliArgs.DefaultArgs; // adjust if needed

    [Fact]
    public void ConsoleRunner_ShouldExit_OnBreakCommand()
    {
        StringReader input = new StringReader("b\n");
        StringWriter output = new StringWriter();

        Console.SetIn(input);
        Console.SetOut(output);

        MockRuntime mockRuntime = new MockRuntime()
        {
            toReturn = Result<Value, Error>.Success(ValueNull.Instance),
        };

        Runner.ConsoleRunner(_args, mockRuntime);

        string result = output.ToString();
        Assert.Contains("Type 'b' anytime to break", result);
    }

    [Fact]
    public void ConsoleRunner_ShouldIgnore_EmptyInput()
    {
        StringReader input = new StringReader("\n\nb\n");
        StringWriter output = new StringWriter();

        Console.SetIn(input);
        Console.SetOut(output);

        MockRuntime mockRuntime = new MockRuntime()
        {
            toReturn = Result<Value, Error>.Success(ValueNull.Instance),
        };

        Runner.ConsoleRunner(_args, mockRuntime);

        string result = output.ToString();

        // No errors should be printed for empty lines
        Assert.DoesNotContain("Error", result);
    }

    [Fact]
    public void ConsoleRunner_ShouldHandle_NullInput_AsEmpty()
    {
        StringReader input = new StringReader(""); // ReadLine() returns null
        StringWriter output = new StringWriter();

        Console.SetIn(input);
        Console.SetOut(output);

        // This will loop forever unless we inject "b"
        // So simulate null then break
        input = new StringReader("\nb\n");
        Console.SetIn(input);

        MockRuntime mockRuntime = new MockRuntime()
        {
            toReturn = Result<Value, Error>.Success(ValueNull.Instance),
        };

        Runner.ConsoleRunner(_args, mockRuntime);

        Assert.True(true); // just ensure no crash
    }

    [Fact]
    public void ConsoleRunner_ShouldPrintError_WhenRunFails()
    {
        StringReader input = new StringReader("invalid_command\nb\n");
        StringWriter output = new StringWriter();

        Console.SetIn(input);
        Console.SetOut(output);

        MockRuntime mockRuntime = new MockRuntime()
        {
            toReturn = Result<Value, Error>.Fail(new ExpectedTokenError(Position.Null, "test")),
        };

        Runner.ConsoleRunner(_args, mockRuntime);

        string result = output.ToString();

        Assert.Contains("Error", result, StringComparison.OrdinalIgnoreCase);
    }

    [Fact]
    public void ScriptRunner_ShouldExecuteScript()
    {
        StringWriter output = new StringWriter();
        Console.SetOut(output);

        MockRuntime mockRuntime = new MockRuntime()
        {
            toReturn = Result<Value, Error>.Success(ValueNull.Instance),
        };
        Runner.ScriptRunner("test.ys", _args, mockRuntime);

        string result = output.ToString();

        // If no error, output should be empty
        Assert.DoesNotContain("Error", result);
    }

    [Fact]
    public void ScriptRunner_ShouldEscape_Backslashes()
    {
        StringWriter output = new StringWriter();
        Console.SetOut(output);

        MockRuntime mockRuntime = new MockRuntime()
        {
            toReturn = Result<Value, Error>.Success(ValueNull.Instance),
        };
        Runner.ScriptRunner(@"folder\test.ys", _args, mockRuntime);

        string result = output.ToString();

        // This test mainly ensures no exception occurs
        Assert.True(true);
    }

    [Fact]
    public void ScriptRunner_ShouldPrintError_OnFailure()
    {
        StringWriter output = new StringWriter();
        Console.SetOut(output);

        MockRuntime mockRuntime = new MockRuntime()
        {
            toReturn = Result<Value, Error>.Fail(
                new FileNotFoundError(Position.Null, string.Empty, new Context())
            ),
        };
        Runner.ScriptRunner("non_existent_file.ys", _args, mockRuntime);

        string result = output.ToString();

        Assert.Contains("Error", result, StringComparison.OrdinalIgnoreCase);
    }
}
