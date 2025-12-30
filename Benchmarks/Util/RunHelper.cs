using BenchmarkDotNet.Columns;
using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Diagnosers;
using BenchmarkDotNet.Exporters;
using BenchmarkDotNet.Exporters.Json;
using BenchmarkDotNet.Jobs;
using BenchmarkDotNet.Loggers;
using BenchmarkDotNet.Running;
using BenchmarkDotNet.Validators;

namespace YSharp.Benchmarks.Util;

public class RunHelper
{
    public enum Benches
    {
        Lexer,
        Parser,
        Interpreter,
        Runtime,
    }

    public record UserInput(Job Job, bool UseMemDiagnoser, Benches[] Benches);

    public static void Run(UserInput userInput)
    {
        var config = ManualConfig
            .CreateEmpty()
            .AddLogger(ConsoleLogger.Default)
            .AddColumnProvider(DefaultColumnProviders.Instance)
            .AddExporter(JsonExporter.Full)
            .AddJob(userInput.Job)
            .AddValidator(JitOptimizationsValidator.FailOnError);

        if (userInput.UseMemDiagnoser)
        {
            config = config.AddDiagnoser(MemoryDiagnoser.Default);
        }

        foreach (Benches bench in userInput.Benches)
        {
            RunBench(bench, config);
        }
    }

    private static void RunBench(Benches bench, IConfig config)
    {
        var summary = bench switch
        {
            Benches.Lexer => BenchmarkRunner.Run<LexerBench>(config),
            Benches.Parser => BenchmarkRunner.Run<ParserBench>(config),
            Benches.Interpreter => BenchmarkRunner.Run<InterpreterBench>(config),
            Benches.Runtime => BenchmarkRunner.Run<RunTimeBench>(config),
            _ => throw new ArgumentException(nameof(bench), "NOT VALIDE"),
        };
    }
}
