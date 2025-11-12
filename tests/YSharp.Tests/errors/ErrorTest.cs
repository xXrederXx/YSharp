using Xunit;
using YSharp.Types.Common;
using YSharp.Utils;

namespace YSharp.Tests.Errors;

public class ErrorTest
{
    private readonly RunClass _runClass = new();

    [Theory]
    [InlineData("", typeof(IllegalCharError))]
    [InlineData("", typeof(IllegalEscapeCharError))]
    [InlineData("", typeof(InvalidEncodingError))]
    [InlineData("", typeof(ExpectedCharError))]
    [InlineData("", typeof(IllegalNumberFormat))]
    public void TestErrors_0(string input, Type expectedErrorType)
    {
        // When
        var res = _runClass.Run("TEST", input);

        // Then
        Assert.NotNull(res.Item2);
        Assert.IsType(expectedErrorType, res.Item2);
    }

    [Theory]
    [InlineData("", typeof(InvalidSyntaxError))]
    [InlineData("", typeof(ExpectedKeywordError))]
    [InlineData("", typeof(UnexpectedEOFError))]
    [InlineData("", typeof(UnexpectedIndentError))]
    [InlineData("", typeof(ExpectedBlockError))]
    [InlineData("", typeof(UnmatchedBracketError))]
    [InlineData("", typeof(ExpectedTokenError))]
    [InlineData("", typeof(ExpectedNewlineError))]
    [InlineData("", typeof(ExpectedIdnetifierError))]
    public void TestErrors_1(string input, Type expectedErrorType)
    {
        // When
        var res = _runClass.Run("TEST", input);

        // Then
        Assert.NotNull(res.Item2);
        Assert.IsType(expectedErrorType, res.Item2);
    }

    [Theory]
    [InlineData("", typeof(VarNotFoundError))]
    [InlineData("", typeof(FuncNotFoundError))]
    [InlineData("", typeof(AssignmentToConstantError))]
    [InlineData("", typeof(ReservedNameError))]
    public void TestErrors_2(string input, Type expectedErrorType)
    {
        // When
        var res = _runClass.Run("TEST", input);

        // Then
        Assert.NotNull(res.Item2);
        Assert.IsType(expectedErrorType, res.Item2);
    }

    [Theory]
    [InlineData("", typeof(WrongTypeError))]
    [InlineData("", typeof(NullReferenceError))]
    [InlineData("", typeof(InvalidCastError))]
    [InlineData("", typeof(WrongFormatError))]
    public void TestErrors_3(string input, Type expectedErrorType)
    {
        // When
        var res = _runClass.Run("TEST", input);

        // Then
        Assert.NotNull(res.Item2);
        Assert.IsType(expectedErrorType, res.Item2);
    }

    [Theory]
    [InlineData("", typeof(NumArgsError))]
    [InlineData("", typeof(ArgOutOfRangeError))]
    [InlineData("", typeof(MissingRequiredArgError))]
    public void TestErrors_4(string input, Type expectedErrorType)
    {
        // When
        var res = _runClass.Run("TEST", input);

        // Then
        Assert.NotNull(res.Item2);
        Assert.IsType(expectedErrorType, res.Item2);
    }

    [Theory]
    [InlineData("", typeof(DivisionByZeroError))]
    [InlineData("", typeof(MathOverflowError))]
    [InlineData("", typeof(InvalidMathOpError))]
    [InlineData("", typeof(IlligalOperationError))]
    public void TestErrors_5(string input, Type expectedErrorType)
    {
        // When
        var res = _runClass.Run("TEST", input);

        // Then
        Assert.NotNull(res.Item2);
        Assert.IsType(expectedErrorType, res.Item2);
    }

    [Theory]
    [InlineData("", typeof(BreakOutsideLoopError))]
    [InlineData("", typeof(ReturnOutsideFunctionError))]
    [InlineData("", typeof(MaxRecursionDepthError))]
    public void TestErrors_6(string input, Type expectedErrorType)
    {
        // When
        var res = _runClass.Run("TEST", input);

        // Then
        Assert.NotNull(res.Item2);
        Assert.IsType(expectedErrorType, res.Item2);
    }

    [Theory]
    [InlineData("", typeof(FileNotFoundError))]
    [InlineData("", typeof(FileReadError))]
    [InlineData("", typeof(FileWriteError))]
    [InlineData("", typeof(PermissionDeniedError))]
    public void TestErrors_7(string input, Type expectedErrorType)
    {
        // When
        var res = _runClass.Run("TEST", input);

        // Then
        Assert.NotNull(res.Item2);
        Assert.IsType(expectedErrorType, res.Item2);
    }

    [Theory]
    [InlineData("", typeof(ModuleNotFoundError))]
    [InlineData("", typeof(CircularImportError))]
    [InlineData("", typeof(ImportSyntaxError))]
    public void TestErrors_8(string input, Type expectedErrorType)
    {
        // When
        var res = _runClass.Run("TEST", input);

        // Then
        Assert.NotNull(res.Item2);
        Assert.IsType(expectedErrorType, res.Item2);
    }

    [Theory]
    [InlineData("", typeof(InternalError))]
    [InlineData("", typeof(InternalLexerError))]
    [InlineData("", typeof(InternalParserError))]
    [InlineData("", typeof(InternalInterpreterError))]
    [InlineData("", typeof(AssertionFailedError))]
    public void TestErrors_9(string input, Type expectedErrorType)
    {
        // When
        var res = _runClass.Run("TEST", input);

        // Then
        Assert.NotNull(res.Item2);
        Assert.IsType(expectedErrorType, res.Item2);
    }
}
