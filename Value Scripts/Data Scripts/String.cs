namespace YSharp_2._0;

public class String(string value) : Value()
{
    public readonly string value = value;

    public override ValueError GetFunc(string name, List<Value> argNodes)
    {
        if (name == "ToNumber")
        {
            Error err = CheckArgs(argNodes, 0, [typeof(String)]); // format argument
            if (err.IsError)
            {
                return (ValueNull.Instance, err);
            }

            if (double.TryParse(value, out double res))
            {
                return (new Number(res), NoError.Instance);
            }
            else
            {
                return (ValueNull.Instance, new WrongFormatError(startPos, value + " can't be converted to Number"));
            }
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
    public override ValueError GetVar(string name)
    {
        if(name == "Length"){
            return (new Number(value.Length), NoError.Instance);
        }
        return base.GetVar(name);
    }
    public override ValueError addedTo(Value other)
    {
        Value? ret = other switch
        {
            String _other => new String(value + _other.value).SetContext(context),
            _ => null
        };
        if (ret == null)
        {
            return (ValueNull.Instance, IlligalOperation(other));
        }
        return (ret, NoError.Instance);
    }
    public override ValueError muledTo(Value other)
    {
        Value? ret = null;
        if (other is Number)
        {
            string _value = "";
            int mulTimes = (int)((Number)other).value;

            for (int i = 0; i < mulTimes; i++)
            {
                _value += value;
            }
            ret = new String(_value).SetContext(context);
        }

        if (ret == null)
        {
            return (ValueNull.Instance, IlligalOperation(other));
        }
        return (ret, NoError.Instance);
    }

    public override ValueError getComparisonEQ(Value other)
    {
        if (other is String _other)
        {
            return (new Bool(value == _other.value), NoError.Instance);
        }
        else
        {
            return (ValueNull.Instance, IlligalOperation(other));
        }
    }
    public override ValueError getComparisonNE(Value other)
    {
        if (other is String _other)
        {
            return (new Bool(value != _other.value), NoError.Instance);
        }
        else
        {
            return (ValueNull.Instance, IlligalOperation(other));
        }
    }

    public override bool isTrue()
    {
        return value.Length > 0;
    }
    public override Value copy()
    {
        String copy = new(value);
        copy.SetPos(startPos, endPos);
        copy.SetContext(context);
        return copy;
    }

    public override string ToString()
    {
        return $"\"{value}\"";
    }
}

