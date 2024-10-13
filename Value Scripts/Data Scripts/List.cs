namespace YSharp_2._0;

public class List(List<Value> elements) : Value
{
    public List<Value> elements = elements;

    public override ValueError GetVar(string name)
    {
        if (name == "Count")
        {
            return (new Number(elements.Count), NoError.Instance);
        }
        return base.GetVar(name);
    }
    private (int, Error) convertToCSindex(List<Value> argValues)
    { // converts a value to a csharp usable index
        Error err = CheckArgs(argValues, 1, [typeof(Number)]);
        if (err.IsError)
        {
            return (0, err);
        }

        int index = (int)((Number)argValues[0]).value;

        if (index >= elements.Count)
        {
            return (0, new ArgOutOfRangeError(argValues[0].startPos, "Index was out of range. Must be less than size of list."));
        }

        if (index < 0)
        {
            index = elements.Count + index; // -1 would be last element
        }

        if (index < 0)
        {
            return (0, new ArgOutOfRangeError(argValues[0].startPos, "Index was out of range. Negative size cant be greater than size of list."));
        }

        return (index, NoError.Instance);
    }
    public override ValueError GetFunc(string name, List<Value> argValues)
    {
        if (name == "Add")
        {
            elements.AddRange(argValues);
            return (ValueNull.Instance, NoError.Instance);
        }
        else if (name == "Get")
        {
            (int, Error) index = convertToCSindex(argValues);
            if (index.Item2.IsError)
            {
                return (ValueNull.Instance, index.Item2);
            }

            return (elements[index.Item1], NoError.Instance);
        }
        else if (name == "Remove")
        {
            (int, Error) index = convertToCSindex(argValues);
            if (index.Item2.IsError)
            {
                return (ValueNull.Instance, index.Item2);
            }

            elements.RemoveAt(index.Item1);
            return (ValueNull.Instance, NoError.Instance);
        }
        else if (name == "IndexOf")
        { // returns -1 if not found
            Error err = IsRightLength(1, argValues);
            if (err.IsError)
            {
                return (ValueNull.Instance, err);
            }

            return (IndexOf(argValues[0]), NoError.Instance);
        }
        return base.GetFunc(name, argValues);
    }
    private Number IndexOf(Value value)
    {
        if (value == null) return new(-1);

        // Use pattern matching to simplify type checks and casts
        var index = value switch
        {
            Number num => elements.FindIndex(v => v is Number n && n.value == num.value),
            String str => elements.FindIndex(v => v is String s && s.value == str.value),
            Bool b => elements.FindIndex(v => v is Bool boolVal && boolVal.value == b.value),
            List list => elements.FindIndex(v => v is List l && l.elements.SequenceEqual(list.elements)),
            _ => -1
        };

        return new(index);
    }

    public override ValueError addedTo(Value other)
    {
        if (other is List _other)
        {
            elements.AddRange(_other.elements);
            return (new List(elements), NoError.Instance);
        }
        else
        {
            return (ValueNull.Instance, IlligalOperation(other));
        }
    }
    public override ValueError muledTo(Value other)
    {
        if (other is Number _other)
        {
            List<Value> startElments = [.. elements];
            if (_other.value < 2)
            {
                return (_other.value < 1 ? new List([]) : new List(elements), NoError.Instance); // * 1 = original * 0 or less = []
            }

            for (int i = 2; i <= _other.value; i++)
            {
                elements.AddRange(startElments);
            }

            return (new List(elements), NoError.Instance);
        }
        else
        {
            return (ValueNull.Instance, IlligalOperation(other));
        }
    }

    public override ValueError getComparisonEQ(Value other)
    {
        if (other is List _other)
        {
            return (new Bool(elements == _other.elements), NoError.Instance);
        }
        else
        {
            return (ValueNull.Instance, IlligalOperation(other));
        }
    }
    public override ValueError getComparisonNE(Value other)
    {
        if (other is List _other)
        {
            return (new Bool(elements != _other.elements), NoError.Instance);
        }
        else
        {
            return (ValueNull.Instance, IlligalOperation(other));
        }
    }

    public override bool isTrue()
    {
        return elements.Count > 0;
    }
    public override Value copy()
    {
        List copy = new(elements);
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

