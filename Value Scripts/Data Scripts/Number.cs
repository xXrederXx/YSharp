namespace YSharp_2._0;

public class Number(double value) : Value()
{
    public readonly double value = value;

    public override ValueError GetFunc(string name, List<Value> argNodes)
    {
        if (name == "ToString")
        {
            Error err = CheckArgs(argNodes, 0, []); // no argument
            if (!err.IsError)
            {
                return (new String(value.ToString()), NoError.Instance);
            }

            err = CheckArgs(argNodes, 1, [typeof(String)]); // format argument
            if (err.IsError)
            {
                return (ValueNull.Instance, err);
            }

            string format = value.ToString(((String)argNodes[0]).value);
            //TODO: Add format exeption
            return (new String(format), NoError.Instance);

        }
        else if (name == "ToBool")
        {
            Error err = CheckArgs(argNodes, 0, []); // no argument
            if (!err.IsError)
            {
                return (ValueNull.Instance, err);
            }

            return (new Bool(isTrue()), NoError.Instance);
        }
        return base.GetFunc(name, argNodes);
    }
    // arethmetic
    public override ValueError addedTo(Value other)
    { // add
        Value? ret = other switch
        {
            Number _other => new Number(value + _other.value),
            _ => null
        };
        if (ret == null)
        {
            return (ValueNull.Instance, IlligalOperation(other));
        }
        return (ret, NoError.Instance);
    }
    public override ValueError subedTo(Value other)
    { // substract
        Value? ret = other switch
        {
            Number _other => new Number(value - _other.value),
            _ => null
        };
        if (ret == null)
        {
            return (ValueNull.Instance, IlligalOperation(other));
        }
        return (ret, NoError.Instance);
    }
    public override ValueError muledTo(Value other)
    { // multiply
        Value? ret = other switch
        {
            Number _other => new Number(value * _other.value),
            _ => null
        };
        if (ret == null)
        {
            return (ValueNull.Instance, IlligalOperation(other));
        }
        return (ret, NoError.Instance);
    }
    public override ValueError divedTo(Value other)
    { // divide
        if (other is Number _other)
        {
            if (_other.value == 0)
            {
                return (new Bool(false), new RunTimeError(_other.startPos, "Can't divide by 0", context));
            }
            Number? ret = new(value / _other.value);
            ret.SetContext(context);
            return (ret, NoError.Instance);
        }
        else
        {
            return (ValueNull.Instance, IlligalOperation(other));
        }
    }
    public override ValueError powedTo(Value other)
    { // power
        Value? ret = other switch
        {
            Number _other => new Number(System.Math.Pow(value, _other.value)),
            _ => null
        };
        if (ret == null)
        {
            return (ValueNull.Instance, IlligalOperation(other));
        }
        return (ret, NoError.Instance);
    }
    // comparison
    public override ValueError getComparisonEQ(Value other)
    {
        Value? ret = other switch
        {
            Number _other => new Bool(value == _other.value),
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
            Number _other => new Bool(value != _other.value),
            _ => null
        };
        if (ret == null)
        {
            return (ValueNull.Instance, IlligalOperation(other));
        }
        return (ret, NoError.Instance);
    }
    public override ValueError getComparisonLT(Value other)
    {
        Value? ret = other switch
        {
            Number _other => new Bool(value < _other.value),
            _ => null
        };
        if (ret == null)
        {
            return (ValueNull.Instance, IlligalOperation(other));
        }
        return (ret, NoError.Instance);
    }
    public override ValueError getComparisonGT(Value other)
    {
        Value? ret = other switch
        {
            Number _other => new Bool(value > _other.value),
            _ => null
        };
        if (ret == null)
        {
            return (ValueNull.Instance, IlligalOperation(other));
        }
        return (ret, NoError.Instance);
    }
    public override ValueError getComparisonLTE(Value other)
    {
        Value? ret = other switch
        {
            Number _other => new Bool(value <= _other.value),
            _ => null
        };
        if (ret == null)
        {
            return (ValueNull.Instance, IlligalOperation(other));
        }
        return (ret, NoError.Instance);
    }
    public override ValueError getComparisonGTE(Value other)
    {
        Value? ret = other switch
        {
            Number _other => new Bool(value >= _other.value),
            _ => null
        };
        if (ret == null)
        {
            return (ValueNull.Instance, IlligalOperation(other));
        }
        return (ret, NoError.Instance);
    }
    public override bool isTrue()
    {
        return value != 0;
    }
    public override Number copy()
    {
        Number copy = new(value);
        copy.SetPos(startPos, endPos);
        copy.SetContext(context);
        return copy;
    }

    // custom string representation
    public override string ToString()
    {
        return $"{value}";
    }
}

