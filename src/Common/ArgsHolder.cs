namespace YSharp.Common;

public class ArgsHolder
{
    public static readonly CliArgs DefaultArgs = new CliArgs()
    {
        Optimization = 0,
        RenderDot = false,
        ScriptPath = null,
    };
    public static CliArgs UserArgs
    {
        get;
        set
        {
            if (field != DefaultArgs)
                throw new InvalidOperationException("You can only set it once");
            field = value;
        }
    } = DefaultArgs;
}
