namespace YSharp.Types.Interpreter.Primitives;

public sealed partial class VString(string value) : Value
{
    public string value { get; set; } = value;

    public override ValueAndError GetFunc(string name, List<Value> argNodes) =>
        methodTable.Invoke(name, this, argNodes);

    public override ValueAndError GetVar(string name) => propertyTable.Get(name, this);

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
