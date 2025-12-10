using System.Globalization;
using System.Text;
using CsvHelper;
using CsvHelper.Configuration;

namespace YSharp.Utils;

public static class BenchReportWriter
{
    static BenchReportWriter()
    {
        if (!Directory.Exists(MdFolder)) Directory.CreateDirectory(MdFolder);
        if (!Directory.Exists(DataFolder)) Directory.CreateDirectory(DataFolder);
    }

    public static void UpdateFiles<T>(string changeDescription)
    {
        UpdateDataFiles<T>();
        UpdateMdFile<T>();
    }

    private static string BenchDataListToMdString(BenchData[] datas)
    {
        string str = BenchData.MDHeader;
        foreach (BenchData bench in datas) str += bench.ToMDString();
        return str;
    }

    private static string BenchDataListToString(BenchData[] datas)
    {
        string str = BenchData.CSVHeader;
        foreach (BenchData bench in datas) str += bench.ToCSVString();
        return str;
    }

    private static string BestMemDataPath<T>() =>
        Path.Combine(DataFolder, "best-Mem-" + typeof(T).Name + ".csv");

    private static string BestTimeDataPath<T>() =>
        Path.Combine(DataFolder, "best-time-" + typeof(T).Name + ".csv");

    private static BenchData[] CalcTableDifference(BenchData[] latestData, BenchData[] oldData)
    {
        int count = latestData.Length < oldData.Length ? latestData.Length : oldData.Length;
        BenchData[] differenceData = new BenchData[count];
        for (int i = 0; i < count; i++)
        {
            BenchData old = oldData[i];
            BenchData latest = latestData[i];
            BenchData diff = new()
            {
                Method = latest.Method,
                Mean = (ValueToDouble(latest.Mean) - ValueToDouble(old.Mean)).ToString("F2") + "us",
                Error =
                    (ValueToDouble(latest.Error) - ValueToDouble(old.Error)).ToString("F2") + "us",
                StdDev =
                    (ValueToDouble(latest.StdDev) - ValueToDouble(old.StdDev)).ToString("F2")
                    + "us",
                Gen0 = (ValueToDouble(latest.Gen0) - ValueToDouble(old.Gen0)).ToString("F2"),
                Gen1 = (ValueToDouble(latest.Gen1) - ValueToDouble(old.Gen1)).ToString("F2"),
                Gen2 = (ValueToDouble(latest.Gen2) - ValueToDouble(old.Gen2)).ToString("F2"),
                Allocated =
                    (ValueToDouble(latest.Allocated) - ValueToDouble(old.Allocated)).ToString("F2")
                    + "KB"
            };
            differenceData[i] = diff;
        }

        return differenceData;
    }

    private static void CheckFileExist(string path)
    {
        if (!File.Exists(path)) File.Create(path).Dispose();
    }

    private static BenchData[] GetLatestData<T>()
    {
        string path = Path.Combine(
            "BenchmarkDotNet.Artifacts",
            "results",
            $"{typeof(T)}-report.csv"
        );
        Console.WriteLine(path);
        if (!File.Exists(path)) return [];
        string text;
        using (StreamReader reader = new(path))
        {
            text = reader.ReadToEnd();
        }

        BenchData[] records = StringToBenchDataList(text);
        return records;
    }

    private static string NewDataPath<T>() =>
        Path.Combine(DataFolder, "new-" + typeof(T).Name + ".csv");

    private static string OldDataPath<T>() =>
        Path.Combine(DataFolder, "old-" + typeof(T).Name + ".csv");

