# NOT

## Description

The `NOT` keyword is used as a logical operator. It takes one boola value (one right to it) and flips it. If the start value was true it outputs false. If the start value was false it outputs true.

It exists to make more compact code. For example by inverting an if call.

## Syntax

```text
NOT {boolen expression}
```

## Use Cases

A common use case is to invert a fuction return value if it returns a boolean indicating a success. It also makes the code more readable.

An example could be if you have a function isGreaterThan(a, b) and this function returns a value and you want to print if the number is smaller or equal. In other words if the number is not greater than.

```ysharp
IF NOT isGreaterThan(2, 4) THEN
    PRINT("2 is not bigger than 4")
END
```

## Examples

### Basic Example

```ysharp
VAR isSunny = FALSE

IF NOT isSunny THEN
    PRINT("its not sunny")
END

>> its sunny and hot
```

```ysharp
VAR isHot = TRUE

IF NOT isHot THEN
    PRINT("its cold")
END

>> "null"
```

### Common Pattern

```ysharp
VAR input = "idk"

IF NOT (input == "yes" OR input == "no") THEN
    PRINT("invalide input")
END

>> invalide input
```

## Notes

This does not perform a bitwise NOT operation. It can only use single booleans.

You can theoreticaly chain NOTs but every second not will cancel it self.

```ysharp
VAR isHot = TRUE

IF isHot THEN
    ...
END

is same as

IF NOT NOT isHot THEN
    ...
END
```

## See Also

* [AND](./and.md)
* [OR](./or.md)
