using YSharp.Core;
using YSharp.Types.Interpreter;
using YSharp.Utils;

namespace YSharp.Types.Common;

public class Error(int index, string message, Position start)
{
    private readonly string Message = message;
    public readonly int ErrorIndex = index;
    public readonly Position StartPosition = start;
    public bool IsError => this is not ErrorNull;

    public override string ToString() =>
        $"{FileNameRegistry.GetFileName(StartPosition.FileId)}({StartPosition.Line + 1}, {StartPosition.Column + 1}): ERROR YS{ErrorIndex:D4} {Message}";
}

// This is NoError, used instead of error = null
public class ErrorNull : Error
{
    public static readonly ErrorNull Instance = new();

    private ErrorNull()
        : base(0, string.Empty, Position.Null) { }

    public override string ToString() => string.Empty;
}

// Specific error types

//Y0S690 -> Internal Use not accualy an error
// TODO: Find way around this
public class EndKeywordError(Position posStart) : Error(690, "End keyword there", posStart) { }

public class IllegalCharError(Position posStart, char illegalChar)
    : Error(110, $"The character '{illegalChar}' is not a character", posStart) { }

public class ExpectedCharError(Position posStart, char expectedChar)
    : Error(111, $"Expected the character '{expectedChar}'", posStart) { }

public class IllegalEscapeCharError(Position posStart, char illegalChar)
    : Error(112, $"The character '{illegalChar}' is not a valide escape character", posStart) { }

public class IllegalNumberFormat(Position posStart)
    : Error(222, "You can't have 2 dots in a number", posStart) { }

public class InvalidSyntaxError(Position posStart, string details)
    : Error(220, details, posStart) { }

public class ExpectedKeywordError(Position posStart, string Expectedkeyword)
    : Error(221, "Expected the keyword " + Expectedkeyword, posStart) { }

public class ExpectedTokenError(Position posStart, string expectedToken)
    : Error(222, $"Expected the Token {expectedToken}", posStart) { }

public class ExpectedNewlineError(Position posStart)
    : Error(223, $"Expected a Newline", posStart) { }

public class ExpectedIdnetifierError(Position posStart)
    : Error(223, $"Expected an Identifier", posStart) { }

public class RunTimeError(Position posStart, string details, Context? context, int errorIndex = 300)
    : Error(errorIndex, details, posStart)
{
    private readonly Context? context = context;

    public override string ToString()
    {
        return base.ToString() + "\n" + GenerateTraceback();
    }

    private string GenerateTraceback()
    {
        if (StartPosition.IsNull)
            return "";

        string result = "";
        Position pos = StartPosition;
        Context? ctx = context;

        while (ctx != null && !pos.IsNull)
        {
            result =
                $"  File {FileNameRegistry.GetFileName(pos.FileId)}, line {pos.Line + 1}, in {ctx.displayName}\n"
                + result;
            pos = ctx.parentEntryPos;
            ctx = ctx.parent;
        }

        return "Traceback (most recent call last):\n" + result;
    }
}

public class VarNotFoundError(Position posStart, string varName, Context? context)
    : RunTimeError(posStart, $"Variable '{varName}' is not defined", context, 301);

public class FuncNotFoundError(Position posStart, string funcName, Context? context)
    : RunTimeError(posStart, $"Variable '{funcName}' is not defined", context, 302);

public class WrongFormatError(Position posStart, string details, Context? context)
    : RunTimeError(posStart, details, context, 303);

public class WrongTypeError(Position posStart, string details, Context? context)
    : RunTimeError(posStart, details, context, 303);

public class NumArgsError(Position posStart, int expectedCount, int parsedCount, Context? context)
    : RunTimeError(posStart, $"Expected {expectedCount} argument values, but only {parsedCount} where provided", context, 304);

public class ArgOutOfRangeError(Position posStart, string details, Context? context)
    : RunTimeError(posStart, details, context, 305);

public class IlligalOperationError(Position posStart, string details, Context? context)
    : RunTimeError(posStart, details, context, 306);

public class FileNotFoundError(Position posStart, string details, Context? context)
    : RunTimeError(posStart, details, context, 307);

public class InternalError(string details) : Error(400, details, Position.Null) { }

public class InternalLexerError(string details) : Error(410, details, Position.Null) { }

public class InternalParserError(string details) : Error(420, details, Position.Null) { }

public class InternalInterpreterError(string details) : Error(430, details, Position.Null) { }
