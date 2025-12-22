namespace YSharp.Runtime.Primatives.Bool;


public sealed partial class VBool(bool value) : Value
{
    public bool value { get; set; } = value;

    public override VBool Copy()
    {
        VBool copy = new(value);
        copy.SetPos(StartPos, EndPos);
        copy.SetContext(Context);
        return copy;
    }

    public override bool IsTrue() => value;

    public override string ToString() => value.ToString();
}