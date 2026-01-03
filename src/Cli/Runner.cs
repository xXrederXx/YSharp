using YSharp.Common;
using YSharp.Runtime;
using YSharp.Util;

namespace YSharp.Cli;

using RunResult = Result<Value, Error>;

public static class Runner
{
    /// <summary>
    /// This function starts emulating a sort of shell for the user.
    /// </summary>
    public static void ConsoleRunner()
    {
        RunClass runClass = new();
        Console.WriteLine("Type 'b' anytime to break");

        while (true)
        {
            string inp = Console.ReadLine() ?? string.Empty;

            if (inp == "b")
                break;

            if (inp.Trim() == string.Empty)
                continue;

            RunResult res = runClass.Run("<stdin>", inp); // run the app

            if (res.IsFailed)
                Console.WriteLine(res.GetError());
        }
    }

    /// <summary>
    /// This function is used to execute a specific script.
    /// </summary>
    /// <param name="path">The absolute or relative path to the script which should be executed</param>
    public static void ScriptRunner(string path)
    {
        RunClass runClass = new();
        // intrnr = internal runner
        RunResult res = runClass.Run("<intrnr>", $"RUN(\"{path.Replace("\\", "\\\\")}\")"); // run the app
        if (res.IsFailed)
            Console.WriteLine(res.GetError());
    }
}
