using YSharp.Common;
using YSharp.Lexer;
using YSharp.Parser.Nodes;

namespace YSharp.Parser;

public sealed partial class Parser
{
    private ParseResult ArithExpr()
    {
        ParseResult res = new();

        BaseNode left = res.Register(Term());
        if (res.HasError)
            return res;

        while (currentToken.IsOneOf([TokenType.PLUS, TokenType.MINUS]))
        {
            BaseToken opTok = (BaseToken)currentToken;

            AdvanceParser(res);
            BaseNode right = res.Register(Term());
            if (res.HasError)
                return res;

            left = new BinOpNode(left, opTok, right);
        }

        return res.Success(left);
    }

    // Important Methods Start at bottom to top
    private ParseResult Atom()
    {
        ParseResult res = new();

        BaseToken tok = currentToken;

        // check tyoes
        if (tok.IsOneOf([TokenType.INT, TokenType.FLOAT]))
        {
            AdvanceParser(res);
            return res.Success(new NumberNode((Token<double>)tok));
        }

        if (tok.IsType(TokenType.STRING))
        {
            AdvanceParser(res);
            return res.Success(new StringNode((Token<string>)tok));
        }
        // check identifier

        if (tok.IsType(TokenType.IDENTIFIER))
        {
            BaseNode identifierExpr = res.Register(IdentifierExpr());
            if (res.HasError)
                return res;
            return res.Success(identifierExpr);
        }
        // check other symbols

        if (tok.IsType(TokenType.LPAREN))
        {
            AdvanceParser(res);
            BaseNode expr = res.Register(Expression());
            if (res.HasError)
                return res;

            if (currentToken.IsType(TokenType.RPAREN))
            {
                AdvanceParser(res);
                return res.Success(expr);
            }

            return res.Failure(new InvalidSyntaxError(currentToken.StartPos, "Expected ')'"));
        }

        if (tok.IsType(TokenType.LSQUARE))
        {
            BaseNode listExpression = res.Register(ListExpr());
            if (res.HasError)
                return res;
            return res.Success(listExpression);
        }
        // check keywords
        Token<KeywordType>? keywordTok = tok as Token<KeywordType>;
        if (keywordTok is not null)
        {
            if (keywordTok.ValueEquals(KeywordType.IF))
            {
                BaseNode ifExpression = res.Register(IfExpr());
                if (res.HasError)
                    return res;
                return res.Success(ifExpression);
            }

            if (keywordTok.ValueEquals(KeywordType.FOR))
            {
                BaseNode forExpression = res.Register(ForExpr());
                if (res.HasError)
                    return res;
                return res.Success(forExpression);
            }

            if (keywordTok.ValueEquals(KeywordType.WHILE))
            {
                BaseNode whileExpression = res.Register(WhileExpr());
                if (res.HasError)
                    return res;
                return res.Success(whileExpression);
            }

            if (keywordTok.ValueEquals(KeywordType.FUN))
            {
                BaseNode funcExpression = res.Register(FuncDef());
                if (res.HasError)
                    return res;
                return res.Success(funcExpression);
            }

            if (keywordTok.ValueEquals(KeywordType.TRY))
            {
                BaseNode tryCatch = res.Register(TryCatchExpr());
                if (res.HasError)
                    return res;
                return res.Success(tryCatch);
            }

            if (keywordTok.ValueEquals(KeywordType.IMPORT))
            {
                BaseNode importNode = res.Register(ImportExpr());
                if (res.HasError)
                    return res;
                return res.Success(importNode);
            }
        }

        return res.Failure(
            new InvalidSyntaxError(
                tok.StartPos,
                $"Expected anything else but current token is of type {tok.Type}"
            )
        );
    }

    private ParseResult CompExpr()
    {
        ParseResult res = new();

        if (IsCurrentTokenKeyword(KeywordType.NOT))
        {
            BaseToken opTok = currentToken;

            AdvanceParser(res);
            BaseNode node = res.Register(CompExpr());
            if (res.HasError)
                return res;
            return res.Success(new UnaryOpNode(opTok, node));
        }

        // Binary Operation
        BaseNode left = res.Register(ArithExpr());
        if (res.HasError)
            return res;

        // This checks for the comparison operators
        while (
            currentToken.IsOneOf([
                TokenType.EE,
                TokenType.NE,
                TokenType.GT,
                TokenType.LT,
                TokenType.GTE,
                TokenType.LTE,
            ])
        )
        {
            if (
                !TryCastTokenNoValue(
                    currentToken,
                    out BaseToken opTok,
                    out InternalTokenCastError<BaseToken> error
                )
            )
                return res.Failure(error);

            AdvanceParser(res);
            BaseNode right = res.Register(ArithExpr());
            if (res.HasError)
                return res;

            left = new BinOpNode(left, opTok, right);
        }

        return res.Success(left);
    }

