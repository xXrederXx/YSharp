using YSharp.Types.Interpreter.Internal;
using YSharp.Utils;

namespace YSharp.Types.Common;

public abstract class Error(int index, string message, Position start)
{
    private readonly string Message = message;
    public readonly int ErrorCode = index;
    public readonly Position StartPosition = start;
    public bool IsError => this is not ErrorNull;

    public override string ToString() =>
        $"{FileNameRegistry.GetFileName(StartPosition.FileId)}({StartPosition.Line + 1}, {StartPosition.Column + 1}): ERROR YS{ErrorCode:D4} {Message}";
}

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

//* 0000–0999: Lexical Errors
public class IllegalCharError(Position posStart, char illegalChar)
    : Error(0001, $"Illegal character '{illegalChar}'", posStart) { }

public class IllegalEscapeCharError(Position posStart, char illegalChar)
    : Error(0002, $"'{illegalChar}' is not a valid escape character", posStart) { }

public class InvalidEncodingError(Position posStart, string encoding)
    : Error(0003, $"Unsupported encoding: '{encoding}'", posStart) { }

public class ExpectedCharError(Position posStart, char expectedChar)
    : Error(0004, $"Expected the character '{expectedChar}'", posStart) { }

public class IllegalNumberFormat(Position posStart)
    : Error(0005, "You can't have 2 dots in a number", posStart) { }


//* 1000–1999: Syntax & Structure Errors
public class InvalidSyntaxError(Position posStart, string details)
    : Error(1000, details, posStart)
{ }

public class ExpectedKeywordError(Position posStart, string keyword)
    : Error(1001, $"Expected keyword '{keyword}'", posStart) { }

public class UnexpectedEOFError(Position posStart)
    : Error(1002, "Unexpected end of file", posStart) { }

public class UnexpectedIndentError(Position posStart)
    : Error(1003, "Unexpected indent", posStart) { }

public class ExpectedBlockError(Position posStart)
    : Error(1004, "Expected an indented code block", posStart) { }

public class UnmatchedBracketError(Position posStart, char bracket)
    : Error(1005, $"Unmatched bracket '{bracket}'", posStart) { }

public class ExpectedTokenError(Position posStart, string expectedToken)
    : Error(1006, $"Expected the Token {expectedToken}", posStart) { }

public class ExpectedNewlineError(Position posStart)
    : Error(1007, $"Expected a Newline", posStart) { }

public class ExpectedIdnetifierError(Position posStart)
    : Error(1008, $"Expected an Identifier", posStart) { }

//* 2000–2999: Semantic Errors
public class VarNotFoundError(Position posStart, string varName, Context? context)
    : RunTimeError(posStart, $"Variable '{varName}' is not defined", context, 2000);

public class FuncNotFoundError(Position posStart, string funcName, Context? context)
    : RunTimeError(posStart, $"Function '{funcName}' is not defined", context, 2001);

public class AssignmentToConstantError(Position posStart, string name, Context? context)
    : RunTimeError(posStart, $"Cannot assign to constant '{name}'", context, 2002);

public class ReservedNameError(Position posStart, string name, Context? context)
    : RunTimeError(posStart, $"'{name}' is a reserved keyword", context, 2003);

//* 3000–3999: Type & Value Errors
public class WrongTypeError(Position posStart, string details, Context? context)
    : RunTimeError(posStart, details, context, 3000);

public class NullReferenceError(Position posStart, string varName, Context? context)
    : RunTimeError(posStart, $"'{varName}' is null", context, 3001);

public class InvalidCastError(Position posStart, string fromType, string toType, Context? context)
    : RunTimeError(posStart, $"Cannot convert type '{fromType}' to '{toType}'", context, 3002);

public class WrongFormatError(Position posStart, string details, Context? context)
    : RunTimeError(posStart, details, context, 3003);

//* 4000–4999: Argument & Function Errors
public class NumArgsError(Position posStart, int expected, int actual, Context? context)
    : RunTimeError(posStart, $"Expected {expected} args, got {actual}", context, 4000);

public class ArgOutOfRangeError(Position posStart, string details, Context? context)
    : RunTimeError(posStart, details, context, 4001);

public class MissingRequiredArgError(Position posStart, string name, Context? context)
    : RunTimeError(posStart, $"Missing required argument '{name}'", context, 4002);

//* 5000–5999: Arithmetic Errors
public class DivisionByZeroError(Position posStart, Context? context)
    : RunTimeError(posStart, "Division by zero", context, 5000);

public class MathOverflowError(Position posStart, string details, Context? context)
    : RunTimeError(posStart, details, context, 5001);

public class InvalidMathOpError(Position posStart, string op, Context? context)
    : RunTimeError(posStart, $"Invalid operation: '{op}'", context, 5002);

public class IlligalOperationError(Position posStart, string details, Context? context)
    : RunTimeError(posStart, details, context, 306);

//* 6000–6999: Control Flow Errors
public class BreakOutsideLoopError(Position posStart, Context? context)
    : RunTimeError(posStart, "'break' used outside loop", context, 6000);

public class ReturnOutsideFunctionError(Position posStart, Context? context)
    : RunTimeError(posStart, "'return' used outside function", context, 6001);

public class MaxRecursionDepthError(Position posStart, Context? context)
    : RunTimeError(posStart, "Maximum recursion depth exceeded", context, 6002);

//* 7000–7999: IO & File Errors
public class FileNotFoundError(Position posStart, string details, Context? context)
    : RunTimeError(posStart, details, context, 7000);

public class FileReadError(Position posStart, string details, Context? context)
    : RunTimeError(posStart, details, context, 7001);

public class FileWriteError(Position posStart, string details, Context? context)
    : RunTimeError(posStart, details, context, 7002);

public class PermissionDeniedError(Position posStart, string file, Context? context)
    : RunTimeError(posStart, $"Permission denied: '{file}'", context, 7003);

//* 8000–8999: Module / Import Errors
public class ModuleNotFoundError(Position posStart, string moduleName, Context? context)
    : RunTimeError(posStart, $"Module '{moduleName}' not found", context, 8000);

public class CircularImportError(Position posStart, string moduleName, Context? context)
    : RunTimeError(posStart, $"Circular import detected: '{moduleName}'", context, 8001);

public class ImportSyntaxError(Position posStart, string details, Context? context)
    : RunTimeError(posStart, details, context, 8002);

//* 9000–9999: Internal / System Errors
public class InternalError(string details) : Error(9000, details, Position.Null) { }

public class InternalLexerError(string details) : Error(9001, details, Position.Null) { }

public class InternalParserError(string details) : Error(9002, details, Position.Null) { }

public class InternalInterpreterError(string details) : Error(9003, details, Position.Null) { }

public class AssertionFailedError(Position posStart, string message)
    : Error(9004, $"Assertion failed: {message}", posStart) { }
