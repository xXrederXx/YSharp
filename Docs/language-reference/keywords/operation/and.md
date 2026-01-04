# keyword

## Description

The `AND` keyword is used as a logical operator. It takes two boolan values (one left and one right to it) and combines them to a single boolean. If both are true the result will also be true. Otherwise it will be false. 

It exists to make more compact code. For example reducing the number of if checks.

## Syntax

```text
{boolen expression} AND {boolen expression}
```

## Use Cases

A common use case is to check two values if both are true in a single if statement instead of using two nested ones. It also makes the code more readable.

An example could be if you want to check the weather. You have a boolen isSunny and another boolean isHot. Then you want to print if it is sunny and hot. Here would be the corresponding code.

```ysharp
IF isSunny AND isHot THEN
    PRINT("its sunny and hot")
END
```

## Examples

### Basic Example

```ysharp
VAR isSunny = TRUE
VAR isHot = TRUE

IF isSunny AND isHot THEN
    PRINT("its sunny and hot")
END

>> its sunny and hot
```

```ysharp
VAR isSunny = TRUE
VAR isHot = FALSE

IF isSunny AND isHot THEN
    PRINT("its sunny and hot")
END

>> "null"
```

### Common Pattern

```ysharp
VAR isSunny = TRUE
VAR tempCelsius = 35

IF isSunny AND tempCelsius > 30 THEN
    PRINT("its sunny and hot")
END

>> its sunny and hot
```

## Notes

This does not perform a bitwise AND operation. It can only use single booleans.

## See Also

* [NOT](./not.md)
* [OR](./or.md)