    private ParseResult Expression()
    {
        ParseResult res = new();

        // A variable assignement
        if (IsCurrentTokenKeyword(KeywordType.VAR))
        {
            BaseNode node = res.Register(VarAssignExpr());
            if (res.HasError)
                return res;

            return res.Success(node);
        }

        // Binary Operation
        BaseNode left = res.Register(CompExpr());
        if (res.HasError)
            return res;

        while (IsCurrentTokenKeyword(KeywordType.AND) || IsCurrentTokenKeyword(KeywordType.OR))
        {
            BaseToken opTok = currentToken;

            AdvanceParser(res);
            BaseNode right = res.Register(CompExpr());
            if (res.HasError)
                return res;

            left = new BinOpNode(left, opTok, right);
        }

        return res.Success(left);
    }

    private ParseResult Factor()
    {
        ParseResult res = new();

        // if there is a plus or a minus it could be +5 or -5
        if (currentToken.IsOneOf([TokenType.PLUS, TokenType.MINUS]))
        {
            if (
                !TryCastTokenNoValue(
                    currentToken,
                    out BaseToken opTok,
                    out InternalTokenCastError<BaseToken> error
                )
            )
                return res.Failure(error);
            AdvanceParser(res);
            BaseNode factor = res.Register(Factor());
            if (res.HasError)
                return res;
            return res.Success(new UnaryOpNode(opTok, factor));
        }

        return Power();
    }

    private ParseResult ForExpr()
    {
        ParseResult res = new();

        if (IsCurrentTokenNotKeyword(KeywordType.FOR))
            return res.Failure(new ExpectedKeywordError(currentToken.StartPos, "FOR"));

        AdvanceParser(res);

        if (currentToken.IsNotType(TokenType.IDENTIFIER))
            return res.Failure(new ExpectedIdnetifierError(currentToken.StartPos));

        if (
            !TryCastToken(
                currentToken,
                out Token<string> varName,
                out InternalTokenCastError<string> error
            )
        )
            return res.Failure(error);

        AdvanceParser(res);

        if (currentToken.IsNotType(TokenType.EQ))
            return res.Failure(new ExpectedTokenError(currentToken.StartPos, "'='"));

        AdvanceParser(res);

        BaseNode startValue = res.Register(Expression());
        if (res.HasError)
            return res;

        if (IsCurrentTokenNotKeyword(KeywordType.TO))
            return res.Failure(new ExpectedKeywordError(currentToken.StartPos, "TO"));

        AdvanceParser(res);
        BaseNode endValue = res.Register(Expression());
        if (res.HasError)
            return res;

        BaseNode? stepValue = null;
        if (IsCurrentTokenKeyword(KeywordType.STEP))
        {
            AdvanceParser(res);

            stepValue = res.Register(Expression());
            if (res.HasError)
                return res;
        }

        if (IsCurrentTokenNotKeyword(KeywordType.THEN))
            return res.Failure(new ExpectedKeywordError(currentToken.StartPos, "THEN"));

        AdvanceParser(res);

        BaseNode body = GetBodyNode(res);
        if (HasErrorButEnd(res))
            return res;
        return res.Success(new ForNode(varName, startValue, endValue, stepValue, body, false));
    }

