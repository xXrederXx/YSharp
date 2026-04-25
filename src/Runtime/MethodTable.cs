using YSharp.Common;
using YSharp.Lexer;

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

    public Result<Value, Error> Invoke(Token<string> nameToken, T self, ReadOnlySpan<Value> args) =>
        _methods.TryGetValue(nameToken.Value, out ValueMethod? func)
            ? func.Invoke(self, args)
            : Result<Value, Error>.Fail(
                new FuncNotFoundError(nameToken.StartPos, nameToken.Value, self.Context)
            );
}
