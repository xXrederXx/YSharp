using YSharp.Types;
using YSharp.Types.InternalTypes;

namespace YSharp.Internal;

public partial class Parser
{
    private ParseResult IfExpr()
    {
        ParseResult res = new();
        List<SubIfNode> cases = [];
        INode elseCase = NodeNull.Instance;

        if (currentToken.IsNotMatchingKeyword(KeywordType.IF))
        {
            return res.Failure(new ExpectedKeywordError(currentToken.StartPos, $"Expected IF"));
        }
        // currentToken.IsMatching(TokenType.KEYWORD, KeywordType.IF) is replaced with first run to avoid multiple ifs instead of elifs
        bool firstRun = true;
        while (firstRun || currentToken.IsMatchingKeyword(KeywordType.ELIF))
        {
            firstRun = false;
            AdvanceParser(res);

            INode caseCondition = res.Register(Statement());
            if (res.HasError)
            {
                return res;
            }

            if (currentToken.IsNotMatchingKeyword(KeywordType.THEN))
            {
                return res.Failure(new ExpectedKeywordError(currentToken.StartPos, $"THEN"));
            }

            AdvanceParser(res);

            if (currentToken.IsNotType(TokenType.NEWLINE))
            {
                return res.Failure(
                    new InvalidSyntaxError(currentToken.StartPos, "Newline expected")
                );
            }

            AdvanceParser(res);

            INode caseBodyNode = res.Register(Statements());
            if (res.HasError && res.Error is not EndKeywordError)
            {
                return res;
            }
            res.ResetError();

            cases.Add(new SubIfNode(caseCondition, caseBodyNode));

            if (currentToken.IsMatchingKeyword(KeywordType.END))
            {
                AdvanceParser(res);
                return res.Success(new IfNode(cases, elseCase));
            }
        }

        if (currentToken.IsNotMatchingKeyword(KeywordType.ELSE))
        {
            return res.Failure(new ExpectedKeywordError(currentToken.StartPos, "END or ELSE"));
        }

        AdvanceParser(res);

        if (currentToken.IsNotType(TokenType.NEWLINE))
        {
            return res.Failure(new InvalidSyntaxError(currentToken.StartPos, "Newline expected"));
        }

        AdvanceParser(res);

        elseCase = res.Register(Statements());
        if (HasErrorButEnd(res))
        {
            return res;
        }

        return res.Success(new IfNode(cases, elseCase));
    }

    private ParseResult ForExpr()
    {
        ParseResult res = new();

        if (currentToken.IsNotMatchingKeyword(KeywordType.FOR))
        {
            return res.Failure(new ExpectedKeywordError(currentToken.StartPos, "FOR"));
        }

        AdvanceParser(res);

        if (currentToken.IsNotType(TokenType.IDENTIFIER))
        {
            return res.Failure(
                new ExpectedTokenError(currentToken.StartPos, "Expected identifier")
            );
        }

        if (!TryCastToken(currentToken, out Token<string> varName, out InternalError error))
        {
            return res.Failure(error);
        }

        AdvanceParser(res);

        if (currentToken.IsNotType(TokenType.EQ))
        {
            return res.Failure(new ExpectedTokenError(currentToken.StartPos, "Expected '='"));
        }

        AdvanceParser(res);

        INode startValue = res.Register(Expression());
        if (res.HasError)
        {
            return res;
        }

        if (currentToken.IsNotMatchingKeyword(KeywordType.TO))
        {
            return res.Failure(new ExpectedKeywordError(currentToken.StartPos, "Expected TO"));
        }

        AdvanceParser(res);
        INode endValue = res.Register(Expression());
        if (res.HasError)
        {
            return res;
        }

        INode? stepValue = null;
        if (currentToken.IsMatchingKeyword(KeywordType.STEP))
        {
            AdvanceParser(res);

            stepValue = res.Register(Expression());
            if (res.HasError)
            {
                return res;
            }
        }

        if (currentToken.IsNotMatchingKeyword(KeywordType.THEN))
        {
            return res.Failure(new ExpectedKeywordError(currentToken.StartPos, "Expected THEN"));
        }

        AdvanceParser(res);

        INode body = GetBodyNode(res);
        if (HasErrorButEnd(res))
        {
            return res;
        }
        return res.Success(new ForNode(varName, startValue, endValue, stepValue, body, false));
    }

