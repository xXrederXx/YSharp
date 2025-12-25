using YSharp.Common;

namespace YSharp.Lexer;

/// <summary>
/// Represents a singleton token instance used to denote the absence of a real token.
/// </summary>
/// <remarks>
/// <para>
/// <see cref="NullToken"/> is a sentinel object that can be used in place of <c>null</c>
/// to avoid null checks when a token is required syntactically but no meaningful token
/// exists.
/// </para>
/// <para>
/// The token has a fixed <see cref="TokenType"/> of <see cref="TokenType.NULL"/> and
/// uses <see cref="Position.Null"/> for both start and end positions.
/// </para>
/// </remarks>
public sealed class NullToken : BaseToken
{
    /// <summary>
    /// The single shared instance of <see cref="NullToken"/>.
    /// </summary>
    public static readonly NullToken Instance = new();

    /// <summary>
    /// Initializes a new <see cref="NullToken"/> instance.
    /// </summary>
    /// <remarks>
    /// This constructor is private to enforce the singleton pattern.
    /// </remarks>
    private NullToken()
        : base(TokenType.NULL, Position.Null, Position.Null) { }
}
