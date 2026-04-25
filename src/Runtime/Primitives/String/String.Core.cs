using YSharp.Common;
using YSharp.Lexer;

namespace YSharp.Runtime.Primitives.String;

public sealed partial class VString(string value) : Value
{
    public string value { get; set; } = value;

    public override Value Copy()
    {
        VString copy = new(value);
        copy.SetPos(StartPos, EndPos);
        copy.SetContext(Context);
        return copy;
    }

    public override Result<Value, Error> GetFunc(Token<string> nameToken, ReadOnlySpan<Value> argNodes) =>
        methodTable.Invoke(nameToken, this, argNodes);

    public override Result<Value, Error> GetVar(Token<string> nameToken) => propertyTable.Get(nameToken, this);

    public override bool IsTrue() => value.Length > 0;

    public override string ToString() => $"\"{value}\"";
}
