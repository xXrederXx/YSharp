using YSharp.Types.Interpreter.Internal;

namespace YSharp.Types.Interpreter.Primitives;

public sealed partial class VNumber(double value) : Value
{
    public double value { get; set; } = value;

    public override VNumber Copy()
    {
        VNumber copy = new(value);
        copy.SetPos(startPos, endPos);
        copy.SetContext(context);
        return copy;
    }

    public override ValueAndError GetFunc(string name, List<Value> argNodes) =>
        methodTable.Invoke(name, this, argNodes);

    public override bool IsTrue() => value != 0;

    // custom string representation
    public override string ToString() => $"{value}";
}