    private ParseResult FuncDef()
    {
        ParseResult res = new();
        if (IsCurrentTokenNotKeyword(KeywordType.FUN))
            return res.Failure(new ExpectedKeywordError(currentToken.StartPos, "FUN"));

        AdvanceParser(res);

        Token<string> varNameTok;
        if (currentToken.IsType(TokenType.IDENTIFIER))
        {
            if (
                !TryCastToken(
                    currentToken,
                    out Token<string> _varNameTok,
                    out InternalTokenCastError<string> error
                )
            )
                return res.Failure(error);
            varNameTok = _varNameTok;
            AdvanceParser(res);
            if (currentToken.IsNotType(TokenType.LPAREN))
                return res.Failure(new ExpectedTokenError(currentToken.StartPos, "'('"));
        }
        else
        {
            varNameTok = new Token<string>(
                TokenType.NULL,
                "",
                currentToken.StartPos,
                currentToken.EndPos
            );
            if (currentToken.IsNotType(TokenType.LPAREN))
            {
                return res.Failure(
                    new InvalidSyntaxError(currentToken.StartPos, "Expected an identifier or '('")
                );
            }
        }

        AdvanceParser(res);
        List<BaseToken> argNameTok = [];

        if (currentToken.IsType(TokenType.IDENTIFIER))
        {
            // has args
            argNameTok.Add(currentToken);
            AdvanceParser(res);

            while (currentToken.IsType(TokenType.COMMA))
            {
                AdvanceParser(res);

                if (currentToken.IsNotType(TokenType.IDENTIFIER))
                    return res.Failure(new ExpectedIdnetifierError(currentToken.StartPos));
                argNameTok.Add(currentToken);
                AdvanceParser(res);
            }

            if (currentToken.IsNotType(TokenType.RPAREN))
            {
                return res.Failure(
                    new InvalidSyntaxError(currentToken.StartPos, "Expected ',' or ')'")
                );
            }
        }
        else if (currentToken.IsNotType(TokenType.RPAREN))
        {
            return res.Failure(
                new InvalidSyntaxError(currentToken.StartPos, "Expected an identifier or ')'")
            );
        }

        AdvanceParser(res);

        if (currentToken.IsType(TokenType.ARROW))
        {
            AdvanceParser(res);

            BaseNode nodeToReturn = res.Register(Expression());
            if (res.HasError)
                return res;
            return res.Success(new FuncDefNode(varNameTok, argNameTok, nodeToReturn, true));
        }

        if (currentToken.IsNotType(TokenType.NEWLINE))
        {
            return res.Failure(
                new InvalidSyntaxError(currentToken.StartPos, "Expected '->' or newline")
            );
        }

        AdvanceParser(res);
        BaseNode body = res.Register(Statements());
        if (HasErrorButEnd(res))
            return res;
        return res.Success(new FuncDefNode(varNameTok, argNameTok, body, false));
    }

    private ParseResult IdentifierExpr()
    {
        ParseResult res = new();
        BaseNode parent = NodeNull.Instance;

        if (
            !TryCastToken(
                currentToken,
                out Token<string> identifierTok,
                out InternalTokenCastError<string> error
            )
        )
            return res.Failure(error);
        AdvanceParser(res);

        if (currentToken.IsType(TokenType.LPAREN))
        {
            List<BaseNode> args = MakeArgs(res);
            if (res.HasError)
                return res;
            parent = new CallNode(new VarAccessNode(identifierTok), args);
        }
        else
            parent = new VarAccessNode(identifierTok);

        while (currentToken.IsType(TokenType.DOT))
        {
            AdvanceParser(res);
            if (
                !TryCastToken(
                    currentToken,
                    out Token<string> dotIdentifierTok,
                    out InternalTokenCastError<string> dotError
                )
            )
                return res.Failure(dotError);
            AdvanceParser(res);

            if (currentToken.IsType(TokenType.LPAREN))
            {
                List<BaseNode> args = MakeArgs(res);
                if (res.HasError)
                    return res;
                parent = new DotCallNode(dotIdentifierTok, args, parent);
            }
            else
                parent = new DotVarAccessNode(dotIdentifierTok, parent);
        }

        return res.Success(parent);
    }

    private ParseResult IfExpr()
    {
        ParseResult res = new();
        List<SubIfNode> cases = [];
        BaseNode elseCase = NodeNull.Instance;

        if (IsCurrentTokenNotKeyword(KeywordType.IF))
            return res.Failure(new ExpectedKeywordError(currentToken.StartPos, "IF"));
        do
        {
            AdvanceParser(res);

            BaseNode caseCondition = res.Register(Statement());
            if (res.HasError)
                return res;

            if (IsCurrentTokenNotKeyword(KeywordType.THEN))
                return res.Failure(new ExpectedKeywordError(currentToken.StartPos, "THEN"));

            AdvanceParser(res);

            if (currentToken.IsNotType(TokenType.NEWLINE))
                return res.Failure(new ExpectedNewlineError(currentToken.StartPos));

            AdvanceParser(res);

            BaseNode caseBodyNode = res.Register(Statements());
            if (res.HasError && res.Error is not EndKeywordError)
                return res;
            res.ResetError();

            cases.Add(new SubIfNode(caseCondition, caseBodyNode));

            if (IsCurrentTokenNotKeyword(KeywordType.END))
                return res.Failure(new ExpectedKeywordError(currentToken.StartPos, "END"));
            AdvanceParser(res);
            SkipNewLines(res);
        } while (IsCurrentTokenKeyword(KeywordType.ELIF));

        if (IsCurrentTokenNotKeyword(KeywordType.ELSE))
            return res.Success(new IfNode(cases, elseCase));

        AdvanceParser(res);

        if (currentToken.IsNotType(TokenType.NEWLINE))
            return res.Failure(new ExpectedNewlineError(currentToken.StartPos));

        AdvanceParser(res);

        elseCase = res.Register(Statements());
        if (HasErrorButEnd(res))
            return res;

        return res.Success(new IfNode(cases, elseCase));
    }

