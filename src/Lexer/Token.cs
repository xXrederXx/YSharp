using FastEnumUtility;
using YSharp.Common;

namespace YSharp.Lexer;

/// <summary>
///     A token is a class type which stores a StartPos and EndPos a Type and Optionaly a Value.
///     It has various functions which can be helpful
/// </summary>
public interface IToken
{
    Position EndPos { get; }
    Position StartPos { get; }
    TokenType Type { get; }
}

/// <summary>
///     The token version with a Value
/// </summary>
/// <typeparam name="T">Type of the value</typeparam>
public class Token<T> : IToken, IEquatable<Token<T>>
{
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

    // String Representation
    public override string ToString() =>
        Value != null ? $"{Type.FastToString()}:{Value}" : Type.FastToString();

    public override int GetHashCode()
    {
        return HashCode.Combine(EndPos, StartPos, Type, Value);
    }

    public override bool Equals(object? obj)
    {
        if (obj is not Token<T> token)
            return false;
        return Equals(token);
    }

    public bool Equals(Token<T>? other)
    {
        if (other is null)
            return false;
        return EndPos == other.EndPos
            && StartPos == other.StartPos
            && Type == other.Type
            && EqualityComparer<T>.Default.Equals(Value, other.Value);
    }
}
