using FastEnumUtility;
using YSharp.Common;

namespace YSharp.Lexer;

/// <summary>
/// Represents the minimal, immutable identity of a lexical token.
/// </summary>
/// <remarks>
/// <para>
/// <see cref="BaseToken"/> captures only the structural aspects of a token:
/// its <see cref="TokenType"/> and source span (<see cref="StartPos"/> to <see cref="EndPos"/>).
/// It intentionally does not include a value payload.
/// </para>
/// <para>
/// This type serves as a common base or lightweight token representation that can be
/// compared, hashed, and logged efficiently.
/// </para>
/// </remarks>
public class BaseToken : IEquatable<BaseToken>
{
    /// <summary>
    /// The inclusive end position of the token in the source text.
    /// </summary>
    public readonly Position EndPos;

    /// <summary>
    /// The start position of the token in the source text.
    /// </summary>
    public readonly Position StartPos;

    /// <summary>
    /// The lexical category of this token.
    /// </summary>
    public readonly TokenType Type;

    /// <summary>
    /// Initializes a new <see cref="BaseToken"/> with the given type and source span.
    /// </summary>
    /// <param name="type">The lexical category of the token.</param>
    /// <param name="startPos">The start position of the token in the source.</param>
    /// <param name="endPos">The end position of the token in the source.</param>
    public BaseToken(TokenType type, in Position startPos, in Position endPos)
    {
        Type = type;
        StartPos = startPos;
        EndPos = endPos;
    }

    /// <summary>
    /// Returns a string representation of the token type.
    /// </summary>
    /// <returns>
    /// The fast string representation of <see cref="Type"/>.
    /// </returns>
    public override string ToString() => Type.FastToString();

    /// <summary>
    /// Returns a hash code based on the token's type and source span.
    /// </summary>
    /// <returns>
    /// A hash code suitable for use in hash-based collections.
    /// </returns>
    public override int GetHashCode()
    {
        return HashCode.Combine(EndPos, StartPos, Type);
    }

    /// <summary>
    /// Determines whether the specified object is equal to the current token.
    /// </summary>
    /// <param name="obj">The object to compare with the current token.</param>
    /// <returns>
    /// <c>true</c> if the specified object is a <see cref="BaseToken"/> with the same
    /// type and source span; otherwise, <c>false</c>.
    /// </returns>
    public override bool Equals(object? obj)
    {
        if (obj is not BaseToken token)
            return false;
        return Equals(token);
    }

    /// <summary>
    /// Determines whether another <see cref="BaseToken"/> is equal to the current token.
    /// </summary>
    /// <param name="other">The token to compare with the current instance.</param>
    /// <returns>
    /// <c>true</c> if both tokens have identical type, start position, and end position;
    /// otherwise, <c>false</c>.
    /// </returns>
    public bool Equals(BaseToken? other)
    {
        if (other is null)
            return false;
        return EndPos == other.EndPos && StartPos == other.StartPos && Type == other.Type;
    }
}
