using YSharp.Benchmarks.Analyzer;
using YSharp.Benchmarks.Util;

public class Program
{
    static void Main(string[] args)
    {
        string gitHash = args.Length >= 1 ? args[0] : "N/A";
        var config = UserRequester.Request();
        RunHelper.Run(config);

        var newData = JsonExtractor.LoadNewDatas();
    }
}
