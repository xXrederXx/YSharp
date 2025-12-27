record BenchmarksData(string Title, BenchmarkData[] Benchmarks);

record BenchmarkData(string Type, string Method, BenchmarkStatisticData Statistics, BenchmarkMetricData[] Metrics);

record BenchmarkStatisticData(double Mean, BenchmarkConfidenceIntervalData ConfidenceInterval);

record BenchmarkConfidenceIntervalData(double Lower, double Upper);

record BenchmarkMetricData(double Value, BenchmarkMetricDescriptor Descriptor);

record BenchmarkMetricDescriptor(string DisplayName);