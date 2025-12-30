using System.Globalization;
using Grynwald.MarkdownGenerator;

namespace YSharp.Benchmarks.Exporter;

public class MarkdownExporter
{
    private const string MdDirectory = @"..\Docs\Benchmarks\Summaries";

    public static void ExportMarkdown(BenchmarksData data)
    {
        string benchName = ExtractTitleName(data.Title);

        MdDocument doc = new MdDocument();
        doc.Root.Add(new MdHeading(1, $"{benchName} Results"));

        doc.Root.Add(new MdHeading(2, $"Metadata"));
        doc.Root.Add(
            new MdBulletList(
                new MdListItem(
                    new MdTextSpan("Git Commit: "),
                    new MdLinkSpan(
                        data.GitHash.Substring(0, 7),
                        $"https://github.com/xXrederXx/YSharp/commit/{data.GitHash}"
                    )
                ),
                new MdListItem($"Recorded At: {data.DateTime}")
            )
        );

        doc.Root.Add(new MdHeading(2, "Results"));
        doc.Root.Add(GenerateResultTable(data.Benchmarks));

        doc.Save(Path.Combine(MdDirectory, benchName + ".md"));
    }

    private static MdTable GenerateResultTable(BenchmarkData[] data)
    {
        MdTableRow headerRow = new MdTableRow(
            "Method",
            "Time Mean",
            "Aprox. Error",
            "Allocated",
            "Gen 0",
            "Gen 1",
            "Gen 2"
        );
        List<MdTableRow> rows = data.Select(bench => new MdTableRow(
                bench.Method,
                ConvertTime(bench.Statistics.Mean),
                ConvertTime(ApproximateError(bench)),
                ConvertAllocated(GetMetric(bench.Metrics, "Allocated")),
                ConvertGC(GetMetric(bench.Metrics, "Gen0")),
                ConvertGC(GetMetric(bench.Metrics, "Gen1")),
                ConvertGC(GetMetric(bench.Metrics, "Gen2"))
            ))
            .ToList();
        return new MdTable(headerRow, rows);
    }

    private static double ApproximateError(BenchmarkData data) =>
        data.Statistics.Mean - data.Statistics.ConfidenceInterval.Lower;

    private static string ExtractTitleName(string title) =>
        string.Concat(title.Replace("YSharp.Benchmarks.", string.Empty).TakeWhile(char.IsLetter));

    private static string ConvertTime(double timeNs) =>
        timeNs.ToString("N0", CultureInfo.InvariantCulture) + " ns";

    private static string ConvertAllocated(double allocatedByte) =>
        (allocatedByte / 1000).ToString("N0", CultureInfo.InvariantCulture) + " kb";

    private static string ConvertGC(double gc) => gc.ToString("N0", CultureInfo.InvariantCulture);

    private static double GetMetric(BenchmarkMetricData[] metrics, string id)
    {
        foreach (BenchmarkMetricData metric in metrics)
        {
            if (metric.Descriptor.DisplayName == id)
                return metric.Value;
        }
        return -1;
    }
}
