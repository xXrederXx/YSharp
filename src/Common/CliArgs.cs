using CommandLine;

namespace YSharp.Common;

public class CliArgs
{
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
}
