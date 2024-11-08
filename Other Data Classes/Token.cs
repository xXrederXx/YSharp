namespace YSharp_2._0;

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
}

// Keywords optimized with HashSet
public static class TokenTypeHelper
{
    private static readonly HashSet<string> Keywords =
    [
        "VAR", "AND", "OR", "NOT", "IF", "THEN", "ELIF", "ELSE",
        "FOR", "TO", "STEP", "WHILE", "FUN", "END", "RETURN", "CONTINUE", "BREAK", "TRY", "CATCH"
    ];

    public static bool IsKeyword(string s) => Keywords.Contains(s);
}
public interface IToken{
    TokenType Type { get; }
    Position StartPos { get; }
    Position EndPos { get; }
    public bool Matches(TokenType type, string value){
        return false;
    }
}


// Token class optimized with enum and nullable reference types
public class Token<T> : IToken
{
    public TokenType Type { get; }
    public  T Value { get; }
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
    public Token(TokenType type, Position startPos, Position endPos) : this(type, default(T), startPos, endPos){}
    public Token(TokenType type) : this(type, default(T), Position.Null, Position.Null){}

    // String Representation
    public override string ToString()
    {
        return Value != null ? $"{Type}:{Value}" : Type.ToString();
    }

    public bool Matches(TokenType type, string value)
    {
        if(Value is null){
            return Type.Equals(type);
        }
        return Type == type && (Value as string) == value;
    }
}

public class TokenNoValueType{
    public static TokenNoValueType Instance = new();
    private TokenNoValueType(){}
}
