using FastEnumUtility;
using YSharp.Common;

namespace YSharp.Lexer;

/// <summary>
///     The token version with a Value
/// </summary>
/// <typeparam name="T">Type of the value</typeparam>
public sealed class Token<T> : BaseToken, IEquatable<Token<T>>
{
    public T Value { get; }

    // Constructor
    public Token(TokenType type, T value, in Position startPos, in Position endPos)
        : base(type, startPos, endPos)
    {
        Value = value;
    }

    // String Representation
    public override string ToString() =>
        Value != null ? $"{Type.FastToString()}:{Value}" : Type.FastToString();

    public override int GetHashCode()
    {
        return HashCode.Combine(base.GetHashCode(), Value);
    }

    public override bool Equals(object? obj)
    {
        if (obj is not Token<T> token)
            return false;
        return Equals(token);
    }

    public bool Equals(Token<T>? other)
    {
        if (other is null || !base.Equals(other))
            return false;
        return EqualityComparer<T>.Default.Equals(Value, other.Value);
    }
}
