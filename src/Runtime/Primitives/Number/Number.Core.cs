using YSharp.Common;
using YSharp.Lexer;

namespace YSharp.Runtime.Primitives.Number;

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

    public override Result<Value, Error> GetFunc(Token<string> nameToken, ReadOnlySpan<Value> argNodes) =>
        methodTable.Invoke(nameToken, this, argNodes);

    public override bool IsTrue() => value != 0;

    // custom string representation
    public override string ToString() => value.ToString(StaticConfig.numberCulture);
}
