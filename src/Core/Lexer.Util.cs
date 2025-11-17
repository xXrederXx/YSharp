using System.Runtime.CompilerServices;
using System.Text;
using FastEnumUtility;
using YSharp.Types.Common;
using YSharp.Types.Lexer;
using YSharp.Utils;

namespace YSharp.Core;

public sealed partial class Lexer
{
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
        if (current_char == checkChar)
        {
            Advance();
            return new TokenNoValue(
                TTTrue,
                posStart,
                pos
            );
        }

        return new TokenNoValue(
            TTFalse,
            posStart,
            pos
        );
    }

    private (IToken, Error) MakeString()
    {
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
                    stringBuilder.Append(_char);
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
                stringBuilder.Append(current_char);
            }

            Advance();
        }
        Advance();
        string value = stringBuilder.ToString();
        stringBuilder.Clear();
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
        {
            Advance();
            return new TokenNoValue(TokenType.PP, startPos, pos);
        }
        if (current_char == '=')
        {
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

}