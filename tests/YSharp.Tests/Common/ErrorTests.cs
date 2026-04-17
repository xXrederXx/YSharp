using Xunit;
using YSharp.Common;
using YSharp.Lexer;
using YSharp.Runtime;
using YSharp.Util;

namespace YSharp.Tests;

public class ErrorTests
{
    [Fact]
    void checkIsError_whenErrorNull_thenFalse()
    {
        Error err = ErrorNull.Instance;
        Assert.False(err.IsError);
    }

    [Fact]
    void checkIsError_whenErrorNotNull_thenTrue()
    {
        IllegalCharError err = new IllegalCharError(Position.Null, ' ');
        Assert.True(err.IsError);
    }

    [Fact]
    void checkIllegalCharError_whenToString_generateError()
    {
        Position startPos = new(0, 0, 0, FileNameRegistry.GetFileId("<TEST>"));
        Error err = new IllegalCharError(startPos, '#');

        Assert.True(err.IsError);

        string errorText = err.ToString();
        Assert.Contains("YS", errorText);
        Assert.Contains("#", errorText);
    }

    [Fact]
    void checkIllegalEscapeCharError_whenToString_generateError()
    {
        Position startPos = new(0, 0, 0, FileNameRegistry.GetFileId("<TEST>"));
        Error err = new IllegalEscapeCharError(startPos, '#');

        Assert.True(err.IsError);

        string errorText = err.ToString();
        Assert.Contains("YS", errorText);
        Assert.Contains("#", errorText);
    }

    [Fact]
    void checkExpectedCharError_whenToString_generateError()
    {
        Position startPos = new(0, 0, 0, FileNameRegistry.GetFileId("<TEST>"));
        Error err = new ExpectedCharError(startPos, '#');

        Assert.True(err.IsError);

        string errorText = err.ToString();
        Assert.Contains("YS", errorText);
        Assert.Contains("#", errorText);
    }

    [Fact]
    void checkIllegalNumberFormat_whenToString_generateError()
    {
        Position startPos = new(0, 0, 0, FileNameRegistry.GetFileId("<TEST>"));
        Error err = new IllegalNumberFormat(startPos);

        Assert.True(err.IsError);

        string errorText = err.ToString();
        Assert.Contains("YS", errorText);
    }

    [Fact]
    void checkInvalidSyntaxError_whenToString_generateError()
    {
        Position startPos = new(0, 0, 0, FileNameRegistry.GetFileId("<TEST>"));
        Error err = new InvalidSyntaxError(startPos, "some details");

        Assert.True(err.IsError);

        string errorText = err.ToString();
        Assert.Contains("YS", errorText);
        Assert.Contains("some details", errorText);
    }

    [Fact]
    void checkExpectedKeywordError_whenToString_generateError()
    {
        Position startPos = new(0, 0, 0, FileNameRegistry.GetFileId("<TEST>"));
        Error err = new ExpectedKeywordError(startPos, "IF");

        Assert.True(err.IsError);

        string errorText = err.ToString();
        Assert.Contains("YS", errorText);
        Assert.Contains("IF", errorText);
    }

    [Fact]
    void checkUnmatchedBracketError_whenToString_generateError()
    {
        Position startPos = new(0, 0, 0, FileNameRegistry.GetFileId("<TEST>"));
        Error err = new UnmatchedBracketError(startPos, '(');

        Assert.True(err.IsError);

        string errorText = err.ToString();
        Assert.Contains("YS", errorText);
        Assert.Contains("(", errorText);
    }

    [Fact]
    void checkExpectedTokenError_whenToString_generateError()
    {
        Position startPos = new(0, 0, 0, FileNameRegistry.GetFileId("<TEST>"));
        Error err = new ExpectedTokenError(startPos, "TOKEN");

        Assert.True(err.IsError);

        string errorText = err.ToString();
        Assert.Contains("YS", errorText);
        Assert.Contains("TOKEN", errorText);
    }

    [Fact]
    void checkExpectedNewlineError_whenToString_generateError()
    {
        Position startPos = new(0, 0, 0, FileNameRegistry.GetFileId("<TEST>"));
        Error err = new ExpectedNewlineError(startPos);

        Assert.True(err.IsError);

        string errorText = err.ToString();
        Assert.Contains("YS", errorText);
    }

    [Fact]
    void checkExpectedIdnetifierError_whenToString_generateError()
    {
        Position startPos = new(0, 0, 0, FileNameRegistry.GetFileId("<TEST>"));
        Error err = new ExpectedIdnetifierError(startPos);

        Assert.True(err.IsError);

        string errorText = err.ToString();
        Assert.Contains("YS", errorText);
    }