    private ParseResult ImportExpr()
    {
        ParseResult res = new();
        Position startPos = currentToken.StartPos;
        if (IsCurrentTokenNotKeyword(KeywordType.IMPORT))
            return res.Failure(new ExpectedKeywordError(currentToken.StartPos, "IMPORT"));

        AdvanceParser(res);

        if (currentToken.IsNotType(TokenType.STRING))
        {
            return res.Failure(
                new InvalidSyntaxError(
                    currentToken.StartPos,
                    "Expected a string (Path to dll) after IMPORT"
                )
            );
        }

        if (
            !TryCastToken(
                currentToken,
                out Token<string> token,
                out InternalTokenCastError<string> error
            )
        )
            return res.Failure(error);
        return res.Success(new ImportNode(token, startPos, currentToken.EndPos));
    }

    private ParseResult ListExpr()
    {
        ParseResult res = new();
        List<BaseNode> elementNodes = [];
        Position StartPos = currentToken.StartPos;

        if (currentToken.IsNotType(TokenType.LSQUARE))
            return res.Failure(new ExpectedTokenError(StartPos, "'['"));

        AdvanceParser(res);

        if (currentToken.IsType(TokenType.RSQUARE))
            AdvanceParser(res);
        else
        {
            elementNodes.Add(res.Register(Expression()));
            if (res.HasError)
                return res;

            while (currentToken.IsType(TokenType.COMMA))
            {
                AdvanceParser(res);

                elementNodes.Add(res.Register(Expression()));
                if (res.HasError)
                    return res;
            }

            if (currentToken.IsNotType(TokenType.RSQUARE))
                return res.Failure(new UnmatchedBracketError(currentToken.StartPos, ']'));

            AdvanceParser(res);
        }

        return res.Success(new ListNode(elementNodes, StartPos, currentToken.EndPos));
    }

    private ParseResult Power()
    {
        ParseResult res = new();

        BaseNode left = res.Register(Atom());
        if (res.HasError)
            return res;

        while (currentToken.IsType(TokenType.POW))
        {
            if (
                !TryCastTokenNoValue(
                    currentToken,
                    out BaseToken opTok,
                    out InternalTokenCastError<BaseToken> error
                )
            )
                return res.Failure(error);
            AdvanceParser(res);

            BaseNode right = res.Register(Factor());
            if (res.HasError)
                return res;

            left = new BinOpNode(left, opTok, right);
        }

        return res.Success(left);
    }

    private ParseResult Statement()
    {
        ParseResult res = new();
        Position startPos = currentToken.StartPos;

        if (IsCurrentTokenKeyword(KeywordType.RETURN))
        {
            AdvanceParser(res);
            BaseNode? _expr = res.TryRegister(Expression());
            if (_expr == null)
                Reverse(res.ToReverseCount);
            return res.Success(new ReturnNode(_expr, startPos, currentToken.StartPos));
        }

        if (IsCurrentTokenKeyword(KeywordType.CONTINUE))
        {
            AdvanceParser(res);
            return res.Success(new ContinueNode(startPos, currentToken.StartPos));
        }

        if (IsCurrentTokenKeyword(KeywordType.BREAK))
        {
            AdvanceParser(res);
            return res.Success(new BreakNode(startPos, currentToken.StartPos));
        }

        if (IsCurrentTokenKeyword(KeywordType.END))
            return res.Failure(new EndKeywordError(currentToken.StartPos));

        BaseNode expr = res.Register(Expression());

        if (res.HasError)
            return res;

        return res.Success(expr);
    }

    private ParseResult Statements()
    {
        ParseResult res = new();

        Position StartPos = currentToken.StartPos;
        List<BaseNode> AllStatements = [];
        BaseNode nextStatement;

        while (currentToken.IsNotType(TokenType.EOF)) // repeat until no more lines are available
        {
            SkipNewLines(res);
            if (currentToken.IsType(TokenType.EOF))
                break;

            nextStatement = res.Register(Statement());
            if (res.HasError)
            {
                if (res.Error is EndKeywordError)
                    return res.Success(new ListNode(AllStatements, StartPos, currentToken.EndPos));
                return res;
            }

            AllStatements.Add(nextStatement);
        }

        return res.Success(new ListNode(AllStatements, StartPos, currentToken.EndPos));
    }

