using System.Runtime.CompilerServices;

namespace YSharp.Lexer;

/// <summary>
/// Provides convenience extension methods for inspecting and comparing tokens.
/// </summary>
/// <remarks>
/// <para>
/// These helpers are intended to improve readability at call sites and avoid
/// repetitive comparisons against <see cref="TokenType"/> or token values.
/// </para>
/// <para>
/// All methods are implemented as extensions and are safe to use on any
/// non-null token instance.
/// </para>
/// </remarks>
public static class TokenExtensions
{
    /// <summary>
    /// Determines whether the token has the specified <see cref="TokenType"/>.
    /// </summary>
    /// <param name="self">The token to inspect.</param>
    /// <param name="type">The token type to compare against.</param>
    /// <returns>
    /// <c>true</c> if the token's type matches <paramref name="type"/>; otherwise, <c>false</c>.
    /// </returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsType(this BaseToken self, TokenType type) => self.Type == type;

    /// <summary>
    /// Determines whether the token does not have the specified <see cref="TokenType"/>.
    /// </summary>
    /// <param name="self">The token to inspect.</param>
    /// <param name="type">The token type to compare against.</param>
    /// <returns>
    /// <c>true</c> if the token's type does not match <paramref name="type"/>; otherwise, <c>false</c>.
    /// </returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsNotType(this BaseToken self, TokenType type) => self.Type != type;

    /// <summary>
    /// Determines whether the token's type is contained within the provided set of types.
    /// </summary>
    /// <param name="self">The token to inspect.</param>
    /// <param name="types">A span of token types to test against.</param>
    /// <returns>
    /// <c>true</c> if the token's type matches any value in <paramref name="types"/>; otherwise, <c>false</c>.
    /// </returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsOneOf(this BaseToken self, ReadOnlySpan<TokenType> types)
    {
        foreach (TokenType type in types)
            if (self.IsType(type))
                return true;
        return false;
    }

    /// <summary>
    /// Determines whether the token's type is not contained within the provided set of types.
    /// </summary>
    /// <param name="self">The token to inspect.</param>
    /// <param name="types">A span of token types to test against.</param>
    /// <returns>
    /// <c>true</c> if the token's type does not match any value in <paramref name="types"/>; otherwise, <c>false</c>.
    /// </returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsNotOneOf(this BaseToken self, ReadOnlySpan<TokenType> types) =>
        !self.IsOneOf(types);

    /// <summary>
    /// Determines whether the value of the token is equal to the specified value.
    /// </summary>
    /// <typeparam name="T">The type of the token's value.</typeparam>
    /// <param name="self">The token whose value is compared.</param>
    /// <param name="value">The value to compare against.</param>
    /// <returns>
    /// <c>true</c> if the token's value is equal to <paramref name="value"/>; otherwise, <c>false</c>.
    /// </returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool ValueEquals<T>(this Token<T> self, T value) =>
        EqualityComparer<T>.Default.Equals(self.Value, value);

    /// <summary>
    /// Determines whether the value of the token is not equal to the specified value.
    /// </summary>
    /// <typeparam name="T">The type of the token's value.</typeparam>
    /// <param name="self">The token whose value is compared.</param>
    /// <param name="value">The value to compare against.</param>
    /// <returns>
    /// <c>true</c> if the token's value is not equal to <paramref name="value"/>; otherwise, <c>false</c>.
    /// </returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool ValueNotEquals<T>(this Token<T> self, T value) => !self.ValueEquals(value);
}
