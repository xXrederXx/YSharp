namespace YSharp_2._0;

public class VBool(bool value) : Value()
{
    public readonly bool value = value;

    public override ValueError GetComparisonEQ(Value other)
    {
        Value? ret = other switch
        {
            VBool _other => new VBool(value == _other.value),
            _ => null
        };
        if (ret == null)
        {
            return (ValueNull.Instance, IlligalOperation(other));
        }
        return (ret, ErrorNull.Instance);
    }
    public override ValueError GetComparisonNE(Value other)
    {
        Value? ret = other switch
        {
            VBool _other => new VBool(value != _other.value),
            _ => null
        };
        if (ret == null)
        {
            return (ValueNull.Instance, IlligalOperation(other));
        }
        return (ret, ErrorNull.Instance);
    }
    public override ValueError AndedTo(Value other)
    {
        Value? ret = other switch
        {
            VBool _other => new VBool(value && _other.value),
            _ => null
        };
        if (ret == null)
        {
            return (ValueNull.Instance, IlligalOperation(other));
        }
        return (ret, ErrorNull.Instance);
    }
    public override ValueError OredTo(Value other)
    {
        Value? ret = other switch
        {
            VBool _other => new VBool(value || _other.value),
            _ => null
        };
        if (ret == null)
        {
            return (ValueNull.Instance, IlligalOperation(other));
        }
        return (ret, ErrorNull.Instance);
    }
    public override ValueError Notted()
    {
        VBool? ret = new(!value);
        ret.SetContext(context);
        return (ret, ErrorNull.Instance);
    }

    public override bool IsTrue()
    {
        return value;
    }
    public override VBool Copy()
    {
        VBool copy = new(value);
        copy.SetPos(startPos, endPos);
        copy.SetContext(context);
        return copy;
    }

    public override string ToString()
    {
        return value.ToString();
    }
}

