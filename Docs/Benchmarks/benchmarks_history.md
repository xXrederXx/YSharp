# Benchmark Histrory

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

| Method           |       Mean |     Error |    StdDev |     Gen0 |     Gen1 |     Gen2 |  Allocated |
| ---------------- | ---------: | --------: | --------: | -------: | -------: | -------: | ---------: |
| LexerBenchmarkS  |   107.9 μs |   2.10 μs |   3.69 μs |  19.4092 |   6.4697 |        - |  318.47 KB |
| LexerBenchmarkM  |   202.2 μs |   3.81 μs |   4.67 μs |  39.0625 |  16.8457 |        - |  639.32 KB |
| LexerBenchmarkL  | 3,584.6 μs |  69.80 μs |  74.69 μs | 234.3750 | 230.4688 |  70.3125 | 3065.89 KB |
| LexerBenchmarkXL | 7,266.5 μs | 141.95 μs | 132.78 μs | 468.7500 | 460.9375 | 148.4375 | 6132.67 KB |

### 18.07.2025 - 19.45

- **Detail**: V1 
- **Description**: After FileNameRegristry
  
| Method           |        Mean |      Error |     StdDev |     Gen0 |     Gen1 |     Gen2 |  Allocated |
| ---------------- | ----------: | ---------: | ---------: | -------: | -------: | -------: | ---------: |
| LexerBenchmarkS  |    90.81 μs |   1.780 μs |   3.022 μs |  16.7236 |   4.7607 |        - |  274.96 KB |
| LexerBenchmarkM  |   184.22 μs |   2.489 μs |   2.206 μs |  33.6914 |  12.6953 |        - |  551.83 KB |
| LexerBenchmarkL  | 3,084.37 μs |  44.210 μs |  41.354 μs | 203.1250 | 199.2188 |  66.4063 | 2628.66 KB |
| LexerBenchmarkXL | 6,217.37 μs | 121.353 μs | 162.002 μs | 406.2500 | 398.4375 | 140.6250 | 5257.98 KB |

### 19.07.2025 - 14.00

- **Detail**: V1 
- **Description**: After Position Update and Lexer Rewrite

| Method           |        Mean |      Error |     StdDev |     Gen0 |     Gen1 |     Gen2 |  Allocated |
| ---------------- | ----------: | ---------: | ---------: | -------: | -------: | -------: | ---------: |
| LexerBenchmarkS  |    90.13 μs |   1.787 μs |   2.195 μs |  15.3809 |   4.6387 |        - |   253.2 KB |
| LexerBenchmarkM  |   179.86 μs |   2.638 μs |   2.467 μs |  31.0059 |  12.9395 |        - |  508.08 KB |
| LexerBenchmarkL  | 2,779.37 μs |  54.550 μs | 102.459 μs | 183.5938 | 179.6875 |  62.5000 | 2410.07 KB |
| LexerBenchmarkXL | 6,525.45 μs | 132.065 μs | 381.038 μs | 382.8125 | 375.0000 | 148.4375 | 4820.64 KB |

### 19.07.2025 - 16.00

- **Detail**: V1 
- **Description**: After Using Enum For Keywords
  
| Method           |        Mean |      Error |     StdDev |     Gen0 |     Gen1 |     Gen2 |  Allocated |
| ---------------- | ----------: | ---------: | ---------: | -------: | -------: | -------: | ---------: |
| LexerBenchmarkS  |    88.49 μs |   1.689 μs |   1.878 μs |  15.3809 |   5.1270 |        - |  251.73 KB |
| LexerBenchmarkM  |   179.61 μs |   2.824 μs |   2.642 μs |  30.7617 |  10.7422 |        - |  505.16 KB |
| LexerBenchmarkL  | 2,742.52 μs |  54.425 μs | 109.941 μs | 187.5000 | 183.5938 |  66.4063 | 2395.48 KB |
| LexerBenchmarkXL | 6,019.96 μs | 120.374 μs | 341.482 μs | 375.0000 | 367.1875 | 140.6250 | 4791.53 KB |

### 24.07.2025 - 19.52

- **Detail**: V2
- **Description**: before updating Interpreter

| Method           |        Mean |      Error |     StdDev |     Gen0 |     Gen1 |     Gen2 |  Allocated |
| ---------------- | ----------: | ---------: | ---------: | -------: | -------: | -------: | ---------: |
| LexerBenchmarkS  |    88.28 μs |   1.440 μs |   1.347 μs |  15.3809 |   5.1270 |        - |  251.73 KB |
| LexerBenchmarkM  |   181.15 μs |   2.041 μs |   1.909 μs |  30.7617 |  10.7422 |        - |  505.16 KB |
| LexerBenchmarkL  | 3,058.97 μs |  59.808 μs |  91.333 μs | 179.6875 | 175.7813 |  58.5938 | 2395.48 KB |
| LexerBenchmarkXL | 6,234.40 μs | 122.312 μs | 220.553 μs | 359.3750 | 351.5625 | 125.0000 | 4791.51 KB |

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

