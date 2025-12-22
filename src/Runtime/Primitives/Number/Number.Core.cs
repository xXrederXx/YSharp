namespace YSharp.Runtime.Primatives.Number;

public sealed partial class VNumber(double value) : Value
{
    public double value { get; set; } = value;

    public override VNumber Copy()
    {
        VNumber copy = new(value);
        copy.SetPos(StartPos, EndPos);
        copy.SetContext(Context);
        return copy;
    }

    public override ValueAndError GetFunc(string name, List<Value> argNodes) =>
        methodTable.Invoke(name, this, argNodes);

    public override bool IsTrue() => value != 0;

    // custom string representation
    public override string ToString() => $"{value}";
}