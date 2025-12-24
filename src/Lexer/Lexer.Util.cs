using FastEnumUtility;
using YSharp.Common;


namespace YSharp.Lexer;

public sealed partial class Lexer
{
    private BaseToken MakeDecision(
        char checkChar,
        TokenType TTTrue,
        TokenType TTFalse
    )
    {
        Position posStart = pos;
        Advance();
        if (currentChar == checkChar)
        {
            Advance();
            return new BaseToken(
                TTTrue,
                posStart,
                pos
            );
        }

        return new BaseToken(
            TTFalse,
            posStart,
            pos
        );
    }

    private BaseToken MakeIdentifier()
    {
        Position posStart = pos;

        while (IsValidIdentifierChar(currentChar))
        {
            stringBuilder.Append(currentChar);
            Advance();
        }

        string idStr = stringBuilder.ToString();
        bool IsKeyword = FastEnum.TryParse(idStr, out KeywordType keywordType);
        stringBuilder.Clear();

        if (IsKeyword) return new Token<KeywordType>(TokenType.KEYWORD, keywordType, posStart, pos);
        return new Token<string>(TokenType.IDENTIFIER, idStr, posStart, pos);
    }

    private BaseToken MakeMinus()
    {
        Position startPos = pos;
        Advance();

        if (currentChar == '-')
        {
            // it is --
            Advance();
            return new BaseToken(TokenType.MM, startPos, pos);
        }

        if (currentChar == '=')
        {
            // its -=
            Advance();
            return new BaseToken(TokenType.MIEQ, startPos, pos);
        }

        if (currentChar == '>')
        {
            Advance();
            return new BaseToken(TokenType.ARROW, startPos, pos);
        }

        return new BaseToken(TokenType.MINUS, startPos, pos);
    }

    private (BaseToken, Error) MakeNotEquals()
    {
        Position posStart = pos;
        Advance();

        if (currentChar == '=') return (new BaseToken(TokenType.NE, posStart, pos), ErrorNull.Instance);
        return (NullToken.Instance, new ExpectedCharError(posStart, '='));
    }

    // this is used to make a number token of type int or float
    private (BaseToken, Error) MakeNumber()
    {
        bool hasDot = false;
        Position posStart = pos;

        while (char.IsDigit(currentChar) || currentChar == '.' || currentChar == '_')
        {
            // while the char is a number or a dot
            if (currentChar == '_')
            {
                Advance();
                continue;
            }

            if (currentChar == '.')
            {
                if (hasDot) return (NullToken.Instance, new IllegalNumberFormat(posStart));

                hasDot = true;
                stringBuilder.Append('.');
            }
            else
            {
                // just a number
                stringBuilder.Append(currentChar);
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
                $"Couldnt convert Number to double -> Number ({stringBuilder})"
            )
        );
    }

    private BaseToken MakePlus()
    {
        Position startPos = pos;
        Advance();

        if (currentChar == '+')
        {
            Advance();
            return new BaseToken(TokenType.PP, startPos, pos);
        }

        if (currentChar == '=')
        {
            Advance();
            return new BaseToken(TokenType.PLEQ, startPos, pos);
        }

        return new BaseToken(TokenType.PLUS, startPos, pos);
    }

    private (BaseToken, Error) MakeString()
    {
        Position startPos = pos;

        Dictionary<char, char> escapeChars = new()
        {
            { 'n', '\n' },
            { '\\', '\\' },
            { '"', '"' },
            { 't', '\t' }
        };

        Advance();
        while (currentChar != char.MaxValue && currentChar != '"')
        {
            if (currentChar == '\\')
            {
                Advance();
                if (escapeChars.TryGetValue(currentChar, out char _char))
                    stringBuilder.Append(_char);
                else
                {
                    return (
                        NullToken.Instance,
                        new IllegalEscapeCharError(startPos, currentChar)
                    );
                }
            }
            else
                stringBuilder.Append(currentChar);

            Advance();
        }

        Advance();
        string value = stringBuilder.ToString();
        stringBuilder.Clear();
        return (new Token<string>(TokenType.STRING, value, startPos, pos), ErrorNull.Instance);
    }

    private void SkipComment()
    {
        while (currentChar is not '\n' and not '\r' and not ';' and not '#' and not char.MaxValue) Advance();
    }

    private void SkipTypeAnotation()
    {
        while (IsValidIdentifierChar(currentChar) || currentChar == ' ') Advance();
    }
}