| Method            |       Mean |    Error |    StdDev |     Gen0 |     Gen1 |    Gen2 |   Allocated |
| ----------------- | ---------: | -------: | --------: | -------: | -------: | ------: | ----------: |
| ParserBenchmarkS  |   165.0 μs |  2.58 μs |   2.42 μs |  42.4805 |  10.0098 |       - |   697.34 KB |
| ParserBenchmarkM  |   335.1 μs |  6.48 μs |   7.20 μs |  85.9375 |  28.3203 |       - |  1409.48 KB |
| ParserBenchmarkL  | 2,197.0 μs | 21.24 μs |  17.74 μs | 437.5000 | 187.5000 | 23.4375 |  7042.88 KB |
| ParserBenchmarkXL | 4,898.4 μs | 93.51 μs | 103.94 μs | 882.8125 | 570.3125 | 46.8750 | 14094.54 KB |

### 19.07.2025 - 14.00

- **Detail**: V1 
- **Description**: After Position Update and Lexer Rewrite
  
| Method            |       Mean |    Error |    StdDev |     Gen0 |     Gen1 |    Gen2 |   Allocated |
| ----------------- | ---------: | -------: | --------: | -------: | -------: | ------: | ----------: |
| ParserBenchmarkS  |   153.6 μs |  2.02 μs |   1.89 μs |  41.9922 |  10.2539 |       - |   686.05 KB |
| ParserBenchmarkM  |   321.9 μs |  6.34 μs |   7.79 μs |  84.4727 |  27.8320 |       - |  1386.73 KB |
| ParserBenchmarkL  | 2,254.1 μs | 13.52 μs |  11.98 μs | 437.5000 | 203.1250 | 27.3438 |  6929.24 KB |
| ParserBenchmarkXL | 4,644.6 μs | 90.54 μs | 107.78 μs | 875.0000 | 531.2500 | 54.6875 | 13867.17 KB |

### 19.07.2025 - 16.00

- **Detail**: V1 
- **Description**: After Using Enum For Keywords
  
| Method            |       Mean |    Error |   StdDev |     Gen0 |     Gen1 |    Gen2 |   Allocated |
| ----------------- | ---------: | -------: | -------: | -------: | -------: | ------: | ----------: |
| ParserBenchmarkS  |   140.4 μs |  1.08 μs |  1.01 μs |  38.0859 |   9.2773 |       - |   624.27 KB |
| ParserBenchmarkM  |   287.1 μs |  1.75 μs |  1.55 μs |  77.1484 |  25.3906 |       - |   1261.2 KB |
| ParserBenchmarkL  | 2,024.9 μs | 38.39 μs | 48.55 μs | 398.4375 | 164.0625 | 27.3438 |  6300.37 KB |
| ParserBenchmarkXL | 4,621.9 μs | 50.98 μs | 47.69 μs | 796.8750 | 539.0625 | 54.6875 | 12607.94 KB |

### 20.07.2025 - 21.10

- **Detail**: V1 
- **Description**: After Refactor of Parser
  
| Method            |       Mean |    Error |   StdDev |     Gen0 |     Gen1 |    Gen2 |   Allocated |
| ----------------- | ---------: | -------: | -------: | -------: | -------: | ------: | ----------: |
| ParserBenchmarkS  |   156.3 us |  3.01 us |  4.60 us |  38.0859 |   9.2773 |       - |   624.27 KB |
| ParserBenchmarkM  |   322.7 us |  6.33 us |  6.78 us |  77.1484 |  25.3906 |       - |   1261.2 KB |
| ParserBenchmarkL  | 2,279.4 us | 45.47 us | 63.75 us | 398.4375 | 164.0625 | 27.3438 |  6300.37 KB |
| ParserBenchmarkXL | 4,719.7 us | 59.14 us | 55.32 us | 796.8750 | 539.0625 | 54.6875 | 12607.94 KB |

### 22.07.2025 - 20.20

- **Detail**: V1 
- **Description**: After Second Refactor of Parser
  
