using System;

namespace YSharp_2._0;

public class Error (int index, string message, Position start){
    private readonly string Message = message;
    public readonly int ErrorIndex = index;
    public readonly Position StartPosition = start;
    public bool IsError => this is not NoError;

    public override string ToString()
    {
        string msg = StartPosition.FileName + "(" + (StartPosition.Line + 1) + ", "  + (StartPosition.Column + 1) + "): ";
        string number = ErrorIndex.ToString();
        while(number.Length < 4){
            number = "0" + number;
        } 
        msg = msg + "error YS" + number + ": ";
        msg += Message;
        return msg;
    }
}

// This is NoError, used instead of error = null
public class NoError : Error
{
    public static readonly NoError Instance = new();
    public NoError()
        : base(0, string.Empty, Position._null) { }

    public override string ToString() => string.Empty;
}

// Specific error types


// YS0100 -> Lexer
public class UnclosedBracketsError(Position posStart, string details) : Error(101, details, posStart){}
public class IllegalCharError(Position posStart, string details) : Error(110, details, posStart){}
public class ExpectedCharError(Position posStart, string details) : Error(111, details, posStart){}


// YS0200 -> Parser
public class InvalidSyntaxError(Position posStart, string details) : Error(220, details, posStart){}
public class ExpectedKeywordError(Position posStart, string details) : Error(221, details, posStart){}
public class ExpectedTokenError(Position posStart, string details) : Error(222, details, posStart){}



// YS0300 -> Interpreter
public class VarNotFoundError(Position posStart, string details) : Error(301, details, posStart){}
public class FuncNotFoundError(Position posStart, string details) : Error(302, details, posStart){}
public class WrongFormatError(Position posStart, string details) : Error(304, details, posStart){}
public class NumArgsError(Position posStart, string details) : Error(305, details, posStart){}
public class ArgOutOfRangeError(Position posStart, string details) : Error(306, details, posStart){}
public class RunTimeError(Position posStart, string details, Context? context, int errorIndex = 310) : Error(errorIndex, details, posStart)
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
            result = $"  File {pos.FileName}, line {pos.Line + 1}, in {ctx.displayName}\n" + result;
            pos = ctx.parentEntryPos;
            ctx = ctx.parent;
        }

        return "Traceback (most recent call last):\n" + result;
    }
}
public class IlligalOperationError(Position posStart, string details, Context? context) : RunTimeError(posStart, details, context, 311);

// YS0400 -> Other
public class InternalError(string details) : Error(400, details, Position._null){}
