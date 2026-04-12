# Welcome to YSharp

![CI](https://github.com/xXrederXx/YSharp/workflows/CI/badge.svg)
[![Coverage](https://codecov.io/gh/xXrederXx/YSharp/branch/main/graph/badge.svg)](https://codecov.io/gh/xXrederXx/YSharp)

## What is YSharp?

YSharp is a simple, general-purpose, interpreted programming language. It runs through a `.exe` interpreter and includes a REPL shell for interactive development.

This language is ideal for scripting, learning programming fundamentals, and experimenting with code in a lightweight environment.


## Features

- REPL shell (interactive command line)
- Simple and readable syntax
- User-defined functions
- Built-in types: numbers, strings, lists, etc.
- Basic control structures (`if`, `for`, `while`)
- Built-in functions for common tasks

## What You'll Find in These Docs

| Section                                          | Description                                                      |
| ------------------------------------------------ | ---------------------------------------------------------------- |
| [Getting Started](./Docs/getting-started/)       | Install the interpreter, run your first script, and use the REPL |
| [Language Reference](./Docs/language-reference/) | Learn the syntax, types, functions, variables, and control flow  |
| [Fundamentals](./Docs/fundementals/)             | How does ysharp work fundamentaly                                |
| [Api](./Docs/api/)                               | Built-in functions or types                                      |
| [Limitations](./Docs/other/limitations.md)       | Known limitations and missing features                           |

## ✅ Quick Example

```plaintext
FUN greet(name)
    PRINT("Hello, " + name)
END

greet("World")
```

To run a script from a file:

```plaintext
RUN("hello.txt")
```

Or, use the shell:

```plaintext
>> VAR x = 5
>> PRINT(x + 2)
7
```


## 🛠️ Project Status

This language is in development and under active improvement. Expect changes and incomplete features.

## 🧭 Start Here

👉 [Getting Started →](./Docs/getting-started/)
