using YSharp.Lexer;
using YSharp.Runtime;
using YSharp.Util;

namespace YSharp.Common;

public abstract class Error(int errorCode, string message, Position startPos)
{
    public bool IsError => this is not ErrorNull;
    protected Position StartPos => startPos;

    public override string ToString() =>
        $"{FileNameRegistry.GetFileName(startPos.FileId)}({startPos.Line + 1}, {startPos.Column + 1}): ERROR YS{errorCode:D4} {message}";
}

public abstract class RunTimeError(Position startPos, string details, Context? context, int errorIndex)
    : Error(errorIndex, details, startPos)
{
    public override string ToString() => base.ToString() + "\n" + GenerateTraceback();

    private string GenerateTraceback()
    {
        if (StartPos.IsNull)
            return "";

        string result = "";
        Position pos = StartPos;
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
public class EndKeywordError(Position startPos) : Error(690, "End keyword there", startPos);

//* 0000–0999: Lexical Errors
public class IllegalCharError(Position startPos, char illegalChar)
    : Error(0001, $"Illegal character '{illegalChar}'", startPos)
{ }

public class IllegalEscapeCharError(Position startPos, char illegalChar)
    : Error(0002, $"'{illegalChar}' is not a valid escape character", startPos)
{ }

public class InvalidEncodingError(Position startPos, string encoding)
    : Error(0003, $"Unsupported encoding: '{encoding}'", startPos)
{ }

public class ExpectedCharError(Position startPos, char expectedChar)
    : Error(0004, $"Expected the character '{expectedChar}'", startPos)
{ }

public class IllegalNumberFormat(Position startPos)
    : Error(0005, "You can't have 2 dots in a number", startPos)
{ }

//* 1000–1999: Syntax & Structure Errors
public class InvalidSyntaxError(Position startPos, string details)
    : Error(1000, details, startPos)
{ }

public class ExpectedKeywordError(Position startPos, string keyword)
    : Error(1001, $"Expected keyword '{keyword}'", startPos)
{ }

public class UnexpectedEOFError(Position startPos)
    : Error(1002, "Unexpected end of file", startPos)
{ }

public class UnexpectedIndentError(Position startPos)
    : Error(1003, "Unexpected indent", startPos)
{ }

public class ExpectedBlockError(Position startPos)
    : Error(1004, "Expected an indented code block", startPos)
{ }

public class UnmatchedBracketError(Position startPos, char bracket)
    : Error(1005, $"Unmatched bracket '{bracket}'", startPos)
{ }

public class ExpectedTokenError(Position startPos, string expectedToken)
    : Error(1006, $"Expected the Token {expectedToken}", startPos)
{ }

public class ExpectedNewlineError(Position startPos)
    : Error(1007, "Expected a Newline", startPos)
{ }

public class ExpectedIdnetifierError(Position startPos)
    : Error(1008, "Expected an Identifier", startPos)
{ }

//* 2000–2999: Semantic Errors
public class VarNotFoundError(Position startPos, string varName, Context? context)
    : RunTimeError(startPos, $"Variable '{varName}' is not defined", context, 2000);

public class FuncNotFoundError(Position startPos, string funcName, Context? context)
    : RunTimeError(startPos, $"Function '{funcName}' is not defined", context, 2001);

public class AssignmentToConstantError(Position startPos, string name, Context? context)
    : RunTimeError(startPos, $"Cannot assign to constant '{name}'", context, 2002);

public class ReservedNameError(Position startPos, string name, Context? context)
    : RunTimeError(startPos, $"'{name}' is a reserved keyword", context, 2003);

public class AccessDepricatedError(Position startPos, string name, Context? context)
    : RunTimeError(startPos, $"Cannot access '{name}' because its depricated", context, 2004);

//* 3000–3999: Type & Value Errors
public class WrongTypeError(Position startPos, string details, Context? context)
    : RunTimeError(startPos, details, context, 3000);

public class NullReferenceError(Position startPos, string varName, Context? context)
    : RunTimeError(startPos, $"'{varName}' is null", context, 3001);

public class InvalidCastError(Position startPos, string fromType, string toType, Context? context)
    : RunTimeError(startPos, $"Cannot convert type '{fromType}' to '{toType}'", context, 3002);

public class WrongFormatError(Position startPos, string details, Context? context)
    : RunTimeError(startPos, details, context, 3003);

//* 4000–4999: Argument & Function Errors
public class NumArgsError(Position startPos, int expected, int actual, Context? context)
    : RunTimeError(startPos, $"Expected {expected} args, got {actual}", context, 4000);

public class ArgOutOfRangeError(Position startPos, string details, Context? context)
    : RunTimeError(startPos, details, context, 4001);

public class MissingRequiredArgError(Position startPos, string name, Context? context)
    : RunTimeError(startPos, $"Missing required argument '{name}'", context, 4002);

//* 5000–5999: Arithmetic Errors
public class DivisionByZeroError(Position startPos, Context? context)
    : RunTimeError(startPos, "Division by zero", context, 5000);

public class MathOverflowError(Position startPos, string details, Context? context)
    : RunTimeError(startPos, details, context, 5001);

public class InvalidMathOpError(Position startPos, string op, Context? context)
    : RunTimeError(startPos, $"Invalid operation: '{op}'", context, 5002);

public class IlligalOperationError(Position startPos, string details, Context? context)
    : RunTimeError(startPos, details, context, 306);

//* 6000–6999: Control Flow Errors
public class BreakOutsideLoopError(Position startPos, Context? context)
    : RunTimeError(startPos, "'break' used outside loop", context, 6000);

public class ReturnOutsideFunctionError(Position startPos, Context? context)
    : RunTimeError(startPos, "'return' used outside function", context, 6001);

public class MaxRecursionDepthError(Position startPos, Context? context)
    : RunTimeError(startPos, "Maximum recursion depth exceeded", context, 6002);

//* 7000–7999: IO & File Errors
public class FileNotFoundError(Position startPos, string details, Context? context)
    : RunTimeError(startPos, details, context, 7000);

public class FileReadError(Position startPos, string details, Context? context)
    : RunTimeError(startPos, details, context, 7001);

public class FileWriteError(Position startPos, string details, Context? context)
    : RunTimeError(startPos, details, context, 7002);

public class PermissionDeniedError(Position startPos, string file, Context? context)
    : RunTimeError(startPos, $"Permission denied: '{file}'", context, 7003);

//* 8000–8999: Module / Import Errors
public class ModuleNotFoundError(Position startPos, string moduleName, Context? context)
    : RunTimeError(startPos, $"Module '{moduleName}' not found", context, 8000);

public class CircularImportError(Position startPos, string moduleName, Context? context)
    : RunTimeError(startPos, $"Circular import detected: '{moduleName}'", context, 8001);

public class ImportSyntaxError(Position startPos, string details, Context? context)
    : RunTimeError(startPos, details, context, 8002);

public class InvalidLoadedModuleError(Position startPos, string details, Context? context)
    : RunTimeError(startPos, details, context, 8003);

//* 9000–9999: Internal / System Errors
public class InternalError(int code, string details) : Error(code, details, Position.Null);

public class InternalLexerError(string details) : InternalError(9001, details);

public class InternalParserError(string details) : InternalError(9002, details);

public class InternalInterpreterError(string details) : InternalError(9003, details);

public class AssertionFailedError(Position startPos, string message)
    : Error(9004, $"Assertion failed: {message}", startPos)
{ }

public class InternalSymbolTableError(Context context)
    : InternalError(9005, "The current Symbol Table is null\n" + context);

public class InternalTokenCastError<T>(BaseToken token, string membername) : InternalError(9006,
    $"Casting the token ({token.GetType()}) to a Token<{typeof(T)}> failed in {membername} / Token: {token}");
