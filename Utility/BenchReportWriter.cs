using System;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using BenchmarkDotNet.Environments;
using BenchmarkDotNet.Exporters;

namespace YSharp.Utility;

public static class BenchReportWriter
{
    public const string Folder = "./Documentation/Benchmarks";
    private static string SummaryPath => Path.Combine(Folder, "benchmarks_summary.md");
    private static string HistoryPath => Path.Combine(Folder, "benchmarks_history.md");
    private static string DetailsPath => Path.Combine(Folder, "benchmarks_details.md");

    static BenchReportWriter()
    {
        if (!Directory.Exists(Folder))
            Directory.CreateDirectory(Folder);

        if (!File.Exists(SummaryPath))
            File.Create(SummaryPath);

        if (!File.Exists(HistoryPath))
            File.Create(HistoryPath);

        if (!File.Exists(DetailsPath))
            File.Create(DetailsPath);
    }

    public static void UpdateFiles<T>(
        BenchmarkDotNet.Reports.Summary summary,
        string changeDescription
    )
    {
        UpdateSummary<T>(summary, changeDescription);
        UpdateHistory<T>(summary, changeDescription);
        UpdateDetails<T>(out int UsedVersion);
        System.Console.WriteLine(UsedVersion);
    }

    private static void UpdateSummary<T>(
        BenchmarkDotNet.Reports.Summary summary,
        string changeDescription
    ) { }

    private static void UpdateHistory<T>(
        BenchmarkDotNet.Reports.Summary summary,
        string changeDescription
    ) { }

    private static void UpdateDetails<T>(out int UsedVersion)
    {
        string oldText = File.Exists(DetailsPath) ? File.ReadAllText(DetailsPath) : "";
        if (oldText == "")
        {
            oldText = "# Details\n";
        }
        string VersionRegex = @"## V(\d+)";
        RegexOptions options = RegexOptions.Multiline;
        Match m = Regex.Match(oldText, VersionRegex, options);

        int lastVersion = 0;
        int lastVersionIndex = 0;

        if (m.Success)
        {
            lastVersion = int.Parse(m.Groups[1].Value);
            lastVersionIndex = m.Index;
        }

        string envInfo = HostEnvironmentInfo.GetInformation();
        IEnumerable<CustomAttributeData> attributes = typeof(T).CustomAttributes;
        string newDetails = GenerateDetailEntry(envInfo, attributes);

        if (oldText.Contains(newDetails))
        {
            // Find most recent matching version
            MatchCollection matches = Regex.Matches(oldText, VersionRegex, options);
            for (int i = 0; i < matches.Count; i++)
            {
                int versionStart = matches[i].Index;
                int nextVersion;
                if (i + 1 < matches.Count)
                {
                    nextVersion = matches[i + 1].Index;
                }
                else
                {
                    nextVersion = oldText.Length;
                }

                string block = oldText.Substring(versionStart, nextVersion - versionStart);
                if (block.Contains(newDetails))
                {
                    UsedVersion = int.Parse(matches[i].Groups[1].Value);
                    return;
                }
            }
        }

        // New version needed
        UsedVersion = lastVersion + 1;
        string insertBlock = $"\n\n## V{UsedVersion}\n\n{newDetails.Trim()}\n";

        if (oldText.Length == 0)
            oldText = insertBlock;
        else
            oldText = oldText.Insert(lastVersionIndex, insertBlock);

        File.WriteAllText(DetailsPath, oldText);
    }

    private static string GenerateDetailEntry(
        string envInfo,
        IEnumerable<CustomAttributeData> attributes
    )
    {
        string text =
            "\n```\n"
            + envInfo
            + "\n```\n"
            + "Attributes ```"
            + string.Join(", ", attributes.Select(x => x.AttributeType.Name).ToList())
                .Replace("Attribute", "")
            + "```\n";
        return text;
    }
}
