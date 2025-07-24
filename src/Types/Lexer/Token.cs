using FastEnumUtility;
using YSharp.Types.Common;

namespace YSharp.Types.Lexer;

// Token types optimized using enum
public enum TokenType
{
    NULL,

    // Types
    INT,
    FLOAT,
    STRING,

    // Arithmetic
    PLUS,
    MINUS,
    MUL,
    DIV,
    POW,
    LPAREN,
    RPAREN,
    EQ,

    PP, // ++
    MM, // --
    PLEQ, // +=
    MIEQ, // -=
    MUEQ, // *=
    DIEQ, // /=

    // Comparison
    EE, // ==
    NE, // !=
    LT, // <
    GT, // >
    LTE, // <=
    GTE, // >=

    // Other
    IDENTIFIER,
    KEYWORD,
    COMMA,
    DOT,
    ARROW,
    LSQUARE, // [
    RSQUARE, // ]
    NEWLINE,
    EOF // End of file
    ,
}

public enum KeywordType
{
    VAR,
    AND,
    OR,
    NOT,
    IF,
    THEN,
    ELIF,
    ELSE,
    FOR,
    TO,
    STEP,
    WHILE,
    FUN,
    END,
    RETURN,
    CONTINUE,
    BREAK,
    TRY,
    CATCH,
    IMPORT,
}

public interface IToken
{
    TokenType Type { get; }
    Position StartPos { get; }
    Position EndPos { get; }
    public bool IsMatchingKeyword(KeywordType value);
    public bool IsNotMatchingKeyword(KeywordType value);
    public bool IsType(TokenType type);
    public bool IsType(params TokenType[] types);
    public bool IsNotType(TokenType type);
    public bool IsNotType(params TokenType[] types);
}

// Token class optimized with enum and nullable reference types
public class Token<T> : IToken
{
    public TokenType Type { get; }
    public T Value { get; }
    public Position StartPos { get; }
    public Position EndPos { get; }

    // Constructor
    public Token(TokenType type, T value, Position startPos, Position endPos)
    {
        Type = type;
        Value = value;

        if (!startPos.IsNull)
        {
            StartPos = startPos;
            EndPos = StartPos; // If there is no end Position just assume it to be 1 char
            EndPos.Advance(' ');
        }

        if (!endPos.IsNull)
        {
            EndPos = endPos;
        }
    }

    public Token(TokenType type, Position startPos, Position endPos)
        : this(type, default(T), startPos, endPos) { }

    public Token(TokenType type)
        : this(type, default(T), Position.Null, Position.Null) { }

    // String Representation
    public override string ToString()
    {
        return Value != null ? $"{Type.FastToString()}:{Value}" : Type.FastToString();
    }

    public bool IsMatchingKeyword(KeywordType value) =>
        Type == TokenType.KEYWORD && (Value is KeywordType keywordType) && keywordType == value;

    public bool IsNotMatchingKeyword(KeywordType value) => !IsMatchingKeyword(value);

    public bool IsType(TokenType type) => Type == type;

    public bool IsType(params TokenType[] types)
    {
        foreach (TokenType type in types)
        {
            if (Type == type)
            {
                return true;
            }
        }
        return false;
    }

    public bool IsNotType(TokenType type) => Type != type;

    public bool IsNotType(params TokenType[] types) => !IsType(types);
}

public class TokenNoValueType
{
    public static TokenNoValueType Instance = new();

    private TokenNoValueType() { }
}
