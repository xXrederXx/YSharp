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

    private List<INode> MakeArgs(ParseResult res)
    {

        if (currentToken.IsNotType(TokenType.LPAREN))
        {
            res.Failure(new ExpectedTokenError(currentToken.StartPos, "expected a '('"));
            return [];
        }

        AdvanceParser(res);
        if (currentToken.IsType(TokenType.RPAREN))
        {
            AdvanceParser(res);
            return []; // empty node just for the parseresult.succses
        }

        // get argument
        List<INode> argNodes = [];
        argNodes.Add(res.Register(Expression()));
        if (res.HasError)
        {
            return [];
        }

        // get the rest of the arguments which are seperated by commas
        while (currentToken.IsType(TokenType.COMMA))
        {
            AdvanceParser(res);

            argNodes.Add(res.Register(Expression()));
            if (res.HasError)
            {
                return [];
            }
        }

        if (currentToken.IsNotType(TokenType.RPAREN))
        {
            res.Failure(new ExpectedTokenError(currentToken.StartPos, "expected a ')' or a ','"));
            return [];
        }

        AdvanceParser(res);

        return argNodes; // empty node just for the parseresult.succses
    }
}
