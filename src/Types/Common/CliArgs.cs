using CommandLine;

namespace YSharp.Types.Common;
public class CliArgs
{
    [Option(
        'b',
        "run-bench",
        Required = false,
        HelpText = "This flag is used to run the benchmarks"
    )]
    public bool RunBench { get; set; }

    [Option('t', "run-test", Required = false, HelpText = "This flag is used to run the tests")]
    public bool RunTest { get; set; }

    [Option(
        'd',
        "dot",
        Required = false,
        HelpText = "This flag is used render a DOT graph of the AST"
    )]
    public bool RenderDot { get; set; }

    [Option('o', "optimization", Required = false, Default = 0)]
    public int Optimization { get; set; }
}