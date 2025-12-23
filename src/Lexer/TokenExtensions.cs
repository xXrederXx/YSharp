using System.Runtime.CompilerServices;

namespace YSharp.Lexer;

public static class TokenExtensions
{
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsType(this IToken self, TokenType type) => self.Type == type;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsNotType(this IToken self, TokenType type) => self.Type != type;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsOneOf(this IToken self, ReadOnlySpan<TokenType> types)
    {
        foreach (TokenType type in types)
            if (self.IsType(type))
                return true;
        return false;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsNotOneOf(this IToken self, ReadOnlySpan<TokenType> types) =>
        !self.IsOneOf(types);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool ValueEquals<T>(this Token<T> self, T value)
        => EqualityComparer<T>.Default.Equals(self.Value, value);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool ValueNotEquals<T>(this Token<T> self, T value)
        => !self.ValueEquals(value);
}
