using FastEnumUtility;
using YSharp.Common;
using YSharp.Util;

namespace YSharp.Lexer;

using LexerOperationResult = Result<BaseToken, Error>;

public sealed partial class Lexer
{
    private static readonly Dictionary<char, char> escapeChars = new()
    {
        { 'n', '\n' },
        { '\\', '\\' },
        { '"', '"' },
        { 't', '\t' },
    };

    private BaseToken MakeDecision(char checkChar, TokenType TTTrue, TokenType TTFalse)
    {
        Position startPos = pos;
        Advance();
        if (currentChar == checkChar)
        {
            Advance();
            return new BaseToken(TTTrue, startPos, pos);
        }

        return new BaseToken(TTFalse, startPos, pos);
    }

    private BaseToken MakeIdentifier()
    {
        Position startPos = pos;

        while (IsValidIdentifierChar(currentChar))
        {
            stringBuilder.Append(currentChar);
            Advance();
        }

        string idStr = stringBuilder.GetAndClear();
        bool IsKeyword = FastEnum.TryParse(idStr, out KeywordType keywordType);

        return IsKeyword
            ? new Token<KeywordType>(TokenType.KEYWORD, keywordType, startPos, pos)
            : new Token<string>(TokenType.IDENTIFIER, idStr, startPos, pos);
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

    private LexerOperationResult MakeNotEquals()
    {
        Position startPos = pos;
        Advance();
        if (currentChar == '=')
        {
            Position endPos = pos;
            Advance();
            return LexerOperationResult.Success(new BaseToken(TokenType.NE, startPos, endPos));
        }
        return LexerOperationResult.Fail(new ExpectedCharError(startPos, '='));
    }

    // this is used to make a number token of type int or float
    private LexerOperationResult MakeNumber()
    {
        bool hasDot = false;
        Position startPos = pos;

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
                if (hasDot)
                    return LexerOperationResult.Fail(new IllegalNumberFormat(startPos));

                hasDot = true;
            }
            stringBuilder.Append(currentChar);

            Advance();
        }

        if (
            double.TryParse(
                stringBuilder.GetAndClear(),
                StaticConfig.numberCulture,
                out double value
            )
        )
        {
            return LexerOperationResult.Success(
                new Token<double>(TokenType.NUMBER, value, startPos, pos)
            );
        }

        return LexerOperationResult.Fail(
            new InternalLexerError($"Couldnt convert Number to double -> Number ({stringBuilder})")
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

    private LexerOperationResult MakeString()
    {
        Position startPos = pos;

        Advance();
        while (currentChar != StopChar && currentChar != '"')
        {
            if (currentChar == '\\')
            {
                Advance();
                if (!escapeChars.TryGetValue(currentChar, out char escapedChar))
                {
                    return LexerOperationResult.Fail(new IllegalEscapeCharError(pos, currentChar));
                }
                stringBuilder.Append(escapedChar);
            }
            else
            {
                stringBuilder.Append(currentChar);
            }

            Advance();
        }

        Advance();
        return LexerOperationResult.Success(
            new Token<string>(TokenType.STRING, stringBuilder.GetAndClear(), startPos, pos)
        );
    }

    private void SkipComment()
    {
        while (currentChar is not '\n' and not '\r' and not ';' and not '#' and not StopChar)
        {
            Advance();
        }
    }

    private void SkipTypeAnotation()
    {
        while (IsValidIdentifierChar(currentChar) || currentChar == ' ')
        {
            Advance();
        }
    }
}
