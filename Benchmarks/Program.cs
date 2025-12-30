using YSharp.Benchmarks.Analyzer;
using YSharp.Benchmarks.Util;

public class Program
{
    static void Main(string[] args)
    {
        string gitHash = args.Length >= 1 ? args[0] : "N/A";
        var config = UserRequester.Request();
        RunHelper.Run(config);

        List<BenchmarksData> newData = JsonExtractor.LoadNewDatas();
        newData =  newData.Select(x => new BenchmarksData(x.Title, gitHash, dateTime, x.Benchmarks)).ToList();
        JsonExtractor.SaveData(newData);

        foreach (BenchmarksData bench in newData)
        {
            MarkdownExporter.ExportMarkdown(bench);
        }
    }
}
