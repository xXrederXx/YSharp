using System.Collections.Specialized;
using System.Runtime.InteropServices;
using Microsoft.VisualBasic;
using ScottPlot;
using YSharp.Benchmarks.Analyzer;

namespace YSharp.Benchmarks.Exporter;

public class PlotExporter
{
    private const string PlotFolder = @"..\Docs\Benchmarks\Plots";
    private const int PlotWidth = 1280;
    private const int PlotHeight = 720;
    private const int BottomMargin = 64;

    private record PlotData(
        string Title,
        string Method,
        string CommitHash,
        DateTime DateTime,
        double Mean,
        double Lower,
        double Upper
    );

    private record BenchPlotData(string Title, List<MethodPlotData> Methods);

    private record MethodPlotData(string Method, List<PlotData> Data);

    public static void GeneratePlots()
    {
        IEnumerable<BenchPlotData> data = GetData();

        foreach (BenchPlotData bench in data)
        {
            foreach (MethodPlotData method in bench.Methods)
            {
                GeneratePlot(method, $"{bench.Title}-{method.Method}.png");
            }
        }
    }

    private static IEnumerable<BenchPlotData> GetData() =>
        JsonExtractor
            .LoadAllDatas()
            .SelectMany(res =>
            {
                List<PlotData> datas = new List<PlotData>(res.Benchmarks.Length);
                foreach (BenchmarkData bench in res.Benchmarks)
                {
                    datas.Add(
                        new PlotData(
                            ExtractTitleName(res.Title),
                            bench.Method,
                            res.GitHash.Substring(0, 7),
                            res.DateTime,
                            bench.Statistics.Mean,
                            bench.Statistics.ConfidenceInterval.Lower,
                            bench.Statistics.ConfidenceInterval.Upper
                        )
                    );
                }
                return datas;
            })
            .GroupBy(x => x.Title)
            .Select(perBench => new BenchPlotData(
                perBench.Key,
                perBench
                    .GroupBy(perMethod => perMethod.Method)
                    .Select(perMethod => new MethodPlotData(perMethod.Key, perMethod.ToList()))
                    .ToList()
            ));

    private static void GeneratePlot(MethodPlotData data, string fileName)
    {
        Plot plot = new();
        data.Data.Sort((a, b) => a.DateTime.CompareTo(b.DateTime));

        plot.Add.Bars(data.Data.Select(x => x.Mean).ToArray());

        plot.Axes.Bottom.SetTicks(
            Enumerable.Range(0, data.Data.Count()).Select(x => (double)x).ToArray(),
            data.Data.Select(x => x.CommitHash).ToArray()
        );

        plot.Axes.Bottom.TickLabelStyle.Rotation = 45;
        plot.Axes.Bottom.TickLabelStyle.Alignment = Alignment.MiddleLeft;
        plot.Axes.Bottom.MinimumSize = BottomMargin;
        plot.Axes.Right.MinimumSize = BottomMargin;

        plot.Axes.Margins(bottom: 0);

        plot.SavePng(Path.Combine(PlotFolder, fileName), PlotWidth, PlotHeight);
    }

    private static string ExtractTitleName(string title) =>
        string.Concat(title.Replace("YSharp.Benchmarks.", string.Empty).TakeWhile(char.IsLetter));
}
