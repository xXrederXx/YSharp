using System.Runtime.CompilerServices;
using YSharp.Internal;
using YSharp.Types;
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
}

// this keeps the result of the parser
public class ParseResult
{
    public Error Error { get; private set; } = ErrorNull.Instance;
    public INode Node { get; private set; } = NodeNull.Instance;
    private int _advanceCount = 0;
    public int ToReverseCount { get; private set; } = 0;

    // Check if an error exists
    public bool HasError => Error.IsError;

    public void ResetError()
    {
        Error = ErrorNull.Instance;
    }

    // Try to register the result; if error is present, mark reversal
    public INode? TryRegister(ParseResult result)
    {
        if (result.HasError)
        {
            ToReverseCount = result._advanceCount;
            return null;
        }
        return Register(result);
    }

    // Register result and accumulate advances
    public INode Register(ParseResult result)
    {
        _advanceCount += result._advanceCount;
        Error = result.Error;
        return result.Node;
    }

    public bool SafeRegrister(ParseResult result, out INode node)
    {
        node = Register(result);
        return result.HasError;
    }

    // Register an advancement count increment
    public void Advance()
    {
        _advanceCount++;
    }

    // Return successful result with Node
    public ParseResult Success(INode node)
    {
        Node = node;
        return this;
    }

    // Return failure result and update error if not already set
    public ParseResult Failure(Error error)
    {
        if (!HasError || _advanceCount == 0)
        {
            Error = error;
        }
        //* For testing -> Console.WriteLine(error.ToString() + Node.ToString());
        return this;
    }

    public override string ToString()
    {
        return Node.ToString() ?? "null";
    }
}
