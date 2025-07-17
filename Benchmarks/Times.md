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

### 17.07.2025 - 13.45

- **Detail**: V1 
- **Description**: After StringBuilder Sharing

| Method           | Mean       | Error     | StdDev    | Gen0     | Gen1     | Gen2     | Allocated  |
|----------------- |-----------:|----------:|----------:|---------:|---------:|---------:|-----------:|
| LexerBenchmarkS  |   107.9 μs |   2.10 μs |   3.69 μs |  19.4092 |   6.4697 |        - |  318.47 KB |
| LexerBenchmarkM  |   202.2 μs |   3.81 μs |   4.67 μs |  39.0625 |  16.8457 |        - |  639.32 KB |
| LexerBenchmarkL  | 3,584.6 μs |  69.80 μs |  74.69 μs | 234.3750 | 230.4688 |  70.3125 | 3065.89 KB |
| LexerBenchmarkXL | 7,266.5 μs | 141.95 μs | 132.78 μs | 468.7500 | 460.9375 | 148.4375 | 6132.67 KB |

### 18.07.2025 - 19.45

- **Detail**: V1 
- **Description**: After FileNameRegristry
  
| Method           | Mean        | Error      | StdDev     | Gen0     | Gen1     | Gen2     | Allocated  |
|----------------- |------------:|-----------:|-----------:|---------:|---------:|---------:|-----------:|
| LexerBenchmarkS  |    90.81 μs |   1.780 μs |   3.022 μs |  16.7236 |   4.7607 |        - |  274.96 KB |
| LexerBenchmarkM  |   184.22 μs |   2.489 μs |   2.206 μs |  33.6914 |  12.6953 |        - |  551.83 KB |
| LexerBenchmarkL  | 3,084.37 μs |  44.210 μs |  41.354 μs | 203.1250 | 199.2188 |  66.4063 | 2628.66 KB |
| LexerBenchmarkXL | 6,217.37 μs | 121.353 μs | 162.002 μs | 406.2500 | 398.4375 | 140.6250 | 5257.98 KB |

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

### 18.07.2025 - 19.45

- **Detail**: V1 
- **Description**: After FileNameRegristry

| Method            | Mean       | Error    | StdDev    | Gen0     | Gen1     | Gen2    | Allocated   |
|------------------ |-----------:|---------:|----------:|---------:|---------:|--------:|------------:|
| ParserBenchmarkS  |   165.0 μs |  2.58 μs |   2.42 μs |  42.4805 |  10.0098 |       - |   697.34 KB |
| ParserBenchmarkM  |   335.1 μs |  6.48 μs |   7.20 μs |  85.9375 |  28.3203 |       - |  1409.48 KB |
| ParserBenchmarkL  | 2,197.0 μs | 21.24 μs |  17.74 μs | 437.5000 | 187.5000 | 23.4375 |  7042.88 KB |
| ParserBenchmarkXL | 4,898.4 μs | 93.51 μs | 103.94 μs | 882.8125 | 570.3125 | 46.8750 | 14094.54 KB |

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

### 18.07.2025 - 19.45

- **Detail**: V1 
- **Description**: After FileNameRegristry

| Method                 | Mean       | Error    | StdDev   | Gen0     | Gen1    | Allocated  |
|----------------------- |-----------:|---------:|---------:|---------:|--------:|-----------:|
| InterpreterBenchmarkS  |   135.5 μs |  2.02 μs |  1.68 μs |  10.2539 |  1.2207 |     170 KB |
| InterpreterBenchmarkM  |   275.7 μs |  1.79 μs |  1.59 μs |  20.9961 |  3.9063 |  344.56 KB |
| InterpreterBenchmarkL  | 1,424.4 μs | 21.59 μs | 19.14 μs | 105.4688 | 42.9688 | 1724.63 KB |
| InterpreterBenchmarkXL | 2,906.5 μs | 24.17 μs | 21.42 μs | 210.9375 | 89.8438 | 3450.26 KB |

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

### 18.07.2025 - 19.45

- **Detail**: V1 
- **Description**: After FileNameRegristry

| Method        | Mean        | Error     | StdDev    | Gen0      | Gen1     | Gen2     | Allocated |
|-------------- |------------:|----------:|----------:|----------:|---------:|---------:|----------:|
| RTBenchmarkS  |    403.0 μs |   1.33 μs |   1.04 μs |   69.8242 |  26.8555 |        - |   1.11 MB |
| RTBenchmarkM  |    859.5 μs |   3.85 μs |   3.60 μs |  140.6250 |  66.4063 |        - |   2.25 MB |
| RTBenchmarkL  |  9,906.1 μs | 127.53 μs | 119.29 μs |  781.2500 | 468.7500 | 125.0000 |  11.13 MB |
| RTBenchmarkXL | 22,350.4 μs | 319.36 μs | 266.68 μs | 1562.5000 | 875.0000 | 250.0000 |  22.27 MB |

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