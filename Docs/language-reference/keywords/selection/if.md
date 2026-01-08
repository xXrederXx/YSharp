# IF

## Description

If is used to make the code work with the values it gets. You give it a condition and if it is true the body of the if will be executed.
This is a key concept of coding and can be used in many ways.

## Syntax

```text
IF <condition> THEN
    <code>
END
```

## Use Cases

One use case could to check if the user inputed yes or no. You would just need to write the right condition. In this case it could look like the example in the next section.

## Examples

### Basic Example

```ysharp
VAR inp = INPUT()
IF inp == "yes" THEN
    PRINT("inp was yes")
END
```

### Common Pattern

Combined with operators you can even write more complex if statements. For example by checking if it is sunny and hot.

```ysharp
VAR isSunny = TRUE
VAR tempCelsius = 35

IF isSunny AND tempCelsius > 30 THEN
    PRINT("its sunny and hot")
END

>> its sunny and hot
```

## See Also

* [IF-ELSE](./if-else.md)
* [IF-ELIF](if-elif.md)