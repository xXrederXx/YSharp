using System.Runtime.CompilerServices;
using System.Text;
using FastEnumUtility;
using YSharp.Types.Common;
using YSharp.Types.Lexer;
using YSharp.Utils;

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
        byte FileId = FileNameRegistry.GetFileId(fileName);
        pos = new Position(0, 0, 0, FileId);
        current_char = pos.Index < text.Length ? text[pos.Index] : char.MaxValue;
    }

    // this uptades to the next charachter
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private void Advance()
    {
        pos = pos.Advance(current_char);
        current_char = pos.Index < text.Length ? text[pos.Index] : char.MaxValue;
    }

    private static bool IsValidIdentifierChar(char c) => char.IsLetterOrDigit(c) || c == '_';

    // this is used to make a number token of type int or float
    private (IToken, Error) MakeNumber()
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
                    return (NullToken.Instance, new IllegalNumberFormat(posStart));
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
        if (double.TryParse(stringBuilder.ToString(), out double value))
        {
            stringBuilder.Clear();
            return (new Token<double>(TokenType.FLOAT, value, posStart, pos), ErrorNull.Instance);
        }
        return (
            new Token<double>(TokenType.FLOAT, value, posStart, pos),
            new InternalLexerError(
                $"Couldnt convert Number to double -> Number ({stringBuilder.ToString()})"
            )
        );
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

    private (IToken, Error) MakeNotEquals()
    {
        Position posStart = pos;
        Advance();

        if (current_char == '=')
        {
            return (new TokenNoValue(TokenType.NE, posStart, pos), ErrorNull.Instance);
        }
        return (NullToken.Instance, new ExpectedCharError(posStart, '='));
    }

    private TokenNoValue MakeDecicion(
        char checkChar,
        TokenType TTTrue,
        TokenType TTFalse
    )
    {
        Position posStart = pos;
        Advance();
        return new TokenNoValue(
            current_char == checkChar ? TTTrue : TTFalse,
            posStart,
            pos
        );
    }

    private (IToken, Error) MakeString()
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
                        NullToken.Instance,
                        new IllegalEscapeCharError(startPos, current_char)
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

    private TokenNoValue MakePlus()
    {
        Position startPos = pos;
        Advance();

        if (current_char == '+')
        { // it is ++
            Advance();
            return new TokenNoValue(TokenType.PP, startPos, pos);
        }
        if (current_char == '=')
        { // its +=
            Advance();
            return new TokenNoValue(TokenType.PLEQ, startPos, pos);
        }
        return new TokenNoValue(TokenType.PLUS, startPos, pos);
    }

    private TokenNoValue MakeMinus()
    {
        Position startPos = pos;
        Advance();

        if (current_char == '-')
        { // it is --
            Advance();
            return new TokenNoValue(TokenType.MM, startPos, pos);
        }
        if (current_char == '=')
        { // its -=
            Advance();
            return new TokenNoValue(TokenType.MIEQ, startPos, pos);
        }
        if (current_char == '>')
        {
            Advance();
            return new TokenNoValue(TokenType.ARROW, startPos, pos);
        }
        return new TokenNoValue(TokenType.MINUS, startPos, pos);
    }

    // generates all tokens
    public (List<IToken>, Error) MakeTokens()
    {
        List<IToken> tokens = [];

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
                tokens.Add(new TokenNoValue(TokenType.NEWLINE, pos, pos));
                Advance();
            }
            else if (current_char == '.')
            {
                tokens.Add(new TokenNoValue(TokenType.DOT, pos, pos));
                Advance();
            }
            else if (char.IsDigit(current_char)) // Check for digits (int)
            {
                (IToken tok, Error err) = MakeNumber();
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
                (IToken tok, Error err) = MakeString();
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
                tokens.Add(new TokenNoValue(TokenType.POW, pos, pos));
                Advance();
            }
            else if (current_char == '(')
            {
                tokens.Add(new TokenNoValue(TokenType.LPAREN, pos, pos));
                Advance();
            }
            else if (current_char == ')')
            {
                tokens.Add(new TokenNoValue(TokenType.RPAREN, pos, pos));
                Advance();
            }
            // Comparison
            else if (current_char == '!')
            {
                (IToken, Error) res = MakeNotEquals();
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
                tokens.Add(new TokenNoValue(TokenType.COMMA, pos, pos));
                Advance();
            }
            else if (current_char == '[')
            {
                tokens.Add(new TokenNoValue(TokenType.LSQUARE, pos, pos));
                Advance();
            }
            else if (current_char == ']')
            {
                tokens.Add(new TokenNoValue(TokenType.RSQUARE, pos, pos));
                Advance();
            }
            else
            {
                // Not a valid token, return an error
                return (new List<IToken>(), new IllegalCharError(pos, current_char));
            }
        }
        tokens.Add(new TokenNoValue(TokenType.EOF, pos, pos)); // Add the End Of File token
        return (tokens, ErrorNull.Instance);
    }
}
