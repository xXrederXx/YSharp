using System.Runtime.CompilerServices;
using System.Text;
using FastEnumUtility;
using YSharp.Types.Common;
using YSharp.Types.Lexer;

namespace YSharp.Core;

public class Lexer
{
    private readonly string text;
    private Position pos;
    private char current_char = char.MaxValue;
    private readonly StringBuilder stringBuilder = new();

    // Initalizer
    public Lexer(string text, string fileName)
    {
        this.text = text;

        // The .MaxValue enshures that the first advance call overflows to
        pos = new Position(-1, 0, ushort.MaxValue, fileName);
        Advance();
    }

    // this uptades to the next charachter
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private void Advance()
    {
        pos.Advance(current_char);
        current_char = pos.Index < text.Length ? text[pos.Index] : char.MaxValue;
    }

    private static bool IsValidIdentifierChar(char c) => char.IsLetterOrDigit(c) || c == '_';

    // this is used to make a number token of type int or float
    private (Token<double>, Error) MakeNumber()
    {
        bool hasDot = false;
        Position posStart = pos;

        while (char.IsDigit(current_char) || current_char == '.' || current_char == '_')
        { // while the char is a number or a dot
            if (current_char == '_')
            {
                Advance();
                continue;
            }

            if (current_char == '.')
            {
                if (hasDot)
                {
                    return (
                        new Token<double>(TokenType.NULL),
                        new IllegalCharError(posStart, "You can't have 2 dots in a number")
                    ); // cant have 2 dots in a number
                }

                hasDot = true;
                stringBuilder.Append('.');
            }
            else
            { // just a number
                stringBuilder.Append(current_char);
            }
            Advance();
        }
        double value = double.Parse(stringBuilder.ToString());
        stringBuilder.Clear();
        return (new Token<double>(TokenType.FLOAT, value, posStart, pos), ErrorNull.Instance);
    }

    private IToken MakeIdentifier()
    {
        Position posStart = pos;

        while (IsValidIdentifierChar(current_char))
        {
            stringBuilder.Append(current_char);
            Advance();
        }

        string idStr = stringBuilder.ToString();
        bool IsKeyword = FastEnum.TryParse<KeywordType>(idStr, out KeywordType keywordType);
        stringBuilder.Clear();

        if (IsKeyword)
        {
            return new Token<KeywordType>(TokenType.KEYWORD, keywordType, posStart, pos);
        }
        return new Token<string>(TokenType.IDENTIFIER, idStr, posStart, pos);
    }

    private (Token<TokenNoValueType>, Error) MakeNotEquals()
    {
        Position posStart = pos;
        Advance();

        if (current_char == '=')
        {
            return (
                new Token<TokenNoValueType>(TokenType.NE, startPos: posStart, endPos: pos),
                ErrorNull.Instance
            );
        }
        return (
            new Token<TokenNoValueType>(TokenType.NULL),
            new ExpectedCharError(posStart, "Expected '=' after '!'")
        );
    }

    private Token<TokenNoValueType> MakeDecicion(
        char checkChar,
        TokenType TTTrue,
        TokenType TTFalse
    )
    {
        Position posStart = pos;
        Advance();
        return new Token<TokenNoValueType>(
            current_char == checkChar ? TTTrue : TTFalse,
            posStart,
            pos
        );
    }

    private (Token<string>, Error) MakeString()
    {
        string value = string.Empty;
        Position startPos = pos;

        Dictionary<char, char> escapeChars = new()
        {
            { 'n', '\n' },
            { '\\', '\\' },
            { '"', '"' },
            { 't', '\t' },
        };

        Advance();
        while (current_char != char.MaxValue && current_char != '"')
        {
            if (current_char == '\\')
            {
                Advance();
                if (escapeChars.TryGetValue(current_char, out char _char))
                {
                    value += _char;
                }
                else
                {
                    return (
                        new Token<string>(TokenType.NULL),
                        new IllegalEscapeCharError(
                            startPos,
                            $"The character '{current_char}' is not a valide escape character"
                        )
                    );
                }
            }
            else
            {
                value += current_char;
            }

            Advance();
        }
        Advance();
        return (new Token<string>(TokenType.STRING, value, startPos, pos), ErrorNull.Instance);
    }

    private void SkipComment()
    {
        while (current_char is not '\n' and not '\r' and not ';' and not '#' and not char.MaxValue)
        {
            Advance();
        }
    }

    private void SkipTypeAnotation()
    {
        while (IsValidIdentifierChar(current_char) || current_char == ' ')
        {
            Advance();
        }
    }

    private Token<TokenNoValueType> MakePlus()
    {
        Position startPos = pos;
        Advance();

        if (current_char == '+')
        { // it is ++
            Advance();
            return new Token<TokenNoValueType>(TokenType.PP, startPos, pos);
        }
        if (current_char == '=')
        { // its +=
            Advance();
            return new Token<TokenNoValueType>(TokenType.PLEQ, startPos, pos);
        }
        return new Token<TokenNoValueType>(TokenType.PLUS, startPos, pos);
    }

