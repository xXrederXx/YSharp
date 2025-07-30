# Guide on the Shell

The **Ysharp Shell** is an interactive environment (REPL) where you can write, test, and execute code line by line. This guide will show you how to use the shell effectively, from simple expressions to multi-line functions.

## ⚙️ What is the Shell?

When you launch the `Ysharp.exe` executable, it opens the **Ysharp Shell**—a Read-Eval-Print Loop (REPL). It reads your input, evaluates it, and prints the result.

This is useful for:
- Quickly testing code
- Debugging
- Learning the syntax
- Running small scripts without saving files

Example shell prompt:

```
>>> PRINT("Hello World")
```

## Running Code

You can type simple one-line statements and press `Enter`. The shell doesn’t support traditional multi-line input, but you can write multi-line code using semicolons (;) to separate statements on a single line.

If you want to run files from the shell, use the ```RUN``` command.


## Examples

Running Hello World
```
>>> PRINT("Hello World")
```

Declaring a function and calling it
```
>>>FUN add(a, b);RETURN a+b;END;
>>>PRINT(add(10, 6))
16
```

## Next Steps

You are now ready to write your **Hello World** script. A guid on how you can do this is [here](./hello-world.md)