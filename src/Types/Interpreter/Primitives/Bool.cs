using YSharp.Types.Common;

namespace YSharp.Types.Interpreter.Primatives;

public class VBool(bool value) : Value()
{
    public bool value { get; set; } = value;

    public override ValueAndError GetComparisonEQ(Value other)
    {
        Value? ret = other switch
        {
            VBool _other => new VBool(value == _other.value),
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
            VBool _other => new VBool(value != _other.value),
            _ => null,
        };
        if (ret == null)
        {
            return (ValueNull.Instance, IlligalOperation(other));
        }
        return (ret, ErrorNull.Instance);
    }

    public override ValueAndError AndedTo(Value other)
    {
        Value? ret = other switch
        {
            VBool _other => new VBool(value && _other.value),
            _ => null,
        };
        if (ret == null)
        {
            return (ValueNull.Instance, IlligalOperation(other));
        }
        return (ret, ErrorNull.Instance);
    }

    public override ValueAndError OredTo(Value other)
    {
        Value? ret = other switch
        {
            VBool _other => new VBool(value || _other.value),
            _ => null,
        };
        if (ret == null)
        {
            return (ValueNull.Instance, IlligalOperation(other));
        }
        return (ret, ErrorNull.Instance);
    }

    public override ValueAndError Notted()
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
