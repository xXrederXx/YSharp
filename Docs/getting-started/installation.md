# Installation of Ysharp

Here you will learn how to install Ysharp. When you have completed this step you are ready to use the shell.

## Prerequesites

Before installing Ysharp, make sure the following tools are installed on your system:

- [.NET SDK (>= 9.0)](https://dotnet.microsoft.com/en-us/download) – required to build and run Ysharp
- [Git](https://git-scm.com/) – to clone the repository

You can verify that they’re installed using the following commands in a terminal or command prompt:

```bash
dotnet --version
git --version
```

## 1. Clone the Repo

Use Git to clone the Ysharp source code from GitHub:

```bash
git clone https://github.com/xXrederXx/YSharp.git
```

This will create a new folder named YSharp in your current directory.


## 2. Navigate to the Project Folder

Change directory into the newly cloned repository:

```bash
cd ysharp
```

## 3. Build the Project

Use the .NET CLI to compile Ysharp in release mode:

```bash
dotnet build -c Release
```

If successful, the compiled executable will be located in:

```bash
./bin/Release/net9.0/
```

> Look for a file named Ysharp.exe.


## Next Steps

You can now run the shell. A guid on how you can use the shell is [here](./shell.md)