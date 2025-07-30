# Creating a "Hello World" script

In this guide, you'll learn two simple ways to write and run your first **"Hello World"** program using **Ysharp**:  
- Directly in the interactive shell (REPL)  
- From a saved script file

This is a great first step to get comfortable with how Ysharp runs code.

##  Option 1: Using the Shell (REPL)

Running "Hello World" interactively is quick and easy:

### Steps:
1. Open the Ysharp shell by running the `Ysharp.exe` file:
2. At the prompt, type:

```plaintext
>>> PRINT("Hello World")
```

You should see:

```
Hello World
```

 Thats it! You've just executed your first line of Ysharp code.

## Option 2: Running from a Script File

You can also write your code in a separate file and run it through the shell. This is useful for larger programs or saving your work.

### Steps:
Open any text editor (Notepad, VS Code, etc.).

Create a new file and name it:


```
helloworld.ys
```

I recommend using the .ys extension for Ysharp scripts, but it's not required.

Write the following code into the file:


```
PRINT("Hello World")
```

Save the file. Open the Ysharp shell. In the shell, run the script using the RUN() function:

```
>>> RUN("path/to/your/file/helloworld.ys")
```

Replace "path/to/your/file/helloworld.ys" with the actual path to where you saved your file.

You should see the output:

```
Hello World
```