    [Fact]
    void checVarNotFoundError_whenToString_generateError()
    {
        Position startPos = new(0, 0, 0, FileNameRegistry.GetFileId("<TEST>"));
        Error err = new VarNotFoundError(
            startPos,
            "TOKEN",
            new Context(
                "<TEST-CONTEXT>",
                null,
                Position.Null,
                SymbolTable.GenerateGlobalSymboltable()
            )
        );

        Assert.True(err.IsError);

        string errorText = err.ToString();
        Assert.Contains("YS", errorText);
        Assert.Contains("Traceback", errorText);
        Assert.Contains("TOKEN", errorText);
    }

    [Fact]
    void checFuncNotFoundError_whenToString_generateError()
    {
        Position startPos = new(0, 0, 0, FileNameRegistry.GetFileId("<TEST>"));
        Error err = new FuncNotFoundError(
            startPos,
            "TOKEN",
            new Context(
                "<TEST-CONTEXT>",
                null,
                Position.Null,
                SymbolTable.GenerateGlobalSymboltable()
            )
        );

        Assert.True(err.IsError);

        string errorText = err.ToString();
        Assert.Contains("YS", errorText);
        Assert.Contains("Traceback", errorText);
        Assert.Contains("TOKEN", errorText);
    }

    [Fact]
    void checAccessDepricatedError_whenToString_generateError()
    {
        Position startPos = new(0, 0, 0, FileNameRegistry.GetFileId("<TEST>"));
        Error err = new AccessDepricatedError(
            startPos,
            "TOKEN",
            new Context(
                "<TEST-CONTEXT>",
                null,
                Position.Null,
                SymbolTable.GenerateGlobalSymboltable()
            )
        );

        Assert.True(err.IsError);

        string errorText = err.ToString();
        Assert.Contains("YS", errorText);
        Assert.Contains("Traceback", errorText);
        Assert.Contains("TOKEN", errorText);
    }

    [Fact]
    void checWrongTypeError_whenToString_generateError()
    {
        Position startPos = new(0, 0, 0, FileNameRegistry.GetFileId("<TEST>"));
        Error err = new WrongTypeError(
            startPos,
            "detail",
            new Context(
                "<TEST-CONTEXT>",
                null,
                Position.Null,
                SymbolTable.GenerateGlobalSymboltable()
            )
        );

        Assert.True(err.IsError);

        string errorText = err.ToString();
        Assert.Contains("YS", errorText);
        Assert.Contains("Traceback", errorText);
        Assert.Contains("detail", errorText);
    }

    [Fact]
    void checWrongFormatError_whenToString_generateError()
    {
        Position startPos = new(0, 0, 0, FileNameRegistry.GetFileId("<TEST>"));
        Error err = new WrongFormatError(
            startPos,
            "detail",
            new Context(
                "<TEST-CONTEXT>",
                null,
                Position.Null,
                SymbolTable.GenerateGlobalSymboltable()
            )
        );

        Assert.True(err.IsError);

        string errorText = err.ToString();
        Assert.Contains("YS", errorText);
        Assert.Contains("Traceback", errorText);
        Assert.Contains("detail", errorText);
    }

    [Fact]
    void checNumArgsError_whenToString_generateError()
    {
        Position startPos = new(0, 0, 0, FileNameRegistry.GetFileId("<TEST>"));
        Error err = new NumArgsError(
            startPos,
            1,
            2,
            new Context(
                "<TEST-CONTEXT>",
                null,
                Position.Null,
                SymbolTable.GenerateGlobalSymboltable()
            )
        );

        Assert.True(err.IsError);

        string errorText = err.ToString();
        Assert.Contains("YS", errorText);
        Assert.Contains("Traceback", errorText);
        Assert.Contains("1", errorText);
        Assert.Contains("2", errorText);
    }

    [Fact]
    void checArgOutOfRangeError_whenToString_generateError()
    {
        Position startPos = new(0, 0, 0, FileNameRegistry.GetFileId("<TEST>"));
        Error err = new ArgOutOfRangeError(
            startPos,
            "detail",
            new Context(
                "<TEST-CONTEXT>",
                null,
                Position.Null,
                SymbolTable.GenerateGlobalSymboltable()
            )
        );

        Assert.True(err.IsError);

        string errorText = err.ToString();
        Assert.Contains("YS", errorText);
        Assert.Contains("Traceback", errorText);
        Assert.Contains("detail", errorText);
    }

    [Fact]
    void checDivisionByZeroError_whenToString_generateError()
    {
        Position startPos = new(0, 0, 0, FileNameRegistry.GetFileId("<TEST>"));
        Error err = new DivisionByZeroError(
            startPos,
            new Context(
                "<TEST-CONTEXT>",
                null,
                Position.Null,
                SymbolTable.GenerateGlobalSymboltable()
            )
        );

        Assert.True(err.IsError);

        string errorText = err.ToString();
        Assert.Contains("YS", errorText);
        Assert.Contains("Traceback", errorText);
    }

