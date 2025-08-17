using YSharp.Types.Common;

namespace YSharp.Types.Interpreter.Internal;

public class MethodTable<T>
    where T : Value
{
    private readonly Dictionary<string, Func<T, List<Value>, ValueAndError>> Methods;

    public MethodTable(ReadOnlySpan<(string, Func<T, List<Value>, ValueAndError>)> methods)
    {
        Methods = [];
        foreach ((string, Func<T, List<Value>, ValueAndError>) method in methods)
        {
            Methods[method.Item1] = method.Item2;
        }
    }

    public ValueAndError Invoke(string name, T self, List<Value> args) =>
        Methods.TryGetValue(name, out var func)
            ? func(self, args)
            : (ValueNull.Instance, new FuncNotFoundError(Position.Null, name, self.context));
}
