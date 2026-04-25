using YSharp.Common;
using YSharp.Lexer;

namespace YSharp.Runtime.Utils.Math;


public sealed partial class VMath : Value
{
    public override Value Copy() => new VMath();

    public override Result<Value, Error> GetFunc(Token<string> nameToken, ReadOnlySpan<Value> argNodes) =>
        methodTable.Invoke(nameToken, this, argNodes);

    public override Result<Value, Error> GetVar(Token<string> nameToken) => propertyTable.Get(nameToken, this);
}
