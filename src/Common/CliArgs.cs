using CommandLine;

namespace YSharp.Common;

public class CliArgs : IEquatable<CliArgs>{
    [Option('o', "optimization", Required = false, Default = 0)]
    public int Optimization { get; set; }

    [Option(
        'd',
        "dot",
        Required = false,
        HelpText = "This flag is used render a DOT graph of the AST"
    )]
    public bool RenderDot { get; set; }

    [Option(
        'p',
        "path",
        Required = false,
        HelpText = "Specify a path to a file which will be executed",
        Default = null
    )]
    public string? ScriptPath { get; set; }

    public override int GetHashCode()
    {
        return HashCode.Combine(Optimization, RenderDot, ScriptPath);
    }

    public override bool Equals(object? obj)
    {
        if (obj is not CliArgs cliArgs)
            return false;
        return Equals(cliArgs);
    }

    public bool Equals(CliArgs? other)
    {
        if (other is null)
            return false;

        return Optimization == other.Optimization
               && RenderDot == other.RenderDot
               && ScriptPath == other.ScriptPath;
    }

    public override string ToString()
    {
        return $"Optimize({Optimization}) RenderDot({RenderDot}) ScriptPath({ScriptPath})";
    }

    public static readonly CliArgs DefaultArgs = new CliArgs()
    {
        Optimization = 0,
        RenderDot = false,
        ScriptPath = null,
    };

    public static readonly CliArgs ArgsNoOptimization = new CliArgs()
    {
        Optimization = 0,
        RenderDot = false,
        ScriptPath = null
    };

    public static readonly CliArgs ArgsWithOptimization = new CliArgs()
    {
        Optimization = 1,
        RenderDot = false,
        ScriptPath = null
    };
}