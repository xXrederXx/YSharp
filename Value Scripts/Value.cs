using System;

namespace YSharp_2._0;

public class Value
{
    public Position startPos;
    public Position endPos;
    protected Context? context;

    public Value()
    {
        SetPos(Position._null, Position._null);
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
        return (ValueNull.Instance, new VarNotFoundError(Position._null,  $"Variable {name} was not found"));
    }
    public virtual ValueError GetFunc(string name, List<Value> argNodes)
    {
        return (ValueNull.Instance, new FuncNotFoundError(Position._null, $"Function {name} was not found"));
    }

    protected static Error IsRightLength(int length, List<Value> argValue)
    {
        if (length == argValue.Count)
        {
            return NoError.Instance;
        }
        return new NumArgsError(Position._null, $"Should have {length} args not {argValue.Count}");
    }
    protected static Error IsRightType(List<Type> types, List<Value> argValue)
    {
        if (!(types.Count == 1 || types.Count == argValue.Count))
        {
            return new InternalError("types must have one element or match size of argValue");
        }
        bool oneType = types.Count == 1;
        for (int i = 0; i < argValue.Count; i++)
        {
            Type valType = argValue[i].GetType();

            if (oneType && valType != types[0])
            { // one type
                return new WrongFormatError(Position._null, $"Value should have Type {types[0]} and not {valType}");
            }
            else if (valType != types[i])
            {
                return new WrongFormatError(Position._null, $"Value should have {types[0]} and not Type {valType}");
            }
        }
        return NoError.Instance;
    }
    protected static Error CheckArgs(List<Value> argValue, int length, List<Type> types)
    {
        Error ret;
        ret = IsRightLength(length, argValue);
        if (ret.IsError)
        {
            return ret;
        }
        if (length > 0)
        {
            ret = IsRightType(types, argValue);
        }
        return ret;

    }

    // arethmetic
    public virtual ValueError addedTo(Value other)
    {
        return (ValueNull.Instance, IlligalOperation(other));
    }
    public virtual ValueError subedTo(Value other)
    {
        return (ValueNull.Instance, IlligalOperation(other));
    }
    public virtual ValueError muledTo(Value other)
    {
        return (ValueNull.Instance, IlligalOperation(other));
    }
    public virtual ValueError divedTo(Value other)
    {
        return (ValueNull.Instance, IlligalOperation(other));
    }
    public virtual ValueError powedTo(Value other)
    {
        return (ValueNull.Instance, IlligalOperation(other));
    }
    // comparison
    public virtual ValueError getComparisonEQ(Value other)
    {
        return (ValueNull.Instance, IlligalOperation(other));
    }
    public virtual ValueError getComparisonNE(Value other)
    {
        return (ValueNull.Instance, IlligalOperation(other));
    }
    public virtual ValueError getComparisonLT(Value other)
    {
        return (ValueNull.Instance, IlligalOperation(other));
    }
    public virtual ValueError getComparisonGT(Value other)
    {
        return (ValueNull.Instance, IlligalOperation(other));
    }
    public virtual ValueError getComparisonLTE(Value other)
    {
        return (ValueNull.Instance, IlligalOperation(other));
    }
    public virtual ValueError getComparisonGTE(Value other)
    {
        return (ValueNull.Instance, IlligalOperation(other));
    }
    // other
    public virtual ValueError andedTo(Value other)
    {
        return (ValueNull.Instance, IlligalOperation(other));
    }
    public virtual ValueError oredTo(Value other)
    {
        return (ValueNull.Instance, IlligalOperation(other));
    }
    public virtual ValueError notted()
    {
        return (ValueNull.Instance, IlligalOperation());
    }
    public virtual bool isTrue()
    {
        return false;
    }
    public virtual Value copy()
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
            details += $" between {this.GetType()} and {other.GetType()}";
        }

        return new IlligalOperationError(this.startPos, details, context);
    }
}
public class ValueNull : Value { 
    public static readonly ValueNull Instance = new();
}


