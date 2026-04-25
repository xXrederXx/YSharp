using YSharp.Common;

namespace YSharp.Runtime;

public class MethodTable<T>
    where T : Value
{
    private readonly Dictionary<string, ValueMethod> _methods;
    public delegate Result<Value, Error> ValueMethod(T self, ReadOnlySpan<Value> args);

    public MethodTable(ReadOnlySpan<(string, ValueMethod)> methods)
    {
        _methods = [];
        foreach ((string, ValueMethod) method in methods)
            _methods[method.Item1] = method.Item2;
    }

    public Result<Value, Error> Invoke(string name, T self, ReadOnlySpan<Value> args) =>
        _methods.TryGetValue(name, out ValueMethod? func)
            ? func.Invoke(self, args)
            : Result<Value, Error>.Fail(new FuncNotFoundError(Position.Null, name, self.Context));
}
