# OR

## Description

The `OR` keyword is used as a logical operator. It takes two boolan values (one left and one right to it) and combines them to a single boolean. If at least one of both is true the result will also be true. Otherwise it will be false.

It exists to make more compact code. For example reducing the number of if checks.

## Syntax

```text
{boolen expression} OR {boolen expression}
```

## Use Cases

A common use case is to check two values if one or both values are true in a single if statement instead of using two nested ones. It also makes the code more readable.

An example could be if you want to check the weather. You have a boolen isRaining and another boolean isSnowing. Then you want to print if it is raining or snowing so you know to take a jacket hot. Here would be the corresponding code.

```ysharp
IF isRaining OR isSnowing THEN
    PRINT("take a jacket")
END
```

## Examples

### Basic Example

```ysharp
VAR isRaining = TRUE
VAR isSnowing = TRUE

IF isRaining OR isSnowing THEN
    PRINT("take a jacket")
END

>> take a jacket
```

```ysharp
VAR isRaining = TRUE
VAR isSnowing = FALSE

IF isRaining AND isSnowing THEN
    PRINT("take a jacket")
END

>> "null"
```

### Common Pattern

```ysharp
VAR input = "yes"

IF input == "yes" OR input == "no" THEN
    PRINT("valide input")
END

>> valide input
```

## Notes

This does not perform a bitwise OR operation. It can only use single booleans.

## See Also

* [NOT](./not.md)
* [AND](./and.md)
