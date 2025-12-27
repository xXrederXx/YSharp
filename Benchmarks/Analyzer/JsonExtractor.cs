using System.Text.Json;

namespace YSharp.Benchmarks.Analyzer;

public class JsonExtractor
{
    const string NewDataPath = @"BenchmarkDotNet.Artifacts\results";
    const string DataPath = @"BenchmarkDotNet.Artifacts\results";

    private static string[] GetJsonFiles() =>
        Directory.GetFiles(NewDataPath).Where(x => x.EndsWith(".json")).ToArray();

    public static void LoadData()
    {
        foreach (string path in GetJsonFiles())
        {
            string content = File.ReadAllText(path);
            BenchmarksData? data = JsonSerializer.Deserialize<BenchmarksData>(content);

            if (data is null)
            {
                System.Console.WriteLine($"Data for {path} is null");
                continue;
            }
            File.WriteAllText(Path.Combine(NewDataPath, data.Title + ".json"), JsonSerializer.Serialize(data));
        }
    }
}
