using FastEnumUtility;
using YSharp.Types.Common;

namespace YSharp.Types.Lexer;

public interface IToken
{
    TokenType Type { get; }
    Position StartPos { get; }
    Position EndPos { get; }
    public bool IsMatchingKeyword(KeywordType value);
    public bool IsNotMatchingKeyword(KeywordType value);
    public bool IsType(TokenType type);
    public bool IsType(ReadOnlySpan<TokenType> types);
    public bool IsNotType(TokenType type);
    public bool IsNotType(ReadOnlySpan<TokenType> types);
}

// Token class optimized with enum and nullable reference types
public class Token<T> : IToken
{
    public TokenType Type { get; }
    public T Value { get; }
    public Position StartPos { get; }
    public Position EndPos { get; }

    // Constructor
    public Token(TokenType type, T value, in Position startPos, in Position endPos)
    {
        Type = type;
        Value = value;
        StartPos = startPos;
        EndPos = endPos;
    }

    // String Representation
    public override string ToString()
    {
        return Value != null ? $"{Type.FastToString()}:{Value}" : Type.FastToString();
    }

    public bool IsMatchingKeyword(KeywordType value) =>
        Type == TokenType.KEYWORD && (Value is KeywordType keywordType) && keywordType == value;

    public bool IsNotMatchingKeyword(KeywordType value) => !IsMatchingKeyword(value);

    public bool IsType(TokenType type) => Type == type;

    public bool IsType(ReadOnlySpan<TokenType> types)
    {
        foreach (var t in types)
            if (Type == t)
                return true;
        return false;
    }

    public bool IsNotType(TokenType type) => Type != type;

    public bool IsNotType(ReadOnlySpan<TokenType> types) => !IsType(types);
}

public sealed class TokenNoValue : Token<TokenNoValueType>
{
    public TokenNoValue(TokenType type, in Position startPos, in Position endPos)
        : base(type, TokenNoValueType.Instance, startPos, endPos) { }
}

public sealed class NullToken : Token<TokenNoValueType>
{
    public static readonly NullToken Instance = new();

    private NullToken()
        : base(TokenType.NULL, TokenNoValueType.Instance, Position.Null, Position.Null) { }
}

public class TokenNoValueType
{
    public static readonly TokenNoValueType Instance = new();

    private TokenNoValueType() { }
}
