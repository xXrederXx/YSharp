using System.Runtime.CompilerServices;
using FastEnumUtility;
using YSharp.Types.Common;

namespace YSharp.Types.Lexer;

/// <summary>
///     A token is a class type which stores a StartPos and EndPos a Type and Optionaly a Value.
///     It has various functions which can be helpful
/// </summary>
public interface IToken{
    Position EndPos { get; }
    Position StartPos { get; }
    TokenType Type { get; }
    public bool IsMatchingKeyword(KeywordType value);
    public bool IsNotMatchingKeyword(KeywordType value);
    public bool IsNotType(TokenType type);
    public bool IsNotType(ReadOnlySpan<TokenType> types);
    public bool IsType(TokenType type);
    public bool IsType(ReadOnlySpan<TokenType> types);
}

/// <summary>
///     The token version with a Value
/// </summary>
/// <typeparam name="T">Type of the value</typeparam>
public class Token<T> : IToken{
    public Position EndPos { get; }
    public Position StartPos { get; }
    public TokenType Type { get; }
    public T Value { get; }

    // Constructor
    public Token(TokenType type, T value, in Position startPos, in Position endPos)
    {
        Type = type;
        Value = value;
        StartPos = startPos;
        EndPos = endPos;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public bool IsMatchingKeyword(KeywordType value) =>
        Type == TokenType.KEYWORD && Value is KeywordType keywordType && keywordType == value;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public bool IsNotMatchingKeyword(KeywordType value) => !IsMatchingKeyword(value);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public bool IsNotType(TokenType type) => Type != type;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public bool IsNotType(ReadOnlySpan<TokenType> types) => !IsType(types);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public bool IsType(TokenType type) => Type == type;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public bool IsType(ReadOnlySpan<TokenType> types)
    {
        foreach (TokenType t in types)
            if (Type == t)
                return true;
        return false;
    }

    // String Representation
    public override string ToString() => Value != null ? $"{Type.FastToString()}:{Value}" : Type.FastToString();
}