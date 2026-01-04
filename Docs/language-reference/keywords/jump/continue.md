# CONTINUE

## Description

The `CONTINUE` keyword is used to skip the rest a loop imideately. It can only be used inside a loop. The code after the continue wont be executed. It exists so you can easily skip code in a loop without needing to create a giant if around its contents.

## Syntax

```text
CONTINUE
```

## Use Cases

You should use it when you want to skip an iteration in the loop. This can be usefull if you are first checking if an item is valid, and if it isnt you can just continue with the next items.

## Examples

### Basic Example

This example will iterate over the values 0 to 5. But because there is an if check for the number 3 where it will continue, it will not print the number 3.

```ysharp
FOR i = 0 TO 5 THEN
    IF i == 3 THEN
        CONTINUE
    END
    PRINT(i)
END

>> 0
>> 1
>> 2
>> 4
```

### Common Pattern

This example will give you the numbers from 0 to 10 and ask you if you want to see then. If you dont want to see them it will continue. else it will show it to you and add it to the sum which gets printed at the end.

```ysharp
VAR sum = 0
FOR i = 0 TO 10 THEN
    PRINT("Want to use the number?")
    IF INPUT() == "no" THEN
        CONTINUE
    END
    PRINT(i)
    VAR sum = sum + i
END
PRINT(sum)
```

## Notes

Be care full when working in nested loops. It can be hard to read and also cause unexpected behaviour.

## See Also

* [BREAK](./break.md)
* [RETURN](./return.md)