    private ParseResult Term()
    {
        // will return BinOpNode
        ParseResult res = new();

        BaseNode left = res.Register(Factor());
        if (res.HasError)
            return res;

        while (currentToken.IsOneOf([TokenType.MUL, TokenType.DIV]))
        {
            if (
                !TryCastTokenNoValue(
                    currentToken,
                    out BaseToken opTok,
                    out InternalTokenCastError<BaseToken> error
                )
            )
                return res.Failure(error);

            AdvanceParser(res);
            BaseNode right = res.Register(Factor());
            if (res.HasError)
                return res;

            left = new BinOpNode(left, opTok, right);
        }

        return res.Success(left);
    }

    private ParseResult TryCatchExpr()
    {
        ParseResult res = new();

        if (IsCurrentTokenNotKeyword(KeywordType.TRY))
            return res.Failure(new ExpectedKeywordError(currentToken.StartPos, "TRY"));
        AdvanceParser(res);

        BaseNode tryBlock = res.Register(Statements());
        BaseNode catchBlock = NodeNull.Instance;
        Token<string> varName = new(TokenType.NULL, "", currentToken.StartPos, currentToken.EndPos);
        if (HasErrorButEnd(res))
            return res;

        SkipNewLines(res);

        if (IsCurrentTokenKeyword(KeywordType.CATCH))
        {
            AdvanceParser(res);

            if (currentToken.IsType(TokenType.IDENTIFIER))
            {
                varName = (Token<string>)currentToken;
                AdvanceParser(res);
            }

            catchBlock = res.Register(Statements());
            if (HasErrorButEnd(res))
                return res;
        }

        return res.Success(new TryCatchNode(tryBlock, catchBlock, varName));
    }

    private ParseResult VarAssignExpr()
    {
        ParseResult res = new();

        if (IsCurrentTokenNotKeyword(KeywordType.VAR))
            return res.Failure(new ExpectedKeywordError(currentToken.StartPos, "VAR"));

        AdvanceParser(res);
        if (currentToken.IsNotType(TokenType.IDENTIFIER))
        {
            return res.Failure(
                new InvalidSyntaxError(
                    currentToken.StartPos,
                    "Expected an Identifier after the VAR Keyword"
                )
            );
        }

        if (
            !TryCastToken(
                currentToken,
                out Token<string> varName,
                out InternalTokenCastError<string> error
            )
        )
            return res.Failure(error);

        AdvanceParser(res);
        // look if it is a shorthand written way else use null
        TokenType? EqType = currentToken.Type switch
        {
            TokenType.PLEQ => TokenType.PLUS,
            TokenType.MIEQ => TokenType.MINUS,
            TokenType.MUEQ => TokenType.MUL,
            TokenType.DIEQ => TokenType.DIV,
            _ => null,
        };
        if (EqType is not null)
        {
            BaseNode converted = res.Register(ShortendVarAssignHelper(varName, (TokenType)EqType));
            if (res.HasError)
                return res;
            return res.Success(new VarAssignNode(varName, converted));
        }

        // look if it is increment or decrement
        if (currentToken.IsType(TokenType.PP))
        {
            AdvanceParser(res);
            return res.Success(new SuffixAssignNode(varName, true));
        }

        if (currentToken.IsType(TokenType.MM))
        {
            AdvanceParser(res);
            return res.Success(new SuffixAssignNode(varName, false));
        }

        // normal variable assign
        if (currentToken.IsNotType(TokenType.EQ))
        {
            return res.Failure(
                new InvalidSyntaxError(
                    currentToken.StartPos,
                    "Expected an '=' after the variable name"
                )
            );
        }

        AdvanceParser(res);
        BaseNode expr = res.Register(Expression()); // this gets the "value" of the variable
        if (res.HasError)
            return res;

        return res.Success(new VarAssignNode(varName, expr));
    }

    private ParseResult WhileExpr()
    {
        ParseResult res = new();

        if (IsCurrentTokenNotKeyword(KeywordType.WHILE))
            return res.Failure(new ExpectedKeywordError(currentToken.StartPos, "WHILE"));
        AdvanceParser(res);

        BaseNode condition = res.Register(Expression());
        if (res.HasError)
            return res;

        if (IsCurrentTokenNotKeyword(KeywordType.THEN))
            return res.Failure(new ExpectedKeywordError(currentToken.StartPos, "THEN"));

        AdvanceParser(res);
        BaseNode body = GetBodyNode(res);
        if (HasErrorButEnd(res))
            return res;

        return res.Success(new WhileNode(condition, body, true));
    }
}