    [Fact]
    void checIllegalOperationError_whenToString_generateError()
    {
        Position startPos = new(0, 0, 0, FileNameRegistry.GetFileId("<TEST>"));
        Error err = new IllegalOperationError(
            startPos,
            "detail",
            new Context(
                "<TEST-CONTEXT>",
                null,
                Position.Null,
                SymbolTable.GenerateGlobalSymboltable()
            )
        );

        Assert.True(err.IsError);

        string errorText = err.ToString();
        Assert.Contains("YS", errorText);
        Assert.Contains("Traceback", errorText);
        Assert.Contains("detail", errorText);
    }

    [Fact]
    void checFileNotFoundError_whenToString_generateError()
    {
        Position startPos = new(0, 0, 0, FileNameRegistry.GetFileId("<TEST>"));
        Error err = new FileNotFoundError(
            startPos,
            "details",
            new Context(
                "<TEST-CONTEXT>",
                null,
                Position.Null,
                SymbolTable.GenerateGlobalSymboltable()
            )
        );

        Assert.True(err.IsError);

        string errorText = err.ToString();
        Assert.Contains("YS", errorText);
        Assert.Contains("Traceback", errorText);
        Assert.Contains("details", errorText);
    }

    [Fact]
    void checFileReadError_whenToString_generateError()
    {
        Position startPos = new(0, 0, 0, FileNameRegistry.GetFileId("<TEST>"));
        Error err = new FileReadError(
            startPos,
            "details",
            new Context(
                "<TEST-CONTEXT>",
                null,
                Position.Null,
                SymbolTable.GenerateGlobalSymboltable()
            )
        );

        Assert.True(err.IsError);

        string errorText = err.ToString();
        Assert.Contains("YS", errorText);
        Assert.Contains("Traceback", errorText);
        Assert.Contains("details", errorText);
    }

    [Fact]
    void checkErrorNull_whenToString_generateEmpty()
    {
        Assert.Equal(string.Empty, ErrorNull.Instance.ToString());
    }

    [Fact]
    void checInvalidLoadedModuleError_whenToString_generateError()
    {
        Position startPos = new(0, 0, 0, FileNameRegistry.GetFileId("<TEST>"));
        Error err = new InvalidLoadedModuleError(
            startPos,
            "detail",
            new Context(
                "<TEST-CONTEXT>",
                null,
                Position.Null,
                SymbolTable.GenerateGlobalSymboltable()
            )
        );

        Assert.True(err.IsError);

        string errorText = err.ToString();
        Assert.Contains("YS", errorText);
        Assert.Contains("Traceback", errorText);
        Assert.Contains("detail", errorText);
    }

    [Fact]
    void checInternalLexerError_whenToString_generateError()
    {
        Error err = new InternalLexerError("detail");

        Assert.True(err.IsError);

        string errorText = err.ToString();
        Assert.Contains("YS", errorText);
        Assert.Contains("detail", errorText);
    }

    [Fact]
    void checInternalParserError_whenToString_generateError()
    {
        Error err = new InternalParserError("detail");

        Assert.True(err.IsError);

        string errorText = err.ToString();
        Assert.Contains("YS", errorText);
        Assert.Contains("detail", errorText);
    }

    [Fact]
    void checInternalInterpreterError_whenToString_generateError()
    {
        Error err = new InternalInterpreterError("detail");

        Assert.True(err.IsError);

        string errorText = err.ToString();
        Assert.Contains("YS", errorText);
        Assert.Contains("detail", errorText);
    }

    [Fact]
    void checInternalSymbolTableError_whenToString_generateError()
    {
        Error err = new InternalSymbolTableError(
            new Context(
                "<TEST-CONTEXT>",
                null,
                Position.Null,
                SymbolTable.GenerateGlobalSymboltable()
            )
        );

        Assert.True(err.IsError);

        string errorText = err.ToString();
        Assert.Contains("YS", errorText);
    }

    [Fact]
    void checInternalTokenCastError_whenToString_generateError()
    {
        Error err = new InternalTokenCastError<string>(
            new Token<int>(TokenType.NUMBER, 1, Position.Null, Position.Null),
            "detail"
        );

        Assert.True(err.IsError);

        string errorText = err.ToString();
        Assert.Contains("YS", errorText);
        Assert.Contains("detail", errorText);
        Assert.Contains("String", errorText);
        Assert.Contains("Int", errorText);
    }
}
