using System;
using System.Runtime.CompilerServices;
using YSharp.Types;
using YSharp.Types.InternalTypes;
using YSharp.Utility;

namespace YSharp.Internal;

public partial class Parser
{
    public INode GetBodyNode(ParseResult res)
    {
        if (currentToken.IsType(TokenType.NEWLINE))
        {
            AdvanceParser(res);
            return res.Register(Statements());
        }
        else
        {
            return res.Register(Statement());
        }
    }

    public bool HasErrorButEnd(ParseResult res)
    {
        if (res.HasError && res.Error is not EndKeywordError)
        {
            return true;
        }
        res.ResetError();

        if (currentToken.IsNotMatchingKeyword(KeywordType.END))
        {
            res.Failure(new ExpectedKeywordError(currentToken.StartPos, "END"));
            return true;
        }
        AdvanceParser(res);
        return false;
    }

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
}
