
namespace YSharp_2._0;

public class Value
{
    public Position startPos;
    public Position endPos;
    protected Context? context;

    public Value()
    {
        SetPos(Position.Null, Position.Null);
        SetContext();
    }
    
    public Value SetPos(Position startPos, Position endPos)
    { // set the positions
        this.startPos = startPos;
        this.endPos = endPos;
        return this;
    }
    public Value SetContext(Context? context = null)
    {
        this.context = context;
        return this;
    }

    public virtual ValueError GetVar(string name)
    {
        return (ValueNull.Instance, new VarNotFoundError(Position.Null,  $"Variable {name} was not found", context));
    }
    public virtual ValueError GetFunc(string name, List<Value> argNodes)
    {
        return (ValueNull.Instance, new FuncNotFoundError(Position.Null, $"Function {name} was not found", context));
    }

    // arethmetic
    public virtual ValueError AddedTo(Value other)
    {
        return (ValueNull.Instance, IlligalOperation(other));
    }
    public virtual ValueError SubedTo(Value other)
    {
        return (ValueNull.Instance, IlligalOperation(other));
    }
    public virtual ValueError MuledTo(Value other)
    {
        return (ValueNull.Instance, IlligalOperation(other));
    }
    public virtual ValueError DivedTo(Value other)
    {
        return (ValueNull.Instance, IlligalOperation(other));
    }
    public virtual ValueError PowedTo(Value other)
    {
        return (ValueNull.Instance, IlligalOperation(other));
    }
    // comparison
    public virtual ValueError GetComparisonEQ(Value other)
    {
        return (ValueNull.Instance, IlligalOperation(other));
    }
    public virtual ValueError GetComparisonNE(Value other)
    {
        return (ValueNull.Instance, IlligalOperation(other));
    }
    public virtual ValueError GetComparisonLT(Value other)
    {
        return (ValueNull.Instance, IlligalOperation(other));
    }
    public virtual ValueError GetComparisonGT(Value other)
    {
        return (ValueNull.Instance, IlligalOperation(other));
    }
    public virtual ValueError GetComparisonLTE(Value other)
    {
        return (ValueNull.Instance, IlligalOperation(other));
    }
    public virtual ValueError GetComparisonGTE(Value other)
    {
        return (ValueNull.Instance, IlligalOperation(other));
    }
    // other
    public virtual ValueError AndedTo(Value other)
    {
        return (ValueNull.Instance, IlligalOperation(other));
    }
    public virtual ValueError OredTo(Value other)
    {
        return (ValueNull.Instance, IlligalOperation(other));
    }
    public virtual ValueError Notted()
    {
        return (ValueNull.Instance, IlligalOperation());
    }
    public virtual bool IsTrue()
    {
        return false;
    }
    public virtual Value Copy()
    {
        throw new Exception("No copy method defined");
    }

    /// <summary>
    ///  This is used when this operation cant be processed
    /// </summary>
    /// <returns>A new Runtime Error</returns>
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
public class ValueNull : Value { 
    public static readonly ValueNull Instance = new();
    private ValueNull() { }
}


