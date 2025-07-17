# Benchmark Times

## Lexer

### 16.07.2025 - 21.20

- **Detail**: V1 
- **Description**: Initial

| Method           |        Mean |     Error |    StdDev |     Gen0 |     Gen1 |     Gen2 |  Allocated |
| ---------------- | ----------: | --------: | --------: | -------: | -------: | -------: | ---------: |
| LexerBenchmarkS  |    121.6 μs |   2.27 μs |   2.70 μs |  28.0762 |   9.2773 |        - |  459.73 KB |
| LexerBenchmarkM  |    245.7 μs |   4.31 μs |   3.60 μs |  56.3965 |  21.4844 |        - |  923.48 KB |
| LexerBenchmarkL  |  5,809.8 μs | 108.72 μs | 206.85 μs | 328.1250 | 320.3125 |  78.1250 | 4486.75 KB |
| LexerBenchmarkXL | 11,486.1 μs | 228.07 μs | 604.81 μs | 640.6250 | 625.0000 | 156.2500 |  8975.2 KB |

### 16.07.2025 - 21.20

- **Detail**: V1 
- **Description**: After StringBuilder Sharing

| Method           | Mean       | Error     | StdDev    | Gen0     | Gen1     | Gen2     | Allocated  |
|----------------- |-----------:|----------:|----------:|---------:|---------:|---------:|-----------:|
| LexerBenchmarkS  |   107.9 μs |   2.10 μs |   3.69 μs |  19.4092 |   6.4697 |        - |  318.47 KB |
| LexerBenchmarkM  |   202.2 μs |   3.81 μs |   4.67 μs |  39.0625 |  16.8457 |        - |  639.32 KB |
| LexerBenchmarkL  | 3,584.6 μs |  69.80 μs |  74.69 μs | 234.3750 | 230.4688 |  70.3125 | 3065.89 KB |
| LexerBenchmarkXL | 7,266.5 μs | 141.95 μs | 132.78 μs | 468.7500 | 460.9375 | 148.4375 | 6132.67 KB |

## Parser

### 16.07.2025 - 21.20

- **Detail**: V1 
- **Description**: Initial

| Method            |       Mean |     Error |    StdDev |     Gen0 |     Gen1 |    Gen2 |   Allocated |
| ----------------- | ---------: | --------: | --------: | -------: | -------: | ------: | ----------: |
| ParserBenchmarkS  |   167.0 μs |   3.25 μs |   3.34 μs |  43.9453 |  10.7422 |       - |   719.45 KB |
| ParserBenchmarkM  |   346.5 μs |   6.72 μs |   7.46 μs |  88.8672 |  29.2969 |       - |  1454.09 KB |
| ParserBenchmarkL  | 2,425.7 μs |  26.47 μs |  23.47 μs | 449.2188 | 187.5000 | 19.5313 |  7265.79 KB |
| ParserBenchmarkXL | 5,112.9 μs | 102.16 μs | 173.48 μs | 898.4375 | 625.0000 | 39.0625 | 14540.54 KB |


## Interpreter

### 16.07.2025 - 21.20

- **Detail**: V1 
- **Description**: Initial
  
| Method                 |       Mean |    Error |    StdDev |     Gen0 |    Gen1 |  Allocated |
| ---------------------- | ---------: | -------: | --------: | -------: | ------: | ---------: |
| InterpreterBenchmarkS  |   158.6 μs |  3.12 μs |   5.13 μs |  11.2305 |  1.7090 |  184.39 KB |
| InterpreterBenchmarkM  |   330.5 μs |  6.53 μs |  13.77 μs |  22.9492 |  5.3711 |   373.5 KB |
| InterpreterBenchmarkL  | 1,657.2 μs | 32.76 μs |  43.73 μs | 113.2813 | 41.0156 | 1869.91 KB |
| InterpreterBenchmarkXL | 3,674.1 μs | 72.51 μs | 143.13 μs | 230.4688 | 97.6563 | 3745.97 KB |


## Runtime

### 16.07.2025 - 21.20

- **Detail**: V1 
- **Description**: Initial

| Method        |        Mean |     Error |    StdDev |      Median |      Gen0 |      Gen1 |     Gen2 | Allocated |
| ------------- | ----------: | --------: | --------: | ----------: | --------: | --------: | -------: | --------: |
| RTBenchmarkS  |    483.7 μs |  12.10 μs |  35.47 μs |    461.6 μs |   83.0078 |   38.0859 |        - |   1.33 MB |
| RTBenchmarkM  |    950.3 μs |   4.63 μs |   4.10 μs |    950.8 μs |  167.9688 |   78.1250 |        - |   2.68 MB |
| RTBenchmarkL  | 12,129.1 μs | 106.60 μs |  99.72 μs | 12,165.4 μs |  937.5000 |  578.1250 | 140.6250 |   13.3 MB |
| RTBenchmarkXL | 26,466.1 μs | 523.42 μs | 602.77 μs | 26,469.9 μs | 1843.7500 | 1031.2500 | 281.2500 |  26.62 MB |


## Details

### V1

```
BenchmarkDotNet v0.15.2, Windows 11 (10.0.26100.4484/24H2/2024Update/HudsonValley)  
AMD Ryzen 5 5600X 3.70GHz, 1 CPU, 12 logical and 6 physical cores  
.NET SDK 9.0.201  
  [Host]     : .NET 9.0.3 (9.0.325.11113), X64 RyuJIT AVX2  
  DefaultJob : .NET 9.0.3 (9.0.325.11113), X64 RyuJIT AVX2  
```

Attributes `[SimpleJob] [MemoryDiagnoser]`