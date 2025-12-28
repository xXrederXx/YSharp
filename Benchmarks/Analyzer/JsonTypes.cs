public record BenchmarksData(string Title, BenchmarkData[] Benchmarks);

public record BenchmarkData(
    string Type,
    string Method,
    BenchmarkStatisticData Statistics,
    BenchmarkMetricData[] Metrics
);

public record BenchmarkStatisticData(
    double Mean,
    BenchmarkConfidenceIntervalData ConfidenceInterval
);

public record BenchmarkConfidenceIntervalData(double Lower, double Upper);

public record BenchmarkMetricData(double Value, BenchmarkMetricDescriptor Descriptor);

public record BenchmarkMetricDescriptor(string DisplayName);
