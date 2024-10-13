namespace YSharp_2._0;

public class Bool(bool value) : Value()
{
    public readonly bool value = value;

    public override ValueError getComparisonEQ(Value other)
    {
        Value? ret = other switch
        {
            Bool _other => new Bool(value == _other.value),
            _ => null
        };
        if (ret == null)
        {
            return (ValueNull.Instance, IlligalOperation(other));
        }
        return (ret, NoError.Instance);
    }
    public override ValueError getComparisonNE(Value other)
    {
        Value? ret = other switch
        {
            Bool _other => new Bool(value != _other.value),
            _ => null
        };
        if (ret == null)
        {
            return (ValueNull.Instance, IlligalOperation(other));
        }
        return (ret, NoError.Instance);
    }
    public override ValueError andedTo(Value other)
    {
        Value? ret = other switch
        {
            Bool _other => new Bool(value && _other.value),
            _ => null
        };
        if (ret == null)
        {
            return (ValueNull.Instance, IlligalOperation(other));
        }
        return (ret, NoError.Instance);
    }
    public override ValueError oredTo(Value other)
    {
        Value? ret = other switch
        {
            Bool _other => new Bool(value || _other.value),
            _ => null
        };
        if (ret == null)
        {
            return (ValueNull.Instance, IlligalOperation(other));
        }
        return (ret, NoError.Instance);
    }
    public override ValueError notted()
    {
        Bool? ret = new(!value);
        ret.SetContext(context);
        return (ret, NoError.Instance);
    }

    public override bool isTrue()
    {
        return value;
    }
    public override Bool copy()
    {
        Bool copy = new(value);
        copy.SetPos(startPos, endPos);
        copy.SetContext(context);
        return copy;
    }

    public override string ToString()
    {
        return value.ToString();
    }
}

