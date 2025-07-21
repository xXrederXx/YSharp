using System;
using System.Runtime.CompilerServices;
using YSharp.Internal;
using YSharp.Types.InternalTypes;

namespace YSharp.Utility;

public static class ParserUtil
{
    public static bool TryCastToken<T>(
        IToken token,
        out Token<T> result,
        out InternalError error,
        [CallerMemberName] string membername = "NoMemberName"
    )
    {
        if (token is Token<T> casted)
        {
            result = casted;
            error = null!;

            return true;
        }

        result = null!;
        error = new InternalError(
            $"Casting the token ({token.GetType()}) to a Token<{typeof(T)}> failed in {membername} / Token: {token}"
        );

        return false;
    }

    public static bool HasErrorButEnd(ParseResult res, Parser parser)
    {
        if (res.HasError && res.Error is not EndKeywordError)
        {
            return true;
        }
        res.ResetError();

        if (parser.currentToken.IsNotMatching(TokenType.KEYWORD, KeywordType.END))
        {
            res.Failure(new ExpectedKeywordError(parser.currentToken.StartPos, "END"));
            return true;
        }
        parser.AdvanceParser(res);
        return false;
    }
}
