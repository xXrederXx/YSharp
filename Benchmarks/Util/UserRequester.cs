using BenchmarkDotNet.Analysers;
using BenchmarkDotNet.Diagnosers;
using BenchmarkDotNet.Jobs;
using BenchmarkDotNet.Running;
using Spectre.Console;

namespace YSharp.Benchmarks.Util;

public class UserRequester
{
    public static RunHelper.UserInput Request()
    {
        Job job = AnsiConsole.Prompt(
            new SelectionPrompt<Job>()
                .Title("What Job should be used")
                .AddChoices(Job.ShortRun, Job.MediumRun, Job.LongRun)
        );

        bool useMemDiagnoser = AnsiConsole.Confirm("Use MemoryDiagnoser");

        List<RunHelper.Benches> benches = AnsiConsole.Prompt(
            new MultiSelectionPrompt<RunHelper.Benches>()
                .Title("Select which to run")
                .AddChoices(
                    RunHelper.Benches.Interpreter,
                    RunHelper.Benches.Lexer,
                    RunHelper.Benches.Parser,
                    RunHelper.Benches.Runtime
                )
        );

        return new RunHelper.UserInput(job, useMemDiagnoser, benches.ToArray());
    }
}
