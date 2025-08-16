namespace YSharp.Types.Interpreter.Utils;

public sealed partial class VMath : Value
{
    public override ValueAndError GetFunc(string name, List<Value> argNodes) =>
        methodTable.Invoke(name, this, argNodes);

    public override ValueAndError GetVar(string name) => propertyTable.Get(name, this);

    public override Value Copy()
    {
        return new VMath();
    }
}
