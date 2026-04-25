using YSharp.Common;

namespace YSharp.Runtime.Utils.Math;


public sealed partial class VMath : Value
{
    public override Value Copy() => new VMath();

    public override Result<Value, Error> GetFunc(string name, ReadOnlySpan<Value> argNodes) =>
        methodTable.Invoke(name, this, argNodes);

    public override Result<Value, Error> GetVar(string name) => propertyTable.Get(name, this);
}
