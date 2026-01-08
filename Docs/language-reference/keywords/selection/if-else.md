# IF-ELSE

## Description

In this file i will explain mostly the ELSE part of an IF-ELSE statement. To get information about the IF or ELIF part, go the the corresponding docs. You can find the links [here](#see-also)

When an if condition turns out to be false and also all ELIF blocks are false the else code block will be executed. This is usefull for doing a something if everithing else is false.

## Syntax

```text
IF <condition> THEN
    <code>
END
```

## Use Cases

You can use this after almost every IF check. For example by checking the user input to be any other value than to the ones checked.

## Examples

### Basic Example

```ysharp
VAR inp = INPUT()
IF inp == "yes" THEN
    PRINT("inp was yes")
ELSE
    PRINT("inp was not yes")
END 
```

### Common Pattern

It is even more usefull if you have multiple elifs. Just like here:

```ysharp
VAR tempCelsius = -20

IF tempCelsius > 30 THEN
    PRINT("hot")
ELIF tempCelsius > 15
    PRINT("Normal Temperature")
ELIF tempCelsius > 0
    PRINT("Slightly Cold")
ELSE
    PRINT("Its freezing")
END

>> Its freezing
```

## See Also

* [IF](./if.md)
* [IF-ELIF](if-elif.md)
