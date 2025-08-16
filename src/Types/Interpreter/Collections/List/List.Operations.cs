using YSharp.Types.Common;
using YSharp.Types.Interpreter.Primitives;

namespace YSharp.Types.Interpreter.Collection;

public sealed partial class VList : Value
{
    public override ValueAndError AddedTo(Value other)
    {
        if (other is VList _other)
        {
            value.AddRange(_other.value);
            return (new VList(value), ErrorNull.Instance);
        }
        else
        {
            return (ValueNull.Instance, IlligalOperation(other));
        }
    }

    public override ValueAndError MuledTo(Value other)
    {
        if (other is VNumber _other)
        {
            List<Value> startElments = [.. value];
            if (_other.value < 2)
            {
                return (_other.value < 1 ? new VList([]) : new VList(value), ErrorNull.Instance); // * 1 = original * 0 or less = []
            }

            for (int i = 2; i <= _other.value; i++)
            {
                value.AddRange(startElments);
            }

            return (new VList(value), ErrorNull.Instance);
        }
        else
        {
            return (ValueNull.Instance, IlligalOperation(other));
        }
    }

    public override ValueAndError GetComparisonEQ(Value other)
    {
        if (other is VList _other)
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
        if (other is VList _other)
        {
            return (new VBool(value != _other.value), ErrorNull.Instance);
        }
        else
        {
            return (ValueNull.Instance, IlligalOperation(other));
        }
    }
}
