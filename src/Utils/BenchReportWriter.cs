using System.Globalization;
using System.Reflection;
using System.Text.RegularExpressions;
using BenchmarkDotNet.Environments;
using CsvHelper;
using CsvHelper.Configuration;

namespace YSharp.Utils;

public static class BenchReportWriter
{
    public const string MdFolder = "./Docs/Benchmarks";
    public const string DataFolder = "./Docs/Benchmarks/Data";
    private static string SummaryPath => Path.Combine(MdFolder, "benchmarks_summary.md");
    private static string HistoryPath => Path.Combine(MdFolder, "benchmarks_history.md");

    static BenchReportWriter()
    {
        if (!Directory.Exists(MdFolder))
            Directory.CreateDirectory(MdFolder);

        if (!File.Exists(SummaryPath))
            File.Create(SummaryPath).Dispose();

        if (!File.Exists(HistoryPath))
            File.Create(HistoryPath).Dispose();
    }

    public static void UpdateFiles<T>(string changeDescription)
    {
        UpdateDataFiles<T>();
    }

    private static void UpdateDataFiles<T>()
    {
        var newData = GetData<T>();

        string oldData;
        string NewPath = NewDataPath<T>();
        string OldPath = OldDataPath<T>();

        if (!File.Exists(NewPath))
            File.Create(NewPath).Dispose();
        if (!File.Exists(OldPath))
            File.Create(OldPath).Dispose();

        using (var reader = new StreamReader(NewPath))
            oldData = reader.ReadToEnd();

        using (var writer = new StreamWriter(OldPath))
            writer.Write(oldData);

        using (var writer = new StreamWriter(NewPath))
            writer.Write(ListToString(newData));
    }

    private static string NewDataPath<T>() =>
        Path.Combine(DataFolder, "new-" + typeof(T).Name + ".csv");

    private static string OldDataPath<T>() =>
        Path.Combine(DataFolder, "old-" + typeof(T).Name + ".csv");

    private static string ListToString(BenchData[] datas)
    {
        string str = "Method;Mean;Error;StdDev;Gen0;Gen1;Gen2;Allocated\n";
        foreach (BenchData bench in datas)
        {
            str += bench.ToString();
        }
        return str;
    }

    private static BenchData[] GetData<T>()
    {
        string path = Path.Combine(
            "BenchmarkDotNet.Artifacts",
            "results",
            $"{typeof(T)}-report.csv"
        );
        System.Console.WriteLine(path);
        if (!File.Exists(path))
        {
            return [];
        }

        CsvConfiguration csvConfig = new CsvConfiguration(CultureInfo.CurrentCulture)
        {
            HasHeaderRecord = true,
        };
        BenchData[] records;
        using (var reader = new StreamReader(path))
        using (var csv = new CsvReader(reader, csvConfig))
        {
            records = csv.GetRecords<BenchData>().ToArray();
        }
        System.Console.WriteLine(string.Join(", ", records.ToList()));
        return records;
    }
}

record BenchData(
    string Method,
    string Mean,
    string Error,
    string StdDev,
    string Gen0,
    string Gen1,
    string Gen2,
    string Allocated
)
{
    public override string ToString() =>
        $"{Method};{Mean};{Error};{StdDev};{Gen0};{Gen1};{Gen2};{Allocated}\n";
}
