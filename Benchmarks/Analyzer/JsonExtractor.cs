using System.Text.Json;

namespace YSharp.Benchmarks.Analyzer;

public class JsonExtractor
{
    public const string NewDataPath = @"BenchmarkDotNet.Artifacts\results";
    public const string DataPath = @"..\Docs\Benchmarks\Data";

    public static List<BenchmarksData> LoadNewDatas() => LoadData(GetJsonFiles(NewDataPath));

    public static List<BenchmarksData> LoadAllDatas() => LoadData(GetJsonFiles(DataPath));

    private static string[] GetJsonFiles(string directoryPath) =>
        Directory.GetFiles(directoryPath).Where(x => x.EndsWith(".json")).ToArray();

    public static void SaveData(List<BenchmarksData> data)
    {
        foreach (BenchmarksData bench in data)
        {
            File.WriteAllText(
                Path.Combine(DataPath, bench.Title + ".json"),
                JsonSerializer.Serialize(bench, JsonSerializerOptions.Default)
            );
        }
    }

    private static List<BenchmarksData> LoadData(string[] paths)
    {
        List<BenchmarksData> benchmarkDatas = new List<BenchmarksData>(paths.Length);
        foreach (string path in paths)
        {
            string content = File.ReadAllText(path);
            BenchmarksData? data = JsonSerializer.Deserialize<BenchmarksData>(content);

            if (data is null)
            {
                System.Console.WriteLine($"Data for {path} is null");
                continue;
            }
            benchmarkDatas.Add(data);
        }
        return benchmarkDatas;
    }
}