    private Token<TokenNoValueType> MakeMinus()
    {
        Position startPos = pos;
        Advance();

        if (current_char == '-')
        { // it is --
            Advance();
            return new Token<TokenNoValueType>(TokenType.MM, startPos, pos);
        }
        if (current_char == '=')
        { // its -=
            Advance();
            return new Token<TokenNoValueType>(TokenType.MIEQ, startPos, pos);
        }
        if (current_char == '>')
        {
            Advance();
            return new Token<TokenNoValueType>(TokenType.ARROW, startPos, pos);
        }
        return new Token<TokenNoValueType>(TokenType.MINUS, startPos, pos);
    }

    // generates all tokens
    public (List<IToken>, Error) MakeTokens()
    {
        List<IToken> tokens = [];
        int parenthesesCount = 0; // to check if there are unclosed parentesies
        int squarBracketCount = 0; // to check if there are unclosed square brackets

        while (current_char != char.MaxValue)
        {
            if (current_char is ' ' or '\t')
            {
                // Discards spaces and tabs
                Advance();
            }
            else if (current_char == '#')
            {
                Advance();
                SkipComment();
            }
            else if (current_char == ':')
            {
                Advance();
                SkipTypeAnotation();
            }
            else if (current_char is ';' or '\n' or '\r')
            {
                tokens.Add(new Token<TokenNoValueType>(TokenType.NEWLINE, pos, pos));
                Advance();
            }
            else if (current_char == '.')
            {
                tokens.Add(new Token<TokenNoValueType>(TokenType.DOT, pos, pos));
                Advance();
            }
            else if (char.IsDigit(current_char)) // Check for digits (int)
            {
                (Token<double> tok, Error err) = MakeNumber();
                if (err.IsError)
                {
                    return ([], err);
                }
                tokens.Add(tok);
            }
            else if (char.IsLetter(current_char)) // Check for letters
            {
                tokens.Add(MakeIdentifier());
            }
            else if (current_char == '"')
            {
                (Token<string> tok, Error err) = MakeString();
                if (err.IsError)
                {
                    return ([], err);
                }
                tokens.Add(tok);
            }
            // Arithmetic
            else if (current_char == '+')
            {
                tokens.Add(MakePlus());
            }
            else if (current_char == '-')
            {
                tokens.Add(MakeMinus());
            }
            else if (current_char == '*')
            {
                tokens.Add(MakeDecicion('=', TokenType.MUEQ, TokenType.MUL));
                Advance();
            }
            else if (current_char == '/')
            {
                tokens.Add(MakeDecicion('=', TokenType.DIEQ, TokenType.DIV));
                Advance();
            }
            else if (current_char == '^')
            {
                tokens.Add(new Token<TokenNoValueType>(TokenType.POW, pos, pos));
                Advance();
            }
            else if (current_char == '(')
            {
                parenthesesCount++;
                tokens.Add(new Token<TokenNoValueType>(TokenType.LPAREN, pos, pos));
                Advance();
            }
            else if (current_char == ')')
            {
                parenthesesCount--;
                tokens.Add(new Token<TokenNoValueType>(TokenType.RPAREN, pos, pos));
                Advance();
            }
            // Comparison
            else if (current_char == '!')
            {
                (Token<TokenNoValueType>, Error) res = MakeNotEquals();
                if (res.Item2.IsError)
                {
                    return ([], res.Item2);
                }
                tokens.Add(res.Item1);
                Advance();
            }
            else if (current_char == '=')
            {
                tokens.Add(MakeDecicion('=', TokenType.EE, TokenType.EQ));
                Advance();
            }
            else if (current_char == '<')
            {
                tokens.Add(MakeDecicion('=', TokenType.LTE, TokenType.LT));
                Advance();
            }
            else if (current_char == '>')
            {
                tokens.Add(MakeDecicion('=', TokenType.GTE, TokenType.GT));
                Advance();
            }
            // Other
            else if (current_char == ',')
            {
                tokens.Add(new Token<TokenNoValueType>(TokenType.COMMA, pos, pos));
                Advance();
            }
            else if (current_char == '[')
            {
                squarBracketCount++;
                tokens.Add(new Token<TokenNoValueType>(TokenType.LSQUARE, pos, pos));
                Advance();
            }
            else if (current_char == ']')
            {
                squarBracketCount--;
                tokens.Add(new Token<TokenNoValueType>(TokenType.RSQUARE, pos, pos));
                Advance();
            }
            else
            {
                // Not a valid token, return an error
                return (
                    new List<IToken>(),
                    new IllegalCharError(pos, "Invalid char: " + current_char)
                );
            }
        }

        if (parenthesesCount != 0)
        {
            return (
                [],
                new UnclosedBracketsError(
                    Position.Null,
                    "There are more or less Open parentheses than closed ones"
                )
            );
        }
        if (squarBracketCount != 0)
        {
            return (
                [],
                new UnclosedBracketsError(
                    Position.Null,
                    "There are more or less Open square brackets than closed ones"
                )
            );
        }

        tokens.Add(new Token<TokenNoValueType>(TokenType.EOF, pos, pos)); // Add the End Of File token
        return (tokens, ErrorNull.Instance);
    }
}