| Method            |       Mean |     Error |    StdDev |     Gen0 |     Gen1 |    Gen2 |   Allocated |
| ----------------- | ---------: | --------: | --------: | -------: | -------: | ------: | ----------: |
| ParserBenchmarkS  |   156.3 us |   3.05 us |   4.38 us |  38.5742 |  10.4980 |       - |   632.59 KB |
| ParserBenchmarkM  |   336.2 us |   6.67 us |   8.19 us |  78.1250 |  28.3203 |       - |  1277.55 KB |
| ParserBenchmarkL  | 2,506.5 us |  25.65 us |  21.42 us | 398.4375 | 191.4063 | 23.4375 |  6382.14 KB |
| ParserBenchmarkXL | 5,465.1 us | 104.03 us | 138.88 us | 804.6875 | 562.5000 | 54.6875 | 12771.23 KB |

### 24.07.2025 - 19.53

- **Detail**: V2
- **Description**: before updating Interpreter

| Method            |       Mean |    Error |   StdDev |     Gen0 |     Gen1 |    Gen2 |   Allocated |
| ----------------- | ---------: | -------: | -------: | -------: | -------: | ------: | ----------: |
| ParserBenchmarkS  |   149.4 μs |  2.03 μs |  1.80 μs |  38.5742 |  10.4980 |       - |   632.59 KB |
| ParserBenchmarkM  |   313.0 μs |  1.89 μs |  1.48 μs |  78.1250 |  28.3203 |       - |  1277.55 KB |
| ParserBenchmarkL  | 2,453.0 μs | 25.91 μs | 24.24 μs | 398.4375 | 191.4063 | 23.4375 |  6382.14 KB |
| ParserBenchmarkXL | 5,172.6 μs | 95.83 μs | 89.64 μs | 804.6875 | 562.5000 | 54.6875 | 12771.23 KB |

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

| Method                 |       Mean |    Error |   StdDev |     Gen0 |    Gen1 |  Allocated |
| ---------------------- | ---------: | -------: | -------: | -------: | ------: | ---------: |
| InterpreterBenchmarkS  |   135.5 μs |  2.02 μs |  1.68 μs |  10.2539 |  1.2207 |     170 KB |
| InterpreterBenchmarkM  |   275.7 μs |  1.79 μs |  1.59 μs |  20.9961 |  3.9063 |  344.56 KB |
| InterpreterBenchmarkL  | 1,424.4 μs | 21.59 μs | 19.14 μs | 105.4688 | 42.9688 | 1724.63 KB |
| InterpreterBenchmarkXL | 2,906.5 μs | 24.17 μs | 21.42 μs | 210.9375 | 89.8438 | 3450.26 KB |

### 19.07.2025 - 14.00

- **Detail**: V1 
- **Description**: After Position Update and Lexer Rewrite

| Method                 |       Mean |    Error |    StdDev |     Median |     Gen0 |    Gen1 |  Allocated |
| ---------------------- | ---------: | -------: | --------: | ---------: | -------: | ------: | ---------: |
| InterpreterBenchmarkS  |   139.3 μs |  2.75 μs |   4.43 μs |   141.5 μs |  10.0098 |  1.2207 |  163.93 KB |
| InterpreterBenchmarkM  |   283.1 μs |  5.55 μs |   9.72 μs |   287.4 μs |  20.0195 |  3.9063 |  331.12 KB |
| InterpreterBenchmarkL  | 1,464.5 μs | 28.50 μs |  43.52 μs | 1,485.8 μs | 101.5625 | 39.0625 | 1657.53 KB |
| InterpreterBenchmarkXL | 3,086.9 μs | 60.65 μs | 102.99 μs | 3,084.0 μs | 203.1250 | 89.8438 | 3321.65 KB |

### 19.07.2025 - 16.00

- **Detail**: V1 
- **Description**: After Using Enum For Keywords
  
| Method                 |       Mean |     Error |    StdDev |     Gen0 |    Gen1 |  Allocated |
| ---------------------- | ---------: | --------: | --------: | -------: | ------: | ---------: |
| InterpreterBenchmarkS  |   132.4 μs |   0.73 μs |   0.57 μs |  10.0098 |  1.2207 |  163.05 KB |
| InterpreterBenchmarkM  |   265.1 μs |   2.03 μs |   1.80 μs |  20.0195 |  3.9063 |  330.37 KB |
| InterpreterBenchmarkL  | 1,387.7 μs |   7.83 μs |   6.94 μs | 101.5625 | 39.0625 | 1652.26 KB |
| InterpreterBenchmarkXL | 3,424.9 μs | 112.32 μs | 331.19 μs | 203.1250 | 89.8438 |    3304 KB |

### 24.07.2025 - 19.54

- **Detail**: V2
- **Description**: before updating Interpreter

