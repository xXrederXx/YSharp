using YSharp.Types.Interpreter.Internal;

namespace YSharp.Types.Interpreter.Utils;

public sealed partial class VMath : Value{
    public override Value Copy() => new VMath();

    public override ValueAndError GetFunc(string name, List<Value> argNodes) =>
        methodTable.Invoke(name, this, argNodes);

    public override ValueAndError GetVar(string name) => propertyTable.Get(name, this);
}