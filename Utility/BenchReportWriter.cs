using System;
using System.Globalization;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using BenchmarkDotNet.Environments;
using BenchmarkDotNet.Exporters;
using BenchmarkDotNet.Reports;

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
        Summary summary,
        string changeDescription
    )
    {
        UpdateDetails<T>(out int UsedVersion);
        UpdateHistory<T>(changeDescription, UsedVersion);
        UpdateSummary<T>(summary, changeDescription);
        System.Console.WriteLine(UsedVersion);
    }

    private static void UpdateSummary<T>(
        Summary summary,
        string changeDescription
    ) { }

    private static void UpdateHistory<T>(string changeDescription, int UsedVersion)
    {
        string oldText = File.ReadAllText(HistoryPath);

        string BenchTitle = "## " + typeof(T).Name.Replace("Bench", "");
        string dateStamp = DateTime.Now.ToString(
            "dd.MM.yyyy - HH.mm",
            CultureInfo.InvariantCulture
        );

        string HeadingSection =
            $"\n\n### {dateStamp}\n\n- **Detail**: V{UsedVersion}\n- **Description**: {changeDescription}\n\n";
        string Table = SummaryToMarkdownTable<T>();
        string NewInfo = HeadingSection + Table;

        int idx = oldText.IndexOf(BenchTitle);
        if (idx == -1)
        {
            oldText += BenchTitle;
            idx = oldText.Length;
        }
        else
        {
            idx = oldText.IndexOf('\n', idx);
        }
        oldText = oldText.Insert(idx, NewInfo);

        File.WriteAllText(HistoryPath, oldText);
    }

    public static string SummaryToMarkdownTable<T>()
    {
        string path = Path.Combine(
            "BenchmarkDotNet.Artifacts",
            "results",
            $"{typeof(T)}-report-github.md"
        );
        if (!File.Exists(path))
        {
            return "N/A";
        }
        string text = File.ReadAllText(path);
        return text.Substring(text.IndexOf('|'));
    }

    private static void UpdateDetails<T>(out int UsedVersion)
    {
        string oldText = File.Exists(DetailsPath) ? File.ReadAllText(DetailsPath) : "";
        string VersionRegex = @"## V(\d+)";
        RegexOptions options = RegexOptions.Multiline;
        Match m = Regex.Match(oldText, VersionRegex, options);

        int lastVersion = 0;
        int lastVersionIndex = oldText.Length;

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
