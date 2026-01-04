# RETURN

## Description

The `RETURN` keyword is used to exit a function imideately. It can only be used inside a function. The code on the next line after the return wont be executed. It exists so you can easily quit a function without needing to create a giant if around its contents.

Another capabilety of the return is to return a value out of a function. This is havely used.

## Syntax

Return no value, just stop the function

```text
RETURN
```

Return a value and stop the function

```text
RETURN TRUE
```

## Use Cases

You should use it when you want to stop a function and optionaly want to return a result. This is usefull in many ways and you will start using it as soon as you start working with functions.

## Examples

### Basic Example

This example declares a method which returns when the for loop is on the number 3.

```ysharp
FUN foo()
    FOR i = 0 TO 5 THEN
        IF i == 3 THEN
            RETURN
        END
        PRINT(i)
    END
END
>> 0
>> 1
>> 2
```

### Common Pattern

Commonly its used when a function has finished generating a value. for example by doubeling its input.

```ysharp
FUN double(n)
    RETURN n * 2
END
```

## Notes

A function dosn't need a return.
Not every path needs to return the same value or even the same type.

## See Also

* [CONTINUE](./continue.md)
* [BREAK](./break.md)
