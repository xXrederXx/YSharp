using YSharp.Types.Common;

namespace YSharp.Types.Interpreter;

public class Value
{
    public Position startPos;
    public Position endPos;
    protected Context? context;

    public Value() { }

    public Value SetPos(in Position startPos, in Position endPos)
    { // set the positions
        this.startPos = startPos;
        this.endPos = endPos;
        return this;
    }

    public Value SetContext(Context? context)
    {
        this.context = context;
        return this;
    }

    public virtual ValueAndError GetVar(string name)
    {
        return (
            ValueNull.Instance,
            new VarNotFoundError(Position.Null, name, context)
        );
    }

    public virtual ValueAndError GetFunc(string name, List<Value> argNodes)
    {
        return (
            ValueNull.Instance,
            new FuncNotFoundError(Position.Null, name, context)
        );
    }

    // arethmetic
    public virtual ValueAndError AddedTo(Value other)
    {
        return (ValueNull.Instance, IlligalOperation(other));
    }

    public virtual ValueAndError SubedTo(Value other)
    {
        return (ValueNull.Instance, IlligalOperation(other));
    }

    public virtual ValueAndError MuledTo(Value other)
    {
        return (ValueNull.Instance, IlligalOperation(other));
    }

    public virtual ValueAndError DivedTo(Value other)
    {
        return (ValueNull.Instance, IlligalOperation(other));
    }

    public virtual ValueAndError PowedTo(Value other)
    {
        return (ValueNull.Instance, IlligalOperation(other));
    }

    // comparison
    public virtual ValueAndError GetComparisonEQ(Value other)
    {
        return (ValueNull.Instance, IlligalOperation(other));
    }

    public virtual ValueAndError GetComparisonNE(Value other)
    {
        return (ValueNull.Instance, IlligalOperation(other));
    }

    public virtual ValueAndError GetComparisonLT(Value other)
    {
        return (ValueNull.Instance, IlligalOperation(other));
    }

    public virtual ValueAndError GetComparisonGT(Value other)
    {
        return (ValueNull.Instance, IlligalOperation(other));
    }

    public virtual ValueAndError GetComparisonLTE(Value other)
    {
        return (ValueNull.Instance, IlligalOperation(other));
    }

    public virtual ValueAndError GetComparisonGTE(Value other)
    {
        return (ValueNull.Instance, IlligalOperation(other));
    }

    // other
    public virtual ValueAndError AndedTo(Value other)
    {
        return (ValueNull.Instance, IlligalOperation(other));
    }

    public virtual ValueAndError OredTo(Value other)
    {
        return (ValueNull.Instance, IlligalOperation(other));
    }

    public virtual ValueAndError Notted()
    {
        return (ValueNull.Instance, IlligalOperation());
    }

    public virtual bool IsTrue()
    {
        return false;
    }

    public virtual Value Copy()
    {
        throw new NotImplementedException("No copy method defined");
    }

    protected Error IlligalOperation(Value? other = null)
    {
        string details = "Illigal Operation";
        if (other == null)
        {
            details += " on self";
        }
        else
        {
            details += $" between {GetType()} and {other.GetType()}";
        }

        return new IlligalOperationError(startPos, details, context);
    }
}

public class ValueNull : Value
{
    public static readonly ValueNull Instance = new();

    private ValueNull() { }
}
