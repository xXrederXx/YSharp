using YSharp.Types.Common;

namespace YSharp.Types.Interpreter.ClassTypes;

public class VString(string value) : Value()
{
    public string value { get; set; } = value;

    public override ValueAndError GetFunc(string name, List<Value> argNodes)
    {
        if (name == "ToNumber")
        {
            Error err = ValueHelper.CheckArgs(argNodes, 0, [typeof(VString)], context); // format argument
            if (err.IsError)
            {
                return (ValueNull.Instance, err);
            }

            if (double.TryParse(value, out double res))
            {
                return (new VNumber(res), ErrorNull.Instance);
            }
            else
            {
                return (
                    ValueNull.Instance,
                    new WrongFormatError(startPos, value + " can't be converted to Number", context)
                );
            }
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

    public override ValueAndError GetVar(string name)
    {
        if (name == "Length")
        {
            return (new VNumber(value.Length), ErrorNull.Instance);
        }
        return base.GetVar(name);
    }

    public override ValueAndError AddedTo(Value other)
    {
        Value? ret = other switch
        {
            VString _other => new VString(value + _other.value).SetContext(context),
            _ => null,
        };
        if (ret == null)
        {
            return (ValueNull.Instance, IlligalOperation(other));
        }
        return (ret, ErrorNull.Instance);
    }

    public override ValueAndError MuledTo(Value other)
    {
        Value? ret = null;
        if (other is VNumber)
        {
            string _value = "";
            int mulTimes = (int)((VNumber)other).value;

            for (int i = 0; i < mulTimes; i++)
            {
                _value += value;
            }
            ret = new VString(_value).SetContext(context);
        }

        if (ret == null)
        {
            return (ValueNull.Instance, IlligalOperation(other));
        }
        return (ret, ErrorNull.Instance);
    }

    public override ValueAndError GetComparisonEQ(Value other)
    {
        if (other is VString _other)
        {
            return (new VBool(value == _other.value), ErrorNull.Instance);
        }
        else
        {
            return (ValueNull.Instance, IlligalOperation(other));
        }
    }

    public override ValueAndError GetComparisonNE(Value other)
    {
        if (other is VString _other)
        {
            return (new VBool(value != _other.value), ErrorNull.Instance);
        }
        else
        {
            return (ValueNull.Instance, IlligalOperation(other));
        }
    }

    public override bool IsTrue()
    {
        return value.Length > 0;
    }

    public override Value Copy()
    {
        VString copy = new(value);
        copy.SetPos(startPos, endPos);
        copy.SetContext(context);
        return copy;
    }

    public override string ToString()
    {
        return $"\"{value}\"";
    }
}
