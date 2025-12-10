using YSharp.Types.Interpreter.Internal;

namespace YSharp.Types.Interpreter.Collection;

public sealed partial class VList(List<Value> elements) : Value{
    public List<Value> value { get; set; } = elements;

    public override Value Copy()
    {
        VList copy = new(value);
        copy.SetPos(startPos, endPos);
        copy.SetContext(context);
        return copy;
    }

    public override ValueAndError GetFunc(string name, List<Value> argValues) =>
        methodTable.Invoke(name, this, argValues);

    public override ValueAndError GetVar(string name) => propertyTable.Get(name, this);

    public override bool IsTrue() => value.Count > 0;

    public override string ToString()
    {
        string res = "[";

        for (int i = 0; i < value.Count; i++)
        {
            Value v = value[i];
            res += v.ToString();
            if (i != value.Count - 1) res += ", ";
        }

        return res + "]";
    }
}