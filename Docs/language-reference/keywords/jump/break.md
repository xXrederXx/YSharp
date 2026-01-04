# BREAK

## Description

The `BREAK` keyword is used to stop a loop imideately. It can only be used inside a loop. The code after the break wont be executed. It exists so you can easily quit a loop without needing to create a giant if around its contents.

## Syntax

```text
BREAK
```

## Use Cases

You should use it when you want to stop iterating over the loop. This can be usefull if you are searching something in a loop and only want the first fit.

## Examples

### Basic Example

This example will iterate over the values 0 to 5. But because there is an if check for the number 3 where it will break, it will only print from 0 to 2.

```ysharp
FOR i = 0 TO 5 THEN
    IF i == 3 THEN
        BREAK
    END
    PRINT(i)
END

>> 0
>> 1
>> 2
```

### Common Pattern

This example waits untill the user inputs yes.

```ysharp
WHILE TRUE THEN
    VAR inp = INPUT()
    IF inp == "yes" THEN
        BREAK
    END
END
```

## Notes

Be care full when working in nested loops. It can be hard to read and also cause unexpected behaviour.

## See Also

* [CONTINUE](./continue.md)
* [RETURN](./return.md)