| Method                 |       Mean |    Error |  StdDev |     Gen0 |    Gen1 |  Allocated |
| ---------------------- | ---------: | -------: | ------: | -------: | ------: | ---------: |
| InterpreterBenchmarkS  |   133.4 μs |  1.26 μs | 1.05 μs |  10.0098 |  1.2207 |     163 KB |
| InterpreterBenchmarkM  |   275.6 μs |  1.30 μs | 1.15 μs |  20.0195 |  3.9063 |  330.44 KB |
| InterpreterBenchmarkL  | 1,444.5 μs | 10.42 μs | 9.75 μs | 101.5625 | 37.1094 | 1652.56 KB |
| InterpreterBenchmarkXL | 2,934.4 μs |  9.09 μs | 7.59 μs | 203.1250 | 89.8438 | 3310.19 KB |

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

| Method        |        Mean |     Error |    StdDev |      Gen0 |     Gen1 |     Gen2 | Allocated |
| ------------- | ----------: | --------: | --------: | --------: | -------: | -------: | --------: |
| RTBenchmarkS  |    403.0 μs |   1.33 μs |   1.04 μs |   69.8242 |  26.8555 |        - |   1.11 MB |
| RTBenchmarkM  |    859.5 μs |   3.85 μs |   3.60 μs |  140.6250 |  66.4063 |        - |   2.25 MB |
| RTBenchmarkL  |  9,906.1 μs | 127.53 μs | 119.29 μs |  781.2500 | 468.7500 | 125.0000 |  11.13 MB |
| RTBenchmarkXL | 22,350.4 μs | 319.36 μs | 266.68 μs | 1562.5000 | 875.0000 | 250.0000 |  22.27 MB |

### 19.07.2025 - 14.00

- **Detail**: V1 
- **Description**: After Position Update and Lexer Rewrite
  
| Method        |        Mean |     Error |    StdDev |      Gen0 |     Gen1 |     Gen2 | Allocated |
| ------------- | ----------: | --------: | --------: | --------: | -------: | -------: | --------: |
| RTBenchmarkS  |    433.1 μs |   8.64 μs |  16.22 μs |   67.3828 |  25.8789 |        - |   1.08 MB |
| RTBenchmarkM  |    900.4 μs |  17.82 μs |  26.67 μs |  135.7422 |  66.4063 |        - |   2.17 MB |
| RTBenchmarkL  | 10,168.1 μs | 188.70 μs | 176.51 μs |  734.3750 | 406.2500 | 109.3750 |  10.74 MB |
| RTBenchmarkXL | 22,521.6 μs | 443.59 μs | 636.19 μs | 1500.0000 | 781.2500 | 250.0000 |   21.5 MB |

### 19.07.2025 - 16.00

- **Detail**: V1 
- **Description**: After Using Enum For Keywords
  
| Method        |        Mean |     Error |    StdDev |      Gen0 |     Gen1 |     Gen2 | Allocated |
| ------------- | ----------: | --------: | --------: | --------: | -------: | -------: | --------: |
| RTBenchmarkS  |    393.1 μs |   2.20 μs |   1.84 μs |   63.4766 |  23.4375 |        - |   1.01 MB |
| RTBenchmarkM  |    800.5 μs |   3.32 μs |   3.11 μs |  127.9297 |  54.6875 |        - |   2.04 MB |
| RTBenchmarkL  |  9,414.8 μs | 186.76 μs | 174.69 μs |  703.1250 | 312.5000 | 109.3750 |   10.1 MB |
| RTBenchmarkXL | 21,396.9 μs | 396.78 μs | 331.33 μs | 1437.5000 | 812.5000 | 250.0000 |  20.22 MB | ## RunTime |

### 24.07.2025 - 19.55

- **Detail**: V2
- **Description**: before updating Interpreter

| Method        |        Mean |     Error |    StdDev |      Gen0 |     Gen1 |     Gen2 | Allocated |
| ------------- | ----------: | --------: | --------: | --------: | -------: | -------: | --------: |
| RTBenchmarkS  |    409.6 μs |   2.15 μs |   1.80 μs |   63.9648 |  25.8789 |        - |   1.02 MB |
| RTBenchmarkM  |    933.4 μs |   5.67 μs |   4.74 μs |  128.9063 |  56.6406 |        - |   2.06 MB |
| RTBenchmarkL  | 10,330.6 μs | 160.63 μs | 150.25 μs |  703.1250 | 375.0000 | 109.3750 |  10.19 MB |
| RTBenchmarkXL | 23,685.5 μs | 472.28 μs | 441.77 μs | 1406.2500 | 937.5000 | 218.7500 |   20.4 MB |
