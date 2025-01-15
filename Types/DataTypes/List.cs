using YSharp.Types.InternalTypes;

namespace YSharp.Types.ClassTypes;

public class VList(List<Value> elements) : Value
{
    public List<Value> elements = elements;

    public override ValueAndError GetVar(string name)
    {
        if (name == "Count")
        {
            return (new VNumber(elements.Count), ErrorNull.Instance);
        }
        return base.GetVar(name);
    }

    private (int, Error) ConvertToCSindex(List<Value> argValues)
    { // converts a value to a csharp usable index
        Error err = ValueHelper.CheckArgs(argValues, 1, [typeof(VNumber)], context);
        if (err.IsError)
        {
            return (0, err);
        }

        int index = (int)((VNumber)argValues[0]).value;

        if (index >= elements.Count)
        {
            return (
                0,
                new ArgOutOfRangeError(
                    argValues[0].startPos,
                    "Index was out of range. Must be less than size of list.",
                    context
                )
            );
        }

        if (index < 0)
        {
            index = elements.Count + index; // -1 would be last element
        }

        if (index < 0)
        {
            return (
                0,
                new ArgOutOfRangeError(
                    argValues[0].startPos,
                    "Index was out of range. Negative size cant be greater than size of list.",
                    context
                )
            );
        }

        return (index, ErrorNull.Instance);
    }

    public override ValueAndError GetFunc(string name, List<Value> argValues)
    {
        if (name == "Add")
        {
            elements.AddRange(argValues);
            return (ValueNull.Instance, ErrorNull.Instance);
        }
        else if (name == "Get")
        {
            (int, Error) index = ConvertToCSindex(argValues);
            if (index.Item2.IsError)
            {
                return (ValueNull.Instance, index.Item2);
            }

            return (elements[index.Item1], ErrorNull.Instance);
        }
        else if (name == "Remove")
        {
            (int, Error) index = ConvertToCSindex(argValues);
            if (index.Item2.IsError)
            {
                return (ValueNull.Instance, index.Item2);
            }

            elements.RemoveAt(index.Item1);
            return (ValueNull.Instance, ErrorNull.Instance);
        }
        else if (name == "IndexOf")
        { // returns -1 if not found
            Error err = ValueHelper.IsRightLength(1, argValues, context);
            if (err.IsError)
            {
                return (ValueNull.Instance, err);
            }

            return (IndexOf(argValues[0]), ErrorNull.Instance);
        }
        return base.GetFunc(name, argValues);
    }

    private VNumber IndexOf(Value value)
    {
        if (value == null)
            return new(-1);

        // Use pattern matching to simplify type checks and casts
        int index = value switch
        {
            VNumber num => elements.FindIndex(v => v is VNumber n && n.value == num.value),
            VString str => elements.FindIndex(v => v is VString s && s.value == str.value),
            VBool b => elements.FindIndex(v => v is VBool boolVal && boolVal.value == b.value),
            VList list => elements.FindIndex(v =>
                v is VList l && l.elements.SequenceEqual(list.elements)
            ),
            _ => -1,
        };

        return new(index);
    }

    public override ValueAndError AddedTo(Value other)
    {
        if (other is VList _other)
        {
            elements.AddRange(_other.elements);
            return (new VList(elements), ErrorNull.Instance);
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
            List<Value> startElments = [.. elements];
            if (_other.value < 2)
            {
                return (_other.value < 1 ? new VList([]) : new VList(elements), ErrorNull.Instance); // * 1 = original * 0 or less = []
            }

            for (int i = 2; i <= _other.value; i++)
            {
                elements.AddRange(startElments);
            }

            return (new VList(elements), ErrorNull.Instance);
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
            return (new VBool(elements == _other.elements), ErrorNull.Instance);
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
            return (new VBool(elements != _other.elements), ErrorNull.Instance);
        }
        else
        {
            return (ValueNull.Instance, IlligalOperation(other));
        }
    }

    public override bool IsTrue()
    {
        return elements.Count > 0;
    }

    public override Value Copy()
    {
        VList copy = new(elements);
        copy.SetPos(startPos, endPos);
        copy.SetContext(context);
        return copy;
    }

    public override string ToString()
    {
        string res = "[";

        for (int i = 0; i < elements.Count; i++)
        {
            Value v = elements[i];
            res += v.ToString();
            if (i != elements.Count - 1)
            {
                res += ", ";
            }
        }
        return res + "]";
    }
}
