using YSharp.Common;
using YSharp.Runtime;

namespace YSharp.Tests;

public class MockRuntime : IRuntimeEnviroment
{
    public Result<Value, Error> toReturn;
    public Result<Value, Error> Run(string fn, string text, CliArgs args)
    {
        return toReturn;
    }
}
