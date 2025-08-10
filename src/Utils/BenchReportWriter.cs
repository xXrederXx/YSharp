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
        {
            Directory.CreateDirectory(MdFolder);
        }

        if (!File.Exists(SummaryPath))
        {
            File.Create(SummaryPath).Dispose();
        }

        if (!File.Exists(HistoryPath))
        {
            File.Create(HistoryPath).Dispose();
        }
    }

    public static void UpdateFiles<T>(string changeDescription)
    {
        UpdateDataFiles<T>();
    }

    private static void UpdateDataFiles<T>()
    {
        BenchData[] newData = GetLatestData<T>();
        BenchData[] oldData;
        BenchData[] bestTimeData;
        BenchData[] bestMemData;

        string NewPath = NewDataPath<T>();
        string OldPath = OldDataPath<T>();
        string bestTimePath = BestTimeDataPath<T>();
        string BestMemPath = BestMemDataPath<T>();

        CheckFileExist(NewPath);
        CheckFileExist(OldPath);
        CheckFileExist(bestTimePath);
        CheckFileExist(BestMemPath);

        // Get Old Data
        using (var reader = new StreamReader(NewPath))
        {
            oldData = StringToBenchDataList(reader.ReadToEnd());
        }

        using (var reader = new StreamReader(bestTimePath))
        {
            string text = reader.ReadToEnd();
            if (text == string.Empty)
            {
                bestTimeData = newData;
            }
            else
            {
                bestTimeData = StringToBenchDataList(text);
            }
        }

        using (var reader = new StreamReader(BestMemPath))
        {
            string text = reader.ReadToEnd();
            if (text == string.Empty)
            {
                bestMemData = newData;
            }
            else
            {
                bestMemData = StringToBenchDataList(text);
            }
        }

        if (ValueToDouble(newData.Last().Mean) < ValueToDouble(bestTimeData.Last().Mean))
        {
            bestTimeData = newData;
        }
        if (ValueToDouble(newData.Last().Allocated) < ValueToDouble(bestTimeData.Last().Allocated))
        {
            bestMemData = newData;
        }

        // Write new data
        using (var writer = new StreamWriter(OldPath))
        {
            writer.Write(BenchDataListToString(oldData));
        }

        using (var writer = new StreamWriter(NewPath))
        {
            writer.Write(BenchDataListToString(newData));
        }

        using (var writer = new StreamWriter(bestTimePath))
        {
            writer.Write(BenchDataListToString(bestTimeData));
        }

        using (var writer = new StreamWriter(BestMemPath))
        {
            writer.Write(BenchDataListToString(bestMemData));
        }
    }

    private static double ValueToDouble(string input)
    {
        string numericPart = new string(
            input.TakeWhile(c => char.IsDigit(c) || c == '.' || c == ',').ToArray()
        );
        return double.Parse(numericPart, System.Globalization.CultureInfo.InvariantCulture);
    }

    private static void CheckFileExist(string path)
    {
        if (!File.Exists(path))
        {
            File.Create(path).Dispose();
        }
    }

    private static string BenchDataListToString(BenchData[] datas)
    {
        string str = "Method;Mean;Error;StdDev;Gen0;Gen1;Gen2;Allocated\n";
        foreach (BenchData bench in datas)
        {
            str += bench.ToString();
        }
        return str;
    }

    private static BenchData[] StringToBenchDataList(string data)
    {
        CsvConfiguration csvConfig = new CsvConfiguration(CultureInfo.CurrentCulture)
        {
            HasHeaderRecord = true,
        };
        BenchData[] records;
        using (var reader = new StringReader(data))
        using (var csv = new CsvReader(reader, csvConfig))
        {
            records = csv.GetRecords<BenchData>().ToArray();
        }

        return records;
    }

    private static BenchData[] GetLatestData<T>()
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
        string text;
        using (var reader = new StreamReader(path))
        {
            text = reader.ReadToEnd();
        }

        BenchData[] records = StringToBenchDataList(text);
        System.Console.WriteLine(string.Join(", ", records.ToList()));
        return records;
    }

    private static string BestTimeDataPath<T>() =>
        Path.Combine(DataFolder, "best-time-" + typeof(T).Name + ".csv");

    private static string BestMemDataPath<T>() =>
        Path.Combine(DataFolder, "best-Mem-" + typeof(T).Name + ".csv");

    private static string NewDataPath<T>() =>
        Path.Combine(DataFolder, "new-" + typeof(T).Name + ".csv");

    private static string OldDataPath<T>() =>
        Path.Combine(DataFolder, "old-" + typeof(T).Name + ".csv");
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
