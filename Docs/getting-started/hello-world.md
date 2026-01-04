# Creating a "Hello World" script

In this guide, you'll learn two simple ways to write and run your first "Hello World" program in Ysharp:  

- Directly in the interactive shell
- From a saved script file

This is the first step to get comfortable with Ysharp.

## Option 1: Using the Shell (REPL)

Running "Hello World" interactively is quick and easy:

### Steps

1. Open the Ysharp shell by running the `Ysharp.exe` file:
2. At the prompt, type:

```plaintext
>>> PRINT("Hello World")
```

You should see:

```plaintext
Hello World
```

Thats it! You've just executed your first line of Ysharp code.

## Option 2: Running from a Script File

You can also write your code in a separate file and run it through the shell. This is useful for larger programs or saving your work.

### Steps

Open any text editor (Notepad, VS Code, etc.).

Create a new file and name it `helloworld.ys` or whatever you like. I recommend using the .ys extension for Ysharp scripts, but it's not required.

Then write the following code into the file:

```plaintext
PRINT("Hello World")
```

Save the file. Open the terminal and navigate to your YSharp executable. Then you will need to run the executable with a path argument like this:

```bash
.\YSharp.exe --path "path/to/your/file/helloworld.ys"
```

Replace "path/to/your/file/helloworld.ys" with the actual path to where you saved your file. The path can be absolute or relative.

You should see the output:

```plaintext
Hello World
```