    private ParseResult WhileExpr()
    {
        ParseResult res = new();

        if (currentToken.IsNotMatchingKeyword(KeywordType.WHILE))
        {
            return res.Failure(new ExpectedKeywordError(currentToken.StartPos, "WHILE"));
        }
        AdvanceParser(res);

        INode condition = res.Register(Expression());
        if (res.HasError)
        {
            return res;
        }

        if (currentToken.IsNotMatchingKeyword(KeywordType.THEN))
        {
            return res.Failure(new ExpectedKeywordError(currentToken.StartPos, "THEN"));
        }

        AdvanceParser(res);
        INode body = GetBodyNode(res);
        if (HasErrorButEnd(res))
        {
            return res;
        }

        return res.Success(new WhileNode(condition, body, true));
    }

    private ParseResult ListExpr()
    {
        ParseResult res = new();
        List<INode> elementNodes = [];
        Position StartPos = currentToken.StartPos;

        if (currentToken.IsNotType(TokenType.LSQUARE))
        {
            return res.Failure(new ExpectedTokenError(StartPos, "Expected '['"));
        }

        AdvanceParser(res);

        if (currentToken.IsType(TokenType.RSQUARE))
        {
            AdvanceParser(res);
        }
        else
        {
            elementNodes.Add(res.Register(Expression()));
            if (res.HasError)
            {
                return res;
            }

            while (currentToken.IsType(TokenType.COMMA))
            {
                AdvanceParser(res);

                elementNodes.Add(res.Register(Expression()));
                if (res.HasError)
                {
                    return res;
                }
            }

            if (currentToken.IsNotType(TokenType.RSQUARE))
            {
                return res.Failure(
                    new ExpectedTokenError(currentToken.StartPos, "expected ',' or ']'")
                );
            }

            AdvanceParser(res);
        }
        return res.Success(new ListNode(elementNodes, StartPos, currentToken.EndPos));
    }

    private ParseResult IdentifierExpr()
    {
        ParseResult res = new();
        if (!TryCastToken(currentToken, out Token<string> tok, out InternalError error))
        {
            return res.Failure(error);
        }

        AdvanceParser(res);
        if (currentToken.IsNotType(TokenType.DOT)) // normal identifier
        {
            return res.Success(new VarAccessNode(tok));
        }

        // there is a dot notaited identifier
        AdvanceParser(res);
        if (currentToken.IsNotType(TokenType.IDENTIFIER))
        {
            return res.Failure(
                new ExpectedTokenError(currentToken.StartPos, "Expected IDENTIFIER")
            );
        }

        if (!TryCastToken(currentToken, out Token<string> varName, out InternalError errorNameCast))
        {
            return res.Failure(errorNameCast);
        }

        AdvanceParser(res);
        if (currentToken.IsNotType(TokenType.LPAREN)) // it is a variable
        {
            return res.Success(new DotVarAccessNode(varName, new VarAccessNode(tok)));
        }

        // it is a function
        List<INode> args = MakeArgs(res);
        if (res.HasError)
        {
            return res;
        }
        return res.Success(new DotCallNode(varName, args, new VarAccessNode(tok)));
    }

