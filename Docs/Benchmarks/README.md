# Benchmarks

This directory contains all data and documentation related to performance benchmarks.

## Information

### Data

This folder stores JSON files with the data required to generate benchmark documentation.
To reduce file size, only a subset of the BenchmarkDotNet output is saved.

These files are generated automatically whenever a benchmark completes.

### Plots

This folder contains generated plot images used in the benchmark summaries.
The plots visualize performance metrics over time and are automatically created and updated as new data becomes available.

### Summaries

This folder contains markdown summaries for individual benchmarks.
Each summary includes:

* The Git commit hash corresponding to the benchmarked code
* The execution timestamp of the benchmark run
* Plots showing performance trends over time

Summaries are automatically generated or updated whenever new benchmark results are recorded.

## How To Run

If you just run `dotnet run` in the benchmark directory it wont fully work as expectet.

Please use the provided `.bat` file if possible. If not possible you need to get your current (full) git commit hash and pass it as the first argument to the programm.
