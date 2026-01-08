# Keyword Reference

## VAR

Declares a variable and assigns a value to it.
YSharp variables are dynamically typed, so you don’t have to declare their type.

```plaintext
VAR x = 7
VAR name = "Alice"
```

You can not change the value later without redeclaring:

```plaintext
x = 10 # error
VAR x = 10 # right
```

## IF

Starts a conditional block.
You must close it with `END`.

```plaintext
IF x == 5 THEN
    PRINT("x = 5")
END
```

## THEN

Used after a condition or loop definition to start its block of code.

```plaintext
IF x == 5 THEN
    PRINT("x = 5")
END
```

## ELIF

Short for “else if”.
Checks another condition if the previous `IF` (or another `ELIF`) was false.

```plaintext
IF x == 5 THEN
    PRINT("x = 5")
ELIF x == 7 THEN
    PRINT("x = 7")
END
```

## ELSE

Runs if none of the `IF` or `ELIF` conditions are true.

```plaintext
IF x == 5 THEN
    PRINT("x = 5")
ELIF x == 7 THEN
    PRINT("x = 7")
ELSE
    PRINT("x is not 5 or 7")
END
```

## FOR

Creates a counted loop.
You must use `TO` to set the ending value, and optionally `STEP` to set the increment.
The end value is **inclusive**.

```plaintext
FOR i = 0 TO 7 THEN
    PRINT(i)
END
```

## TO

Used inside a `FOR` loop to set the loop’s end value.

```plaintext
FOR i = 0 TO 7 THEN
    PRINT(i)
END
```

## STEP

Changes how much the loop variable changes each time. Default is `1`.

```plaintext
FOR i = 0 TO 7 STEP 0.1 THEN
    PRINT(i)
END
```

You can also step backwards:

```plaintext
FOR i = 10 TO 0 STEP -2 THEN
    PRINT(i)
END
```

## WHILE

Runs a block repeatedly while a condition is true.
Can run forever if the condition never becomes false.

```plaintext
VAR x = 0
WHILE x < 10 THEN
    x++
END
```

## FUN

Defines a function.
Arguments are separated by commas. Functions must end with `END`.

```plaintext
FUN add(a, b)
    RETURN a + b
END
```

## END

Closes any block started by `IF`, `FOR`, `WHILE`, `FUN`, `TRY`, or `CATCH`.
Always required — indentation does not matter.

## TRY

Starts a block of code where you want to catch errors.

```plaintext
TRY
    PRINT(undefinedVar)
CATCH err
    PRINT("Error caught: ", err)
END
```

Note: `TRY`/`CATCH` is not fully implemented yet.

## CATCH

Runs if an error happens in the matching `TRY` block.
Can take a variable to store the error.

## IMPORT

(Not yet implemented)
Will be used to load code from another file.

```plaintext
IMPORT "path/to/file"
```
