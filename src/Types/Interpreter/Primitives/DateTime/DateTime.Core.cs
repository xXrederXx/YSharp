namespace YSharp.Types.Interpreter.Primitives;

public sealed partial class VDateTime(DateTime? dateTime = null) : Value()
{
    public DateTime dateTime = dateTime ?? DateTime.Now;

    public override ValueAndError GetVar(string name) => propertyTable.Get(name, this);

    public override string ToString()
    {
        return dateTime.ToString("MM-dd-yyyyTHH:mm:ss.fffffff");
    }

    public override VDateTime Copy()
    {
        VDateTime copy = new(dateTime);
        copy.SetPos(startPos, endPos);
        copy.SetContext(context);
        return copy;
    }
}
