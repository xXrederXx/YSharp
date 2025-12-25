using FastEnumUtility;
using YSharp.Common;

namespace YSharp.Lexer;

/// <summary>
/// Represents a lexical token that carries an associated value.
/// </summary>
/// <typeparam name="T">
/// The type of the value associated with the token.
/// </typeparam>
/// <remarks>
/// <para>
/// <see cref="Token{T}"/> extends <see cref="BaseToken"/> by attaching a typed value,
/// such as an identifier name, numeric literal, or string literal.
/// </para>
/// <para>
/// Equality and hashing incorporate both the base token identity
/// (type and source span) and the associated value.
/// </para>
/// </remarks>
public sealed class Token<T> : BaseToken, IEquatable<Token<T>>
{
    /// <summary>
    /// The value associated with this token.
    /// </summary>
    /// <remarks>
    /// The meaning of this value depends on the token type
    /// (e.g. literal contents, identifier text).
    /// </remarks>
    public readonly T Value;

    /// <summary>
    /// Initializes a new <see cref="Token{T}"/> with the given type, value, and source span.
    /// </summary>
    /// <param name="type">The lexical category of the token.</param>
    /// <param name="value">The value associated with the token.</param>
    /// <param name="startPos">The start position of the token in the source.</param>
    /// <param name="endPos">The end position of the token in the source.</param>
    public Token(TokenType type, T value, in Position startPos, in Position endPos)
        : base(type, startPos, endPos)
    {
        Value = value;
    }

    /// <summary>
    /// Returns a string representation of the token, including its value when present.
    /// </summary>
    /// <returns>
    /// A string containing the token type and, if non-null, its associated value.
    /// </returns>
    public override string ToString() =>
        Value != null ? $"{Type.FastToString()}:{Value}" : Type.FastToString();

    /// <summary>
    /// Returns a hash code based on the token's base identity and its value.
    /// </summary>
    /// <returns>
    /// A hash code suitable for use in hash-based collections.
    /// </returns>
    public override int GetHashCode()
    {
        return HashCode.Combine(base.GetHashCode(), Value);
    }

    /// <summary>
    /// Determines whether the specified object is equal to the current token.
    /// </summary>
    /// <param name="obj">The object to compare with the current token.</param>
    /// <returns>
    /// <c>true</c> if the specified object is a <see cref="Token{T}"/> with the same
    /// base token identity and value; otherwise, <c>false</c>.
    /// </returns>
    public override bool Equals(object? obj)
    {
        if (obj is not Token<T> token)
            return false;
        return Equals(token);
    }

    /// <summary>
    /// Determines whether another <see cref="Token{T}"/> is equal to the current token.
    /// </summary>
    /// <param name="other">The token to compare with the current instance.</param>
    /// <returns>
    /// <c>true</c> if both tokens have identical type, source span, and value;
    /// otherwise, <c>false</c>.
    /// </returns>
    public bool Equals(Token<T>? other)
    {
        if (other is null || !base.Equals(other))
            return false;
        return EqualityComparer<T>.Default.Equals(Value, other.Value);
    }
}
