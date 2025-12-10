using YSharp.Types.Common;
using YSharp.Types.Interpreter.Internal;

namespace YSharp.Types.Interpreter;

public class Value{
    public Context? context;
    public Position endPos;
    public Position startPos;

    // arethmetic
    public virtual ValueAndError AddedTo(Value other) => (ValueNull.Instance, IlligalOperation(other));

    // other
    public virtual ValueAndError AndedTo(Value other) => (ValueNull.Instance, IlligalOperation(other));

    public virtual Value Copy() => throw new NotImplementedException("No copy method defined");

    public virtual ValueAndError DivedTo(Value other) => (ValueNull.Instance, IlligalOperation(other));

    // comparison
    public virtual ValueAndError GetComparisonEQ(Value other) => (ValueNull.Instance, IlligalOperation(other));

    public virtual ValueAndError GetComparisonGT(Value other) => (ValueNull.Instance, IlligalOperation(other));

    public virtual ValueAndError GetComparisonGTE(Value other) => (ValueNull.Instance, IlligalOperation(other));

    public virtual ValueAndError GetComparisonLT(Value other) => (ValueNull.Instance, IlligalOperation(other));

    public virtual ValueAndError GetComparisonLTE(Value other) => (ValueNull.Instance, IlligalOperation(other));

    public virtual ValueAndError GetComparisonNE(Value other) => (ValueNull.Instance, IlligalOperation(other));

    public virtual ValueAndError GetFunc(string name, List<Value> argNodes) =>
    (
        ValueNull.Instance,
        new FuncNotFoundError(Position.Null, name, context)
    );

    public virtual ValueAndError GetVar(string name) =>
    (
        ValueNull.Instance,
        new VarNotFoundError(Position.Null, name, context)
    );

    public virtual bool IsTrue() => false;

    public virtual ValueAndError MuledTo(Value other) => (ValueNull.Instance, IlligalOperation(other));

    public virtual ValueAndError Notted() => (ValueNull.Instance, IlligalOperation());

    public virtual ValueAndError OredTo(Value other) => (ValueNull.Instance, IlligalOperation(other));

    public virtual ValueAndError PowedTo(Value other) => (ValueNull.Instance, IlligalOperation(other));

    public Value SetContext(Context? context)
    {
        this.context = context;
        return this;
    }

    public Value SetPos(in Position startPos, in Position endPos)
    {
        // set the positions
        this.startPos = startPos;
        this.endPos = endPos;
        return this;
    }

    public virtual ValueAndError SubedTo(Value other) => (ValueNull.Instance, IlligalOperation(other));

    protected Error IlligalOperation(Value? other = null)
    {
        string details = "Illigal Operation";
        if (other == null)
            details += " on self";
        else
            details += $" between {GetType()} and {other.GetType()}";

        return new IlligalOperationError(startPos, details, context);
    }
}

public class ValueNull : Value{
    public static readonly ValueNull Instance = new();

    private ValueNull() { }
}