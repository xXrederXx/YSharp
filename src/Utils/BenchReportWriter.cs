using System.Globalization;
using System.Reflection;
using System.Text.RegularExpressions;
using BenchmarkDotNet.Environments;
using BenchmarkDotNet.Reports;

namespace YSharp.Utils;

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

    public static void UpdateFiles<T>(Summary summary, string changeDescription)
    {
        UpdateDetails<T>(out int UsedVersion);
        UpdateHistory<T>(changeDescription, UsedVersion);
        //UpdateSummary<T>(changeDescription, UsedVersion);
    }

    private static void UpdateSummary<T>(string changeDescription, int UsedVersion)
    {
        throw new NotImplementedException("This not working blud");
        string oldText = File.ReadAllText(SummaryPath);

        string H1Title = $"# Benchmarks Summary V{UsedVersion}\n";
        if (!oldText.Contains(H1Title))
        {
            oldText += '\n' + H1Title;
        }

        int relevantStart = oldText.IndexOf(H1Title);
        int relevantEnd = oldText.IndexOf($"# Benchmarks Summary V{UsedVersion + 1}");
        if (relevantEnd == -1)
        {
            relevantEnd = oldText.Length;
        }

        string H2Title = "## " + typeof(T).Name.Replace("Bench", "");
        int H2idx = oldText.IndexOf(H2Title, relevantStart, relevantEnd - relevantStart);
        if (H2idx == -1)
        {
            string insertText =
                "\n\n"
                + H2Title
                + "\n\n### Initial"
                + "\n\n### Improvements"
                + "\n\n- Revisions: 0"
                + "\n\n### Current"
                + "\n\n- Details: -\n\n";
            H2idx = oldText.IndexOf('\n', relevantStart);
            oldText = oldText.Insert(H2idx, insertText);
        }

        relevantStart = H2idx;
        relevantEnd = GetNewEndPos(oldText, relevantStart);

        string currentStatsTable = SummaryToMarkdownTable<T>();

        // Insert initial table if not present
        oldText = InsertInitialIfMissing(oldText, relevantStart, relevantEnd, currentStatsTable);
        relevantEnd = GetNewEndPos(oldText, relevantStart);

        // Replace current table
        oldText = ReplaceSectionTable(
            oldText,
            "### Current",
            currentStatsTable,
            relevantStart,
            relevantEnd
        );
        relevantEnd = GetNewEndPos(oldText, relevantStart);

        // Generate delta improvements
        string initialTable = ExtractSectionTable(
            oldText,
            "### Initial",
            relevantStart,
            relevantEnd
        );
        string improvementTable = GenerateDeltaTable(initialTable, currentStatsTable);
        oldText = ReplaceSectionTable(
            oldText,
            "### Improvements",
            "\n\n" + improvementTable,
            relevantStart,
            relevantEnd
        );
        relevantEnd = GetNewEndPos(oldText, relevantStart);

        // Replace metadata
        oldText = ReplaceKeywordValue(
            oldText,
            relevantStart,
            relevantEnd,
            "Details",
            changeDescription
        );
        relevantEnd = GetNewEndPos(oldText, relevantStart);
        oldText = IncrementKeywordNumber(oldText, relevantStart, relevantEnd, "Revisions");

        File.WriteAllText(SummaryPath, oldText);
    }

    private static int GetNewEndPos(string oldText, int relevantStart)
    {
        int relevantEnd = oldText.IndexOf("\n## ", relevantStart + 1);
        if (relevantEnd == -1)
            relevantEnd = oldText.Length - 1;
        return relevantEnd;
    }

    private static string InsertInitialIfMissing(
        string text,
        int start,
        int end,
        string currentTable
    )
    {
        string section = ExtractSection(text, "### Initial", start, end);
        if (!section.Contains("|")) // no markdown table
        {
            text = ReplaceSectionTable(text, "### Initial", currentTable, start, end);
        }
        return text;
    }

    private static string ExtractSection(string text, string header, int start, int end)
    {
        int headerStart = text.IndexOf(header, start, end - start);
        if (headerStart == -1)
            return "";
        int nextHeader = text.IndexOf("### ", headerStart + 1);
        if (nextHeader == -1 || nextHeader > end)
            nextHeader = end;
        return text.Substring(headerStart, nextHeader - headerStart);
    }

    private static string ExtractSectionTable(string text, string header, int start, int end)
    {
        string section = ExtractSection(text, header, start, end);
        int tableStart = section.IndexOf('|');
        if (tableStart == -1)
            return "";
        return section.Substring(tableStart).Trim();
    }

    private static string ReplaceSectionTable(
        string text,
        string header,
        string newTable,
        int start,
        int end
    )
    {
        int headerStart = text.IndexOf(header, start, end - start);
        if (headerStart == -1)
            return text;
        int tableStart = text.IndexOf('|', headerStart);
        if (tableStart == -1 || tableStart > end)
        {
            // No table found, just insert
            int insertPos = text.IndexOf('\n', headerStart) + 1;
            return text.Insert(insertPos, "\n" + newTable + "\n");
        }

        int nextHeader = text.IndexOf("### ", tableStart + 1);
        if (nextHeader == -1 || nextHeader > end)
            nextHeader = end;

        return text.Substring(0, tableStart) + newTable + "\n" + text.Substring(nextHeader);
    }

    private static string GenerateDeltaTable(string initialTable, string currentTable)
    {
        var initialLines = initialTable.Split('\n').Where(l => l.StartsWith("|")).ToArray();
        var currentLines = currentTable.Split('\n').Where(l => l.StartsWith("|")).ToArray();

        if (initialLines.Length != currentLines.Length)
            return "_Improvement data not aligned._";

        var deltaLines = new List<string> { "**Δ (Change)**" };

        for (int i = 0; i < initialLines.Length; i++)
        {
            if (i == 1) // header separator line
            {
                deltaLines.Add(currentLines[i]);
                continue;
            }

            var initCells = initialLines[i].Split('|').Select(c => c.Trim()).ToArray();
            var currCells = currentLines[i].Split('|').Select(c => c.Trim()).ToArray();

            var deltaCells = new List<string>();
            for (int j = 0; j < initCells.Length; j++)
            {
                if (
                    j == 0
                    || string.IsNullOrWhiteSpace(initCells[j])
                    || !double.TryParse(initCells[j], out double initVal)
                    || !double.TryParse(currCells[j], out double currVal)
                )
                {
                    deltaCells.Add(""); // skip header/first col or non-numeric
                }
                else
                {
                    double delta = currVal - initVal;
                    string sign = delta >= 0 ? "+" : "-";
                    deltaCells.Add($"{sign}{Math.Abs(delta):0.##}");
                }
            }

            deltaLines.Add("| " + string.Join(" | ", deltaCells) + " |");
        }

        return string.Join("\n", deltaLines);
    }

    public static string ReplaceKeywordValue(
        string text,
        int startIndex,
        int endIndex,
        string keyword,
        string replacement
    )
    {
        if (startIndex < 0 || endIndex >= text.Length || startIndex > endIndex)
            throw new ArgumentOutOfRangeException(
                "Invalid start or end index. " + startIndex + " - " + endIndex
            );

        if (replacement.Contains("\n") || replacement.Contains("\r"))
            throw new ArgumentException("Replacement string cannot contain newlines.");

        string searchArea = text.Substring(startIndex, endIndex - startIndex + 1);

        // Pattern: keyword: [anything until newline or end]
        string pattern = $@"{Regex.Escape(keyword)}:\s*[^\r\n]*";
        Match match = Regex.Match(searchArea, pattern);

        if (match.Success)
        {
            string newEntry = $"{keyword}: {replacement}";
            string updatedPart = Regex.Replace(searchArea, pattern, newEntry);
            return text.Substring(0, startIndex) + updatedPart + text.Substring(endIndex + 1);
        }
        else
        {
            System.Console.WriteLine("No match found " + keyword + searchArea);
        }

        return text; // keyword not found in range
    }

    public static string IncrementKeywordNumber(
        string text,
        int startIndex,
        int endIndex,
        string keyword
    )
    {
        if (startIndex < 0 || endIndex >= text.Length || startIndex > endIndex)
            throw new ArgumentOutOfRangeException("Invalid start or end index.");

        string searchArea = text.Substring(startIndex, endIndex - startIndex + 1);

        string pattern = $@"{Regex.Escape(keyword)}:\s*(\d+)";
        Match match = Regex.Match(searchArea, pattern);

        if (match.Success)
        {
            int number = int.Parse(match.Groups[1].Value);
            int newNumber = number + 1;

            string updatedPart = Regex.Replace(searchArea, pattern, $"{keyword}: {newNumber}");

            return text.Substring(0, startIndex) + updatedPart + text.Substring(endIndex + 1);
        }

        return text; // keyword not found or no number behind it
    }

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
