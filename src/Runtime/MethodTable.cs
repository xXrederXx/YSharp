using YSharp.Common;

namespace YSharp.Runtime;

public class MethodTable<T>
    where T : Value
{
    private readonly Dictionary<string, Func<T, List<Value>, Result<Value, Error>>> Methods;

    public MethodTable(ReadOnlySpan<(string, Func<T, List<Value>, Result<Value, Error>>)> methods)
    {
        Methods = [];
        foreach ((string, Func<T, List<Value>, Result<Value, Error>>) method in methods)
            Methods[method.Item1] = method.Item2;
    }

    public Result<Value, Error> Invoke(string name, T self, List<Value> args) =>
        Methods.TryGetValue(name, out Func<T, List<Value>, Result<Value, Error>>? func)
            ? func(self, args)
            : Result<Value, Error>.Fail(new FuncNotFoundError(Position.Null, name, self.Context));
}
