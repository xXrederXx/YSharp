namespace YSharp.Types.Interpreter.Primitives;

public sealed partial class VBool(bool value) : Value()
{
    public bool value { get; set; } = value;

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
