# Keyword Reference

## VAR

Declares a variable and assigns a value to it.
YSharp variables are dynamically typed, so you don’t have to declare their type.

```
VAR x = 7
VAR name = "Alice"
```

You can not change the value later without redeclaring:

```
x = 10 # error
VAR x = 10 # right
```

## IF

Starts a conditional block.
You must close it with `END`.

```
IF x == 5 THEN
    PRINT("x = 5")
END
```



## THEN

Used after a condition or loop definition to start its block of code.

```
IF x == 5 THEN
    PRINT("x = 5")
END
```



## ELIF

Short for “else if”.
Checks another condition if the previous `IF` (or another `ELIF`) was false.

```
IF x == 5 THEN
    PRINT("x = 5")
ELIF x == 7 THEN
    PRINT("x = 7")
END
```



## ELSE

Runs if none of the `IF` or `ELIF` conditions are true.

```
IF x == 5 THEN
    PRINT("x = 5")
ELIF x == 7 THEN
    PRINT("x = 7")
ELSE
    PRINT("x is not 5 or 7")
END
```

## AND

Logical operator that returns `true` only if **both** expressions are true.

```
IF x == 7 AND y == 5 THEN
    PRINT("Both conditions are true")
END
```



## OR

Logical operator that returns `true` if **at least one** expression is true.

```
IF x == 7 OR y == 5 THEN
    PRINT("One or both conditions are true")
END
```



## NOT

Logical operator that flips the value of a boolean.
Only works on boolean values — not on numbers, strings, etc.

```
VAR isOn = true
IF NOT isOn THEN
    PRINT("It is off")
END
```

## FOR

Creates a counted loop.
You must use `TO` to set the ending value, and optionally `STEP` to set the increment.
The end value is **inclusive**.

```
FOR i = 0 TO 7 THEN
    PRINT(i)
END
```



## TO

Used inside a `FOR` loop to set the loop’s end value.

```
FOR i = 0 TO 7 THEN
    PRINT(i)
END
```



## STEP

Changes how much the loop variable changes each time. Default is `1`.

```
FOR i = 0 TO 7 STEP 0.1 THEN
    PRINT(i)
END
```

You can also step backwards:

```
FOR i = 10 TO 0 STEP -2 THEN
    PRINT(i)
END
```



## WHILE

Runs a block repeatedly while a condition is true.
Can run forever if the condition never becomes false.

```
VAR x = 0
WHILE x < 10 THEN
    x++
END
```

## CONTINUE

Skips the rest of the current loop iteration and jumps to the next one.

```
FOR i = 1 TO 5 THEN
    IF i == 3 THEN
        CONTINUE
    END
    PRINT(i)
END
```



## BREAK

Immediately stops the loop.

```
FOR i = 1 TO 10 THEN
    IF i > 5 THEN
        BREAK
    END
    PRINT(i)
END
```


## FUN

Defines a function.
Arguments are separated by commas. Functions must end with `END`.

```
FUN add(a, b)
    RETURN a + b
END
```

## RETURN

Exits a function and sends a value back to where it was called.

```
FUN double(n)
    RETURN n * 2
END
```


## END

Closes any block started by `IF`, `FOR`, `WHILE`, `FUN`, `TRY`, or `CATCH`.
Always required — indentation does not matter.


## TRY

Starts a block of code where you want to catch errors.

```
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

```
IMPORT "path/to/file"
```

