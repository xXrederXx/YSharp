using YSharp.Common;
using YSharp.Runtime;

namespace YSharp.Runtime;

public interface IRuntimeEnviroment
{
    public Result<Value, Error> Run(string fn, string text, CliArgs args);
}