    private static BenchData[] StringToBenchDataList(string data)
    {
        CsvConfiguration _csvConfig = new(CultureInfo.CurrentCulture)
        {
            HasHeaderRecord = true,
            HeaderValidated = null,
            MissingFieldFound = null,
            ReadingExceptionOccurred = null
        };
        BenchData[] records;
        using (StringReader reader = new(data))
        using (CsvReader csv = new(reader, _csvConfig))
        {
            records = csv.GetRecords<BenchData>().ToArray();
        }

        return records;
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
        using (StreamReader reader = new(NewPath))
        {
            oldData = StringToBenchDataList(reader.ReadToEnd());
        }

        using (StreamReader reader = new(bestTimePath))
        {
            string text = reader.ReadToEnd();
            if (text == string.Empty)
                bestTimeData = newData;
            else
                bestTimeData = StringToBenchDataList(text);
        }

        using (StreamReader reader = new(BestMemPath))
        {
            string text = reader.ReadToEnd();
            if (text == string.Empty)
                bestMemData = newData;
            else
                bestMemData = StringToBenchDataList(text);
        }

        if (ValueToDouble(newData.Last().Mean) < ValueToDouble(bestTimeData.Last().Mean)) bestTimeData = newData;
        if (ValueToDouble(newData.Last().Allocated) < ValueToDouble(bestTimeData.Last().Allocated))
            bestMemData = newData;

        // Write new data
        using (StreamWriter writer = new(OldPath))
        {
            writer.Write(BenchDataListToString(oldData));
        }

        using (StreamWriter writer = new(NewPath))
        {
            writer.Write(BenchDataListToString(newData));
        }

        using (StreamWriter writer = new(bestTimePath))
        {
            writer.Write(BenchDataListToString(bestTimeData));
        }

        using (StreamWriter writer = new(BestMemPath))
        {
            writer.Write(BenchDataListToString(bestMemData));
        }
    }

    private static void UpdateMdFile<T>()
    {
        BenchData[] latestData;
        using (StreamReader reader = new(NewDataPath<T>()))
        {
            latestData = StringToBenchDataList(reader.ReadToEnd());
        }

        BenchData[] oldData;
        using (StreamReader reader = new(OldDataPath<T>()))
        {
            oldData = StringToBenchDataList(reader.ReadToEnd());
        }

        BenchData[] bestMemData;
        using (StreamReader reader = new(BestMemDataPath<T>()))
        {
            bestMemData = StringToBenchDataList(reader.ReadToEnd());
        }

        BenchData[] bestTimeData;
        using (StreamReader reader = new(BestTimeDataPath<T>()))
        {
            bestTimeData = StringToBenchDataList(reader.ReadToEnd());
        }

        BenchData[] differenceDataMem = CalcTableDifference(latestData, bestMemData);
        BenchData[] differenceDataTime = CalcTableDifference(latestData, bestTimeData);

        StringBuilder sb = new();
        sb.AppendLine($"# {typeof(T).Name} Benchmark Summary");
        sb.AppendLine();
        sb.AppendLine($"## Latest Bench Results ({DateTime.Now:dd-MM-yyyy})");
        sb.AppendLine();
        sb.Append(BenchDataListToMdString(latestData));
        sb.AppendLine();
        sb.AppendLine("## Differenc To Best Time");
        sb.AppendLine();
        sb.Append(BenchDataListToMdString(differenceDataTime));
        sb.AppendLine();
        sb.AppendLine("## Differenc To Best Memory");
        sb.AppendLine();
        sb.Append(BenchDataListToMdString(differenceDataMem));

        string path = Path.Combine(MdFolder, $"{typeof(T).Name}-bench-summary.md");
        using (StreamWriter writer = new(path))
        {
            writer.Write(sb.ToString());
        }
    }

    private static double ValueToDouble(string input)
    {
        string numericPart = new(
            input.TakeWhile(c => char.IsDigit(c) || c == '.' || c == ',').ToArray()
        );
        double value = double.Parse(numericPart, CultureInfo.InvariantCulture);

        if (input.Contains("MB"))
            return value * 1024;
        if (input.Contains("ms"))
            return value * 1000;
        return value;
    }

    public const string MdFolder = "./Docs/Benchmarks";
    public const string DataFolder = "./Docs/Benchmarks/Data";
}

internal record BenchData
{
    public string Allocated { get; init; } = "0 KB";
    public string Error { get; init; } = "0 us";
    public string Gen0 { get; init; } = "0";
    public string Gen1 { get; init; } = "0";
    public string Gen2 { get; init; } = "0";
    public string Mean { get; init; } = "0 us";
    public string Method { get; init; } = "";
    public string StdDev { get; init; } = "0 us";

    public string ToCSVString() =>
        $"{Method};{Mean};{Error};{StdDev};{Gen0};{Gen1};{Gen2};{Allocated}\n";

    public string ToMDString() =>
        $"|{Method}|{Mean}|{Error}|{StdDev}|{Gen0}|{Gen1}|{Gen2}|{Allocated}|\n";

    public const string CSVHeader = "Method;Mean;Error;StdDev;Gen0;Gen1;Gen2;Allocated\n";

    public const string MDHeader =
        "|Method|Mean|Error|StdDev|Gen0|Gen1|Gen2|Allocated|\n|----------------------- |----------:|---------:|---------:|---------:|--------:|-----------:|-----------:|\n";
}