    private ParseResult FuncDef()
    {
        ParseResult res = new();
        if (currentToken.IsNotMatchingKeyword(KeywordType.FUN))
        {
            return res.Failure(new ExpectedKeywordError(currentToken.StartPos, "FUN"));
        }

        AdvanceParser(res);

        Token<string> varNameTok;
        if (currentToken.IsType(TokenType.IDENTIFIER))
        {
            if (!TryCastToken(currentToken, out Token<string> _varNameTok, out InternalError error))
            {
                return res.Failure(error);
            }
            varNameTok = _varNameTok;
            AdvanceParser(res);
            if (currentToken.IsNotType(TokenType.LPAREN))
            {
                return res.Failure(new ExpectedTokenError(currentToken.StartPos, "expected '('"));
            }
        }
        else
        {
            varNameTok = new Token<string>(TokenType.NULL);
            if (currentToken.IsNotType(TokenType.LPAREN))
            {
                return res.Failure(
                    new ExpectedTokenError(currentToken.StartPos, "expected identifier or '('")
                );
            }
        }

        AdvanceParser(res);
        List<IToken> argNameTok = [];

        if (currentToken.IsType(TokenType.IDENTIFIER))
        { // has args
            argNameTok.Add(currentToken);
            AdvanceParser(res);

            while (currentToken.IsType(TokenType.COMMA))
            {
                AdvanceParser(res);

                if (currentToken.IsNotType(TokenType.IDENTIFIER))
                {
                    return res.Failure(
                        new ExpectedTokenError(currentToken.StartPos, "expected identifier")
                    );
                }
                argNameTok.Add(currentToken);
                AdvanceParser(res);
            }

            if (currentToken.IsNotType(TokenType.RPAREN))
            {
                return res.Failure(
                    new ExpectedTokenError(currentToken.StartPos, "expected ',' or ')'")
                );
            }
        }
        else if (currentToken.IsNotType(TokenType.RPAREN))
        {
            return res.Failure(
                new ExpectedTokenError(currentToken.StartPos, "expected identifier or ')'")
            );
        }

        AdvanceParser(res);

        if (currentToken.IsType(TokenType.ARROW))
        {
            AdvanceParser(res);

            INode nodeToReturn = res.Register(Expression());
            if (res.HasError)
            {
                return res;
            }
            return res.Success(new FuncDefNode(varNameTok, argNameTok, nodeToReturn, true));
        }

        if (currentToken.IsNotType(TokenType.NEWLINE))
        {
            return res.Failure(
                new ExpectedTokenError(currentToken.StartPos, "expected '->' or newline")
            );
        }

        AdvanceParser(res);
        INode body = res.Register(Statements());
        if (HasErrorButEnd(res))
        {
            return res;
        }
        return res.Success(new FuncDefNode(varNameTok, argNameTok, body, false));
    }

    private ParseResult ShortendVarAssignHelper(Token<string> varName, TokenType type)
    {
        ParseResult res = new();

        AdvanceParser(res);
        INode expr = res.Register(Expression()); // this gets the "value" of the variable
        if (res.HasError)
        {
            return res;
        }

        // This converts varName += Expr to varName = varName + Expr
        INode converted = new BinOpNode(
            new VarAccessNode(varName),
            new Token<TokenNoValueType>(type, expr.StartPos, expr.StartPos),
            expr
        );
        return res.Success(new VarAssignNode(varName, converted));
    }

