using YSharp.Types.InternalTypes;

namespace YSharp.Types.ClassTypes;

public class VNumber(double value) : Value(), IDefaultConvertableValue<double>
{
    public double value { get; set; } = value;

    public override ValueAndError GetFunc(string name, List<Value> argNodes)
    {
        if (name == "ToString")
        {
            Error err = ValueHelper.CheckArgs(argNodes, 0, [], context); // no argument
            if (!err.IsError)
            {
                return (new VString(value.ToString()), ErrorNull.Instance);
            }

            err = ValueHelper.CheckArgs(argNodes, 1, [typeof(VString)], context); // format argument
            if (err.IsError)
            {
                return (ValueNull.Instance, err);
            }

            string format = value.ToString(((VString)argNodes[0]).value);
            //TODO: Add format exeption
            return (new VString(format), ErrorNull.Instance);
        }
        else if (name == "ToBool")
        {
            Error err = ValueHelper.CheckArgs(argNodes, 0, [], context); // no argument
            if (!err.IsError)
            {
                return (ValueNull.Instance, err);
            }

            return (new VBool(IsTrue()), ErrorNull.Instance);
        }
        return base.GetFunc(name, argNodes);
    }

    // arethmetic
    public override ValueAndError AddedTo(Value other)
    { // add
        Value? ret = other switch
        {
            VNumber _other => new VNumber(value + _other.value),
            _ => null,
        };
        if (ret == null)
        {
            return (ValueNull.Instance, IlligalOperation(other));
        }
        return (ret, ErrorNull.Instance);
    }

    public override ValueAndError SubedTo(Value other)
    { // substract
        Value? ret = other switch
        {
            VNumber _other => new VNumber(value - _other.value),
            _ => null,
        };
        if (ret == null)
        {
            return (ValueNull.Instance, IlligalOperation(other));
        }
        return (ret, ErrorNull.Instance);
    }

    public override ValueAndError MuledTo(Value other)
    { // multiply
        Value? ret = other switch
        {
            VNumber _other => new VNumber(value * _other.value),
            _ => null,
        };
        if (ret == null)
        {
            return (ValueNull.Instance, IlligalOperation(other));
        }
        return (ret, ErrorNull.Instance);
    }

    public override ValueAndError DivedTo(Value other)
    { // divide
        if (other is VNumber _other)
        {
            if (_other.value == 0)
            {
                return (
                    new VBool(false),
                    new RunTimeError(_other.startPos, "Can't divide by 0", context)
                );
            }
            VNumber? ret = new(value / _other.value);
            ret.SetContext(context);
            return (ret, ErrorNull.Instance);
        }
        else
        {
            return (ValueNull.Instance, IlligalOperation(other));
        }
    }

    public override ValueAndError PowedTo(Value other)
    { // power
        Value? ret = other switch
        {
            VNumber _other => new VNumber(System.Math.Pow(value, _other.value)),
            _ => null,
        };
        if (ret == null)
        {
            return (ValueNull.Instance, IlligalOperation(other));
        }
        return (ret, ErrorNull.Instance);
    }

    // comparison
    public override ValueAndError GetComparisonEQ(Value other)
    {
        Value? ret = other switch
        {
            VNumber _other => new VBool(value == _other.value),
            _ => null,
        };
        if (ret == null)
        {
            return (ValueNull.Instance, IlligalOperation(other));
        }
        return (ret, ErrorNull.Instance);
    }

    public override ValueAndError GetComparisonNE(Value other)
    {
        Value? ret = other switch
        {
            VNumber _other => new VBool(value != _other.value),
            _ => null,
        };
        if (ret == null)
        {
            return (ValueNull.Instance, IlligalOperation(other));
        }
        return (ret, ErrorNull.Instance);
    }

    public override ValueAndError GetComparisonLT(Value other)
    {
        Value? ret = other switch
        {
            VNumber _other => new VBool(value < _other.value),
            _ => null,
        };
        if (ret == null)
        {
            return (ValueNull.Instance, IlligalOperation(other));
        }
        return (ret, ErrorNull.Instance);
    }

    public override ValueAndError GetComparisonGT(Value other)
    {
        Value? ret = other switch
        {
            VNumber _other => new VBool(value > _other.value),
            _ => null,
        };
        if (ret == null)
        {
            return (ValueNull.Instance, IlligalOperation(other));
        }
        return (ret, ErrorNull.Instance);
    }

    public override ValueAndError GetComparisonLTE(Value other)
    {
        Value? ret = other switch
        {
            VNumber _other => new VBool(value <= _other.value),
            _ => null,
        };
        if (ret == null)
        {
            return (ValueNull.Instance, IlligalOperation(other));
        }
        return (ret, ErrorNull.Instance);
    }

    public override ValueAndError GetComparisonGTE(Value other)
    {
        Value? ret = other switch
        {
            VNumber _other => new VBool(value >= _other.value),
            _ => null,
        };
        if (ret == null)
        {
            return (ValueNull.Instance, IlligalOperation(other));
        }
        return (ret, ErrorNull.Instance);
    }

    public override bool IsTrue()
    {
        return value != 0;
    }

    public override VNumber Copy()
    {
        VNumber copy = new(value);
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