    private ParseResult VarAssignExpr()
    {
        ParseResult res = new();

        if (currentToken.IsNotMatchingKeyword(KeywordType.VAR))
        {
            return res.Failure(new ExpectedKeywordError(currentToken.StartPos, "VAR"));
        }

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
        if (currentToken is not Token<string> varName)
        {
            return res.Failure(
                new InternalError(
                    "Trying to cast the current token to a string token failed in VarAssignExpr"
                )
            );
        }

        AdvanceParser(res);
        // look if it is a shorthand written way else use null
        TokenType? EqType = currentToken.Type switch
        {
            TokenType.PLEQ => (TokenType?)TokenType.PLUS,
            TokenType.MIEQ => (TokenType?)TokenType.MINUS,
            TokenType.MUEQ => (TokenType?)TokenType.MUL,
            TokenType.DIEQ => (TokenType?)TokenType.DIV,
            _ => null,
        };
        if (EqType is not null)
        {
            INode converted = res.Register(ShortendVarAssignHelper(varName, (TokenType)EqType));
            if (res.HasError)
            {
                return res;
            }
            return res.Success(new VarAssignNode(varName, converted));
        }

        // look if it is increment or decrement
        if (currentToken.IsType(TokenType.PP))
        {
            return res.Success(
                new VarAssignNode(
                    varName,
                    new BinOpNode(
                        new VarAccessNode(varName),
                        new Token<TokenNoValueType>(TokenType.PLUS),
                        new NumberNode(
                            new Token<double>(
                                TokenType.INT,
                                1,
                                currentToken.StartPos,
                                currentToken.EndPos
                            )
                        )
                    )
                )
            );
        }
        if (currentToken.IsType(TokenType.MM))
        {
            return res.Success(
                new VarAssignNode(
                    varName,
                    new BinOpNode(
                        new VarAccessNode(varName),
                        new Token<TokenNoValueType>(TokenType.MINUS),
                        new NumberNode(
                            new Token<double>(
                                TokenType.INT,
                                1,
                                currentToken.StartPos,
                                currentToken.EndPos
                            )
                        )
                    )
                )
            );
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
        INode expr = res.Register(Expression()); // this gets the "value" of the variable
        if (res.HasError)
        {
            return res;
        }

        return res.Success(new VarAssignNode(varName, expr));
    }

    private ParseResult TryCatchExpr()
    {
        ParseResult res = new();

        if (currentToken.IsNotMatchingKeyword(KeywordType.TRY))
        {
            return res.Failure(new ExpectedKeywordError(currentToken.StartPos, $"Expected TRY"));
        }
        AdvanceParser(res);

        INode tryBlock = res.Register(Statements());
        INode catchBlock = NodeNull.Instance;
        Token<string> varName = new Token<string>(TokenType.NULL);
        if (HasErrorButEnd(res))
        {
            return res;
        }

        while (currentToken.IsType(TokenType.NEWLINE))
        {
            AdvanceParser(res);
        }

        if (currentToken.IsMatchingKeyword(KeywordType.CATCH))
        {
            AdvanceParser(res);

            if (currentToken.IsType(TokenType.IDENTIFIER))
            {
                varName = (Token<string>)currentToken;
                AdvanceParser(res);
            }

            catchBlock = res.Register(Statements());
            if (HasErrorButEnd(res))
            {
                return res;
            }
        }

        return res.Success(new TryCatchNode(tryBlock, catchBlock, varName));
    }

    private ParseResult ImportExpr()
    {
        ParseResult res = new();
        Position startPos = currentToken.StartPos;
        if (currentToken.IsNotMatchingKeyword(KeywordType.IMPORT))
        {
            return res.Failure(new ExpectedKeywordError(currentToken.StartPos, "IMPORT"));
        }

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
        if (!TryCastToken(currentToken, out Token<string> token, out InternalError error))
        {
            return res.Failure(error);
        }
        return res.Success(new ImportNode(token, startPos, currentToken.EndPos));
    }

    // Important Methods Start at bottom to top
    private ParseResult Atom()
    {
        ParseResult res = new();

        IToken tok = currentToken;

        // check tyoes
        if (tok.IsType(TokenType.INT, TokenType.FLOAT))
        {
            AdvanceParser(res);
            return res.Success(new NumberNode((Token<double>)tok));
        }
        else if (tok.IsType(TokenType.STRING))
        {
            AdvanceParser(res);
            return res.Success(new StringNode((Token<string>)tok));
        }
        // check identifier
        else if (tok.IsType(TokenType.IDENTIFIER))
        {
            INode identifierExpr = res.Register(IdentifierExpr());
            if (res.HasError)
            {
                return res;
            }
            return res.Success(identifierExpr);
        }
        // check other symbols
        else if (tok.IsType(TokenType.LPAREN))
        {
            AdvanceParser(res);
            INode expr = res.Register(Expression());
            if (res.HasError)
            {
                return res;
            }

            if (currentToken.IsType(TokenType.RPAREN))
            {
                AdvanceParser(res);
                return res.Success(expr);
            }
            return res.Failure(new InvalidSyntaxError(currentToken.StartPos, "Expected ')'"));
        }
        else if (tok.IsType(TokenType.LSQUARE))
        {
            INode listExpression = res.Register(ListExpr());
            if (res.HasError)
            {
                return res;
            }
            return res.Success(listExpression);
        }
        // check keywords
        else if (tok.IsMatchingKeyword(KeywordType.IF))
        {
            INode ifExpression = res.Register(IfExpr());
            if (res.HasError)
            {
                return res;
            }
            return res.Success(ifExpression);
        }
        else if (tok.IsMatchingKeyword(KeywordType.FOR))
        {
            INode forExpression = res.Register(ForExpr());
            if (res.HasError)
            {
                return res;
            }
            return res.Success(forExpression);
        }
        else if (tok.IsMatchingKeyword(KeywordType.WHILE))
        {
            INode whileExpression = res.Register(WhileExpr());
            if (res.HasError)
            {
                return res;
            }
            return res.Success(whileExpression);
        }
        else if (tok.IsMatchingKeyword(KeywordType.FUN))
        {
            INode funcExpression = res.Register(FuncDef());
            if (res.HasError)
            {
                return res;
            }
            return res.Success(funcExpression);
        }
        else if (tok.IsMatchingKeyword(KeywordType.TRY))
        {
            INode tryCatch = res.Register(TryCatchExpr());
            if (res.HasError)
            {
                return res;
            }
            return res.Success(tryCatch);
        }
        else if (tok.IsMatchingKeyword(KeywordType.IMPORT))
        {
            INode importNode = res.Register(ImportExpr());
            if (res.HasError)
            {
                return res;
            }
            return res.Success(importNode);
        }
        return res.Failure(
            new InvalidSyntaxError(
                tok.StartPos,
                $"expected int, float, identifier, IF, FOR, WHILE, FUN, '(' or '[' but current token is of type {tok.Type}"
            )
        );
    }

    private ParseResult Call()
    {
        ParseResult res = new();

        INode atom = res.Register(Atom());
        if (res.HasError)
        {
            return res;
        }

        if (currentToken.IsType(TokenType.LPAREN))
        {
            List<INode> args = MakeArgs(res);
            if (res.HasError)
            {
                return res;
            }
            return res.Success(new CallNode(atom, args));
        }
        return res.Success(atom);
    }

    private ParseResult Power()
    {
        ParseResult res = new();

        INode left = res.Register(Call());
        if (res.HasError)
        {
            return res;
        }

        while (currentToken.IsType(TokenType.POW))
        {
            if (
                !TryCastToken(
                    currentToken,
                    out Token<TokenNoValueType> opTok,
                    out InternalError error
                )
            )
            {
                return res.Failure(error);
            }
            AdvanceParser(res);

            INode right = res.Register(Factor());
            if (res.HasError)
            {
                return res;
            }

            left = new BinOpNode(left, opTok, right);
        }
        return res.Success(left);
    }

    private ParseResult Factor()
    {
        ParseResult res = new();

        // if there is a plus or a minus it could be +5 or -5
        if (currentToken.IsType(TokenType.PLUS, TokenType.MINUS))
        {
            if (
                !TryCastToken(
                    currentToken,
                    out Token<TokenNoValueType> opTok,
                    out InternalError error
                )
            )
            {
                return res.Failure(error);
            }
            AdvanceParser(res);
            INode factor = res.Register(Factor());
            if (res.HasError)
            {
                return res;
            }
            return res.Success(new UnaryOpNode(opTok, factor));
        }

        return Power();
    }

    private ParseResult Term()
    { // will return BinOpNode
        ParseResult res = new();

        INode left = res.Register(Factor());
        if (res.HasError)
        {
            return res;
        }

        while (currentToken.IsType(TokenType.MUL, TokenType.DIV))
        {
            if (
                !TryCastToken(
                    currentToken,
                    out Token<TokenNoValueType> opTok,
                    out InternalError error
                )
            )
            {
                return res.Failure(error);
            }

            AdvanceParser(res);
            INode right = res.Register(Factor());
            if (res.HasError)
            {
                return res;
            }

            left = new BinOpNode(left, opTok, right);
        }
        return res.Success(left);
    }

    private ParseResult ArithExpr()
    {
        ParseResult res = new();

        INode left = res.Register(Term());
        if (res.HasError)
        {
            return res;
        }

        while (currentToken.IsType(TokenType.PLUS, TokenType.MINUS))
        {
            Token<TokenNoValueType> opTok = (Token<TokenNoValueType>)currentToken;

            AdvanceParser(res);
            INode right = res.Register(Term());
            if (res.HasError)
            {
                return res;
            }

            left = new BinOpNode(left, opTok, right);
        }
        return res.Success(left);
    }

    private ParseResult CompExpr()
    {
        ParseResult res = new();

        if (currentToken.IsMatchingKeyword(KeywordType.NOT))
        {
            IToken opTok = currentToken;

            AdvanceParser(res);
            INode node = res.Register(CompExpr());
            if (res.HasError)
            {
                return res;
            }
            return res.Success(new UnaryOpNode(opTok, node));
        }

        // Binary Operation
        INode left = res.Register(ArithExpr());
        if (res.HasError)
        {
            return res;
        }

        // This checks for the comparison operators
        while (
            currentToken.IsType(
                TokenType.EE,
                TokenType.NE,
                TokenType.GT,
                TokenType.LT,
                TokenType.GTE,
                TokenType.LTE
            )
        )
        {
            if (
                !TryCastToken(
                    currentToken,
                    out Token<TokenNoValueType> opTok,
                    out InternalError error
                )
            )
            {
                return res.Failure(error);
            }

            AdvanceParser(res);
            INode right = res.Register(ArithExpr());
            if (res.HasError)
            {
                return res;
            }

            left = new BinOpNode(left, opTok, right);
        }
        return res.Success(left);
    }

    private ParseResult Expression()
    {
        ParseResult res = new();

        // A variable assignement
        if (currentToken.IsMatchingKeyword(KeywordType.VAR))
        {
            INode node = res.Register(VarAssignExpr());
            if (res.HasError)
            {
                return res;
            }

            return res.Success(node);
        }

        // Binary Operation
        INode left = res.Register(CompExpr());
        if (res.HasError)
        {
            return res;
        }

        while (
            currentToken.IsMatchingKeyword(KeywordType.AND)
            || currentToken.IsMatchingKeyword(KeywordType.OR)
        )
        {
            IToken opTok = currentToken;

            AdvanceParser(res);
            INode right = res.Register(CompExpr());
            if (res.HasError)
            {
                return res;
            }

            left = new BinOpNode(left, opTok, right);
        }
        return res.Success(left);
    }

    private ParseResult Statement()
    {
        ParseResult res = new();
        Position startPos = currentToken.StartPos;

        if (currentToken.IsMatchingKeyword(KeywordType.RETURN))
        {
            AdvanceParser(res);
            INode? _expr = res.TryRegister(Expression());
            if (_expr == null)
            {
                Reverse(res.ToReverseCount);
            }
            return res.Success(new ReturnNode(_expr, startPos, currentToken.StartPos));
        }
        if (currentToken.IsMatchingKeyword(KeywordType.CONTINUE))
        {
            AdvanceParser(res);
            return res.Success(new ContinueNode(startPos, currentToken.StartPos));
        }
        if (currentToken.IsMatchingKeyword(KeywordType.BREAK))
        {
            AdvanceParser(res);
            return res.Success(new BreakNode(startPos, currentToken.StartPos));
        }
        if (currentToken.IsMatchingKeyword(KeywordType.END))
        {
            return res.Failure(new EndKeywordError(currentToken.StartPos));
        }

        INode expr = res.Register(Expression());

        if (res.HasError)
        {
            return res;
        }

        return res.Success(expr);
    }

    private ParseResult Statements()
    {
        ParseResult res = new();
        if (currentToken.StartPos.IsNull)
        {
            return res.Failure(new InternalError("start pos is null"));
        }

        Position StartPos = currentToken.StartPos;
        List<INode> AllStatements = [];
        INode nextStatement;

        while (currentToken.IsNotType(TokenType.EOF)) // repeat until no more lines are available
        {
            while (currentToken.IsType(TokenType.NEWLINE)) // skip all new Lines
            {
                AdvanceParser(res);
            }
            if (currentToken.IsType(TokenType.EOF))
            {
                break;
            }

            nextStatement = res.Register(Statement());
            if (res.HasError)
            {
                if (res.Error is EndKeywordError)
                {
                    return res.Success(new ListNode(AllStatements, StartPos, currentToken.EndPos));
                }
                return res;
            }

            AllStatements.Add(nextStatement);
        }
        if (currentToken.EndPos.IsNull)
        {
            return res.Success(new ListNode(AllStatements, StartPos, StartPos));
        }
        return res.Success(new ListNode(AllStatements, StartPos, currentToken.EndPos));
    }
}
